using UnityEngine;

namespace Dekuple.View
{
    using Agent;
    using Model;
    using Registry;

    /// <inheritdoc />
    /// <summary>
    /// Common registry for all objects that are in the Unity3d scene (or canvas)
    /// </summary>
    public interface IViewRegistry
        : IRegistry<IViewBase>
    {
        TIView FromPrefab<TIView>(Object prefab)
            where TIView : class, IViewBase;

        TIView FromPrefab<TIView, TIAgent, TModel>(
            IViewBase owner, Object prefab, TModel model)
            where TIView : class, IViewBase
            where TIAgent : class, IAgent, IHasDestroyHandler<IAgent>
            where TModel : IModel;
    }
}
