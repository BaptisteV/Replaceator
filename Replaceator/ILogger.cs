using System;
using System.Collections.Generic;
using System.Text;

namespace Replaceator
{
    public interface ILogger
    {
        public void Log(string message);
    }

    public class Logger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
