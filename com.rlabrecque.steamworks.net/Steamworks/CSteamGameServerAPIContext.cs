using System;

namespace Steamworks;

internal static class CSteamGameServerAPIContext
{
	private static nint m_pSteamClient;

	private static nint m_pSteamGameServer;

	private static nint m_pSteamUtils;

	private static nint m_pSteamNetworking;

	private static nint m_pSteamGameServerStats;

	private static nint m_pSteamHTTP;

	private static nint m_pSteamInventory;

	private static nint m_pSteamUGC;

	private static nint m_pSteamNetworkingUtils;

	private static nint m_pSteamNetworkingSockets;

	private static nint m_pSteamNetworkingMessages;

	internal static void Clear()
	{
		m_pSteamClient = IntPtr.Zero;
		m_pSteamGameServer = IntPtr.Zero;
		m_pSteamUtils = IntPtr.Zero;
		m_pSteamNetworking = IntPtr.Zero;
		m_pSteamGameServerStats = IntPtr.Zero;
		m_pSteamHTTP = IntPtr.Zero;
		m_pSteamInventory = IntPtr.Zero;
		m_pSteamUGC = IntPtr.Zero;
		m_pSteamNetworkingUtils = IntPtr.Zero;
		m_pSteamNetworkingSockets = IntPtr.Zero;
		m_pSteamNetworkingMessages = IntPtr.Zero;
	}

	internal static bool Init()
	{
		HSteamUser hSteamUser = GameServer.GetHSteamUser();
		HSteamPipe hSteamPipe = GameServer.GetHSteamPipe();
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
		m_pSteamGameServer = SteamGameServerClient.GetISteamGameServer(hSteamUser, hSteamPipe, "SteamGameServer014");
		if (m_pSteamGameServer == IntPtr.Zero)
		{
			return false;
		}
		m_pSteamUtils = SteamGameServerClient.GetISteamUtils(hSteamPipe, "SteamUtils010");
		if (m_pSteamUtils == IntPtr.Zero)
		{
			return false;
		}
		m_pSteamNetworking = SteamGameServerClient.GetISteamNetworking(hSteamUser, hSteamPipe, "SteamNetworking006");
		if (m_pSteamNetworking == IntPtr.Zero)
		{
			return false;
		}
		m_pSteamGameServerStats = SteamGameServerClient.GetISteamGameServerStats(hSteamUser, hSteamPipe, "SteamGameServerStats001");
		if (m_pSteamGameServerStats == IntPtr.Zero)
		{
			return false;
		}
		m_pSteamHTTP = SteamGameServerClient.GetISteamHTTP(hSteamUser, hSteamPipe, "STEAMHTTP_INTERFACE_VERSION003");
		if (m_pSteamHTTP == IntPtr.Zero)
		{
			return false;
		}
		m_pSteamInventory = SteamGameServerClient.GetISteamInventory(hSteamUser, hSteamPipe, "STEAMINVENTORY_INTERFACE_V003");
		if (m_pSteamInventory == IntPtr.Zero)
		{
			return false;
		}
		m_pSteamUGC = SteamGameServerClient.GetISteamUGC(hSteamUser, hSteamPipe, "STEAMUGC_INTERFACE_VERSION016");
		if (m_pSteamUGC == IntPtr.Zero)
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
			m_pSteamNetworkingSockets = NativeMethods.SteamInternal_FindOrCreateGameServerInterface(hSteamUser, pszVersion2);
		}
		if (m_pSteamNetworkingSockets == IntPtr.Zero)
		{
			return false;
		}
		using (InteropHelp.UTF8StringHandle pszVersion3 = new InteropHelp.UTF8StringHandle("SteamNetworkingMessages002"))
		{
			m_pSteamNetworkingMessages = NativeMethods.SteamInternal_FindOrCreateGameServerInterface(hSteamUser, pszVersion3);
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

	internal static nint GetSteamGameServer()
	{
		return m_pSteamGameServer;
	}

	internal static nint GetSteamUtils()
	{
		return m_pSteamUtils;
	}

	internal static nint GetSteamNetworking()
	{
		return m_pSteamNetworking;
	}

	internal static nint GetSteamGameServerStats()
	{
		return m_pSteamGameServerStats;
	}

	internal static nint GetSteamHTTP()
	{
		return m_pSteamHTTP;
	}

	internal static nint GetSteamInventory()
	{
		return m_pSteamInventory;
	}

	internal static nint GetSteamUGC()
	{
		return m_pSteamUGC;
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
