using System;
using H5.Contract.Constants;
using Newtonsoft.Json;

namespace H5.Contract
{
    [JsonConverter(typeof(ResourceConfigConverter))]
    public class ResourceConfig
    {
        public ResourceConfig()
        {
            Items = new ResourceConfigItem[] { };
        }

        public ResourceConfigItem[] Items{ get; set; }

        public bool HasExtractResources()
        {
            return ExtractItems!= null && ExtractItems.Length > 0;
        }

        public bool HasEmbedResources()
        {
            return EmbedItems != null && EmbedItems.Length > 0;
        }

        public bool IsPrepared { get; private set; }

        private ResourceConfigItem @default;
        public ResourceConfigItem Default
        {
            get
            {
                if (!IsPrepared)
                {
                    throw new InvalidOperationException("Accessing Default before calling Prepare");
                }

                return @default;
            }
        }

        private ResourceConfigItem[] embedItems;
        public ResourceConfigItem[] EmbedItems
        {
            get
            {
                if (!IsPrepared)
                {
                    throw new InvalidOperationException("Accessing EmbedItems before calling Prepare");
                }

                return embedItems;
            }
        }

        private ResourceConfigItem[] extractItems;
        public ResourceConfigItem[] ExtractItems
        {
            get
            {
                if (!IsPrepared)
                {
                    throw new InvalidOperationException("Accessing ExtractItems before calling Prepare");
                }

                return extractItems;
            }
        }

        public void Prepare(ResourceConfigItem @default, ResourceConfigItem[] embedItems, ResourceConfigItem[] extractItems)
        {
            this.@default = @default;
            this.extractItems = extractItems;
            this.embedItems = embedItems;
            IsPrepared = true;
        }
    }

    public class ResourceConfigItem
    {
        public ResourceConfigItem()
        {
        }

        public void SetDefaulValues()
        {
            if (!Extract.HasValue)
            {
                Extract = true;
            }

            if (!Inject.HasValue)
            {
                Inject = true;
            }

            if (!Load.HasValue)
            {
                Load = true;
            }

            if (!Silent.HasValue)
            {
                Silent = true;
            }

            if (!RemoveBom.HasValue)
            {
                RemoveBom = true;
            }
        }

        public string Header{ get; set; }

        public bool? Extract{ get; set; }

        public bool? Inject{ get; set; }

        public bool? Load { get; set; }

        public bool? Silent{ get; set; }

        public bool? RemoveBom{ get; set; }

        public string Name{ get; set; }

        public string Assembly{ get; set; }

        public string Output{ get; set; }

        public string[] Files{ get; set; }

        public string Remark{ get; set; }

        public ResourceConfigItem CloneWithFile(string finalPath, string outputPath)
        {
            return new ResourceConfigItem()
            {
                Header = Header,
                Extract = Extract,
                Inject = Inject,
                Load = Load,
                Silent = Silent,
                RemoveBom = RemoveBom,
                Name = System.IO.Path.GetFileName(finalPath),
                Assembly = Assembly,
                Output = outputPath,
                Files = new[] { finalPath },
                Remark = Remark
            };
        }
    }

    public class ResourceConfigConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var resourceConfig = value as ResourceConfig;

            serializer.Serialize(writer, resourceConfig.Items);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            ResourceConfigItem[] items = null;

            if (reader.TokenType == JsonToken.Boolean)
            {
                // Check if the resource setting format used is "resources": true/false
                var inject = serializer.Deserialize<bool>(reader);

                items = new ResourceConfigItem[]
                {
                    new ResourceConfigItem()
                    {
                        Inject = inject
                    }
                };

            }
            else if (reader.TokenType == JsonToken.StartArray)
            {
                // Check if the resource setting format used is "resources": [ {} ]
                items = serializer.Deserialize<ResourceConfigItem[]>(reader);
            }
            else if (reader.TokenType == JsonToken.Null || reader.TokenType == JsonToken.None)
            {
                return existingValue;
            }
            else
            {
                throw new JsonReaderException(
                   string.Format(
                       Messages.Exceptions.ERROR_CONFIG_DESERIALIZATION_NODE,
                       "Resources. It should be either bool value false/true or an array of objects [{ ... }].")
                   );
            }

            var resourceConfig = existingValue as ResourceConfig;
            resourceConfig.Items = items;

            return resourceConfig;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ResourceConfigItem[]) || objectType == typeof(bool);
        }
    }
}
