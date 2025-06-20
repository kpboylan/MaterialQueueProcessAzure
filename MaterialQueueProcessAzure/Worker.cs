using Azure.Messaging.ServiceBus;
using MaterialQueueProcessAzure.DAL;
using MaterialQueueProcessAzure.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace MaterialQueueProcessAzure
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private const string QueueName = "Material_Queue";
        private string _connString = string.Empty;
        private ServiceBusClient _client;
        private ServiceBusProcessor _processor;
        private readonly IRepository _repository;

        public Worker(ILogger<Worker> logger, IConfiguration configuration, IRepository repository)
        {


            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));

            Console.WriteLine("Worker constructor started...");
            Console.WriteLine("Queue to process: " + QueueName);

            string serviceBusConn = configuration.GetConnectionString("ServiceBusConn");
            _connString = configuration.GetConnectionString("SqlConnString");

            if (string.IsNullOrEmpty(serviceBusConn) || string.IsNullOrEmpty(_connString))
            {
                Console.WriteLine("One or more connection strings are missing.");
                throw new ArgumentException("Missing required connection strings.");
            }

            _client = new ServiceBusClient(configuration.GetConnectionString("ServiceBusConn"));
            _processor = _client.CreateProcessor(QueueName, new ServiceBusProcessorOptions());
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _processor.ProcessMessageAsync += MessageHandler;
            _processor.ProcessErrorAsync += ErrorHandler;

            _logger.LogInformation("Starting Service Bus processor...");
            await _processor.StartProcessingAsync(stoppingToken);
        }

        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            _logger.LogInformation($"Received message: {body}");

            var material = JsonConvert.DeserializeObject<Material>(body);

            _logger.LogInformation("CreateMaterialReceivedHandler Called - Material: {materialName} at: {time}", material.MaterialName, DateTimeOffset.Now);

            _repository.AddMaterial(material, _logger, _connString);

            await args.CompleteMessageAsync(args.Message);
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception, "Service Bus Error");
            return Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _processor.StopProcessingAsync(cancellationToken);
            await _processor.DisposeAsync();
            await _client.DisposeAsync();
        }
    }
}
