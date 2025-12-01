using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyLib.DependencyInjection;

var config = new ConfigurationBuilder().AddEnvironmentVariables().Build();
var services = new ServiceCollection();
services.AddMyLibServices();
services.AddLlmClient(config);
var sp = services.BuildServiceProvider();

var hello = sp.GetRequiredService<MyLib.Services.IHelloService>();
var message = hello.GetGreeting("Child (package)");
Console.WriteLine(message);

var llm = sp.GetRequiredService<MyLib.LLM.ILLMClient>();
var llmAnswer = await llm.QueryAsync("Say hello from package child app");
Console.WriteLine($"LLM: {llmAnswer}");

// Demonstrate Selenium availability (provided transitively by `MyLib` package)
Console.WriteLine($"Selenium IWebDriver type loaded: {typeof(OpenQA.Selenium.IWebDriver)}");
Console.WriteLine($"SeleniumDemo helper reports: {SeleniumDemo.GetWebDriverType()}");
