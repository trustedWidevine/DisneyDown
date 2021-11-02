﻿using Newtonsoft.Json;

namespace DisneyDown.Common.API.Schemas.MediaSchemas.DmcVideoBundleSchemaContainer
{
    public class DmcVideoBundle
    {
        [JsonProperty("extras")]
        public Extras Extras { get; set; }

        [JsonProperty("promoLabels")]
        public object[] PromoLabels { get; set; }

        [JsonProperty("related")]
        public Related Related { get; set; }

        [JsonProperty("video")]
        public Video Video { get; set; }
    }
}