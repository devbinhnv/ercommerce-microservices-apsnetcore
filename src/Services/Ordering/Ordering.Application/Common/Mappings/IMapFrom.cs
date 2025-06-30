using AutoMapper;

namespace Ordering.Application.Common.Mappings;

public interface IMapFrom<T>
{
    void Mapping(MappingProfile profile) => profile.CreateMap(typeof(T), GetType());
}
