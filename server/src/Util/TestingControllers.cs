using Microsoft.AspNetCore.Mvc;
using Core.Components.Database;
using MongoDB.Driver;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Core.Components.ObjectStorage;

namespace server.Util;

[ApiController]
[Route("Test")]
public class TestingController(MongoDbContext dbContext, MinioClient minioClient) : ControllerBase
{

    [HttpGet("Database")]
    public async Task<IActionResult> TestDatabaseConnection()
    {

        await dbContext.TestConnection();

        return Ok($"Hello from C# REST Server! Connected to MongoDB (DbContext)\n");
    }

    [HttpGet("ObjStorage")]
    public async Task<IActionResult> TestStorageConnection()
    {

        await minioClient.TestConnection();

        return Ok($"Hello from ObjectStorageServeice! Connected to MinIO\n");
    }




}
