using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Model.Login.Token
{
    [Serializable]
    public class JWTokenModel
    {
        [JsonProperty("sub")] public string Subject { get; set; }
        [JsonProperty("typ")] public string Typ { get; set; }
        [JsonProperty("realm_access")] public Realms Roles { get; set; }
        [JsonProperty("email_verified")] public bool EmailVerified { get; set; }
        [JsonProperty("preferred_username")] public string PreferredUsername { get; set; }
    }

    [Serializable]
    public class Realms
    {
        [JsonProperty("roles")] public IList<string> Roles { get; set; }
    }
}