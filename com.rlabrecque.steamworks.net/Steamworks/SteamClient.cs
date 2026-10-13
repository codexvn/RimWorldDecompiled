namespace Steamworks;

public static class SteamClient
{
	public static HSteamPipe CreateSteamPipe()
	{
		InteropHelp.TestIfAvailableClient();
		return (HSteamPipe)NativeMethods.ISteamClient_CreateSteamPipe(CSteamAPIContext.GetSteamClient());
	}

	public static bool BReleaseSteamPipe(HSteamPipe hSteamPipe)
	{
		InteropHelp.TestIfAvailableClient();
		return NativeMethods.ISteamClient_BReleaseSteamPipe(CSteamAPIContext.GetSteamClient(), hSteamPipe);
	}

	public static HSteamUser ConnectToGlobalUser(HSteamPipe hSteamPipe)
	{
		InteropHelp.TestIfAvailableClient();
		return (HSteamUser)NativeMethods.ISteamClient_ConnectToGlobalUser(CSteamAPIContext.GetSteamClient(), hSteamPipe);
	}

	public static HSteamUser CreateLocalUser(out HSteamPipe phSteamPipe, EAccountType eAccountType)
	{
		InteropHelp.TestIfAvailableClient();
		return (HSteamUser)NativeMethods.ISteamClient_CreateLocalUser(CSteamAPIContext.GetSteamClient(), out phSteamPipe, eAccountType);
	}

	public static void ReleaseUser(HSteamPipe hSteamPipe, HSteamUser hUser)
	{
		InteropHelp.TestIfAvailableClient();
		NativeMethods.ISteamClient_ReleaseUser(CSteamAPIContext.GetSteamClient(), hSteamPipe, hUser);
	}

	public static nint GetISteamUser(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableClient();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamUser(CSteamAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamGameServer(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableClient();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamGameServer(CSteamAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, pchVersion2);
	}

	public static void SetLocalIPBinding(ref SteamIPAddress_t unIP, ushort usPort)
	{
		InteropHelp.TestIfAvailableClient();
		NativeMethods.ISteamClient_SetLocalIPBinding(CSteamAPIContext.GetSteamClient(), ref unIP, usPort);
	}

	public static nint GetISteamFriends(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableClient();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamFriends(CSteamAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamUtils(HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableClient();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamUtils(CSteamAPIContext.GetSteamClient(), hSteamPipe, pchVersion2);
	}

	public static nint GetISteamMatchmaking(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableClient();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamMatchmaking(CSteamAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamMatchmakingServers(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableClient();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamMatchmakingServers(CSteamAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamGenericInterface(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableClient();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamGenericInterface(CSteamAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamUserStats(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableClient();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamUserStats(CSteamAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamGameServerStats(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableClient();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamGameServerStats(CSteamAPIContext.GetSteamClient(), hSteamuser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamApps(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableClient();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamApps(CSteamAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamNetworking(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableClient();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamNetworking(CSteamAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamRemoteStorage(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableClient();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamRemoteStorage(CSteamAPIContext.GetSteamClient(), hSteamuser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamScreenshots(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableClient();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamScreenshots(CSteamAPIContext.GetSteamClient(), hSteamuser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamGameSearch(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableClient();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamGameSearch(CSteamAPIContext.GetSteamClient(), hSteamuser, hSteamPipe, pchVersion2);
	}

	public static uint GetIPCCallCount()
	{
		InteropHelp.TestIfAvailableClient();
		return NativeMethods.ISteamClient_GetIPCCallCount(CSteamAPIContext.GetSteamClient());
	}

	public static void SetWarningMessageHook(SteamAPIWarningMessageHook_t pFunction)
	{
		InteropHelp.TestIfAvailableClient();
		NativeMethods.ISteamClient_SetWarningMessageHook(CSteamAPIContext.GetSteamClient(), pFunction);
	}

	public static bool BShutdownIfAllPipesClosed()
	{
		InteropHelp.TestIfAvailableClient();
		return NativeMethods.ISteamClient_BShutdownIfAllPipesClosed(CSteamAPIContext.GetSteamClient());
	}

	public static nint GetISteamHTTP(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableClient();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamHTTP(CSteamAPIContext.GetSteamClient(), hSteamuser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamController(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableClient();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamController(CSteamAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamUGC(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableClient();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamUGC(CSteamAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamAppList(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableClient();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamAppList(CSteamAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamMusic(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableClient();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamMusic(CSteamAPIContext.GetSteamClient(), hSteamuser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamMusicRemote(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableClient();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamMusicRemote(CSteamAPIContext.GetSteamClient(), hSteamuser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamHTMLSurface(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableClient();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamHTMLSurface(CSteamAPIContext.GetSteamClient(), hSteamuser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamInventory(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableClient();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamInventory(CSteamAPIContext.GetSteamClient(), hSteamuser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamVideo(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableClient();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamVideo(CSteamAPIContext.GetSteamClient(), hSteamuser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamParentalSettings(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableClient();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamParentalSettings(CSteamAPIContext.GetSteamClient(), hSteamuser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamInput(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableClient();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamInput(CSteamAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamParties(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableClient();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamParties(CSteamAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamRemotePlay(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableClient();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamRemotePlay(CSteamAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, pchVersion2);
	}
}
