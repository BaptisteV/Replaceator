using Replaceator.TestUtility;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Replaceator.tests
{
    public class ReplaceatorTests : IDisposable
    {
        private static Random random = new Random();
        private TempFolder _tempFolder = new TempFolder();
        private string _pattern = "<pattern>";
        private FileInfo _templateFile;
        public ReplaceatorTests()
        {
            _templateFile = new FileInfo(Path.Combine(_tempFolder.Folder.FullName, "pattern_file.txt"));
            using var writer = File.CreateText(_templateFile.FullName);
            writer.Write(_pattern);
        }

        [Fact]
        public async Task AppendTextToASingleFileFromAListOfWords()
        {
            var outputFile = new FileInfo(Path.Combine(_tempFolder.Folder.FullName, "test"));
            using (File.CreateText(outputFile.FullName)) { }

            var options = new ReplaceatorOptions()
            {
                Pattern = _pattern,
                ReplaceWords = new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" },
                TemplateFile = _templateFile.FullName,
                Output = outputFile.FullName,
            };
            var replaceator = new Replaceator(options, new Logger());

            await replaceator.Generate().ConfigureAwait(false);

            var text = File.ReadAllText(outputFile.FullName);
            Assert.Equal("Sun\r\nMon\r\nTue\r\nWed\r\nThu\r\nFri\r\nSat\r\n", text);
            File.Delete(outputFile.FullName);
        }

        [Fact]
        public async Task AppendTextToASingleFileFromAListOfFiles()
        {
            var outputFile = new FileInfo(Path.Combine(_tempFolder.Folder.FullName, "test"));
            using (File.CreateText(outputFile.FullName)) { }

            var contentFile1 = "Einstein\r\nTesla\r\n";
            var inputFile1 = new FileInfo(Path.Combine(_tempFolder.Folder.FullName, "input1"));
            using (var writer = File.CreateText(inputFile1.FullName)) 
            {
                writer.Write(contentFile1);
            }

            var contentFile2 = "Copernic\r\nPtolémé\r\n";
            var inputFile2 = new FileInfo(Path.Combine(_tempFolder.Folder.FullName, "input2"));
            using (var writer = File.CreateText(inputFile2.FullName))
            {
                writer.Write(contentFile2);
            }

            var options = new ReplaceatorOptions()
            {
                Pattern = _pattern,
                ReplaceWords = new string[] { inputFile1.FullName, inputFile2.FullName },
                TemplateFile = _templateFile.FullName,
                Output = outputFile.FullName,
            };
            var replaceator = new Replaceator(options, new Logger());

            await replaceator.Generate().ConfigureAwait(false);

            var text = File.ReadAllText(outputFile.FullName);
            Assert.Equal(contentFile1 + contentFile2, text);
            File.Delete(outputFile.FullName);
            File.Delete(inputFile1.FullName);
            File.Delete(inputFile2.FullName);
        }

        public void Dispose()
        {
            _tempFolder.Dispose();
        }
    }
}


