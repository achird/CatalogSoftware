using catalog.Core.Application.Common.Service;
using catalog.Infrastructure.Library.Extensions;
using System.Collections.ObjectModel;

namespace catalog.Infrastructure.Cache;

/// <summary>
/// Накопительный кэш с вытеснением по времени неиспользуемых элементов
/// </summary>
/// <typeparam name="TKey">Тип ключа</typeparam>
/// <typeparam name="TValue">Тип значения</typeparam>
public class ComulativeCache<TKey, TValue> : ICaching<TKey, TValue>
{
    /// <summary>
    /// Флаг использования элемента
    /// </summary>
    class UsedFlag
    {
        long value = 1;

        /// <summary>
        /// Проверить, используется ли элемент
        /// </summary>
        public bool IsNotUsed => value == 0;

        /// <summary>
        /// Установаить флаг, элемент используется
        /// </summary>
        public void Set()
        {
            if (IsNotUsed) Interlocked.Exchange(ref value, 1);
        }

        /// <summary>
        /// Очистить флаг, элемент не используется
        /// </summary>
        public void Clear()
        {
            if (!IsNotUsed) Interlocked.Exchange(ref value, 0);
        }
    }

    /// <summary>
    /// Внутренний тип элемента кэша
    /// </summary>
    class CachedValue
    {
        public CachedValue(TValue value)
        {
            Value = value;
            UsedFlag = new UsedFlag();
        }

        /// <summary>
        /// Значение элемента кэша
        /// </summary>
        public TValue Value { get; }

        /// <summary>
        /// Флаг использования
        /// </summary>
        public UsedFlag UsedFlag { get; }
    }

    private ReadOnlyDictionary<TKey, CachedValue> cache;
    private readonly RWLock rwLock = new RWLock();

    /// <summary>
    /// Накопительный кэш
    /// </summary>
    /// <param name="timeout">Служба с таймером обновления</param>
    public ComulativeCache(ITimeout timeout)
    {
        if (timeout == null) throw new ArgumentNullException(nameof(timeout));

        cache = new ReadOnlyDictionary<TKey, CachedValue>(new Dictionary<TKey, CachedValue>());
        timeout.StartTimer(Replace);
    }

    /// <summary>
    /// Попытаться получить элемент
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <param name="value">Значение</param>
    /// <returns>true, если элемент получен, иначе false</returns>
    public bool TryGet(TKey key, out TValue value)
    {
        if (cache.TryGetValue(key, out var timedValue))
        {
            value = timedValue.Value;
            timedValue.UsedFlag.Set();
            return true;
        }

        value = default;
        return false;
    }

    /// <summary>
    /// Получить элемент
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <returns>Значение</returns>
    public TValue Get(TKey key)
    {
        if (cache.TryGetValue(key, out var timedValue))
        {
            timedValue.UsedFlag.Set();
            return timedValue.Value;
        }

        return default;
    }


    /// <summary>
    /// Получить или добавить элемент
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <param name="value">Значение</param>
    /// <returns>Значение из кэша</returns>
    public TValue GetOrAdd(TKey key, TValue value)
    {
        if (cache.TryGetValue(key, out var cachedValue))
        {
            cachedValue.UsedFlag.Set();
            return cachedValue.Value;
        }

        return Add(key, (_) => value);
    }

    /// <summary>
    /// Получить или добавить элемент
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <param name="valueFactory">Делегат создания элемента</param>
    /// <returns>Эначение из кэша</returns>
    public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
    {
        if (valueFactory == null) throw new ArgumentNullException(nameof(valueFactory));

        if (cache.TryGetValue(key, out var cachedValue))
        {
            cachedValue.UsedFlag.Set();
            return cachedValue.Value;
        }

        return Add(key, valueFactory);
    }

    /// <summary>
    /// Добавить диапазон значений
    /// </summary>
    /// <param name="range">Элементы для добавления</param>
    public void AddRange(IReadOnlyDictionary<TKey, TValue> range)
    {
        if (range.Count == 0) return;

        using (rwLock.WriteLock())
        {
            var local = new Dictionary<TKey, CachedValue>(cache);
            foreach (var value in range)
                local.TryAdd(value.Key, new CachedValue(value.Value));
            Interlocked.Exchange(ref cache, new ReadOnlyDictionary<TKey, CachedValue>(local));
        }
    }

    /// <summary>
    /// Добавить элемент
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <param name="valueFactory">Делегат создания элемента</param>
    /// <returns>Добавленный элемент</returns>
    private TValue Add(TKey key, Func<TKey, TValue> valueFactory)
    {
        using (rwLock.WriteLock())
        {
            if (cache.ContainsKey(key) != true)
            {
                var local = new Dictionary<TKey, CachedValue>(cache);
                var value = valueFactory(key);

                if (local.TryAdd(key, new CachedValue(value)))
                {
                    Interlocked.Exchange(ref cache, new ReadOnlyDictionary<TKey, CachedValue>(local));
                    return value;
                }
            }
        }

        if (cache.TryGetValue(key, out var cachedValue))
        {
            cachedValue.UsedFlag.Set();
            return cachedValue.Value;
        }

        return default;
    }

    /// <summary>
    /// Попытаться удалить элемент из кэша
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <returns>true, если элемент был удален, иначе false</returns>
    public bool TryRemove(TKey key)
    {
        using (rwLock.WriteLock())
        {
            if (cache.ContainsKey(key))
            {
                var local = new Dictionary<TKey, CachedValue>(cache);
                if (local.Remove(key))
                {
                    Interlocked.Exchange(ref cache, new ReadOnlyDictionary<TKey, CachedValue>(local));
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Очистить кэш
    /// </summary>
    public void Clear()
    {
        if (cache.Count == 0) return;

        using (rwLock.WriteLock())
        {
            var local = new Dictionary<TKey, CachedValue>();
            Interlocked.Exchange(ref cache, new ReadOnlyDictionary<TKey, CachedValue>(local));
        }
    }

    /// <summary>
    /// Вытеснение неиспользуемых элементов из кэша
    /// </summary>
    private void Replace()
    {
        using (rwLock.WriteLock())
        {
            var local = new Dictionary<TKey, CachedValue>(cache);
            foreach (var key in local.Where(a => a.Value.UsedFlag.IsNotUsed).Select(a => a.Key))
            {
                local.Remove(key);
            }
            foreach (var item in local)
            {
                item.Value.UsedFlag.Clear();
            }
            Interlocked.Exchange(ref cache, new ReadOnlyDictionary<TKey, CachedValue>(local));
        }
    }

    ~ComulativeCache()
    {
        if (rwLock != null)
        {
            rwLock.Dispose();
        }
    }
}
