
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoTracker.Data;
using TodoTracker.Extensions;
using TodoTracker.Models.Dto;
using TodoTracker.Models;

namespace TodoTracker.Controllers;


[Authorize]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _db; 
    public TasksController(AppDbContext context)
    {
        _db = context;

    }

    [HttpPost("/api/projects/{projectId}/tasks")]
    public async Task<ActionResult<TodoTaskDto>> Create(Guid projectId, CreateTodoTaskRequest request)
    {
        var userId = User.GetId();
        var project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == projectId && p.OwnerId == userId);

        if(project is null) return NotFound();

        var task = new TodoTask
        {
            Id = Guid.CreateVersion7(),
            Name = request.Name,
            Description = request.Description,
            IsDone = false,
            ProjectId = projectId
            
        };
        _db.TodoTasks.Add(task);
        await _db.SaveChangesAsync();
        return Created($"/api/tasks/{task.Id}", ToDto(task));

    }

    [HttpGet("/api/projects/{projectId}/tasks")]
    public async Task<ActionResult<IEnumerable<TodoTaskDto>>> GetAll(Guid projectId)
    {
        var userId = User.GetId();

        var project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == projectId && p.OwnerId == userId);

        if(project is null) return NotFound();

        var tasks = await _db.TodoTasks
            .Where(t => t.ProjectId == projectId)
            .ToListAsync();

        var tasksDto = tasks.Select(t => ToDto(t));

        return Ok(tasksDto);
    }


    private static TodoTaskDto ToDto(TodoTask t) => new TodoTaskDto
    {
        Id = t.Id,
        Name = t.Name,
        Description = t.Description,
        IsDone = t.IsDone,
        ProjectId = t.ProjectId
    };


}   