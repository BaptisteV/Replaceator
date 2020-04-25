using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Replaceator.TestUtility
{
    public interface ITestCaseOptions
    {
        public ReplaceatorOptions Options { get; set; }
    }

    public class CreateTempFolderWithTemplateFile
    {
        protected TempFolder _tempFolder = new TempFolder();
        protected string _pattern = "<pattern>";
        protected FileInfo _templateFile;
        public CreateTempFolderWithTemplateFile()
        {
            _templateFile = new FileInfo(Path.Combine(_tempFolder.Folder.FullName, "pattern_file.txt"));
            using var writer = File.CreateText(_templateFile.FullName);
            writer.Write(_pattern);
        }
    }
    public class AppendTextToASingleFileFromAListOfWords : CreateTempFolderWithTemplateFile, ITestCaseOptions, IDisposable
    {
        public ReplaceatorOptions Options { get; set; }

        public AppendTextToASingleFileFromAListOfWords(string[] replaceWords)
        {
            var outputFile = new FileInfo(Path.Combine(_tempFolder.Folder.FullName, "test"));
            using (File.CreateText(outputFile.FullName)) { }

            Options = new ReplaceatorOptions()
            {
                Pattern = _pattern,
                ReplaceWords = replaceWords,
                TemplateFile = _templateFile.FullName,
                Output = outputFile.FullName,
            };
        }
        public void Dispose()
        {
            _tempFolder.Dispose();
        }
    }
    public class AppendTextToASingleFileFromAListOfFiles : CreateTempFolderWithTemplateFile, ITestCaseOptions, IDisposable
    {        
        public ReplaceatorOptions Options { get; set; }
        public List<string> inputFilesContent;
        public AppendTextToASingleFileFromAListOfFiles(string[] replaceWords)
        {
            inputFilesContent = new List<string>();
            var outputFile = new FileInfo(Path.Combine(_tempFolder.Folder.FullName, "test"));
            using (File.CreateText(outputFile.FullName)) { }
            
            var contentFile1 = replaceWords[0];
            inputFilesContent.Add(contentFile1);
            var inputFile1 = new FileInfo(Path.Combine(_tempFolder.Folder.FullName, "input1"));
            using (var writer = File.CreateText(inputFile1.FullName))
            {
                writer.Write(contentFile1);
            }

            var contentFile2 = replaceWords[1];
            inputFilesContent.Add(contentFile2);
            var inputFile2 = new FileInfo(Path.Combine(_tempFolder.Folder.FullName, "input2"));
            using (var writer = File.CreateText(inputFile2.FullName))
            {
                writer.Write(contentFile2);
            }

            Options = new ReplaceatorOptions()
            {
                Pattern = _pattern,
                ReplaceWords = replaceWords,
                TemplateFile = _templateFile.FullName,
                Output = outputFile.FullName,
            };
        }
        public void Dispose()
        {
            _tempFolder.Dispose();
        }
    }

}
