using System;
using Jetproger.Tools.Convert.Bases; 
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Convert.Commanders
{
    public class CommandProvider : MarshalByRefObject
    {
        private static readonly CommandController Controller = new CommandController();

        public bool Live()
        {
            return true;
        }

        public bool MainExecute(string executingAssemblyName, string[] args)
        {
            Settings.ConsoleSetting.Parse(args);
            var commandType = ResolveType(executingAssemblyName, out var commandTypeName);
            if (commandType == null)
            {
                f.log((new TypeNotFoundException(commandTypeName)).Message);
                return false;
            }
            var command = f.sys.valueof(commandType) as IAppCommand;
            if (command == null)
            {
                f.log((new TypeNotSubtypeException(commandTypeName, typeof(IAppCommand).FullName)).Message);
                return false;
            }
            return command.Execute();
        }

        public void BaseExecute(string executingAssemblyName)
        {
            if (ServerCommandsJob.Instance == null) return;
            var commandType = ResolveType(executingAssemblyName, out var commandTypeName);
            if (commandType == null)
            {
                f.log((new TypeNotFoundException(commandTypeName)).Message);
                return;
            }
            var command = f.sys.valueof(commandType) as Commands.ICommand;
            if (command == null)
            {
                f.log(new TypeNotSubtypeException(commandTypeName, typeof(Commands.ICommand).FullName));
                return;
            }
            command.Execute();
        }

        public string HttpExecute(string json)
        {
            try
            {
                return Controller.Execute(json.As<NewtonsoftJson>().As<CommandRequest>()).As<NewtonsoftJson>().As<string>();
            }
            catch (Exception e)
            {
                f.log(e);
                return $"HttpExecuteException: {e.Message}";
            }
        }

        private Type ResolveType(string executingAssemblyName, out string fullName)
        {
            if (executingAssemblyName.EndsWith(".exe") || executingAssemblyName.EndsWith(".dll")) executingAssemblyName = executingAssemblyName.Substring(0, executingAssemblyName.Length - 4);
            var names = executingAssemblyName.Split('.');
            var commandTypeName = names.Length > 1 ? names[1] : (names.Length > 0 ? names[0] : executingAssemblyName);
            commandTypeName = commandTypeName.EndsWith("Job") ? commandTypeName : commandTypeName + "Job";
            fullName = executingAssemblyName + "." + commandTypeName;
            return f.sys.classof(fullName);
        }
    }

    public interface IAppCommand
    {
        bool Execute();
    }
}