using Infinite.Core.Specifications.Base;
using Infinite.Shared.Entities;

namespace Infinite.Core.Specifications;

public class PersonalBlogsSearchFilterSpecification : BaseSpecification<Blog>
{
    public PersonalBlogsSearchFilterSpecification(string searchString)
    {
        if (string.IsNullOrEmpty(searchString))
        {
            FilterCondition = p => true;
        }
        else
        {
            searchString = searchString.ToLower();
            FilterCondition = p => p.Title.ToLower().Contains(searchString);
        }
    }
}