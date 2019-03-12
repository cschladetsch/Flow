using System;
using System.Collections.Generic;

namespace Dekuple.Registry
{
    /// <summary>
    /// Root interface for any registry
    /// </summary>
    public interface IRegistry
        : IPrintable
    {
        int NumInstances { get; }
        bool Has(Guid id);
        bool Resolve();
    }

    /// <inheritdoc />
    /// <summary>
    /// Interface for a Registry that uses instances that implement
    /// a given interface.
    /// </summary>
    /// <typeparam name="TBase"></typeparam>
    public interface IRegistry<TBase>
        : IRegistry
        where TBase
            : class
            , IHasId
            , IHasDestroyHandler<TBase>
    {
        IEnumerable<TBase> Instances { get; }

        bool Has(TBase instance);
        TBase Get(Guid id);

        // bind an interface to an implementation, which can be abstract
        bool Bind<TInterface, TImpl>()
            where TInterface : TBase where TImpl : TInterface;

        // bind an interface to a singleton
        bool Bind<TInterface, TImpl>(TImpl single)
            where TInterface : TBase where TImpl : TInterface;

        // make a new instance given interface
        TIBase New<TIBase>(params object[] args)
            where TIBase : class, TBase, IHasRegistry<TBase>, IHasDestroyHandler<TBase>;

        TBase Inject(TBase model, Inject inject, Type iface, TBase single);

        TBase Prepare(TBase model);
    }
}
