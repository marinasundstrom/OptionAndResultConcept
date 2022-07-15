using System;
using System.Runtime.InteropServices;

namespace ResultAndOption;

[StructLayout(LayoutKind.Auto, Pack = 0, Size = 0)]
public struct Unit
{
    public static readonly Unit Value = new Unit();
}

