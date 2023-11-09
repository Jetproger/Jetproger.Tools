using System;
using System.Collections.Generic;
using Jetproger.Tools.AppConfig;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Factories;

namespace Jetproger.Tools.Convert.Commands
{
    public abstract class DelayCommand : DelayCommand<object, object, object, object, object, object, object, object, object, object, object, object> { }
    public abstract class DelayCommand<T> : DelayCommand<T, T, object, object, object, object, object, object, object, object, object, object> { }
    public abstract class DelayCommand<T, T_> : DelayCommand<T, T_, object, object, object, object, object, object, object, object, object, object> { }
    public abstract class DelayCommand<T, T_, T0> : DelayCommand<T, T_, T0, object, object, object, object, object, object, object, object, object> { }
    public abstract class DelayCommand<T, T_, T0, T1> : DelayCommand<T, T_, T0, T1, object, object, object, object, object, object, object, object> { }
    public abstract class DelayCommand<T, T_, T0, T1, T2> : DelayCommand<T, T_, T0, T1, T2, object, object, object, object, object, object, object> { }
    public abstract class DelayCommand<T, T_, T0, T1, T2, T3> : DelayCommand<T, T_, T0, T1, T2, T3, object, object, object, object, object, object> { }
    public abstract class DelayCommand<T, T_, T0, T1, T2, T3, T4> : DelayCommand<T, T_, T0, T1, T2, T3, T4, object, object, object, object, object> { }
    public abstract class DelayCommand<T, T_, T0, T1, T2, T3, T4, T5> : DelayCommand<T, T_, T0, T1, T2, T3, T4, T5, object, object, object, object> { }
    public abstract class DelayCommand<T, T_, T0, T1, T2, T3, T4, T5, T6> : DelayCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, object, object, object> { }
    public abstract class DelayCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7> : DelayCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, object, object> { }
    public abstract class DelayCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, T8> : DelayCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, T8, object> { }
    public abstract class DelayCommand<TResult, TValue, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> : BaseCommand<TResult, TValue, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>, IDelay
    {
        protected DelayCommand() { if (DelaysCommandsJob.Instance == null) return; }
        public virtual void Complete() { base.SetState(ECommandState.Completed); }
        bool IDelay.CancelDelay { get; set; }
        private bool _isDelayed;

        protected override void SetState(ECommandState state)
        {
            if (state == ECommandState.Completed && !((IDelay)this).CancelDelay)
            {
                if (!_isDelayed)
                {
                    _isDelayed = true;
                    DelaysCommandsJob.Instance.Delay(this);
                }
                return;
            }
            base.SetState(state);
        }
    }

    public class DelaysCommandsJob : InfinityJob
    {
        private DelaysCommandsJob() : base(k<JetprogerJobCommandPeriodSeconds>.key<int>() * 1000, 1000, 5000) { OnExecute += () => new _DelayCommand(this); }
        private static readonly DelaysCommandsJob[] InstanceHolder = { null };
        private readonly Queue<IDelay> _commands = new Queue<IDelay>();

        public static DelaysCommandsJob Instance
        {
            get
            {
                if (InstanceHolder[0] == null)
                {
                    lock (InstanceHolder)
                    {
                        if (InstanceHolder[0] == null)
                        {
                            InstanceHolder[0] = new DelaysCommandsJob();
                            InstanceHolder[0].Execute();
                        }
                    }
                }
                return InstanceHolder[0];
            }
        }

        public IDelay Delayed()
        {
            lock (_commands) { return _commands.Count > 0 ? _commands.Dequeue() : null; }
        }

        public void Delay(IDelay command)
        {
            lock (_commands) { _commands.Enqueue(command); }
        }

        private class _DelayCommand : BaseCommand<object, object>, ICommand
        {
            public _DelayCommand(DelaysCommandsJob job) { _job = job; } 
            private readonly DelaysCommandsJob _job;
            public void Execute()
            {
                try
                {
                    if (State != ECommandState.None) return;
                    State = ECommandState.Running;
                    var delayed = _job.Delayed();
                    if (delayed != null) delayed.Complete();
                    Result = delayed ?? (object)this;
                }
                catch (Exception e)
                {
                    Error = e;
                }
            }
        }
    }

    public interface IDelay
    {
        bool CancelDelay { get; set; } 
        void Complete();
    }
}