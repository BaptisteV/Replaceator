using System;
using System.IO;

namespace Replaceator.TestUtility
{
    public class TempFolder : IDisposable
    {
        private static readonly Random _random = new Random();

        public DirectoryInfo Folder { get; }

        public TempFolder(string prefix = "TempFolder")
        {
            string folderName;

            lock (_random)
            {
                folderName = prefix + _random.Next(1000000000);
            }

            Folder = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), folderName));
        }

        public void Dispose()
        {
            Directory.Delete(Folder.FullName, true);
        }
    }
}
