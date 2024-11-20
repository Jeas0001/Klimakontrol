using Models;
using DBAccess;

namespace BusinessLogic
{
    public class BLogic
    {
        public async Task<bool> AddNewReading(Reading reading, string deviceID)
        {
            DbAccess dBAccess = new DbAccess();
            bool success;
            success = await dBAccess.AddNewReading(reading, deviceID);
            return success;
        }

        public async Task<List<Reading>> GetReadings(int roomID)
        {
            DbAccess dBAccess = new DbAccess();
            var readings = await dBAccess.GetReadings(roomID);

            if (readings == null) { return new List<Reading>(); }

            return readings;
        }

        public async Task<bool> AddNewRoom(Room room)
        {
            DbAccess dBAccess = new DbAccess();
            bool success;
            success = await dBAccess.AddNewRoom(room);
            return success;
        }

        public async Task<List<Room>> GetRooms()
        {
            DbAccess dbAccess = new DbAccess();
            var rooms = dbAccess.GetRooms();

            return await rooms;
        }

        public async Task<bool> EditRoom(int ID, Room room)
        {
            DbAccess dBAccess = new DbAccess();

            bool success = await dBAccess.EditRoom(ID, room);

            return success;
        }

        public async Task<bool> DeleteRoom(int ID)
        {
            using var dBContext = new DBContext();
            int succes;

            var room = await dBContext.Rooms.FirstOrDefaultAsync(r => r.ID == ID);

            if (room == null) { return false; }

            dBContext.Rooms.Remove(room);

            succes = await dBContext.SaveChangesAsync();

            return succes == 1;
        }

        public async Task<bool> AddFirm(Firm firm)
        {
            DbAccess dBAccess = new DbAccess();
            bool success;
            success = await dBAccess.AddFirm(firm);
            return success;
        }
    }
}
