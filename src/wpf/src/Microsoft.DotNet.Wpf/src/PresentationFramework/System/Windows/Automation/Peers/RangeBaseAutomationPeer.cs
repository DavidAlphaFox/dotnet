﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Windows.Automation.Provider;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
    ///
    public class RangeBaseAutomationPeer : FrameworkElementAutomationPeer, IRangeValueProvider
    {
        ///
        public RangeBaseAutomationPeer(RangeBase owner): base(owner)
        {
        }
    
        ///
        public override object GetPattern(PatternInterface patternInterface)
        {
            if (patternInterface == PatternInterface.RangeValue)
                return this;
            else
                return base.GetPattern(patternInterface);
        }

        // BUG 1555137: Never inline, as we don't want to unnecessarily link the automation DLL
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        internal void RaiseMinimumPropertyChangedEvent(double oldValue, double newValue)
        {
            RaisePropertyChangedEvent(RangeValuePatternIdentifiers.MinimumProperty, oldValue, newValue);
        }

        // BUG 1555137: Never inline, as we don't want to unnecessarily link the automation DLL
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        internal void RaiseMaximumPropertyChangedEvent(double oldValue, double newValue)
        {
            RaisePropertyChangedEvent(RangeValuePatternIdentifiers.MaximumProperty, oldValue, newValue);
        }

        // BUG 1555137: Never inline, as we don't want to unnecessarily link the automation DLL
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        internal void RaiseValuePropertyChangedEvent(double oldValue, double newValue)
        {
            RaisePropertyChangedEvent(RangeValuePatternIdentifiers.ValueProperty, oldValue, newValue);
        }

        /// <summary>
        /// Helper function for IRangeValueProvider.SetValue to provide a way for drive classes to have
        /// custom way of implementing it.
        /// </summary>
        /// <param name="val"></param>
        internal virtual void SetValueCore(double val)
        {
            RangeBase owner = (RangeBase)Owner;
            ArgumentOutOfRangeException.ThrowIfLessThan(val, owner.Minimum);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(val, owner.Maximum);

            owner.Value = (double)val;
        }

        /// <summary>
        /// Request to set the value that this UI element is representing
        /// </summary>
        /// <param name="val">Value to set the UI to, as an object</param>
        /// <returns>true if the UI element was successfully set to the specified value</returns>
        //[CodeAnalysis("AptcaMethodsShouldOnlyCallAptcaMethods")] //Tracking Bug: 29647
        void IRangeValueProvider.SetValue(double val)
        {
            if (!IsEnabled())
                throw new ElementNotEnabledException();

            SetValueCore(val);
        }


        /// <summary>Value of a value control, as an object</summary>
        double IRangeValueProvider.Value
        {
            get
            {
                return ((RangeBase)Owner).Value;
            }
        }

        ///<summary>Indicates that the value can only be read, not modified.
        ///returns True if the control is read-only</summary>
        bool IRangeValueProvider.IsReadOnly
        {
            get
            {
                return !IsEnabled();
            }
        }

        ///<summary>maximum value </summary>
        double IRangeValueProvider.Maximum
        {
            get
            {
                return ((RangeBase)Owner).Maximum;
            }
        }

        ///<summary>minimum value</summary>
        double IRangeValueProvider.Minimum
        {
            get
            {
                return ((RangeBase)Owner).Minimum;
            }
        }

        ///<summary>Value of a Large Change</summary>
        double IRangeValueProvider.LargeChange
        {
            get
            {
                return ((RangeBase)Owner).LargeChange;
            }
        }

        ///<summary>Value of a Small Change</summary>
        double IRangeValueProvider.SmallChange
        {
            get
            {
                return ((RangeBase)Owner).SmallChange;
            }
        }
    }
}

