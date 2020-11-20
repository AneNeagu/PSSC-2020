using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.NewtonsoftJson;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Orleans;
using StackUnderflow.Backend.Interfaces;

namespace StackUnderflow.API.GraphQL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebHookController : ControllerBase
    {
        private readonly IClusterClient _clusterClient;

        public WebHookController(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        public IActionResult Get() {
            return Ok("Hello World");
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {

            var eventType = Request.Headers["X-Acloud-Eventtype"].ToString();
            if (eventType == null)
            {
                return BadRequest("X-Acloud-Eventtype missing from headers");
            }

            using(var sr = new StreamReader(HttpContext.Request.Body))
            {
                using(var jsonReader = new JsonTextReader(sr))
                {
                    var json = await JObject.LoadAsync(jsonReader);

                    // todo: replace this with functional style and stream
                    switch (eventType)
                    {
                        case "ApplicationSubscriptionUpdated":

                            var subscriptionEventData = json.ToObject<ApplicationSubscriptionUpdatedEvent>();
                            var subscriptionGrain = _clusterClient.GetGrain<IBackofficeGrain>(Guid.Empty, "backoffice");

                            // todo: split this into two operations create tenant and invite user
                            var subscriptionResponse = subscriptionGrain.CreateTenantAndAdmin(subscriptionEventData.ApplicationSubscription.Organisation.Id,
                                subscriptionEventData.ApplicationSubscription.Organisation.Name, string.Empty, "admin@email.com", "admin", Guid.NewGuid());
                            

                            break;

                        case "ApplicationUserUpdated":

                            var userEventData = json.ToObject<ApplicationUserUpdatedEvent>();
                            var userGrain = _clusterClient.GetGrain<IBackofficeGrain>(Guid.Empty, "backoffice");
                            var organisation = userEventData.ApplicationUser.ApplicationSubscription.Organisation;
                            var user = userEventData.ApplicationUser.User;

                            // todo: split this into two operations create tenant and invite user
                            var response = userGrain.InviteUser(organisation.Id, 
                                user.Id, user.EmailAddress, user.FullName);

                            break;


                        default:
                            break;
                    }

                    
                }
            }

            return Ok();
        }
    }
}
