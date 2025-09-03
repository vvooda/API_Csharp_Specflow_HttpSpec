using System;
using TechTalk.SpecFlow;
using Microsoft.Extensions.DependencyInjection;
using ApiTestingFramework.Interfaces;
using ApiTestingFramework.Implementations;

namespace ApiTestingFramework.Hooks
{
    [Binding]
    public class TestHooks
    {
        private readonly ScenarioContext _scenarioContext;

        public TestHooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario]
        public void SetupDependencies()
        {
            var services = new ServiceCollection();
            
            // Register dependencies
            services.AddSingleton<IApiClient, HttpClientService>();
            services.AddSingleton<IResponseHandler, ResponseHandler>();
            services.AddSingleton<IJsonExtractor, JsonExtractor>();
            
            var serviceProvider = services.BuildServiceProvider();
            _scenarioContext.Set(serviceProvider, "ServiceProvider");
        }

        [AfterScenario]
        public void Cleanup()
        {
            var serviceProvider = _scenarioContext.Get<IServiceProvider>("ServiceProvider");
            var apiClient = serviceProvider.GetService<IApiClient>();
            
            if (apiClient is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}