using AutoMapper;
using MinimalAPIsTopLearn.DTOs;
using MinimalAPIsTopLearn.Entities;

namespace MinimalAPIsTopLearn.Utilities;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CategoryInfo, CategoryDTO>().ReverseMap();
        CreateMap<CategoryCreateDTO, CategoryInfo>().ReverseMap();
    }
}
