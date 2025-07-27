using Microsoft.AspNetCore.Mvc;
using Core;
using MongoDB.Driver;

namespace server.Controllers;

[ApiController]
[Route("api/util")]
public class ValuesController : ControllerBase
{
    private readonly MongoDbContext _dbContext;

    public ValuesController(MongoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("values")]
    public async Task<IActionResult> Get()
    {

        var collections = await _dbContext.database.ListCollectionNames().ToListAsync();

        return Ok($"Hello from C# REST Server! Connected to MongoDB (DbContext). Collections: {string.Join(", ", collections)}\n");
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        return Ok($"You requested value with ID: {id}");
    }
}
