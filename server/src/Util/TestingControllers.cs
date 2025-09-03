using Microsoft.AspNetCore.Mvc;
using Core.Components.Database;
using MongoDB.Driver;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace server.Util;

[ApiController]
[Route("Test")]
public class TestingController(MongoDbContext dbContext) : ControllerBase
{

    [HttpGet("Database")]
    public async Task<IActionResult> TestDatabaseConnection()
    {

        await dbContext.database.ListCollectionNames().ToListAsync();

        return Ok($"Hello from C# REST Server! Connected to MongoDB (DbContext)\n");
    }
    
}
