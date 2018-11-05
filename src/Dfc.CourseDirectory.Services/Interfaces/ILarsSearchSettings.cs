﻿namespace Dfc.CourseDirectory.Services.Interfaces
{
    public interface ILarsSearchSettings
    {
        string ApiUrl { get; }
        string ApiVersion { get; }
        string ApiKey { get; }
        string Indexes { get; }
    }
}