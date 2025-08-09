using System.Text;
using RabbitMQ.Client;


public class QueueService : IDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly string _queueName;

    public QueueService(IConfiguration configuration)
    {
        var rabbitConfig = configuration.GetSection("RabbitMQ");
        var factory = new ConnectionFactory()
        {
            HostName = rabbitConfig["HostName"],
            UserName = rabbitConfig["UserName"],
            Password = rabbitConfig["Password"],
            VirtualHost = rabbitConfig["VirtualHost"] ?? "/",
            Port = int.TryParse(rabbitConfig["Port"], out var port) ? port : 5672
        };

        _queueName = rabbitConfig["SendProductQueueName"] ?? "default_queue";
        _connection = factory.CreateConnectionAsync().Result;
        _channel = _connection.CreateChannelAsync().Result;
        _channel.QueueDeclareAsync(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
    }

    public async Task SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        var properties = new BasicProperties();
        await _channel.BasicPublishAsync(exchange: "", routingKey: _queueName, mandatory: false, basicProperties: properties, body: body, cancellationToken: CancellationToken.None);
    }

 
    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}
