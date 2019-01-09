using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

[assembly: InternalsVisibleTo("Localization.Database.EFCore.Tests")]
namespace Scalesoft.Localization.Database.EFCore.Logging
{
    internal static class LogProvider
    {
        private static ILoggerFactory m_loggerFactory;

        public static void AttachLoggerFactory(ILoggerFactory loggerFactory)
        {           
            m_loggerFactory = loggerFactory;
        }

        //[CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static ILogger GetCurrentClassLogger()
        {
            if (m_loggerFactory == null)
            {
                return NullLogger.Instance;
            }

            return m_loggerFactory.CreateLogger(GetClassFullName());
        }

        /// <summary>
        /// Copyright (c) 2004-2016 Jaroslaw Kowalski &lt;jaak@jkowalski.net&gt;, Kim Christensen, Julian Verdurmen
        ///
        ///All rights reserved.
        ///
        ///Redistribution and use in source and binary forms, with or without 
        ///modification, are permitted provided that the following conditions 
        ///are met:
        ///
        ///* Redistributions of source code must retain the above copyright notice, 
        ///  this list of conditions and the following disclaimer. 
        ///
        ///* Redistributions in binary form must reproduce the above copyright notice,
        ///  this list of conditions and the following disclaimer in the documentation
        ///  and/or other materials provided with the distribution. 
        ///
        ///* Neither the name of Jaroslaw Kowalski nor the names of its 
        ///  contributors may be used to endorse or promote products derived from this
        ///  software without specific prior written permission. 
        ///
        ///THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
        ///AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
        ///IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
        ///ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
        ///LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
        ///CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
        ///SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
        ///INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
        ///CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
        ///ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF 
        ///THE POSSIBILITY OF SUCH DAMAGE.
        /// </summary>
        /// <returns></returns>
        private static string GetClassFullName()
        {
            var framesToSkip = 2;

            var className = string.Empty;

            var stackTrace = Environment.StackTrace;
            var stackTraceLines = stackTrace.Replace("\r", "").Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < stackTraceLines.Length; ++i)
            {
                var callingClassAndMethod = stackTraceLines[i].Split(new[] { " ", "<>", "(", ")" }, StringSplitOptions.RemoveEmptyEntries)[1];
                var methodStartIndex = callingClassAndMethod.LastIndexOf(".", StringComparison.Ordinal);
                if (methodStartIndex > 0)
                {
                    // Trim method name. 
                    var callingClass = callingClassAndMethod.Substring(0, methodStartIndex);
                    // Needed because of extra dot, for example if method was .ctor()
                    className = callingClass.TrimEnd('.');
                    if (!className.StartsWith("System.Environment") && framesToSkip != 0)
                    {
                        i += framesToSkip - 1;
                        framesToSkip = 0;
                        continue;
                    }
                    if (!className.StartsWith("System."))
                        break;
                }
            }

            return className;
        }
    }
}