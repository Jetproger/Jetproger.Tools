using System; 
using System.Collections.Generic;
using System.Threading;
using Jetproger.Tools.Convert.Bases; 
using Jetproger.Tools.Convert.Commands;
using Jetproger.Tools.Convert.Converts;
using Jetproger.Tools.Convert.Factories;

namespace Jetproger.Tools.Convert.Commanders
{ 
    public class CmdExpander
    {
        
        public readonly string SYS_ICON = @"AAABAAEAEBAAAAAAGABoAwAAFgAAACgAAAAQAAAAIAAAAAEAGAAAAAAAAAMAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACyYzmARyl9RSh7RCd6RCd3QiZyQCVvPiRsPCNpOyJmOSFjNyBhNh9gNR8AAAAAAAC4ZjvYoIPYoIPQjmvHeE+4ZjueVzKGSit4QiZ3QiZ4QiZ3QiZbMh1hNh8AAAAAAAC/aj3oybjiuaTbqY7TlXTJfVW+aj2eVzKDSSp4QiZsPCOMTi17RCdjNyAAAAAAAADEcUXht6Hw29DiuaTZo4fQj2zHeE62ZTqSUS9jNyDCbD67aDwAAABmOSEAAAAAAADHeE/Mg13iuaTw29DiuaTYoIPOiWTDb0KSUS/Zo4fSknEAAADOiGNpOyIAAAAAAADLgVrz4djKf1fiuaPw2s/ht6HUl3egWTPhtqDjvKcAAADiuaPQjmtsPCMAAAAAAADOimb+/fzz4djFc0jiuaPlwK2HSyvbqY59RSh9RSjdrZTnxrXTlXRvPiQAAAAAAADRkW/+/fz+/fzw29DFc0jFc0ju1snz49rw29Dt1Mfrz8HpyrrWnH1yQCUAAAAAAADUmHj+/fz+/fz+/fz9+vj68/D47+rnxLKyYzmtYDfhtqDrz8HZo4Z3QiYAAAAAAADXn4H+/fz+/fz+/fz+/fz9+vj68/DDb0LbqY/FdEmtYDft1Mfcq5F6RCcAAAAAAADZo4b+/fz+/fz+/fz+/fz+/fz9+vjEcUXu1srbqY+yYznw29Dfspp7RCcAAAAAAADZo4b+/fz+/fz+/fz+/fz+/fz+/fzw29DEcUXDb0LnxLLz49rhtqB9RSgAAAAAAADZo4b+/fz+/fz+/fz+/fz+/fz+/fz+/fz9+vj68/D47+r16OHz49qARykAAAAAAADZo4bZo4bZo4bZo4bXn4HUmHjRkW/OimbLgVrHeE/EcUW/aj24ZjuyYzkAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//wAAgAEAAIABAACAAQAAgAEAAIABAACAAQAAgAEAAIABAACAAQAAgAEAAIABAACAAQAAgAEAAIABAAD//wAA";
        public readonly string SYS_IMAGE = @"iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAIDSURBVDhPpZLrS5NhGMb3j4SWh0oRQVExD4gonkDpg4hGYKxG6WBogkMZKgPNCEVJFBGdGETEvgwyO9DJE5syZw3PIlPEE9pgBCLZ5XvdMB8Ew8gXbl54nuf63dd90OGSnwCahxbPRNPAPMw9Xpg6ZmF46kZZ0xSKzJPIrhpDWsVnpBhGkKx3nAX8Pv7z1zg8OoY/cITdn4fwbf/C0kYAN3Ma/w3gWfZL5kzTKBxjWyK2DftwI9tyMYCZKXbNHaD91bLYJrDXsYbrWfUKwJrPE9M2M1OcVzOOpHI7Jr376Hi9ogHqFIANO0/MmmmbmSmm9a8ze+I4MrNWAdjtoJgWcx+PSzg166yZZ8xM8XvXDix9c4jIqFYAjoriBV9AhEPv1mH/sonogha0afbZMMZz+yreTGyhpusHwtNNCsA5U1zS4BLxzJIfg299qO32Ir7UJtZfftyATqeT+8o2D8JSjQrAJblrncYL7ZJ2+bfaFnC/1S1NjL3diRat7qrO7wLRP3HjWsojBeComDEo5mNjuweFGvjWg2EBhCbpkW78htSHHwRyNdmgAFzPEee2iFkzayy2OLXzT4gr6UdUnlXrullsxxQ+kx0g8BTA3aZlButjSTyjODq/WcQcW/B/Je4OQhLvKQDnzN1mp0nnkvAhR8VuMzNrpm1mpjgkoVwB/v8DTgDQASA1MVpwzwAAAABJRU5ErkJggg==";
        
        public readonly string SYS_APPDOMAIN = "APPDOMAIN";
        public readonly string SYS_CMDDOMAIN = "CMDDOMAIN";

        public AsyncCommand asyncof(EmptyCommand command) { return new AsyncCommand(command); }
        public AsyncCommand<T> asyncof<T>(EmptyCommand<T> command) { return new AsyncCommand<T>(command); }
        public AsyncCommand<T, T_> asyncof<T, T_>(EmptyCommand<T, T_> command) { return new AsyncCommand<T, T_>(command); }
        public AsyncCommand<T, T_, T0> asyncof<T, T_, T0>(EmptyCommand<T, T_, T0> command) { return new AsyncCommand<T, T_, T0>(command); }
        public AsyncCommand<T, T_, T0, T1> asyncof<T, T_, T0, T1>(EmptyCommand<T, T_, T0, T1> command) { return new AsyncCommand<T, T_, T0, T1>(command); }
        public AsyncCommand<T, T_, T0, T1, T2> asyncof<T, T_, T0, T1, T2>(EmptyCommand<T, T_, T0, T1, T2> command) { return new AsyncCommand<T, T_, T0, T1, T2>(command); }
        public AsyncCommand<T, T_, T0, T1, T2, T3> asyncof<T, T_, T0, T1, T2, T3>(EmptyCommand<T, T_, T0, T1, T2, T3> command) { return new AsyncCommand<T, T_, T0, T1, T2, T3>(command); }
        public AsyncCommand<T, T_, T0, T1, T2, T3, T4> asyncof<T, T_, T0, T1, T2, T3, T4>(EmptyCommand<T, T_, T0, T1, T2, T3, T4> command) { return new AsyncCommand<T, T_, T0, T1, T2, T3, T4>(command); }
        public AsyncCommand<T, T_, T0, T1, T2, T3, T4, T5> asyncof<T, T_, T0, T1, T2, T3, T4, T5>(EmptyCommand<T, T_, T0, T1, T2, T3, T4, T5> command) { return new AsyncCommand<T, T_, T0, T1, T2, T3, T4, T5>(command); }
        public AsyncCommand<T, T_, T0, T1, T2, T3, T4, T5, T6> asyncof<T, T_, T0, T1, T2, T3, T4, T5, T6>(EmptyCommand<T, T_, T0, T1, T2, T3, T4, T5, T6> command) { return new AsyncCommand<T, T_, T0, T1, T2, T3, T4, T5, T6>(command); }
        public AsyncCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7> asyncof<T, T_, T0, T1, T2, T3, T4, T5, T6, T7>(EmptyCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7> command) { return new AsyncCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7>(command); }
        public AsyncCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, T8> asyncof<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, T8>(EmptyCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, T8> command) { return new AsyncCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, T8>(command); }
        public AsyncCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> asyncof<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(EmptyCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> command) { return new AsyncCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(command); }

        public void wait(ICommand command)
        {
            var mre = new ManualResetEvent(false);
            var delayedCommand = command as IDelay;
            if (delayedCommand == null)
            {
                command.Execute();
                return;
            }
            delayedCommand.CancelDelay = true;
            command.Executed += () => mre.Set();
            command.Execute();
            mre.WaitOne();
        }

        public CommandRecord paramsof(IParameterizedCommand parameterizedCommand)
        {
            return parameterizedCommand == null ? null : new CommandRecord
            {
                Name = parameterizedCommand.GetType().FullName,
                P_ = parameterizedCommand.P_ != null && f.sys.issimple(parameterizedCommand.P_.GetType()) ? parameterizedCommand.P_.As<string>() : string.Empty,
                P0 = parameterizedCommand.P0 != null && f.sys.issimple(parameterizedCommand.P0.GetType()) ? parameterizedCommand.P0.As<string>() : string.Empty,
                P1 = parameterizedCommand.P1 != null && f.sys.issimple(parameterizedCommand.P1.GetType()) ? parameterizedCommand.P1.As<string>() : string.Empty,
                P2 = parameterizedCommand.P2 != null && f.sys.issimple(parameterizedCommand.P2.GetType()) ? parameterizedCommand.P2.As<string>() : string.Empty,
                P3 = parameterizedCommand.P3 != null && f.sys.issimple(parameterizedCommand.P3.GetType()) ? parameterizedCommand.P3.As<string>() : string.Empty,
                P4 = parameterizedCommand.P4 != null && f.sys.issimple(parameterizedCommand.P4.GetType()) ? parameterizedCommand.P4.As<string>() : string.Empty,
                P5 = parameterizedCommand.P5 != null && f.sys.issimple(parameterizedCommand.P5.GetType()) ? parameterizedCommand.P5.As<string>() : string.Empty,
                P6 = parameterizedCommand.P6 != null && f.sys.issimple(parameterizedCommand.P6.GetType()) ? parameterizedCommand.P6.As<string>() : string.Empty,
                P7 = parameterizedCommand.P7 != null && f.sys.issimple(parameterizedCommand.P7.GetType()) ? parameterizedCommand.P7.As<string>() : string.Empty,
                P8 = parameterizedCommand.P8 != null && f.sys.issimple(parameterizedCommand.P8.GetType()) ? parameterizedCommand.P8.As<string>() : string.Empty,
                P9 = parameterizedCommand.P9 != null && f.sys.issimple(parameterizedCommand.P9.GetType()) ? parameterizedCommand.P9.As<string>() : string.Empty,
            };
        }

        public CommandRecord xparamsof(IParameterizedCommand parameterizedCommand)
        {
            return parameterizedCommand == null ? null : new CommandRecord
            {
                Name = parameterizedCommand.GetType().FullName,
                P_ = parameterizedCommand.P_ == null || parameterizedCommand.P_.GetType() == typeof(object) ? null : (f.sys.issimple(parameterizedCommand.P_.GetType()) ? parameterizedCommand.P_.As<string>() : parameterizedCommand.P_.As<SimpleXml>().As<string>()),
                P0 = parameterizedCommand.P0 == null || parameterizedCommand.P0.GetType() == typeof(object) ? null : (f.sys.issimple(parameterizedCommand.P0.GetType()) ? parameterizedCommand.P0.As<string>() : parameterizedCommand.P0.As<SimpleXml>().As<string>()),
                P1 = parameterizedCommand.P1 == null || parameterizedCommand.P1.GetType() == typeof(object) ? null : (f.sys.issimple(parameterizedCommand.P1.GetType()) ? parameterizedCommand.P1.As<string>() : parameterizedCommand.P1.As<SimpleXml>().As<string>()),
                P2 = parameterizedCommand.P2 == null || parameterizedCommand.P2.GetType() == typeof(object) ? null : (f.sys.issimple(parameterizedCommand.P2.GetType()) ? parameterizedCommand.P2.As<string>() : parameterizedCommand.P2.As<SimpleXml>().As<string>()),
                P3 = parameterizedCommand.P3 == null || parameterizedCommand.P3.GetType() == typeof(object) ? null : (f.sys.issimple(parameterizedCommand.P3.GetType()) ? parameterizedCommand.P3.As<string>() : parameterizedCommand.P3.As<SimpleXml>().As<string>()),
                P4 = parameterizedCommand.P4 == null || parameterizedCommand.P4.GetType() == typeof(object) ? null : (f.sys.issimple(parameterizedCommand.P4.GetType()) ? parameterizedCommand.P4.As<string>() : parameterizedCommand.P4.As<SimpleXml>().As<string>()),
                P5 = parameterizedCommand.P5 == null || parameterizedCommand.P5.GetType() == typeof(object) ? null : (f.sys.issimple(parameterizedCommand.P5.GetType()) ? parameterizedCommand.P5.As<string>() : parameterizedCommand.P5.As<SimpleXml>().As<string>()),
                P6 = parameterizedCommand.P6 == null || parameterizedCommand.P6.GetType() == typeof(object) ? null : (f.sys.issimple(parameterizedCommand.P6.GetType()) ? parameterizedCommand.P6.As<string>() : parameterizedCommand.P6.As<SimpleXml>().As<string>()),
                P7 = parameterizedCommand.P7 == null || parameterizedCommand.P7.GetType() == typeof(object) ? null : (f.sys.issimple(parameterizedCommand.P7.GetType()) ? parameterizedCommand.P7.As<string>() : parameterizedCommand.P7.As<SimpleXml>().As<string>()),
                P8 = parameterizedCommand.P8 == null || parameterizedCommand.P8.GetType() == typeof(object) ? null : (f.sys.issimple(parameterizedCommand.P8.GetType()) ? parameterizedCommand.P8.As<string>() : parameterizedCommand.P8.As<SimpleXml>().As<string>()),
                P9 = parameterizedCommand.P9 == null || parameterizedCommand.P9.GetType() == typeof(object) ? null : (f.sys.issimple(parameterizedCommand.P9.GetType()) ? parameterizedCommand.P9.As<string>() : parameterizedCommand.P9.As<SimpleXml>().As<string>()),
            };
        }

        #region history

        public ICommand lhistoryof(ICommand command)
        {
            return command.History;
        }

        public ICommand fhistoryof(ICommand command)
        {
            for (var history = command.History; ;)
            {
                if (history == null) return null;
                if (history.History == null) return history;
                history = history.History;
            }
        }

        public ICommand ihistoryof(ICommand command, int i)
        {
            var commands = new List<ICommand>();
            for (var history = command.History; ;)
            {
                if (history == null) break;
                commands.Add(history);
                history = history.History;
            }
            i = i >= 0 ? i : commands.Count + i;
            return i >= 0 && i < commands.Count ? commands[i] : null;
        }

        public T chistoryof<T>(ICommand command) where T : class, ICommand
        {
            for (var history = command.History; ;)
            {
                if (history == null) return null;
                var cmd = history as T;
                if (cmd != null) return cmd;
                history = history.History;
            }
        }

        public ICommand vhistoryof<T>(ICommand command)
        {
            for (var history = command.History; ;)
            {
                if (history == null) return null;
                if (history.Value is T) return history;
                history = history.History;
            }
        }

        public ICommand rhistoryof<T>(ICommand command)
        {
            for (var history = command.History; ;)
            {
                if (history == null) return null;
                if (history.Result is T) return history;
                history = history.History;
            }
        }
        
        #endregion
    }
}