using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using Tools;
using MD = Tools.Metadata;

namespace Jetproger.Tools.Plugin.Commands
{
    public class CommandAgent : MarshalByRefObject
    {
        private readonly Dictionary<string, ICommandMapper> _mappers = new Dictionary<string, ICommandMapper>();

        private Command _command;

        public CommandAgent() : this(null) { }

        public CommandAgent(Command command)
        {
            _command = command ?? new CommandDirect();
            Items = new List<CommandAgent>();
        }

        public CommandAgent Parent { get; set; }

        public List<CommandAgent> Items { get; set; }

        public CommandState State => _command.State;

        public Command Command => _command;

        public void Reset()
        {
            if (_command.State != CommandState.New && _command.State != CommandState.Running) _command = (Command)MD.CreateInstance(_command.GetType());
        }

        public void Wait()
        {
            while (_command.State == CommandState.New || _command.State == CommandState.Running) Thread.Sleep(333);
        }

        public void Cancel()
        {
            _command.Cancel();
        }

        public void Map<TTarget, TSource>(Expression<Func<TTarget, object>> target, Expression<Func<TSource, object>> source)
        {
            AddOrUpdateMapper(new Mapper<TTarget, TSource>(target, source));
        }

        public void Map<TTarget>(Expression<Func<TTarget, object>> target)
        {
            AddOrUpdateMapper(new Mapper<TTarget, TTarget>(target, target));
        }

        public void Map<TTarget>(Expression<Func<TTarget, object>> target, object value)
        {
            AddOrUpdateMapper(new Mapper<TTarget>(target, value));
        }

        public void Map(ICommandMapper mapper)
        {
            AddOrUpdateMapper(mapper);
        }

        private void AddOrUpdateMapper(ICommandMapper mapper)
        {
            lock (_mappers)
            {
                if (!_mappers.ContainsKey(mapper.PropertyName)) _mappers.Add(mapper.PropertyName, mapper); else _mappers[mapper.PropertyName] = mapper;
            }
        }

        #region Run

        public void Run()
        {
            try
            {
                if (_command.State == CommandState.Running) return;
                if (IsParentError()) return;
                var proc = new Action(BeginRun);
                proc.BeginInvoke(EndRun, proc);
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e);
            }
        }

        private void BeginRun()
        {
            try
            {
                Queue();
                Parallels();
                _command.Execute();
                foreach (var agent in Items)
                {
                    agent.Run();
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e);
            }
        }

        private void EndRun(IAsyncResult asyncResult)
        {
            try
            {
                ((Action)asyncResult.AsyncState).EndInvoke(asyncResult);
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e);
            }
        }

        private bool IsParentError()
        {
            var agent = Parent;
            while (true)
            {
                if (agent == null) return false;
                if (agent._command?.State == CommandState.Error) return true;
                agent = agent.Parent;
            }
        }

        #endregion

        private void Parallels()
        {
            if (Parent == null) return;
            var merger = _command as IMerger;
            if (merger == null) return;
            var mergerType = typeof(IMerger<>);
            var commandType = _command.GetType();
            var genericType = commandType.GetGenericArgumentType();
            if (genericType == null) return;
            mergerType = mergerType.MakeGenericType(genericType);
            if (!commandType.IsTypeOf(mergerType)) return;
            var sources = new Dictionary<int, Command>();
            var counter = 0;
            foreach (var agent in Parent.Items)
            {
                if (agent == this) continue;
                if (agent._command == null) continue;
                if (agent._command.GetType().IsTypeOf(genericType)) sources.Add(counter++, agent._command);
            }
            while (true)
            {
                var deleted = new List<int>();
                foreach (var source in sources)
                {
                    if (source.Value.State == CommandState.New || source.Value.State == CommandState.Running) continue;
                    if (source.Value.State == CommandState.Successful) merger.Merge(source.Value);
                    deleted.Add(source.Key);
                }
                foreach (var key in deleted)
                {
                    sources.Remove(key);
                }
                if (sources.Count == 0)
                {
                    break;
                }
                Thread.Sleep(333);
            }
        }

        private void Queue()
        {
            var mappers = new List<ICommandMapper>();
            lock (_mappers)
            {
                mappers.AddRange(_mappers.Values);
            }
            var sources = new Dictionary<int, Tuple<ICommandMapper, Command>>();
            var counter = 0;
            foreach (var mapper in mappers)
            {
                if (mapper.SourceType == null)
                {
                    mapper.Map(_command, null);
                    continue;
                }
                var source = FindSource(mapper.SourceType);
                if (source != null) sources.Add(counter++, new Tuple<ICommandMapper, Command>(mapper, source));
            }
            while (true)
            {
                var deleted = new List<int>();
                foreach (var source in sources)
                {
                    if (source.Value.Item2.State == CommandState.New || source.Value.Item2.State == CommandState.Running) continue;
                    if (source.Value.Item2.State == CommandState.Successful) source.Value.Item1.Map(_command, source.Value.Item2);
                    deleted.Add(source.Key);
                }
                foreach (var key in deleted)
                {
                    sources.Remove(key);
                }
                if (sources.Count == 0)
                {
                    break;
                }
                Thread.Sleep(333);
            }
        }

        private Command FindSource(Type sourceType)
        {
            var agent = Parent;
            while (true)
            {
                if (agent == null) return null;
                if (agent._command != null)
                {
                    var type = agent._command.GetType();
                    if (type.IsTypeOf(sourceType)) return agent._command;
                }
                agent = agent.Parent;
            }
        }
    }

    public interface IMerger<in T> : IMerger
    {
        void Merge(T item);
    }

    public interface IMerger
    {
        void Merge(object item);
    }
}