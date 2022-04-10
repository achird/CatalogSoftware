namespace catalog.Core.Application.Common.Cqrs;

/// <summary>
/// Обработчик запросов к контексту домена
/// </summary>
public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    /// <summary>
    /// Выполнить запрос
    /// </summary>
    /// <param name="query">Запрос</param>
    /// <returns></returns>
    Task<TResult> HandleAsync(TQuery query);
}
