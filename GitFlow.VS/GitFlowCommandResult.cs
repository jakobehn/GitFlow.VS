namespace GitFlow.VS
{
    public class GitFlowCommandResult
    {
        public GitFlowCommandResult(bool success, string commandOutput)
        {
            Success = success;
            CommandOutput = commandOutput;
        }

        public bool Success { get; set; }
        public string CommandOutput { get; set; }
    }

    public class GitFlowTimedOutCommandResult : GitFlowCommandResult
    {
        public GitFlowTimedOutCommandResult(string command)
            :base(false, "The command '" + command + "' is taking longer than expected. You might be prompted for information such as credentials. Please run the command from command line to find out what is blocking the process")
        {
        }
    }

}