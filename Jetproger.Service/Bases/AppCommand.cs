using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;
using NLog;

namespace Jetproger.Service.Bases
{
    public class AppCommand : MarshalByRefObject
    {
        public void Reset() { _buffer = new byte[4096]; }
        public Stream ResponseReader { get; private set; }
        public Stream ResponseWriter { get; private set; }
        public Stream RequestReader { get; private set; }
        public Stream RequestWriter { get; private set; }
        public Exception Error { get; private set; }
        private Stream _currentReader;
        private Stream _currentWriter;
        private byte[] _buffer;

        public AppCommand(HttpListenerContext context)
        {
            ResponseWriter = context.Response.OutputStream;
            RequestReader = context.Request.InputStream;
            RequestWriter = new MemoryStream();
            Reset();
        }

        public void Execute()
        {
            _currentReader = RequestReader;
            _currentWriter = RequestWriter;
            _currentReader.BeginRead(_buffer, 0, _buffer.Length, TryEndRead, this);
        }

        private void EndRead(IAsyncResult ar, AppCommand state)
        {
            var read = state._currentReader.EndRead(ar);
            if (read > 0)
            {
                state._currentWriter.BeginWrite(state._buffer, 0, read, TryEndWrite, state);
                return;
            }
            if (ReferenceEquals(_currentReader, ResponseReader))
            {
                state.Dispose();
                return;
            }
            //var request = MemToStr((MemoryStream)state.StreamWriter);
            //var response = AppProcess.HttpExecute(request); 
            _currentReader = ResponseReader;
            _currentWriter = ResponseWriter;
            _currentReader.BeginRead(_buffer, 0, _buffer.Length, TryEndRead, this);
        }

        private void EndWrite(IAsyncResult ar, AppCommand state)
        {
            state._currentWriter.EndWrite(ar);
            state.Reset();
            state._currentReader.BeginRead(state._buffer, 0, state._buffer.Length, TryEndRead, state);
        }

        private void TryEndRead(IAsyncResult ar)
        {
            var state = (AppCommand)null;
            try
            {
                state = (AppCommand)ar.AsyncState;
                EndRead(ar, state);
            }
            catch (Exception e)
            {
                if (state != null) state.Dispose();
                f.log.Error(e);
            }
        }

        private void TryEndWrite(IAsyncResult ar)
        {
            var state = (AppCommand)null;
            try
            {
                state = (AppCommand)ar.AsyncState;
                EndWrite(ar, state);
            }
            catch (Exception e)
            {
                if (state != null) state.Dispose();
                f.log.Error(e);
            }
        }

        #region dispose

        public void Dispose()
        {
            DisposeResponseWriter();
            DisposeResponseReader();
            DisposeRequestWriter();
            DisposeRequestReader();
        }

        public void DisposeResponseWriter()
        {
            try
            {
                if (ResponseWriter == null) return;
                ResponseWriter.Dispose();
                ResponseWriter = null;
            }
            catch (Exception e)
            {
                ResponseWriter = null;
                Error = e;
            }
        }

        public void DisposeResponseReader()
        {
            try
            {
                if (ResponseReader == null) return;
                ResponseReader.Dispose();
                ResponseReader = null;
            }
            catch (Exception e)
            {
                ResponseReader = null;
                Error = e;
            }
        }

        public void DisposeRequestWriter()
        {
            try
            {
                if (RequestWriter == null) return;
                RequestWriter.Dispose();
                RequestWriter = null;
            }
            catch (Exception e)
            {
                RequestWriter = null;
                Error = e;
            }
        }

        public void DisposeRequestReader()
        {
            try
            {
                if (RequestReader == null) return;
                RequestReader.Dispose();
                RequestReader = null;
            }
            catch (Exception e)
            {
                RequestReader = null;
                Error = e;
            }
        }

        #endregion
    }
}