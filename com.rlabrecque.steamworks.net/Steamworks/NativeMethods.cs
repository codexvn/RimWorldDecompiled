using System.Runtime.InteropServices;
using System.Security;

namespace Steamworks;

[SuppressUnmanagedCodeSecurity]
internal static class NativeMethods
{
	internal const string NativeLibraryName = "steam_api64";

	internal const string NativeLibrary_SDKEncryptedAppTicket = "sdkencryptedappticket64";

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool SteamAPI_Init();

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern void SteamAPI_Shutdown();

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool SteamAPI_RestartAppIfNecessary(AppId_t unOwnAppID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern void SteamAPI_ReleaseCurrentThreadMemory();

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern void SteamAPI_WriteMiniDump(uint uStructuredExceptionCode, nint pvExceptionInfo, uint uBuildID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern void SteamAPI_SetMiniDumpComment(InteropHelp.UTF8StringHandle pchMsg);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern void SteamAPI_RunCallbacks();

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern void SteamAPI_RegisterCallback(nint pCallback, int iCallback);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern void SteamAPI_UnregisterCallback(nint pCallback);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern void SteamAPI_RegisterCallResult(nint pCallback, ulong hAPICall);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern void SteamAPI_UnregisterCallResult(nint pCallback, ulong hAPICall);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool SteamAPI_IsSteamRunning();

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern int SteamAPI_GetSteamInstallPath();

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern int SteamAPI_GetHSteamPipe();

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern void SteamAPI_SetTryCatchCallbacks([MarshalAs(UnmanagedType.I1)] bool bTryCatchCallbacks);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern int SteamAPI_GetHSteamUser();

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern nint SteamInternal_ContextInit(nint pContextInitData);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern nint SteamInternal_CreateInterface(InteropHelp.UTF8StringHandle ver);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern nint SteamInternal_FindOrCreateUserInterface(HSteamUser hSteamUser, InteropHelp.UTF8StringHandle pszVersion);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern nint SteamInternal_FindOrCreateGameServerInterface(HSteamUser hSteamUser, InteropHelp.UTF8StringHandle pszVersion);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern void SteamAPI_UseBreakpadCrashHandler(InteropHelp.UTF8StringHandle pchVersion, InteropHelp.UTF8StringHandle pchDate, InteropHelp.UTF8StringHandle pchTime, [MarshalAs(UnmanagedType.I1)] bool bFullMemoryDumps, nint pvContext, nint m_pfnPreMinidumpCallback);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern void SteamAPI_SetBreakpadAppID(uint unAppID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern void SteamAPI_ManualDispatch_Init();

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern void SteamAPI_ManualDispatch_RunFrame(HSteamPipe hSteamPipe);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool SteamAPI_ManualDispatch_GetNextCallback(HSteamPipe hSteamPipe, nint pCallbackMsg);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern void SteamAPI_ManualDispatch_FreeLastCallback(HSteamPipe hSteamPipe);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool SteamAPI_ManualDispatch_GetAPICallResult(HSteamPipe hSteamPipe, SteamAPICall_t hSteamAPICall, nint pCallback, int cubCallback, int iCallbackExpected, out bool pbFailed);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern void SteamGameServer_Shutdown();

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern void SteamGameServer_RunCallbacks();

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern void SteamGameServer_ReleaseCurrentThreadMemory();

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool SteamGameServer_BSecure();

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern ulong SteamGameServer_GetSteamID();

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern int SteamGameServer_GetHSteamPipe();

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern int SteamGameServer_GetHSteamUser();

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool SteamInternal_GameServer_Init(uint unIP, ushort usPort, ushort usGamePort, ushort usQueryPort, EServerMode eServerMode, InteropHelp.UTF8StringHandle pchVersionString);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern nint SteamClient();

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern nint SteamGameServerClient();

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern void SteamAPI_SteamNetworkingIPAddr_Clear(ref SteamNetworkingIPAddr self);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool SteamAPI_SteamNetworkingIPAddr_IsIPv6AllZeros(ref SteamNetworkingIPAddr self);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern void SteamAPI_SteamNetworkingIPAddr_SetIPv6(ref SteamNetworkingIPAddr self, [In][Out] byte[] ipv6, ushort nPort);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern void SteamAPI_SteamNetworkingIPAddr_SetIPv4(ref SteamNetworkingIPAddr self, uint nIP, ushort nPort);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool SteamAPI_SteamNetworkingIPAddr_IsIPv4(ref SteamNetworkingIPAddr self);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern uint SteamAPI_SteamNetworkingIPAddr_GetIPv4(ref SteamNetworkingIPAddr self);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern void SteamAPI_SteamNetworkingIPAddr_SetIPv6LocalHost(ref SteamNetworkingIPAddr self, ushort nPort);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool SteamAPI_SteamNetworkingIPAddr_IsLocalHost(ref SteamNetworkingIPAddr self);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern void SteamAPI_SteamNetworkingIPAddr_ToString(ref SteamNetworkingIPAddr self, nint buf, uint cbBuf, [MarshalAs(UnmanagedType.I1)] bool bWithPort);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool SteamAPI_SteamNetworkingIPAddr_ParseString(ref SteamNetworkingIPAddr self, InteropHelp.UTF8StringHandle pszStr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool SteamAPI_SteamNetworkingIPAddr_IsEqualTo(ref SteamNetworkingIPAddr self, ref SteamNetworkingIPAddr x);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern ESteamNetworkingFakeIPType SteamAPI_SteamNetworkingIPAddr_GetFakeIPType(ref SteamNetworkingIPAddr self);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool SteamAPI_SteamNetworkingIPAddr_IsFakeIP(ref SteamNetworkingIPAddr self);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern void SteamAPI_SteamNetworkingIdentity_Clear(ref SteamNetworkingIdentity self);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool SteamAPI_SteamNetworkingIdentity_IsInvalid(ref SteamNetworkingIdentity self);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern void SteamAPI_SteamNetworkingIdentity_SetSteamID(ref SteamNetworkingIdentity self, ulong steamID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern ulong SteamAPI_SteamNetworkingIdentity_GetSteamID(ref SteamNetworkingIdentity self);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern void SteamAPI_SteamNetworkingIdentity_SetSteamID64(ref SteamNetworkingIdentity self, ulong steamID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern ulong SteamAPI_SteamNetworkingIdentity_GetSteamID64(ref SteamNetworkingIdentity self);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool SteamAPI_SteamNetworkingIdentity_SetXboxPairwiseID(ref SteamNetworkingIdentity self, InteropHelp.UTF8StringHandle pszString);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern nint SteamAPI_SteamNetworkingIdentity_GetXboxPairwiseID(ref SteamNetworkingIdentity self);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern nint SteamAPI_SteamNetworkingIdentity_SetIPAddr(ref SteamNetworkingIdentity self, ref SteamNetworkingIPAddr addr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern nint SteamAPI_SteamNetworkingIdentity_GetIPAddr(ref SteamNetworkingIdentity self);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern void SteamAPI_SteamNetworkingIdentity_SetIPv4Addr(ref SteamNetworkingIdentity self, uint nIPv4, ushort nPort);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern uint SteamAPI_SteamNetworkingIdentity_GetIPv4(ref SteamNetworkingIdentity self);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern ESteamNetworkingFakeIPType SteamAPI_SteamNetworkingIdentity_GetFakeIPType(ref SteamNetworkingIdentity self);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool SteamAPI_SteamNetworkingIdentity_IsFakeIP(ref SteamNetworkingIdentity self);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern void SteamAPI_SteamNetworkingIdentity_SetLocalHost(ref SteamNetworkingIdentity self);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool SteamAPI_SteamNetworkingIdentity_IsLocalHost(ref SteamNetworkingIdentity self);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool SteamAPI_SteamNetworkingIdentity_SetGenericString(ref SteamNetworkingIdentity self, InteropHelp.UTF8StringHandle pszString);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern nint SteamAPI_SteamNetworkingIdentity_GetGenericString(ref SteamNetworkingIdentity self);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool SteamAPI_SteamNetworkingIdentity_SetGenericBytes(ref SteamNetworkingIdentity self, [In][Out] byte[] data, uint cbLen);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern nint SteamAPI_SteamNetworkingIdentity_GetGenericBytes(ref SteamNetworkingIdentity self, out int cbLen);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool SteamAPI_SteamNetworkingIdentity_IsEqualTo(ref SteamNetworkingIdentity self, ref SteamNetworkingIdentity x);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern void SteamAPI_SteamNetworkingIdentity_ToString(ref SteamNetworkingIdentity self, nint buf, uint cbBuf);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool SteamAPI_SteamNetworkingIdentity_ParseString(ref SteamNetworkingIdentity self, InteropHelp.UTF8StringHandle pszStr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern void SteamAPI_SteamNetworkingMessage_t_Release(nint self);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool SteamAPI_ISteamNetworkingConnectionSignaling_SendSignal(ref ISteamNetworkingConnectionSignaling self, HSteamNetConnection hConn, ref SteamNetConnectionInfo_t info, nint pMsg, int cbMsg);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern void SteamAPI_ISteamNetworkingConnectionSignaling_Release(ref ISteamNetworkingConnectionSignaling self);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern nint SteamAPI_ISteamNetworkingSignalingRecvContext_OnConnectRequest(ref ISteamNetworkingSignalingRecvContext self, HSteamNetConnection hConn, ref SteamNetworkingIdentity identityPeer, int nLocalVirtualPort);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl)]
	public static extern void SteamAPI_ISteamNetworkingSignalingRecvContext_SendRejectionSignal(ref ISteamNetworkingSignalingRecvContext self, ref SteamNetworkingIdentity identityPeer, nint pMsg, int cbMsg);

	[DllImport("sdkencryptedappticket64", CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool SteamEncryptedAppTicket_BDecryptTicket([In][Out] byte[] rgubTicketEncrypted, uint cubTicketEncrypted, [In][Out] byte[] rgubTicketDecrypted, ref uint pcubTicketDecrypted, [MarshalAs(UnmanagedType.LPArray, SizeConst = 32)] byte[] rgubKey, int cubKey);

	[DllImport("sdkencryptedappticket64", CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool SteamEncryptedAppTicket_BIsTicketForApp([In][Out] byte[] rgubTicketDecrypted, uint cubTicketDecrypted, AppId_t nAppID);

	[DllImport("sdkencryptedappticket64", CallingConvention = CallingConvention.Cdecl)]
	public static extern uint SteamEncryptedAppTicket_GetTicketIssueTime([In][Out] byte[] rgubTicketDecrypted, uint cubTicketDecrypted);

	[DllImport("sdkencryptedappticket64", CallingConvention = CallingConvention.Cdecl)]
	public static extern void SteamEncryptedAppTicket_GetTicketSteamID([In][Out] byte[] rgubTicketDecrypted, uint cubTicketDecrypted, out CSteamID psteamID);

	[DllImport("sdkencryptedappticket64", CallingConvention = CallingConvention.Cdecl)]
	public static extern uint SteamEncryptedAppTicket_GetTicketAppID([In][Out] byte[] rgubTicketDecrypted, uint cubTicketDecrypted);

	[DllImport("sdkencryptedappticket64", CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool SteamEncryptedAppTicket_BUserOwnsAppInTicket([In][Out] byte[] rgubTicketDecrypted, uint cubTicketDecrypted, AppId_t nAppID);

	[DllImport("sdkencryptedappticket64", CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool SteamEncryptedAppTicket_BUserIsVacBanned([In][Out] byte[] rgubTicketDecrypted, uint cubTicketDecrypted);

	[DllImport("sdkencryptedappticket64", CallingConvention = CallingConvention.Cdecl)]
	public static extern nint SteamEncryptedAppTicket_GetUserVariableData([In][Out] byte[] rgubTicketDecrypted, uint cubTicketDecrypted, out uint pcubUserData);

	[DllImport("sdkencryptedappticket64", CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool SteamEncryptedAppTicket_BIsTicketSigned([In][Out] byte[] rgubTicketDecrypted, uint cubTicketDecrypted, [In][Out] byte[] pubRSAKey, uint cubRSAKey);

	[DllImport("sdkencryptedappticket64", CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool SteamEncryptedAppTicket_BIsLicenseBorrowed([In][Out] byte[] rgubTicketDecrypted, uint cubTicketDecrypted);

	[DllImport("sdkencryptedappticket64", CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool SteamEncryptedAppTicket_BIsLicenseTemporary([In][Out] byte[] rgubTicketDecrypted, uint cubTicketDecrypted);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamAppList_GetNumInstalledApps")]
	public static extern uint ISteamAppList_GetNumInstalledApps(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamAppList_GetInstalledApps")]
	public static extern uint ISteamAppList_GetInstalledApps(nint instancePtr, [In][Out] AppId_t[] pvecAppID, uint unMaxAppIDs);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamAppList_GetAppName")]
	public static extern int ISteamAppList_GetAppName(nint instancePtr, AppId_t nAppID, nint pchName, int cchNameMax);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamAppList_GetAppInstallDir")]
	public static extern int ISteamAppList_GetAppInstallDir(nint instancePtr, AppId_t nAppID, nint pchDirectory, int cchNameMax);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamAppList_GetAppBuildId")]
	public static extern int ISteamAppList_GetAppBuildId(nint instancePtr, AppId_t nAppID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamApps_BIsSubscribed")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamApps_BIsSubscribed(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamApps_BIsLowViolence")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamApps_BIsLowViolence(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamApps_BIsCybercafe")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamApps_BIsCybercafe(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamApps_BIsVACBanned")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamApps_BIsVACBanned(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamApps_GetCurrentGameLanguage")]
	public static extern nint ISteamApps_GetCurrentGameLanguage(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamApps_GetAvailableGameLanguages")]
	public static extern nint ISteamApps_GetAvailableGameLanguages(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamApps_BIsSubscribedApp")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamApps_BIsSubscribedApp(nint instancePtr, AppId_t appID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamApps_BIsDlcInstalled")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamApps_BIsDlcInstalled(nint instancePtr, AppId_t appID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamApps_GetEarliestPurchaseUnixTime")]
	public static extern uint ISteamApps_GetEarliestPurchaseUnixTime(nint instancePtr, AppId_t nAppID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamApps_BIsSubscribedFromFreeWeekend")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamApps_BIsSubscribedFromFreeWeekend(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamApps_GetDLCCount")]
	public static extern int ISteamApps_GetDLCCount(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamApps_BGetDLCDataByIndex")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamApps_BGetDLCDataByIndex(nint instancePtr, int iDLC, out AppId_t pAppID, out bool pbAvailable, nint pchName, int cchNameBufferSize);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamApps_InstallDLC")]
	public static extern void ISteamApps_InstallDLC(nint instancePtr, AppId_t nAppID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamApps_UninstallDLC")]
	public static extern void ISteamApps_UninstallDLC(nint instancePtr, AppId_t nAppID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamApps_RequestAppProofOfPurchaseKey")]
	public static extern void ISteamApps_RequestAppProofOfPurchaseKey(nint instancePtr, AppId_t nAppID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamApps_GetCurrentBetaName")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamApps_GetCurrentBetaName(nint instancePtr, nint pchName, int cchNameBufferSize);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamApps_MarkContentCorrupt")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamApps_MarkContentCorrupt(nint instancePtr, [MarshalAs(UnmanagedType.I1)] bool bMissingFilesOnly);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamApps_GetInstalledDepots")]
	public static extern uint ISteamApps_GetInstalledDepots(nint instancePtr, AppId_t appID, [In][Out] DepotId_t[] pvecDepots, uint cMaxDepots);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamApps_GetAppInstallDir")]
	public static extern uint ISteamApps_GetAppInstallDir(nint instancePtr, AppId_t appID, nint pchFolder, uint cchFolderBufferSize);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamApps_BIsAppInstalled")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamApps_BIsAppInstalled(nint instancePtr, AppId_t appID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamApps_GetAppOwner")]
	public static extern ulong ISteamApps_GetAppOwner(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamApps_GetLaunchQueryParam")]
	public static extern nint ISteamApps_GetLaunchQueryParam(nint instancePtr, InteropHelp.UTF8StringHandle pchKey);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamApps_GetDlcDownloadProgress")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamApps_GetDlcDownloadProgress(nint instancePtr, AppId_t nAppID, out ulong punBytesDownloaded, out ulong punBytesTotal);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamApps_GetAppBuildId")]
	public static extern int ISteamApps_GetAppBuildId(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamApps_RequestAllProofOfPurchaseKeys")]
	public static extern void ISteamApps_RequestAllProofOfPurchaseKeys(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamApps_GetFileDetails")]
	public static extern ulong ISteamApps_GetFileDetails(nint instancePtr, InteropHelp.UTF8StringHandle pszFileName);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamApps_GetLaunchCommandLine")]
	public static extern int ISteamApps_GetLaunchCommandLine(nint instancePtr, nint pszCommandLine, int cubCommandLine);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamApps_BIsSubscribedFromFamilySharing")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamApps_BIsSubscribedFromFamilySharing(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamApps_BIsTimedTrial")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamApps_BIsTimedTrial(nint instancePtr, out uint punSecondsAllowed, out uint punSecondsPlayed);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamClient_CreateSteamPipe")]
	public static extern int ISteamClient_CreateSteamPipe(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamClient_BReleaseSteamPipe")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamClient_BReleaseSteamPipe(nint instancePtr, HSteamPipe hSteamPipe);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamClient_ConnectToGlobalUser")]
	public static extern int ISteamClient_ConnectToGlobalUser(nint instancePtr, HSteamPipe hSteamPipe);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamClient_CreateLocalUser")]
	public static extern int ISteamClient_CreateLocalUser(nint instancePtr, out HSteamPipe phSteamPipe, EAccountType eAccountType);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamClient_ReleaseUser")]
	public static extern void ISteamClient_ReleaseUser(nint instancePtr, HSteamPipe hSteamPipe, HSteamUser hUser);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamClient_GetISteamUser")]
	public static extern nint ISteamClient_GetISteamUser(nint instancePtr, HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamClient_GetISteamGameServer")]
	public static extern nint ISteamClient_GetISteamGameServer(nint instancePtr, HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamClient_SetLocalIPBinding")]
	public static extern void ISteamClient_SetLocalIPBinding(nint instancePtr, ref SteamIPAddress_t unIP, ushort usPort);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamClient_GetISteamFriends")]
	public static extern nint ISteamClient_GetISteamFriends(nint instancePtr, HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamClient_GetISteamUtils")]
	public static extern nint ISteamClient_GetISteamUtils(nint instancePtr, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamClient_GetISteamMatchmaking")]
	public static extern nint ISteamClient_GetISteamMatchmaking(nint instancePtr, HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamClient_GetISteamMatchmakingServers")]
	public static extern nint ISteamClient_GetISteamMatchmakingServers(nint instancePtr, HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamClient_GetISteamGenericInterface")]
	public static extern nint ISteamClient_GetISteamGenericInterface(nint instancePtr, HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamClient_GetISteamUserStats")]
	public static extern nint ISteamClient_GetISteamUserStats(nint instancePtr, HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamClient_GetISteamGameServerStats")]
	public static extern nint ISteamClient_GetISteamGameServerStats(nint instancePtr, HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamClient_GetISteamApps")]
	public static extern nint ISteamClient_GetISteamApps(nint instancePtr, HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamClient_GetISteamNetworking")]
	public static extern nint ISteamClient_GetISteamNetworking(nint instancePtr, HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamClient_GetISteamRemoteStorage")]
	public static extern nint ISteamClient_GetISteamRemoteStorage(nint instancePtr, HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamClient_GetISteamScreenshots")]
	public static extern nint ISteamClient_GetISteamScreenshots(nint instancePtr, HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamClient_GetISteamGameSearch")]
	public static extern nint ISteamClient_GetISteamGameSearch(nint instancePtr, HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamClient_GetIPCCallCount")]
	public static extern uint ISteamClient_GetIPCCallCount(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamClient_SetWarningMessageHook")]
	public static extern void ISteamClient_SetWarningMessageHook(nint instancePtr, SteamAPIWarningMessageHook_t pFunction);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamClient_BShutdownIfAllPipesClosed")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamClient_BShutdownIfAllPipesClosed(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamClient_GetISteamHTTP")]
	public static extern nint ISteamClient_GetISteamHTTP(nint instancePtr, HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamClient_GetISteamController")]
	public static extern nint ISteamClient_GetISteamController(nint instancePtr, HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamClient_GetISteamUGC")]
	public static extern nint ISteamClient_GetISteamUGC(nint instancePtr, HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamClient_GetISteamAppList")]
	public static extern nint ISteamClient_GetISteamAppList(nint instancePtr, HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamClient_GetISteamMusic")]
	public static extern nint ISteamClient_GetISteamMusic(nint instancePtr, HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamClient_GetISteamMusicRemote")]
	public static extern nint ISteamClient_GetISteamMusicRemote(nint instancePtr, HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamClient_GetISteamHTMLSurface")]
	public static extern nint ISteamClient_GetISteamHTMLSurface(nint instancePtr, HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamClient_GetISteamInventory")]
	public static extern nint ISteamClient_GetISteamInventory(nint instancePtr, HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamClient_GetISteamVideo")]
	public static extern nint ISteamClient_GetISteamVideo(nint instancePtr, HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamClient_GetISteamParentalSettings")]
	public static extern nint ISteamClient_GetISteamParentalSettings(nint instancePtr, HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamClient_GetISteamInput")]
	public static extern nint ISteamClient_GetISteamInput(nint instancePtr, HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamClient_GetISteamParties")]
	public static extern nint ISteamClient_GetISteamParties(nint instancePtr, HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamClient_GetISteamRemotePlay")]
	public static extern nint ISteamClient_GetISteamRemotePlay(nint instancePtr, HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetPersonaName")]
	public static extern nint ISteamFriends_GetPersonaName(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_SetPersonaName")]
	public static extern ulong ISteamFriends_SetPersonaName(nint instancePtr, InteropHelp.UTF8StringHandle pchPersonaName);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetPersonaState")]
	public static extern EPersonaState ISteamFriends_GetPersonaState(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetFriendCount")]
	public static extern int ISteamFriends_GetFriendCount(nint instancePtr, EFriendFlags iFriendFlags);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetFriendByIndex")]
	public static extern ulong ISteamFriends_GetFriendByIndex(nint instancePtr, int iFriend, EFriendFlags iFriendFlags);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetFriendRelationship")]
	public static extern EFriendRelationship ISteamFriends_GetFriendRelationship(nint instancePtr, CSteamID steamIDFriend);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetFriendPersonaState")]
	public static extern EPersonaState ISteamFriends_GetFriendPersonaState(nint instancePtr, CSteamID steamIDFriend);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetFriendPersonaName")]
	public static extern nint ISteamFriends_GetFriendPersonaName(nint instancePtr, CSteamID steamIDFriend);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetFriendGamePlayed")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamFriends_GetFriendGamePlayed(nint instancePtr, CSteamID steamIDFriend, out FriendGameInfo_t pFriendGameInfo);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetFriendPersonaNameHistory")]
	public static extern nint ISteamFriends_GetFriendPersonaNameHistory(nint instancePtr, CSteamID steamIDFriend, int iPersonaName);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetFriendSteamLevel")]
	public static extern int ISteamFriends_GetFriendSteamLevel(nint instancePtr, CSteamID steamIDFriend);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetPlayerNickname")]
	public static extern nint ISteamFriends_GetPlayerNickname(nint instancePtr, CSteamID steamIDPlayer);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetFriendsGroupCount")]
	public static extern int ISteamFriends_GetFriendsGroupCount(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetFriendsGroupIDByIndex")]
	public static extern short ISteamFriends_GetFriendsGroupIDByIndex(nint instancePtr, int iFG);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetFriendsGroupName")]
	public static extern nint ISteamFriends_GetFriendsGroupName(nint instancePtr, FriendsGroupID_t friendsGroupID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetFriendsGroupMembersCount")]
	public static extern int ISteamFriends_GetFriendsGroupMembersCount(nint instancePtr, FriendsGroupID_t friendsGroupID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetFriendsGroupMembersList")]
	public static extern void ISteamFriends_GetFriendsGroupMembersList(nint instancePtr, FriendsGroupID_t friendsGroupID, [In][Out] CSteamID[] pOutSteamIDMembers, int nMembersCount);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_HasFriend")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamFriends_HasFriend(nint instancePtr, CSteamID steamIDFriend, EFriendFlags iFriendFlags);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetClanCount")]
	public static extern int ISteamFriends_GetClanCount(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetClanByIndex")]
	public static extern ulong ISteamFriends_GetClanByIndex(nint instancePtr, int iClan);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetClanName")]
	public static extern nint ISteamFriends_GetClanName(nint instancePtr, CSteamID steamIDClan);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetClanTag")]
	public static extern nint ISteamFriends_GetClanTag(nint instancePtr, CSteamID steamIDClan);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetClanActivityCounts")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamFriends_GetClanActivityCounts(nint instancePtr, CSteamID steamIDClan, out int pnOnline, out int pnInGame, out int pnChatting);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_DownloadClanActivityCounts")]
	public static extern ulong ISteamFriends_DownloadClanActivityCounts(nint instancePtr, [In][Out] CSteamID[] psteamIDClans, int cClansToRequest);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetFriendCountFromSource")]
	public static extern int ISteamFriends_GetFriendCountFromSource(nint instancePtr, CSteamID steamIDSource);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetFriendFromSourceByIndex")]
	public static extern ulong ISteamFriends_GetFriendFromSourceByIndex(nint instancePtr, CSteamID steamIDSource, int iFriend);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_IsUserInSource")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamFriends_IsUserInSource(nint instancePtr, CSteamID steamIDUser, CSteamID steamIDSource);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_SetInGameVoiceSpeaking")]
	public static extern void ISteamFriends_SetInGameVoiceSpeaking(nint instancePtr, CSteamID steamIDUser, [MarshalAs(UnmanagedType.I1)] bool bSpeaking);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_ActivateGameOverlay")]
	public static extern void ISteamFriends_ActivateGameOverlay(nint instancePtr, InteropHelp.UTF8StringHandle pchDialog);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_ActivateGameOverlayToUser")]
	public static extern void ISteamFriends_ActivateGameOverlayToUser(nint instancePtr, InteropHelp.UTF8StringHandle pchDialog, CSteamID steamID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_ActivateGameOverlayToWebPage")]
	public static extern void ISteamFriends_ActivateGameOverlayToWebPage(nint instancePtr, InteropHelp.UTF8StringHandle pchURL, EActivateGameOverlayToWebPageMode eMode);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_ActivateGameOverlayToStore")]
	public static extern void ISteamFriends_ActivateGameOverlayToStore(nint instancePtr, AppId_t nAppID, EOverlayToStoreFlag eFlag);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_SetPlayedWith")]
	public static extern void ISteamFriends_SetPlayedWith(nint instancePtr, CSteamID steamIDUserPlayedWith);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_ActivateGameOverlayInviteDialog")]
	public static extern void ISteamFriends_ActivateGameOverlayInviteDialog(nint instancePtr, CSteamID steamIDLobby);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetSmallFriendAvatar")]
	public static extern int ISteamFriends_GetSmallFriendAvatar(nint instancePtr, CSteamID steamIDFriend);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetMediumFriendAvatar")]
	public static extern int ISteamFriends_GetMediumFriendAvatar(nint instancePtr, CSteamID steamIDFriend);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetLargeFriendAvatar")]
	public static extern int ISteamFriends_GetLargeFriendAvatar(nint instancePtr, CSteamID steamIDFriend);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_RequestUserInformation")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamFriends_RequestUserInformation(nint instancePtr, CSteamID steamIDUser, [MarshalAs(UnmanagedType.I1)] bool bRequireNameOnly);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_RequestClanOfficerList")]
	public static extern ulong ISteamFriends_RequestClanOfficerList(nint instancePtr, CSteamID steamIDClan);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetClanOwner")]
	public static extern ulong ISteamFriends_GetClanOwner(nint instancePtr, CSteamID steamIDClan);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetClanOfficerCount")]
	public static extern int ISteamFriends_GetClanOfficerCount(nint instancePtr, CSteamID steamIDClan);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetClanOfficerByIndex")]
	public static extern ulong ISteamFriends_GetClanOfficerByIndex(nint instancePtr, CSteamID steamIDClan, int iOfficer);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetUserRestrictions")]
	public static extern uint ISteamFriends_GetUserRestrictions(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_SetRichPresence")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamFriends_SetRichPresence(nint instancePtr, InteropHelp.UTF8StringHandle pchKey, InteropHelp.UTF8StringHandle pchValue);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_ClearRichPresence")]
	public static extern void ISteamFriends_ClearRichPresence(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetFriendRichPresence")]
	public static extern nint ISteamFriends_GetFriendRichPresence(nint instancePtr, CSteamID steamIDFriend, InteropHelp.UTF8StringHandle pchKey);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetFriendRichPresenceKeyCount")]
	public static extern int ISteamFriends_GetFriendRichPresenceKeyCount(nint instancePtr, CSteamID steamIDFriend);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetFriendRichPresenceKeyByIndex")]
	public static extern nint ISteamFriends_GetFriendRichPresenceKeyByIndex(nint instancePtr, CSteamID steamIDFriend, int iKey);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_RequestFriendRichPresence")]
	public static extern void ISteamFriends_RequestFriendRichPresence(nint instancePtr, CSteamID steamIDFriend);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_InviteUserToGame")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamFriends_InviteUserToGame(nint instancePtr, CSteamID steamIDFriend, InteropHelp.UTF8StringHandle pchConnectString);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetCoplayFriendCount")]
	public static extern int ISteamFriends_GetCoplayFriendCount(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetCoplayFriend")]
	public static extern ulong ISteamFriends_GetCoplayFriend(nint instancePtr, int iCoplayFriend);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetFriendCoplayTime")]
	public static extern int ISteamFriends_GetFriendCoplayTime(nint instancePtr, CSteamID steamIDFriend);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetFriendCoplayGame")]
	public static extern uint ISteamFriends_GetFriendCoplayGame(nint instancePtr, CSteamID steamIDFriend);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_JoinClanChatRoom")]
	public static extern ulong ISteamFriends_JoinClanChatRoom(nint instancePtr, CSteamID steamIDClan);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_LeaveClanChatRoom")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamFriends_LeaveClanChatRoom(nint instancePtr, CSteamID steamIDClan);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetClanChatMemberCount")]
	public static extern int ISteamFriends_GetClanChatMemberCount(nint instancePtr, CSteamID steamIDClan);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetChatMemberByIndex")]
	public static extern ulong ISteamFriends_GetChatMemberByIndex(nint instancePtr, CSteamID steamIDClan, int iUser);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_SendClanChatMessage")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamFriends_SendClanChatMessage(nint instancePtr, CSteamID steamIDClanChat, InteropHelp.UTF8StringHandle pchText);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetClanChatMessage")]
	public static extern int ISteamFriends_GetClanChatMessage(nint instancePtr, CSteamID steamIDClanChat, int iMessage, nint prgchText, int cchTextMax, out EChatEntryType peChatEntryType, out CSteamID psteamidChatter);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_IsClanChatAdmin")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamFriends_IsClanChatAdmin(nint instancePtr, CSteamID steamIDClanChat, CSteamID steamIDUser);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_IsClanChatWindowOpenInSteam")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamFriends_IsClanChatWindowOpenInSteam(nint instancePtr, CSteamID steamIDClanChat);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_OpenClanChatWindowInSteam")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamFriends_OpenClanChatWindowInSteam(nint instancePtr, CSteamID steamIDClanChat);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_CloseClanChatWindowInSteam")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamFriends_CloseClanChatWindowInSteam(nint instancePtr, CSteamID steamIDClanChat);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_SetListenForFriendsMessages")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamFriends_SetListenForFriendsMessages(nint instancePtr, [MarshalAs(UnmanagedType.I1)] bool bInterceptEnabled);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_ReplyToFriendMessage")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamFriends_ReplyToFriendMessage(nint instancePtr, CSteamID steamIDFriend, InteropHelp.UTF8StringHandle pchMsgToSend);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetFriendMessage")]
	public static extern int ISteamFriends_GetFriendMessage(nint instancePtr, CSteamID steamIDFriend, int iMessageID, nint pvData, int cubData, out EChatEntryType peChatEntryType);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetFollowerCount")]
	public static extern ulong ISteamFriends_GetFollowerCount(nint instancePtr, CSteamID steamID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_IsFollowing")]
	public static extern ulong ISteamFriends_IsFollowing(nint instancePtr, CSteamID steamID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_EnumerateFollowingList")]
	public static extern ulong ISteamFriends_EnumerateFollowingList(nint instancePtr, uint unStartIndex);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_IsClanPublic")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamFriends_IsClanPublic(nint instancePtr, CSteamID steamIDClan);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_IsClanOfficialGameGroup")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamFriends_IsClanOfficialGameGroup(nint instancePtr, CSteamID steamIDClan);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_GetNumChatsWithUnreadPriorityMessages")]
	public static extern int ISteamFriends_GetNumChatsWithUnreadPriorityMessages(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_ActivateGameOverlayRemotePlayTogetherInviteDialog")]
	public static extern void ISteamFriends_ActivateGameOverlayRemotePlayTogetherInviteDialog(nint instancePtr, CSteamID steamIDLobby);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_RegisterProtocolInOverlayBrowser")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamFriends_RegisterProtocolInOverlayBrowser(nint instancePtr, InteropHelp.UTF8StringHandle pchProtocol);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamFriends_ActivateGameOverlayInviteDialogConnectString")]
	public static extern void ISteamFriends_ActivateGameOverlayInviteDialogConnectString(nint instancePtr, InteropHelp.UTF8StringHandle pchConnectString);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_SetProduct")]
	public static extern void ISteamGameServer_SetProduct(nint instancePtr, InteropHelp.UTF8StringHandle pszProduct);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_SetGameDescription")]
	public static extern void ISteamGameServer_SetGameDescription(nint instancePtr, InteropHelp.UTF8StringHandle pszGameDescription);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_SetModDir")]
	public static extern void ISteamGameServer_SetModDir(nint instancePtr, InteropHelp.UTF8StringHandle pszModDir);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_SetDedicatedServer")]
	public static extern void ISteamGameServer_SetDedicatedServer(nint instancePtr, [MarshalAs(UnmanagedType.I1)] bool bDedicated);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_LogOn")]
	public static extern void ISteamGameServer_LogOn(nint instancePtr, InteropHelp.UTF8StringHandle pszToken);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_LogOnAnonymous")]
	public static extern void ISteamGameServer_LogOnAnonymous(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_LogOff")]
	public static extern void ISteamGameServer_LogOff(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_BLoggedOn")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamGameServer_BLoggedOn(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_BSecure")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamGameServer_BSecure(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_GetSteamID")]
	public static extern ulong ISteamGameServer_GetSteamID(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_WasRestartRequested")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamGameServer_WasRestartRequested(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_SetMaxPlayerCount")]
	public static extern void ISteamGameServer_SetMaxPlayerCount(nint instancePtr, int cPlayersMax);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_SetBotPlayerCount")]
	public static extern void ISteamGameServer_SetBotPlayerCount(nint instancePtr, int cBotplayers);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_SetServerName")]
	public static extern void ISteamGameServer_SetServerName(nint instancePtr, InteropHelp.UTF8StringHandle pszServerName);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_SetMapName")]
	public static extern void ISteamGameServer_SetMapName(nint instancePtr, InteropHelp.UTF8StringHandle pszMapName);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_SetPasswordProtected")]
	public static extern void ISteamGameServer_SetPasswordProtected(nint instancePtr, [MarshalAs(UnmanagedType.I1)] bool bPasswordProtected);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_SetSpectatorPort")]
	public static extern void ISteamGameServer_SetSpectatorPort(nint instancePtr, ushort unSpectatorPort);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_SetSpectatorServerName")]
	public static extern void ISteamGameServer_SetSpectatorServerName(nint instancePtr, InteropHelp.UTF8StringHandle pszSpectatorServerName);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_ClearAllKeyValues")]
	public static extern void ISteamGameServer_ClearAllKeyValues(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_SetKeyValue")]
	public static extern void ISteamGameServer_SetKeyValue(nint instancePtr, InteropHelp.UTF8StringHandle pKey, InteropHelp.UTF8StringHandle pValue);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_SetGameTags")]
	public static extern void ISteamGameServer_SetGameTags(nint instancePtr, InteropHelp.UTF8StringHandle pchGameTags);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_SetGameData")]
	public static extern void ISteamGameServer_SetGameData(nint instancePtr, InteropHelp.UTF8StringHandle pchGameData);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_SetRegion")]
	public static extern void ISteamGameServer_SetRegion(nint instancePtr, InteropHelp.UTF8StringHandle pszRegion);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_SetAdvertiseServerActive")]
	public static extern void ISteamGameServer_SetAdvertiseServerActive(nint instancePtr, [MarshalAs(UnmanagedType.I1)] bool bActive);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_GetAuthSessionTicket")]
	public static extern uint ISteamGameServer_GetAuthSessionTicket(nint instancePtr, byte[] pTicket, int cbMaxTicket, out uint pcbTicket);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_BeginAuthSession")]
	public static extern EBeginAuthSessionResult ISteamGameServer_BeginAuthSession(nint instancePtr, byte[] pAuthTicket, int cbAuthTicket, CSteamID steamID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_EndAuthSession")]
	public static extern void ISteamGameServer_EndAuthSession(nint instancePtr, CSteamID steamID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_CancelAuthTicket")]
	public static extern void ISteamGameServer_CancelAuthTicket(nint instancePtr, HAuthTicket hAuthTicket);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_UserHasLicenseForApp")]
	public static extern EUserHasLicenseForAppResult ISteamGameServer_UserHasLicenseForApp(nint instancePtr, CSteamID steamID, AppId_t appID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_RequestUserGroupStatus")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamGameServer_RequestUserGroupStatus(nint instancePtr, CSteamID steamIDUser, CSteamID steamIDGroup);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_GetGameplayStats")]
	public static extern void ISteamGameServer_GetGameplayStats(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_GetServerReputation")]
	public static extern ulong ISteamGameServer_GetServerReputation(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_GetPublicIP")]
	public static extern SteamIPAddress_t ISteamGameServer_GetPublicIP(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_HandleIncomingPacket")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamGameServer_HandleIncomingPacket(nint instancePtr, byte[] pData, int cbData, uint srcIP, ushort srcPort);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_GetNextOutgoingPacket")]
	public static extern int ISteamGameServer_GetNextOutgoingPacket(nint instancePtr, byte[] pOut, int cbMaxOut, out uint pNetAdr, out ushort pPort);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_AssociateWithClan")]
	public static extern ulong ISteamGameServer_AssociateWithClan(nint instancePtr, CSteamID steamIDClan);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_ComputeNewPlayerCompatibility")]
	public static extern ulong ISteamGameServer_ComputeNewPlayerCompatibility(nint instancePtr, CSteamID steamIDNewPlayer);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_SendUserConnectAndAuthenticate_DEPRECATED")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamGameServer_SendUserConnectAndAuthenticate_DEPRECATED(nint instancePtr, uint unIPClient, byte[] pvAuthBlob, uint cubAuthBlobSize, out CSteamID pSteamIDUser);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_CreateUnauthenticatedUserConnection")]
	public static extern ulong ISteamGameServer_CreateUnauthenticatedUserConnection(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_SendUserDisconnect_DEPRECATED")]
	public static extern void ISteamGameServer_SendUserDisconnect_DEPRECATED(nint instancePtr, CSteamID steamIDUser);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServer_BUpdateUserData")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamGameServer_BUpdateUserData(nint instancePtr, CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchPlayerName, uint uScore);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServerStats_RequestUserStats")]
	public static extern ulong ISteamGameServerStats_RequestUserStats(nint instancePtr, CSteamID steamIDUser);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServerStats_GetUserStatInt32")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamGameServerStats_GetUserStatInt32(nint instancePtr, CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, out int pData);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServerStats_GetUserStatFloat")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamGameServerStats_GetUserStatFloat(nint instancePtr, CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, out float pData);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServerStats_GetUserAchievement")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamGameServerStats_GetUserAchievement(nint instancePtr, CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, out bool pbAchieved);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServerStats_SetUserStatInt32")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamGameServerStats_SetUserStatInt32(nint instancePtr, CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, int nData);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServerStats_SetUserStatFloat")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamGameServerStats_SetUserStatFloat(nint instancePtr, CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, float fData);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServerStats_UpdateUserAvgRateStat")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamGameServerStats_UpdateUserAvgRateStat(nint instancePtr, CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, float flCountThisSession, double dSessionLength);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServerStats_SetUserAchievement")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamGameServerStats_SetUserAchievement(nint instancePtr, CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServerStats_ClearUserAchievement")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamGameServerStats_ClearUserAchievement(nint instancePtr, CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameServerStats_StoreUserStats")]
	public static extern ulong ISteamGameServerStats_StoreUserStats(nint instancePtr, CSteamID steamIDUser);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_Init")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamHTMLSurface_Init(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_Shutdown")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamHTMLSurface_Shutdown(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_CreateBrowser")]
	public static extern ulong ISteamHTMLSurface_CreateBrowser(nint instancePtr, InteropHelp.UTF8StringHandle pchUserAgent, InteropHelp.UTF8StringHandle pchUserCSS);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_RemoveBrowser")]
	public static extern void ISteamHTMLSurface_RemoveBrowser(nint instancePtr, HHTMLBrowser unBrowserHandle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_LoadURL")]
	public static extern void ISteamHTMLSurface_LoadURL(nint instancePtr, HHTMLBrowser unBrowserHandle, InteropHelp.UTF8StringHandle pchURL, InteropHelp.UTF8StringHandle pchPostData);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_SetSize")]
	public static extern void ISteamHTMLSurface_SetSize(nint instancePtr, HHTMLBrowser unBrowserHandle, uint unWidth, uint unHeight);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_StopLoad")]
	public static extern void ISteamHTMLSurface_StopLoad(nint instancePtr, HHTMLBrowser unBrowserHandle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_Reload")]
	public static extern void ISteamHTMLSurface_Reload(nint instancePtr, HHTMLBrowser unBrowserHandle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_GoBack")]
	public static extern void ISteamHTMLSurface_GoBack(nint instancePtr, HHTMLBrowser unBrowserHandle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_GoForward")]
	public static extern void ISteamHTMLSurface_GoForward(nint instancePtr, HHTMLBrowser unBrowserHandle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_AddHeader")]
	public static extern void ISteamHTMLSurface_AddHeader(nint instancePtr, HHTMLBrowser unBrowserHandle, InteropHelp.UTF8StringHandle pchKey, InteropHelp.UTF8StringHandle pchValue);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_ExecuteJavascript")]
	public static extern void ISteamHTMLSurface_ExecuteJavascript(nint instancePtr, HHTMLBrowser unBrowserHandle, InteropHelp.UTF8StringHandle pchScript);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_MouseUp")]
	public static extern void ISteamHTMLSurface_MouseUp(nint instancePtr, HHTMLBrowser unBrowserHandle, EHTMLMouseButton eMouseButton);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_MouseDown")]
	public static extern void ISteamHTMLSurface_MouseDown(nint instancePtr, HHTMLBrowser unBrowserHandle, EHTMLMouseButton eMouseButton);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_MouseDoubleClick")]
	public static extern void ISteamHTMLSurface_MouseDoubleClick(nint instancePtr, HHTMLBrowser unBrowserHandle, EHTMLMouseButton eMouseButton);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_MouseMove")]
	public static extern void ISteamHTMLSurface_MouseMove(nint instancePtr, HHTMLBrowser unBrowserHandle, int x, int y);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_MouseWheel")]
	public static extern void ISteamHTMLSurface_MouseWheel(nint instancePtr, HHTMLBrowser unBrowserHandle, int nDelta);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_KeyDown")]
	public static extern void ISteamHTMLSurface_KeyDown(nint instancePtr, HHTMLBrowser unBrowserHandle, uint nNativeKeyCode, EHTMLKeyModifiers eHTMLKeyModifiers, [MarshalAs(UnmanagedType.I1)] bool bIsSystemKey);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_KeyUp")]
	public static extern void ISteamHTMLSurface_KeyUp(nint instancePtr, HHTMLBrowser unBrowserHandle, uint nNativeKeyCode, EHTMLKeyModifiers eHTMLKeyModifiers);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_KeyChar")]
	public static extern void ISteamHTMLSurface_KeyChar(nint instancePtr, HHTMLBrowser unBrowserHandle, uint cUnicodeChar, EHTMLKeyModifiers eHTMLKeyModifiers);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_SetHorizontalScroll")]
	public static extern void ISteamHTMLSurface_SetHorizontalScroll(nint instancePtr, HHTMLBrowser unBrowserHandle, uint nAbsolutePixelScroll);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_SetVerticalScroll")]
	public static extern void ISteamHTMLSurface_SetVerticalScroll(nint instancePtr, HHTMLBrowser unBrowserHandle, uint nAbsolutePixelScroll);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_SetKeyFocus")]
	public static extern void ISteamHTMLSurface_SetKeyFocus(nint instancePtr, HHTMLBrowser unBrowserHandle, [MarshalAs(UnmanagedType.I1)] bool bHasKeyFocus);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_ViewSource")]
	public static extern void ISteamHTMLSurface_ViewSource(nint instancePtr, HHTMLBrowser unBrowserHandle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_CopyToClipboard")]
	public static extern void ISteamHTMLSurface_CopyToClipboard(nint instancePtr, HHTMLBrowser unBrowserHandle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_PasteFromClipboard")]
	public static extern void ISteamHTMLSurface_PasteFromClipboard(nint instancePtr, HHTMLBrowser unBrowserHandle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_Find")]
	public static extern void ISteamHTMLSurface_Find(nint instancePtr, HHTMLBrowser unBrowserHandle, InteropHelp.UTF8StringHandle pchSearchStr, [MarshalAs(UnmanagedType.I1)] bool bCurrentlyInFind, [MarshalAs(UnmanagedType.I1)] bool bReverse);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_StopFind")]
	public static extern void ISteamHTMLSurface_StopFind(nint instancePtr, HHTMLBrowser unBrowserHandle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_GetLinkAtPosition")]
	public static extern void ISteamHTMLSurface_GetLinkAtPosition(nint instancePtr, HHTMLBrowser unBrowserHandle, int x, int y);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_SetCookie")]
	public static extern void ISteamHTMLSurface_SetCookie(nint instancePtr, InteropHelp.UTF8StringHandle pchHostname, InteropHelp.UTF8StringHandle pchKey, InteropHelp.UTF8StringHandle pchValue, InteropHelp.UTF8StringHandle pchPath, uint nExpires, [MarshalAs(UnmanagedType.I1)] bool bSecure, [MarshalAs(UnmanagedType.I1)] bool bHTTPOnly);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_SetPageScaleFactor")]
	public static extern void ISteamHTMLSurface_SetPageScaleFactor(nint instancePtr, HHTMLBrowser unBrowserHandle, float flZoom, int nPointX, int nPointY);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_SetBackgroundMode")]
	public static extern void ISteamHTMLSurface_SetBackgroundMode(nint instancePtr, HHTMLBrowser unBrowserHandle, [MarshalAs(UnmanagedType.I1)] bool bBackgroundMode);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_SetDPIScalingFactor")]
	public static extern void ISteamHTMLSurface_SetDPIScalingFactor(nint instancePtr, HHTMLBrowser unBrowserHandle, float flDPIScaling);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_OpenDeveloperTools")]
	public static extern void ISteamHTMLSurface_OpenDeveloperTools(nint instancePtr, HHTMLBrowser unBrowserHandle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_AllowStartRequest")]
	public static extern void ISteamHTMLSurface_AllowStartRequest(nint instancePtr, HHTMLBrowser unBrowserHandle, [MarshalAs(UnmanagedType.I1)] bool bAllowed);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_JSDialogResponse")]
	public static extern void ISteamHTMLSurface_JSDialogResponse(nint instancePtr, HHTMLBrowser unBrowserHandle, [MarshalAs(UnmanagedType.I1)] bool bResult);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTMLSurface_FileLoadDialogResponse")]
	public static extern void ISteamHTMLSurface_FileLoadDialogResponse(nint instancePtr, HHTMLBrowser unBrowserHandle, nint pchSelectedFiles);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTTP_CreateHTTPRequest")]
	public static extern uint ISteamHTTP_CreateHTTPRequest(nint instancePtr, EHTTPMethod eHTTPRequestMethod, InteropHelp.UTF8StringHandle pchAbsoluteURL);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTTP_SetHTTPRequestContextValue")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamHTTP_SetHTTPRequestContextValue(nint instancePtr, HTTPRequestHandle hRequest, ulong ulContextValue);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTTP_SetHTTPRequestNetworkActivityTimeout")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamHTTP_SetHTTPRequestNetworkActivityTimeout(nint instancePtr, HTTPRequestHandle hRequest, uint unTimeoutSeconds);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTTP_SetHTTPRequestHeaderValue")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamHTTP_SetHTTPRequestHeaderValue(nint instancePtr, HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchHeaderName, InteropHelp.UTF8StringHandle pchHeaderValue);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTTP_SetHTTPRequestGetOrPostParameter")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamHTTP_SetHTTPRequestGetOrPostParameter(nint instancePtr, HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchParamName, InteropHelp.UTF8StringHandle pchParamValue);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTTP_SendHTTPRequest")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamHTTP_SendHTTPRequest(nint instancePtr, HTTPRequestHandle hRequest, out SteamAPICall_t pCallHandle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTTP_SendHTTPRequestAndStreamResponse")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamHTTP_SendHTTPRequestAndStreamResponse(nint instancePtr, HTTPRequestHandle hRequest, out SteamAPICall_t pCallHandle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTTP_DeferHTTPRequest")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamHTTP_DeferHTTPRequest(nint instancePtr, HTTPRequestHandle hRequest);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTTP_PrioritizeHTTPRequest")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamHTTP_PrioritizeHTTPRequest(nint instancePtr, HTTPRequestHandle hRequest);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTTP_GetHTTPResponseHeaderSize")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamHTTP_GetHTTPResponseHeaderSize(nint instancePtr, HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchHeaderName, out uint unResponseHeaderSize);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTTP_GetHTTPResponseHeaderValue")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamHTTP_GetHTTPResponseHeaderValue(nint instancePtr, HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchHeaderName, byte[] pHeaderValueBuffer, uint unBufferSize);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTTP_GetHTTPResponseBodySize")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamHTTP_GetHTTPResponseBodySize(nint instancePtr, HTTPRequestHandle hRequest, out uint unBodySize);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTTP_GetHTTPResponseBodyData")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamHTTP_GetHTTPResponseBodyData(nint instancePtr, HTTPRequestHandle hRequest, byte[] pBodyDataBuffer, uint unBufferSize);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTTP_GetHTTPStreamingResponseBodyData")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamHTTP_GetHTTPStreamingResponseBodyData(nint instancePtr, HTTPRequestHandle hRequest, uint cOffset, byte[] pBodyDataBuffer, uint unBufferSize);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTTP_ReleaseHTTPRequest")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamHTTP_ReleaseHTTPRequest(nint instancePtr, HTTPRequestHandle hRequest);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTTP_GetHTTPDownloadProgressPct")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamHTTP_GetHTTPDownloadProgressPct(nint instancePtr, HTTPRequestHandle hRequest, out float pflPercentOut);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTTP_SetHTTPRequestRawPostBody")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamHTTP_SetHTTPRequestRawPostBody(nint instancePtr, HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchContentType, byte[] pubBody, uint unBodyLen);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTTP_CreateCookieContainer")]
	public static extern uint ISteamHTTP_CreateCookieContainer(nint instancePtr, [MarshalAs(UnmanagedType.I1)] bool bAllowResponsesToModify);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTTP_ReleaseCookieContainer")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamHTTP_ReleaseCookieContainer(nint instancePtr, HTTPCookieContainerHandle hCookieContainer);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTTP_SetCookie")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamHTTP_SetCookie(nint instancePtr, HTTPCookieContainerHandle hCookieContainer, InteropHelp.UTF8StringHandle pchHost, InteropHelp.UTF8StringHandle pchUrl, InteropHelp.UTF8StringHandle pchCookie);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTTP_SetHTTPRequestCookieContainer")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamHTTP_SetHTTPRequestCookieContainer(nint instancePtr, HTTPRequestHandle hRequest, HTTPCookieContainerHandle hCookieContainer);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTTP_SetHTTPRequestUserAgentInfo")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamHTTP_SetHTTPRequestUserAgentInfo(nint instancePtr, HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchUserAgentInfo);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTTP_SetHTTPRequestRequiresVerifiedCertificate")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamHTTP_SetHTTPRequestRequiresVerifiedCertificate(nint instancePtr, HTTPRequestHandle hRequest, [MarshalAs(UnmanagedType.I1)] bool bRequireVerifiedCertificate);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTTP_SetHTTPRequestAbsoluteTimeoutMS")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamHTTP_SetHTTPRequestAbsoluteTimeoutMS(nint instancePtr, HTTPRequestHandle hRequest, uint unMilliseconds);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamHTTP_GetHTTPRequestWasTimedOut")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamHTTP_GetHTTPRequestWasTimedOut(nint instancePtr, HTTPRequestHandle hRequest, out bool pbWasTimedOut);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_Init")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamInput_Init(nint instancePtr, [MarshalAs(UnmanagedType.I1)] bool bExplicitlyCallRunFrame);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_Shutdown")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamInput_Shutdown(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_SetInputActionManifestFilePath")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamInput_SetInputActionManifestFilePath(nint instancePtr, InteropHelp.UTF8StringHandle pchInputActionManifestAbsolutePath);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_RunFrame")]
	public static extern void ISteamInput_RunFrame(nint instancePtr, [MarshalAs(UnmanagedType.I1)] bool bReservedValue);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_BWaitForData")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamInput_BWaitForData(nint instancePtr, [MarshalAs(UnmanagedType.I1)] bool bWaitForever, uint unTimeout);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_BNewDataAvailable")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamInput_BNewDataAvailable(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_GetConnectedControllers")]
	public static extern int ISteamInput_GetConnectedControllers(nint instancePtr, [In][Out] InputHandle_t[] handlesOut);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_EnableDeviceCallbacks")]
	public static extern void ISteamInput_EnableDeviceCallbacks(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_EnableActionEventCallbacks")]
	public static extern void ISteamInput_EnableActionEventCallbacks(nint instancePtr, SteamInputActionEventCallbackPointer pCallback);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_GetActionSetHandle")]
	public static extern ulong ISteamInput_GetActionSetHandle(nint instancePtr, InteropHelp.UTF8StringHandle pszActionSetName);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_ActivateActionSet")]
	public static extern void ISteamInput_ActivateActionSet(nint instancePtr, InputHandle_t inputHandle, InputActionSetHandle_t actionSetHandle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_GetCurrentActionSet")]
	public static extern ulong ISteamInput_GetCurrentActionSet(nint instancePtr, InputHandle_t inputHandle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_ActivateActionSetLayer")]
	public static extern void ISteamInput_ActivateActionSetLayer(nint instancePtr, InputHandle_t inputHandle, InputActionSetHandle_t actionSetLayerHandle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_DeactivateActionSetLayer")]
	public static extern void ISteamInput_DeactivateActionSetLayer(nint instancePtr, InputHandle_t inputHandle, InputActionSetHandle_t actionSetLayerHandle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_DeactivateAllActionSetLayers")]
	public static extern void ISteamInput_DeactivateAllActionSetLayers(nint instancePtr, InputHandle_t inputHandle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_GetActiveActionSetLayers")]
	public static extern int ISteamInput_GetActiveActionSetLayers(nint instancePtr, InputHandle_t inputHandle, [In][Out] InputActionSetHandle_t[] handlesOut);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_GetDigitalActionHandle")]
	public static extern ulong ISteamInput_GetDigitalActionHandle(nint instancePtr, InteropHelp.UTF8StringHandle pszActionName);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_GetDigitalActionData")]
	public static extern InputDigitalActionData_t ISteamInput_GetDigitalActionData(nint instancePtr, InputHandle_t inputHandle, InputDigitalActionHandle_t digitalActionHandle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_GetDigitalActionOrigins")]
	public static extern int ISteamInput_GetDigitalActionOrigins(nint instancePtr, InputHandle_t inputHandle, InputActionSetHandle_t actionSetHandle, InputDigitalActionHandle_t digitalActionHandle, [In][Out] EInputActionOrigin[] originsOut);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_GetStringForDigitalActionName")]
	public static extern nint ISteamInput_GetStringForDigitalActionName(nint instancePtr, InputDigitalActionHandle_t eActionHandle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_GetAnalogActionHandle")]
	public static extern ulong ISteamInput_GetAnalogActionHandle(nint instancePtr, InteropHelp.UTF8StringHandle pszActionName);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_GetAnalogActionData")]
	public static extern InputAnalogActionData_t ISteamInput_GetAnalogActionData(nint instancePtr, InputHandle_t inputHandle, InputAnalogActionHandle_t analogActionHandle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_GetAnalogActionOrigins")]
	public static extern int ISteamInput_GetAnalogActionOrigins(nint instancePtr, InputHandle_t inputHandle, InputActionSetHandle_t actionSetHandle, InputAnalogActionHandle_t analogActionHandle, [In][Out] EInputActionOrigin[] originsOut);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_GetGlyphPNGForActionOrigin")]
	public static extern nint ISteamInput_GetGlyphPNGForActionOrigin(nint instancePtr, EInputActionOrigin eOrigin, ESteamInputGlyphSize eSize, uint unFlags);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_GetGlyphSVGForActionOrigin")]
	public static extern nint ISteamInput_GetGlyphSVGForActionOrigin(nint instancePtr, EInputActionOrigin eOrigin, uint unFlags);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_GetGlyphForActionOrigin_Legacy")]
	public static extern nint ISteamInput_GetGlyphForActionOrigin_Legacy(nint instancePtr, EInputActionOrigin eOrigin);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_GetStringForActionOrigin")]
	public static extern nint ISteamInput_GetStringForActionOrigin(nint instancePtr, EInputActionOrigin eOrigin);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_GetStringForAnalogActionName")]
	public static extern nint ISteamInput_GetStringForAnalogActionName(nint instancePtr, InputAnalogActionHandle_t eActionHandle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_StopAnalogActionMomentum")]
	public static extern void ISteamInput_StopAnalogActionMomentum(nint instancePtr, InputHandle_t inputHandle, InputAnalogActionHandle_t eAction);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_GetMotionData")]
	public static extern InputMotionData_t ISteamInput_GetMotionData(nint instancePtr, InputHandle_t inputHandle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_TriggerVibration")]
	public static extern void ISteamInput_TriggerVibration(nint instancePtr, InputHandle_t inputHandle, ushort usLeftSpeed, ushort usRightSpeed);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_TriggerVibrationExtended")]
	public static extern void ISteamInput_TriggerVibrationExtended(nint instancePtr, InputHandle_t inputHandle, ushort usLeftSpeed, ushort usRightSpeed, ushort usLeftTriggerSpeed, ushort usRightTriggerSpeed);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_TriggerSimpleHapticEvent")]
	public static extern void ISteamInput_TriggerSimpleHapticEvent(nint instancePtr, InputHandle_t inputHandle, EControllerHapticLocation eHapticLocation, byte nIntensity, char nGainDB, byte nOtherIntensity, char nOtherGainDB);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_SetLEDColor")]
	public static extern void ISteamInput_SetLEDColor(nint instancePtr, InputHandle_t inputHandle, byte nColorR, byte nColorG, byte nColorB, uint nFlags);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_Legacy_TriggerHapticPulse")]
	public static extern void ISteamInput_Legacy_TriggerHapticPulse(nint instancePtr, InputHandle_t inputHandle, ESteamControllerPad eTargetPad, ushort usDurationMicroSec);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_Legacy_TriggerRepeatedHapticPulse")]
	public static extern void ISteamInput_Legacy_TriggerRepeatedHapticPulse(nint instancePtr, InputHandle_t inputHandle, ESteamControllerPad eTargetPad, ushort usDurationMicroSec, ushort usOffMicroSec, ushort unRepeat, uint nFlags);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_ShowBindingPanel")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamInput_ShowBindingPanel(nint instancePtr, InputHandle_t inputHandle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_GetInputTypeForHandle")]
	public static extern ESteamInputType ISteamInput_GetInputTypeForHandle(nint instancePtr, InputHandle_t inputHandle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_GetControllerForGamepadIndex")]
	public static extern ulong ISteamInput_GetControllerForGamepadIndex(nint instancePtr, int nIndex);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_GetGamepadIndexForController")]
	public static extern int ISteamInput_GetGamepadIndexForController(nint instancePtr, InputHandle_t ulinputHandle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_GetStringForXboxOrigin")]
	public static extern nint ISteamInput_GetStringForXboxOrigin(nint instancePtr, EXboxOrigin eOrigin);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_GetGlyphForXboxOrigin")]
	public static extern nint ISteamInput_GetGlyphForXboxOrigin(nint instancePtr, EXboxOrigin eOrigin);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_GetActionOriginFromXboxOrigin")]
	public static extern EInputActionOrigin ISteamInput_GetActionOriginFromXboxOrigin(nint instancePtr, InputHandle_t inputHandle, EXboxOrigin eOrigin);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_TranslateActionOrigin")]
	public static extern EInputActionOrigin ISteamInput_TranslateActionOrigin(nint instancePtr, ESteamInputType eDestinationInputType, EInputActionOrigin eSourceOrigin);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_GetDeviceBindingRevision")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamInput_GetDeviceBindingRevision(nint instancePtr, InputHandle_t inputHandle, out int pMajor, out int pMinor);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_GetRemotePlaySessionID")]
	public static extern uint ISteamInput_GetRemotePlaySessionID(nint instancePtr, InputHandle_t inputHandle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInput_GetSessionInputConfigurationSettings")]
	public static extern ushort ISteamInput_GetSessionInputConfigurationSettings(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_GetResultStatus")]
	public static extern EResult ISteamInventory_GetResultStatus(nint instancePtr, SteamInventoryResult_t resultHandle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_GetResultItems")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamInventory_GetResultItems(nint instancePtr, SteamInventoryResult_t resultHandle, [In][Out] SteamItemDetails_t[] pOutItemsArray, ref uint punOutItemsArraySize);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_GetResultItemProperty")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamInventory_GetResultItemProperty(nint instancePtr, SteamInventoryResult_t resultHandle, uint unItemIndex, InteropHelp.UTF8StringHandle pchPropertyName, nint pchValueBuffer, ref uint punValueBufferSizeOut);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_GetResultTimestamp")]
	public static extern uint ISteamInventory_GetResultTimestamp(nint instancePtr, SteamInventoryResult_t resultHandle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_CheckResultSteamID")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamInventory_CheckResultSteamID(nint instancePtr, SteamInventoryResult_t resultHandle, CSteamID steamIDExpected);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_DestroyResult")]
	public static extern void ISteamInventory_DestroyResult(nint instancePtr, SteamInventoryResult_t resultHandle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_GetAllItems")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamInventory_GetAllItems(nint instancePtr, out SteamInventoryResult_t pResultHandle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_GetItemsByID")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamInventory_GetItemsByID(nint instancePtr, out SteamInventoryResult_t pResultHandle, [In][Out] SteamItemInstanceID_t[] pInstanceIDs, uint unCountInstanceIDs);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_SerializeResult")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamInventory_SerializeResult(nint instancePtr, SteamInventoryResult_t resultHandle, byte[] pOutBuffer, out uint punOutBufferSize);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_DeserializeResult")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamInventory_DeserializeResult(nint instancePtr, out SteamInventoryResult_t pOutResultHandle, byte[] pBuffer, uint unBufferSize, [MarshalAs(UnmanagedType.I1)] bool bRESERVED_MUST_BE_FALSE);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_GenerateItems")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamInventory_GenerateItems(nint instancePtr, out SteamInventoryResult_t pResultHandle, [In][Out] SteamItemDef_t[] pArrayItemDefs, [In][Out] uint[] punArrayQuantity, uint unArrayLength);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_GrantPromoItems")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamInventory_GrantPromoItems(nint instancePtr, out SteamInventoryResult_t pResultHandle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_AddPromoItem")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamInventory_AddPromoItem(nint instancePtr, out SteamInventoryResult_t pResultHandle, SteamItemDef_t itemDef);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_AddPromoItems")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamInventory_AddPromoItems(nint instancePtr, out SteamInventoryResult_t pResultHandle, [In][Out] SteamItemDef_t[] pArrayItemDefs, uint unArrayLength);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_ConsumeItem")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamInventory_ConsumeItem(nint instancePtr, out SteamInventoryResult_t pResultHandle, SteamItemInstanceID_t itemConsume, uint unQuantity);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_ExchangeItems")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamInventory_ExchangeItems(nint instancePtr, out SteamInventoryResult_t pResultHandle, [In][Out] SteamItemDef_t[] pArrayGenerate, [In][Out] uint[] punArrayGenerateQuantity, uint unArrayGenerateLength, [In][Out] SteamItemInstanceID_t[] pArrayDestroy, [In][Out] uint[] punArrayDestroyQuantity, uint unArrayDestroyLength);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_TransferItemQuantity")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamInventory_TransferItemQuantity(nint instancePtr, out SteamInventoryResult_t pResultHandle, SteamItemInstanceID_t itemIdSource, uint unQuantity, SteamItemInstanceID_t itemIdDest);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_SendItemDropHeartbeat")]
	public static extern void ISteamInventory_SendItemDropHeartbeat(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_TriggerItemDrop")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamInventory_TriggerItemDrop(nint instancePtr, out SteamInventoryResult_t pResultHandle, SteamItemDef_t dropListDefinition);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_TradeItems")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamInventory_TradeItems(nint instancePtr, out SteamInventoryResult_t pResultHandle, CSteamID steamIDTradePartner, [In][Out] SteamItemInstanceID_t[] pArrayGive, [In][Out] uint[] pArrayGiveQuantity, uint nArrayGiveLength, [In][Out] SteamItemInstanceID_t[] pArrayGet, [In][Out] uint[] pArrayGetQuantity, uint nArrayGetLength);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_LoadItemDefinitions")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamInventory_LoadItemDefinitions(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_GetItemDefinitionIDs")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamInventory_GetItemDefinitionIDs(nint instancePtr, [In][Out] SteamItemDef_t[] pItemDefIDs, ref uint punItemDefIDsArraySize);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_GetItemDefinitionProperty")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamInventory_GetItemDefinitionProperty(nint instancePtr, SteamItemDef_t iDefinition, InteropHelp.UTF8StringHandle pchPropertyName, nint pchValueBuffer, ref uint punValueBufferSizeOut);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_RequestEligiblePromoItemDefinitionsIDs")]
	public static extern ulong ISteamInventory_RequestEligiblePromoItemDefinitionsIDs(nint instancePtr, CSteamID steamID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_GetEligiblePromoItemDefinitionIDs")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamInventory_GetEligiblePromoItemDefinitionIDs(nint instancePtr, CSteamID steamID, [In][Out] SteamItemDef_t[] pItemDefIDs, ref uint punItemDefIDsArraySize);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_StartPurchase")]
	public static extern ulong ISteamInventory_StartPurchase(nint instancePtr, [In][Out] SteamItemDef_t[] pArrayItemDefs, [In][Out] uint[] punArrayQuantity, uint unArrayLength);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_RequestPrices")]
	public static extern ulong ISteamInventory_RequestPrices(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_GetNumItemsWithPrices")]
	public static extern uint ISteamInventory_GetNumItemsWithPrices(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_GetItemsWithPrices")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamInventory_GetItemsWithPrices(nint instancePtr, [In][Out] SteamItemDef_t[] pArrayItemDefs, [In][Out] ulong[] pCurrentPrices, [In][Out] ulong[] pBasePrices, uint unArrayLength);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_GetItemPrice")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamInventory_GetItemPrice(nint instancePtr, SteamItemDef_t iDefinition, out ulong pCurrentPrice, out ulong pBasePrice);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_StartUpdateProperties")]
	public static extern ulong ISteamInventory_StartUpdateProperties(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_RemoveProperty")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamInventory_RemoveProperty(nint instancePtr, SteamInventoryUpdateHandle_t handle, SteamItemInstanceID_t nItemID, InteropHelp.UTF8StringHandle pchPropertyName);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_SetPropertyString")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamInventory_SetPropertyString(nint instancePtr, SteamInventoryUpdateHandle_t handle, SteamItemInstanceID_t nItemID, InteropHelp.UTF8StringHandle pchPropertyName, InteropHelp.UTF8StringHandle pchPropertyValue);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_SetPropertyBool")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamInventory_SetPropertyBool(nint instancePtr, SteamInventoryUpdateHandle_t handle, SteamItemInstanceID_t nItemID, InteropHelp.UTF8StringHandle pchPropertyName, [MarshalAs(UnmanagedType.I1)] bool bValue);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_SetPropertyInt64")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamInventory_SetPropertyInt64(nint instancePtr, SteamInventoryUpdateHandle_t handle, SteamItemInstanceID_t nItemID, InteropHelp.UTF8StringHandle pchPropertyName, long nValue);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_SetPropertyFloat")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamInventory_SetPropertyFloat(nint instancePtr, SteamInventoryUpdateHandle_t handle, SteamItemInstanceID_t nItemID, InteropHelp.UTF8StringHandle pchPropertyName, float flValue);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_SubmitUpdateProperties")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamInventory_SubmitUpdateProperties(nint instancePtr, SteamInventoryUpdateHandle_t handle, out SteamInventoryResult_t pResultHandle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamInventory_InspectItem")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamInventory_InspectItem(nint instancePtr, out SteamInventoryResult_t pResultHandle, InteropHelp.UTF8StringHandle pchItemToken);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_GetFavoriteGameCount")]
	public static extern int ISteamMatchmaking_GetFavoriteGameCount(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_GetFavoriteGame")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMatchmaking_GetFavoriteGame(nint instancePtr, int iGame, out AppId_t pnAppID, out uint pnIP, out ushort pnConnPort, out ushort pnQueryPort, out uint punFlags, out uint pRTime32LastPlayedOnServer);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_AddFavoriteGame")]
	public static extern int ISteamMatchmaking_AddFavoriteGame(nint instancePtr, AppId_t nAppID, uint nIP, ushort nConnPort, ushort nQueryPort, uint unFlags, uint rTime32LastPlayedOnServer);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_RemoveFavoriteGame")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMatchmaking_RemoveFavoriteGame(nint instancePtr, AppId_t nAppID, uint nIP, ushort nConnPort, ushort nQueryPort, uint unFlags);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_RequestLobbyList")]
	public static extern ulong ISteamMatchmaking_RequestLobbyList(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_AddRequestLobbyListStringFilter")]
	public static extern void ISteamMatchmaking_AddRequestLobbyListStringFilter(nint instancePtr, InteropHelp.UTF8StringHandle pchKeyToMatch, InteropHelp.UTF8StringHandle pchValueToMatch, ELobbyComparison eComparisonType);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_AddRequestLobbyListNumericalFilter")]
	public static extern void ISteamMatchmaking_AddRequestLobbyListNumericalFilter(nint instancePtr, InteropHelp.UTF8StringHandle pchKeyToMatch, int nValueToMatch, ELobbyComparison eComparisonType);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_AddRequestLobbyListNearValueFilter")]
	public static extern void ISteamMatchmaking_AddRequestLobbyListNearValueFilter(nint instancePtr, InteropHelp.UTF8StringHandle pchKeyToMatch, int nValueToBeCloseTo);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_AddRequestLobbyListFilterSlotsAvailable")]
	public static extern void ISteamMatchmaking_AddRequestLobbyListFilterSlotsAvailable(nint instancePtr, int nSlotsAvailable);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_AddRequestLobbyListDistanceFilter")]
	public static extern void ISteamMatchmaking_AddRequestLobbyListDistanceFilter(nint instancePtr, ELobbyDistanceFilter eLobbyDistanceFilter);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_AddRequestLobbyListResultCountFilter")]
	public static extern void ISteamMatchmaking_AddRequestLobbyListResultCountFilter(nint instancePtr, int cMaxResults);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_AddRequestLobbyListCompatibleMembersFilter")]
	public static extern void ISteamMatchmaking_AddRequestLobbyListCompatibleMembersFilter(nint instancePtr, CSteamID steamIDLobby);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_GetLobbyByIndex")]
	public static extern ulong ISteamMatchmaking_GetLobbyByIndex(nint instancePtr, int iLobby);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_CreateLobby")]
	public static extern ulong ISteamMatchmaking_CreateLobby(nint instancePtr, ELobbyType eLobbyType, int cMaxMembers);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_JoinLobby")]
	public static extern ulong ISteamMatchmaking_JoinLobby(nint instancePtr, CSteamID steamIDLobby);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_LeaveLobby")]
	public static extern void ISteamMatchmaking_LeaveLobby(nint instancePtr, CSteamID steamIDLobby);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_InviteUserToLobby")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMatchmaking_InviteUserToLobby(nint instancePtr, CSteamID steamIDLobby, CSteamID steamIDInvitee);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_GetNumLobbyMembers")]
	public static extern int ISteamMatchmaking_GetNumLobbyMembers(nint instancePtr, CSteamID steamIDLobby);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_GetLobbyMemberByIndex")]
	public static extern ulong ISteamMatchmaking_GetLobbyMemberByIndex(nint instancePtr, CSteamID steamIDLobby, int iMember);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_GetLobbyData")]
	public static extern nint ISteamMatchmaking_GetLobbyData(nint instancePtr, CSteamID steamIDLobby, InteropHelp.UTF8StringHandle pchKey);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_SetLobbyData")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMatchmaking_SetLobbyData(nint instancePtr, CSteamID steamIDLobby, InteropHelp.UTF8StringHandle pchKey, InteropHelp.UTF8StringHandle pchValue);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_GetLobbyDataCount")]
	public static extern int ISteamMatchmaking_GetLobbyDataCount(nint instancePtr, CSteamID steamIDLobby);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_GetLobbyDataByIndex")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMatchmaking_GetLobbyDataByIndex(nint instancePtr, CSteamID steamIDLobby, int iLobbyData, nint pchKey, int cchKeyBufferSize, nint pchValue, int cchValueBufferSize);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_DeleteLobbyData")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMatchmaking_DeleteLobbyData(nint instancePtr, CSteamID steamIDLobby, InteropHelp.UTF8StringHandle pchKey);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_GetLobbyMemberData")]
	public static extern nint ISteamMatchmaking_GetLobbyMemberData(nint instancePtr, CSteamID steamIDLobby, CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchKey);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_SetLobbyMemberData")]
	public static extern void ISteamMatchmaking_SetLobbyMemberData(nint instancePtr, CSteamID steamIDLobby, InteropHelp.UTF8StringHandle pchKey, InteropHelp.UTF8StringHandle pchValue);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_SendLobbyChatMsg")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMatchmaking_SendLobbyChatMsg(nint instancePtr, CSteamID steamIDLobby, byte[] pvMsgBody, int cubMsgBody);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_GetLobbyChatEntry")]
	public static extern int ISteamMatchmaking_GetLobbyChatEntry(nint instancePtr, CSteamID steamIDLobby, int iChatID, out CSteamID pSteamIDUser, byte[] pvData, int cubData, out EChatEntryType peChatEntryType);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_RequestLobbyData")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMatchmaking_RequestLobbyData(nint instancePtr, CSteamID steamIDLobby);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_SetLobbyGameServer")]
	public static extern void ISteamMatchmaking_SetLobbyGameServer(nint instancePtr, CSteamID steamIDLobby, uint unGameServerIP, ushort unGameServerPort, CSteamID steamIDGameServer);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_GetLobbyGameServer")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMatchmaking_GetLobbyGameServer(nint instancePtr, CSteamID steamIDLobby, out uint punGameServerIP, out ushort punGameServerPort, out CSteamID psteamIDGameServer);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_SetLobbyMemberLimit")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMatchmaking_SetLobbyMemberLimit(nint instancePtr, CSteamID steamIDLobby, int cMaxMembers);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_GetLobbyMemberLimit")]
	public static extern int ISteamMatchmaking_GetLobbyMemberLimit(nint instancePtr, CSteamID steamIDLobby);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_SetLobbyType")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMatchmaking_SetLobbyType(nint instancePtr, CSteamID steamIDLobby, ELobbyType eLobbyType);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_SetLobbyJoinable")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMatchmaking_SetLobbyJoinable(nint instancePtr, CSteamID steamIDLobby, [MarshalAs(UnmanagedType.I1)] bool bLobbyJoinable);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_GetLobbyOwner")]
	public static extern ulong ISteamMatchmaking_GetLobbyOwner(nint instancePtr, CSteamID steamIDLobby);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_SetLobbyOwner")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMatchmaking_SetLobbyOwner(nint instancePtr, CSteamID steamIDLobby, CSteamID steamIDNewOwner);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmaking_SetLinkedLobby")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMatchmaking_SetLinkedLobby(nint instancePtr, CSteamID steamIDLobby, CSteamID steamIDLobbyDependent);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmakingServers_RequestInternetServerList")]
	public static extern nint ISteamMatchmakingServers_RequestInternetServerList(nint instancePtr, AppId_t iApp, nint ppchFilters, uint nFilters, nint pRequestServersResponse);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmakingServers_RequestLANServerList")]
	public static extern nint ISteamMatchmakingServers_RequestLANServerList(nint instancePtr, AppId_t iApp, nint pRequestServersResponse);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmakingServers_RequestFriendsServerList")]
	public static extern nint ISteamMatchmakingServers_RequestFriendsServerList(nint instancePtr, AppId_t iApp, nint ppchFilters, uint nFilters, nint pRequestServersResponse);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmakingServers_RequestFavoritesServerList")]
	public static extern nint ISteamMatchmakingServers_RequestFavoritesServerList(nint instancePtr, AppId_t iApp, nint ppchFilters, uint nFilters, nint pRequestServersResponse);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmakingServers_RequestHistoryServerList")]
	public static extern nint ISteamMatchmakingServers_RequestHistoryServerList(nint instancePtr, AppId_t iApp, nint ppchFilters, uint nFilters, nint pRequestServersResponse);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmakingServers_RequestSpectatorServerList")]
	public static extern nint ISteamMatchmakingServers_RequestSpectatorServerList(nint instancePtr, AppId_t iApp, nint ppchFilters, uint nFilters, nint pRequestServersResponse);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmakingServers_ReleaseRequest")]
	public static extern void ISteamMatchmakingServers_ReleaseRequest(nint instancePtr, HServerListRequest hServerListRequest);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmakingServers_GetServerDetails")]
	public static extern nint ISteamMatchmakingServers_GetServerDetails(nint instancePtr, HServerListRequest hRequest, int iServer);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmakingServers_CancelQuery")]
	public static extern void ISteamMatchmakingServers_CancelQuery(nint instancePtr, HServerListRequest hRequest);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmakingServers_RefreshQuery")]
	public static extern void ISteamMatchmakingServers_RefreshQuery(nint instancePtr, HServerListRequest hRequest);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmakingServers_IsRefreshing")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMatchmakingServers_IsRefreshing(nint instancePtr, HServerListRequest hRequest);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmakingServers_GetServerCount")]
	public static extern int ISteamMatchmakingServers_GetServerCount(nint instancePtr, HServerListRequest hRequest);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmakingServers_RefreshServer")]
	public static extern void ISteamMatchmakingServers_RefreshServer(nint instancePtr, HServerListRequest hRequest, int iServer);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmakingServers_PingServer")]
	public static extern int ISteamMatchmakingServers_PingServer(nint instancePtr, uint unIP, ushort usPort, nint pRequestServersResponse);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmakingServers_PlayerDetails")]
	public static extern int ISteamMatchmakingServers_PlayerDetails(nint instancePtr, uint unIP, ushort usPort, nint pRequestServersResponse);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmakingServers_ServerRules")]
	public static extern int ISteamMatchmakingServers_ServerRules(nint instancePtr, uint unIP, ushort usPort, nint pRequestServersResponse);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMatchmakingServers_CancelServerQuery")]
	public static extern void ISteamMatchmakingServers_CancelServerQuery(nint instancePtr, HServerQuery hServerQuery);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameSearch_AddGameSearchParams")]
	public static extern EGameSearchErrorCode_t ISteamGameSearch_AddGameSearchParams(nint instancePtr, InteropHelp.UTF8StringHandle pchKeyToFind, InteropHelp.UTF8StringHandle pchValuesToFind);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameSearch_SearchForGameWithLobby")]
	public static extern EGameSearchErrorCode_t ISteamGameSearch_SearchForGameWithLobby(nint instancePtr, CSteamID steamIDLobby, int nPlayerMin, int nPlayerMax);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameSearch_SearchForGameSolo")]
	public static extern EGameSearchErrorCode_t ISteamGameSearch_SearchForGameSolo(nint instancePtr, int nPlayerMin, int nPlayerMax);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameSearch_AcceptGame")]
	public static extern EGameSearchErrorCode_t ISteamGameSearch_AcceptGame(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameSearch_DeclineGame")]
	public static extern EGameSearchErrorCode_t ISteamGameSearch_DeclineGame(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameSearch_RetrieveConnectionDetails")]
	public static extern EGameSearchErrorCode_t ISteamGameSearch_RetrieveConnectionDetails(nint instancePtr, CSteamID steamIDHost, nint pchConnectionDetails, int cubConnectionDetails);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameSearch_EndGameSearch")]
	public static extern EGameSearchErrorCode_t ISteamGameSearch_EndGameSearch(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameSearch_SetGameHostParams")]
	public static extern EGameSearchErrorCode_t ISteamGameSearch_SetGameHostParams(nint instancePtr, InteropHelp.UTF8StringHandle pchKey, InteropHelp.UTF8StringHandle pchValue);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameSearch_SetConnectionDetails")]
	public static extern EGameSearchErrorCode_t ISteamGameSearch_SetConnectionDetails(nint instancePtr, InteropHelp.UTF8StringHandle pchConnectionDetails, int cubConnectionDetails);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameSearch_RequestPlayersForGame")]
	public static extern EGameSearchErrorCode_t ISteamGameSearch_RequestPlayersForGame(nint instancePtr, int nPlayerMin, int nPlayerMax, int nMaxTeamSize);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameSearch_HostConfirmGameStart")]
	public static extern EGameSearchErrorCode_t ISteamGameSearch_HostConfirmGameStart(nint instancePtr, ulong ullUniqueGameID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameSearch_CancelRequestPlayersForGame")]
	public static extern EGameSearchErrorCode_t ISteamGameSearch_CancelRequestPlayersForGame(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameSearch_SubmitPlayerResult")]
	public static extern EGameSearchErrorCode_t ISteamGameSearch_SubmitPlayerResult(nint instancePtr, ulong ullUniqueGameID, CSteamID steamIDPlayer, EPlayerResult_t EPlayerResult);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamGameSearch_EndGame")]
	public static extern EGameSearchErrorCode_t ISteamGameSearch_EndGame(nint instancePtr, ulong ullUniqueGameID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamParties_GetNumActiveBeacons")]
	public static extern uint ISteamParties_GetNumActiveBeacons(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamParties_GetBeaconByIndex")]
	public static extern ulong ISteamParties_GetBeaconByIndex(nint instancePtr, uint unIndex);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamParties_GetBeaconDetails")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamParties_GetBeaconDetails(nint instancePtr, PartyBeaconID_t ulBeaconID, out CSteamID pSteamIDBeaconOwner, out SteamPartyBeaconLocation_t pLocation, nint pchMetadata, int cchMetadata);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamParties_JoinParty")]
	public static extern ulong ISteamParties_JoinParty(nint instancePtr, PartyBeaconID_t ulBeaconID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamParties_GetNumAvailableBeaconLocations")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamParties_GetNumAvailableBeaconLocations(nint instancePtr, out uint puNumLocations);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamParties_GetAvailableBeaconLocations")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamParties_GetAvailableBeaconLocations(nint instancePtr, [In][Out] SteamPartyBeaconLocation_t[] pLocationList, uint uMaxNumLocations);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamParties_CreateBeacon")]
	public static extern ulong ISteamParties_CreateBeacon(nint instancePtr, uint unOpenSlots, ref SteamPartyBeaconLocation_t pBeaconLocation, InteropHelp.UTF8StringHandle pchConnectString, InteropHelp.UTF8StringHandle pchMetadata);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamParties_OnReservationCompleted")]
	public static extern void ISteamParties_OnReservationCompleted(nint instancePtr, PartyBeaconID_t ulBeacon, CSteamID steamIDUser);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamParties_CancelReservation")]
	public static extern void ISteamParties_CancelReservation(nint instancePtr, PartyBeaconID_t ulBeacon, CSteamID steamIDUser);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamParties_ChangeNumOpenSlots")]
	public static extern ulong ISteamParties_ChangeNumOpenSlots(nint instancePtr, PartyBeaconID_t ulBeacon, uint unOpenSlots);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamParties_DestroyBeacon")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamParties_DestroyBeacon(nint instancePtr, PartyBeaconID_t ulBeacon);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamParties_GetBeaconLocationData")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamParties_GetBeaconLocationData(nint instancePtr, SteamPartyBeaconLocation_t BeaconLocation, ESteamPartyBeaconLocationData eData, nint pchDataStringOut, int cchDataStringOut);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusic_BIsEnabled")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMusic_BIsEnabled(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusic_BIsPlaying")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMusic_BIsPlaying(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusic_GetPlaybackStatus")]
	public static extern AudioPlayback_Status ISteamMusic_GetPlaybackStatus(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusic_Play")]
	public static extern void ISteamMusic_Play(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusic_Pause")]
	public static extern void ISteamMusic_Pause(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusic_PlayPrevious")]
	public static extern void ISteamMusic_PlayPrevious(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusic_PlayNext")]
	public static extern void ISteamMusic_PlayNext(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusic_SetVolume")]
	public static extern void ISteamMusic_SetVolume(nint instancePtr, float flVolume);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusic_GetVolume")]
	public static extern float ISteamMusic_GetVolume(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusicRemote_RegisterSteamMusicRemote")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMusicRemote_RegisterSteamMusicRemote(nint instancePtr, InteropHelp.UTF8StringHandle pchName);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusicRemote_DeregisterSteamMusicRemote")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMusicRemote_DeregisterSteamMusicRemote(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusicRemote_BIsCurrentMusicRemote")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMusicRemote_BIsCurrentMusicRemote(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusicRemote_BActivationSuccess")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMusicRemote_BActivationSuccess(nint instancePtr, [MarshalAs(UnmanagedType.I1)] bool bValue);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusicRemote_SetDisplayName")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMusicRemote_SetDisplayName(nint instancePtr, InteropHelp.UTF8StringHandle pchDisplayName);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusicRemote_SetPNGIcon_64x64")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMusicRemote_SetPNGIcon_64x64(nint instancePtr, byte[] pvBuffer, uint cbBufferLength);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusicRemote_EnablePlayPrevious")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMusicRemote_EnablePlayPrevious(nint instancePtr, [MarshalAs(UnmanagedType.I1)] bool bValue);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusicRemote_EnablePlayNext")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMusicRemote_EnablePlayNext(nint instancePtr, [MarshalAs(UnmanagedType.I1)] bool bValue);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusicRemote_EnableShuffled")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMusicRemote_EnableShuffled(nint instancePtr, [MarshalAs(UnmanagedType.I1)] bool bValue);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusicRemote_EnableLooped")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMusicRemote_EnableLooped(nint instancePtr, [MarshalAs(UnmanagedType.I1)] bool bValue);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusicRemote_EnableQueue")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMusicRemote_EnableQueue(nint instancePtr, [MarshalAs(UnmanagedType.I1)] bool bValue);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusicRemote_EnablePlaylists")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMusicRemote_EnablePlaylists(nint instancePtr, [MarshalAs(UnmanagedType.I1)] bool bValue);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusicRemote_UpdatePlaybackStatus")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMusicRemote_UpdatePlaybackStatus(nint instancePtr, AudioPlayback_Status nStatus);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusicRemote_UpdateShuffled")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMusicRemote_UpdateShuffled(nint instancePtr, [MarshalAs(UnmanagedType.I1)] bool bValue);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusicRemote_UpdateLooped")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMusicRemote_UpdateLooped(nint instancePtr, [MarshalAs(UnmanagedType.I1)] bool bValue);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusicRemote_UpdateVolume")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMusicRemote_UpdateVolume(nint instancePtr, float flValue);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusicRemote_CurrentEntryWillChange")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMusicRemote_CurrentEntryWillChange(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusicRemote_CurrentEntryIsAvailable")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMusicRemote_CurrentEntryIsAvailable(nint instancePtr, [MarshalAs(UnmanagedType.I1)] bool bAvailable);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusicRemote_UpdateCurrentEntryText")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMusicRemote_UpdateCurrentEntryText(nint instancePtr, InteropHelp.UTF8StringHandle pchText);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusicRemote_UpdateCurrentEntryElapsedSeconds")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMusicRemote_UpdateCurrentEntryElapsedSeconds(nint instancePtr, int nValue);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusicRemote_UpdateCurrentEntryCoverArt")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMusicRemote_UpdateCurrentEntryCoverArt(nint instancePtr, byte[] pvBuffer, uint cbBufferLength);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusicRemote_CurrentEntryDidChange")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMusicRemote_CurrentEntryDidChange(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusicRemote_QueueWillChange")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMusicRemote_QueueWillChange(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusicRemote_ResetQueueEntries")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMusicRemote_ResetQueueEntries(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusicRemote_SetQueueEntry")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMusicRemote_SetQueueEntry(nint instancePtr, int nID, int nPosition, InteropHelp.UTF8StringHandle pchEntryText);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusicRemote_SetCurrentQueueEntry")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMusicRemote_SetCurrentQueueEntry(nint instancePtr, int nID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusicRemote_QueueDidChange")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMusicRemote_QueueDidChange(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusicRemote_PlaylistWillChange")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMusicRemote_PlaylistWillChange(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusicRemote_ResetPlaylistEntries")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMusicRemote_ResetPlaylistEntries(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusicRemote_SetPlaylistEntry")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMusicRemote_SetPlaylistEntry(nint instancePtr, int nID, int nPosition, InteropHelp.UTF8StringHandle pchEntryText);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusicRemote_SetCurrentPlaylistEntry")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMusicRemote_SetCurrentPlaylistEntry(nint instancePtr, int nID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamMusicRemote_PlaylistDidChange")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamMusicRemote_PlaylistDidChange(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworking_SendP2PPacket")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworking_SendP2PPacket(nint instancePtr, CSteamID steamIDRemote, byte[] pubData, uint cubData, EP2PSend eP2PSendType, int nChannel);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworking_IsP2PPacketAvailable")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworking_IsP2PPacketAvailable(nint instancePtr, out uint pcubMsgSize, int nChannel);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworking_ReadP2PPacket")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworking_ReadP2PPacket(nint instancePtr, byte[] pubDest, uint cubDest, out uint pcubMsgSize, out CSteamID psteamIDRemote, int nChannel);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworking_AcceptP2PSessionWithUser")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworking_AcceptP2PSessionWithUser(nint instancePtr, CSteamID steamIDRemote);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworking_CloseP2PSessionWithUser")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworking_CloseP2PSessionWithUser(nint instancePtr, CSteamID steamIDRemote);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworking_CloseP2PChannelWithUser")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworking_CloseP2PChannelWithUser(nint instancePtr, CSteamID steamIDRemote, int nChannel);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworking_GetP2PSessionState")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworking_GetP2PSessionState(nint instancePtr, CSteamID steamIDRemote, out P2PSessionState_t pConnectionState);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworking_AllowP2PPacketRelay")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworking_AllowP2PPacketRelay(nint instancePtr, [MarshalAs(UnmanagedType.I1)] bool bAllow);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworking_CreateListenSocket")]
	public static extern uint ISteamNetworking_CreateListenSocket(nint instancePtr, int nVirtualP2PPort, SteamIPAddress_t nIP, ushort nPort, [MarshalAs(UnmanagedType.I1)] bool bAllowUseOfPacketRelay);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworking_CreateP2PConnectionSocket")]
	public static extern uint ISteamNetworking_CreateP2PConnectionSocket(nint instancePtr, CSteamID steamIDTarget, int nVirtualPort, int nTimeoutSec, [MarshalAs(UnmanagedType.I1)] bool bAllowUseOfPacketRelay);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworking_CreateConnectionSocket")]
	public static extern uint ISteamNetworking_CreateConnectionSocket(nint instancePtr, SteamIPAddress_t nIP, ushort nPort, int nTimeoutSec);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworking_DestroySocket")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworking_DestroySocket(nint instancePtr, SNetSocket_t hSocket, [MarshalAs(UnmanagedType.I1)] bool bNotifyRemoteEnd);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworking_DestroyListenSocket")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworking_DestroyListenSocket(nint instancePtr, SNetListenSocket_t hSocket, [MarshalAs(UnmanagedType.I1)] bool bNotifyRemoteEnd);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworking_SendDataOnSocket")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworking_SendDataOnSocket(nint instancePtr, SNetSocket_t hSocket, byte[] pubData, uint cubData, [MarshalAs(UnmanagedType.I1)] bool bReliable);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworking_IsDataAvailableOnSocket")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworking_IsDataAvailableOnSocket(nint instancePtr, SNetSocket_t hSocket, out uint pcubMsgSize);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworking_RetrieveDataFromSocket")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworking_RetrieveDataFromSocket(nint instancePtr, SNetSocket_t hSocket, byte[] pubDest, uint cubDest, out uint pcubMsgSize);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworking_IsDataAvailable")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworking_IsDataAvailable(nint instancePtr, SNetListenSocket_t hListenSocket, out uint pcubMsgSize, out SNetSocket_t phSocket);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworking_RetrieveData")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworking_RetrieveData(nint instancePtr, SNetListenSocket_t hListenSocket, byte[] pubDest, uint cubDest, out uint pcubMsgSize, out SNetSocket_t phSocket);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworking_GetSocketInfo")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworking_GetSocketInfo(nint instancePtr, SNetSocket_t hSocket, out CSteamID pSteamIDRemote, out int peSocketStatus, out SteamIPAddress_t punIPRemote, out ushort punPortRemote);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworking_GetListenSocketInfo")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworking_GetListenSocketInfo(nint instancePtr, SNetListenSocket_t hListenSocket, out SteamIPAddress_t pnIP, out ushort pnPort);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworking_GetSocketConnectionType")]
	public static extern ESNetSocketConnectionType ISteamNetworking_GetSocketConnectionType(nint instancePtr, SNetSocket_t hSocket);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworking_GetMaxPacketSize")]
	public static extern int ISteamNetworking_GetMaxPacketSize(nint instancePtr, SNetSocket_t hSocket);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingMessages_SendMessageToUser")]
	public static extern EResult ISteamNetworkingMessages_SendMessageToUser(nint instancePtr, ref SteamNetworkingIdentity identityRemote, nint pubData, uint cubData, int nSendFlags, int nRemoteChannel);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingMessages_ReceiveMessagesOnChannel")]
	public static extern int ISteamNetworkingMessages_ReceiveMessagesOnChannel(nint instancePtr, int nLocalChannel, [In][Out] nint[] ppOutMessages, int nMaxMessages);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingMessages_AcceptSessionWithUser")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworkingMessages_AcceptSessionWithUser(nint instancePtr, ref SteamNetworkingIdentity identityRemote);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingMessages_CloseSessionWithUser")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworkingMessages_CloseSessionWithUser(nint instancePtr, ref SteamNetworkingIdentity identityRemote);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingMessages_CloseChannelWithUser")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworkingMessages_CloseChannelWithUser(nint instancePtr, ref SteamNetworkingIdentity identityRemote, int nLocalChannel);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingMessages_GetSessionConnectionInfo")]
	public static extern ESteamNetworkingConnectionState ISteamNetworkingMessages_GetSessionConnectionInfo(nint instancePtr, ref SteamNetworkingIdentity identityRemote, out SteamNetConnectionInfo_t pConnectionInfo, out SteamNetConnectionRealTimeStatus_t pQuickStatus);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_CreateListenSocketIP")]
	public static extern uint ISteamNetworkingSockets_CreateListenSocketIP(nint instancePtr, ref SteamNetworkingIPAddr localAddress, int nOptions, [In][Out] SteamNetworkingConfigValue_t[] pOptions);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_ConnectByIPAddress")]
	public static extern uint ISteamNetworkingSockets_ConnectByIPAddress(nint instancePtr, ref SteamNetworkingIPAddr address, int nOptions, [In][Out] SteamNetworkingConfigValue_t[] pOptions);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_CreateListenSocketP2P")]
	public static extern uint ISteamNetworkingSockets_CreateListenSocketP2P(nint instancePtr, int nLocalVirtualPort, int nOptions, [In][Out] SteamNetworkingConfigValue_t[] pOptions);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_ConnectP2P")]
	public static extern uint ISteamNetworkingSockets_ConnectP2P(nint instancePtr, ref SteamNetworkingIdentity identityRemote, int nRemoteVirtualPort, int nOptions, [In][Out] SteamNetworkingConfigValue_t[] pOptions);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_AcceptConnection")]
	public static extern EResult ISteamNetworkingSockets_AcceptConnection(nint instancePtr, HSteamNetConnection hConn);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_CloseConnection")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworkingSockets_CloseConnection(nint instancePtr, HSteamNetConnection hPeer, int nReason, InteropHelp.UTF8StringHandle pszDebug, [MarshalAs(UnmanagedType.I1)] bool bEnableLinger);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_CloseListenSocket")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworkingSockets_CloseListenSocket(nint instancePtr, HSteamListenSocket hSocket);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_SetConnectionUserData")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworkingSockets_SetConnectionUserData(nint instancePtr, HSteamNetConnection hPeer, long nUserData);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_GetConnectionUserData")]
	public static extern long ISteamNetworkingSockets_GetConnectionUserData(nint instancePtr, HSteamNetConnection hPeer);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_SetConnectionName")]
	public static extern void ISteamNetworkingSockets_SetConnectionName(nint instancePtr, HSteamNetConnection hPeer, InteropHelp.UTF8StringHandle pszName);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_GetConnectionName")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworkingSockets_GetConnectionName(nint instancePtr, HSteamNetConnection hPeer, nint pszName, int nMaxLen);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_SendMessageToConnection")]
	public static extern EResult ISteamNetworkingSockets_SendMessageToConnection(nint instancePtr, HSteamNetConnection hConn, nint pData, uint cbData, int nSendFlags, out long pOutMessageNumber);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_SendMessages")]
	public static extern void ISteamNetworkingSockets_SendMessages(nint instancePtr, int nMessages, [In][Out] SteamNetworkingMessage_t[] pMessages, [In][Out] long[] pOutMessageNumberOrResult);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_FlushMessagesOnConnection")]
	public static extern EResult ISteamNetworkingSockets_FlushMessagesOnConnection(nint instancePtr, HSteamNetConnection hConn);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_ReceiveMessagesOnConnection")]
	public static extern int ISteamNetworkingSockets_ReceiveMessagesOnConnection(nint instancePtr, HSteamNetConnection hConn, [In][Out] nint[] ppOutMessages, int nMaxMessages);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_GetConnectionInfo")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworkingSockets_GetConnectionInfo(nint instancePtr, HSteamNetConnection hConn, out SteamNetConnectionInfo_t pInfo);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_GetConnectionRealTimeStatus")]
	public static extern EResult ISteamNetworkingSockets_GetConnectionRealTimeStatus(nint instancePtr, HSteamNetConnection hConn, ref SteamNetConnectionRealTimeStatus_t pStatus, int nLanes, ref SteamNetConnectionRealTimeLaneStatus_t pLanes);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_GetDetailedConnectionStatus")]
	public static extern int ISteamNetworkingSockets_GetDetailedConnectionStatus(nint instancePtr, HSteamNetConnection hConn, nint pszBuf, int cbBuf);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_GetListenSocketAddress")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworkingSockets_GetListenSocketAddress(nint instancePtr, HSteamListenSocket hSocket, out SteamNetworkingIPAddr address);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_CreateSocketPair")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworkingSockets_CreateSocketPair(nint instancePtr, out HSteamNetConnection pOutConnection1, out HSteamNetConnection pOutConnection2, [MarshalAs(UnmanagedType.I1)] bool bUseNetworkLoopback, ref SteamNetworkingIdentity pIdentity1, ref SteamNetworkingIdentity pIdentity2);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_ConfigureConnectionLanes")]
	public static extern EResult ISteamNetworkingSockets_ConfigureConnectionLanes(nint instancePtr, HSteamNetConnection hConn, int nNumLanes, out int pLanePriorities, out ushort pLaneWeights);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_GetIdentity")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworkingSockets_GetIdentity(nint instancePtr, out SteamNetworkingIdentity pIdentity);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_InitAuthentication")]
	public static extern ESteamNetworkingAvailability ISteamNetworkingSockets_InitAuthentication(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_GetAuthenticationStatus")]
	public static extern ESteamNetworkingAvailability ISteamNetworkingSockets_GetAuthenticationStatus(nint instancePtr, out SteamNetAuthenticationStatus_t pDetails);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_CreatePollGroup")]
	public static extern uint ISteamNetworkingSockets_CreatePollGroup(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_DestroyPollGroup")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworkingSockets_DestroyPollGroup(nint instancePtr, HSteamNetPollGroup hPollGroup);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_SetConnectionPollGroup")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworkingSockets_SetConnectionPollGroup(nint instancePtr, HSteamNetConnection hConn, HSteamNetPollGroup hPollGroup);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_ReceiveMessagesOnPollGroup")]
	public static extern int ISteamNetworkingSockets_ReceiveMessagesOnPollGroup(nint instancePtr, HSteamNetPollGroup hPollGroup, [In][Out] nint[] ppOutMessages, int nMaxMessages);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_ReceivedRelayAuthTicket")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworkingSockets_ReceivedRelayAuthTicket(nint instancePtr, nint pvTicket, int cbTicket, out SteamDatagramRelayAuthTicket pOutParsedTicket);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_FindRelayAuthTicketForServer")]
	public static extern int ISteamNetworkingSockets_FindRelayAuthTicketForServer(nint instancePtr, ref SteamNetworkingIdentity identityGameServer, int nRemoteVirtualPort, out SteamDatagramRelayAuthTicket pOutParsedTicket);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_ConnectToHostedDedicatedServer")]
	public static extern uint ISteamNetworkingSockets_ConnectToHostedDedicatedServer(nint instancePtr, ref SteamNetworkingIdentity identityTarget, int nRemoteVirtualPort, int nOptions, [In][Out] SteamNetworkingConfigValue_t[] pOptions);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_GetHostedDedicatedServerPort")]
	public static extern ushort ISteamNetworkingSockets_GetHostedDedicatedServerPort(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_GetHostedDedicatedServerPOPID")]
	public static extern uint ISteamNetworkingSockets_GetHostedDedicatedServerPOPID(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_GetHostedDedicatedServerAddress")]
	public static extern EResult ISteamNetworkingSockets_GetHostedDedicatedServerAddress(nint instancePtr, out SteamDatagramHostedAddress pRouting);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_CreateHostedDedicatedServerListenSocket")]
	public static extern uint ISteamNetworkingSockets_CreateHostedDedicatedServerListenSocket(nint instancePtr, int nLocalVirtualPort, int nOptions, [In][Out] SteamNetworkingConfigValue_t[] pOptions);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_GetGameCoordinatorServerLogin")]
	public static extern EResult ISteamNetworkingSockets_GetGameCoordinatorServerLogin(nint instancePtr, nint pLoginInfo, out int pcbSignedBlob, nint pBlob);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_ConnectP2PCustomSignaling")]
	public static extern uint ISteamNetworkingSockets_ConnectP2PCustomSignaling(nint instancePtr, out ISteamNetworkingConnectionSignaling pSignaling, ref SteamNetworkingIdentity pPeerIdentity, int nRemoteVirtualPort, int nOptions, [In][Out] SteamNetworkingConfigValue_t[] pOptions);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_ReceivedP2PCustomSignal")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworkingSockets_ReceivedP2PCustomSignal(nint instancePtr, nint pMsg, int cbMsg, out ISteamNetworkingSignalingRecvContext pContext);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_GetCertificateRequest")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworkingSockets_GetCertificateRequest(nint instancePtr, out int pcbBlob, nint pBlob, out SteamNetworkingErrMsg errMsg);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_SetCertificate")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworkingSockets_SetCertificate(nint instancePtr, nint pCertificate, int cbCertificate, out SteamNetworkingErrMsg errMsg);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_ResetIdentity")]
	public static extern void ISteamNetworkingSockets_ResetIdentity(nint instancePtr, ref SteamNetworkingIdentity pIdentity);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_RunCallbacks")]
	public static extern void ISteamNetworkingSockets_RunCallbacks(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_BeginAsyncRequestFakeIP")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworkingSockets_BeginAsyncRequestFakeIP(nint instancePtr, int nNumPorts);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_GetFakeIP")]
	public static extern void ISteamNetworkingSockets_GetFakeIP(nint instancePtr, int idxFirstPort, out SteamNetworkingFakeIPResult_t pInfo);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_CreateListenSocketP2PFakeIP")]
	public static extern uint ISteamNetworkingSockets_CreateListenSocketP2PFakeIP(nint instancePtr, int idxFakePort, int nOptions, [In][Out] SteamNetworkingConfigValue_t[] pOptions);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_GetRemoteFakeIPForConnection")]
	public static extern EResult ISteamNetworkingSockets_GetRemoteFakeIPForConnection(nint instancePtr, HSteamNetConnection hConn, out SteamNetworkingIPAddr pOutAddr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingSockets_CreateFakeUDPPort")]
	public static extern nint ISteamNetworkingSockets_CreateFakeUDPPort(nint instancePtr, int idxFakeServerPort);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingUtils_AllocateMessage")]
	public static extern nint ISteamNetworkingUtils_AllocateMessage(nint instancePtr, int cbAllocateBuffer);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingUtils_InitRelayNetworkAccess")]
	public static extern void ISteamNetworkingUtils_InitRelayNetworkAccess(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingUtils_GetRelayNetworkStatus")]
	public static extern ESteamNetworkingAvailability ISteamNetworkingUtils_GetRelayNetworkStatus(nint instancePtr, out SteamRelayNetworkStatus_t pDetails);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingUtils_GetLocalPingLocation")]
	public static extern float ISteamNetworkingUtils_GetLocalPingLocation(nint instancePtr, out SteamNetworkPingLocation_t result);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingUtils_EstimatePingTimeBetweenTwoLocations")]
	public static extern int ISteamNetworkingUtils_EstimatePingTimeBetweenTwoLocations(nint instancePtr, ref SteamNetworkPingLocation_t location1, ref SteamNetworkPingLocation_t location2);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingUtils_EstimatePingTimeFromLocalHost")]
	public static extern int ISteamNetworkingUtils_EstimatePingTimeFromLocalHost(nint instancePtr, ref SteamNetworkPingLocation_t remoteLocation);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingUtils_ConvertPingLocationToString")]
	public static extern void ISteamNetworkingUtils_ConvertPingLocationToString(nint instancePtr, ref SteamNetworkPingLocation_t location, nint pszBuf, int cchBufSize);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingUtils_ParsePingLocationString")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworkingUtils_ParsePingLocationString(nint instancePtr, InteropHelp.UTF8StringHandle pszString, out SteamNetworkPingLocation_t result);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingUtils_CheckPingDataUpToDate")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworkingUtils_CheckPingDataUpToDate(nint instancePtr, float flMaxAgeSeconds);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingUtils_GetPingToDataCenter")]
	public static extern int ISteamNetworkingUtils_GetPingToDataCenter(nint instancePtr, SteamNetworkingPOPID popID, out SteamNetworkingPOPID pViaRelayPoP);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingUtils_GetDirectPingToPOP")]
	public static extern int ISteamNetworkingUtils_GetDirectPingToPOP(nint instancePtr, SteamNetworkingPOPID popID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingUtils_GetPOPCount")]
	public static extern int ISteamNetworkingUtils_GetPOPCount(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingUtils_GetPOPList")]
	public static extern int ISteamNetworkingUtils_GetPOPList(nint instancePtr, out SteamNetworkingPOPID list, int nListSz);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingUtils_GetLocalTimestamp")]
	public static extern long ISteamNetworkingUtils_GetLocalTimestamp(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingUtils_SetDebugOutputFunction")]
	public static extern void ISteamNetworkingUtils_SetDebugOutputFunction(nint instancePtr, ESteamNetworkingSocketsDebugOutputType eDetailLevel, FSteamNetworkingSocketsDebugOutput pfnFunc);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingUtils_IsFakeIPv4")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworkingUtils_IsFakeIPv4(nint instancePtr, uint nIPv4);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingUtils_GetIPv4FakeIPType")]
	public static extern ESteamNetworkingFakeIPType ISteamNetworkingUtils_GetIPv4FakeIPType(nint instancePtr, uint nIPv4);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingUtils_GetRealIdentityForFakeIP")]
	public static extern EResult ISteamNetworkingUtils_GetRealIdentityForFakeIP(nint instancePtr, ref SteamNetworkingIPAddr fakeIP, out SteamNetworkingIdentity pOutRealIdentity);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingUtils_SetConfigValue")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworkingUtils_SetConfigValue(nint instancePtr, ESteamNetworkingConfigValue eValue, ESteamNetworkingConfigScope eScopeType, nint scopeObj, ESteamNetworkingConfigDataType eDataType, nint pArg);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingUtils_GetConfigValue")]
	public static extern ESteamNetworkingGetConfigValueResult ISteamNetworkingUtils_GetConfigValue(nint instancePtr, ESteamNetworkingConfigValue eValue, ESteamNetworkingConfigScope eScopeType, nint scopeObj, out ESteamNetworkingConfigDataType pOutDataType, nint pResult, ref ulong cbResult);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingUtils_GetConfigValueInfo")]
	public static extern nint ISteamNetworkingUtils_GetConfigValueInfo(nint instancePtr, ESteamNetworkingConfigValue eValue, out ESteamNetworkingConfigDataType pOutDataType, out ESteamNetworkingConfigScope pOutScope);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingUtils_IterateGenericEditableConfigValues")]
	public static extern ESteamNetworkingConfigValue ISteamNetworkingUtils_IterateGenericEditableConfigValues(nint instancePtr, ESteamNetworkingConfigValue eCurrent, [MarshalAs(UnmanagedType.I1)] bool bEnumerateDevVars);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingUtils_SteamNetworkingIPAddr_ToString")]
	public static extern void ISteamNetworkingUtils_SteamNetworkingIPAddr_ToString(nint instancePtr, ref SteamNetworkingIPAddr addr, nint buf, uint cbBuf, [MarshalAs(UnmanagedType.I1)] bool bWithPort);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingUtils_SteamNetworkingIPAddr_ParseString")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworkingUtils_SteamNetworkingIPAddr_ParseString(nint instancePtr, out SteamNetworkingIPAddr pAddr, InteropHelp.UTF8StringHandle pszStr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingUtils_SteamNetworkingIPAddr_GetFakeIPType")]
	public static extern ESteamNetworkingFakeIPType ISteamNetworkingUtils_SteamNetworkingIPAddr_GetFakeIPType(nint instancePtr, ref SteamNetworkingIPAddr addr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingUtils_SteamNetworkingIdentity_ToString")]
	public static extern void ISteamNetworkingUtils_SteamNetworkingIdentity_ToString(nint instancePtr, ref SteamNetworkingIdentity identity, nint buf, uint cbBuf);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamNetworkingUtils_SteamNetworkingIdentity_ParseString")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamNetworkingUtils_SteamNetworkingIdentity_ParseString(nint instancePtr, out SteamNetworkingIdentity pIdentity, InteropHelp.UTF8StringHandle pszStr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamParentalSettings_BIsParentalLockEnabled")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamParentalSettings_BIsParentalLockEnabled(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamParentalSettings_BIsParentalLockLocked")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamParentalSettings_BIsParentalLockLocked(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamParentalSettings_BIsAppBlocked")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamParentalSettings_BIsAppBlocked(nint instancePtr, AppId_t nAppID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamParentalSettings_BIsAppInBlockList")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamParentalSettings_BIsAppInBlockList(nint instancePtr, AppId_t nAppID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamParentalSettings_BIsFeatureBlocked")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamParentalSettings_BIsFeatureBlocked(nint instancePtr, EParentalFeature eFeature);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamParentalSettings_BIsFeatureInBlockList")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamParentalSettings_BIsFeatureInBlockList(nint instancePtr, EParentalFeature eFeature);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemotePlay_GetSessionCount")]
	public static extern uint ISteamRemotePlay_GetSessionCount(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemotePlay_GetSessionID")]
	public static extern uint ISteamRemotePlay_GetSessionID(nint instancePtr, int iSessionIndex);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemotePlay_GetSessionSteamID")]
	public static extern ulong ISteamRemotePlay_GetSessionSteamID(nint instancePtr, RemotePlaySessionID_t unSessionID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemotePlay_GetSessionClientName")]
	public static extern nint ISteamRemotePlay_GetSessionClientName(nint instancePtr, RemotePlaySessionID_t unSessionID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemotePlay_GetSessionClientFormFactor")]
	public static extern ESteamDeviceFormFactor ISteamRemotePlay_GetSessionClientFormFactor(nint instancePtr, RemotePlaySessionID_t unSessionID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemotePlay_BGetSessionClientResolution")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamRemotePlay_BGetSessionClientResolution(nint instancePtr, RemotePlaySessionID_t unSessionID, out int pnResolutionX, out int pnResolutionY);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemotePlay_BSendRemotePlayTogetherInvite")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamRemotePlay_BSendRemotePlayTogetherInvite(nint instancePtr, CSteamID steamIDFriend);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileWrite")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamRemoteStorage_FileWrite(nint instancePtr, InteropHelp.UTF8StringHandle pchFile, byte[] pvData, int cubData);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileRead")]
	public static extern int ISteamRemoteStorage_FileRead(nint instancePtr, InteropHelp.UTF8StringHandle pchFile, byte[] pvData, int cubDataToRead);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileWriteAsync")]
	public static extern ulong ISteamRemoteStorage_FileWriteAsync(nint instancePtr, InteropHelp.UTF8StringHandle pchFile, byte[] pvData, uint cubData);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileReadAsync")]
	public static extern ulong ISteamRemoteStorage_FileReadAsync(nint instancePtr, InteropHelp.UTF8StringHandle pchFile, uint nOffset, uint cubToRead);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileReadAsyncComplete")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamRemoteStorage_FileReadAsyncComplete(nint instancePtr, SteamAPICall_t hReadCall, byte[] pvBuffer, uint cubToRead);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileForget")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamRemoteStorage_FileForget(nint instancePtr, InteropHelp.UTF8StringHandle pchFile);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileDelete")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamRemoteStorage_FileDelete(nint instancePtr, InteropHelp.UTF8StringHandle pchFile);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileShare")]
	public static extern ulong ISteamRemoteStorage_FileShare(nint instancePtr, InteropHelp.UTF8StringHandle pchFile);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_SetSyncPlatforms")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamRemoteStorage_SetSyncPlatforms(nint instancePtr, InteropHelp.UTF8StringHandle pchFile, ERemoteStoragePlatform eRemoteStoragePlatform);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileWriteStreamOpen")]
	public static extern ulong ISteamRemoteStorage_FileWriteStreamOpen(nint instancePtr, InteropHelp.UTF8StringHandle pchFile);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileWriteStreamWriteChunk")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamRemoteStorage_FileWriteStreamWriteChunk(nint instancePtr, UGCFileWriteStreamHandle_t writeHandle, byte[] pvData, int cubData);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileWriteStreamClose")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamRemoteStorage_FileWriteStreamClose(nint instancePtr, UGCFileWriteStreamHandle_t writeHandle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileWriteStreamCancel")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamRemoteStorage_FileWriteStreamCancel(nint instancePtr, UGCFileWriteStreamHandle_t writeHandle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileExists")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamRemoteStorage_FileExists(nint instancePtr, InteropHelp.UTF8StringHandle pchFile);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_FilePersisted")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamRemoteStorage_FilePersisted(nint instancePtr, InteropHelp.UTF8StringHandle pchFile);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetFileSize")]
	public static extern int ISteamRemoteStorage_GetFileSize(nint instancePtr, InteropHelp.UTF8StringHandle pchFile);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetFileTimestamp")]
	public static extern long ISteamRemoteStorage_GetFileTimestamp(nint instancePtr, InteropHelp.UTF8StringHandle pchFile);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetSyncPlatforms")]
	public static extern ERemoteStoragePlatform ISteamRemoteStorage_GetSyncPlatforms(nint instancePtr, InteropHelp.UTF8StringHandle pchFile);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetFileCount")]
	public static extern int ISteamRemoteStorage_GetFileCount(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetFileNameAndSize")]
	public static extern nint ISteamRemoteStorage_GetFileNameAndSize(nint instancePtr, int iFile, out int pnFileSizeInBytes);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetQuota")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamRemoteStorage_GetQuota(nint instancePtr, out ulong pnTotalBytes, out ulong puAvailableBytes);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_IsCloudEnabledForAccount")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamRemoteStorage_IsCloudEnabledForAccount(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_IsCloudEnabledForApp")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamRemoteStorage_IsCloudEnabledForApp(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_SetCloudEnabledForApp")]
	public static extern void ISteamRemoteStorage_SetCloudEnabledForApp(nint instancePtr, [MarshalAs(UnmanagedType.I1)] bool bEnabled);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_UGCDownload")]
	public static extern ulong ISteamRemoteStorage_UGCDownload(nint instancePtr, UGCHandle_t hContent, uint unPriority);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetUGCDownloadProgress")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamRemoteStorage_GetUGCDownloadProgress(nint instancePtr, UGCHandle_t hContent, out int pnBytesDownloaded, out int pnBytesExpected);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetUGCDetails")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamRemoteStorage_GetUGCDetails(nint instancePtr, UGCHandle_t hContent, out AppId_t pnAppID, out nint ppchName, out int pnFileSizeInBytes, out CSteamID pSteamIDOwner);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_UGCRead")]
	public static extern int ISteamRemoteStorage_UGCRead(nint instancePtr, UGCHandle_t hContent, byte[] pvData, int cubDataToRead, uint cOffset, EUGCReadAction eAction);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetCachedUGCCount")]
	public static extern int ISteamRemoteStorage_GetCachedUGCCount(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetCachedUGCHandle")]
	public static extern ulong ISteamRemoteStorage_GetCachedUGCHandle(nint instancePtr, int iCachedContent);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_PublishWorkshopFile")]
	public static extern ulong ISteamRemoteStorage_PublishWorkshopFile(nint instancePtr, InteropHelp.UTF8StringHandle pchFile, InteropHelp.UTF8StringHandle pchPreviewFile, AppId_t nConsumerAppId, InteropHelp.UTF8StringHandle pchTitle, InteropHelp.UTF8StringHandle pchDescription, ERemoteStoragePublishedFileVisibility eVisibility, nint pTags, EWorkshopFileType eWorkshopFileType);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_CreatePublishedFileUpdateRequest")]
	public static extern ulong ISteamRemoteStorage_CreatePublishedFileUpdateRequest(nint instancePtr, PublishedFileId_t unPublishedFileId);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_UpdatePublishedFileFile")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamRemoteStorage_UpdatePublishedFileFile(nint instancePtr, PublishedFileUpdateHandle_t updateHandle, InteropHelp.UTF8StringHandle pchFile);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_UpdatePublishedFilePreviewFile")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamRemoteStorage_UpdatePublishedFilePreviewFile(nint instancePtr, PublishedFileUpdateHandle_t updateHandle, InteropHelp.UTF8StringHandle pchPreviewFile);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_UpdatePublishedFileTitle")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamRemoteStorage_UpdatePublishedFileTitle(nint instancePtr, PublishedFileUpdateHandle_t updateHandle, InteropHelp.UTF8StringHandle pchTitle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_UpdatePublishedFileDescription")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamRemoteStorage_UpdatePublishedFileDescription(nint instancePtr, PublishedFileUpdateHandle_t updateHandle, InteropHelp.UTF8StringHandle pchDescription);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_UpdatePublishedFileVisibility")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamRemoteStorage_UpdatePublishedFileVisibility(nint instancePtr, PublishedFileUpdateHandle_t updateHandle, ERemoteStoragePublishedFileVisibility eVisibility);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_UpdatePublishedFileTags")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamRemoteStorage_UpdatePublishedFileTags(nint instancePtr, PublishedFileUpdateHandle_t updateHandle, nint pTags);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_CommitPublishedFileUpdate")]
	public static extern ulong ISteamRemoteStorage_CommitPublishedFileUpdate(nint instancePtr, PublishedFileUpdateHandle_t updateHandle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetPublishedFileDetails")]
	public static extern ulong ISteamRemoteStorage_GetPublishedFileDetails(nint instancePtr, PublishedFileId_t unPublishedFileId, uint unMaxSecondsOld);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_DeletePublishedFile")]
	public static extern ulong ISteamRemoteStorage_DeletePublishedFile(nint instancePtr, PublishedFileId_t unPublishedFileId);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_EnumerateUserPublishedFiles")]
	public static extern ulong ISteamRemoteStorage_EnumerateUserPublishedFiles(nint instancePtr, uint unStartIndex);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_SubscribePublishedFile")]
	public static extern ulong ISteamRemoteStorage_SubscribePublishedFile(nint instancePtr, PublishedFileId_t unPublishedFileId);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_EnumerateUserSubscribedFiles")]
	public static extern ulong ISteamRemoteStorage_EnumerateUserSubscribedFiles(nint instancePtr, uint unStartIndex);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_UnsubscribePublishedFile")]
	public static extern ulong ISteamRemoteStorage_UnsubscribePublishedFile(nint instancePtr, PublishedFileId_t unPublishedFileId);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_UpdatePublishedFileSetChangeDescription")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamRemoteStorage_UpdatePublishedFileSetChangeDescription(nint instancePtr, PublishedFileUpdateHandle_t updateHandle, InteropHelp.UTF8StringHandle pchChangeDescription);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetPublishedItemVoteDetails")]
	public static extern ulong ISteamRemoteStorage_GetPublishedItemVoteDetails(nint instancePtr, PublishedFileId_t unPublishedFileId);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_UpdateUserPublishedItemVote")]
	public static extern ulong ISteamRemoteStorage_UpdateUserPublishedItemVote(nint instancePtr, PublishedFileId_t unPublishedFileId, [MarshalAs(UnmanagedType.I1)] bool bVoteUp);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetUserPublishedItemVoteDetails")]
	public static extern ulong ISteamRemoteStorage_GetUserPublishedItemVoteDetails(nint instancePtr, PublishedFileId_t unPublishedFileId);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_EnumerateUserSharedWorkshopFiles")]
	public static extern ulong ISteamRemoteStorage_EnumerateUserSharedWorkshopFiles(nint instancePtr, CSteamID steamId, uint unStartIndex, nint pRequiredTags, nint pExcludedTags);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_PublishVideo")]
	public static extern ulong ISteamRemoteStorage_PublishVideo(nint instancePtr, EWorkshopVideoProvider eVideoProvider, InteropHelp.UTF8StringHandle pchVideoAccount, InteropHelp.UTF8StringHandle pchVideoIdentifier, InteropHelp.UTF8StringHandle pchPreviewFile, AppId_t nConsumerAppId, InteropHelp.UTF8StringHandle pchTitle, InteropHelp.UTF8StringHandle pchDescription, ERemoteStoragePublishedFileVisibility eVisibility, nint pTags);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_SetUserPublishedFileAction")]
	public static extern ulong ISteamRemoteStorage_SetUserPublishedFileAction(nint instancePtr, PublishedFileId_t unPublishedFileId, EWorkshopFileAction eAction);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_EnumeratePublishedFilesByUserAction")]
	public static extern ulong ISteamRemoteStorage_EnumeratePublishedFilesByUserAction(nint instancePtr, EWorkshopFileAction eAction, uint unStartIndex);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_EnumeratePublishedWorkshopFiles")]
	public static extern ulong ISteamRemoteStorage_EnumeratePublishedWorkshopFiles(nint instancePtr, EWorkshopEnumerationType eEnumerationType, uint unStartIndex, uint unCount, uint unDays, nint pTags, nint pUserTags);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_UGCDownloadToLocation")]
	public static extern ulong ISteamRemoteStorage_UGCDownloadToLocation(nint instancePtr, UGCHandle_t hContent, InteropHelp.UTF8StringHandle pchLocation, uint unPriority);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetLocalFileChangeCount")]
	public static extern int ISteamRemoteStorage_GetLocalFileChangeCount(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetLocalFileChange")]
	public static extern nint ISteamRemoteStorage_GetLocalFileChange(nint instancePtr, int iFile, out ERemoteStorageLocalFileChange pEChangeType, out ERemoteStorageFilePathType pEFilePathType);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_BeginFileWriteBatch")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamRemoteStorage_BeginFileWriteBatch(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamRemoteStorage_EndFileWriteBatch")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamRemoteStorage_EndFileWriteBatch(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamScreenshots_WriteScreenshot")]
	public static extern uint ISteamScreenshots_WriteScreenshot(nint instancePtr, byte[] pubRGB, uint cubRGB, int nWidth, int nHeight);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamScreenshots_AddScreenshotToLibrary")]
	public static extern uint ISteamScreenshots_AddScreenshotToLibrary(nint instancePtr, InteropHelp.UTF8StringHandle pchFilename, InteropHelp.UTF8StringHandle pchThumbnailFilename, int nWidth, int nHeight);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamScreenshots_TriggerScreenshot")]
	public static extern void ISteamScreenshots_TriggerScreenshot(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamScreenshots_HookScreenshots")]
	public static extern void ISteamScreenshots_HookScreenshots(nint instancePtr, [MarshalAs(UnmanagedType.I1)] bool bHook);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamScreenshots_SetLocation")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamScreenshots_SetLocation(nint instancePtr, ScreenshotHandle hScreenshot, InteropHelp.UTF8StringHandle pchLocation);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamScreenshots_TagUser")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamScreenshots_TagUser(nint instancePtr, ScreenshotHandle hScreenshot, CSteamID steamID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamScreenshots_TagPublishedFile")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamScreenshots_TagPublishedFile(nint instancePtr, ScreenshotHandle hScreenshot, PublishedFileId_t unPublishedFileID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamScreenshots_IsScreenshotsHooked")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamScreenshots_IsScreenshotsHooked(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamScreenshots_AddVRScreenshotToLibrary")]
	public static extern uint ISteamScreenshots_AddVRScreenshotToLibrary(nint instancePtr, EVRScreenshotType eType, InteropHelp.UTF8StringHandle pchFilename, InteropHelp.UTF8StringHandle pchVRFilename);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_CreateQueryUserUGCRequest")]
	public static extern ulong ISteamUGC_CreateQueryUserUGCRequest(nint instancePtr, AccountID_t unAccountID, EUserUGCList eListType, EUGCMatchingUGCType eMatchingUGCType, EUserUGCListSortOrder eSortOrder, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint unPage);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_CreateQueryAllUGCRequestPage")]
	public static extern ulong ISteamUGC_CreateQueryAllUGCRequestPage(nint instancePtr, EUGCQuery eQueryType, EUGCMatchingUGCType eMatchingeMatchingUGCTypeFileType, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint unPage);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_CreateQueryAllUGCRequestCursor")]
	public static extern ulong ISteamUGC_CreateQueryAllUGCRequestCursor(nint instancePtr, EUGCQuery eQueryType, EUGCMatchingUGCType eMatchingeMatchingUGCTypeFileType, AppId_t nCreatorAppID, AppId_t nConsumerAppID, InteropHelp.UTF8StringHandle pchCursor);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_CreateQueryUGCDetailsRequest")]
	public static extern ulong ISteamUGC_CreateQueryUGCDetailsRequest(nint instancePtr, [In][Out] PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_SendQueryUGCRequest")]
	public static extern ulong ISteamUGC_SendQueryUGCRequest(nint instancePtr, UGCQueryHandle_t handle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_GetQueryUGCResult")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_GetQueryUGCResult(nint instancePtr, UGCQueryHandle_t handle, uint index, out SteamUGCDetails_t pDetails);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_GetQueryUGCNumTags")]
	public static extern uint ISteamUGC_GetQueryUGCNumTags(nint instancePtr, UGCQueryHandle_t handle, uint index);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_GetQueryUGCTag")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_GetQueryUGCTag(nint instancePtr, UGCQueryHandle_t handle, uint index, uint indexTag, nint pchValue, uint cchValueSize);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_GetQueryUGCTagDisplayName")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_GetQueryUGCTagDisplayName(nint instancePtr, UGCQueryHandle_t handle, uint index, uint indexTag, nint pchValue, uint cchValueSize);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_GetQueryUGCPreviewURL")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_GetQueryUGCPreviewURL(nint instancePtr, UGCQueryHandle_t handle, uint index, nint pchURL, uint cchURLSize);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_GetQueryUGCMetadata")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_GetQueryUGCMetadata(nint instancePtr, UGCQueryHandle_t handle, uint index, nint pchMetadata, uint cchMetadatasize);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_GetQueryUGCChildren")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_GetQueryUGCChildren(nint instancePtr, UGCQueryHandle_t handle, uint index, [In][Out] PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_GetQueryUGCStatistic")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_GetQueryUGCStatistic(nint instancePtr, UGCQueryHandle_t handle, uint index, EItemStatistic eStatType, out ulong pStatValue);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_GetQueryUGCNumAdditionalPreviews")]
	public static extern uint ISteamUGC_GetQueryUGCNumAdditionalPreviews(nint instancePtr, UGCQueryHandle_t handle, uint index);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_GetQueryUGCAdditionalPreview")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_GetQueryUGCAdditionalPreview(nint instancePtr, UGCQueryHandle_t handle, uint index, uint previewIndex, nint pchURLOrVideoID, uint cchURLSize, nint pchOriginalFileName, uint cchOriginalFileNameSize, out EItemPreviewType pPreviewType);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_GetQueryUGCNumKeyValueTags")]
	public static extern uint ISteamUGC_GetQueryUGCNumKeyValueTags(nint instancePtr, UGCQueryHandle_t handle, uint index);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_GetQueryUGCKeyValueTag")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_GetQueryUGCKeyValueTag(nint instancePtr, UGCQueryHandle_t handle, uint index, uint keyValueTagIndex, nint pchKey, uint cchKeySize, nint pchValue, uint cchValueSize);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_GetQueryFirstUGCKeyValueTag")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_GetQueryFirstUGCKeyValueTag(nint instancePtr, UGCQueryHandle_t handle, uint index, InteropHelp.UTF8StringHandle pchKey, nint pchValue, uint cchValueSize);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_ReleaseQueryUGCRequest")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_ReleaseQueryUGCRequest(nint instancePtr, UGCQueryHandle_t handle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_AddRequiredTag")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_AddRequiredTag(nint instancePtr, UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pTagName);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_AddRequiredTagGroup")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_AddRequiredTagGroup(nint instancePtr, UGCQueryHandle_t handle, nint pTagGroups);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_AddExcludedTag")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_AddExcludedTag(nint instancePtr, UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pTagName);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_SetReturnOnlyIDs")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_SetReturnOnlyIDs(nint instancePtr, UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnOnlyIDs);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_SetReturnKeyValueTags")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_SetReturnKeyValueTags(nint instancePtr, UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnKeyValueTags);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_SetReturnLongDescription")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_SetReturnLongDescription(nint instancePtr, UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnLongDescription);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_SetReturnMetadata")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_SetReturnMetadata(nint instancePtr, UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnMetadata);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_SetReturnChildren")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_SetReturnChildren(nint instancePtr, UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnChildren);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_SetReturnAdditionalPreviews")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_SetReturnAdditionalPreviews(nint instancePtr, UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnAdditionalPreviews);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_SetReturnTotalOnly")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_SetReturnTotalOnly(nint instancePtr, UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnTotalOnly);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_SetReturnPlaytimeStats")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_SetReturnPlaytimeStats(nint instancePtr, UGCQueryHandle_t handle, uint unDays);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_SetLanguage")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_SetLanguage(nint instancePtr, UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pchLanguage);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_SetAllowCachedResponse")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_SetAllowCachedResponse(nint instancePtr, UGCQueryHandle_t handle, uint unMaxAgeSeconds);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_SetCloudFileNameFilter")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_SetCloudFileNameFilter(nint instancePtr, UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pMatchCloudFileName);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_SetMatchAnyTag")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_SetMatchAnyTag(nint instancePtr, UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bMatchAnyTag);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_SetSearchText")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_SetSearchText(nint instancePtr, UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pSearchText);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_SetRankedByTrendDays")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_SetRankedByTrendDays(nint instancePtr, UGCQueryHandle_t handle, uint unDays);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_SetTimeCreatedDateRange")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_SetTimeCreatedDateRange(nint instancePtr, UGCQueryHandle_t handle, uint rtStart, uint rtEnd);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_SetTimeUpdatedDateRange")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_SetTimeUpdatedDateRange(nint instancePtr, UGCQueryHandle_t handle, uint rtStart, uint rtEnd);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_AddRequiredKeyValueTag")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_AddRequiredKeyValueTag(nint instancePtr, UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pKey, InteropHelp.UTF8StringHandle pValue);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_RequestUGCDetails")]
	public static extern ulong ISteamUGC_RequestUGCDetails(nint instancePtr, PublishedFileId_t nPublishedFileID, uint unMaxAgeSeconds);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_CreateItem")]
	public static extern ulong ISteamUGC_CreateItem(nint instancePtr, AppId_t nConsumerAppId, EWorkshopFileType eFileType);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_StartItemUpdate")]
	public static extern ulong ISteamUGC_StartItemUpdate(nint instancePtr, AppId_t nConsumerAppId, PublishedFileId_t nPublishedFileID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_SetItemTitle")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_SetItemTitle(nint instancePtr, UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchTitle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_SetItemDescription")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_SetItemDescription(nint instancePtr, UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchDescription);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_SetItemUpdateLanguage")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_SetItemUpdateLanguage(nint instancePtr, UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchLanguage);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_SetItemMetadata")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_SetItemMetadata(nint instancePtr, UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchMetaData);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_SetItemVisibility")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_SetItemVisibility(nint instancePtr, UGCUpdateHandle_t handle, ERemoteStoragePublishedFileVisibility eVisibility);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_SetItemTags")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_SetItemTags(nint instancePtr, UGCUpdateHandle_t updateHandle, nint pTags);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_SetItemContent")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_SetItemContent(nint instancePtr, UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pszContentFolder);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_SetItemPreview")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_SetItemPreview(nint instancePtr, UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pszPreviewFile);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_SetAllowLegacyUpload")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_SetAllowLegacyUpload(nint instancePtr, UGCUpdateHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bAllowLegacyUpload);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_RemoveAllItemKeyValueTags")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_RemoveAllItemKeyValueTags(nint instancePtr, UGCUpdateHandle_t handle);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_RemoveItemKeyValueTags")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_RemoveItemKeyValueTags(nint instancePtr, UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchKey);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_AddItemKeyValueTag")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_AddItemKeyValueTag(nint instancePtr, UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchKey, InteropHelp.UTF8StringHandle pchValue);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_AddItemPreviewFile")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_AddItemPreviewFile(nint instancePtr, UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pszPreviewFile, EItemPreviewType type);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_AddItemPreviewVideo")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_AddItemPreviewVideo(nint instancePtr, UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pszVideoID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_UpdateItemPreviewFile")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_UpdateItemPreviewFile(nint instancePtr, UGCUpdateHandle_t handle, uint index, InteropHelp.UTF8StringHandle pszPreviewFile);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_UpdateItemPreviewVideo")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_UpdateItemPreviewVideo(nint instancePtr, UGCUpdateHandle_t handle, uint index, InteropHelp.UTF8StringHandle pszVideoID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_RemoveItemPreview")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_RemoveItemPreview(nint instancePtr, UGCUpdateHandle_t handle, uint index);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_SubmitItemUpdate")]
	public static extern ulong ISteamUGC_SubmitItemUpdate(nint instancePtr, UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchChangeNote);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_GetItemUpdateProgress")]
	public static extern EItemUpdateStatus ISteamUGC_GetItemUpdateProgress(nint instancePtr, UGCUpdateHandle_t handle, out ulong punBytesProcessed, out ulong punBytesTotal);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_SetUserItemVote")]
	public static extern ulong ISteamUGC_SetUserItemVote(nint instancePtr, PublishedFileId_t nPublishedFileID, [MarshalAs(UnmanagedType.I1)] bool bVoteUp);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_GetUserItemVote")]
	public static extern ulong ISteamUGC_GetUserItemVote(nint instancePtr, PublishedFileId_t nPublishedFileID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_AddItemToFavorites")]
	public static extern ulong ISteamUGC_AddItemToFavorites(nint instancePtr, AppId_t nAppId, PublishedFileId_t nPublishedFileID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_RemoveItemFromFavorites")]
	public static extern ulong ISteamUGC_RemoveItemFromFavorites(nint instancePtr, AppId_t nAppId, PublishedFileId_t nPublishedFileID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_SubscribeItem")]
	public static extern ulong ISteamUGC_SubscribeItem(nint instancePtr, PublishedFileId_t nPublishedFileID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_UnsubscribeItem")]
	public static extern ulong ISteamUGC_UnsubscribeItem(nint instancePtr, PublishedFileId_t nPublishedFileID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_GetNumSubscribedItems")]
	public static extern uint ISteamUGC_GetNumSubscribedItems(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_GetSubscribedItems")]
	public static extern uint ISteamUGC_GetSubscribedItems(nint instancePtr, [In][Out] PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_GetItemState")]
	public static extern uint ISteamUGC_GetItemState(nint instancePtr, PublishedFileId_t nPublishedFileID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_GetItemInstallInfo")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_GetItemInstallInfo(nint instancePtr, PublishedFileId_t nPublishedFileID, out ulong punSizeOnDisk, nint pchFolder, uint cchFolderSize, out uint punTimeStamp);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_GetItemDownloadInfo")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_GetItemDownloadInfo(nint instancePtr, PublishedFileId_t nPublishedFileID, out ulong punBytesDownloaded, out ulong punBytesTotal);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_DownloadItem")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_DownloadItem(nint instancePtr, PublishedFileId_t nPublishedFileID, [MarshalAs(UnmanagedType.I1)] bool bHighPriority);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_BInitWorkshopForGameServer")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_BInitWorkshopForGameServer(nint instancePtr, DepotId_t unWorkshopDepotID, InteropHelp.UTF8StringHandle pszFolder);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_SuspendDownloads")]
	public static extern void ISteamUGC_SuspendDownloads(nint instancePtr, [MarshalAs(UnmanagedType.I1)] bool bSuspend);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_StartPlaytimeTracking")]
	public static extern ulong ISteamUGC_StartPlaytimeTracking(nint instancePtr, [In][Out] PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_StopPlaytimeTracking")]
	public static extern ulong ISteamUGC_StopPlaytimeTracking(nint instancePtr, [In][Out] PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_StopPlaytimeTrackingForAllItems")]
	public static extern ulong ISteamUGC_StopPlaytimeTrackingForAllItems(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_AddDependency")]
	public static extern ulong ISteamUGC_AddDependency(nint instancePtr, PublishedFileId_t nParentPublishedFileID, PublishedFileId_t nChildPublishedFileID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_RemoveDependency")]
	public static extern ulong ISteamUGC_RemoveDependency(nint instancePtr, PublishedFileId_t nParentPublishedFileID, PublishedFileId_t nChildPublishedFileID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_AddAppDependency")]
	public static extern ulong ISteamUGC_AddAppDependency(nint instancePtr, PublishedFileId_t nPublishedFileID, AppId_t nAppID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_RemoveAppDependency")]
	public static extern ulong ISteamUGC_RemoveAppDependency(nint instancePtr, PublishedFileId_t nPublishedFileID, AppId_t nAppID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_GetAppDependencies")]
	public static extern ulong ISteamUGC_GetAppDependencies(nint instancePtr, PublishedFileId_t nPublishedFileID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_DeleteItem")]
	public static extern ulong ISteamUGC_DeleteItem(nint instancePtr, PublishedFileId_t nPublishedFileID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_ShowWorkshopEULA")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUGC_ShowWorkshopEULA(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUGC_GetWorkshopEULAStatus")]
	public static extern ulong ISteamUGC_GetWorkshopEULAStatus(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUser_GetHSteamUser")]
	public static extern int ISteamUser_GetHSteamUser(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUser_BLoggedOn")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUser_BLoggedOn(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUser_GetSteamID")]
	public static extern ulong ISteamUser_GetSteamID(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUser_InitiateGameConnection_DEPRECATED")]
	public static extern int ISteamUser_InitiateGameConnection_DEPRECATED(nint instancePtr, byte[] pAuthBlob, int cbMaxAuthBlob, CSteamID steamIDGameServer, uint unIPServer, ushort usPortServer, [MarshalAs(UnmanagedType.I1)] bool bSecure);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUser_TerminateGameConnection_DEPRECATED")]
	public static extern void ISteamUser_TerminateGameConnection_DEPRECATED(nint instancePtr, uint unIPServer, ushort usPortServer);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUser_TrackAppUsageEvent")]
	public static extern void ISteamUser_TrackAppUsageEvent(nint instancePtr, CGameID gameID, int eAppUsageEvent, InteropHelp.UTF8StringHandle pchExtraInfo);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUser_GetUserDataFolder")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUser_GetUserDataFolder(nint instancePtr, nint pchBuffer, int cubBuffer);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUser_StartVoiceRecording")]
	public static extern void ISteamUser_StartVoiceRecording(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUser_StopVoiceRecording")]
	public static extern void ISteamUser_StopVoiceRecording(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUser_GetAvailableVoice")]
	public static extern EVoiceResult ISteamUser_GetAvailableVoice(nint instancePtr, out uint pcbCompressed, nint pcbUncompressed_Deprecated, uint nUncompressedVoiceDesiredSampleRate_Deprecated);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUser_GetVoice")]
	public static extern EVoiceResult ISteamUser_GetVoice(nint instancePtr, [MarshalAs(UnmanagedType.I1)] bool bWantCompressed, byte[] pDestBuffer, uint cbDestBufferSize, out uint nBytesWritten, [MarshalAs(UnmanagedType.I1)] bool bWantUncompressed_Deprecated, nint pUncompressedDestBuffer_Deprecated, uint cbUncompressedDestBufferSize_Deprecated, nint nUncompressBytesWritten_Deprecated, uint nUncompressedVoiceDesiredSampleRate_Deprecated);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUser_DecompressVoice")]
	public static extern EVoiceResult ISteamUser_DecompressVoice(nint instancePtr, byte[] pCompressed, uint cbCompressed, byte[] pDestBuffer, uint cbDestBufferSize, out uint nBytesWritten, uint nDesiredSampleRate);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUser_GetVoiceOptimalSampleRate")]
	public static extern uint ISteamUser_GetVoiceOptimalSampleRate(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUser_GetAuthSessionTicket")]
	public static extern uint ISteamUser_GetAuthSessionTicket(nint instancePtr, byte[] pTicket, int cbMaxTicket, out uint pcbTicket);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUser_BeginAuthSession")]
	public static extern EBeginAuthSessionResult ISteamUser_BeginAuthSession(nint instancePtr, byte[] pAuthTicket, int cbAuthTicket, CSteamID steamID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUser_EndAuthSession")]
	public static extern void ISteamUser_EndAuthSession(nint instancePtr, CSteamID steamID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUser_CancelAuthTicket")]
	public static extern void ISteamUser_CancelAuthTicket(nint instancePtr, HAuthTicket hAuthTicket);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUser_UserHasLicenseForApp")]
	public static extern EUserHasLicenseForAppResult ISteamUser_UserHasLicenseForApp(nint instancePtr, CSteamID steamID, AppId_t appID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUser_BIsBehindNAT")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUser_BIsBehindNAT(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUser_AdvertiseGame")]
	public static extern void ISteamUser_AdvertiseGame(nint instancePtr, CSteamID steamIDGameServer, uint unIPServer, ushort usPortServer);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUser_RequestEncryptedAppTicket")]
	public static extern ulong ISteamUser_RequestEncryptedAppTicket(nint instancePtr, byte[] pDataToInclude, int cbDataToInclude);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUser_GetEncryptedAppTicket")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUser_GetEncryptedAppTicket(nint instancePtr, byte[] pTicket, int cbMaxTicket, out uint pcbTicket);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUser_GetGameBadgeLevel")]
	public static extern int ISteamUser_GetGameBadgeLevel(nint instancePtr, int nSeries, [MarshalAs(UnmanagedType.I1)] bool bFoil);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUser_GetPlayerSteamLevel")]
	public static extern int ISteamUser_GetPlayerSteamLevel(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUser_RequestStoreAuthURL")]
	public static extern ulong ISteamUser_RequestStoreAuthURL(nint instancePtr, InteropHelp.UTF8StringHandle pchRedirectURL);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUser_BIsPhoneVerified")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUser_BIsPhoneVerified(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUser_BIsTwoFactorEnabled")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUser_BIsTwoFactorEnabled(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUser_BIsPhoneIdentifying")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUser_BIsPhoneIdentifying(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUser_BIsPhoneRequiringVerification")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUser_BIsPhoneRequiringVerification(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUser_GetMarketEligibility")]
	public static extern ulong ISteamUser_GetMarketEligibility(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUser_GetDurationControl")]
	public static extern ulong ISteamUser_GetDurationControl(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUser_BSetDurationControlOnlineState")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUser_BSetDurationControlOnlineState(nint instancePtr, EDurationControlOnlineState eNewState);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_RequestCurrentStats")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUserStats_RequestCurrentStats(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_GetStatInt32")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUserStats_GetStatInt32(nint instancePtr, InteropHelp.UTF8StringHandle pchName, out int pData);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_GetStatFloat")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUserStats_GetStatFloat(nint instancePtr, InteropHelp.UTF8StringHandle pchName, out float pData);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_SetStatInt32")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUserStats_SetStatInt32(nint instancePtr, InteropHelp.UTF8StringHandle pchName, int nData);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_SetStatFloat")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUserStats_SetStatFloat(nint instancePtr, InteropHelp.UTF8StringHandle pchName, float fData);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_UpdateAvgRateStat")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUserStats_UpdateAvgRateStat(nint instancePtr, InteropHelp.UTF8StringHandle pchName, float flCountThisSession, double dSessionLength);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_GetAchievement")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUserStats_GetAchievement(nint instancePtr, InteropHelp.UTF8StringHandle pchName, out bool pbAchieved);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_SetAchievement")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUserStats_SetAchievement(nint instancePtr, InteropHelp.UTF8StringHandle pchName);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_ClearAchievement")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUserStats_ClearAchievement(nint instancePtr, InteropHelp.UTF8StringHandle pchName);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_GetAchievementAndUnlockTime")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUserStats_GetAchievementAndUnlockTime(nint instancePtr, InteropHelp.UTF8StringHandle pchName, out bool pbAchieved, out uint punUnlockTime);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_StoreStats")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUserStats_StoreStats(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_GetAchievementIcon")]
	public static extern int ISteamUserStats_GetAchievementIcon(nint instancePtr, InteropHelp.UTF8StringHandle pchName);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_GetAchievementDisplayAttribute")]
	public static extern nint ISteamUserStats_GetAchievementDisplayAttribute(nint instancePtr, InteropHelp.UTF8StringHandle pchName, InteropHelp.UTF8StringHandle pchKey);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_IndicateAchievementProgress")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUserStats_IndicateAchievementProgress(nint instancePtr, InteropHelp.UTF8StringHandle pchName, uint nCurProgress, uint nMaxProgress);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_GetNumAchievements")]
	public static extern uint ISteamUserStats_GetNumAchievements(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_GetAchievementName")]
	public static extern nint ISteamUserStats_GetAchievementName(nint instancePtr, uint iAchievement);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_RequestUserStats")]
	public static extern ulong ISteamUserStats_RequestUserStats(nint instancePtr, CSteamID steamIDUser);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_GetUserStatInt32")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUserStats_GetUserStatInt32(nint instancePtr, CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, out int pData);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_GetUserStatFloat")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUserStats_GetUserStatFloat(nint instancePtr, CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, out float pData);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_GetUserAchievement")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUserStats_GetUserAchievement(nint instancePtr, CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, out bool pbAchieved);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_GetUserAchievementAndUnlockTime")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUserStats_GetUserAchievementAndUnlockTime(nint instancePtr, CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, out bool pbAchieved, out uint punUnlockTime);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_ResetAllStats")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUserStats_ResetAllStats(nint instancePtr, [MarshalAs(UnmanagedType.I1)] bool bAchievementsToo);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_FindOrCreateLeaderboard")]
	public static extern ulong ISteamUserStats_FindOrCreateLeaderboard(nint instancePtr, InteropHelp.UTF8StringHandle pchLeaderboardName, ELeaderboardSortMethod eLeaderboardSortMethod, ELeaderboardDisplayType eLeaderboardDisplayType);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_FindLeaderboard")]
	public static extern ulong ISteamUserStats_FindLeaderboard(nint instancePtr, InteropHelp.UTF8StringHandle pchLeaderboardName);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_GetLeaderboardName")]
	public static extern nint ISteamUserStats_GetLeaderboardName(nint instancePtr, SteamLeaderboard_t hSteamLeaderboard);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_GetLeaderboardEntryCount")]
	public static extern int ISteamUserStats_GetLeaderboardEntryCount(nint instancePtr, SteamLeaderboard_t hSteamLeaderboard);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_GetLeaderboardSortMethod")]
	public static extern ELeaderboardSortMethod ISteamUserStats_GetLeaderboardSortMethod(nint instancePtr, SteamLeaderboard_t hSteamLeaderboard);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_GetLeaderboardDisplayType")]
	public static extern ELeaderboardDisplayType ISteamUserStats_GetLeaderboardDisplayType(nint instancePtr, SteamLeaderboard_t hSteamLeaderboard);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_DownloadLeaderboardEntries")]
	public static extern ulong ISteamUserStats_DownloadLeaderboardEntries(nint instancePtr, SteamLeaderboard_t hSteamLeaderboard, ELeaderboardDataRequest eLeaderboardDataRequest, int nRangeStart, int nRangeEnd);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_DownloadLeaderboardEntriesForUsers")]
	public static extern ulong ISteamUserStats_DownloadLeaderboardEntriesForUsers(nint instancePtr, SteamLeaderboard_t hSteamLeaderboard, [In][Out] CSteamID[] prgUsers, int cUsers);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_GetDownloadedLeaderboardEntry")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUserStats_GetDownloadedLeaderboardEntry(nint instancePtr, SteamLeaderboardEntries_t hSteamLeaderboardEntries, int index, out LeaderboardEntry_t pLeaderboardEntry, [In][Out] int[] pDetails, int cDetailsMax);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_UploadLeaderboardScore")]
	public static extern ulong ISteamUserStats_UploadLeaderboardScore(nint instancePtr, SteamLeaderboard_t hSteamLeaderboard, ELeaderboardUploadScoreMethod eLeaderboardUploadScoreMethod, int nScore, [In][Out] int[] pScoreDetails, int cScoreDetailsCount);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_AttachLeaderboardUGC")]
	public static extern ulong ISteamUserStats_AttachLeaderboardUGC(nint instancePtr, SteamLeaderboard_t hSteamLeaderboard, UGCHandle_t hUGC);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_GetNumberOfCurrentPlayers")]
	public static extern ulong ISteamUserStats_GetNumberOfCurrentPlayers(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_RequestGlobalAchievementPercentages")]
	public static extern ulong ISteamUserStats_RequestGlobalAchievementPercentages(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_GetMostAchievedAchievementInfo")]
	public static extern int ISteamUserStats_GetMostAchievedAchievementInfo(nint instancePtr, nint pchName, uint unNameBufLen, out float pflPercent, out bool pbAchieved);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_GetNextMostAchievedAchievementInfo")]
	public static extern int ISteamUserStats_GetNextMostAchievedAchievementInfo(nint instancePtr, int iIteratorPrevious, nint pchName, uint unNameBufLen, out float pflPercent, out bool pbAchieved);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_GetAchievementAchievedPercent")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUserStats_GetAchievementAchievedPercent(nint instancePtr, InteropHelp.UTF8StringHandle pchName, out float pflPercent);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_RequestGlobalStats")]
	public static extern ulong ISteamUserStats_RequestGlobalStats(nint instancePtr, int nHistoryDays);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_GetGlobalStatInt64")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUserStats_GetGlobalStatInt64(nint instancePtr, InteropHelp.UTF8StringHandle pchStatName, out long pData);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_GetGlobalStatDouble")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUserStats_GetGlobalStatDouble(nint instancePtr, InteropHelp.UTF8StringHandle pchStatName, out double pData);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_GetGlobalStatHistoryInt64")]
	public static extern int ISteamUserStats_GetGlobalStatHistoryInt64(nint instancePtr, InteropHelp.UTF8StringHandle pchStatName, [In][Out] long[] pData, uint cubData);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_GetGlobalStatHistoryDouble")]
	public static extern int ISteamUserStats_GetGlobalStatHistoryDouble(nint instancePtr, InteropHelp.UTF8StringHandle pchStatName, [In][Out] double[] pData, uint cubData);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_GetAchievementProgressLimitsInt32")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUserStats_GetAchievementProgressLimitsInt32(nint instancePtr, InteropHelp.UTF8StringHandle pchName, out int pnMinProgress, out int pnMaxProgress);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUserStats_GetAchievementProgressLimitsFloat")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUserStats_GetAchievementProgressLimitsFloat(nint instancePtr, InteropHelp.UTF8StringHandle pchName, out float pfMinProgress, out float pfMaxProgress);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUtils_GetSecondsSinceAppActive")]
	public static extern uint ISteamUtils_GetSecondsSinceAppActive(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUtils_GetSecondsSinceComputerActive")]
	public static extern uint ISteamUtils_GetSecondsSinceComputerActive(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUtils_GetConnectedUniverse")]
	public static extern EUniverse ISteamUtils_GetConnectedUniverse(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUtils_GetServerRealTime")]
	public static extern uint ISteamUtils_GetServerRealTime(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUtils_GetIPCountry")]
	public static extern nint ISteamUtils_GetIPCountry(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUtils_GetImageSize")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUtils_GetImageSize(nint instancePtr, int iImage, out uint pnWidth, out uint pnHeight);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUtils_GetImageRGBA")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUtils_GetImageRGBA(nint instancePtr, int iImage, byte[] pubDest, int nDestBufferSize);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUtils_GetCurrentBatteryPower")]
	public static extern byte ISteamUtils_GetCurrentBatteryPower(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUtils_GetAppID")]
	public static extern uint ISteamUtils_GetAppID(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUtils_SetOverlayNotificationPosition")]
	public static extern void ISteamUtils_SetOverlayNotificationPosition(nint instancePtr, ENotificationPosition eNotificationPosition);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUtils_IsAPICallCompleted")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUtils_IsAPICallCompleted(nint instancePtr, SteamAPICall_t hSteamAPICall, out bool pbFailed);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUtils_GetAPICallFailureReason")]
	public static extern ESteamAPICallFailure ISteamUtils_GetAPICallFailureReason(nint instancePtr, SteamAPICall_t hSteamAPICall);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUtils_GetAPICallResult")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUtils_GetAPICallResult(nint instancePtr, SteamAPICall_t hSteamAPICall, nint pCallback, int cubCallback, int iCallbackExpected, out bool pbFailed);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUtils_GetIPCCallCount")]
	public static extern uint ISteamUtils_GetIPCCallCount(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUtils_SetWarningMessageHook")]
	public static extern void ISteamUtils_SetWarningMessageHook(nint instancePtr, SteamAPIWarningMessageHook_t pFunction);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUtils_IsOverlayEnabled")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUtils_IsOverlayEnabled(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUtils_BOverlayNeedsPresent")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUtils_BOverlayNeedsPresent(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUtils_CheckFileSignature")]
	public static extern ulong ISteamUtils_CheckFileSignature(nint instancePtr, InteropHelp.UTF8StringHandle szFileName);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUtils_ShowGamepadTextInput")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUtils_ShowGamepadTextInput(nint instancePtr, EGamepadTextInputMode eInputMode, EGamepadTextInputLineMode eLineInputMode, InteropHelp.UTF8StringHandle pchDescription, uint unCharMax, InteropHelp.UTF8StringHandle pchExistingText);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUtils_GetEnteredGamepadTextLength")]
	public static extern uint ISteamUtils_GetEnteredGamepadTextLength(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUtils_GetEnteredGamepadTextInput")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUtils_GetEnteredGamepadTextInput(nint instancePtr, nint pchText, uint cchText);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUtils_GetSteamUILanguage")]
	public static extern nint ISteamUtils_GetSteamUILanguage(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUtils_IsSteamRunningInVR")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUtils_IsSteamRunningInVR(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUtils_SetOverlayNotificationInset")]
	public static extern void ISteamUtils_SetOverlayNotificationInset(nint instancePtr, int nHorizontalInset, int nVerticalInset);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUtils_IsSteamInBigPictureMode")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUtils_IsSteamInBigPictureMode(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUtils_StartVRDashboard")]
	public static extern void ISteamUtils_StartVRDashboard(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUtils_IsVRHeadsetStreamingEnabled")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUtils_IsVRHeadsetStreamingEnabled(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUtils_SetVRHeadsetStreamingEnabled")]
	public static extern void ISteamUtils_SetVRHeadsetStreamingEnabled(nint instancePtr, [MarshalAs(UnmanagedType.I1)] bool bEnabled);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUtils_IsSteamChinaLauncher")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUtils_IsSteamChinaLauncher(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUtils_InitFilterText")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUtils_InitFilterText(nint instancePtr, uint unFilterOptions);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUtils_FilterText")]
	public static extern int ISteamUtils_FilterText(nint instancePtr, ETextFilteringContext eContext, CSteamID sourceSteamID, InteropHelp.UTF8StringHandle pchInputMessage, nint pchOutFilteredText, uint nByteSizeOutFilteredText);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUtils_GetIPv6ConnectivityState")]
	public static extern ESteamIPv6ConnectivityState ISteamUtils_GetIPv6ConnectivityState(nint instancePtr, ESteamIPv6ConnectivityProtocol eProtocol);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUtils_IsSteamRunningOnSteamDeck")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUtils_IsSteamRunningOnSteamDeck(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUtils_ShowFloatingGamepadTextInput")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUtils_ShowFloatingGamepadTextInput(nint instancePtr, EFloatingGamepadTextInputMode eKeyboardMode, int nTextFieldXPosition, int nTextFieldYPosition, int nTextFieldWidth, int nTextFieldHeight);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUtils_SetGameLauncherMode")]
	public static extern void ISteamUtils_SetGameLauncherMode(nint instancePtr, [MarshalAs(UnmanagedType.I1)] bool bLauncherMode);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamUtils_DismissFloatingGamepadTextInput")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamUtils_DismissFloatingGamepadTextInput(nint instancePtr);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamVideo_GetVideoURL")]
	public static extern void ISteamVideo_GetVideoURL(nint instancePtr, AppId_t unVideoAppID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamVideo_IsBroadcasting")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamVideo_IsBroadcasting(nint instancePtr, out int pnNumViewers);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamVideo_GetOPFSettings")]
	public static extern void ISteamVideo_GetOPFSettings(nint instancePtr, AppId_t unVideoAppID);

	[DllImport("steam_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamAPI_ISteamVideo_GetOPFStringForApp")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ISteamVideo_GetOPFStringForApp(nint instancePtr, AppId_t unVideoAppID, nint pchBuffer, ref int pnBufferSize);
}
