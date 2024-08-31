

namespace W3Labs.ViralRunner.Network
{
    public interface ISerializationOption
    {
        // string ContentType { get; }

        //UserIdentifier UserIdentifier { get; }

        T Deserialize<T>(string text);
    }
}
