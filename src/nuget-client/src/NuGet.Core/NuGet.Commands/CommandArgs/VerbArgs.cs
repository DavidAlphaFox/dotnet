// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Commands
{
    public partial class AddSourceArgs
    {
        public string Source { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool StorePasswordInClearText { get; set; }
        public string ValidAuthenticationTypes { get; set; }
        public string ProtocolVersion { get; set; }
        public string Configfile { get; set; }
        public bool AllowInsecureConnections { get; set; }
    }

    public partial class AddClientCertArgs
    {
        public string PackageSource { get; set; }
        public string Path { get; set; }
        public string Password { get; set; }
        public bool StorePasswordInClearText { get; set; }
        public string StoreLocation { get; set; }
        public string StoreName { get; set; }
        public string FindBy { get; set; }
        public string FindValue { get; set; }
        public bool Force { get; set; }
        public string Configfile { get; set; }
    }

    public partial class DisableSourceArgs
    {
        public string Name { get; set; }
        public string Configfile { get; set; }
    }

    public partial class EnableSourceArgs
    {
        public string Name { get; set; }
        public string Configfile { get; set; }
    }

    public partial class ListSourceArgs
    {
        public string Format { get; set; }
        public string Configfile { get; set; }
    }

    public partial class ListClientCertArgs
    {
        public string Configfile { get; set; }
    }

    public partial class RemoveSourceArgs
    {
        public string Name { get; set; }
        public string Configfile { get; set; }
    }

    public partial class RemoveClientCertArgs
    {
        public string PackageSource { get; set; }
        public string Configfile { get; set; }
    }

    public partial class UpdateSourceArgs
    {
        public string Name { get; set; }
        public string Source { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool StorePasswordInClearText { get; set; }
        public string ValidAuthenticationTypes { get; set; }
        public string ProtocolVersion { get; set; }
        public string Configfile { get; set; }
        public bool AllowInsecureConnections { get; set; }
    }

    public partial class UpdateClientCertArgs
    {
        public string PackageSource { get; set; }
        public string Path { get; set; }
        public string Password { get; set; }
        public bool StorePasswordInClearText { get; set; }
        public string StoreLocation { get; set; }
        public string StoreName { get; set; }
        public string FindBy { get; set; }
        public string FindValue { get; set; }
        public bool Force { get; set; }
        public string Configfile { get; set; }
    }

}
