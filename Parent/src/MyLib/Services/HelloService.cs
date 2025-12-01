using MyLib.Models;

namespace MyLib.Services;

    public class HelloService : IHelloService
{
    public string GetGreeting(string name)
    {
    var g = new Greeting { Name = name };
        return $"Hello, {g.Name}!";
    }
}
