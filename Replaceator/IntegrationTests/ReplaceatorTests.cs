using Replaceator.TestUtility;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Replaceator.tests
{
    public class ReplaceatorTests
    {
        [Fact]
        public async Task AppendTextToASingleFileFromAListOfWords()
        {
            var replaceWords = new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
            using var testCase = new AppendTextToASingleFileFromAListOfWords(replaceWords);
            var replaceator = new Replaceator(testCase.Options, new Logger());
            var outputFiles = await replaceator.Generate().ConfigureAwait(false);

            var text = File.ReadAllText(outputFiles.First().FullName);
            Assert.Equal("Sun\r\nMon\r\nTue\r\nWed\r\nThu\r\nFri\r\nSat\r\n", text);
        }

        [Fact]
        public async Task AppendTextToASingleFileFromAListOfFiles()
        {
            var replaceWords = new string[] { "Einstein\r\nTesla\r\n", "Copernic\r\nPtolémé\r\n" };
            using var testCase = new AppendTextToASingleFileFromAListOfFiles(replaceWords);
            var replaceator = new Replaceator(testCase.Options, new Logger());

            var outputFiles = await replaceator.Generate().ConfigureAwait(false);

            var text = File.ReadAllText(outputFiles.First().FullName);
            Assert.Equal(replaceWords[0] + "\r\n" + replaceWords[1] + "\r\n", text);
        }
    }
}


