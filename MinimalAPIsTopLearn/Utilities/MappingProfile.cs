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

        CreateMap<InstructorInfo, InstructorDTO>().ReverseMap();
        CreateMap<InstructorCreateDTO, InstructorInfo>()
            .ForMember(p => p.Picture, options=> options.Ignore())
            .ReverseMap();

        CreateMap<CourseInfo, CourseDTO>().ReverseMap();
        CreateMap<CourseCreateDTO, CourseInfo>()
            .ForMember(p => p.Thumbnail, options => options.Ignore())
            .ReverseMap();
    }
}
