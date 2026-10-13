using System.Runtime.InteropServices;

namespace Steamworks;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct SteamParamStringArray_t
{
	public nint m_ppStrings;

	public int m_nNumStrings;
}
