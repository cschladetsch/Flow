using System;

namespace Dekuple
{
    /// <summary>
    /// Basic interface for all responses to Requests
    /// </summary>
    public interface IResponse
    {
        IRequest Request { get; set; }
        EResponse Type { get; }
        EError Error { get; }
        Guid RequestId { get; }
        string Text { get;}
        object PayloadObject { get; }
        bool Failed { get; }
        bool Success { get; }
    }

    /// <inheritdoc />
    /// <summary>
    /// An IResponse with a typed payload
    /// </summary>
    /// <typeparam name="TPayload"></typeparam>
    public interface IResponse<out TPayload>
        : IResponse
    {
        TPayload Payload { get; }
    }
}
