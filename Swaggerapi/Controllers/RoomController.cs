using BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Swaggerapi.Controllers
{
    [ApiController]
    [Route("api/room/[controller]")]
    public class RoomController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> CreateRoom(Room room)
        {
            BLogic bLogic = new BLogic();
            bool success = await bLogic.AddNewRoom(room);
            if (!success) { return Problem(); }
            return Ok();
        }

        [HttpPost("reading/{roomID}")]
        public async Task<IActionResult> AddReading([FromBody] Reading reading, int roomID = 0)
        {
            BLogic bLogic = new BLogic();
            bool success = await bLogic.AddNewReading(reading, roomID);
            if (!success) { return BadRequest(new { error = "Room does not exist" }); }
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> ReadRoom(int roomID)
        {
            BLogic bLogic = new BLogic();
            var readings = await bLogic.GetReadings(roomID);
            if (readings != null) { return Ok(readings); } else { return BadRequest(new { error = "Room does not exist" }); }
        }
    }
}
