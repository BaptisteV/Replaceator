using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Replaceator
{
    public interface IReplaceator
    {
        public Task<IEnumerable<FileInfo>> Generate();
    }
    public class Replaceator : IReplaceator
    {
        private OutputWriter _outputWriter;
        private InputReader _inputReader;

        private readonly FileInfo _templateFile;

        private readonly string _patternToReplace;
        private ILogger _logger;

        public Replaceator(IReplaceatorOptions options, ILogger logger)
        {
            _logger = logger;
            _templateFile = new FileInfo(options.TemplateFile);
            _patternToReplace = options.Pattern;
            var attributes = File.GetAttributes(options.Output);
            if (attributes.HasFlag(FileAttributes.Directory))
            {
                _outputWriter = new OutputWriter(new DirectoryInfo(options.Output));
            }
            else
            {
                _outputWriter = new OutputWriter(new FileInfo(options.Output));
            }
            if (!string.IsNullOrEmpty(options.Extension))
            {
                _outputWriter.FileExtension = options.Extension;
            }
            var inputAreFiles = options.ReplaceWords.All(s => File.Exists(s));
            if (inputAreFiles)
            {
                _logger.Log("File input mode");
                _inputReader = new InputReader(options.ReplaceWords.Select(s => new FileInfo(s)).ToList());
            }
            else
            {
                _logger.Log("Word input mode");
                _inputReader = new InputReader(options.ReplaceWords);
            }

        }

        private async Task<FileInfo> CreateOutputFile(string content)
        {
            var file = await _outputWriter.WriteAsync(content).ConfigureAwait(false);
            _logger.Log($"Created file {file.FullName}");
            return file;
        }

        public async Task<IEnumerable<FileInfo>> Generate()
        {
            var createdFiles = new List<FileInfo>();
            var templateFileContent = await File.ReadAllTextAsync(_templateFile.FullName);

            if (!templateFileContent.Contains(_patternToReplace))
            {
                throw new Exception($"pattern {_patternToReplace} not found in file {_templateFile.FullName}");
            }

            foreach(var word in _inputReader.Words)
            {
                createdFiles.Add(await CreateOutputFile(templateFileContent.Replace(_patternToReplace, word)));
            }
            return createdFiles;
        }
    }
}
