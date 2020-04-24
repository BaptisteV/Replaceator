using Replaceator.TestUtility;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Replaceator.tests
{
    public class OutputWriterTests
    {
        [Fact]
        public async Task WriteToDirectoryAsync()
        {
            using var tempFolder = new TempFolder();
            var fileWriter = new OutputWriter(tempFolder.Folder);
            var fileCount = tempFolder.Folder.GetFiles().Length;

            await fileWriter.WriteAsync("toto").ConfigureAwait(false);

            Assert.Equal(fileCount + 1, tempFolder.Folder.GetFiles().Length);
        }

        [Fact]
        public async Task AppendToFileAsync()
        {
            using var tempFolder = new TempFolder();
            var testFile = new FileInfo(Path.Combine(tempFolder.Folder.FullName, "test"));
            var fileWriter = new OutputWriter(testFile);

            await fileWriter.WriteAsync("toto").ConfigureAwait(false);

            var text = File.ReadAllText(testFile.FullName);
            Assert.Equal("toto\r\n", text);
        }
    }
}
