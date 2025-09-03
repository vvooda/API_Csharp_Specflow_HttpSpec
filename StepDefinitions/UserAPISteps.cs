using System;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using FluentAssertions;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using ApiTestingFramework.Interfaces;
using ApiTestingFramework.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApiTestingFramework.StepDefinitions
{
    [Binding]
    public class UserAPISteps
    {
        private readonly ScenarioContext _scenarioContext;
        private HttpResponseMessage _response;
        private string _responseContent;
        private readonly IApiClient _apiClient;
        private readonly IResponseHandler _responseHandler;
        private readonly IJsonExtractor _jsonExtractor;

        public UserAPISteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            var serviceProvider = scenarioContext.Get<IServiceProvider>("ServiceProvider");
            _apiClient = serviceProvider.GetRequiredService<IApiClient>();
            _responseHandler = serviceProvider.GetRequiredService<IResponseHandler>();
            _jsonExtractor = serviceProvider.GetRequiredService<IJsonExtractor>();
        }

        [Given(@"I set the base URL to ""(.*)""")]
        public void GivenISetTheBaseURLTo(string baseUrl)
        {
            _apiClient.SetBaseUrl(baseUrl);
        }

        [When(@"I send a GET request to ""(.*)""")]
        public async Task WhenISendAGETRequestTo(string endpoint)
        {
            _response = await _apiClient.GetAsync(endpoint);
            _responseContent = await _responseHandler.GetResponseContentAsync(_response);
            _scenarioContext.Set(_responseContent, "ResponseContent");
        }

        [When(@"I send a POST request to ""(.*)"" with the user data")]
        public async Task WhenISendAPOSTRequestToWithTheUserData(string endpoint)
        {
            var table = _scenarioContext.Get<Table>("UserDataTable");
            var userData = table.Rows[0];
            var user = new User
            {
                Name = userData["name"],
                Username = userData["username"],
                Email = userData["email"],
                Phone = userData["phone"],
                Website = userData["website"]
            };

            var jsonContent = JsonConvert.SerializeObject(user);
            _response = await _apiClient.PostAsync(endpoint, jsonContent);
            _responseContent = await _responseHandler.GetResponseContentAsync(_response);
            _scenarioContext.Set(_responseContent, "ResponseContent");
        }

        [Then(@"the response status code should be (\d+)")]
        public void ThenTheResponseStatusCodeShouldBe(int statusCode)
        {
            _responseHandler.VerifyStatusCode(_response, statusCode);
        }

        [Then(@"the response should contain a list of users")]
        public async Task ThenTheResponseShouldContainAListOfUsers()
        {
            var users = await _responseHandler.DeserializeArrayResponseAsync<User>(_response);
            users.Should().NotBeNull();
            users.Should().NotBeEmpty();
            _scenarioContext.Set(users, "UsersList");

            // Console output of all users
            Console.WriteLine("=== ALL RETRIEVED USERS ===");
            foreach (var user in users)
            {
                Console.WriteLine($"ID: {user.Id}, Name: {user.Name}, Email: {user.Email}");
            }
            Console.WriteLine($"Total users retrieved: {users.Length}");
            Console.WriteLine("===========================");


            // Detailed console output
            Console.WriteLine("=== DETAILED USER INFORMATION ===");
            for (int i = 0; i < users.Length; i++)
            {
                var user = users[i];
                Console.WriteLine($"User #{i + 1}:");
                Console.WriteLine($"  ID: {user.Id}");
                Console.WriteLine($"  Name: {user.Name}");
                Console.WriteLine($"  Username: {user.Username}");
                Console.WriteLine($"  Email: {user.Email}");
                Console.WriteLine($"  Phone: {user.Phone}");
                Console.WriteLine($"  Website: {user.Website}");

                if (user.Address != null)
                {
                    Console.WriteLine($"  Address: {user.Address.Street}, {user.Address.City}, {user.Address.Zipcode}");
                }

                if (user.Company != null)
                {
                    Console.WriteLine($"  Company: {user.Company.Name}");
                }
                Console.WriteLine("---");
            }
            Console.WriteLine($"Total users: {users.Length}");
            Console.WriteLine("=================================");
            

             // Pretty JSON output
                Console.WriteLine("=== USERS IN JSON FORMAT ===");
                var json = JsonConvert.SerializeObject(users, Formatting.Indented);
                Console.WriteLine(json);
                Console.WriteLine("============================");

        }

        [Then(@"I should be able to extract the first user's name")]
        public void ThenIShouldBeAbleToExtractTheFirstUsersName()
        {
            var users = _scenarioContext.Get<User[]>("UsersList");
            users[0].Name.Should().NotBeNullOrEmpty();
            Console.WriteLine($"First user's name: {users[0].Name}");
        }

        [Given(@"I have the following user data:")]
        public void GivenIHaveTheFollowingUserData(Table table)
        {
            // Store the table for later use
            _scenarioContext.Set(table, "UserDataTable");
        }

        [Then(@"the response should contain the created user data")]
        public async Task ThenTheResponseShouldContainTheCreatedUserData()
        {
            var createdUser = await _responseHandler.DeserializeResponseAsync<User>(_response);
            createdUser.Should().NotBeNull();
            createdUser.Name.Should().NotBeNullOrEmpty();
            
            // Verify against the original table data
            var table = _scenarioContext.Get<Table>("UserDataTable");
            var expectedName = table.Rows[0]["name"];
            createdUser.Name.Should().Be(expectedName);
        }

        [When(@"I extract the (.*) from response using json path ""(.*)""")]
        public void WhenIExtractTheFromResponseUsingJsonPath(string valueName, string jsonPath)
        {
            var extractedValue = _jsonExtractor.ExtractString(_responseContent, jsonPath);
            _scenarioContext.Set(extractedValue, valueName);
            Console.WriteLine($"Extracted {valueName}: {extractedValue}");
        }

        [Then(@"the extracted (.*) should be ""(.*)""")]
        public void ThenTheExtractedShouldBe(string valueName, string expectedValue)
        {
            var actualValue = _scenarioContext.Get<string>(valueName);
            actualValue.Should().Be(expectedValue);
        }

        [Then(@"I extract the city from address using json path ""(.*)""")]
        public void ThenIExtractTheCityFromAddressUsingJsonPath(string jsonPath)
        {
            var city = _jsonExtractor.ExtractString(_responseContent, jsonPath);
            _scenarioContext.Set(city, "city");
            Console.WriteLine($"Extracted city: {city}");
        }

        [Then(@"I extract the first username from response using json path ""(.*)""")]
        public void ThenIExtractTheFirstUsernameFromResponseUsingJsonPath(string jsonPath)
        {
            var extractedValue = _jsonExtractor.ExtractString(_responseContent, jsonPath);
            _scenarioContext.Set(extractedValue, "username");
            Console.WriteLine($"Extracted username: {extractedValue}");
        }
    }
}