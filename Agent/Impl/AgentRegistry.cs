namespace Dekuple.Agent
{
    using Flow;
    using Registry;

    /// <summary>
    /// Factory and registry for creating agents.
    ///
    /// Agents have a Model, and are also ITransient
    /// and do work over time in a Flow.Kernel.
    ///
    /// NOTE that you can obviously make your own Agent registries
    /// that do not require you to use IAgent. But this is a good starting point.
    /// </summary>
    public class AgentRegistry
        : Registry<IAgent>
    {
        public IKernel Kernel { get; }
        public IFactory Factory { get; }

        public AgentRegistry()
        {
            Kernel = Create.Kernel();
            Factory = Kernel.Factory;
        }

        public override IAgent Prepare(IAgent agent)
        {
            base.Prepare(agent);
            agent.OnDestroyed += a => a.Complete();
            return Factory.Prepare(agent);
        }
    }
}
