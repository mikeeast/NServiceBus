namespace NServiceBus.Sagas
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Extensibility;
    using Persistence;

    /// <summary>
    /// Defines the basic functionality of a persister for storing
    /// and retrieving a sagaData.
    /// </summary>
    public interface ISagaPersister
    {
        /// <summary>
        /// Saves the sagaData entity to the persistence store.
        /// </summary>
        /// <param name="sagaData">The sagaData data to save.</param>
        /// <param name="correlationProperty">The property to correlate. Can be null.</param>
        /// <param name="session">Storage session.</param>
        /// <param name="context">The current pipeline context.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
        Task Save(IContainSagaData sagaData, SagaCorrelationProperty correlationProperty, ISynchronizedStorageSession session, ContextBag context, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing sagaData entity in the persistence store.
        /// </summary>
        /// <param name="sagaData">The sagaData data to updated.</param>
        /// <param name="session">The session.</param>
        /// <param name="context">The current pipeline context.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
        Task Update(IContainSagaData sagaData, ISynchronizedStorageSession session, ContextBag context, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a sagaData entity from the persistence store by its Id.
        /// </summary>
        /// <param name="sagaId">The Id of the sagaData data to get.</param>
        /// <param name="session">The session.</param>
        /// <param name="context">The current pipeline context.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
        Task<TSagaData> Get<TSagaData>(Guid sagaId, ISynchronizedStorageSession session, ContextBag context, CancellationToken cancellationToken = default)
            where TSagaData : class, IContainSagaData;

        /// <summary>
        /// Looks up a sagaData entity by a given property.
        /// </summary>
        /// <param name="propertyName">From the data store, analyze this property.</param>
        /// <param name="propertyValue">From the data store, look for this value in the identified property.</param>
        /// <param name="session">The session.</param>
        /// <param name="context">The current pipeline context.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
        Task<TSagaData> Get<TSagaData>(string propertyName, object propertyValue, ISynchronizedStorageSession session, ContextBag context, CancellationToken cancellationToken = default)
            where TSagaData : class, IContainSagaData;

        /// <summary>
        /// Sets a sagaData as completed and removes it from the active sagaData list
        /// in the persistence store.
        /// </summary>
        /// <param name="sagaData">The sagaData to complete.</param>
        /// <param name="session">The session.</param>
        /// <param name="context">The current pipeline context.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
        Task Complete(IContainSagaData sagaData, ISynchronizedStorageSession session, ContextBag context, CancellationToken cancellationToken = default);
    }
}