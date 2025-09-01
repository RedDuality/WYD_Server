using Microsoft.AspNetCore.Mvc;
using Core.Components.Database;
using MongoDB.Driver;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace server.Controllers;

[ApiController]
[Route("testing")]
public class TestingController(MongoDbContext dbContext) : ControllerBase
{

    [HttpGet("values")]
    public async Task<IActionResult> Get()
    {

        var collections = await dbContext.database.ListCollectionNames().ToListAsync();

        return Ok($"Hello from C# REST Server! Connected to MongoDB (DbContext). Collections: {string.Join(", ", collections)}\n");
    }

    [Authorize]
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Or, if your provider uses a different claim type like "user_id"
        var firebaseUserId = User.FindFirstValue("user_id");

        if (userId == null)
        {
            return Unauthorized(); // Or handle the case where the claim isn't present
        }

        return Ok($"Your user ID is: {userId}");
    }
}
