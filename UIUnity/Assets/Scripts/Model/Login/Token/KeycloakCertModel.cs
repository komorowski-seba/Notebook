using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Model.Login.Token
{
    [Serializable]
    public class KeycloakCertModel
    {
        [JsonProperty("n")] public string PublicKey { get; set; }
        [JsonProperty("e")] public string Exponent { get; set; }
    }

    [Serializable]
    public class Certs
    {
        [JsonProperty("keys")] public IList<KeycloakCertModel> Keys { get; set; }
    }
}