namespace catalog.Core.Application.Common.Service;

/// <summary>
/// Фабрика служб кэширования
/// </summary>
public interface ICacheFactory
{
    /// <summary>
    /// Создать накопительный кэш
    /// </summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="TValue">Тип значения</typeparam>
    /// <param name="timeout">Время обновления</param>
    /// <returns>Служба кэширования</returns>
    ICaching<TKey, TValue> CreateComulativeCache<TKey, TValue>(TimeSpan timeout);

    /// <summary>
    /// Создать "Untyped" накопительный кэш
    /// </summary>
    /// <param name="timeout">Время обновления</param>
    /// <returns>Служба кэширования</returns>
    public ICaching<object, object> CreateComulativeCache(TimeSpan timeout);
}
