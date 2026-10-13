using System;

namespace Steamworks;

[Serializable]
public struct ISteamNetworkingSignalingRecvContext
{
	public nint OnConnectRequest(HSteamNetConnection hConn, ref SteamNetworkingIdentity identityPeer, int nLocalVirtualPort)
	{
		return NativeMethods.SteamAPI_ISteamNetworkingSignalingRecvContext_OnConnectRequest(ref this, hConn, ref identityPeer, nLocalVirtualPort);
	}

	public void SendRejectionSignal(ref SteamNetworkingIdentity identityPeer, nint pMsg, int cbMsg)
	{
		NativeMethods.SteamAPI_ISteamNetworkingSignalingRecvContext_SendRejectionSignal(ref this, ref identityPeer, pMsg, cbMsg);
	}
}
