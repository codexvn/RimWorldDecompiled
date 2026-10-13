using System;

namespace Steamworks;

internal static class CSteamAPIContext
{
	private static nint m_pSteamClient;

	private static nint m_pSteamUser;

	private static nint m_pSteamFriends;

	private static nint m_pSteamUtils;

	private static nint m_pSteamMatchmaking;

	private static nint m_pSteamUserStats;

	private static nint m_pSteamApps;

	private static nint m_pSteamMatchmakingServers;

	private static nint m_pSteamNetworking;

	private static nint m_pSteamRemoteStorage;

	private static nint m_pSteamScreenshots;

	private static nint m_pSteamGameSearch;

	private static nint m_pSteamHTTP;

	private static nint m_pController;

	private static nint m_pSteamUGC;

	private static nint m_pSteamAppList;

	private static nint m_pSteamMusic;

	private static nint m_pSteamMusicRemote;

	private static nint m_pSteamHTMLSurface;

	private static nint m_pSteamInventory;

	private static nint m_pSteamVideo;

	private static nint m_pSteamParentalSettings;

	private static nint m_pSteamInput;

	private static nint m_pSteamParties;

	private static nint m_pSteamRemotePlay;

	private static nint m_pSteamNetworkingUtils;

	private static nint m_pSteamNetworkingSockets;

	private static nint m_pSteamNetworkingMessages;

	internal static void Clear()
	{
		m_pSteamClient = IntPtr.Zero;
		m_pSteamUser = IntPtr.Zero;
		m_pSteamFriends = IntPtr.Zero;
		m_pSteamUtils = IntPtr.Zero;
		m_pSteamMatchmaking = IntPtr.Zero;
		m_pSteamUserStats = IntPtr.Zero;
		m_pSteamApps = IntPtr.Zero;
		m_pSteamMatchmakingServers = IntPtr.Zero;
		m_pSteamNetworking = IntPtr.Zero;
		m_pSteamRemoteStorage = IntPtr.Zero;
		m_pSteamHTTP = IntPtr.Zero;
		m_pSteamScreenshots = IntPtr.Zero;
		m_pSteamGameSearch = IntPtr.Zero;
		m_pSteamMusic = IntPtr.Zero;
		m_pController = IntPtr.Zero;
		m_pSteamUGC = IntPtr.Zero;
		m_pSteamAppList = IntPtr.Zero;
		m_pSteamMusic = IntPtr.Zero;
		m_pSteamMusicRemote = IntPtr.Zero;
		m_pSteamHTMLSurface = IntPtr.Zero;
		m_pSteamInventory = IntPtr.Zero;
		m_pSteamVideo = IntPtr.Zero;
		m_pSteamParentalSettings = IntPtr.Zero;
		m_pSteamInput = IntPtr.Zero;
		m_pSteamParties = IntPtr.Zero;
		m_pSteamRemotePlay = IntPtr.Zero;
		m_pSteamNetworkingUtils = IntPtr.Zero;
		m_pSteamNetworkingSockets = IntPtr.Zero;
		m_pSteamNetworkingMessages = IntPtr.Zero;
	}

	internal static bool Init()
	{
		HSteamUser hSteamUser = SteamAPI.GetHSteamUser();
		HSteamPipe hSteamPipe = SteamAPI.GetHSteamPipe();
		if (hSteamPipe == (HSteamPipe)0)
		{
			return false;
		}
		using (InteropHelp.UTF8StringHandle ver = new InteropHelp.UTF8StringHandle("SteamClient020"))
		{
			m_pSteamClient = NativeMethods.SteamInternal_CreateInterface(ver);
		}
		if (m_pSteamClient == IntPtr.Zero)
		{
			return false;
		}
		m_pSteamUser = SteamClient.GetISteamUser(hSteamUser, hSteamPipe, "SteamUser021");
		if (m_pSteamUser == IntPtr.Zero)
		{
			return false;
		}
		m_pSteamFriends = SteamClient.GetISteamFriends(hSteamUser, hSteamPipe, "SteamFriends017");
		if (m_pSteamFriends == IntPtr.Zero)
		{
			return false;
		}
		m_pSteamUtils = SteamClient.GetISteamUtils(hSteamPipe, "SteamUtils010");
		if (m_pSteamUtils == IntPtr.Zero)
		{
			return false;
		}
		m_pSteamMatchmaking = SteamClient.GetISteamMatchmaking(hSteamUser, hSteamPipe, "SteamMatchMaking009");
		if (m_pSteamMatchmaking == IntPtr.Zero)
		{
			return false;
		}
		m_pSteamMatchmakingServers = SteamClient.GetISteamMatchmakingServers(hSteamUser, hSteamPipe, "SteamMatchMakingServers002");
		if (m_pSteamMatchmakingServers == IntPtr.Zero)
		{
			return false;
		}
		m_pSteamUserStats = SteamClient.GetISteamUserStats(hSteamUser, hSteamPipe, "STEAMUSERSTATS_INTERFACE_VERSION012");
		if (m_pSteamUserStats == IntPtr.Zero)
		{
			return false;
		}
		m_pSteamApps = SteamClient.GetISteamApps(hSteamUser, hSteamPipe, "STEAMAPPS_INTERFACE_VERSION008");
		if (m_pSteamApps == IntPtr.Zero)
		{
			return false;
		}
		m_pSteamNetworking = SteamClient.GetISteamNetworking(hSteamUser, hSteamPipe, "SteamNetworking006");
		if (m_pSteamNetworking == IntPtr.Zero)
		{
			return false;
		}
		m_pSteamRemoteStorage = SteamClient.GetISteamRemoteStorage(hSteamUser, hSteamPipe, "STEAMREMOTESTORAGE_INTERFACE_VERSION016");
		if (m_pSteamRemoteStorage == IntPtr.Zero)
		{
			return false;
		}
		m_pSteamScreenshots = SteamClient.GetISteamScreenshots(hSteamUser, hSteamPipe, "STEAMSCREENSHOTS_INTERFACE_VERSION003");
		if (m_pSteamScreenshots == IntPtr.Zero)
		{
			return false;
		}
		m_pSteamGameSearch = SteamClient.GetISteamGameSearch(hSteamUser, hSteamPipe, "SteamMatchGameSearch001");
		if (m_pSteamGameSearch == IntPtr.Zero)
		{
			return false;
		}
		m_pSteamHTTP = SteamClient.GetISteamHTTP(hSteamUser, hSteamPipe, "STEAMHTTP_INTERFACE_VERSION003");
		if (m_pSteamHTTP == IntPtr.Zero)
		{
			return false;
		}
		m_pSteamUGC = SteamClient.GetISteamUGC(hSteamUser, hSteamPipe, "STEAMUGC_INTERFACE_VERSION016");
		if (m_pSteamUGC == IntPtr.Zero)
		{
			return false;
		}
		m_pSteamAppList = SteamClient.GetISteamAppList(hSteamUser, hSteamPipe, "STEAMAPPLIST_INTERFACE_VERSION001");
		if (m_pSteamAppList == IntPtr.Zero)
		{
			return false;
		}
		m_pSteamMusic = SteamClient.GetISteamMusic(hSteamUser, hSteamPipe, "STEAMMUSIC_INTERFACE_VERSION001");
		if (m_pSteamMusic == IntPtr.Zero)
		{
			return false;
		}
		m_pSteamMusicRemote = SteamClient.GetISteamMusicRemote(hSteamUser, hSteamPipe, "STEAMMUSICREMOTE_INTERFACE_VERSION001");
		if (m_pSteamMusicRemote == IntPtr.Zero)
		{
			return false;
		}
		m_pSteamHTMLSurface = SteamClient.GetISteamHTMLSurface(hSteamUser, hSteamPipe, "STEAMHTMLSURFACE_INTERFACE_VERSION_005");
		if (m_pSteamHTMLSurface == IntPtr.Zero)
		{
			return false;
		}
		m_pSteamInventory = SteamClient.GetISteamInventory(hSteamUser, hSteamPipe, "STEAMINVENTORY_INTERFACE_V003");
		if (m_pSteamInventory == IntPtr.Zero)
		{
			return false;
		}
		m_pSteamVideo = SteamClient.GetISteamVideo(hSteamUser, hSteamPipe, "STEAMVIDEO_INTERFACE_V002");
		if (m_pSteamVideo == IntPtr.Zero)
		{
			return false;
		}
		m_pSteamParentalSettings = SteamClient.GetISteamParentalSettings(hSteamUser, hSteamPipe, "STEAMPARENTALSETTINGS_INTERFACE_VERSION001");
		if (m_pSteamParentalSettings == IntPtr.Zero)
		{
			return false;
		}
		m_pSteamInput = SteamClient.GetISteamInput(hSteamUser, hSteamPipe, "SteamInput006");
		if (m_pSteamInput == IntPtr.Zero)
		{
			return false;
		}
		m_pSteamParties = SteamClient.GetISteamParties(hSteamUser, hSteamPipe, "SteamParties002");
		if (m_pSteamParties == IntPtr.Zero)
		{
			return false;
		}
		m_pSteamRemotePlay = SteamClient.GetISteamRemotePlay(hSteamUser, hSteamPipe, "STEAMREMOTEPLAY_INTERFACE_VERSION001");
		if (m_pSteamRemotePlay == IntPtr.Zero)
		{
			return false;
		}
		using (InteropHelp.UTF8StringHandle pszVersion = new InteropHelp.UTF8StringHandle("SteamNetworkingUtils004"))
		{
			m_pSteamNetworkingUtils = ((NativeMethods.SteamInternal_FindOrCreateUserInterface(hSteamUser, pszVersion) != IntPtr.Zero) ? NativeMethods.SteamInternal_FindOrCreateUserInterface(hSteamUser, pszVersion) : NativeMethods.SteamInternal_FindOrCreateGameServerInterface(hSteamUser, pszVersion));
		}
		if (m_pSteamNetworkingUtils == IntPtr.Zero)
		{
			return false;
		}
		using (InteropHelp.UTF8StringHandle pszVersion2 = new InteropHelp.UTF8StringHandle("SteamNetworkingSockets012"))
		{
			m_pSteamNetworkingSockets = NativeMethods.SteamInternal_FindOrCreateUserInterface(hSteamUser, pszVersion2);
		}
		if (m_pSteamNetworkingSockets == IntPtr.Zero)
		{
			return false;
		}
		using (InteropHelp.UTF8StringHandle pszVersion3 = new InteropHelp.UTF8StringHandle("SteamNetworkingMessages002"))
		{
			m_pSteamNetworkingMessages = NativeMethods.SteamInternal_FindOrCreateUserInterface(hSteamUser, pszVersion3);
		}
		if (m_pSteamNetworkingMessages == IntPtr.Zero)
		{
			return false;
		}
		return true;
	}

	internal static nint GetSteamClient()
	{
		return m_pSteamClient;
	}

	internal static nint GetSteamUser()
	{
		return m_pSteamUser;
	}

	internal static nint GetSteamFriends()
	{
		return m_pSteamFriends;
	}

	internal static nint GetSteamUtils()
	{
		return m_pSteamUtils;
	}

	internal static nint GetSteamMatchmaking()
	{
		return m_pSteamMatchmaking;
	}

	internal static nint GetSteamUserStats()
	{
		return m_pSteamUserStats;
	}

	internal static nint GetSteamApps()
	{
		return m_pSteamApps;
	}

	internal static nint GetSteamMatchmakingServers()
	{
		return m_pSteamMatchmakingServers;
	}

	internal static nint GetSteamNetworking()
	{
		return m_pSteamNetworking;
	}

	internal static nint GetSteamRemoteStorage()
	{
		return m_pSteamRemoteStorage;
	}

	internal static nint GetSteamScreenshots()
	{
		return m_pSteamScreenshots;
	}

	internal static nint GetSteamGameSearch()
	{
		return m_pSteamGameSearch;
	}

	internal static nint GetSteamHTTP()
	{
		return m_pSteamHTTP;
	}

	internal static nint GetSteamController()
	{
		return m_pController;
	}

	internal static nint GetSteamUGC()
	{
		return m_pSteamUGC;
	}

	internal static nint GetSteamAppList()
	{
		return m_pSteamAppList;
	}

	internal static nint GetSteamMusic()
	{
		return m_pSteamMusic;
	}

	internal static nint GetSteamMusicRemote()
	{
		return m_pSteamMusicRemote;
	}

	internal static nint GetSteamHTMLSurface()
	{
		return m_pSteamHTMLSurface;
	}

	internal static nint GetSteamInventory()
	{
		return m_pSteamInventory;
	}

	internal static nint GetSteamVideo()
	{
		return m_pSteamVideo;
	}

	internal static nint GetSteamParentalSettings()
	{
		return m_pSteamParentalSettings;
	}

	internal static nint GetSteamInput()
	{
		return m_pSteamInput;
	}

	internal static nint GetSteamParties()
	{
		return m_pSteamParties;
	}

	internal static nint GetSteamRemotePlay()
	{
		return m_pSteamRemotePlay;
	}

	internal static nint GetSteamNetworkingUtils()
	{
		return m_pSteamNetworkingUtils;
	}

	internal static nint GetSteamNetworkingSockets()
	{
		return m_pSteamNetworkingSockets;
	}

	internal static nint GetSteamNetworkingMessages()
	{
		return m_pSteamNetworkingMessages;
	}
}
