# SDET Coding Exercise - Julio Guerra

## Introduction
Automated tests using Selenium Webdriver C# for the website https://www.saucedemo.com/

## Tests

1. Successful Login (SuccessfulLogin)
2. Failed Login (FailedLogin)
3. Happy Path workflow (SubmitOrder)
4. Multiple scenarios workflow (MultipleScenarios)

## Stack
- Selenium Webdriver 4.13.1
- .Net 7.0

Libraries
- Bogus (Fake data)
- DotNetEnv (Env viariables)
- DotNetSelenium Extras
- WebdriverManager (Use latest browser versions)
- NUnit

## Installation
The project requires .Net 7.0

1. Git clone this repository
2. Visual Studio is recommended to open this project

## Usage

### Visual Studio
1. Go to Visual Studio and open project
2. Run tests from the side bar
3. Ensure files like TestData.json and .env has Copy to Output directory active

### Command line
1. Go to main folder in terminal
2. Run the command: `dotnet test`

Note: The 4 tests will run in parallel at the same time. Configuration is: [Parallelizable(ParallelScope.Children)]

## Additional information 
🔧 To Setup Main URL edit .env file /SauceDemoCSTests/.env

📁 To Setup Browser edit App.config file /SauceDemoCSTests/App.config
