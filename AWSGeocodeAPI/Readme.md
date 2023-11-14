# AWS Geocode API

This project shows how to run an ASP.NET Core Web API project as an AWS Lambda exposed through Amazon API Gateway, alongside with the Google Maps Geocode API.
The NuGet package [Amazon.Lambda.AspNetCoreServer](https://www.nuget.org/packages/Amazon.Lambda.AspNetCoreServer) contains a Lambda function that is used to translate requests from API Gateway into the ASP.NET Core framework and then the responses from ASP.NET Core back to API Gateway.

For more information about how the Amazon.Lambda.AspNetCoreServer package works and how to extend its behavior view its [README](https://github.com/aws/aws-lambda-dotnet/blob/master/Libraries/src/Amazon.Lambda.AspNetCoreServer/README.md) file in GitHub.

### Configuring for Application Load Balancer ###

To configure this project to handle requests from an Application Load Balancer instead of API Gateway change
the base class of `LambdaEntryPoint` from `Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction` to 
`Amazon.Lambda.AspNetCoreServer.ApplicationLoadBalancerFunction`.

## Steps to follow to get started from the command line:

1. Install Amazon.Lambda.Tools Global Tools if not already installed.
```
    dotnet tool install -g Amazon.Lambda.Tools
```

2. If already installed check if new version is available.
```
    dotnet tool update -g Amazon.Lambda.Tools
```

3. Execute unit tests
```
    cd AWSGeocodeAPI.Tests
    dotnet test
```

4. Deploy application
```
    cd AWSGeocodeAPI
    dotnet lambda deploy-serverless
```
