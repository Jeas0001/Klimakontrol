using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Firm
    {
        public int Id { get; set; }

        public string FirmName { get; set; }

        public List<Room> Rooms { get; set; }
    }
}
