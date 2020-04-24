using CommandLine;
using System;
using System.Collections.Generic;

namespace Replaceator
{
    public interface IReplaceatorOptions
    {
        string TemplateFile { get; }
        string Pattern { get; }
        string Output { get; }
        string Extension { get; }
        IEnumerable<string> ReplaceWords { get; }

        public ReplaceatorOptions Parse(string[] args);
    }
    public class ReplaceatorOptions : IReplaceatorOptions
    {
        [Option('t', "template", Required = true, HelpText = "Template file that contains the pattern word")]
        public string TemplateFile { get; set; }

        [Option('p', "pattern", Required = true, HelpText = "Pattern in template file to replace by replace words")]
        public string Pattern { get; set; }

        [Option('r', "replace", Required = true, HelpText = "Replace words that will be put in place of the pattern. It can also be a list of existing files. If you pass files, a replace word will be a line in these files")]
        public IEnumerable<string> ReplaceWords { get; set; }

        [Option('e', "extension", Required = false, HelpText = "Extension of the output file (defaults to 3 random characters)")]
        public string Extension { get; set; }

        [Option('o', "output", Required = false, HelpText = "Output for the replacement. The default is the current directory (usually exe's directory). It can point to \r\n" +
            "\t- A directory\t=> A file with a random name will be generated for each replace word\r\n" +
            "\t- A file\t=> The file will be erased and the content will become the content of the template file times the replace words (append mode)")]
        public string Output { get; set; }

        public ReplaceatorOptions Parse(string[] args)
        {
            ReplaceatorOptions options = null;
            Parser.Default.ParseArguments<ReplaceatorOptions>(args)
                .WithParsed((opts) =>
                {
                    options = opts;
                    if (string.IsNullOrEmpty(options.Output))
                    {
                        options.Output = AppDomain.CurrentDomain.BaseDirectory;
                    }
                })
                .WithNotParsed((errors) =>
                {
                    /*foreach (var err in errors)
                    {
                        throw new Exception(err.ToString());
                    }*/
                });

            return options;
        }
    }
}
