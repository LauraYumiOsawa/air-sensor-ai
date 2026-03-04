using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class DHTController : ControllerBase
{
    private readonly IFirebaseClient _client;

    public DHTController(IFirebaseClient client)
    {
        _client = client;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] DeviceDto deviceData)
    {
        PushResponse response = await _client.PushAsync("Leituras/", deviceData);
        return Ok(deviceData);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        FirebaseResponse response = await _client.GetAsync("Leituras/");
        var data = response.ResultAs<Dictionary<string, DeviceDto>>();
        return Ok(data?.Values.ToList() ?? new List<DeviceDto>());
    }
}