using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using ParallelTestFramework.Tests.v3;
using Xunit;
using Xunit.Sdk;

[assembly: RegisterXunitSerializer(typeof(XunitJsonSerializer), typeof(SerializerTests.Person))]

namespace ParallelTestFramework.Tests.v3;

public class XunitJsonSerializer : IXunitSerializer
{
    public object Deserialize(Type type, string serializedValue)
    {
        return JsonSerializer.Deserialize(serializedValue, type)!;
    }

    public bool IsSerializable(Type type, object? value, [NotNullWhen(false)] out string? failureReason)
    {
        failureReason = null;
        return true;
    }

    public string Serialize(object value)
    {
        return JsonSerializer.Serialize(value);
    }
}

public class SerializerTests
{
    public record Person(string Name, int Age)
    {
        public override string ToString() => Name;
    }

    public static readonly Person[] People = [new("Tom", 10), new("Jim", 20), new("Tim", 30)];
    public static readonly TheoryData<Person> TestCases = new(People);

    [Theory]
    [MemberData(nameof(TestCases))]
    public async Task JsonSerializer_Test(Person person)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(3000));
    }
}
