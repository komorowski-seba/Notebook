using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace WebApi.Application.Models
{
    public class KeycloakCertsModel
    {
        [JsonProperty(PropertyName = "keys")] public IList<JsonWebKey> Keys { get; set; }
    }
}