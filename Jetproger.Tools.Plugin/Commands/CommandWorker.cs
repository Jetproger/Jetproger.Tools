using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;
using System.Security.Principal;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Resource.Bases;
using TP = Tools.Plugin;
using DI = Tools.DI;
using MD = Tools.Metadata;

namespace Jetproger.Tools.Plugin.Commands
{
    public class CommandWorker
    {
        public static string DomainName => !string.IsNullOrWhiteSpace(_domainName) ? _domainName : AppDomain.CurrentDomain.FriendlyName;
        private static IpcChannel _channel;
        private static Type _channelType;
        private static string _domainName;

        private Command _command;
        private bool _isStopped;
        private string _result;
        private string _error;

        public static string Login
        {
            get; private set;
        }

        public static string Password
        {
            get; private set;
        }

        public bool IsStopped()
        {
            return _isStopped || TP.IsStopped;
        }

        public string GetError()
        {
            return _error ?? TP.Error;
        }

        public string GetResult()
        {
            return _result;
        }

        public string GetSource()
        {
            return TP.Name;
        }

        public AppDomain GetDomain()
        {
            return AppDomain.CurrentDomain;
        }

        public string[] GetMessages()
        {
            return _command?.GetMessages();
        }

        public void Unexecute(CommandRequest request)
        {
            try
            {
                _result = LoadCommand(request)?.Unexecute();
            }
            catch (Exception e)
            {
                _error = e.AsString();
            }
            finally
            {
                _isStopped = true;
            }
        }

        public void Execute(CommandRequest request)
        {
            try
            {
                _result = LoadCommand(request)?.Execute();
            }
            catch (Exception e)
            {
                _error = e.AsString();
            }
            finally
            {
                _isStopped = true;
            }
        }

        public void Enabled(CommandRequest request)
        {
            try
            {
                _result = LoadCommand(request)?.Enabled();
            }
            catch (Exception e)
            {
                _error = e.AsString();
            }
            finally
            {
                _isStopped = true;
            }
        }

        private ICommandIsolate LoadCommand(CommandRequest request)
        {
            try
            {
                CreateChannel();
                if (string.IsNullOrWhiteSpace(_domainName))
                {
                    _domainName = $"{AppDomain.CurrentDomain.FriendlyName}, {request.TypeName}";
                }
                if (!string.IsNullOrWhiteSpace(request.Login))
                {
                    Login = request.Login;
                    Password = request.Password;
                }
                var type = MD.GetType(request.AssemblyName, request.TypeName);
                if (type == null)
                {
                    _error = string.Format(Toolx.Name.MetaCreateType(), request.AssemblyName, request.TypeName);
                    System.Diagnostics.Trace.WriteLine(_error);
                    return null;
                }
                type = DI.TypeOf(type);
                var command = request.Json.As(type);
                var commandIsolate = command as ICommandIsolate;
                if (command != null && commandIsolate == null)
                {
                    _error = string.Format(Toolx.Name.MetaImplementInterface(), request.AssemblyName, request.TypeName, "ICommandIsolate");
                    System.Diagnostics.Trace.WriteLine(_error);
                    return null;
                }
                _command = command as Command;
                if (_command != null && request.IsRemote)
                {
                    System.Diagnostics.Trace.Listeners.Add(_command);
                }
                return commandIsolate;
            }
            catch (Exception e)
            {
                _error = e.AsString();
                System.Diagnostics.Trace.WriteLine(_error);
                return null;
            }
        }

        private static void CreateChannel()
        {
            if (_channel != null) return;
            _channelType = typeof(CommandWorker);
            var type = typeof (CommandWorker);
            var portName = $"{type.FullName.Replace(".", "-").ToLower()}-{Guid.NewGuid()}";
            var objectUri = type.Name.ToLower();
            var client = new BinaryClientFormatterSinkProvider();
            var server = new BinaryServerFormatterSinkProvider { TypeFilterLevel = TypeFilterLevel.Full };
            var config = new Hashtable
            {
                ["name"] = string.Empty,
                ["portName"] = portName,
                ["tokenImpersonationLevel"] = TokenImpersonationLevel.Impersonation,
                ["impersonate"] = true,
                ["useDefaultCredentials"] = true,
                ["secure"] = true,
                ["typeFilterLevel"] = TypeFilterLevel.Full
            };
            _channel = new IpcChannel(config, client, server);
            ChannelServices.RegisterChannel(_channel, true);
            RemotingConfiguration.RegisterWellKnownServiceType(_channelType, objectUri, WellKnownObjectMode.SingleCall);
        }
    }
}