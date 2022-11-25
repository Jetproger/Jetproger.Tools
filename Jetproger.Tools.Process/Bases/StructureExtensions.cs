using System;
using System.Collections.Generic;
using System.Linq;
using Jetproger.Tools.Cache.Bases;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Converts;
using Jetproger.Tools.Inject.Bases;
using Jetproger.Tools.Process.Commands;
using Jetproger.Tools.Model.Bases;

namespace Jetproger.Tools.Process.Bases
{
    public static class StructureExtensions
    {
        public static void SetParameter(this Task task, string code, object value)
        {
            var item = task.Items.FirstOrDefault(x => x.Code == code);
            if (item == null)
            {
                item = new TaskItem { Code = code, Scope = ParameterScope.Constant, Name = code, Note = code };
                task.Add(item);
            }
            var stringValue = string.Empty;
            if (value != null)
            {
                var type = value.GetType();
                stringValue = ValueExtensions.IsPrimitive(type) ? value.As<string>() : f.web.Of(value);
            }
            item.Value = stringValue;
        }

        public static Command AsCommand(this Task task)
        {
            if (task == null) return null;
            var commandType = Ex.Dotnet.GetType(task.AssemblyName, task.TypeName);
            if (commandType == null) return null;
            var command = Ex.Inject.Resolve(commandType) as Command;
            if (command == null) return null;
            foreach (var item in task.Items)
            {
                if (item.Scope == ParameterScope.Context || item.Scope == ParameterScope.Previous) continue;
                var value = item.Scope == ParameterScope.Constant ? item.Value : ((ExSetting)Ex.Dotnet.GetConfig(Ex.Dotnet.GetType(item.Value))).Name;
                foreach (var p in Ex.Dotnet.GetSimpleProperties(commandType))
                {
                    if (p.Name != item.Name) continue;
                    p.SetValue(command, value, null);
                    break;
                }
            }
            return command;
        }

        public static CommandAgent AsCommandAgent(this Task task)
        {
            if (task == null) return new CommandAgent();
            var commandType = Ex.Dotnet.GetType(task.AssemblyName, task.TypeName);
            if (commandType == null) return null;
            var command = Ex.Inject.Resolve(commandType) as Command;
            if (command == null) return null;
            var agent = new CommandAgent(command);
            foreach (var item in task.Items)
            {
                agent.Map(CreateMapper(task, item));
            }
            return agent;
        }

        private static ICommandMapper CreateMapper(Task task, TaskItem item)
        {
            switch (item.Scope)
            {
                case ParameterScope.Configuration: return CreateMapperConst(task, item, ((ExSetting)Ex.Dotnet.GetConfig(Ex.Dotnet.GetType(item.Value))).Name);
                case ParameterScope.Constant: return CreateMapperConst(task, item, item.Value);
                default: return CreateMapperContext(task, item);
            }
        }

        private static ICommandMapper CreateMapperConst(Task task, TaskItem item, object value)
        {
            var targetType = Ex.Dotnet.GetType(task.AssemblyName, task.TypeName);
            if (targetType == null) return null;
            var mapperType = typeof(Mapper<>);
            mapperType = mapperType.MakeGenericType(targetType);
            var targetName = $"{task.AssemblyName},{task.TypeName},{item.Name}";
            return Activator.CreateInstance(mapperType, targetName, item.Value) as ICommandMapper;
        }

        private static ICommandMapper CreateMapperContext(Task task, TaskItem item)
        {
            var targetType = Ex.Dotnet.GetType(task.AssemblyName, task.TypeName);
            if (targetType == null) return null;
            string sourceAssemblyName, sourceTypeName, sourcePropertyName;
            Ex.Dotnet.ParseName(item.Value, out sourceAssemblyName, out sourceTypeName, out sourcePropertyName);
            var sourceType = Ex.Dotnet.GetType(sourceAssemblyName, sourceTypeName);
            if (sourceType == null) return null;
            var mapperType = typeof(Mapper<,>);
            mapperType = mapperType.MakeGenericType(targetType, sourceType);
            var targetName = $"{task.AssemblyName},{task.TypeName},{item.Name}";
            return Activator.CreateInstance(mapperType, targetName, item.Value) as ICommandMapper;
        }

        public static CommandStrategy AsCommandStrategy(this Tree tree)
        {
            return new CommandStrategy
            {
                Items = tree.AsCommandAgents()
            };
        }

        public static List<CommandAgent> AsCommandAgents(this Tree tree)
        {
            var agents = new List<CommandAgent>();
            TreeItemsAsCommandAgents(tree.Items, agents);
            return agents;
        }

        public static List<CommandAgent> AsCommandAgents(this IEnumerable<TreeItem> treeItems)
        {
            var agents = new List<CommandAgent>();
            TreeItemsAsCommandAgents(treeItems, agents);
            return agents;
        }

        private static void TreeItemsAsCommandAgents(IEnumerable<TreeItem> treeItems, List<CommandAgent> commandAgents)
        {
            foreach (var treeItem in treeItems)
            {
                //var agent = treeItem.Task.AsCommandAgent();
                //commandAgents.Add(agent);
                //TreeItemsAsCommandAgents(treeItem.Items, agent.Items);
            }
        }
    }
}