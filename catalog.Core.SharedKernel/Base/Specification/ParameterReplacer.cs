using System.Linq.Expressions;

namespace catalog.Core.SharedKernel.Base.Specification;

/// <summary>
/// Заменитель параметра в выражении Expression
/// </summary>
internal class ParameterReplacer : ExpressionVisitor
{

    private readonly ParameterExpression parameter;

    protected override Expression VisitParameter(ParameterExpression node) => base.VisitParameter(parameter);

    internal ParameterReplacer(ParameterExpression parameter)
    {
        this.parameter = parameter;
    }
}
