using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoTracker.Models;
using TodoTracker.Models.Dto;
using TodoTracker.Data;
using Microsoft.AspNetCore.Authorization;
using TodoTracker.Extensions;

namespace TodoTracker.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly AppDbContext _db;

    public ProjectsController(AppDbContext context)
    {
        _db = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAll()
    {
        var userId = User.GetId();

        var projects = await _db.Projects
            .Where(p => p.OwnerId == userId)
            .Select(p => ToDto(p))
            .ToListAsync();
        return Ok(projects);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDto>>  GetById (Guid id)
    {
        var userId = User.GetId();

        var project = await _db.Projects
            .FirstOrDefaultAsync(p => p.OwnerId == userId && p.Id == id);

        if(project == null) return NotFound();

        var projectDto = ToDto(project);
        return projectDto;
    }

    [HttpPost]
    public async Task<ActionResult<ProjectDto>> Create(CreateProjectRequest request)
    {
        var  p = new Project()
        {
            Name = request.Name,
            Description = request.Description,
            Id = Guid.CreateVersion7(),
            OwnerId = User.GetId()
        };

        _db.Projects.Add(p);
        await _db.SaveChangesAsync();

        var pDto = ToDto(p);
        
        return CreatedAtAction(nameof(GetById), new { id = p.Id }, pDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProjectDto>> Update(Guid id, UpdateProjectRequest request)
    {
        var userId = User.GetId();
        var project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == id && p.OwnerId == userId);
        if(project is null) return NotFound();
        project.Name = request.Name;
        project.Description = request.Description;
        await _db.SaveChangesAsync();
        return ToDto(project);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ProjectDto>> Delete(Guid id)
    {
        var userId = User.GetId();
        var project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == id && p.OwnerId == userId);
        if(project is null) return NotFound();
        _db.Projects.Remove(project);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    private static ProjectDto ToDto(Project p) => new ProjectDto
    {
        Id = p.Id,
        Name = p.Name,
        Description = p.Description
    };
}
