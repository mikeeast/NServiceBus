namespace NServiceBus.AcceptanceTests.Core.Sagas
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AcceptanceTesting;
    using EndpointTemplates;
    using Extensibility;
    using NServiceBus;
    using NServiceBus.Persistence;
    using NServiceBus.Pipeline;
    using NServiceBus.Sagas;
    using NUnit.Framework;

    [TestFixture]
    public class When_adding_state_to_context : NServiceBusAcceptanceTest
    {
        [Test]
        public async Task Should_make_state_available_to_finder_context()
        {
            var context = await Scenario.Define<Context>()
                .WithEndpoint<SagaEndpoint>(b => b
                    .When(session => session.SendLocal(new StartSagaMessage())))
                .Done(c => c.FinderUsed)
                .Run();

            Assert.True(context.FinderUsed);
            Assert.AreEqual("SomeData", context.ContextBag.Get<SagaEndpoint.BehaviorWhichAddsThingsToTheContext.State>().SomeData);
        }

        public class Context : ScenarioContext
        {
            public bool FinderUsed { get; set; }
            public IReadOnlyContextBag ContextBag { get; set; }
        }

        public class SagaEndpoint : EndpointConfigurationBuilder
        {
            public SagaEndpoint()
            {
                EndpointSetup<DefaultServer>(c =>
                {
                    //use InMemoryPersistence as custom finder support is required
                    c.UsePersistence<AcceptanceTestingPersistence>();
                    c.Pipeline.Register(new BehaviorWhichAddsThingsToTheContext(), "adds some data to the context");
                });
            }

            class CustomFinder : ISagaFinder<TestSaga07.SagaData07, StartSagaMessage>
            {
                public CustomFinder(Context testContext)
                {
                    this.testContext = testContext;
                }

                public Task<TestSaga07.SagaData07> FindBy(StartSagaMessage message, ISynchronizedStorageSession storageSession, IReadOnlyContextBag context, CancellationToken cancellationToken = default)
                {
                    testContext.ContextBag = context;
                    testContext.FinderUsed = true;
                    return Task.FromResult(default(TestSaga07.SagaData07));
                }

                Context testContext;
            }

            public class TestSaga07 : Saga<TestSaga07.SagaData07>, IAmStartedByMessages<StartSagaMessage>
            {
                public Task Handle(StartSagaMessage message, IMessageHandlerContext context)
                {
                    return Task.FromResult(0);
                }

                protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData07> mapper)
                {
                    // custom finder used
                }

                public class SagaData07 : ContainSagaData
                {
                }
            }

            public class BehaviorWhichAddsThingsToTheContext : IBehavior<IIncomingPhysicalMessageContext, IIncomingPhysicalMessageContext>
            {
                public Task Invoke(IIncomingPhysicalMessageContext context, Func<IIncomingPhysicalMessageContext, Task> next)
                {
                    context.Extensions.Set(new State
                    {
                        SomeData = "SomeData"
                    });

                    return next(context);
                }

                public class State
                {
                    public string SomeData { get; set; }
                }
            }
        }

        public class StartSagaMessage : IMessage
        {
        }
    }
}