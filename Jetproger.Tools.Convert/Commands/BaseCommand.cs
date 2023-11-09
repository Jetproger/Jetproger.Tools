using System;
using System.Collections.Generic;
using System.Diagnostics;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Caches;
using Jetproger.Tools.Convert.Commanders;
using Jetproger.Tools.Convert.Converts;
using Jetproger.Tools.Convert.Factories;

namespace Jetproger.Tools.Convert.Commands
{
    public abstract class BaseCommand : BaseCommand<object, object, object, object, object, object, object, object, object, object, object, object> { }
    public abstract class BaseCommand<T> : BaseCommand<T, T, object, object, object, object, object, object, object, object, object, object> { }
    public abstract class BaseCommand<T, T_> : BaseCommand<T, T_, object, object, object, object, object, object, object, object, object, object> { }
    public abstract class BaseCommand<T, T_, T0> : BaseCommand<T, T_, T0, object, object, object, object, object, object, object, object, object> { }
    public abstract class BaseCommand<T, T_, T0, T1> : BaseCommand<T, T_, T0, T1, object, object, object, object, object, object, object, object> { }
    public abstract class BaseCommand<T, T_, T0, T1, T2> : BaseCommand<T, T_, T0, T1, T2, object, object, object, object, object, object, object> { }
    public abstract class BaseCommand<T, T_, T0, T1, T2, T3> : BaseCommand<T, T_, T0, T1, T2, T3, object, object, object, object, object, object> { }
    public abstract class BaseCommand<T, T_, T0, T1, T2, T3, T4> : BaseCommand<T, T_, T0, T1, T2, T3, T4, object, object, object, object, object> { }
    public abstract class BaseCommand<T, T_, T0, T1, T2, T3, T4, T5> : BaseCommand<T, T_, T0, T1, T2, T3, T4, T5, object, object, object, object> { }
    public abstract class BaseCommand<T, T_, T0, T1, T2, T3, T4, T5, T6> : BaseCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, object, object, object> { }
    public abstract class BaseCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7> : BaseCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, object, object> { }
    public abstract class BaseCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, T8> : BaseCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, T8, object> { }
    public abstract class BaseCommand<TResult, TValue, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> : TraceListener, IParameterizedCommand, IBaseCommand
    {
        event Action IBaseCommand.Executed { add { _executed += value; } remove { if (_executed != null) _executed -= value; } }
        private Action _executed;

        ICommand IBaseCommand.History { get => History; set => History = value; }
        public ICommand History { get => GetHistory(); private set => SetHistory(value); }
        protected virtual void SetHistory(ICommand history) { Set(_history, history); }
        protected virtual ICommand GetHistory() { return Get(_history); }
        private readonly ICommand[] _history = { null };

        object IBaseCommand.Result { get => Result; set => Result = GetResult(value); }
        public TResult Result { get => GetResult(); protected set => SetResult(value); }
        protected virtual TResult GetResult() { return Get(_result); }
        private readonly TResult[] _result = { default(TResult) };

        Exception IBaseCommand.Error { get => Error; set => Error = value; }
        public Exception Error { get => GetError(); protected set => SetError(value); }
        protected virtual Exception GetError() { return Get(_error); }
        private readonly Exception[] _error = { null };

        ECommandState IBaseCommand.State => State;
        public ECommandState State { get => GetState(); protected set => SetState(value); }
        protected virtual ECommandState GetState() { return Get(_state); }
        private readonly ECommandState[] _state = { ECommandState.None };

        private List<CommandMessage> Messages { get { return f.one.of(_messagesHolder, () => new List<CommandMessage>()); } }
        private void WriteLine(CommandMessage message) { lock (Messages) { Messages.Add(message); } }
        public override void WriteLine(string message) { WriteLine((object)message); }
        public override void Write(string message) { WriteLine((object)message); }
        public override void Write(object message) { WriteLine(message); }
        private readonly List<CommandMessage>[] _messagesHolder = { null };
        CommandMessage[] IBaseCommand.Trace { get { return GetTrace(); } }

        protected TResult GetResult(object result) { try { return result.As<TResult>(); } catch { return default(TResult); } }
        protected void Set<T>(T[] holder, T value) { lock (holder) { holder[0] = value; } }
        protected T Get<T>(T[] holder) { lock (holder) { return holder[0]; } }

        object IBaseCommand.Value { get => GetValue(); set => SetValue(value); }
        protected virtual TValue GetValue() { return Value; }

        protected virtual void SetValue(object value)
        {
            try
            {
                if (TryRequest(value)) return;
                if (TryResponse(value)) return;
                if (TryParameterized(value)) return;
                if (TryCommand(value)) return;
                Value = value.As<TValue>();
            }
            catch (Exception e)
            {
                Error = e;
            }
        }

        private bool TryCommand(object value)
        {
            var baseCommand = value as IBaseCommand;
            if (baseCommand == null) return false;
            Value = baseCommand.Result.As<TValue>();
            return true;
        }

        private bool TryParameterized(object value)
        {
            var parameterizedCommand = value as IParameterizedCommand;
            if (parameterizedCommand == null) return false;
            var baseCommand = value as IBaseCommand;
            Value = baseCommand != null ? baseCommand.Result.As<TValue>() : parameterizedCommand.P_.As<TValue>();
            P0 = parameterizedCommand.P0.As<T0>();
            P1 = parameterizedCommand.P1.As<T1>();
            P2 = parameterizedCommand.P2.As<T2>();
            P3 = parameterizedCommand.P3.As<T3>();
            P4 = parameterizedCommand.P4.As<T4>();
            P5 = parameterizedCommand.P5.As<T5>();
            P6 = parameterizedCommand.P6.As<T6>();
            P7 = parameterizedCommand.P7.As<T7>();
            P8 = parameterizedCommand.P8.As<T8>();
            P9 = parameterizedCommand.P9.As<T9>();
            return true;
        }

        private bool TryRequest(object value)
        {
            var request = value as CommandRequest;
            if (request == null) return false;
            if (typeof(TValue) == typeof(CommandRequest))
            {
                Value = request.As<TValue>();
                return true;
            }
            P0 = string.IsNullOrWhiteSpace(request.P0) || Type0 == TypeObject ? default(T0) : (f.sys.issimple(Type0) ? request.P0.As<T0>() : request.P0.As<SimpleXml>().As<T0>());
            P1 = string.IsNullOrWhiteSpace(request.P1) || Type1 == TypeObject ? default(T1) : (f.sys.issimple(Type1) ? request.P1.As<T1>() : request.P1.As<SimpleXml>().As<T1>());
            P2 = string.IsNullOrWhiteSpace(request.P2) || Type2 == TypeObject ? default(T2) : (f.sys.issimple(Type2) ? request.P2.As<T2>() : request.P2.As<SimpleXml>().As<T2>());
            P3 = string.IsNullOrWhiteSpace(request.P3) || Type3 == TypeObject ? default(T3) : (f.sys.issimple(Type3) ? request.P3.As<T3>() : request.P3.As<SimpleXml>().As<T3>());
            P4 = string.IsNullOrWhiteSpace(request.P4) || Type4 == TypeObject ? default(T4) : (f.sys.issimple(Type4) ? request.P4.As<T4>() : request.P4.As<SimpleXml>().As<T4>());
            P5 = string.IsNullOrWhiteSpace(request.P5) || Type5 == TypeObject ? default(T5) : (f.sys.issimple(Type5) ? request.P5.As<T5>() : request.P5.As<SimpleXml>().As<T5>());
            P6 = string.IsNullOrWhiteSpace(request.P6) || Type6 == TypeObject ? default(T6) : (f.sys.issimple(Type6) ? request.P6.As<T6>() : request.P6.As<SimpleXml>().As<T6>());
            P7 = string.IsNullOrWhiteSpace(request.P7) || Type7 == TypeObject ? default(T7) : (f.sys.issimple(Type7) ? request.P7.As<T7>() : request.P7.As<SimpleXml>().As<T7>());
            P8 = string.IsNullOrWhiteSpace(request.P8) || Type8 == TypeObject ? default(T8) : (f.sys.issimple(Type8) ? request.P8.As<T8>() : request.P8.As<SimpleXml>().As<T8>());
            P9 = string.IsNullOrWhiteSpace(request.P9) || Type9 == TypeObject ? default(T9) : (f.sys.issimple(Type9) ? request.P9.As<T9>() : request.P9.As<SimpleXml>().As<T9>());
            Value =string.IsNullOrWhiteSpace(request.Value) || TypeValue == TypeObject ? default(TValue) : (f.sys.issimple(TypeValue) ? request.Value.As<TValue>() : request.Value.As<SimpleXml>().As<TValue>());
            return true;
        }

        private bool TryResponse(object value)
        {
            var response = value as CommandResponse;
            if (response == null) return false;
            if (typeof(TResult) == typeof(CommandResponse))
            {
                Result = response.As<TResult>();
                return true;
            }
            P0 = string.IsNullOrWhiteSpace(response.P0) || Type0 == TypeObject ? default(T0) : (f.sys.issimple(Type0) ? response.P0.As<T0>() : response.P0.As<SimpleXml>().As<T0>());
            P1 = string.IsNullOrWhiteSpace(response.P1) || Type1 == TypeObject ? default(T1) : (f.sys.issimple(Type1) ? response.P1.As<T1>() : response.P1.As<SimpleXml>().As<T1>());
            P2 = string.IsNullOrWhiteSpace(response.P2) || Type2 == TypeObject ? default(T2) : (f.sys.issimple(Type2) ? response.P2.As<T2>() : response.P2.As<SimpleXml>().As<T2>());
            P3 = string.IsNullOrWhiteSpace(response.P3) || Type3 == TypeObject ? default(T3) : (f.sys.issimple(Type3) ? response.P3.As<T3>() : response.P3.As<SimpleXml>().As<T3>());
            P4 = string.IsNullOrWhiteSpace(response.P4) || Type4 == TypeObject ? default(T4) : (f.sys.issimple(Type4) ? response.P4.As<T4>() : response.P4.As<SimpleXml>().As<T4>());
            P5 = string.IsNullOrWhiteSpace(response.P5) || Type5 == TypeObject ? default(T5) : (f.sys.issimple(Type5) ? response.P5.As<T5>() : response.P5.As<SimpleXml>().As<T5>());
            P6 = string.IsNullOrWhiteSpace(response.P6) || Type6 == TypeObject ? default(T6) : (f.sys.issimple(Type6) ? response.P6.As<T6>() : response.P6.As<SimpleXml>().As<T6>());
            P7 = string.IsNullOrWhiteSpace(response.P7) || Type7 == TypeObject ? default(T7) : (f.sys.issimple(Type7) ? response.P7.As<T7>() : response.P7.As<SimpleXml>().As<T7>());
            P8 = string.IsNullOrWhiteSpace(response.P8) || Type8 == TypeObject ? default(T8) : (f.sys.issimple(Type8) ? response.P8.As<T8>() : response.P8.As<SimpleXml>().As<T8>());
            P9 = string.IsNullOrWhiteSpace(response.P9) || Type9 == TypeObject ? default(T9) : (f.sys.issimple(Type9) ? response.P9.As<T9>() : response.P9.As<SimpleXml>().As<T9>());
            Result = string.IsNullOrWhiteSpace(response.Result) || TypeResult == TypeObject ? default(TResult) : (f.sys.issimple(TypeResult) ? response.Result.As<TResult>() : response.Result.As<SimpleXml>().As<TResult>());
            return true;
        }

        public override void WriteLine(object message)
        {
            if (message == null) { }
            else
            if (message is CommandMessage commandMessage) WriteLine(commandMessage);
            else
            if (message is Exception exception) WriteLine(exception.As<CommandMessage>());
            else
            if (true) WriteLine(new CommandMessage { Category = ECommandMessage.Trace.ToString(), Message = message.ToString() });
        }

        private CommandMessage[] GetTrace()
        {
            lock (Messages)
            {
                var messages = Messages.ToArray();
                Messages.Clear();
                return messages;
            }
        }

        public void Reset()
        {
            Set(_error, null);
            Set(_result, default(TResult));
            Set(_state, ECommandState.None);
        }

        protected virtual void SetError(Exception error)
        {
            if (Error != null) return;
            Set(_error, error);
            if (error != null) State = ECommandState.Completed;
        }

        protected virtual void SetResult(TResult result)
        {
            if (State == ECommandState.Completed) return;
            Set(_result, result);
            State = ECommandState.Completed;
        }

        protected virtual void SetState(ECommandState state)
        {
            if (State == ECommandState.Completed) return;
            if (state == ECommandState.Running)
            {
                if (f.mem.of(this, out object result))
                {
                    state = ECommandState.Completed;
                    Set(_result, result.As<TResult>());
                }
            }
            Set(_state, state);
            if (state == ECommandState.Completed)
            {
                if (Error == null) f.mem.to(this, Result);
                Completing();
            }
        }

        private void Completing()
        {
            ErrorProcessing();
            if (_executed != null) _executed();
        }

        private void ErrorProcessing()
        {
            var error = Error;
            if (error == null) return;
            WriteLine(error);
            f.log(error);
        }

        #region parameters

        protected static readonly bool IsOneParam = ((typeof(TValue) == typeof(object) ? 0 : 1) + (typeof(T0) == typeof(object) ? 0 : 1) + (typeof(T1) == typeof(object) ? 0 : 1) + (typeof(T2) == typeof(object) ? 0 : 1) + (typeof(T3) == typeof(object) ? 0 : 1) + (typeof(T4) == typeof(object) ? 0 : 1) + (typeof(T5) == typeof(object) ? 0 : 1) + (typeof(T6) == typeof(object) ? 0 : 1) + (typeof(T7) == typeof(object) ? 0 : 1) + (typeof(T8) == typeof(object) ? 0 : 1) + (typeof(T9) == typeof(object) ? 0 : 1)) == 1;
        protected static readonly bool IsIgnoreResult = typeof(TResult) == typeof(object);
        protected static readonly bool IsIgnoreValue = typeof(TValue) == typeof(object);
        protected static readonly bool IsIgnore0 = typeof(T0) == typeof(object);
        protected static readonly bool IsIgnore1 = typeof(T1) == typeof(object);
        protected static readonly bool IsIgnore2 = typeof(T2) == typeof(object);
        protected static readonly bool IsIgnore3 = typeof(T3) == typeof(object);
        protected static readonly bool IsIgnore4 = typeof(T4) == typeof(object);
        protected static readonly bool IsIgnore5 = typeof(T5) == typeof(object);
        protected static readonly bool IsIgnore6 = typeof(T6) == typeof(object);
        protected static readonly bool IsIgnore7 = typeof(T7) == typeof(object);
        protected static readonly bool IsIgnore8 = typeof(T8) == typeof(object);
        protected static readonly bool IsIgnore9 = typeof(T9) == typeof(object);

        protected static readonly Type TypeResult = typeof(TResult);
        protected static readonly Type TypeObject = typeof(object);
        protected static readonly Type TypeValue = typeof(TValue);
        protected static readonly Type Type0 = typeof(T0);
        protected static readonly Type Type1 = typeof(T1);
        protected static readonly Type Type2 = typeof(T2);
        protected static readonly Type Type3 = typeof(T3);
        protected static readonly Type Type4 = typeof(T4);
        protected static readonly Type Type5 = typeof(T5);
        protected static readonly Type Type6 = typeof(T6);
        protected static readonly Type Type7 = typeof(T7);
        protected static readonly Type Type8 = typeof(T8);
        protected static readonly Type Type9 = typeof(T9);

        object IParameterizedCommand.P_ { get => Value; set => Value = value.As<TValue>(); }
        object IParameterizedCommand.P0 { get => P0; set => P0 = value.As<T0>(); }
        object IParameterizedCommand.P1 { get => P1; set => P1 = value.As<T1>(); }
        object IParameterizedCommand.P2 { get => P2; set => P2 = value.As<T2>(); }
        object IParameterizedCommand.P3 { get => P3; set => P3 = value.As<T3>(); }
        object IParameterizedCommand.P4 { get => P4; set => P4 = value.As<T4>(); }
        object IParameterizedCommand.P5 { get => P5; set => P5 = value.As<T5>(); }
        object IParameterizedCommand.P6 { get => P6; set => P6 = value.As<T6>(); }
        object IParameterizedCommand.P7 { get => P7; set => P7 = value.As<T7>(); }
        object IParameterizedCommand.P8 { get => P8; set => P8 = value.As<T8>(); }
        object IParameterizedCommand.P9 { get => P9; set => P9 = value.As<T9>(); }

        public TValue Value { get => Get(_p_); set => Set(_p_, value); }
        protected T0 P0 { get => Get(_p0); set => Set(_p0, value); }
        protected T1 P1 { get => Get(_p1); set => Set(_p1, value); }
        protected T2 P2 { get => Get(_p2); set => Set(_p2, value); }
        protected T3 P3 { get => Get(_p3); set => Set(_p3, value); }
        protected T4 P4 { get => Get(_p4); set => Set(_p4, value); }
        protected T5 P5 { get => Get(_p5); set => Set(_p5, value); }
        protected T6 P6 { get => Get(_p6); set => Set(_p6, value); }
        protected T7 P7 { get => Get(_p7); set => Set(_p7, value); }
        protected T8 P8 { get => Get(_p8); set => Set(_p8, value); }
        protected T9 P9 { get => Get(_p9); set => Set(_p9, value); }

        private readonly TValue[] _p_ = { default(TValue) };
        private readonly T0[] _p0 = { default(T0) };
        private readonly T1[] _p1 = { default(T1) };
        private readonly T2[] _p2 = { default(T2) };
        private readonly T3[] _p3 = { default(T3) };
        private readonly T4[] _p4 = { default(T4) };
        private readonly T5[] _p5 = { default(T5) };
        private readonly T6[] _p6 = { default(T6) };
        private readonly T7[] _p7 = { default(T7) };
        private readonly T8[] _p8 = { default(T8) };
        private readonly T9[] _p9 = { default(T9) };

        #endregion
    }

    public interface IParameterizedCommand
    {
        object P_ { get; set; }
        object P0 { get; set; }
        object P1 { get; set; }
        object P2 { get; set; }
        object P3 { get; set; }
        object P4 { get; set; }
        object P5 { get; set; }
        object P6 { get; set; }
        object P7 { get; set; }
        object P8 { get; set; }
        object P9 { get; set; }
    }

    public interface IBaseCommand
    {
        CommandMessage[] Trace { get; }
        ICommand History { get; set; }
        Exception Error { get; set; }
        ECommandState State { get; }
        object Result { get; set; }
        object Value { get; set; }
        event Action Executed;
    }

    public enum ECommandState
    {
        None,
        Running,
        Completed
    }
}