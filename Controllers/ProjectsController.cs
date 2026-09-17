using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoTracker.Models;
using TodoTracker.Models.Dto;
using TodoTracker.Data;
namespace TodoTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{

    private static readonly Guid _fakeOwnerId = Guid.CreateVersion7();

    private readonly AppDbContext _db;
    public ProjectsController(AppDbContext context)
    {
        _db = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAll()
    {
        var dtos = await _db.Projects.Select(p => ToDto(p)).ToListAsync();
        return Ok(dtos);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDto>>  GetById (Guid id)
    {
        var project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == id);
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
            OwnerId = _fakeOwnerId // фейковый
        };

        _db.Projects.Add(p);
        await _db.SaveChangesAsync();

        var pDto = ToDto(p);

        
        return CreatedAtAction(nameof(GetById), new { id = p.Id }, pDto);
    }

    private static ProjectDto ToDto(Project p) => new ProjectDto
    {
        Id = p.Id,
        Name = p.Name,
        Description = p.Description
    };

}