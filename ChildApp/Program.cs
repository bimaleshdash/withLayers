using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyLib.DependencyInjection;

var config = new ConfigurationBuilder().AddEnvironmentVariables().Build();
var services = new ServiceCollection();
services.AddMyLibServices();
services.AddLlmClient(config);
var sp = services.BuildServiceProvider();

var hello = sp.GetRequiredService<MyLib.Services.IHelloService>();
var message = hello.GetGreeting("Child");
Console.WriteLine(message);

var llm = sp.GetRequiredService<MyLib.LLM.ILLMClient>();
var llmAnswer = await llm.QueryAsync("Say hello from child app");
Console.WriteLine($"LLM: {llmAnswer}");

// Demonstrate Selenium availability & Page Object Model (LoginPage)
Console.WriteLine($"Selenium IWebDriver type loaded: {typeof(OpenQA.Selenium.IWebDriver)}");
Console.WriteLine($"SeleniumDemo helper reports: {SeleniumDemo.GetWebDriverType()}");

// Page Object Model demo: we can't safely create a ChromeDriver in CI without setup,
// so show that the LoginPage compiles and can be created using reflection.
var loginPageType = typeof(ChildApp.Pages.LoginPage);
Console.WriteLine($"LoginPage type: {loginPageType.FullName}");
