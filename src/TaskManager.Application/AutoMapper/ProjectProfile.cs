using AutoMapper;
using TaskManager.Application.DTOs.Request;
using TaskManager.Application.DTOs.Response;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.AutoMapper;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<ProjectEntity, ProjectResponseDto>();
        CreateMap<ProjectRequestDto, ProjectEntity>();
    }
}