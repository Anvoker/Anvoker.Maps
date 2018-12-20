using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Reflection;
using System.IO;

namespace Anvoker.Collections.Tests.Common
{
    public static class Tracing
    {
        static Tracing()
        {
            string path = Path.Combine(DllPath, "SetupTrace.log");
            //streamLogFile = File.Create(path, 4096, FileOptions.Asynchronous);
            streamLogFile = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read);
            Trace.AutoFlush = true;
        }

        public static TextWriterTraceListener SetupTracer
            => new TextWriterTraceListener(
                streamLogFile,
                "SetupTrace")
            {
                TraceOutputOptions = TraceOptions.LogicalOperationStack
                | TraceOptions.DateTime
                | TraceOptions.Timestamp
                | TraceOptions.ProcessId
                | TraceOptions.ThreadId
            };

        public static string DllPath { get; }
            = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private static readonly Stream streamLogFile;
    }
}