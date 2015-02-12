namespace GitFlowVS.Extension
{
    public interface IGitFlowSection
    {
        void UpdateVisibleState();
        void Refresh();
        void ShowErrorNotification(string message);

    }
}