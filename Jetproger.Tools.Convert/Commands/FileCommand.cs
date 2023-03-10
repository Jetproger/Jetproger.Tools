﻿using System; 
using System.IO;
using System.Text;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Commanders;
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Convert.Commands
{
    public abstract class FileCommandWriter<T> : FileCommand<T>
    {
        protected FileCommandWriter(string fileName, Encoding encoding, T content) : base(fileName, encoding, content) { }
        protected FileCommandWriter(string fileName, T content) : base(fileName, Encoding.UTF8, content) { }
    }

    public abstract class FileCommandReader<T> : FileCommand<T>
    {
        protected FileCommandReader(string fileName, Encoding encoding) : base(fileName, encoding) { }
        protected FileCommandReader(string fileName) : base(fileName, Encoding.UTF8) { }
    }

    public abstract class FileCommand<T> : Command<T>
    {
        public string FileName { get; private set; }
        private readonly Encoding _encoding;
        private readonly bool _isWriter;

        protected FileCommand(string fileName, Encoding encoding, T content)
        {
            FileName = f.fss.pathnameextof(fileName);
            _encoding = encoding;
            _isWriter = true;
            Value = content;
        }

        protected FileCommand(string fileName, Encoding encoding)
        {
            FileName = f.fss.pathnameextof(fileName);
            _encoding = encoding;
            _isWriter = false;
        }

        protected override void Execute()
        {
            __BeginRead();
        }

        private CommandStreams BeginRead()
        {
            var state = GetStreams();
            if (state.StreamReader is MemoryStream ms && ms.Length == 0) f.sys.threadof(() => Completing(state));
            else state.StreamReader.BeginRead(state.Buffer, 0, state.Buffer.Length, __EndRead, state);
            return state;
        }

        private void EndRead(IAsyncResult ar, CommandStreams state)
        {
            var read = state.StreamReader.EndRead(ar);
            if (read > 0) state.StreamWriter.BeginWrite(state.Buffer, 0, read, __EndWrite, state);
            else f.sys.threadof(() => Completing(state));
        }

        private void EndWrite(IAsyncResult ar, CommandStreams state)
        {
            state.StreamWriter.EndWrite(ar);
            state.Reset();
            state.StreamReader.BeginRead(state.Buffer, 0, state.Buffer.Length, __EndRead, state);
        }

        private void Completing(CommandStreams state)
        {
            try
            {
                var result = state.StreamWriter is MemoryStream ms ? ms.As<SimpleJson>(_encoding).As<T>() : default(T);
                state.Dispose();
                Result = result;
            }
            catch (Exception e)
            {
                state.Dispose();
                Error = e;
            }
        }

        private CommandStreams GetStreams()
        {
            var reader = CreateReader();
            var writer = CreateWriter(reader);
            return new CommandStreams { StreamReader = reader, StreamWriter = writer };
        }

        private Stream CreateReader()
        {
            return _isWriter ? (Stream)Value.As<SimpleJson>(_encoding).As<MemoryStream>() : new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
        }

        private Stream CreateWriter(Stream reader)
        {
            return _isWriter ? (reader.Length > 0 ? new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite) : (Stream)null) : new MemoryStream();
        }

        #region exceptions

        private void __BeginRead()
        {
            var state = (CommandStreams)null;
            try
            {
                state = BeginRead();
            }
            catch (Exception e)
            {
                if (state != null) state.Dispose();
                Error = e;
            }
        }

        private void __EndRead(IAsyncResult ar)
        {
            var state = (CommandStreams)null;
            try
            {
                state = (CommandStreams)ar.AsyncState;
                EndRead(ar, state);
            }
            catch (Exception e)
            {
                if (state != null) state.Dispose();
                Error = e;
            }
        }

        private void __EndWrite(IAsyncResult ar)
        {
            var state = (CommandStreams)null;
            try
            {
                state = (CommandStreams)ar.AsyncState;
                EndWrite(ar, state);
            }
            catch (Exception e)
            {
                if (state != null) state.Dispose();
                Error = e;
            }
        }

        #endregion
    }
}