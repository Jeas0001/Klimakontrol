namespace Models
{
    public class Reading
    {
        public int ID { get; set; }

        public double Temp { get; set; }

        public int CO2Level { get; set; }

        public double Humidity { get; set; }

        public int UnixTime { get; set; }
    }
}
