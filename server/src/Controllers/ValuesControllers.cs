using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver; // Add this using directive

namespace server.Controllers // Updated namespace to match new folder/project name
{
    [ApiController]
    [Route("[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly IMongoDatabase _database;

        public ValuesController(IMongoDatabase database)
        {
            _database = database;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var collections = await _database.ListCollectionNames().ToListAsync();
            return Ok($"Hello from C# REST Server! Connected to MongoDB. Collections: {string.Join(", ", collections)}\n");
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok($"You requested value with ID: {id}");
        }
    }
}
