﻿using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DBAccess
{
    public class DbAccess()
    {
        public async Task<bool> AddNewReading(Reading reading, string DeviceID)
        {
            using var dBContext = new DBContext();
            var room = await dBContext.Rooms.Include(r => r.Readings).FirstOrDefaultAsync(x => x.DeviceID == DeviceID);
            int success;

            if (room == null) { return false; }

            room.Readings.Add(reading);
            success = await dBContext.SaveChangesAsync();

            return success == 1;
        }

        public async Task<List<Reading>> GetReadings(int RoomID)
        {
            using var dBContext = new DBContext();
            var room = await dBContext.Rooms.Include(r => r.Readings).FirstOrDefaultAsync(x => x.ID == RoomID);

            if (room == null) { return new List<Reading>(); }

            return room.Readings.ToList();
        }

        public async Task<bool> AddNewRoom(Room room)
        {
            using var dBContext = new DBContext();
            var rooms = await dBContext.Rooms.ToListAsync();
            int success;

            foreach (var ro in rooms)
            {
                if (ro.DeviceID == room.DeviceID) { return false; }
            }

            rooms.Add(room);
            success = await dBContext.SaveChangesAsync();

            return success == 1;
        }

        public async Task<List<Room>> GetRooms()
        {
            using var dBContext = new DBContext();

            return await dBContext.Rooms.ToListAsync();
        }

        public async Task<bool> EditRoom(int ID, Room room)
        {
            using var dBContext = new DBContext();
            int success;

            var cRoom = await dBContext.Rooms.FirstOrDefaultAsync(r => r.ID == ID);

            if (cRoom == null || room == null) { return false; }

            cRoom = room;

            success = await dBContext.SaveChangesAsync();

            return success == 1;
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
            using var dBContext = new DBContext();
            var firms = await dBContext.Firms.ToListAsync();
            int success;

            foreach (var ro in firms)
            {
                if (ro.FirmName == firm.FirmName) { return false; }
            }

            firms.Add(firm);
            success = await dBContext.SaveChangesAsync();

            return success == 1;
        }
    }
}
