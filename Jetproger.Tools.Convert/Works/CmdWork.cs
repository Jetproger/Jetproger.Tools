using System; 
using ePlus.Kiz.Extensions.Commands;
using ePlus.Kiz.Extensions.Bases; 

namespace Jetproger.Tools.Convert.Works
{
    public static class CmdWork<TResult, TParameter>
    { 
        public static CmdWork<TResult> Op<TCommand>(TParameter parameter) where TCommand : BaseCommand<TResult, TParameter>
        {
            return new CmdWork<TResult>(Activator.CreateInstance<TCommand>(), Kz.Xml<ClearXml>.AsString(parameter));
        }
    }

    public class CmdWork : CmdWork<object>
    {
        public CmdWork(ICommand command, string xml) : base(command, xml) { }

        public new static CmdWork Op(ICommand command, object parameter)
        {
            return new CmdWork(command, Kz.Xml<ClearXml>.AsString(parameter));
        }
    }

    public class CmdWork<TResult> : Work<TResult>
    {
        protected readonly ICommandAsync CommandAsync;

        protected readonly string XmlParameter;

        protected readonly ICommand Command;

        public CmdWork(ICommand command, string xml)
        {
            CommandAsync = command as ICommandAsync;
            XmlParameter = xml;
            Command = command;
        }

        public static CmdWork<TResult> Op(ICommand command, object parameter)
        {
            return new CmdWork<TResult>(command, Kz.Xml<ClearXml>.AsString(parameter));
        }

        public static CmdWork<TResult> Op<TCommand>(object parameter) where TCommand : ICommand
        {
            return new CmdWork<TResult>(Activator.CreateInstance<TCommand>(), Kz.Xml<ClearXml>.AsString(parameter));
        }

        protected override void OnStart()
        {
            if (CommandAsync != null) OnStartAsync(); else OnStartSync();
        }

        private void OnStartSync()
        {
            try
            {
                Result = GetResult(Command.Execute(XmlParameter));
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

        private void OnStartAsync()
        {
            try
            {
                CommandAsync.BeginExecute(TryEndExecute, XmlParameter);
            }
            catch (Exception e)
            {
                ErrorAction(e);
            }
        }

        private void TryEndExecute(string xmlResult)
        {
            try
            {
                Result = GetResult(xmlResult);
                Error = Error ?? Kz.Cmd.Exception(CommandAsync.Error);
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

        private TResult GetResult(string xmlResult)
        {
            return !string.IsNullOrWhiteSpace(xmlResult) ? Kz.Xml<ClearXml>.As<TResult>(xmlResult) : default(TResult);
        }
    }
}