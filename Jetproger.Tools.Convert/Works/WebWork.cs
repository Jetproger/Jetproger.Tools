using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Xml;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Convert.Works
{
    public class WebWork : WebWork<object>
    { 
        public WebWork(string url) : base(url)
        { 
        }

        public static new WebWork Op(string url)
        {
            return new WebWork(url);
        }
    }

    public class WebWork<TResult> : Work<TResult>
    {
        private readonly List<Tuple<HttpRequestHeader, string>> _httpRequestHeaders = new List<Tuple<HttpRequestHeader, string>>();
        private static readonly int RequestTimeout = Je<HttpConnectionTimeoutSeconds>.As.As<int>() * 1000;
        private string _networkCredentialPassword;
        private string _networkCredentialUser;
        private X509Certificate2 _certificate;
        protected readonly Type ResultType;
        private Stream _targetStream;
        private byte[] _contentBytes;
        private WebState _webState;
        private string _content;
        private string _method;
        private string _url;

        public WebWork(string url)
        {
            ResultType = typeof(TResult);
            Url(url);
        }

        protected override void Disposing()
        {
            Je.err.TryLess(() => (_webState != null).If(() => _webState.Dispose()));
            Je.err.TryLess(() => _webState = null);
        }

        public static WebWork<TResult> Op(string url)
        {
            return new WebWork<TResult>(url);
        }

        public bool IsSecurityProtocol()
        {
            return (_url ?? string.Empty).StartsWith("https");
        }

        public WebWork<TResult> Header(HttpRequestHeader header, string value)
        {
            _httpRequestHeaders.Add(new Tuple<HttpRequestHeader, string>(header, value));
            return this;
        }

        public WebWork<TResult> NetworkCredential(string userName, string password)
        {
            _networkCredentialPassword = password;
            _networkCredentialUser = userName;
            return this;
        }

        public WebWork<TResult> Content(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                Content(new byte[0]);
                return this;
            }
            var bytes = obj as byte[];
            if (bytes != null)
            {
                Content(bytes);
                return this;
            }
            var str = obj as string;
            if (str != null)
            {
                Content(str);
                return this;
            }
            var doc = obj as XmlDocument;
            if (doc != null)
            {
                Content(doc.InnerXml);
                return this;
            }
            System.Web.UI.WebControls.Content(Je.web.To(obj));
            return this;
        }

        public WebWork<TResult> Content(string content)
        {
            _content = content;
            return this;
        }

        public WebWork<TResult> Content(byte[] content)
        {
            _contentBytes = content;
            return this;
        }

        public WebWork<TResult> Certificate(X509Certificate2 certificate)
        {
            _certificate = certificate;
            return this;
        }

        public WebWork<TResult> Url(string url)
        {
            _url = url;
            return this;
        }

        public WebWork<TResult> Target(Stream targetStream)
        {
            _targetStream = targetStream;
            return this;
        }

        private void AddHeaders(WebRequest request)
        {
            foreach (var tuple in _httpRequestHeaders)
            {
                switch (tuple.Item1)
                {
                    case HttpRequestHeader.ContentLength: request.ContentLength = tuple.Item2.As<long>(); break;
                    case HttpRequestHeader.Connection: request.ConnectionGroupName = tuple.Item2; break;
                    case HttpRequestHeader.ContentType: request.ContentType = tuple.Item2; break;
                    case HttpRequestHeader.Accept: ((HttpWebRequest)request).Accept = tuple.Item2; break;
                    default: request.Headers.Add(tuple.Item1, tuple.Item2); break;
                }
            }
        }

        public WebWork<TResult> GET() { return SetMethod("GET"); }
        public WebWork<TResult> POST() { return SetMethod("POST"); }
        public WebWork<TResult> HEAD() { return SetMethod("HEAD"); }
        public WebWork<TResult> OPTIONS() { return SetMethod("OPTIONS"); }
        public WebWork<TResult> PUT() { return SetMethod("PUT"); }
        public WebWork<TResult> PATCH() { return SetMethod("PATCH"); }
        public WebWork<TResult> DELETE() { return SetMethod("DELETE"); }
        public WebWork<TResult> TRACE() { return SetMethod("TRACE"); }
        public WebWork<TResult> CONNECT() { return SetMethod("CONNECT"); }

        private WebWork<TResult> SetMethod(string method)
        {
            _method = method;
            return this;
        }

        protected override void OnStart()
        {
            var request = (HttpWebRequest)WebRequest.Create(_url);
            request.Method = _method;
            request.ProtocolVersion = HttpVersion.Version10;
            request.Credentials = CredentialCache.DefaultCredentials;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            request.AllowWriteStreamBuffering = false;
            if (IsSecurityProtocol()) request.ClientCertificates.Add(_certificate ?? Je.Cert.Signer);
            AddHeaders(request);
            if (_networkCredentialUser != null && _networkCredentialPassword != null)
            {
                request.PreAuthenticate = true;
                request.Credentials = new NetworkCredential(_networkCredentialUser, _networkCredentialPassword);
            }
            request.Proxy = GetProxy(_url);
            _webState = new WebState { Request = request };
            TryBeginGetRequestStream(_webState);
        }

        private void TryBeginGetRequestStream(WebState state)
        {
            try
            {
                var bytes = _contentBytes ?? (_content != null ? Encoding.Convert(Encoding.GetEncoding("utf-16"), Encoding.GetEncoding("utf-8"), Encoding.GetEncoding("utf-16").GetBytes(_content)) : null);
                if (bytes == null)
                {
                    TryBeginGetResponse(state);
                    return;
                }
                state.Bytes = bytes;
                state.Request.ContentLength = bytes.Length;
                state.Request.BeginGetRequestStream(TryEndGetRequestStream, state);
            }
            catch (Exception e)
            {
                ErrorAction(e);
            }
        }

        private void TryEndGetRequestStream(IAsyncResult ar)
        {
            try
            {
                _webState.WriterStream = _webState.Request.EndGetRequestStream(ar);
                _webState.WriterStream.BeginWrite(_webState.Buffer, 0, _webState.Buffer.Length, TryEndWriteRequest, _webState);
            }
            catch (Exception e)
            {
                ErrorAction(e);
            }
        }

        private void TryEndWriteRequest(IAsyncResult ar)
        {
            try
            {
                _webState.WriterStream.EndWrite(ar);
                if (_webState.Next())
                {
                    _webState.WriterStream.BeginWrite(_webState.Buffer, 0, _webState.Buffer.Length, TryEndWriteRequest, _webState);
                }
                else
                {
                    _webState.WriterStream.Close();
                    TryBeginGetResponse(_webState);
                }
            }
            catch (Exception e)
            {
                ErrorAction(e);
            }
        }

        private void TryBeginGetResponse(WebState state)
        {
            try
            {
                var ar = state.Request.BeginGetResponse(TryEndGetResponse, state);
                ThreadPool.RegisterWaitForSingleObject(ar.AsyncWaitHandle, TimeoutCallback, state.Request, RequestTimeout, true);
            }
            catch (Exception e)
            {
                ErrorAction(e);
            }
        }

        private void TryEndGetResponse(IAsyncResult ar)
        {
            try
            {
                _webState.WriterStream = _targetStream ?? new MemoryStream();
                _webState.Response = (HttpWebResponse)_webState.Request.EndGetResponse(ar);
                _webState.ReaderStream = _webState.Response.GetResponseStream();
                if (_webState.ReaderStream != null) _webState.ReaderStream.BeginRead(_webState.Buffer, 0, _webState.Buffer.Length, TryEndReadResponse, _webState);
            }
            catch (Exception e)
            {
                ErrorAction(e);
            }
        }

        private void TryEndReadResponse(IAsyncResult ar)
        {
            try
            {
                var state = (WebState)ar.AsyncState;
                var read = state.ReaderStream.EndRead(ar);
                if (read > 0)
                {
                    state.WriterStream.BeginWrite(state.Buffer, 0, read, TryEndWriteAfterRead, state);
                }
                else
                {
                    ThreadStock.Run(Completing);
                }
            }
            catch (Exception e)
            {
                ErrorAction(e);
            }
        }

        private void TryEndWriteAfterRead(IAsyncResult ar)
        {
            try
            {
                _webState.WriterStream.EndWrite(ar);
                _webState.ReaderStream.BeginRead(_webState.Buffer, 0, _webState.Buffer.Length, TryEndReadResponse, _webState);
            }
            catch (Exception e)
            {
                ErrorAction(e);
            }
        }

        private void Completing()
        {
            try
            {
                if (!(_webState.WriterStream is MemoryStream)) return;
                var bytes = new byte[_webState.WriterStream.Length];
                _webState.WriterStream.Seek(0, SeekOrigin.Begin);
                _webState.WriterStream.Read(bytes, 0, bytes.Length);
                Result = GetResult(bytes);
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

        private TResult GetResult(byte[] bytes)
        {
            if (ResultType == typeof(byte[])) return (TResult)(object)bytes;
            var str = Encoding.UTF8.GetString(bytes);
            if (ResultType == typeof(string)) return (TResult)(object)str;
            if (ResultType == typeof(XmlDocument))
            {
                var doc = new XmlDocument();
                doc.LoadXml(str);
                return (TResult)(object)doc;
            }
            return (TResult)Je.web.Of(str, ResultType);
        }

        static WebWork()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
            if (!TrySetSecurityProtocol()) TrySetSecurityProtocolXP();
        }

        private static bool TrySetSecurityProtocol()
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool TrySetSecurityProtocolXP()
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)768;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static WebProxy GetProxy(string url)
        {
            if (!Je<ProxyUse>.As.As<bool>()) return new WebProxy();
            if (!Je.Config.UseProxyFor(url)) return new WebProxy();
            var proxy = !string.IsNullOrWhiteSpace(Je<ProxyServer>.As) ? new WebProxy(Je<ProxyServer>.As, Je<ProxyPort>.As.As<int>()) : WebProxy.GetDefaultProxy();
            proxy.Credentials = !string.IsNullOrWhiteSpace(Je<ProxyUser>.As) ? new NetworkCredential(Je<ProxyUser>.As, Je<ProxyPassword>.As) : CredentialCache.DefaultCredentials;
            return proxy;
        }

        private static void TimeoutCallback(object state, bool timedOut)
        {
            if (timedOut) (state as HttpWebRequest).With(request => request.Abort());
        }
    }

    public class WebState
    {
        const int BUFFER_SIZE = 4096;
        private int _startPosition;
        private byte[] _buffer;
        private byte[] _bytes;
        public HttpWebResponse Response;
        public HttpWebRequest Request;
        public Stream WriterStream;
        public Stream ReaderStream;

        public WebState()
        {
            WriterStream = new MemoryStream();
            _buffer = new byte[BUFFER_SIZE];
            ReaderStream = null;
            Request = null;
        }

        public byte[] Buffer
        {
            get { return _buffer; }
        }

        public byte[] Bytes
        {
            get { return _bytes; }
            set { SetBytes(value); }
        }

        private void SetBytes(byte[] bytes)
        {
            _bytes = bytes ?? new byte[0];
            _startPosition = -1 * BUFFER_SIZE;
            Next();
        }

        public bool Next()
        {
            if (_startPosition >= _bytes.Length) return false;
            _startPosition = _startPosition + (_startPosition + BUFFER_SIZE <= _bytes.Length ? BUFFER_SIZE : _bytes.Length - _startPosition);
            var bufferLength = _startPosition + BUFFER_SIZE <= _bytes.Length ? BUFFER_SIZE : _bytes.Length - _startPosition;
            if (bufferLength == 0) return false;
            _buffer = _buffer.Length == bufferLength ? _buffer : new byte[bufferLength];
            Array.Copy(_bytes, _startPosition, _buffer, 0, _buffer.Length);
            return true;
        }

        public void Dispose()
        {
            Je.err.TryLess(() => (WriterStream != null).If(() => WriterStream.Close()));
            Je.err.TryLess(() => WriterStream = null);
            Je.err.TryLess(() => (ReaderStream != null).If(() => ReaderStream.Close()));
            Je.err.TryLess(() => ReaderStream = null);
        }
    }
}