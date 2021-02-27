﻿using Newtonsoft.Json;

namespace MyListApp.Graph
{
    public class SharePointColumnImage
    {
        [JsonProperty("type")]
        public string Type { get { return "thumbnail"; } }

        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("fieldName")]
        public string FieldName { get; set; }

        [JsonProperty("serverUrl")]
        public string ServerUrl { get; set; }

        [JsonProperty("serverRelativeUrl")]
        public string ServerRelativeUrl { get; set; }
    }
}
