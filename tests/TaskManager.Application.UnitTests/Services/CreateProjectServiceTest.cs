// using TaskManager.Application.UnitTests.Common;
// using TaskManager.Domain.Interfaces;
// using TaskManager.TestCommon.Project;
// using TaskManager.Application.Projects.Commands.CreateProject;
// using MediatR;
//
// namespace TaskManager.Application.UnitTests.Services;
//
// public class CreateProjectServiceTest : ServiceBaseTest
// {
//     private readonly ISender _sender;
//     
//     public CreateProjectServiceTest()
//     {
//         var projectRepositoryMapper = new Mock<IProjectRepository>();
//     
//         MockUnitOfWork
//             .Setup(x => x.ProjectRepository)
//             .Returns(projectRepositoryMapper.Object);
//     }
//     
//     [Fact]
//     public async Task IsValid_ShouldRegisterNew_WhenFieldsGreaterThan5()
//     {
//         // Arrange
//         var request = ProjectFactory.CreateValidPayload();
//         var command = new CreateProjectCommand(1, request.Title, request.Description);
//     
//         // Act
//         var result = await _sender.Send(command);
//     
//         // Assert
//         Assert.False(result.IsError);
//         Assert.NotNull(result.Value);
//         Assert.Empty(result.Errors);
//     }
//     
//     [Fact]
//     public async Task IsInvalid_ShouldNotRegisterNew_WhenFieldsLessThan5Chars()
//     {
//         // Arrange
//         var request = ProjectFactory.CreateInvalidPayload();
//         var command = new CreateProjectCommand(1, request.Title, request.Description);
//         
//         const string expectedMessage = "The field title must have minimum 5 char.\nThe field description must have minimum 5 char.";
//
//         // Act
//         var result = await _sender.Send(command);
//     
//         // Assert
//         Assert.True(result.IsError);
//         Assert.Null(result.Value);
//         Assert.NotEmpty(result.Errors);
//     }
// }