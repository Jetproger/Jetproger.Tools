using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using Jetproger.Tools.Cache.Bases;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Process.Commands
{
    public class CommandAgent : MarshalByRefObject
    {
        private readonly Dictionary<string, ICommandMapper> _mappers = new Dictionary<string, ICommandMapper>();
        private CommandAgent _parent;
        private Command _command;

        public CommandAgent() : this(null) { }

        public CommandAgent(Command command)
        {
            _command = command ?? new CommandDirect();
            Items = new List<CommandAgent>();
        }

        public CommandAgent Parent
        {
            get { return _parent; }
            set { SetParent(value); }
        }

        public List<CommandAgent> Items { get; set; }

        public CommandState State => _command.State;

        public Command Command => _command;

        public void Reset()
        {
            if (_command.State != CommandState.Wait && _command.State != CommandState.Work) _command = (Command)Je.meta.CreateInstance(_command.GetType());
        }

        public CommandAgent Copy()
        {
            var agent = new CommandAgent(_command);
            lock (_mappers)
            {
                foreach (var pair in _mappers)
                {
                    agent._mappers.Add(pair.Key, pair.Value);
                }
            }
            return agent;
        }

        private void SetParent(CommandAgent parentAgent)
        {
            if (ReferenceEquals(parentAgent, this)) return;
            var oldIndex = _parent.IndexOf(this);
            if (ReferenceEquals(parentAgent, _parent) && oldIndex == _parent.Items.Count - 1) return;
            _parent.Items.RemoveAt(oldIndex);
            _parent = parentAgent;
            _parent.Items.Add(this);
        }

        public void Remove(CommandAgent agent)
        {
            var i = IndexOf(agent);
            if (i >= 0) Items.RemoveAt(i);
        }

        public int IndexOf(CommandAgent agent)
        {
            var i = 0;
            foreach (var commandAgent in Items)
            {
                if (ReferenceEquals(commandAgent, agent)) return i;
                i++;
            }
            return -1;
        }

        public void Wait()
        {
            while (_command.State == CommandState.Wait || _command.State == CommandState.Work) Thread.Sleep(333);
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
                if (_command.State == CommandState.Work) return;
                if (IsParentError())
                {
                    _command.Cancel();
                    return;
                }
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
                if (agent._command?.State == CommandState.Fail) return true;
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
            var genericType = Je.meta.GenericOf(commandType);
            if (genericType == null) return;
            mergerType = mergerType.MakeGenericType(genericType);
            if (!Je.meta.IsTypeOf(commandType, mergerType)) return;
            var sources = new Dictionary<int, Command>();
            var counter = 0;
            foreach (var agent in Parent.Items)
            {
                if (agent == this) continue;
                if (agent._command == null) continue;
                if (Je.meta.IsTypeOf(agent._command.GetType(), genericType)) sources.Add(counter++, agent._command);
            }
            while (true)
            {
                var deleted = new List<int>();
                foreach (var source in sources)
                {
                    if (source.Value.State == CommandState.Wait || source.Value.State == CommandState.Work) continue;
                    if (source.Value.State == CommandState.Ok) merger.Merge(source.Value);
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
                    if (source.Value.Item2.State == CommandState.Wait || source.Value.Item2.State == CommandState.Work) continue;
                    if (source.Value.Item2.State == CommandState.Ok) source.Value.Item1.Map(_command, source.Value.Item2);
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
                    if (Je.meta.IsTypeOf(type, sourceType)) return agent._command;
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