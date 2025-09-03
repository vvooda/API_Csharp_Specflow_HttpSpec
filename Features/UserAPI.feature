Feature: User API Tests
  As a API tester
  I want to verify User API endpoints
  So that I can ensure the API works correctly

Scenario: Get all users from API
  Given I set the base URL to "https://jsonplaceholder.typicode.com"
  When I send a GET request to "/users"
  Then the response status code should be 200
  And the response should contain a list of users
  And I should be able to extract the first user's name

Scenario: Create a new user via POST
  Given I set the base URL to "https://jsonplaceholder.typicode.com"
  And I have the following user data:
    | name     | username | email          | phone       | website    |
    | John Doe | johndoe  | john@test.com  | 123-456-789 | john.com   |
  When I send a POST request to "/users" with the user data
  Then the response status code should be 201
  And the response should contain the created user data

Scenario: Extract nested data from JSON response
  Given I set the base URL to "https://jsonplaceholder.typicode.com"
  When I send a GET request to "/users/1"
  Then the response status code should be 200
  And I extract the city from address using json path "$.address.city"
  And the extracted city should be "Gwenborough"

Scenario: Extract data from array response using JSONPath
  Given I set the base URL to "https://jsonplaceholder.typicode.com"
  When I send a GET request to "/users"
  Then the response status code should be 200
  And I extract the first username from response using json path "$[0].username"
  And the extracted username should be "Bret"