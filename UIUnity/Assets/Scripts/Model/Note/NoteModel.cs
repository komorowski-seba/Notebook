using System;
using Newtonsoft.Json;

namespace Model.Note
{
    [Serializable]
    public class NoteModel
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("topic")] public string Topic { get; set; }
        [JsonProperty("desc")] public string Desc { get; set; }
    }
}