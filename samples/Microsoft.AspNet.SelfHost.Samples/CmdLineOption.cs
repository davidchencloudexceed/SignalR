using CommandLine;

namespace Microsoft.AspNet.SelfHost.Samples
{
    class CmdLineOption
    {
        [Option('g', "group", Required = true, HelpText = "stock symbol name")]
        public string stockSymbol { get; set; }
        [Option('p', "price", Required = true, HelpText = "stock price")]
        public double price { get; set; }
    }
}
