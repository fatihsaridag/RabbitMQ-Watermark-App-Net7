using RabbitMQ.Client;
using System.Linq.Expressions;

namespace RabbitmqApp.MVC.Service
{
    public class RabbitMQClientService : IDisposable
    {
        //Burası bizim client'ımız exchange'in oluşturulması , kuyrugun oluşturulması , kuyruktan alınması hepsi burada gerçekleşecek çünkü burada diğer apilerle haberleşme yok yalnızca bir uygulama


        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;
        public static string ExchangeName = "ImageDirectExchange";
        public static string RoutingWatermark = "watermark-route-image";
        public static string QueueName = "queue-watermark-image";

        private readonly ILogger<RabbitMQClientService> _loggger;


        public RabbitMQClientService(ConnectionFactory connectionFactory, ILogger<RabbitMQClientService> logger)
        {
            _connectionFactory = connectionFactory;
            _loggger = logger;
        }

        public IModel Connect()
        {
            _connection = _connectionFactory.CreateConnection();
            if (_channel is { IsOpen: true})
            {
                return _channel;
            }
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(ExchangeName, type: "direct", true, false);
            _channel.QueueDeclare(QueueName, true, false,false,null);



            _channel.QueueBind(exchange: ExchangeName, queue: QueueName, routingKey: RoutingWatermark);
            _loggger.LogInformation("RabbitMq ile bağlantı kuruldu...");

            return _channel; 


        }

        public void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();
            _connection?.Close();
            _connection?.Dispose();

            _loggger.LogInformation("RabbitMq ile bağlantı koptu...");   

        }
    }
}
