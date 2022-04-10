using System;
using System.Linq.Expressions;

namespace catalog.Core.SharedKernel.Base.Specification;

/// <summary>
/// Специальная спецификация
/// </summary>
/// <typeparam name="T"></typeparam>
public class AdHocSpecification<T> : Specification<T>
{
    Expression<Func<T, bool>> expression;

    public AdHocSpecification(Expression<Func<T, bool>> expression)
    {
        this.expression = expression;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        return expression;
    }
}
