using System.Linq.Expressions;

namespace catalog.Core.SharedKernel.Base.Specification;

/// <summary>
/// Спецификация
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Specification<T>
{
    /// <summary>
    /// Получить Expression из спецификации
    /// </summary>
    /// <returns></returns>
    public abstract Expression<Func<T, bool>> ToExpression();

    /// <summary>
    /// Проверка объекта на условия спицификации
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool IsSatisfiedBy(T item)
    {
        Func<T, bool> predicate = ToExpression().Compile();
        return predicate(item);
    }

    public static Specification<T> operator &(Specification<T> spec1, Specification<T> spec2)
    {
        return new AndSpecification<T>(spec1, spec2);
    }

    public static Specification<T> operator |(Specification<T> spec1, Specification<T> spec2)
    {
        return new OrSpecification<T>(spec1, spec2);
    }

    public static Specification<T> operator !(Specification<T> specification)
    {
        return new NotSpecification<T>(specification);
    }

    public static bool operator true(Specification<T> specification)
    {
        return true;
    }

    public static bool operator false(Specification<T> specification)
    {
        return false;
    }
}
