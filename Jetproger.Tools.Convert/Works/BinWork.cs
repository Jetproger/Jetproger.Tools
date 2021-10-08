using System;
using System.IO;
using System.Text;
using System.Xml;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Convert.Works
{
    public class BinWork : BinWork<object>
    {
        public BinWork(Stream stream, object content) : base(stream)
        {
            Result = content;
        }

        public static BinWork Op(string fileName, object content)
        {
            return new BinWork(new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite), content);
        }
    }

    public class BinWork<TResult> : Work<TResult>
    {
        protected Encoding Encoding;
        protected Type ResultType;

        private Stream _writerStream;
        private Stream _stream;
        private BinState _binState;

        public static BinWork<TResult> Op(string fileName)
        {
            return new BinWork<TResult>(new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite));
        }

        public BinWork(Stream stream)
        {
            ResultType = typeof(TResult);
            Encoding = Encoding.UTF8;
            _stream = stream;
        }

        protected override void Disposing()
        {
            Je.err.TryLess(() => (_binState != null).If(() => _binState.Dispose()));
            Je.err.TryLess(() => _binState = null);
        }

        public BinWork<TResult> SetEncoding(Encoding encoding)
        {
            Encoding = encoding;
            return this;
        }

        public BinWork<TResult> SetWriterStream(Stream writerStream)
        {
            _writerStream = writerStream;
            return this;
        }

        protected override void OnStart()
        {
            if (ResultType != typeof(object)) TryBeginRead(); else TryBeginWrite();
        }

        private void TryBeginRead()
        {
            try
            {
                _binState = new BinState();
                _binState.ReaderStream = _stream;
                _binState.WriterStream = _writerStream ?? _binState.WriterStream;
                _binState.ReaderStream.BeginRead(_binState.Buffer, 0, _binState.Buffer.Length, TryEndRead, _binState);
            }
            catch (Exception e)
            {
                ErrorAction(e);
            }
        }

        private void TryEndRead(IAsyncResult ar)
        { 
            try
            {
                var read = _binState.ReaderStream.EndRead(ar);
                if (read > 0)
                {
                    _binState.WriterStream.BeginWrite(_binState.Buffer, 0, read, TryEndWriteAfterRead, _binState);
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
                _binState.WriterStream.EndWrite(ar);
                _binState.ReaderStream.BeginRead(_binState.Buffer, 0, _binState.Buffer.Length, TryEndRead, _binState);
            }
            catch (Exception e)
            {
                ErrorAction(e);
            }
        }

        private void TryBeginWrite()
        {
            try
            {
                _binState = new BinState();
                _binState.WriterStream = _stream;
                _binState.Bytes = GetBytes(Result);
                if (_binState.Bytes.Length == 0)
                {
                    ThreadStock.Run(Completing);
                    return;
                }
                _binState.WriterStream.BeginWrite(_binState.Buffer, 0, _binState.Buffer.Length, TryEndWrite, _binState);
            }
            catch (Exception e)
            {
                ErrorAction(e);
            }
        }

        private void TryEndWrite(IAsyncResult ar)
        {
            try
            {
                _binState.WriterStream.EndWrite(ar);
                if (_binState.Next())
                {
                    _binState.WriterStream.BeginWrite(_binState.Buffer, 0, _binState.Buffer.Length, TryEndWrite, _binState);
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

        private void Completing()
        {
            try
            {
                if (ResultType == typeof (object)) return;
                var bytes = new byte[_binState.WriterStream.Length];
                _binState.WriterStream.Seek(0, SeekOrigin.Begin);
                _binState.WriterStream.Read(bytes, 0, bytes.Length);
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

        private byte[] GetBytes(object obj)
        {
            if (obj == null || obj == DBNull.Value) return new byte[0];
            var bytes = obj as byte[];
            if (bytes != null) return bytes;
            var str = obj as string;
            if (str != null) return Encoding.GetBytes(str);
            var doc = obj as XmlDocument;
            if (doc != null) return Encoding.GetBytes(doc.InnerXml);
            return Encoding.GetBytes(Je.web.To(obj));
        }

        private TResult GetResult(byte[] bytes)
        {
            if (ResultType == typeof(byte[])) return (TResult)(object)bytes;
            var str = Encoding.GetString(bytes);
            if (ResultType == typeof(string)) return (TResult)(object)str;
            if (ResultType == typeof(XmlDocument))
            {
                var doc = new XmlDocument();
                doc.LoadXml(str);
                return (TResult)(object)doc;
            }
            return (TResult)Je.web.Of(str, ResultType);
        }
    }

    public class BinState
    {
        const int BUFFER_SIZE = 4096;
        private int _startPosition; 
        private byte[] _buffer;
        private byte[] _bytes;

        public Stream WriterStream;
        public Stream ReaderStream;

        public BinState()
        {
            WriterStream = new MemoryStream();
            _buffer = new byte[BUFFER_SIZE];
            ReaderStream = null;
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