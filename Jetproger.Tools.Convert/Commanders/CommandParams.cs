using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Commands;
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Convert.Commanders
{
    public abstract class Command<TResult, T0, T1> : Command<TResult, T0, T1, object, object, object, object, object, object, object, object> { }
    public abstract class Command<TResult, T0, T1, T2> : Command<TResult, T0, T1, T2, object, object, object, object, object, object, object> { }
    public abstract class Command<TResult, T0, T1, T2, T3> : Command<TResult, T0, T1, T2, T3, object, object, object, object, object, object> { }
    public abstract class Command<TResult, T0, T1, T2, T3, T4> : Command<TResult, T0, T1, T2, T3, T4, object, object, object, object, object> { }
    public abstract class Command<TResult, T0, T1, T2, T3, T4, T5> : Command<TResult, T0, T1, T2, T3, T4, T5, object, object, object, object> { }
    public abstract class Command<TResult, T0, T1, T2, T3, T4, T5, T6> : Command<TResult, T0, T1, T2, T3, T4, T5, T6, object, object, object> { }
    public abstract class Command<TResult, T0, T1, T2, T3, T4, T5, T6, T7> : Command<TResult, T0, T1, T2, T3, T4, T5, T6, T7, object, object> { }
    public abstract class Command<TResult, T0, T1, T2, T3, T4, T5, T6, T7, T8> : Command<TResult, T0, T1, T2, T3, T4, T5, T6, T7, T8, object> { } 
    
    public abstract class Command<TResult, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> : Command<TResult, CommandParams<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>>
    {
        private readonly CommandParams<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> _commandParams = new CommandParams<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>();
        protected override CommandParams<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> GetValue() { return _commandParams; }
        protected override void SetValue(CommandParams<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> value) { }
        public T0 P0 { get => _commandParams.P0; set => _commandParams.P0 = value; }
        public T1 P1 { get => _commandParams.P1; set => _commandParams.P1 = value; }
        public T2 P2 { get => _commandParams.P2; set => _commandParams.P2 = value; }
        public T3 P3 { get => _commandParams.P3; set => _commandParams.P3 = value; }
        public T4 P4 { get => _commandParams.P4; set => _commandParams.P4 = value; }
        public T5 P5 { get => _commandParams.P5; set => _commandParams.P5 = value; }
        public T6 P6 { get => _commandParams.P6; set => _commandParams.P6 = value; }
        public T7 P7 { get => _commandParams.P7; set => _commandParams.P7 = value; }
        public T8 P8 { get => _commandParams.P8; set => _commandParams.P8 = value; }
        public T9 P9 { get => _commandParams.P9; set => _commandParams.P9 = value; }
    }

    public class CommandParams<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> : ICommandParams
    {
        string ICommandParams.P0 => f.sys.issimple(typeof(T0)) ? P0.As<string>() : string.Empty;
        string ICommandParams.P1 => f.sys.issimple(typeof(T1)) ? P1.As<string>() : string.Empty;
        string ICommandParams.P2 => f.sys.issimple(typeof(T2)) ? P2.As<string>() : string.Empty;
        string ICommandParams.P3 => f.sys.issimple(typeof(T3)) ? P3.As<string>() : string.Empty;
        string ICommandParams.P4 => f.sys.issimple(typeof(T4)) ? P4.As<string>() : string.Empty;
        string ICommandParams.P5 => f.sys.issimple(typeof(T5)) ? P5.As<string>() : string.Empty;
        string ICommandParams.P6 => f.sys.issimple(typeof(T6)) ? P6.As<string>() : string.Empty;
        string ICommandParams.P7 => f.sys.issimple(typeof(T7)) ? P7.As<string>() : string.Empty;
        string ICommandParams.P8 => f.sys.issimple(typeof(T8)) ? P8.As<string>() : string.Empty;
        string ICommandParams.P9 => f.sys.issimple(typeof(T9)) ? P9.As<string>() : string.Empty;
        public T0 P0 { get; set; }
        public T1 P1 { get; set; }
        public T2 P2 { get; set; }
        public T3 P3 { get; set; }
        public T4 P4 { get; set; }
        public T5 P5 { get; set; }
        public T6 P6 { get; set; }
        public T7 P7 { get; set; }
        public T8 P8 { get; set; }
        public T9 P9 { get; set; }
    }

    [Serializable]
    public class CommandParams : ICommandParams
    {
        public int LifetimeSeconds { get; set; }
        public string P0 { get; set; }
        public string P1 { get; set; }
        public string P2 { get; set; }
        public string P3 { get; set; }
        public string P4 { get; set; }
        public string P5 { get; set; }
        public string P6 { get; set; }
        public string P7 { get; set; }
        public string P8 { get; set; }
        public string P9 { get; set; }
        public CommandParams() { }

        public CommandParams(ICommandParams commandParams)
        {
            P0 = !string.IsNullOrWhiteSpace(commandParams.P0) ? commandParams.P0 : string.Empty;
            P1 = !string.IsNullOrWhiteSpace(commandParams.P1) ? commandParams.P1 : string.Empty;
            P2 = !string.IsNullOrWhiteSpace(commandParams.P2) ? commandParams.P2 : string.Empty;
            P3 = !string.IsNullOrWhiteSpace(commandParams.P3) ? commandParams.P3 : string.Empty;
            P4 = !string.IsNullOrWhiteSpace(commandParams.P4) ? commandParams.P4 : string.Empty;
            P5 = !string.IsNullOrWhiteSpace(commandParams.P5) ? commandParams.P5 : string.Empty;
            P6 = !string.IsNullOrWhiteSpace(commandParams.P6) ? commandParams.P6 : string.Empty;
            P7 = !string.IsNullOrWhiteSpace(commandParams.P7) ? commandParams.P7 : string.Empty;
            P8 = !string.IsNullOrWhiteSpace(commandParams.P8) ? commandParams.P8 : string.Empty;
            P9 = !string.IsNullOrWhiteSpace(commandParams.P9) ? commandParams.P9 : string.Empty;
        }
    }

    public interface ICommandParams
    {
        string P0 { get; }
        string P1 { get; }
        string P2 { get; }
        string P3 { get; }
        string P4 { get; }
        string P5 { get; }
        string P6 { get; }
        string P7 { get; }
        string P8 { get; }
        string P9 { get; }
    }
}