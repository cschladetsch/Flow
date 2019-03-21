namespace Dekuple.Model
{
    /// <summary>
    /// Base for models which require a fully tracked transform.
    /// </summary>
    public interface ITransformModel
        : IPositionedModel, IRotatedModel, ILocalScaledModel, IWorldScaledModel
    {
    }
}