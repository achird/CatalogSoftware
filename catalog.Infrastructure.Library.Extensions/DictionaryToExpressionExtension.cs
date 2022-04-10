using System.Linq.Expressions;

namespace catalog.Infrastructure.Library.Extensions;

/// <summary>
/// Расширение, создающее Expression по Dictionary
/// </summary>
public static class DictionaryToExpressionExtension
{
    /// <summary>
    /// Преобразовать в выражение равенства по ключу
    /// </summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="TValue">Тип значения</typeparam>
    /// <typeparam name="TSource">Тип параметра</typeparam>
    /// <typeparam name="TResult">Тип результата</typeparam>
    /// <param name="dictionary">Dictionary</param>
    /// <param name="sourceProperty">Свойство для сравнения с ключом</param>
    /// <param name="resultProperty">Свойство значения TValue результата сравнения</param>
    /// <param name="defaultResult">Результат по умолчанию, для несовпавших значений</param>
    /// <returns></returns>
    public static Expression<Func<TSource, TResult>> ToExpression<TKey, TValue, TSource, TResult>(this Dictionary<TKey, TValue> dictionary, Expression<Func<TSource, TKey>> sourceProperty, Func<TValue, TResult> resultProperty, TResult defaultResult = default)
    {
        var memberExpression = sourceProperty.Body as MemberExpression;
        if (memberExpression == null)
            throw new ArgumentException("Invalid sourceProperty");

        var mi = memberExpression.Member;

        var param = Expression.Parameter(typeof(TSource));
        var property = Expression.Property(param, mi.Name);

        var r = Expression.Lambda<Func<TSource, TResult>>(ExpressionAtElement(dictionary, 0, property, resultProperty, defaultResult), param);
        return r;
    }

    private static Expression ExpressionAtElement<TKey, TValue, TResult>(Dictionary<TKey, TValue> dictionary, int i, MemberExpression property, Func<TValue, TResult> valueFunc, TResult defaultResult)
    {
        var item = dictionary.ElementAt(i);
        var condition = Expression.Equal(property, Expression.Constant(item.Key));
        var trueClause = Expression.Constant(valueFunc(item.Value));
        Expression falseClause;
        if (i < dictionary.Count - 1)
            falseClause = ExpressionAtElement(dictionary, ++i, property, valueFunc, defaultResult);
        else
            falseClause = Expression.Constant(defaultResult);

        return Expression.Condition(condition, trueClause, falseClause);
    }
}
