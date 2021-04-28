using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Json2Kafka.Services
{
    class ProducerService
    {
        private static IProducer<Null, string> _Producer; // singleton du producer
        private static Boolean _SetupDone = false; //pour ne pas rejouer x fois la config
        private static ProducerConfig _ProducerConfig; // configuration du producer (depuis les variables d'environnement)
        private static string _Topic; // (depuis les variables d'environnement)
        private static ILogger _logger;
        
        
        
        // Constructeur du singleton et de la gestion de l'obtention de son instance (paramètres interdits dans le constructeur, donc Setup depuis une méthode séparée)
        private static readonly ProducerService _ProducerServiceInstance = new ProducerService();
        public static ProducerService GetInstance(ILogger<Program> logger, IConfiguration Configuration) {
            if (!_SetupDone)
            {
                _logger = logger;
                // création de la configuration Kafka à partir des infos récupèrées
                // dans appsettings.json qui peuvent être surchargées par
                // les variables d'environements pour tourner en container

                // configs obligatoires
                _Topic = Configuration["Topic"];            
                _ProducerConfig = new ProducerConfig {
                    ClientId = Configuration["ClientId"],
                    CompressionType = CompressionType.Gzip, //Compression active par défaut, faible charge CPU et 0.5x de traffic et empreinte stockage sur kafka
                    BootstrapServers = Configuration["BootstrapServers"],
                    EnableSslCertificateVerification = bool.Parse( Configuration["EnableSslCertificateVerification"] ),
                    EnableIdempotence = bool.Parse( Configuration["EnableIdempotence"] )
                };
                
                // configs optionnelles
                if ("" != Configuration["SaslPassword"]) _ProducerConfig.SaslPassword = Configuration["SaslPassword"];
                if ("" != Configuration["SaslUsername"]) _ProducerConfig.SaslUsername = Configuration["SaslUsername"];
                if ("" != Configuration["SslCaLocation"]) _ProducerConfig.SslCaLocation = Configuration["SslCaLocation"];
                if ("" != Configuration["SaslMechanism"]) _ProducerConfig.SaslMechanism = (SaslMechanism)int.Parse(Configuration["SaslMechanism"]);
                if ("" != Configuration["SecurityProtocol"]) _ProducerConfig.SecurityProtocol = (SecurityProtocol)int.Parse(Configuration["SecurityProtocol"]);

                _Producer = new ProducerBuilder<Null, string>(_ProducerConfig).Build();
                _SetupDone = true;
            }

            return _ProducerServiceInstance;
        }

        // méthode principale pour écrire dans Kafka
        public async Task<object> ProduceAwait(object msg)
        {
            try
            {
                _logger.LogInformation("Sending {json}",msg);
                var deliveryResult = await _Producer.ProduceAsync(_Topic, new Message<Null, string> { Value=msg.ToString() });
                Console.WriteLine($"{{ \"TopicPartitionOffset\":\"{deliveryResult.TopicPartitionOffset}\",\"Kafka_delivered\":{deliveryResult.Value} }}");        
                return deliveryResult;
            }
            catch (ProduceException<Null, string> e)
            {
                _logger.LogError($"Delivery failed: {e.Error.Reason}");
                return e;
            }
        } 

    }

}