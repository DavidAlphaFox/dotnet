﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Recommendations;

[Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
public sealed class AddKeywordRecommenderTests : KeywordRecommenderTests
{
    [Fact]
    public async Task TestNotAtRoot_Interactive()
    {
        await VerifyAbsenceAsync(SourceCodeKind.Script,
@"$$");
    }

    [Fact]
    public async Task TestNotAfterClass_Interactive()
    {
        await VerifyAbsenceAsync(SourceCodeKind.Script,
            """
            class C { }
            $$
            """);
    }

    [Fact]
    public async Task TestNotAfterGlobalStatement_Interactive()
    {
        await VerifyAbsenceAsync(SourceCodeKind.Script,
            """
            System.Console.WriteLine();
            $$
            """);
    }

    [Fact]
    public async Task TestNotAfterGlobalVariableDeclaration_Interactive()
    {
        await VerifyAbsenceAsync(SourceCodeKind.Script,
            """
            int i = 0;
            $$
            """);
    }

    [Fact]
    public async Task TestNotInUsingAlias()
    {
        await VerifyAbsenceAsync(
@"using Goo = $$");
    }

    [Fact]
    public async Task TestNotInGlobalUsingAlias()
    {
        await VerifyAbsenceAsync(
@"global using Goo = $$");
    }

    [Fact]
    public async Task TestNotInEmptyStatement()
    {
        await VerifyAbsenceAsync(AddInsideMethod(
@"$$"));
    }

    [Fact]
    public async Task TestAfterEvent()
    {
        await VerifyKeywordAsync(
            """
            class C {
               event Goo Bar { $$
            """);
    }

    [Fact]
    public async Task TestAfterAttribute()
    {
        await VerifyKeywordAsync(
            """
            class C {
               event Goo Bar { [Bar] $$
            """);
    }

    [Fact]
    public async Task TestAfterRemove()
    {
        await VerifyKeywordAsync(
            """
            class C {
               event Goo Bar { remove { } $$
            """);
    }

    [Fact]
    public async Task TestAfterRemoveAndAttribute()
    {
        await VerifyKeywordAsync(
            """
            class C {
               event Goo Bar { remove { } [Bar] $$
            """);
    }

    [Fact]
    public async Task TestAfterRemoveBlock()
    {
        await VerifyKeywordAsync(
            """
            class C {
               event Goo Bar { set { } $$
            """);
    }

    [Fact]
    public async Task TestNotAfterAddKeyword()
    {
        await VerifyAbsenceAsync(
            """
            class C {
               event Goo Bar { add $$
            """);
    }

    [Fact]
    public async Task TestNotAfterAddAccessor()
    {
        await VerifyAbsenceAsync(
            """
            class C {
               event Goo Bar { add { } $$
            """);
    }

    [Fact]
    public async Task TestNotInProperty()
    {
        await VerifyAbsenceAsync(
            """
            class C {
               int Goo { $$
            """);
    }
}
