using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AsLegacy.Configs.Converters
{
    /// <summary>
    /// Provides type discriminated conversion for a feature of a <see cref="Configuration"/>.
    /// </summary>
    /// <typeparam name="FeatureInterface">The interface type of the feature.</typeparam>
    public abstract class BaseFeatureConverter<FeatureInterface> : JsonConverter<FeatureInterface>
    {
        /// <summary>
        /// Performs deserialization from the provided reader to the specified 
        /// feature implementation.
        /// </summary>
        /// <typeparam name="FeatureImplementation">The type of the feature 
        /// implementation to be deserialized to.</typeparam>
        /// <param name="reader">The reader providing the json to be deserialized.</param>
        /// <returns>The deserialized feature implementation.</returns>
        protected static FeatureInterface Deserialize<FeatureImplementation>(
            ref Utf8JsonReader reader) where FeatureImplementation : FeatureInterface
        {
            return JsonSerializer.Deserialize<FeatureImplementation>(ref reader);
        }

        /// <summary>
        /// Wrapping delegate for 
        /// <see cref="Deserialize{FeatureImplementation}(ref Utf8JsonReader)"/>.
        /// </summary>
        /// <param name="reader">The reader providing the json to be deserialized.</param>
        /// <returns>The deserialized feature implementation.</returns>
        protected delegate FeatureInterface DeserializeDelegate(ref Utf8JsonReader reader);

        /// <summary>
        /// The mapping of type discriminators to feature implementation deserializations.
        /// </summary>
        protected abstract Dictionary<int, DeserializeDelegate> DeserializationMapping { get; }


        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert) =>
            typeof(FeatureInterface).IsAssignableFrom(typeToConvert);

        /// <inheritdoc/>
        public override FeatureInterface Read(ref Utf8JsonReader reader, Type typeToConvert, 
            JsonSerializerOptions options)
        {
            Utf8JsonReader readerClone = reader;

            if (readerClone.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            readerClone.Read();
            if (readerClone.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }

            string propertyName = readerClone.GetString();
            if (propertyName != "TypeDiscriminator")
            {
                throw new JsonException();
            }

            readerClone.Read();
            if (readerClone.TokenType != JsonTokenType.Number)
            {
                throw new JsonException();
            }

            return DeserializationMapping[readerClone.GetInt32()](ref reader);
        }

        /// <inheritdoc/>
        public override void Write(
            Utf8JsonWriter writer, FeatureInterface t, JsonSerializerOptions options)
        {
            throw new NotSupportedException("Configuration Converters do not " +
                "support serialization.");
        }
    }
}
