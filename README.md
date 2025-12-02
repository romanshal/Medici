# Medici

[![NuGet](https://img.shields.io/nuget/v/medici.svg)](https://www.nuget.org/packages/medici)

Just another simple mediator implementation library.

## How to use it?

First you need to install the according packages from NuGet:
- `Medici.Abstractions` contains all abstract contracts and interfaces to use in your code
- `Medici` contains the implementations for the abstractions above
- `Medici.DependencyInjection` contains basic extensions for your `IServiceCollection` for proper dependency registration
- `Medici.CQRS.Abstractions` contains abstract contracts to CQRS and Result patterns
- `Medici.Behaviors.Validation` contains simple validation pre-processor realisation
- `Medici.OpenProcessors` contains the implementations for open pre/post behavior

## Message types

- `IBaseRequest` - marker interface for requests
- `IRequest` - a request message, no return value (including generic variants)
- `ICommand` - a command message with a Result-pattern response
- `IQuery` - a query message with a Result-pattern response

## Handler types

- `IRequestHandler<in TRequest>`
- `IRequestHandler<in TRequest, TResponse>`
- `ICommandHandler<in TCommand>`
- `ICommandHandler<in TCommand, TResponse>`
- `IQueryHandler<in TQuery>`
- `IQueryHandler<in TQuery, TResponse>`

## Pipeline types

- `RequestPreProcessorBehavior<TRequest, TResponse>`
- `RequestPostProcessorBehavior<TRequest, TResponse>`

## Result pattern types

- `Result`
- `Result<TValue>`

## Configuration with `IServiceCollection`

Medici supports `Microsoft.Extensions.DependencyInjection.Abstractions` directly. To register various Medici services and handlers:

```csharp
services.AddMedici(cfg => cfg.RegisterServicesFromAssemblyWithType<Program>());
```

or with an assembly:

```csharp
services.AddMedici(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
```

This registers:

- `IMedici` as transient
- `ISender` as transient
- `IRequestHandler<,>` concrete implementations as transient
- `IRequestHandler<>` concrete implementations as transient

To register pre/post processors:

```csharp
services.AddMedici(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.AddRequestPreProcessor<PingPreProcessor>();
    cfg.AddRequestPostProcessor<PingPongPostProcessor>();
    });
```

or with implementation type

```csharp
services.AddMedici(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.AddRequestPreProcessor<IPingPreProcessor, PingPreProcessor>();
    cfg.AddRequestPostProcessor<IPingPongPostProcessor, PingPongPostProcessor>();
    });
```

## Third-Party Notices

- This package depends on [Scrutor](https://github.com/khellang/Scrutor), licensed under the MIT License.
- This package depends on [FluentValidation](https://github.com/FluentValidation/FluentValidation), licensed under the Apache License 2.0.
