namespace Dekuple.Model
{
    using Registry;

    /// <summary>
    /// Default registry for models. Of course, you can create
    /// your own registry that is not based on Dekuple.Model.IModel.
    ///
    /// This is useful as a starting point at least, and will probably
    /// serve well for most applications.
    /// </summary>
    public class ModelRegistry
        : Registry<IModel>
    {
    }
}
