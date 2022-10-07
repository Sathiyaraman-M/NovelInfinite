using Infinite.Core.Specifications.Base;
using Infinite.Shared.Contracts;

namespace Infinite.Core.Extensions;

public static class SpecificationExtensions
{
    public static ISpecification<T> And<T>(this ISpecification<T> left, ISpecification<T> right) where T : class, IEntity
    {
        return new AndSpecification<T>(left, right);
    }

    public static ISpecification<T> Or<T>(this ISpecification<T> left, ISpecification<T> right) where T : class, IEntity
    {
        return new OrSpecification<T>(left, right);
    }
}