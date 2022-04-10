using catalog.Core.Application.Common.Service;

namespace catalog.Infrastructure.Cache;

/// <summary>
/// Фабрика кэша
/// </summary>
public class CacheFactory : ICacheFactory
{
    public CacheFactory()
    {
    }


    /// <summary>
    /// Создать "Typed" накопительный кэш
    /// </summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="TValue">Тип значения</typeparam>
    /// <param name="timeout">Время обновления</param>
    /// <returns>Служба кэширования</returns>
    public ICaching<TKey, TValue> CreateComulativeCache<TKey, TValue>(TimeSpan timeout)
    {
        return new ComulativeCache<TKey, TValue>(new Timeout(timeout));
    }

    /// <summary>
    /// Создать "Untyped" накопительный кэш
    /// </summary>
    /// <param name="timeout">Время обновления</param>
    /// <returns>Служба кэширования</returns>
    public ICaching<object, object> CreateComulativeCache(TimeSpan timeout)
    {
        return new ComulativeCache<object, object>(new Timeout(timeout));
    }
}
