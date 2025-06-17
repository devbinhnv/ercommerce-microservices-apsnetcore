using AutoMapper;
using System.Reflection;

namespace Infrastructure.Mappings;

public static class AutoMapperExtensions
{
    /// <summary>
    /// Do not map non existing (if forgot ignore, value of that filed is null) fields when mapping
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
}
