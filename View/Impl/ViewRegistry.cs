using UnityEngine;

namespace Dekuple.View.Impl
{
    using Agent;
    using Model;
    using Registry;

    /// <inheritdoc cref="Registry" />
    /// <summary>
    /// A registry of views. A view is a unity3d-space representation of
    /// and Agent which in turn has a Model.
    /// This is similar to the MVVC/MVC pattern, but extended so that the
    /// 'ViewController' or 'Controller' is an Agent, which has behavior
    /// defined over time since each Agent is a node in a Flow.Kernel
    /// process graph.
    ///
    /// Of course, you can use your own ViewRegistry that doesn't use
    /// Dekuple.IViewBase. But this is a good start and suitable for
    /// most use-cases.
    /// </summary>
    public class ViewRegistry
        : Registry<IViewBase>
        , IViewRegistry
    {
        public override IViewBase Prepare(IViewBase view)
        {
            //?? view.OnDestroyed += v => view.AgentBase.Destroy();
            return base.Prepare(view);
        }

        public TIView FromPrefab<TIView>(Object prefab)
            where TIView : class, IViewBase
        {
            Assert.IsNotNull(prefab);
            var view = Object.Instantiate(prefab) as TIView;
            Assert.IsNotNull(view);
            return Prepare(Prepare(typeof(TIView), view)) as TIView;
        }

        public TIView FromPrefab<TIView, TIAgent, TModel>(IViewBase player, Object prefab, TModel model)
            where TIView : class , IViewBase
            where TIAgent : class, IAgent, IHasDestroyHandler<IAgent>
            where TModel : IModel
        {
            var view = FromPrefab<TIView>(prefab);
            Assert.IsNotNull(view);
            var agent = player.AgentBase.Registry.New<TIAgent>(model);
            view.SetAgent(player, agent);
            Assert.IsTrue(view.IsValid);
            return view;
        }
    }
}
