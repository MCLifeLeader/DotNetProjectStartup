using Startup.Common.Helpers.Extensions;

namespace Startup.Tests.Common;

[TestFixture(Category = "Unit")]
public class JsonExtensionsTests
{
    [Test]
    public async Task FromJsonAsync_ShouldDeserializeJsonString()
    {
        string json = "{\"Name\":\"John\", \"Age\":30}";
        var result = await json.FromJsonAsync<Person>();
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("John"));
        Assert.That(result.Age, Is.EqualTo(30));
    }

    [Test]
    public void FromJson_ShouldDeserializeJsonString()
    {
        string json = "{\"Name\":\"John\", \"Age\":30}";
        var result = json.FromJson<Person>();
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("John"));
        Assert.That(result.Age, Is.EqualTo(30));
    }

    [Test]
    public async Task ToJsonAsync_ShouldSerializeObjectToJsonString()
    {
        var person = new Person
        {
            Name = "John",
            Age = 30
        };
        var result = await person.ToJsonAsync();
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Does.Contain("\"Name\":\"John\""));
        Assert.That(result, Does.Contain("\"Age\":30"));
    }

    [Test]
    public void ToJson_ShouldSerializeObjectToJsonString()
    {
        var person = new Person
        {
            Name = "John",
            Age = 30
        };
        var result = person.ToJson();
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Does.Contain("\"Name\":\"John\""));
        Assert.That(result, Does.Contain("\"Age\":30"));
    }

    [Test]
    public async Task FromJsonToJObjectAsync_ShouldDeserializeJsonStringToJObject()
    {
        string json = "{\"Name\":\"John\", \"Age\":30}";
        var result = await json.FromJsonToJObjectAsync();
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result["Name"].ToString(), Is.EqualTo("John"));
            Assert.That(result["Age"].ToObject<int>(), Is.EqualTo(30));
        });
    }

    [Test]
    public void FromJsonToJObject_ShouldDeserializeJsonStringToJObject()
    {
        string json = "{\"Name\":\"John\", \"Age\":30}";
        var result = json.FromJsonToJObject();
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result["Name"].ToString(), Is.EqualTo("John"));
            Assert.That(result["Age"].ToObject<int>(), Is.EqualTo(30));
        });
    }

    [Test]
    public async Task FromJsonToJArrayAsync_ShouldDeserializeJsonStringToJArray()
    {
        string json = "[{\"Name\":\"John\", \"Age\":30}, {\"Name\":\"Jane\", \"Age\":25}]";
        var result = await json.FromJsonToJArrayAsync();
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(result[0]["Name"].ToString(), Is.EqualTo("John"));
            Assert.That(result[0]["Age"].ToObject<int>(), Is.EqualTo(30));
            Assert.That(result[1]["Name"].ToString(), Is.EqualTo("Jane"));
            Assert.That(result[1]["Age"].ToObject<int>(), Is.EqualTo(25));
        });
    }

    [Test]
    public void FromJsonToJArray_ShouldDeserializeJsonStringToJArray()
    {
        string json = "[{\"Name\":\"John\", \"Age\":30}, {\"Name\":\"Jane\", \"Age\":25}]";
        var result = json.FromJsonToJArray();
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(result[0]["Name"].ToString(), Is.EqualTo("John"));
            Assert.That(result[0]["Age"].ToObject<int>(), Is.EqualTo(30));
            Assert.That(result[1]["Name"].ToString(), Is.EqualTo("Jane"));
            Assert.That(result[1]["Age"].ToObject<int>(), Is.EqualTo(25));
        });
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}