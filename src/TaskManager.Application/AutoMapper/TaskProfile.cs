using AutoMapper;
using TaskManager.Application.DTOs.Request;
using TaskManager.Application.DTOs.Response;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.AutoMapper;

public class TaskProfile : Profile
{
    public TaskProfile()
    {
        CreateMap<TaskEntity, TaskResponseDto>();
        CreateMap<CreateTaskDto, TaskEntity>();
    }
}