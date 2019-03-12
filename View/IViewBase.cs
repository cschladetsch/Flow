using UnityEngine;

namespace Dekuple.View
{
    using Registry;
    using Agent;

    /// <inheritdoc cref="IEntity" />
    /// <summary>
    /// Common interface for all views
    /// </summary>
    public interface IViewBase
        : IEntity
        , IHasDestroyHandler<IViewBase>
        , IHasRegistry<IViewBase>
    {
        IAgent AgentBase { get; set; }
        GameObject GameObject { get; }

        void SetAgent(IViewBase owner, IAgent agent);
    }

    public interface IView<out TIAgent>
        : IViewBase
        where TIAgent : IAgent
    {
        TIAgent Agent { get; }
    }
}
