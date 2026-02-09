#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable IDE0028 // Simplify collection initialization
#pragma warning disable xUnit1051 // Calls to methods which accept CancellationToken should use TestContext.Current.CancellationToken

using Xunit;
using Xunit.Sdk;

namespace ParallelTestFramework.Tests.v3;

public class XunitSerializable<T> : IXunitSerializable
{
    public T? Value { get; private set; }

    public XunitSerializable() { }

    public XunitSerializable(T? value) => Value = value;

    public virtual void Deserialize(IXunitSerializationInfo info)
    {
        Value = info.GetValue<T>("_value");
    }

    public virtual void Serialize(IXunitSerializationInfo info)
    {
        info.AddValue("_value", Value);
    }

    public override string? ToString() => Value?.ToString();
}

public class SerializableTheoryDataTests
{
    public record Person(string Name, int Age)
    {
        public override string ToString() => Name;
    }

    public static readonly Person[] People = [new("Tom", 10), new("Jim", 20), new("Tim", 30)];
    public static readonly TheoryData<Person> TestCases = new(People);
    public static readonly TheoryData<XunitSerializable<Person>> SerializableTestCases = new(People.Select(m => new XunitSerializable<Person>(m)));

    [Theory]
    [MemberData(nameof(TestCases))]
    public async Task NonSerializable_Test(Person person)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1000));
    }

    [Theory]
    [MemberData(nameof(SerializableTestCases))]
    public async Task Serializable_Test(XunitSerializable<Person> person)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1000));
    }
}
