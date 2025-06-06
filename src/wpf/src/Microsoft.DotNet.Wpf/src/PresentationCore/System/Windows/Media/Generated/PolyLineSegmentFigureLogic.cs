// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

//
//
// This file was generated, please do not edit it directly.
// 
// This file was generated from the codegen template located at:
//     wpf\src\Graphics\codegen\mcg\generators\PolySegmentTemplate.cs
//
// Please see MilCodeGen.html for more information.
//

using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using System.ComponentModel.Design.Serialization;
using System.Windows.Media.Composition;
using System.Reflection;
using MS.Internal;

namespace System.Windows.Media
{
    #region PolyLineSegment

    /// <summary>
    /// PolyLineSegment
    /// </summary>
    public sealed partial class PolyLineSegment : PathSegment
    {
        #region Constructors
        /// <summary>
        /// PolyLineSegment constructor
        /// </summary>
        public PolyLineSegment()
        {
        }

        /// <summary>
        ///
        /// </summary>
        public PolyLineSegment(IEnumerable<Point> points, bool isStroked)
        {
            ArgumentNullException.ThrowIfNull(points);

            Points = new PointCollection(points);
            IsStroked = isStroked;
        }

        /// <summary>
        ///
        /// </summary>
        internal PolyLineSegment(IEnumerable<Point> points, bool isStroked, bool isSmoothJoin)
        {
            ArgumentNullException.ThrowIfNull(points);

            Points = new PointCollection(points);
            IsStroked = isStroked;
            IsSmoothJoin = isSmoothJoin;
        }

        #endregion

        #region AddToFigure
        internal override void AddToFigure(
            Matrix matrix,          // The transformation matrix
            PathFigure figure,      // The figure to add to
            ref Point current)      // Out: Segment endpoint, not transformed
        {            
            PointCollection points = Points;

            if (points != null  && points.Count >= 1)
            {
                if (matrix.IsIdentity)
                {
                    figure.Segments.Add(this);
                }
                else
                {
                    PointCollection copy = new PointCollection();
                    Point pt = new Point();
                    int count = points.Count;             

                    for (int i=0; i<count; i++)
                    {
                        pt = points.Internal_GetItem(i);
                        pt *= matrix;
                        copy.Add(pt);
                    }

                    figure.Segments.Add(new PolyLineSegment(copy, IsStroked, IsSmoothJoin));
                }
                current = points.Internal_GetItem(points.Count - 1);
            }
        }
        #endregion

        internal override bool IsEmpty()
        {
            return (Points == null) || (Points.Count < 1);
        }

        internal override bool IsCurved()
        {
            return false;
        }

        #region Resource
        /// <summary>
        /// SerializeData - Serialize the contents of this Segment to the provided context.
        /// </summary>
        internal override void SerializeData(StreamGeometryContext ctx)
        {
            ctx.PolyLineTo(Points, IsStroked, IsSmoothJoin);
        }                                    
        #endregion
    }
    #endregion
}
