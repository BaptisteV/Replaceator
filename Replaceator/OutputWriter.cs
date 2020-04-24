using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Replaceator
{
    public class OutputWriter
    {
        public DirectoryInfo OutputDirectory { get; set; }
        private readonly FileInfo _outputFile;
        private readonly FileWriterMode _mode;

        private string _fileExtension;
        public string FileExtension
        {
            get => _fileExtension;
            set
            {
                if (value.StartsWith("."))
                {
                    _fileExtension = value.Skip(1).ToString();
                }
                else
                {
                    _fileExtension = value;
                }
            }
        }
        public OutputWriter(DirectoryInfo outputDirectory)
        {
            OutputDirectory = outputDirectory;
            _mode = FileWriterMode.CreateRandomFile;
        }

        public OutputWriter(FileInfo outputFile)
        {
            _outputFile = outputFile;
            _mode = FileWriterMode.Append;
        }

        public async Task<FileInfo> WriteAsync(string text)
        {
            switch (_mode)
            {
                case FileWriterMode.Append:
                    return await AppendTextToFile(text).ConfigureAwait(false);
                case FileWriterMode.CreateRandomFile:
                    return await WriteRandomNameFileAsync(text).ConfigureAwait(false);
                default:
                    return null;
            }
        }

        private async Task<FileInfo> WriteRandomNameFileAsync(string content)
        {
            var fileName = Path.GetRandomFileName();
            if (!string.IsNullOrEmpty(FileExtension))
            {
                fileName = Path.ChangeExtension(fileName, FileExtension);
            }

            using FileStream fs = File.Create(Path.Combine(OutputDirectory.FullName, fileName));
            var text = new UTF8Encoding(true).GetBytes(content);
            await fs.WriteAsync(text, 0, text.Length).ConfigureAwait(false);
            
            return new FileInfo(fileName);
        }

        private async Task<FileInfo> AppendTextToFile(string content)
        {
            using var sw = _outputFile.AppendText();
            await sw.WriteLineAsync(content).ConfigureAwait(false);
            return _outputFile;
        }
    }

    public enum FileWriterMode
    {
        Append,
        CreateRandomFile
    }
}
