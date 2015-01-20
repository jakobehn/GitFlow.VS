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
    };
}