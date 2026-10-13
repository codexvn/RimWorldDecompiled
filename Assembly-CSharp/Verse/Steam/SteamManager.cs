using System;
using System.Text;
using Steamworks;
using UnityEngine;

namespace Verse.Steam
{
	// Token: 0x02000D8C RID: 3468
	public static class SteamManager
	{
		// Token: 0x17000E06 RID: 3590
		// (get) Token: 0x06005880 RID: 22656 RVA: 0x001C8BFE File Offset: 0x001C6DFE
		public static bool Initialized
		{
			get
			{
				return SteamManager.initializedInt;
			}
		}

		// Token: 0x17000E07 RID: 3591
		// (get) Token: 0x06005881 RID: 22657 RVA: 0x000028E7 File Offset: 0x00000AE7
		public static bool Active
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005882 RID: 22658 RVA: 0x001C8C08 File Offset: 0x001C6E08
		public static void InitIfNeeded()
		{
			if (SteamManager.initializedInt)
			{
				return;
			}
			if (!Packsize.Test())
			{
				Log.Error("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.");
			}
			if (!DllCheck.Test())
			{
				Log.Error("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.");
			}
			try
			{
				if (SteamAPI.RestartAppIfNecessary(AppId_t.Invalid))
				{
					Application.Quit();
					return;
				}
			}
			catch (DllNotFoundException ex)
			{
				string str = "[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n";
				DllNotFoundException ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				Application.Quit();
				return;
			}
			SteamManager.initializedInt = SteamAPI.Init();
			if (!SteamManager.initializedInt)
			{
				Log.Warning("[Steamworks.NET] SteamAPI.Init() failed. Possible causes: Steam client not running, launched from outside Steam without steam_appid.txt in place, running with different privileges than Steam client (e.g. \"as administrator\")");
				return;
			}
			if (SteamManager.steamAPIWarningMessageHook == null)
			{
				SteamManager.steamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamManager.SteamAPIDebugTextHook);
				SteamClient.SetWarningMessageHook(SteamManager.steamAPIWarningMessageHook);
			}
			Workshop.Init();
			SteamDeck.Init();
		}

		// Token: 0x06005883 RID: 22659 RVA: 0x001C8CD4 File Offset: 0x001C6ED4
		public static void Update()
		{
			if (!SteamManager.initializedInt)
			{
				return;
			}
			SteamAPI.RunCallbacks();
			SteamDeck.Update();
		}

		// Token: 0x06005884 RID: 22660 RVA: 0x001C8CE8 File Offset: 0x001C6EE8
		public static void ShutdownSteam()
		{
			if (!SteamManager.initializedInt)
			{
				return;
			}
			SteamDeck.Shutdown();
			SteamAPI.Shutdown();
			SteamManager.initializedInt = false;
		}

		// Token: 0x06005885 RID: 22661 RVA: 0x001C8D02 File Offset: 0x001C6F02
		private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
		{
			Log.Error(pchDebugText.ToString());
		}

		// Token: 0x04004042 RID: 16450
		private static SteamAPIWarningMessageHook_t steamAPIWarningMessageHook;

		// Token: 0x04004043 RID: 16451
		private static bool initializedInt;
	}
}
