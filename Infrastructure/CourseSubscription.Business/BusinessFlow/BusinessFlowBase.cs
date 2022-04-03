using System;
using System.Threading.Tasks;

namespace CourseSubscription.Business.BusinessFlow
{
    public abstract class BusinessFlowBase<TResponse, TRequest> : IDisposable
    {
        //-------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Executes Flow.
        /// </summary>
        /// <param name="req"></param> 
        /// <returns></returns>
        public abstract Task<TResponse> Execute(TRequest req);
        //-------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public virtual void Dispose() { }
        //-----------------------------------------------------------------------------------------------------------------------------------------
    }

}
