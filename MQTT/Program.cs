using System;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;

class Program
{
    private static void Main(string[] args)
    {
        // 1. Create a new MQTT client
        var factory = new MqttFactory();
        var mqttClient = factory.CreateMqttClient();

        // 2. Configure MQTT client options
        var options = new MqttClientOptionsBuilder()
            .WithClientId("CSharpSubscriber")
            .WithTcpServer("broker.hivemq.com", 1883) // Public test broker
            .Build();

        // 3. Attach event handlers
        mqttClient.UseConnectedHandler(async e =>
        {
            Console.WriteLine("Connected to broker!");

            // Subscribe to topic
            await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder()
                .WithTopic("test/topic")
                .Build());

            Console.WriteLine("Subscribed to topic 'test/topic'.");
        });

        mqttClient.UseDisconnectedHandler(e =>
        {
            Console.WriteLine("Disconnected from broker.");
        });

        mqttClient.UseApplicationMessageReceivedHandler(e =>
        {
            // Handle received messages
            var message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            Console.WriteLine($"Received message: {message} on topic: {e.ApplicationMessage.Topic}");
        });

        // 4. Connect to the broker
        await mqttClient.ConnectAsync(options);

        // Keep the application running
        Console.WriteLine("Press Enter to exit.");
        Console.ReadLine();

        // 5. Disconnect
        await mqttClient.DisconnectAsync();
    }
}