using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;
using Pearl.Interfaces;

namespace Mocks
{
    /// <summary>
    /// The logger.
    /// </summary>
    [Export(typeof (ILogger))]
    public sealed class Logger : ILogger
    {
        /// <summary>
        /// The debug.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        public void Debug(string s)
        {
            WriteToTestFile("[Debug]" + s);
        }

        /// <summary>
        /// The fatal.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        public void Fatal(string s)
        {
            WriteToTestFile("[Fetal]" + s);
        }

        /// <summary>
        /// The info.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        public void Info(string s)
        {
            WriteToTestFile("[Info]" + s);
        }

        /// <summary>
        /// Gets the logger name.
        /// </summary>
        public string LoggerName
        {
            get { return "Mock Logger"; }
        }

        /// <summary>
        /// The warn.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        public void Warn(string s)
        {
            WriteToTestFile("[Warn]" + s);
        }


        /// <summary>
        /// The debug.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void Debug(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The debug.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void Debug(string s, Exception e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The fatal.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void Fatal(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The fatal.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void Fatal(string s, Exception e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The info.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void Info(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The info.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void Info(string s, Exception e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The warn.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void Warn(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The warn.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void Warn(string s, Exception e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The write to test file.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        private void WriteToTestFile(string text)
        {
            string filePath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), 
                "TestToolMockLog.txt");
            File.AppendAllText(filePath, DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt") + text + Environment.NewLine);
        }
    }
}