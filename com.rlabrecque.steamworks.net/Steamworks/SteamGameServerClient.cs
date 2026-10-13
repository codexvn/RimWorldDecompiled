namespace Steamworks;

public static class SteamGameServerClient
{
	public static HSteamPipe CreateSteamPipe()
	{
		InteropHelp.TestIfAvailableGameServer();
		return (HSteamPipe)NativeMethods.ISteamClient_CreateSteamPipe(CSteamGameServerAPIContext.GetSteamClient());
	}

	public static bool BReleaseSteamPipe(HSteamPipe hSteamPipe)
	{
		InteropHelp.TestIfAvailableGameServer();
		return NativeMethods.ISteamClient_BReleaseSteamPipe(CSteamGameServerAPIContext.GetSteamClient(), hSteamPipe);
	}

	public static HSteamUser ConnectToGlobalUser(HSteamPipe hSteamPipe)
	{
		InteropHelp.TestIfAvailableGameServer();
		return (HSteamUser)NativeMethods.ISteamClient_ConnectToGlobalUser(CSteamGameServerAPIContext.GetSteamClient(), hSteamPipe);
	}

	public static HSteamUser CreateLocalUser(out HSteamPipe phSteamPipe, EAccountType eAccountType)
	{
		InteropHelp.TestIfAvailableGameServer();
		return (HSteamUser)NativeMethods.ISteamClient_CreateLocalUser(CSteamGameServerAPIContext.GetSteamClient(), out phSteamPipe, eAccountType);
	}

	public static void ReleaseUser(HSteamPipe hSteamPipe, HSteamUser hUser)
	{
		InteropHelp.TestIfAvailableGameServer();
		NativeMethods.ISteamClient_ReleaseUser(CSteamGameServerAPIContext.GetSteamClient(), hSteamPipe, hUser);
	}

	public static nint GetISteamUser(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableGameServer();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamUser(CSteamGameServerAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamGameServer(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableGameServer();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamGameServer(CSteamGameServerAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, pchVersion2);
	}

	public static void SetLocalIPBinding(ref SteamIPAddress_t unIP, ushort usPort)
	{
		InteropHelp.TestIfAvailableGameServer();
		NativeMethods.ISteamClient_SetLocalIPBinding(CSteamGameServerAPIContext.GetSteamClient(), ref unIP, usPort);
	}

	public static nint GetISteamFriends(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableGameServer();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamFriends(CSteamGameServerAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamUtils(HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableGameServer();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamUtils(CSteamGameServerAPIContext.GetSteamClient(), hSteamPipe, pchVersion2);
	}

	public static nint GetISteamMatchmaking(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableGameServer();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamMatchmaking(CSteamGameServerAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamMatchmakingServers(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableGameServer();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamMatchmakingServers(CSteamGameServerAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamGenericInterface(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableGameServer();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamGenericInterface(CSteamGameServerAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamUserStats(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableGameServer();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamUserStats(CSteamGameServerAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamGameServerStats(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableGameServer();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamGameServerStats(CSteamGameServerAPIContext.GetSteamClient(), hSteamuser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamApps(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableGameServer();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamApps(CSteamGameServerAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamNetworking(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableGameServer();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamNetworking(CSteamGameServerAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamRemoteStorage(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableGameServer();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamRemoteStorage(CSteamGameServerAPIContext.GetSteamClient(), hSteamuser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamScreenshots(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableGameServer();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamScreenshots(CSteamGameServerAPIContext.GetSteamClient(), hSteamuser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamGameSearch(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableGameServer();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamGameSearch(CSteamGameServerAPIContext.GetSteamClient(), hSteamuser, hSteamPipe, pchVersion2);
	}

	public static uint GetIPCCallCount()
	{
		InteropHelp.TestIfAvailableGameServer();
		return NativeMethods.ISteamClient_GetIPCCallCount(CSteamGameServerAPIContext.GetSteamClient());
	}

	public static void SetWarningMessageHook(SteamAPIWarningMessageHook_t pFunction)
	{
		InteropHelp.TestIfAvailableGameServer();
		NativeMethods.ISteamClient_SetWarningMessageHook(CSteamGameServerAPIContext.GetSteamClient(), pFunction);
	}

	public static bool BShutdownIfAllPipesClosed()
	{
		InteropHelp.TestIfAvailableGameServer();
		return NativeMethods.ISteamClient_BShutdownIfAllPipesClosed(CSteamGameServerAPIContext.GetSteamClient());
	}

	public static nint GetISteamHTTP(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableGameServer();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamHTTP(CSteamGameServerAPIContext.GetSteamClient(), hSteamuser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamController(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableGameServer();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamController(CSteamGameServerAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamUGC(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableGameServer();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamUGC(CSteamGameServerAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamAppList(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableGameServer();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamAppList(CSteamGameServerAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamMusic(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableGameServer();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamMusic(CSteamGameServerAPIContext.GetSteamClient(), hSteamuser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamMusicRemote(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableGameServer();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamMusicRemote(CSteamGameServerAPIContext.GetSteamClient(), hSteamuser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamHTMLSurface(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableGameServer();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamHTMLSurface(CSteamGameServerAPIContext.GetSteamClient(), hSteamuser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamInventory(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableGameServer();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamInventory(CSteamGameServerAPIContext.GetSteamClient(), hSteamuser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamVideo(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableGameServer();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamVideo(CSteamGameServerAPIContext.GetSteamClient(), hSteamuser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamParentalSettings(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableGameServer();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamParentalSettings(CSteamGameServerAPIContext.GetSteamClient(), hSteamuser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamInput(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableGameServer();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamInput(CSteamGameServerAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamParties(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableGameServer();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamParties(CSteamGameServerAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, pchVersion2);
	}

	public static nint GetISteamRemotePlay(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
	{
		InteropHelp.TestIfAvailableGameServer();
		using InteropHelp.UTF8StringHandle pchVersion2 = new InteropHelp.UTF8StringHandle(pchVersion);
		return NativeMethods.ISteamClient_GetISteamRemotePlay(CSteamGameServerAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, pchVersion2);
	}
}
