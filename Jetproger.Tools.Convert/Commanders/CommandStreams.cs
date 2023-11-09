using System;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace Jetproger.Tools.Convert.Commanders
{
    public class WebStreams : CommandStreams
    {
        public HttpWebResponse Response { get; set; }
        public HttpWebRequest Request { get; set; }

        public override void Dispose()
        {
            base.Dispose();
            DisposeResponse();
        }

        private void DisposeResponse()
        {
            try
            {
                if (Response == null) return;
                ((IDisposable)Response).Dispose();
                Response = null;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
        }
    }

    public class CommandStreams
    {
        public byte[] Buffer { get; private set; }
        public Stream StreamReader { get; set; }
        public Stream StreamWriter { get; set; }
        const int BufferSize = 4096;

        public CommandStreams()
        {
            Reset();
        }

        public void Reset()
        {
            Buffer = new byte[BufferSize];
        }

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
                Trace.WriteLine(e);
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
                Trace.WriteLine(e);
            }
        }
    }
}