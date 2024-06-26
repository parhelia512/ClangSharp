// Copyright (c) .NET Foundation and Contributors. All Rights Reserved. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace ClangSharp.Interop;

public unsafe partial struct CXDiagnosticSet(IntPtr handle) : IDisposable, IEquatable<CXDiagnosticSet>, IReadOnlyCollection<CXDiagnostic>
{
    public static CXDiagnosticSet Load(string file, out CXLoadDiag_Error error, out CXString errorString)
    {
        using var marshaledFile = new MarshaledString(file);

        fixed (CXLoadDiag_Error* pError = &error)
        fixed (CXString* pErrorString = &errorString)
        {
            return (CXDiagnosticSet)clang.loadDiagnostics(marshaledFile, pError, pErrorString);
        }
    }

    public readonly CXDiagnostic this[uint index] => GetDiagnostic(index);

    public readonly int Count => (int)NumDiagnostics;

    public IntPtr Handle { get; set; } = handle;

    public readonly uint NumDiagnostics => clang.getNumDiagnosticsInSet(this);

    public static explicit operator CXDiagnosticSet(void* value) => new CXDiagnosticSet((IntPtr)value);

    public static implicit operator void*(CXDiagnosticSet value) => (void*)value.Handle;

    public static bool operator ==(CXDiagnosticSet left, CXDiagnosticSet right) => left.Handle == right.Handle;

    public static bool operator !=(CXDiagnosticSet left, CXDiagnosticSet right) => left.Handle != right.Handle;

    public void Dispose()
    {
        if (Handle != IntPtr.Zero)
        {
            clang.disposeDiagnosticSet(this);
            Handle = IntPtr.Zero;
        }
    }

    public override readonly bool Equals(object? obj) => (obj is CXDiagnosticSet other) && Equals(other);

    public readonly bool Equals(CXDiagnosticSet other) => this == other;

    public readonly CXDiagnostic GetDiagnostic(uint index) => (CXDiagnostic)clang.getDiagnosticInSet(this, index);

    public readonly IEnumerator<CXDiagnostic> GetEnumerator()
    {
        var count = NumDiagnostics;

        for (var index = 0u; index < count; index++)
        {
            yield return GetDiagnostic(index);
        }
    }

    readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public override readonly int GetHashCode() => Handle.GetHashCode();
}
