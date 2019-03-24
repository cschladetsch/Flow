using System;

namespace Dekuple
{
    /// <summary>
    /// A request with a callback to a responder
    /// </summary>
    public class Turnaround
    {
        public IRequest Request;
        public Action<IResponse> Responder;

        public Turnaround(IRequest request, Action<IResponse> responder)
        {
            Request = request;
            Responder = responder;
        }
    }
}
