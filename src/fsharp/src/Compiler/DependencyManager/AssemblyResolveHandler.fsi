// Copyright (c) Microsoft Corporation.  All Rights Reserved.  See License.txt in the project root for license information.

namespace FSharp.Compiler.DependencyManager

open System

/// Signature for ResolutionProbe callback
/// host implements this, it's job is to return a list of assembly paths to probe.
type AssemblyResolutionProbe = delegate of Unit -> seq<string>

/// Handle Assembly resolution
type AssemblyResolveHandler =

    /// Construct a new DependencyProvider
    new: assemblyProbingPaths: AssemblyResolutionProbe | null -> AssemblyResolveHandler

    /// Construct a new DependencyProvider
    internal new: assemblyProbingPaths: AssemblyResolutionProbe option -> AssemblyResolveHandler

    interface IDisposable
