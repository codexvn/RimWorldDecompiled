using System.Runtime.InteropServices;

namespace Steamworks;

public static class SteamAppList
{
	public static uint GetNumInstalledApps()
	{
		InteropHelp.TestIfAvailableClient();
		return NativeMethods.ISteamAppList_GetNumInstalledApps(CSteamAPIContext.GetSteamAppList());
	}

	public static uint GetInstalledApps(AppId_t[] pvecAppID, uint unMaxAppIDs)
	{
		InteropHelp.TestIfAvailableClient();
		return NativeMethods.ISteamAppList_GetInstalledApps(CSteamAPIContext.GetSteamAppList(), pvecAppID, unMaxAppIDs);
	}

	public static int GetAppName(AppId_t nAppID, out string pchName, int cchNameMax)
	{
		InteropHelp.TestIfAvailableClient();
		nint num = Marshal.AllocHGlobal(cchNameMax);
		int num2 = NativeMethods.ISteamAppList_GetAppName(CSteamAPIContext.GetSteamAppList(), nAppID, num, cchNameMax);
		pchName = ((num2 != -1) ? InteropHelp.PtrToStringUTF8(num) : null);
		Marshal.FreeHGlobal(num);
		return num2;
	}

	public static int GetAppInstallDir(AppId_t nAppID, out string pchDirectory, int cchNameMax)
	{
		InteropHelp.TestIfAvailableClient();
		nint num = Marshal.AllocHGlobal(cchNameMax);
		int num2 = NativeMethods.ISteamAppList_GetAppInstallDir(CSteamAPIContext.GetSteamAppList(), nAppID, num, cchNameMax);
		pchDirectory = ((num2 != -1) ? InteropHelp.PtrToStringUTF8(num) : null);
		Marshal.FreeHGlobal(num);
		return num2;
	}

	public static int GetAppBuildId(AppId_t nAppID)
	{
		InteropHelp.TestIfAvailableClient();
		return NativeMethods.ISteamAppList_GetAppBuildId(CSteamAPIContext.GetSteamAppList(), nAppID);
	}
}
