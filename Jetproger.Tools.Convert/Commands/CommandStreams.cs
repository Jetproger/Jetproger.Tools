using System;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace Jetproger.Tools.Convert.Commands
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
                if (Response != null) ((IDisposable)Response).Dispose();
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

        private void DisposeWriter()
        {
            try
            {
                if (StreamWriter != null) StreamWriter.Dispose();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
        }

        private void DisposeReader()
        {
            try
            {
                if (StreamReader != null) StreamReader.Dispose();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
        }
    }
}