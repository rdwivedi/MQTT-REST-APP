using MQTTnet.Client;

namespace MQTT_REST.Controllers
{
    public interface IMqttClientFactory
    {
        IMqttClient CreateMqttClient();
    }
}