using FluentAssertions;
using Medici.Abstractions.Contracts;
using Medici.Abstractions.Contracts.Messaging;
using Medici.Abstractions.Pipelines;
using Medici.CQRS.Abstractions.Results;
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
            var output = new OutputLogger();

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

                config.AddSingleton<OutputLogger>(output);
                config.AddTransient<IPipelineBehavior<Ping, Pong>, ConcreteOuterBehavior>();
                config.AddTransient<IPipelineBehavior<Ping, Pong>, ConcreteInnerBehavior>();
                config.AddTransient<IMedici, Medici>();
            });

            var medici = container.GetRequiredService<IMedici>();

            var response = await medici.SendAsync(new Ping("Ping"));

            response.Message.Should().Be("Ping Pong");

            output.Messages.Should().BeEquivalentTo([
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
            var output = new OutputLogger();
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
                cfg.AddSingleton<OutputLogger>(output);

                cfg.AddTransient(typeof(IPipelineBehavior<,>), typeof(GenericOuterBehavior<,>));
                cfg.AddTransient(typeof(IPipelineBehavior<,>), typeof(GenericInnerBehavior<,>));

                cfg.AddTransient<IMedici, Medici>();
            });

            var medici = container.GetRequiredService<IMedici>();

            var response = await medici.SendAsync(new Ping("Ping"));

            response.Message.Should().Be("Ping Pong");

            output.Messages.Should().BeEquivalentTo([
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
            var output = new OutputLogger();
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
                cfg.AddSingleton<OutputLogger>(output);

                cfg.AddTransient<IPipelineBehavior<NilPing, Nil>, NilOuterBehavior>();
                cfg.AddTransient<IPipelineBehavior<NilPing, Nil>, NilInnerBehavior>();

                cfg.AddTransient<IMedici, Medici>();
            });

            var medici = container.GetRequiredService<IMedici>();

            await medici.SendAsync(new NilPing("Ping"));

            output.Messages.Should().BeEquivalentTo([
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
            var output = new OutputLogger();

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

                config.AddSingleton<OutputLogger>(output);
                config.AddTransient<IMedici, Medici>();
            });

            var medici = container.GetRequiredService<IMedici>();

            var response = await medici.SendAsync(new CommandPing("Ping"));

            response.IsSuccess.Should().BeTrue();
            response.Error.Should().BeEquivalentTo(Error.None);
            response.Value.Should().NotBeNull();
            response.Value.Message.Should().Be("Ping Pong");

            output.Messages.Should().BeEquivalentTo([
                "Handler",
            ]);
        }
    }
}
