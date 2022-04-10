namespace catalog.Core.Application.Common.Cqrs;

/// <summary>
/// Обработчик команд к контексту домена
/// </summary>
/// <typeparam name="TCommand">Команда</typeparam>
/// <typeparam name="TResult">Результат</typeparam>
public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
{
    Task<TResult> HandleAsync(TCommand command);
}

/// <summary>
/// Обработчик команд к контексту домена
/// </summary>
/// <typeparam name="TCommand">Команда</typeparam>
public interface ICommandHandler<TCommand> where TCommand : ICommand
{
    Task HandleAsync(TCommand command);
}
