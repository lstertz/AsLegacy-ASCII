using System.Collections.Generic;

namespace AsLegacy.Configs.Converters
{
    // TEMP implementation to validate configuration feature deserialization.


    public class TestConverter : BaseFeatureConverter<ITest>
    {
        /// <inheritdoc/>
        protected override Dictionary<int, DeserializeDelegate> DeserializationMapping =>
            _deserializationMapping;

        private Dictionary<int, DeserializeDelegate> _deserializationMapping = new()
        {
            { 0, Deserialize<TestA> },
            { 1, Deserialize<TestB> }
        };
    }
}
