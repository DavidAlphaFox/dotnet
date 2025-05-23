// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

namespace System.Windows.Markup
{
#pragma warning disable CA1812 // This type is used inside a TypeConverterAttribute which creates instances of this class.
    internal class StaticExtensionConverter : TypeConverter
#pragma warning restore CA1812
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
            {
                if (!(value is StaticExtension staticExtension))
                {
                    throw new ArgumentException(SR.Format(SR.MustBeOfType, nameof(value), nameof(StaticExtension)));
                }

                return new InstanceDescriptor(
                    typeof(StaticExtension).GetConstructor(new Type[] { typeof(string) }),
                    new object[] { staticExtension.Member }
                );
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
