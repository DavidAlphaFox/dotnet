// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#include "AsmMacros_Shared.h"

#ifdef FEATURE_CACHED_INTERFACE_DISPATCH

    // Macro that generates code to check a single cache entry.
    .macro CHECK_CACHE_ENTRY entry
        // Check a single entry in the cache.
        //  x9   : Cache data structure. Also used for target address jump.
        //  x10  : Instance MethodTable*
        //  x11  : Indirection cell address, preserved
        //  x12  : Trashed
        ldr     x12, [x9, #(OFFSETOF__InterfaceDispatchCache__m_rgEntries + (\entry * 16))]
        cmp     x10, x12
        bne     0f
        ldr     x9, [x9, #(OFFSETOF__InterfaceDispatchCache__m_rgEntries + (\entry * 16) + 8)]
        br      x9
0:
    .endm

//
// Macro that generates a stub consuming a cache with the given number of entries.
//
    .macro DEFINE_INTERFACE_DISPATCH_STUB entries

    NESTED_ENTRY "RhpInterfaceDispatch\entries", _TEXT, NoHandler

        // x11 holds the indirection cell address. Load the cache pointer.
        ldr     x9, [x11, #OFFSETOF__InterfaceDispatchCell__m_pCache]

        // Load the MethodTable from the object instance in x0.
        ALTERNATE_ENTRY RhpInterfaceDispatchAVLocation\entries
        ldr     x10, [x0]

    .global CurrentEntry
    .set CurrentEntry, 0

    .rept \entries
        CHECK_CACHE_ENTRY CurrentEntry
        .set CurrentEntry, CurrentEntry + 1
    .endr

        // x11 still contains the indirection cell address.
        b C_FUNC(RhpInterfaceDispatchSlow)

    NESTED_END "RhpInterfaceDispatch\entries", _TEXT

    .endm

//
// Define all the stub routines we currently need.
//
// If you change or add any new dispatch stubs, exception handling might need to be aware because it refers to the
// *AVLocation symbols defined by the dispatch stubs to be able to unwind and blame user code if a NullRef happens
// during the interface dispatch.
//
    DEFINE_INTERFACE_DISPATCH_STUB 1
    DEFINE_INTERFACE_DISPATCH_STUB 2
    DEFINE_INTERFACE_DISPATCH_STUB 4
    DEFINE_INTERFACE_DISPATCH_STUB 8
    DEFINE_INTERFACE_DISPATCH_STUB 16
    DEFINE_INTERFACE_DISPATCH_STUB 32
    DEFINE_INTERFACE_DISPATCH_STUB 64

//
// Initial dispatch on an interface when we don't have a cache yet.
//
    LEAF_ENTRY RhpInitialInterfaceDispatch, _TEXT
    ALTERNATE_ENTRY RhpInitialDynamicInterfaceDispatch
        // Trigger an AV if we're dispatching on a null this.
        // The exception handling infrastructure is aware of the fact that this is the first
        // instruction of RhpInitialInterfaceDispatch and uses it to translate an AV here
        // to a NullReferenceException at the callsite.
        ldr     xzr, [x0]

        // Just tail call to the cache miss helper.
        b C_FUNC(RhpInterfaceDispatchSlow)
    LEAF_END RhpInitialInterfaceDispatch, _TEXT

#endif // FEATURE_CACHED_INTERFACE_DISPATCH
