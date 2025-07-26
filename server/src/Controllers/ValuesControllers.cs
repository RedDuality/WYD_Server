using Microsoft.AspNetCore.Mvc;
using Core; 
using MongoDB.Driver;

namespace server.Controllers
{
    [ApiController]
    [Route("values")]
    public class ValuesController : ControllerBase
    {
        private readonly MongoDbContext _dbContext;

        public ValuesController(MongoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // Example of using the CosmosDbContext to get collection names
            var collections = await _dbContext.database.ListCollectionNames().ToListAsync();
            
            // Example of getting a specific collection (assuming ProfileEventDocument is your model)
            // var profileEventsCollection = _dbContext.GetCollection<ProfileEventDocument>("ProfileEvents");
            // var profileEventsCount = await profileEventsCollection.CountDocumentsAsync(_ => true);

            return Ok($"Hello from C# REST Server! Connected to MongoDB (DbContext). Collections: {string.Join(", ", collections)}\n");
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok($"You requested value with ID: {id}");
        }
    }
}