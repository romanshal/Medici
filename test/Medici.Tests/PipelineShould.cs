using FluentAssertions;
using Medici.Abstractions.Contracts;
using Medici.Abstractions.Contracts.Messaging;
using Medici.Abstractions.Pipelines;
using Medici.Tests.Behaviors;
using Medici.Tests.Contracts;
using Medici.Tests.Contracts.Requests;
using Medici.Tests.Contracts.Responses;
using Microsoft.Extensions.DependencyInjection;

namespace Medici.Tests
{
    public class PipelineShould
    {
        [Fact]
        public async Task ResolveConcreteTypesWithBehaviors()
        {
            var caller = new Caller();

            var container = TestDIBuilder.Build(config =>
            {
                config.Scan(scanner =>
                {
                    scanner
                        .FromAssemblyOf<Ping>()
                        .AddClasses(t => t.InNamespaceOf<Ping>()
                            .AssignableTo(typeof(IRequestHandler<,>)))
                        .AsImplementedInterfaces();
                });

                config.AddSingleton<Caller>(caller);
                config.AddTransient<IPipelineBehavior<Ping, Pong>, ConcreteOuterBehavior>();
                config.AddTransient<IPipelineBehavior<Ping, Pong>, ConcreteInnerBehavior>();
                config.AddTransient<IMedici, Medici>();
            });

            var medici = container.GetRequiredService<IMedici>();

            var response = await medici.SendAsync(new Ping("Ping"));

            response.Message.Should().Be("Ping Pong");

            caller.Messages.Should().BeEquivalentTo([
                "Outer behavior before",
                "Inner behavior before",
                "Handler",
                "Inner behavior after",
                "Outer behavior after"
            ]);
        }

        [Fact]
        public async Task ResolveGenericTypesWithBehaviors()
        {
            var caller = new Caller();
            var container = TestDIBuilder.Build(cfg =>
            {
                cfg.Scan(scanner =>
                {
                    scanner
                        .FromAssemblyOf<Ping>()
                        .AddClasses(t => t.InNamespaceOf<Ping>()
                            .AssignableTo(typeof(IRequestHandler<,>)))
                        .AsImplementedInterfaces();
                });
                cfg.AddSingleton<Caller>(caller);

                cfg.AddTransient(typeof(IPipelineBehavior<,>), typeof(GenericOuterBehavior<,>));
                cfg.AddTransient(typeof(IPipelineBehavior<,>), typeof(GenericInnerBehavior<,>));

                cfg.AddTransient<IMedici, Medici>();
            });

            var medici = container.GetRequiredService<IMedici>();

            var response = await medici.SendAsync(new Ping("Ping"));

            response.Message.Should().Be("Ping Pong");

            caller.Messages.Should().BeEquivalentTo([
                "Outer behavior before",
                "Inner behavior before",
                "Handler",
                "Inner behavior after",
                "Outer behavior after"
            ]);
        }

        [Fact]
        public async Task ResolveNilTypesWithBehaviors()
        {
            var caller = new Caller();
            var container = TestDIBuilder.Build(cfg =>
            {
                cfg.Scan(scanner =>
                {
                    scanner
                        .FromAssemblyOf<NilPing>()
                        .AddClasses(t => t.InNamespaceOf<NilPing>()
                            .AssignableTo(typeof(IRequestHandler<>)))
                        .AsImplementedInterfaces();
                });
                cfg.AddSingleton<Caller>(caller);

                cfg.AddTransient<IPipelineBehavior<NilPing, Nil>, NilOuterBehavior>();
                cfg.AddTransient<IPipelineBehavior<NilPing, Nil>, NilInnerBehavior>();

                cfg.AddTransient<IMedici, Medici>();
            });

            var medici = container.GetRequiredService<IMedici>();

            await medici.SendAsync(new NilPing("Ping"));

            caller.Messages.Should().BeEquivalentTo([
                "Outer nil behavior before",
                "Inner nil behavior before",
                "Handler",
                "Inner nil behavior after",
                "Outer nil behavior after"
            ]);
        }

        [Fact]
        public async Task ResolveCommandTypesWithBehaviors()
        {
            var caller = new Caller();

            var container = TestDIBuilder.Build(config =>
            {
                config.Scan(scanner =>
                {
                    scanner
                        .FromAssemblyOf<CommandPing>()
                        .AddClasses(t => t.InNamespaceOf<CommandPing>()
                            .AssignableTo(typeof(IRequestHandler<,>)))
                        .AsImplementedInterfaces();
                });

                config.AddSingleton<Caller>(caller);
                config.AddTransient<IMedici, Medici>();
            });

            var medici = container.GetRequiredService<IMedici>();

            var response = await medici.SendAsync(new CommandPing("Ping"));

            response.IsSuccess.Should().BeTrue();
            response.Error.Should().BeNull();
            response.Value.Should().NotBeNull();
            response.Value.Message.Should().Be("Ping Pong");

            caller.Messages.Should().BeEquivalentTo([
                "Handler",
            ]);
        }
    }
}
