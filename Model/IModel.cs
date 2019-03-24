namespace Dekuple.Model
{
    using Registry;

    /// <inheritdoc cref="IEntity" />
    /// <summary>
    /// Base for all persistent models.
    /// </summary>
    public interface IModel
        : Flow.ILogger
        , IEntity
        , IHasDestroyHandler<IModel>
        , IHasRegistry<IModel>
    {
        /// <summary>
        /// If true, this model has already been prepared.
        /// </summary>
        bool Prepared { get; }

        /// <summary>
        /// Models can require other models fields or properties, so this creates all models
        /// needed by this model.
        ///
        /// Calling this more than once has no effect.
        /// </summary>
        void PrepareModels();
    }
}
