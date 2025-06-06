// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using Microsoft.DotNet.Cli.Extensions;

namespace Microsoft.DotNet.Cli.Commands.Tool;

internal static class ToolCommandRestorePassThroughOptions
{
    public static CliOption<bool> DisableParallelOption = new ForwardedOption<bool>("--disable-parallel")
    {
        Description = CliCommandStrings.CmdDisableParallelOptionDescription,
        Arity = ArgumentArity.Zero
    }.ForwardAs("--disable-parallel");

    public static CliOption<bool> NoCacheOption = new ForwardedOption<bool>("--no-cache")
    {
        Description = CliCommandStrings.CmdNoCacheOptionDescription,
        Hidden = true,
        Arity = ArgumentArity.Zero
    }.ForwardAs("--no-cache");

    public static CliOption<bool> NoHttpCacheOption = new ForwardedOption<bool>("--no-http-cache")
    {
        Description = CliCommandStrings.CmdNoCacheOptionDescription,
        Arity = ArgumentArity.Zero
    }.ForwardAs("--no-http-cache");

    public static CliOption<bool> IgnoreFailedSourcesOption = new ForwardedOption<bool>("--ignore-failed-sources")
    {
        Description = CliCommandStrings.CmdIgnoreFailedSourcesOptionDescription,
        Arity = ArgumentArity.Zero
    }.ForwardAs("--ignore-failed-sources");

    public static CliOption<bool> InteractiveRestoreOption = CommonOptions.InteractiveOption();
}
