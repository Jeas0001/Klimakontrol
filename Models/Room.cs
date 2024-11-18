using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Room
    {
        public int ID { get; set; }

        public string Roomnr { get; set; }

        public string DeviceID { get; set; }

        public List<Reading> Readings { get; set; }
    }
}
