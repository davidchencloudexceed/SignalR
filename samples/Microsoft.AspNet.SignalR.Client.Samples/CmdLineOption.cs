using CommandLine;

namespace Microsoft.AspNet.SignalR.Client.Samples
{
    class CmdLineOption
    {
        [Option('a', "action", Required = true, HelpText = "specify your action")]
        public string action { get; set; }

    }
}
