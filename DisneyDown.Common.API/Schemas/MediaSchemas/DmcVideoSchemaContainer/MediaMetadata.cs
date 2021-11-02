﻿using System;
using Newtonsoft.Json;

namespace DisneyDown.Common.API.Schemas.MediaSchemas.DmcVideoSchemaContainer
{
    public class MediaMetadata
    {
        [JsonProperty("activeAspectRatio")]
        public double ActiveAspectRatio { get; set; }

        [JsonProperty("audioTracks")]
        public AudioTrack[] AudioTracks { get; set; }

        [JsonProperty("captions")]
        public AudioTrack[] Captions { get; set; }

        [JsonProperty("facets")]
        public Facet[] Facets { get; set; }

        [JsonProperty("features")]
        public object[] Features { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("mediaId")]
        public Guid MediaId { get; set; }

        [JsonProperty("phase")]
        public string Phase { get; set; }

        [JsonProperty("playbackUrls")]
        public PlaybackUrl[] PlaybackUrls { get; set; }

        [JsonProperty("productType")]
        public string ProductType { get; set; }

        [JsonProperty("runtimeMillis")]
        public long RuntimeMillis { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}