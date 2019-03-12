namespace Dekuple.Registry
{
    /// <summary>
    /// Interface to an instance that has a Registry, a unique Id,
    /// and a Destroy handler.
    /// </summary>
    /// <typeparam name="TBase"></typeparam>
    public interface IHasRegistry<TBase>
        where TBase
            : class
            , IHasId
            , IHasDestroyHandler<TBase>
    {
        IRegistry<TBase> Registry { get; set; }
    }
}
