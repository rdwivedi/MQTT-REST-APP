using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MQTTnet.Client;
using MQTTnet.Protocol;
using MQTTnet.Server;
using MQTTnet;
using System.Text;
using Newtonsoft.Json;

namespace MQTT_REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromBody] User user)
        {
            string broker = "111.93.55.222";
            int port = 1883;
            string clientId = Guid.NewGuid().ToString();
            string topic = "xenio/mqtt";
            string username = "emqxtest";
            string password = "password";

            // Create a MQTT client factory
            var factory = new MqttFactory();

            // Create a MQTT client instance
            var mqttClient = factory.CreateMqttClient();

            // Create MQTT client options
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(broker, port)
                .WithCredentials(username, password)
                .WithClientId(clientId)
                .WithCleanSession()
                .Build();

            // Connect to the MQTT broker
            var connectResult = await mqttClient.ConnectAsync(options);


            // Publish the JSON payload to a topic
            var payload = JsonConvert.SerializeObject(user);
            await mqttClient.PublishAsync(new MqttApplicationMessage
            {
                Topic = topic,
                Payload = Encoding.UTF8.GetBytes(payload),
                QualityOfServiceLevel = MqttQualityOfServiceLevel.AtLeastOnce,
            });

            // Disconnect from the MQTT broker
            await mqttClient.DisconnectAsync();

            return Ok("User created and MQTT command sent.");
        }
    }
}
