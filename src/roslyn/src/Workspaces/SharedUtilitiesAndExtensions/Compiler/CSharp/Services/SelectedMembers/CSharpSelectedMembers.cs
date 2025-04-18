﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable disable

using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp.Extensions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.LanguageService;
using Roslyn.Utilities;

namespace Microsoft.CodeAnalysis.CSharp.LanguageService;

internal sealed class CSharpSelectedMembers : AbstractSelectedMembers<
    MemberDeclarationSyntax,
    FieldDeclarationSyntax,
    PropertyDeclarationSyntax,
    TypeDeclarationSyntax,
    VariableDeclaratorSyntax>
{
    public static readonly CSharpSelectedMembers Instance = new();

    private CSharpSelectedMembers()
    {
    }

    protected override ImmutableArray<(SyntaxNode declarator, SyntaxToken identifier)> GetDeclaratorsAndIdentifiers(MemberDeclarationSyntax member)
    {
        return member switch
        {
            FieldDeclarationSyntax fieldDeclaration => fieldDeclaration.Declaration.Variables.SelectAsArray(
                v => (declaration: (SyntaxNode)v, identifier: v.Identifier)),
            EventFieldDeclarationSyntax eventFieldDeclaration => eventFieldDeclaration.Declaration.Variables.SelectAsArray(
                v => (declaration: (SyntaxNode)v, identifier: v.Identifier)),
            _ => [(declaration: (SyntaxNode)member, identifier: member.GetNameToken())],
        };
    }

    protected override SyntaxList<MemberDeclarationSyntax> GetMembers(TypeDeclarationSyntax containingType)
        => containingType.Members;
}
