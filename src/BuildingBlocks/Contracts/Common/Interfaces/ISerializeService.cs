namespace Contracts.Common.Interfaces;

public interface ISerializeService
{
    string? Seriallize<T>(T obj);
    string Seriallize<T>(T obj, Type type);
    T Deserialize<T>(string text);
}
