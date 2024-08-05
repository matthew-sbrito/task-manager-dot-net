using Application.DTOs.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Extensions;
using Application.Interfaces;
using AutoMapper;
using Common.Enums;
using Domain.Entities;
using Domain.ORM;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.Services;

public class TaskService(
    IMapper mapper,
    IUnitOfWork unitOfWork,
    ITaskHistoryService taskHistoryService,
    IValidator<CreateTaskDto> createTaskValidator,
    IServiceProvider serviceProvider
) : ServiceBase(serviceProvider), ITaskService
{
    public async Task<Response<IEnumerable<TaskResponseDto>>> GetTasksByProjectIdAsync(int projectId)
    {
        var tasks = await unitOfWork.TaskRepository
            .GetTasksByProjectIdAsync(projectId);

        var response = mapper.Map<IEnumerable<TaskResponseDto>>(tasks);

        return ResponseService.Success(response);
    }

    public async Task<Response<TaskResponseDto>> CreateTaskAsync(int projectId, CreateTaskDto body)
    {
        var userId = GetAuthenticatedUserId();

        var validationResult = await createTaskValidator.ValidateAsync(body);
        
        if (!validationResult.IsValid)
            return ResponseService.Error<TaskResponseDto>(validationResult.GetErrorsMessage());

        var taskEntity = mapper.Map<TaskEntity>(body);

        taskEntity.Status = TaskEntityStatus.Pending;
        taskEntity.ProjectId = projectId;
        taskEntity.CreatedByUserId = userId;

        await unitOfWork.TaskRepository.AddAsync(taskEntity);
        await unitOfWork.TaskRepository.SaveAsync();

        var response = mapper.Map<TaskResponseDto>(taskEntity);

        return ResponseService.Success(response, StatusCodes.Status201Created);
    }

    public async Task<Response<TaskResponseDto>> UpdateTaskAsync(int projectId, int taskId, UpdateTaskDto body)
    {
        var user = await GetAuthenticatedUser();
        var task = await unitOfWork.TaskRepository.GetByIdAsync(taskId);

        if (task is null)
            return ResponseService.Error<TaskResponseDto>("Task not found.", StatusCodes.Status404NotFound);

        try
        {
            await unitOfWork.BeginTransactionAsync();

            await taskHistoryService.RegisterHistory(user, task, body);

            mapper.Map(body, task);
            task.UpdatedByUserId = user.Id;

            await unitOfWork.TaskRepository.UpdateAsync(task);
            await unitOfWork.TaskRepository.SaveAsync();

            await unitOfWork.CommitAsync();

            var response = mapper.Map<TaskResponseDto>(task);
            return ResponseService.Success(response);
        }
        catch (Exception)
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
        finally
        {
            await unitOfWork.DisposeAsync();
        }
    }

    public async Task<Response<TaskCommentResponseDto>> CreateCommentAsync(int projectId, int taskId,
        CreateTaskCommentDto body)
    {
        var user = await GetAuthenticatedUser();
        var task = await unitOfWork.TaskRepository.GetByIdAsync(taskId);

        if (task is null)
            return ResponseService.Error<TaskCommentResponseDto>("Task not found.", StatusCodes.Status404NotFound);

        try
        {
            await unitOfWork.BeginTransactionAsync();

            await taskHistoryService.RegisterHistory(user, task, body);

            var comment = mapper.Map<TaskCommentEntity>(body);
            comment.CreatedByUserId = user.Id;

            await unitOfWork.TaskCommentRepository.AddAsync(comment);
            await unitOfWork.TaskCommentRepository.SaveAsync();

            await unitOfWork.CommitAsync();

            var response = mapper.Map<TaskCommentResponseDto>(comment);
            return ResponseService.Success(response);
        }
        catch (Exception)
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
        finally
        {
            await unitOfWork.DisposeAsync();
        }
    }

    public async Task<Response<bool>> RemoveTaskAsync(int projectId, int taskId)
    {
        var taskEntity = await unitOfWork.TaskRepository.GetByIdAsync(taskId);

        if (taskEntity is null)
            return ResponseService.Error<bool>("Task not found.", StatusCodes.Status404NotFound);

        await unitOfWork.TaskRepository.DeleteAsync(taskEntity);
        await unitOfWork.TaskRepository.SaveAsync();

        return ResponseService.Success(true, StatusCodes.Status204NoContent);
    }
}