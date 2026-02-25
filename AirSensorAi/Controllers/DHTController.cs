using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class DHTController : ControllerBase
{
    public static List<DeviceDto> DeviceDataList = new List<DeviceDto>();

    [HttpPost]
    public IActionResult Post([FromBody] DeviceDto deviceData)
    {
        DeviceDataList.Add(deviceData);
        return Ok(deviceData);
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(DeviceDataList);
    }
}