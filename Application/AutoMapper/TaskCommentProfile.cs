using Application.DTOs.Request;
using Application.DTOs.Response;
using AutoMapper;
using Domain.Entities;

namespace Application.AutoMapper;

public class TaskCommentProfile : Profile
{
    public TaskCommentProfile()
    {
        CreateMap<TaskCommentEntity, TaskCommentResponseDto>();
        CreateMap<CreateTaskCommentDto, TaskCommentEntity>();
    }
}