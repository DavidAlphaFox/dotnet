﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading;

namespace Microsoft.CodeAnalysis.Organizing.Organizers;

internal abstract class AbstractSyntaxNodeOrganizer<TSyntaxNode> : ISyntaxOrganizer
    where TSyntaxNode : SyntaxNode
{
    public IEnumerable<Type> SyntaxNodeTypes => [typeof(TSyntaxNode)];

    public SyntaxNode OrganizeNode(SemanticModel semanticModel, SyntaxNode node, CancellationToken cancellationToken)
        => Organize((TSyntaxNode)node, cancellationToken);

    protected abstract TSyntaxNode Organize(TSyntaxNode node, CancellationToken cancellationToken);
}
