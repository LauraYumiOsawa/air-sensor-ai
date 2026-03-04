using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class DHTController : ControllerBase
{
    private readonly FirestoreDb _firestoreDb;

    public DHTController(FirestoreDb firestoreDb)
    {
        _firestoreDb = firestoreDb;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] DeviceDto deviceData)
    {
        var docRef = await _firestoreDb.Collection("Leituras").AddAsync(deviceData);
        return Ok(new { id = docRef.Id, data = deviceData });
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var snapshot = await _firestoreDb.Collection("Leituras").GetSnapshotAsync();
        var data = snapshot.Documents.Select(doc => doc.ConvertTo<DeviceDto>()).ToList();
        return Ok(data);
    }
}