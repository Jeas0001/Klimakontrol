using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using BusinessLogic;
using Models;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;

class Program
{
    static async Task Main(string[] args)
    {
        string broker = "93.166.84.21";
        int port = 1883;
        string clientId = Guid.NewGuid().ToString();
        string topic = "user/jeppe/sensor";

        // Create a MQTT client factory
        var factory = new MqttFactory();

        // Create a MQTT client instance
        var mqttClient = factory.CreateMqttClient();

        // Create MQTT client options
        var options = new MqttClientOptionsBuilder()
            .WithTcpServer(broker, port) // MQTT broker address and port
            .WithCleanSession()
            .Build();

        // Connect to MQTT broker
        var connectResult = await mqttClient.ConnectAsync(options);

        if (connectResult.ResultCode == MqttClientConnectResultCode.Success)
        {
            Console.WriteLine("Connected to MQTT broker successfully.");

            // Subscribe to a topic
            await mqttClient.SubscribeAsync(topic);



            // Callback function when a message is received
            mqttClient.ApplicationMessageReceivedAsync += e =>
            {
                string sensorData = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
                string[] data = sensorData.Split(' ');
                int co2Level;
                double temperature;
                double humidity;
                int time;
                co2Level = Convert.ToInt32(data[1]);
                temperature = Convert.ToDouble(data[3]);
                humidity = Convert.ToDouble(data[5]);
                time = Convert.ToInt32(data[7]);

                if (co2Level != -1 && temperature != 0 && humidity != 0)
                {
                    Reading reading = new Reading();
                    reading.CO2Level = co2Level;
                    reading.Temp = temperature;
                    reading.Humidity = humidity;
                    reading.UnixTime = time;

                    AddReading(reading, data[9]);
                }

                Console.WriteLine($"Received message: {Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment)}");
                return Task.CompletedTask;
            };

            while (true)
            {

            }
            // Unsubscribe and disconnect
            await mqttClient.UnsubscribeAsync(topic);
            await mqttClient.DisconnectAsync();
        }
        else
        {
            Console.WriteLine($"Failed to connect to MQTT broker: {connectResult.ResultCode}");
        }

        async void AddReading(Reading reading, string deviceID)
        {
            BLogic bLogic = new BLogic();
            int roomID;
            var rooms = await bLogic.GetRooms();
            foreach (var item in rooms)
            {
                if (item.DeviceID == deviceID)
                {
                    roomID = item.ID;
                    bool flag = await bLogic.AddNewReading(reading, roomID);
                    Console.WriteLine(flag);
                }
            }
        }
    }
}
