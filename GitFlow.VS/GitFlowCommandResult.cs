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
}