using BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Swaggerapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpGet]
        public async Task<IActionResult> GetRooms()
        {
            BLogic bLogic = new BLogic();
            var rooms = await bLogic.GetRooms();
            if (rooms != null) { return Ok(rooms); } else { return BadRequest(new { error = "Cant find any rooms" }); }
        }

        [HttpPost("reading/{roomID}")]
        public async Task<IActionResult> AddReading([FromBody] Reading reading, int roomID = 0)
        {
            BLogic bLogic = new BLogic();
            bool success = await bLogic.AddNewReading(reading, roomID);
            if (!success) { return BadRequest(new { error = "Room does not exist" }); }
            return Ok();
        }

        [HttpGet("readings/{roomID}")]
        public async Task<IActionResult> ReadRoom(int roomID)
        {
            BLogic bLogic = new BLogic();
            var readings = await bLogic.GetReadings(roomID);
            if (readings != null) { return Ok(readings); } else { return BadRequest(new { error = "Room does not exist" }); }
        }
    }
}
