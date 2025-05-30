// Copyright (c) .NET Foundation and Contributors. All Rights Reserved. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from https://github.com/llvm/llvm-project/tree/llvmorg-20.1.2/clang/tools/libclang
// Original source is Copyright (c) the LLVM Project and Contributors. Licensed under the Apache License v2.0 with LLVM Exceptions. See NOTICE.txt in the project root for license information.

#include "CXString.h"

#pragma warning(push)
#pragma warning(disable : 4146 4244 4267 4291 4624 4996)

#include <llvm/Support/MemAlloc.h>

#pragma warning(pop)

namespace clang::cxstring {
    CXString createDup(llvm::StringRef String) {
        CXString Result;
        char* Spelling = static_cast<char*>(llvm::safe_malloc(String.size() + 1));
        memmove(Spelling, String.data(), String.size());
        Spelling[String.size()] = 0;
        Result.data = Spelling;
        Result.private_flags = (unsigned)CXS_Malloc;
        return Result;
    }

    CXString createEmpty() {
        CXString Str;
        Str.data = "";
        Str.private_flags = CXS_Unmanaged;
        return Str;
    }

    CXString createRef(const char* String) {
        if (String && String[0] == '\0') {
            return createEmpty();
        }

        CXString Str;
        Str.data = String;
        Str.private_flags = CXS_Unmanaged;
        return Str;
    }
}
