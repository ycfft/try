﻿using System.CommandLine;
using System.IO;
using System.Threading.Tasks;
using MLS.Agent.CommandLine;
using MLS.Agent.Tools;
using WorkspaceServer;
using WorkspaceServer.Packaging;

namespace MLS.Agent.CommandLine
{
    public static class InstallCommand
    {
        public static async Task Do(InstallOptions options, IConsole console)
        {
            var dotnet = new Dotnet();
            (await dotnet.ToolInstall(
                options.PackageName,
                options.Location,
                options.AddSource)).ThrowOnFailure();

            var commandPath = Path.Combine(options.Location.FullName, options.PackageName);
            var result = (await MLS.Agent.Tools.CommandLine.Execute(commandPath, "extract-package"));
            result.ThrowOnFailure();
            (await MLS.Agent.Tools.CommandLine.Execute(commandPath, "prepare-package")).ThrowOnFailure();

        }
    }
}
