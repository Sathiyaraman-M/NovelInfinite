using System.Linq.Expressions;
using Infinite.Shared.Contracts;

namespace Infinite.Core.Specifications.Base;

public interface ISpecification<T> where T : class, IEntity
{
    Expression<Func<T, bool>> FilterCondition { get; }

    List<Expression<Func<T, object>>> Includes { get; }

    List<string> IncludeStrings { get; }
}