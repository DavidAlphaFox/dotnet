﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.



#region Using declarations

using System.Windows.Controls;
using System.Collections.Generic;
#if RIBBON_IN_FRAMEWORK
using System.Windows.Controls.Ribbon;

#if RIBBON_IN_FRAMEWORK
namespace System.Windows.Automation.Peers
#else
namespace Microsoft.Windows.Automation.Peers
#endif
{
#else
    using Microsoft.Windows.Controls.Ribbon;
#endif

    #endregion

    public class RibbonGalleryCategoryAutomationPeer : ItemsControlAutomationPeer
    {
        #region constructor
        ///
        public RibbonGalleryCategoryAutomationPeer(RibbonGalleryCategory owner)
            : base(owner)
        {}

        #endregion constructor

        #region AutomationPeer overrides

        ///
        protected override List<AutomationPeer> GetChildrenCore()
        {
            ItemsControl owner = (ItemsControl)Owner;

            if (!owner.IsGrouping)
            {
                return base.GetChildrenCore();
            }

            return null;
        }

        ///
        public override object GetPattern(PatternInterface patternInterface)
        {
            return base.GetPattern(patternInterface);
        }

        ///
        protected override string GetClassNameCore()
        {
            return "RibbonGalleryCategory";
        }

        ///
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Group;
        }

#if !RIBBON_IN_FRAMEWORK
        ///
        override protected bool IsOffscreenCore()
        {
            if (!Owner.IsVisible)
                return true;

            // Borrowed from fix OffScreen fix in 4.0
            Rect boundingRect = RibbonHelper.CalculateVisibleBoundingRect(Owner);
            return (boundingRect == Rect.Empty || boundingRect.Height == 0 || boundingRect.Width == 0);
        }
#endif

        #endregion AutomationPeer overrides

        #region ItemsControl overrides
        ///
        protected override ItemAutomationPeer CreateItemAutomationPeer(object item)
        {
            return new RibbonGalleryItemDataAutomationPeer(item, this, EventsSource as RibbonGalleryCategoryDataAutomationPeer);
        }

        #endregion ItemsControl overrides
    }
}
