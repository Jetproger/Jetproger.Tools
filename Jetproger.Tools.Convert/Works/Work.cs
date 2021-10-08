using System;
using System.Collections.Generic;
using System.Threading;

namespace Jetproger.Tools.Convert.Works
{
    public class Work : Work<object>
    {

        private Work(Action action) : base(action)
        {
        }

        public static Work Op(Action action)
        {
            return new Work(action);
        }
    }

    public class Work<TResult> : IWork
    {
        private readonly WorkError _workError = new WorkError();
        protected IWork ContinuationWork { get; private set; }
        public TResult Result { get; protected set; }
        public Exception Error { get; protected set; } 
        object IWork.Result { get { return Result; } }
        private ManualResetEvent _mre;
        private Func<TResult> _func;
        private Action _action;
        private bool _isDisposing;
        private bool _isStart;

        public static Work<TResult> Op(Func<TResult> func)
        {
            return new Work<TResult>(func);
        }

        protected Work(Func<TResult> func)
        {
            _func = func;
        }

        protected Work(Action action)
        {
            _action = action;
        }

        public Work()
        {
        }

        public virtual void Continue()
        {
        }

        public void Assign(IWork work)
        {
            ContinuationWork = work;
        }

        public TResult Execute()
        {
            _mre = new ManualResetEvent(false);
            Start();
            _mre.WaitOne();
            return Result;
        }

        public void Start()
        {
            if (_isStart)
            {
                ContinueAssignedWork(); 
            }
            else
            {
                _isStart = true;
                OnStart();
            }
        }

        protected virtual void OnStart()
        {
            try
            {
                if (_func != null) Result = _func();
                else
                if (_action != null) _action();
            }
            catch (Exception e)
            {
                ErrorAction(e);
            }
            finally
            {
                Dispose();
            }
        }

        public Work<TResult> Catch<T>(Func<Exception, bool> errorHandler) where T : Exception
        {
            _workError.Catch<T>(errorHandler);
            return this;
        }

        public Work<TResult> Catch<T>(Action<Exception> errorHandler) where T : Exception
        {
            _workError.Catch<T>(errorHandler);
            return this;
        }

        public Work<TResult> Catch(Func<Exception, bool> errorHandler)
        {
            _workError.Catch(errorHandler);
            return this;
        }

        public Work<TResult> Catch(Action<Exception> errorHandler)
        {
            _workError.Catch(errorHandler);
            return this;
        }

        protected void ErrorAction(Exception e)
        {
            var repeat = _workError.ErrorProcessing(e);
            Error = _workError.Error;
            if (repeat) OnStart(); else Dispose();
        }

        public void Disposed()
        {
            ThreadStock.Run(Dispose);
        }

        public void Dispose()
        {
            if (_isDisposing) return; 
            _isDisposing = true;
            Disposing();
            if (_mre != null)
            {
                _mre.Set();
                _mre = null;
            }
            ContinueAssignedWork();
        }

        protected virtual void Disposing()
        {
        }

        private void ContinueAssignedWork()
        {
            if (ContinuationWork == null) return;
            ContinuationWork.Continue();
            ContinuationWork = null;
        }
    }

    public sealed class WorkError
    {
        private Dictionary<Type, List<Func<Exception, bool>>> ErrorTypes { get { return Kz.One.Get(_errorTypesHolder, () => new Dictionary<Type, List<Func<Exception, bool>>>()); } }
        private readonly Dictionary<Type, List<Func<Exception, bool>>>[] _errorTypesHolder = { null };
        public Exception Error { get; private set; }

        public bool ErrorProcessing(Exception error)
        {
            Error = error;
            if (error == null) return false;
            var errorHandlers = GetErrorHandlers(error.GetType());
            if (ExecuteErrorHandlers(error, errorHandlers)) return true;
            errorHandlers = GetErrorHandlers(typeof(Exception)); 
            return ExecuteErrorHandlers(error, errorHandlers);
        }

        private bool ExecuteErrorHandlers(Exception error, List<Func<Exception, bool>> errorHandlers)
        {
            var i = 0;
            foreach (Func<Exception, bool> errorHandler in errorHandlers)
            {
                Error = null;
                if (errorHandler(error))
                {
                    errorHandlers.RemoveAt(i);
                    return true;
                }
                i++;
            }
            return false;
        }

        public void Catch<T>(Func<Exception, bool> errorHandler) where T : Exception
        {
            GetErrorHandlers(typeof(T)).Add(CreateErrorHandlerWrapper(errorHandler));
        }

        public void Catch<T>(Action<Exception> errorHandler) where T : Exception
        {
            GetErrorHandlers(typeof(T)).Add(CreateErrorHandlerWrapper(errorHandler));
        }

        public void Catch(Func<Exception, bool> errorHandler)
        {
            GetErrorHandlers(typeof(Exception)).Add(CreateErrorHandlerWrapper(errorHandler));
        }

        public void Catch(Action<Exception> errorHandler)
        {
            GetErrorHandlers(typeof(Exception)).Add(CreateErrorHandlerWrapper(errorHandler));
        }

        private List<Func<Exception, bool>> GetErrorHandlers(Type type)
        {
            var errorTypes = ErrorTypes;
            lock (errorTypes)
            {
                if (!errorTypes.ContainsKey(type)) errorTypes.Add(type, new List<Func<Exception, bool>>());
                return errorTypes[type];
            }
        }

        private Func<Exception, bool> CreateErrorHandlerWrapper(Func<Exception, bool> errorHandler)
        {
            return x =>
            {
                try
                {
                    return errorHandler(x);
                }
                catch (Exception e)
                {
                    Kz.Log.Error(e);
                    return false;
                }
            };
        }

        private Func<Exception, bool> CreateErrorHandlerWrapper(Action<Exception> errorHandler)
        {
            return x =>
            {
                try
                {
                    errorHandler(x);
                }
                catch (Exception e)
                {
                    Kz.Log.Error(e);
                }
                return false;
            };
        }
    }

    public interface IWork
    {
        object Result { get; }
        Exception Error { get; }
        void Start();
        void Continue();
        void Assign(IWork work);
    }
}