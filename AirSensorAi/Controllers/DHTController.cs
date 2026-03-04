using FirebaseAdmin;
using FirebaseAdmin.Database;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class DHTController : ControllerBase
{
    private static FirebaseApp _firebaseApp;
    private static FirebaseDatabase _database;

    public DHTController()
    {
        if (_firebaseApp == null)
        {
            string jsonConfig = Environment.GetEnvironmentVariable("FIREBASE_CONFIG");

            if (!string.IsNullOrEmpty(jsonConfig))
            {
                _firebaseApp = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromJson(jsonConfig)
                });

                _database = FirebaseDatabase.GetInstance(_firebaseApp, "https://airsensorai-default-rtdb.firebaseio.com");
            }
        }
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