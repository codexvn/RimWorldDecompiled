using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000855 RID: 2133
	public class PostLoadIniter
	{
		// Token: 0x060035E3 RID: 13795 RVA: 0x0011F098 File Offset: 0x0011D298
		public void RegisterForPostLoadInit(IExposable s)
		{
			if (Scribe.mode != LoadSaveMode.LoadingVars)
			{
				Log.Error("Registered " + ((s != null) ? s.ToString() : null) + " for post load init, but current mode is " + Scribe.mode.ToString());
				return;
			}
			if (s == null)
			{
				Log.Warning("Trying to register null in RegisterforPostLoadInit.");
				return;
			}
			try
			{
				if (!this.saveablesToPostLoad.Add(s))
				{
					Log.Warning("Tried to register in RegisterforPostLoadInit when already registered: " + ((s != null) ? s.ToString() : null));
				}
			}
			catch (Exception ex)
			{
				string str = "Could not register an object for post load init: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x060035E4 RID: 13796 RVA: 0x0011F14C File Offset: 0x0011D34C
		public void DoAllPostLoadInits()
		{
			Scribe.mode = LoadSaveMode.PostLoadInit;
			foreach (IExposable exposable in this.saveablesToPostLoad)
			{
				try
				{
					Scribe.loader.curParent = exposable;
					Scribe.loader.curPathRelToParent = null;
					exposable.ExposeData();
				}
				catch (Exception ex)
				{
					string str = "Could not do PostLoadInit on ";
					string str2 = exposable.ToStringSafe<IExposable>();
					string str3 = ": ";
					Exception ex2 = ex;
					Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
			this.Clear();
			Scribe.loader.curParent = null;
			Scribe.loader.curPathRelToParent = null;
			Scribe.mode = LoadSaveMode.Inactive;
		}

		// Token: 0x060035E5 RID: 13797 RVA: 0x0011F214 File Offset: 0x0011D414
		public void Clear()
		{
			this.saveablesToPostLoad.Clear();
		}

		// Token: 0x0400289D RID: 10397
		private HashSet<IExposable> saveablesToPostLoad = new HashSet<IExposable>();
	}
}
