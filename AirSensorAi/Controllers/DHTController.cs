using FirebaseAdmin.Database;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class DHTController : ControllerBase
{
    private readonly FirebaseDatabase _database;

    public DHTController(FirebaseDatabase database)
    {
        _database = database;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] DeviceDto deviceData)
    {
        var dbRef = _database.GetReference("Leituras");
        var newPostRef = dbRef.Push(); // Cria um ID único
        await newPostRef.SetAsync(deviceData);
        return Ok(deviceData);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var snapshot = await _database.GetReference("Leituras").GetValueAsync();
        return Ok(snapshot.Value);
    }
}