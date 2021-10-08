using System;
using System.Collections.Generic;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Works;

namespace Jetproger.Tools.Convert.Commands
{
    public abstract class BaseCommandAsync<TOutput, TInput> : BaseCommand<TOutput, TInput>, ICommandAsync
    {
        protected virtual void ApplyWork(object work) { }
        protected CommandTicket Error { get; set; }
        protected bool IsRunning { get; set; }
        protected string Result { get; set; }
        private Action<string> _callback;

        bool ICommandAsync.IsRunning
        {
            get { return IsRunning; }
        }

        CommandTicket ICommandAsync.Error
        {
            get { return Error; }
        }

        string ICommandAsync.Result
        {
            get { return Result; }
        }

        void ICommandAsync.BeginExecute(Action<string> callback, string xml)
        {
            BeginExecute(callback, xml);
        }

        protected virtual void BeginExecute(Action<string> callback, string xml)
        {
            _callback = callback;
        }

        void ICommandAsync.EndExecute()
        {
            if (_callback != null) _callback(Result);
        }

        protected void EndWork<TWorkResult>(Work<TWorkResult> work, Exception error)
        {
            IsRunning = false;
            var e1 = LogError(error);
            Error = Error ?? Je.cmd.Error(error);
            if (work == null)
            {
                Result = Result == null && e1 ? string.Empty : Result;
                return;
            }
            var e2 = LogError(work.Error);
            Error = Error ?? Je.cmd.Error(work.Error);
            if (work.Result == null)
            {
                Result = Result == null && (e1 || e2) ? string.Empty : Result;
                return;
            }
            TryApplyWork(work);
            Result = Result == null && (Error != null || e1 || e2) ? string.Empty : Result;
        }

        private void TryApplyWork(object work)
        {
            try
            {
                ApplyWork(work);
            }
            catch (Exception e)
            {
                SetError(e);
            }
        }

        protected bool SetError(Exception e)
        {
            if (!LogError(e)) return false;
            Error = Je.cmd.Error(e);
            Result = Result == null && Error != null ? string.Empty : Result;
            return true;
        }
    }

    public abstract class BaseCommand<TOutput, TInput> : ICommand
    { 
        private readonly List<CommandTicket> _report = new List<CommandTicket>();
        protected abstract string Execute(string xml);

        public TOutput Execute(TInput parameter)
        {
            var parameterXml = Je.Xml<ClearXml>.AsString(parameter);
            var resultXml = Execute(parameterXml); 
            return Je.Xml<ClearXml>.As<TOutput>(resultXml);
        }

        object ICommand.Execute(object arg)
        {
            return Execute(arg);
        }

        protected bool LogError(Exception e)
        {
            if (e == null) return false;
            var ticket = Je.cmd.Error(e);
            WriteReport(ticket);
            Je.cmd.Log(ticket);
            return true;
        }

        protected bool LogError(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            var ticket = Je.cmd.Error(s);
            WriteReport(ticket);
            Je.cmd.Log(ticket);
            return true;
        }

        protected bool LogTrace(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            var ticket = Je.cmd.Trace(s);
            WriteReport(ticket);
            Je.cmd.Log(ticket);
            return true;
        }

        private void WriteReport(CommandTicket ticket)
        {
            lock (_report)
            {
                _report.Add(ticket);
            }
        }

        public CommandTicket[] ReadReport()
        {
            lock (_report)
            {
                var report = _report.ToArray();
                _report.Clear();
                return report;
            }
        }
    }

    public interface ICommandAsync : ICommand
    {
        bool IsRunning { get; }
        CommandTicket Error { get; } 
        string Result { get; } 
        void BeginExecute(Action<string> callback, string xml);
        void EndExecute();
    }

    public interface ICommand
    {
        object Execute(object arg); 
        CommandTicket[] ReadReport();
    }
}