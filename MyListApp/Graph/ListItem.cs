using Newtonsoft.Json;
using System.Collections.Generic;

namespace MyListApp.Graph
{
    public class ListItem
    {
        [JsonProperty("fields")]
        public Dictionary<string, string> Fields { get; set; }
    }
}
