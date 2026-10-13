using System.Runtime.InteropServices;

namespace Steamworks;

public static class SteamVideo
{
	public static void GetVideoURL(AppId_t unVideoAppID)
	{
		InteropHelp.TestIfAvailableClient();
		NativeMethods.ISteamVideo_GetVideoURL(CSteamAPIContext.GetSteamVideo(), unVideoAppID);
	}

	public static bool IsBroadcasting(out int pnNumViewers)
	{
		InteropHelp.TestIfAvailableClient();
		return NativeMethods.ISteamVideo_IsBroadcasting(CSteamAPIContext.GetSteamVideo(), out pnNumViewers);
	}

	public static void GetOPFSettings(AppId_t unVideoAppID)
	{
		InteropHelp.TestIfAvailableClient();
		NativeMethods.ISteamVideo_GetOPFSettings(CSteamAPIContext.GetSteamVideo(), unVideoAppID);
	}

	public static bool GetOPFStringForApp(AppId_t unVideoAppID, out string pchBuffer, ref int pnBufferSize)
	{
		InteropHelp.TestIfAvailableClient();
		nint num = Marshal.AllocHGlobal(pnBufferSize);
		bool flag = NativeMethods.ISteamVideo_GetOPFStringForApp(CSteamAPIContext.GetSteamVideo(), unVideoAppID, num, ref pnBufferSize);
		pchBuffer = (flag ? InteropHelp.PtrToStringUTF8(num) : null);
		Marshal.FreeHGlobal(num);
		return flag;
	}
}
