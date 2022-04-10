namespace catalog.Core.Application.Common.Service;

/// <summary>
/// Кэш
/// </summary>
public interface ICaching<TKey, TValue>
{
    /// <summary>
    /// Получить значение
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <returns>Значение</returns>
    TValue Get(TKey key);

    /// <summary>
    /// Попробовать получить значение
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <param name="value">Значение</param>
    /// <returns></returns>
    bool TryGet(TKey key, out TValue value);
    
    /// <summary>
    /// Добавить диапазон значений
    /// </summary>
    /// <param name="range">Новый диапазон</param>
    void AddRange(IReadOnlyDictionary<TKey, TValue> range);
    
    /// <summary>
    /// Получить или добавить значение
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <param name="value">Значение</param>
    /// <returns></returns>
    TValue GetOrAdd(TKey key, TValue value);
    
    /// <summary>
    /// Получить или добавить значение
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <param name="valueFactory">Фабрика значений</param>
    /// <returns></returns>
    TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory);
    
    /// <summary>
    /// Попробовать удалить значение
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <returns></returns>
    bool TryRemove(TKey key);

    /// <summary>
    /// Очистить кеш
    /// </summary>
    void Clear();
}
