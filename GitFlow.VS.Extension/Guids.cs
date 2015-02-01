// Guids.cs
// MUST match guids.h
using System;

namespace GitFlowVS.Extension
{
    static class GuidList
    {
        public const string guidGitFlow_VS_ExtensionPkgString = "19ffe08d-8c71-4125-a024-6296bd27fce8";
        public const string guidGitFlow_VS_ExtensionCmdSetString = "9be3ffde-c8d0-4d89-824b-f310e4d05ca1";

        public static readonly Guid guidGitFlow_VS_ExtensionCmdSet = new Guid(guidGitFlow_VS_ExtensionCmdSetString);

        public const string sampleTeamExplorerSection = "09C3A4DF-7040-4AC4-BA8B-0740B53BD9D7";

        public const string gitFlowPage = "1F9974CD-16C3-4AEF-AED2-0CE37988E2F1";
        public const string startFeatureLink = "355625AA-BD86-4633-B718-8E75E7C39523";
        public const string topSection = "F63C9A55-E0A2-4E7C-A78B-C37512EAEE5D";
        public const string initSection = "12760882-D1DE-46FF-8965-045017C7472D";
    };
}