using System;
using System.Windows;

namespace GitFlow.VS
{
    public class BranchItem
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public DateTimeOffset LastCommit { get; set; }
        public bool IsTracking { get; set; }
        public bool IsCurrentBranch { get; set; }
        public bool IsRemote { get; set; }

        public string LastCommitAsString
        {
            get
            {
                TimeSpan timeSpan = DateTime.Now - LastCommit;
                var daysSince = timeSpan.Days;
                if (daysSince == 0)
                {
                    return timeSpan.Hours + " hours ago";
                }
                return daysSince + " days ago";
            }
        }

        public Visibility IsRemoteBranchVisibility
        {
            get
            {
                if (IsRemote)
                    return Visibility.Visible;
                return Visibility.Hidden;
            }
        }

        public string ToolTip
        {
            get { return "Commit: " + CommitId + "\nAuthor: " + Author + "\nDate: " + LastCommit.Date.ToShortDateString() + "\n\n" + Message; }
        }

        public string CommitId { get; set; }
        public string Message { get; set; }
    }
}