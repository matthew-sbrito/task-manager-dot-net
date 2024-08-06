using AutoMapper;
using TaskManager.Application.DTOs.Request;
using TaskManager.Application.DTOs.Response;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.AutoMapper;

public class TaskCommentProfile : Profile
{
    public TaskCommentProfile()
    {
        CreateMap<TaskCommentEntity, TaskCommentResponseDto>();
        CreateMap<CreateTaskCommentDto, TaskCommentEntity>();
    }
}