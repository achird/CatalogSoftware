using System;
using System.Linq.Expressions;

namespace catalog.Core.SharedKernel.Base.Specification;

/// <summary>
/// Обработчик действия NOT для спецификации
/// </summary>
/// <typeparam name="T"></typeparam>
internal class NotSpecification<T> : Specification<T>
{
    private readonly Specification<T> specification;

    public NotSpecification(Specification<T> specification)
    {
        this.specification = specification;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        var expression1 = specification.ToExpression();

        var param = Expression.Parameter(typeof(T));
        var body = Expression.Not(expression1.Body);
        body = (UnaryExpression)new ParameterReplacer(param).Visit(body);
        var expression = Expression.Lambda<Func<T, bool>>(body, param);

        return expression;
    }
}
