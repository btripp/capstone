// Guids.cs
// MUST match guids.h
using System;

namespace AugustaStateUniversity.SeniorCapstone
{
    static class GuidList
    {
        public const string guidSeniorCapstonePkgString = "04e2b064-e98c-48ab-8ecd-c2c1b3ebfdbb";
        public const string guidSeniorCapstoneCmdSetString = "82a51e80-8480-4241-bcc3-a858085fe9fa";
        public const string guidToolWindowPersistanceString = "7f70a456-a1ef-4bcb-b12c-bac9ff0966e0";

        public static readonly Guid guidSeniorCapstoneCmdSet = new Guid(guidSeniorCapstoneCmdSetString);
    };
}