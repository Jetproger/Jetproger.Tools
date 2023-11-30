using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.ServiceProcess; 
using System.Text;

namespace Jetproger.Service.Bases
{
    public class AppService : ServiceBase
    {
        protected override void OnStop() { _server = null; }  
        public void Start(string[] args) { OnStart(args); }
        public void Start() { OnStart(new string[0]); } 
        private HttpListener _server;

        public AppService()
        {
            var executingAssemblyName = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
            ServiceName = executingAssemblyName.ToLower().Replace(".", "-"); 
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                if (_server != null) return;
                _server = new HttpListener();
                AppUpdater.StartUpdater();
                AppMethods.Instance.BaseExecute();
                if (f.isconfig("AppPort"))
                {
                    _server = new HttpListener();
                    _server.Prefixes.Add($"http://127.0.0.1:{f.configof("AppPort")}/kiz/v1/cmd/");
                    _server.Start();
                    _server.BeginGetContext(EndGetContext, null);
                }
            }
            catch (Exception ex)
            {
                f.log.ErrorException("Error start service", ex);
            }
        }

        private void EndGetContext(IAsyncResult ar)
        {
            var context = _server.EndGetContext(ar);
            context.Response.ContentType = "text/plain";
            _server.BeginGetContext(EndGetContext, null);
            var state = new _State { IsResponse = false, Context = context, StreamReader = context.Request.InputStream, StreamWriter = new MemoryStream() };
            state.StreamReader.BeginRead(state.Buffer, 0, state.Buffer.Length, __EndRead, state);
        }

        private void EndRead(IAsyncResult ar, _State state)
        {
            var read = state.StreamReader.EndRead(ar);
            if (read > 0)
            {
                state.StreamWriter.BeginWrite(state.Buffer, 0, read, __EndWrite, state);
                return;
            }
            if (state.IsResponse)
            {
                state.Dispose();
                return;
            }
            var request = MemToStr((MemoryStream)state.StreamWriter);
            var response = AppMethods.Instance.HttpExecute(request);
            var newState = new _State { IsResponse = true, Context = state.Context, StreamReader = StrToMem(response), StreamWriter = state.Context.Response.OutputStream };
            state.Dispose();
            newState.StreamReader.BeginRead(newState.Buffer, 0, newState.Buffer.Length, __EndRead, newState);
        }

        private void EndWrite(IAsyncResult ar, _State state)
        {
            state.StreamWriter.EndWrite(ar);
            state.Reset();
            state.StreamReader.BeginRead(state.Buffer, 0, state.Buffer.Length, __EndRead, state);
        }

        private string MemToStr(MemoryStream ms)
        {
            var bytes = new byte[ms.Length];
            ms.Seek(0, SeekOrigin.Begin);
            ms.Read(bytes, 0, bytes.Length);
            return Encoding.UTF8.GetString(bytes);
        }

        private MemoryStream StrToMem(string s)
        {
            var utf16 = Encoding.GetEncoding("utf-16");
            var bytes = utf16.GetBytes(s);
            bytes = Encoding.Convert(utf16, Encoding.UTF8, bytes);
            var ms = new MemoryStream(bytes.Length);
            ms.Write(bytes, 0, bytes.Length);
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        #region exceptions

        private void __EndRead(IAsyncResult ar)
        {
            var state = (_State)null;
            try
            {
                state = (_State)ar.AsyncState;
                EndRead(ar, state);
            }
            catch (Exception e)
            {
                if (state != null) state.Dispose();
                f.log.Error(e);
            }
        }

        private void __EndWrite(IAsyncResult ar)
        {
            var state = (_State)null;
            try
            {
                state = (_State)ar.AsyncState;
                EndWrite(ar, state);
            }
            catch (Exception e)
            {
                if (state != null) state.Dispose();
                f.log.Error(e);
            }
        }

        #endregion

        private class _State
        {
            public void Reset() { Buffer = new byte[4096]; }
            public _State() { Reset(); }
            public HttpListenerContext Context;
            public Stream StreamReader;
            public Stream StreamWriter;
            public Exception Error;
            public bool IsResponse;
            public byte[] Buffer;

            public virtual void Dispose()
            {
                DisposeWriter();
                DisposeReader();
            }

            public void DisposeWriter()
            {
                try
                {
                    if (StreamWriter == null) return;
                    StreamWriter.Dispose();
                    StreamWriter = null;
                }
                catch (Exception e)
                {
                    Error = e;
                }
            }

            public void DisposeReader()
            {
                try
                {
                    if (StreamReader == null) return;
                    StreamReader.Dispose();
                    StreamReader = null;
                }
                catch (Exception e)
                {
                    Error = e;
                }
            }
        }
    }
}