using Application.DTOs.Request;
using Application.DTOs.Response;
using AutoMapper;
using Domain.Entities;

namespace Application.AutoMapper;

public class TaskProfile : Profile
{
    public TaskProfile()
    {
        CreateMap<TaskEntity, TaskResponseDto>();
        CreateMap<TaskRequestDto, TaskEntity>();
    }
}