using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace WebApi.Common.Models
{
    public class KeycloakCertsModel
    {
        [JsonProperty(PropertyName = "keys")] public IList<JsonWebKey> Keys { get; set; }
    }
}