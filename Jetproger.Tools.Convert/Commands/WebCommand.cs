using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Commanders;
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Convert.Commands
{
    #region WebCommand

    public abstract class GetWebCommand<TResult, TValue> : WebCommand<TResult, TValue> { protected GetWebCommand(string url, TValue content) : base(url, "GET", content) { } }
    public abstract class GetWebCommand<TResult> : WebCommand<TResult, byte[]> { protected GetWebCommand(string url) : base(url, "GET", new byte[0]) { } }
    public abstract class GetWebCommand : WebCommand<byte[], byte[]> { protected GetWebCommand(string url) : base(url, "GET", new byte[0]) { } }

    public abstract class PostWebCommand<TResult, TValue> : WebCommand<TResult, TValue> { protected PostWebCommand(string url, TValue content) : base(url, "POST", content) { } }
    public abstract class PostWebCommand<TResult> : WebCommand<TResult, byte[]> { protected PostWebCommand(string url) : base(url, "POST", new byte[0]) { } }
    public abstract class PostWebCommand : WebCommand<byte[], byte[]> { protected PostWebCommand(string url) : base(url, "POST", new byte[0]) { } }

    public abstract class HeadWebCommand<TResult, TValue> : WebCommand<TResult, TValue> { protected HeadWebCommand(string url, TValue content) : base(url, "HEAD", content) { } }
    public abstract class HeadWebCommand<TResult> : WebCommand<TResult, byte[]> { protected HeadWebCommand(string url) : base(url, "HEAD", new byte[0]) { } }
    public abstract class HeadWebCommand : WebCommand<byte[], byte[]> { protected HeadWebCommand(string url) : base(url, "HEAD", new byte[0]) { } }

    public abstract class OptionsWebCommand<TResult, TValue> : WebCommand<TResult, TValue> { protected OptionsWebCommand(string url, TValue content) : base(url, "OPTIONS", content) { } }
    public abstract class OptionsWebCommand<TResult> : WebCommand<TResult, byte[]> { protected OptionsWebCommand(string url) : base(url, "OPTIONS", new byte[0]) { } }
    public abstract class OptionsWebCommand : WebCommand<byte[], byte[]> { protected OptionsWebCommand(string url) : base(url, "OPTIONS", new byte[0]) { } }

    public abstract class PutWebCommand<TResult, TValue> : WebCommand<TResult, TValue> { protected PutWebCommand(string url, TValue content) : base(url, "PUT", content) { } }
    public abstract class PutWebCommand<TResult> : WebCommand<TResult, byte[]> { protected PutWebCommand(string url) : base(url, "PUT", new byte[0]) { } }
    public abstract class PutWebCommand : WebCommand<byte[], byte[]> { protected PutWebCommand(string url) : base(url, "PUT", new byte[0]) { } }

    public abstract class PatchWebCommand<TResult, TValue> : WebCommand<TResult, TValue> { protected PatchWebCommand(string url, TValue content) : base(url, "PATCH", content) { } }
    public abstract class PatchWebCommand<TResult> : WebCommand<TResult, byte[]> { protected PatchWebCommand(string url) : base(url, "PATCH", new byte[0]) { } }
    public abstract class PatchWebCommand : WebCommand<byte[], byte[]> { protected PatchWebCommand(string url) : base(url, "PATCH", new byte[0]) { } }

    public abstract class DeleteWebCommand<TResult, TValue> : WebCommand<TResult, TValue> { protected DeleteWebCommand(string url, TValue content) : base(url, "DELETE", content) { } }
    public abstract class DeleteWebCommand<TResult> : WebCommand<TResult, byte[]> { protected DeleteWebCommand(string url) : base(url, "DELETE", new byte[0]) { } }
    public abstract class DeleteWebCommand : WebCommand<byte[], byte[]> { protected DeleteWebCommand(string url) : base(url, "DELETE", new byte[0]) { } }

    public abstract class TraceWebCommand<TResult, TValue> : WebCommand<TResult, TValue> { protected TraceWebCommand(string url, TValue content) : base(url, "TRACE", content) { } }
    public abstract class TraceWebCommand<TResult> : WebCommand<TResult, byte[]> { protected TraceWebCommand(string url) : base(url, "TRACE", new byte[0]) { } }
    public abstract class TraceWebCommand : WebCommand<byte[], byte[]> { protected TraceWebCommand(string url) : base(url, "TRACE", new byte[0]) { } }

    public abstract class ConnectWebCommand<TResult, TValue> : WebCommand<TResult, TValue> { protected ConnectWebCommand(string url, TValue content) : base(url, "CONNECT", content) { } }
    public abstract class ConnectWebCommand<TResult> : WebCommand<TResult, byte[]> { protected ConnectWebCommand(string url) : base(url, "CONNECT", new byte[0]) { } }
    public abstract class ConnectWebCommand : WebCommand<byte[], byte[]> { protected ConnectWebCommand(string url) : base(url, "CONNECT", new byte[0]) { } }

    #endregion

    public abstract class WebCommand<TResult, TValue> : Command<TResult, TValue>
    {
        private readonly List<HttpRequestHeaderValue> _httpRequestHeaders = new List<HttpRequestHeaderValue>();
        public string NetworkCredentialPassword { get; set; }
        public string NetworkCredentialUser { get; set; }
        public X509Certificate2 Certificate { get; set; }
        public string Method { get; private set; }
        public string Url { get; private set; }
        public int Status { get; private set; }

        protected WebCommand(string url, string method, TValue content)
        {
            Value = content;
            Method = method;
            Url = url;
        }

        protected override void Execute()
        {
            __BeginGetRequestStream();
        }

        private WebStreams BeginGetRequestStream()
        {
            var state = new WebStreams();
            state.Request = CreateRequest();
            state.StreamReader = Value.As<MemoryStream>(f.web.WebEncoding);
            if (state.StreamReader.Length == 0)
            {
                BeginGetResponse(state);
                return state;
            }
            state.Request.ContentLength = state.StreamReader.Length;
            state.Request.BeginGetRequestStream(__EndGetRequestStream, state);
            return state;
        }

        private void EndGetRequestStream(IAsyncResult ar, WebStreams state)
        {
            state.StreamWriter = state.Request.EndGetRequestStream(ar);
            state.StreamReader.BeginRead(state.Buffer, 0, state.Buffer.Length, __EndReadRequestMemory, state);
        }

        private void EndReadRequestMemory(IAsyncResult ar, WebStreams state)
        {
            var read = state.StreamReader.EndRead(ar);
            if (read > 0) state.StreamWriter.BeginWrite(state.Buffer, 0, read, __EndWriteRequestStream, state);
            else BeginGetResponse(state);
        }

        private void EndWriteRequestStream(IAsyncResult ar, WebStreams state)
        {
            state.StreamWriter.EndWrite(ar);
            state.Reset();
            state.StreamReader.BeginRead(state.Buffer, 0, state.Buffer.Length, __EndReadRequestMemory, state);
        }

        private void BeginGetResponse(WebStreams requestState)
        {
            requestState.Dispose();
            var state = new WebStreams { StreamWriter = new MemoryStream(), Request = requestState.Request };
            var ar = state.Request.BeginGetResponse(__EndGetResponse, state);
            ThreadPool.RegisterWaitForSingleObject(ar.AsyncWaitHandle, f.web.TimeoutCallback, state.Request, f.web.RequestTimeout, true);
        }

        private void EndGetResponse(IAsyncResult ar, WebStreams state)
        {
            state.Response = (HttpWebResponse)state.Request.EndGetResponse(ar);
            Status = (int)state.Response.StatusCode;
            state.StreamReader = state.Response.GetResponseStream();
            if (state.StreamReader != null) state.StreamReader.BeginRead(state.Buffer, 0, state.Buffer.Length, __EndReadResponse, state);
        }

        private void EndReadResponse(IAsyncResult ar, WebStreams state)
        {
            var read = state.StreamReader.EndRead(ar);
            if (read > 0) state.StreamWriter.BeginWrite(state.Buffer, 0, read, __EndWriteMemory, state);
            else CommandThreads.Run(() => Completing(state));
        }

        private void EndWriteMemory(IAsyncResult ar, WebStreams state)
        {
            state.StreamWriter.EndWrite(ar);
            state.Reset();
            state.StreamReader.BeginRead(state.Buffer, 0, state.Buffer.Length, __EndReadResponse, state);
        }

        private void Completing(WebStreams state)
        {
            try
            {
                var ms = state.StreamWriter as MemoryStream;
                var result = ms != null ? ms.As<TResult>(f.web.WebEncoding) : default(TResult);
                state.Dispose();
                Result = result;
            }
            catch (Exception e)
            {
                state.Dispose();
                Error = ExceptioAsCommandException(e);
            }
        }

        private HttpWebRequest CreateRequest()
        {
            var request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = Method;
            request.ProtocolVersion = HttpVersion.Version10;
            request.Credentials = CredentialCache.DefaultCredentials;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            request.AllowWriteStreamBuffering = false;
            if (IsSecurityProtocol()) request.ClientCertificates.Add(Certificate ?? f.cry.Out);
            AddHeaders(request);
            if (NetworkCredentialUser != null && NetworkCredentialPassword != null)
            {
                request.PreAuthenticate = true;
                request.Credentials = new NetworkCredential(NetworkCredentialUser, NetworkCredentialPassword);
            }
            request.Proxy = f.web.GetProxy(Url);
            return request;
        }

        public bool IsSecurityProtocol()
        {
            return (Url ?? string.Empty).StartsWith("https");
        }

        public void AddHeader(HttpRequestHeader header, string value)
        {
            _httpRequestHeaders.Add(new HttpRequestHeaderValue(header, value));
        }

        public void AddUrl(string partUrl)
        {
            Url += partUrl;
        }

        private void AddHeaders(WebRequest request)
        {
            foreach (var item in _httpRequestHeaders)
            {
                switch (item.Header) {
                    case HttpRequestHeader.ContentLength: request.ContentLength = item.Value.As<long>(); break;
                    case HttpRequestHeader.Connection: request.ConnectionGroupName = item.Value; break;
                    case HttpRequestHeader.ContentType: request.ContentType = item.Value; break;
                    case HttpRequestHeader.Accept: ((HttpWebRequest)request).Accept = item.Value; break;
                    default: request.Headers.Add(item.Header, item.Value); break;
                }
            }
        }

        #region Exceptions

        private void __BeginGetRequestStream()
        {
            var state = (WebStreams)null;
            try
            {
                state = BeginGetRequestStream();
            }
            catch (Exception e)
            {
                if (state != null) state.Dispose();
                Error = ExceptioAsCommandException(e);
            }
        }

        private void __EndGetRequestStream(IAsyncResult ar)
        {
            var state = (WebStreams)null;
            try
            {
                state = (WebStreams)ar.AsyncState;
                EndGetRequestStream(ar, state);
            }
            catch (Exception e)
            {
                if (state != null) state.Dispose();
                Error = ExceptioAsCommandException(e);
            }
        }

        private void __EndReadRequestMemory(IAsyncResult ar)
        {
            var state = (WebStreams)null;
            try
            {
                state = (WebStreams)ar.AsyncState;
                EndReadRequestMemory(ar, state);
            }
            catch (Exception e)
            {
                if (state != null) state.Dispose();
                Error = ExceptioAsCommandException(e);
            }
        }

        private void __EndWriteRequestStream(IAsyncResult ar)
        {
            var state = (WebStreams)null;
            try
            {
                state = (WebStreams)ar.AsyncState;
                EndWriteRequestStream(ar, state);
            }
            catch (Exception e)
            {
                if (state != null) state.Dispose();
                Error = ExceptioAsCommandException(e);
            }
        }

        private void __EndGetResponse(IAsyncResult ar)
        {
            var state = (WebStreams)null;
            try
            {
                state = (WebStreams)ar.AsyncState;
                EndGetResponse(ar, state);
            }
            catch (Exception e)
            {
                if (state != null) state.Dispose();
                Error = ExceptioAsCommandException(e);
            }
        }

        private void __EndReadResponse(IAsyncResult ar)
        {
            var state = (WebStreams)null;
            try
            {
                state = (WebStreams)ar.AsyncState;
                EndReadResponse(ar, state);
            }
            catch (Exception e)
            {
                if (state != null) state.Dispose();
                Error = ExceptioAsCommandException(e);
            }
        }

        private void __EndWriteMemory(IAsyncResult ar)
        {
            var state = (WebStreams)null;
            try
            {
                state = (WebStreams)ar.AsyncState;
                EndWriteMemory(ar, state);
            }
            catch (Exception e)
            {
                if (state != null) state.Dispose();
                Error = ExceptioAsCommandException(e);
            }
        }

        #endregion 

        private CommandException ExceptioAsCommandException(Exception e)
        {
            var ce = new CommandException(e);
            Status = ce.Status;
            return ce;
        }

        private class HttpRequestHeaderValue
        {
            public readonly HttpRequestHeader Header;
            public readonly string Value;
            public HttpRequestHeaderValue(HttpRequestHeader header, string value)
            {
                Header = header;
                Value = value;
            }
        }
    }
}