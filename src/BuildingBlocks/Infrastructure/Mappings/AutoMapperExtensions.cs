using AutoMapper;
using System.Reflection;

namespace Infrastructure.Mappings;

public static class AutoMapperExtensions
{
    /// <summary>
    /// Do not map non existing fields (if forgot ignore, value of that filed is null) fields
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>
        (this IMappingExpression<TSource, TDestination> expression)
    {
        var flags = BindingFlags.Public | BindingFlags.Instance;
        var sourceType = typeof(TSource);
        var destinationProperties = typeof(TDestination).GetProperties(flags);

        foreach (var property in destinationProperties)
        {
            if(sourceType.GetProperty(property.Name, flags) == null)
            {
                expression.ForMember(property.Name, opt => opt.Ignore());
            }
        }

        return expression;
    }

    /// <summary>
    /// Do not map null properties
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static IMappingExpression<TSource, TDestination> IgnoreNullProperties<TSource, TDestination>
        (this IMappingExpression<TSource, TDestination> expression)
    {
        // Only map properties has value
        expression.ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        return expression;
    }
}
