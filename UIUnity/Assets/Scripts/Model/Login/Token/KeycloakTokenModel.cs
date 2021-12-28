using System;
using Newtonsoft.Json;

namespace Model.Login.Token
{
    [Serializable]
    public class KeycloakTokenModel
    {
        [JsonProperty("access_token")] public string AccessToken { get; set; }
    }
}