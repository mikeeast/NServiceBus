﻿namespace NServiceBus
{
    /// <summary>
    /// Called in order to configure logging.
    /// </summary>
    /// <remarks>
    /// If you want logging configured regardless of profiles, do not use this interface,
    /// instead configure logging before you call <see cref="NServiceBus.Configure.With()"/> if you self hosting or configure logging in <see cref="IConfigureThisEndpoint.Customize"/>.
    /// Implementors should work against the generic version of this interface <see cref="IConfigureLoggingForProfile{T}"/>.
    /// </remarks>
    public interface IConfigureLogging
    {
        /// <summary>
        /// Performs all logging configuration.
        /// </summary>
        // ReSharper disable once UnusedParameter.Global            
        void Configure(IConfigureThisEndpoint specifier);
    }
}
