namespace catalog.Core.Application.Common.Cqrs;

/// <summary>
/// Команда к контексту домена, возвращающая результат
/// </summary>
public interface ICommand<TResult>
{
}

/// <summary>
/// Команда к контексту домена
/// </summary>
public interface ICommand
{
}
