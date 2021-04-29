using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Json2Kafka.Services;
using Json2Kafka.BasicAuth;

namespace Json2Kafka.Controllers
{
    // Ajoute une route vide pour afficher l'aide de la méthode GET par défaut en cas d'affichage de la racine sur un navigateur
    [Route("")]
    [Route("api/[controller]")]
    [ApiController]
    public class MsgController : ControllerBase
    {

        private ProducerService _Producer;
        
        // Constructeur du controller de l'API, instanciation du producer Kafka
        public MsgController(ILogger<Program> logger,IConfiguration Configuration) {
            _Producer = ProducerService.GetInstance(logger, Configuration);
        }

        // GET: / or /api/[controller] 
        // Ne fait rien à part afficher une info sur la méthode POST à utiliser
        [HttpGet]
        public ActionResult<Object> GetMessage() => Content("Please use POST method on /api/msg - UTF8 JSON object mandatory\n\nExample : curl -d '{\"key1\":\"value1\", \"key2\":\"value2\"}' -u admin:admin -H \"Content-Type: application/json\" -X POST https://host-xyz.gitpod.io/api/msg");



        // POST: /api/[controller]
        // Méthode asynchrone
        // le message vient du body de la request POST http.
        // le message doit être du JSON UTF8 : {"timestamp": 123,"info": "myData","myKey":"myValue"}
        // si message est différent d'un json la méthode retourne une erreur http "415 Unsupported Media Type"
        // si la requête est incorrecte la méthode retourne "400 Bad Request"
        // si BasicAuth est activé dans la configuration et que le login ou mot de passe sont incorects la méthode retourne une erreur "401 Unauthorized"
        // si la requête est correcte la réponse de kafka est envoyée
        // si kafka est injoignable le producer stock et attend le retour de Kafka (time out kafka par défaut 5 minutes / 300000ms)
        [HttpPost]
        [Route("api/[controller]")] // oblige à utiliser cette route : /api/msg
        [HttpGet("basic")]
        [BasicAuth("thisServiceNotKafka")] 
        public async Task<ActionResult<Object>> PostMessage(Object message)
        {
            var deliveryResult = await _Producer.ProduceAwait(message); // send the message to kafka
            return Created("KafkaDeliveryResult", deliveryResult); // return http 201 created to the requester with a DeliveryResult or ProduceException in case of problem
        }
    }
}
