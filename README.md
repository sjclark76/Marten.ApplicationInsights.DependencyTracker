# Marten.ApplicationInsights.DependencyTracker

## Introduction

By default if you using Azure App Insights and Marten with PostgreSQL dependency auto-collection [is not supported.](https://docs.microsoft.com/en-us/azure/azure-monitor/app/auto-collect-dependencies)

Which means the Postgres dependency does not display on Application Insights [Application map](https://docs.microsoft.com/en-us/azure/azure-monitor/app/app-map?tabs=net) and [Transaction diagnostics](https://docs.microsoft.com/en-us/azure/azure-monitor/app/transaction-diagnostics) views.

## Getting Started

To use the dependency tracker simply add the custom logger to your document store read the [Marten Documentation](https://martendb.io/documentation/documents/diagnostics/) for more information.

```
var store = DocumentStore.For(_ =>
{
    _.Logger(new AppInsightsMartenLogger(telemetryClient));
});

```

You will also need to make sure that you have your Azure Telemetry client configured with your IOC container. If you are using .Net Core make sure you have configured App Insights correctly.

```
public override void ConfigureServices(IServiceCollection services)
{
    services.AddApplicationInsightsTelemetry();
}
```
