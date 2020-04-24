using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Replaceator
{
    public class InputReader
    {
        public List<string> Words { get; }
        public InputReader(IEnumerable<string> words)
        {
            Words = new List<string>();
            Words.AddRange(words);
        }
        public InputReader(IEnumerable<FileInfo> files)
        {
            Words = new List<string>();
            foreach (var file in files)
            {
                Words.AddRange(File.ReadAllLines(file.FullName));
            }
        }
    }
}
