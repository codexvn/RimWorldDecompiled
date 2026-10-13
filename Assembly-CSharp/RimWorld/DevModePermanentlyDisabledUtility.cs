using System;
using System.IO;
using Verse;

namespace RimWorld
{
	// Token: 0x02003628 RID: 13864
	public static class DevModePermanentlyDisabledUtility
	{
		// Token: 0x170031C5 RID: 12741
		// (get) Token: 0x06013413 RID: 78867 RVA: 0x005B63CE File Offset: 0x005B45CE
		public static bool Disabled
		{
			get
			{
				if (!DevModePermanentlyDisabledUtility.initialized)
				{
					DevModePermanentlyDisabledUtility.initialized = true;
					DevModePermanentlyDisabledUtility.disabled = File.Exists(GenFilePaths.DevModePermanentlyDisabledFilePath);
				}
				return DevModePermanentlyDisabledUtility.disabled;
			}
		}

		// Token: 0x06013414 RID: 78868 RVA: 0x005B63F4 File Offset: 0x005B45F4
		public static void Disable()
		{
			try
			{
				File.Create(GenFilePaths.DevModePermanentlyDisabledFilePath).Dispose();
			}
			catch (Exception ex)
			{
				string str = "Could not permanently disable dev mode: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				return;
			}
			DevModePermanentlyDisabledUtility.disabled = true;
			Prefs.DevMode = false;
		}

		// Token: 0x0400D5A8 RID: 54696
		private static bool initialized;

		// Token: 0x0400D5A9 RID: 54697
		private static bool disabled;
	}
}
