﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using MS.Internal; // Invariant.Assert
using System.ComponentModel;

// 
// Description: Helpers for TOM parameter validation.
//

namespace System.Windows.Documents
{
    internal static class ValidationHelper
    {
        //------------------------------------------------------
        //
        //  Internal Methods
        //
        //------------------------------------------------------

        #region Internal Methods

        // Verifies a TextPointer is non-null and
        // is associated with a given TextContainer.
        //
        // Throws an appropriate exception if a test fails.
        internal static void VerifyPosition(ITextContainer tree, ITextPointer position)
        {
            VerifyPosition(tree, position, nameof(position));
        }
        
        // Verifies a TextPointer is non-null and is associated with a given TextContainer.
        //
        // Throws an appropriate exception if a test fails.
        internal static void VerifyPosition(ITextContainer container, ITextPointer position, string paramName)
        {
            if (position == null)
            {
                throw new ArgumentNullException(paramName);
            }

            if (position.TextContainer != container)
            {
                throw new ArgumentException(SR.Format(SR.NotInAssociatedTree, paramName));
            }
        }

        // Verifies two positions are safe to use as a logical text span.
        //
        // Throws ArgumentNullException if startPosition == null || endPosition == null
        //        ArgumentException if startPosition.TextContainer != endPosition.TextContainer or
        //                             startPosition > endPosition
        internal static void VerifyPositionPair(ITextPointer startPosition, ITextPointer endPosition)
        {
            ArgumentNullException.ThrowIfNull(startPosition);
            ArgumentNullException.ThrowIfNull(endPosition);
            if (startPosition.TextContainer != endPosition.TextContainer)
            {
                throw new ArgumentException(SR.Format(SR.InDifferentTextContainers, "startPosition", "endPosition"));
            }
            if (startPosition.CompareTo(endPosition) > 0)
            {
                throw new ArgumentException(SR.Format(SR.BadTextPositionOrder, "startPosition", "endPosition"));
            }
        }

        // Throws an ArgumentException if direction is not a valid enum.
        internal static void VerifyDirection(LogicalDirection direction, string argumentName)
        {
            if (direction != LogicalDirection.Forward &&
                direction != LogicalDirection.Backward)
            {
                throw new InvalidEnumArgumentException(argumentName, (int)direction, typeof(LogicalDirection));
            }
        }

        // Throws an ArgumentException if edge is not a valid enum.
        internal static void VerifyElementEdge(ElementEdge edge, string param)
        {
            if (edge != ElementEdge.BeforeStart &&
                edge != ElementEdge.AfterStart  &&
                edge != ElementEdge.BeforeEnd   &&
                edge != ElementEdge.AfterEnd)
            {
                throw new InvalidEnumArgumentException(param, (int)edge, typeof(ElementEdge));
            }
        }

        // ...............................................................
        //
        // TextSchema Validation
        //
        // ...............................................................

        // Checks whether it is valid to insert the child object at passed position.
        internal static void ValidateChild(TextPointer position, object child, string paramName)
        {
            Invariant.Assert(position != null);

            if (child == null)
            {
                throw new ArgumentNullException(paramName);
            }

            if (!TextSchema.IsValidChild(/*position:*/position, /*childType:*/child.GetType()))
            {
                throw new ArgumentException(SR.Format(SR.TextSchema_ChildTypeIsInvalid, position.Parent.GetType().Name, child.GetType().Name));
            }

            // The new child should not be currently in other text tree
            if (child is TextElement)
            {
                if (((TextElement)child).Parent != null)
                {
                    throw new ArgumentException(SR.Format(SR.TextSchema_TheChildElementBelongsToAnotherTreeAlready, child.GetType().Name));
                }
            }
            else
            {
                Invariant.Assert(child is UIElement);
                // Cannot call UIElement.Parent across assembly boundary. So skip this part of validation. This condition will be checked elsewhere anyway.
                //if (((UIElement)child).Parent != null)
                //{
                //    throw new ArgumentException(SR.Format(SR.TextSchema_TheChildElementBelongsToAnotherTreeAlready, child.GetType().Name));
                //}
            }
        }

        #endregion Internal methods
    }
}
