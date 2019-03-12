using App.Common;

namespace Dekuple.Agent
{
    using Model;
    using Registry;

    /// <inheritdoc cref="Flow.ITransient" />
    /// <summary>
    /// AgentBase for all agents. Each agent represents a model and has it's own log.
    /// </summary>
    public interface IAgent
        : Flow.ILogger
        , Flow.ITransient
        , IEntity
        , IGameActor
        , IHasRegistry<IAgent>
        , IHasDestroyHandler<IAgent>
    {
        IModel BaseModel { get; }
    }

    /// <inheritdoc />
    /// <summary>
    /// A type-specific agent.
    /// </summary>
    /// <typeparam name="TModel">The type of the model this agent represents</typeparam>
    public interface IAgent<out TModel>
        : IAgent
        where TModel
            : IModel
    {
        TModel Model { get; }
    }
}
