// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Windows.Xps.Serialization.RCW
{
    /// <summary>
    /// RCW for xpsobjectmodel.idl found in Windows SDK
    /// This is generated code with minor manual edits. 
    /// i.  Generate TLB
    ///      MIDL /TLB xpsobjectmodel.tlb xpsobjectmodel.IDL //xpsobjectmodel.IDL found in Windows SDK
    /// ii. Generate RCW in a DLL
    ///      TLBIMP xpsobjectmodel.tlb // Generates xpsobjectmodel.dll
    /// iii.Decompile the DLL and copy out the RCW by hand.
    ///      ILDASM xpsobjectmodel.dll
    /// </summary>

    [Guid("12759630-5FBA-4283-8F7D-CCA849809EDB"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    internal interface IXpsOMColorProfileResourceCollection
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        uint GetCount();

        [MethodImpl(MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Interface)]
        IXpsOMColorProfileResource GetAt([In] uint index);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void InsertAt([In] uint index, [MarshalAs(UnmanagedType.Interface)] [In] IXpsOMColorProfileResource @object);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void RemoveAt([In] uint index);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void SetAt([In] uint index, [MarshalAs(UnmanagedType.Interface)] [In] IXpsOMColorProfileResource @object);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void Append([MarshalAs(UnmanagedType.Interface)] [In] IXpsOMColorProfileResource @object);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Interface)]
        IXpsOMColorProfileResource GetByPartName([MarshalAs(UnmanagedType.Interface)] [In] IOpcPartUri partName);
    }
}
