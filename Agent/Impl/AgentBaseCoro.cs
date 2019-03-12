using Flow;

namespace Dekuple.Agent
{
    using Model;

    /// <summary>
    /// Base for agents that perform actions over time.
    /// </summary>
    /// <typeparam name="TModel">The model that this Agent represents</typeparam>
    public abstract class AgentBaseCoro<TModel>
        : AgentBase<TModel>
        where TModel
            : class
            , IModel
    {
        protected AgentBaseCoro(TModel model)
            : base(model)
        {
        }

        protected INode _Node
        {
            get
            {
                if (_node != null)
                    return _node;
                _node = New.Node();
                _node.Name = Name;
                Root.Add(_node);
                return _node;
            }
        }

        private INode _node;
    }
}
