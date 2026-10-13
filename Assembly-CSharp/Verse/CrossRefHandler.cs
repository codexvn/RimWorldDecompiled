using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000850 RID: 2128
	public class CrossRefHandler
	{
		// Token: 0x060035CC RID: 13772 RVA: 0x0011E324 File Offset: 0x0011C524
		public void RegisterForCrossRefResolve(IExposable s)
		{
			if (Scribe.mode != LoadSaveMode.LoadingVars)
			{
				Log.Error("Registered " + ((s != null) ? s.ToString() : null) + " for cross ref resolve, but current mode is " + Scribe.mode.ToString());
				return;
			}
			if (s == null)
			{
				return;
			}
			if (DebugViewSettings.logMapLoad)
			{
				LogSimple.Message("RegisterForCrossRefResolve " + ((s != null) ? s.GetType().ToString() : "null"));
			}
			this.crossReferencingExposables.Add(s);
		}

		// Token: 0x060035CD RID: 13773 RVA: 0x0011E3A8 File Offset: 0x0011C5A8
		public void ResolveAllCrossReferences()
		{
			Scribe.mode = LoadSaveMode.ResolvingCrossRefs;
			if (DebugViewSettings.logMapLoad)
			{
				LogSimple.Message("==================Register the saveables all so we can find them later");
			}
			foreach (IExposable exposable in this.crossReferencingExposables)
			{
				ILoadReferenceable loadReferenceable = exposable as ILoadReferenceable;
				if (loadReferenceable != null)
				{
					if (DebugViewSettings.logMapLoad)
					{
						string str = "RegisterLoaded ";
						Type type = loadReferenceable.GetType();
						LogSimple.Message(str + ((type != null) ? type.ToString() : null));
					}
					this.loadedObjectDirectory.RegisterLoaded(loadReferenceable);
				}
			}
			if (DebugViewSettings.logMapLoad)
			{
				LogSimple.Message("==================Fill all cross-references to the saveables");
			}
			foreach (IExposable exposable2 in this.crossReferencingExposables)
			{
				if (DebugViewSettings.logMapLoad)
				{
					string str2 = "ResolvingCrossRefs ExposeData ";
					Type type2 = exposable2.GetType();
					LogSimple.Message(str2 + ((type2 != null) ? type2.ToString() : null));
				}
				try
				{
					Scribe.loader.curParent = exposable2;
					Scribe.loader.curPathRelToParent = null;
					exposable2.ExposeData();
				}
				catch (Exception ex)
				{
					string str3 = "Could not resolve cross refs: ";
					Exception ex2 = ex;
					Log.Error(str3 + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
			Scribe.loader.curParent = null;
			Scribe.loader.curPathRelToParent = null;
			Scribe.mode = LoadSaveMode.Inactive;
			this.Clear(true);
		}

		// Token: 0x060035CE RID: 13774 RVA: 0x0011E528 File Offset: 0x0011C728
		public T TakeResolvedRef<T>(string pathRelToParent, IExposable parent) where T : ILoadReferenceable
		{
			string loadID = this.loadIDs.Take<T>(pathRelToParent, parent);
			return this.loadedObjectDirectory.ObjectWithLoadID<T>(loadID);
		}

		// Token: 0x060035CF RID: 13775 RVA: 0x0011E550 File Offset: 0x0011C750
		public T TakeResolvedRef<T>(string toAppendToPathRelToParent) where T : ILoadReferenceable
		{
			string text = Scribe.loader.curPathRelToParent;
			if (!toAppendToPathRelToParent.NullOrEmpty())
			{
				text = text + "/" + toAppendToPathRelToParent;
			}
			return this.TakeResolvedRef<T>(text, Scribe.loader.curParent);
		}

		// Token: 0x060035D0 RID: 13776 RVA: 0x0011E590 File Offset: 0x0011C790
		public List<T> TakeResolvedRefList<T>(string pathRelToParent, IExposable parent)
		{
			List<string> list = this.loadIDs.TakeList(pathRelToParent, parent);
			List<T> list2 = new List<T>();
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					list2.Add(this.loadedObjectDirectory.ObjectWithLoadID<T>(list[i]));
				}
			}
			return list2;
		}

		// Token: 0x060035D1 RID: 13777 RVA: 0x0011E5E0 File Offset: 0x0011C7E0
		public List<T> TakeResolvedRefList<T>(string toAppendToPathRelToParent)
		{
			string text = Scribe.loader.curPathRelToParent;
			if (!toAppendToPathRelToParent.NullOrEmpty())
			{
				text = text + "/" + toAppendToPathRelToParent;
			}
			return this.TakeResolvedRefList<T>(text, Scribe.loader.curParent);
		}

		// Token: 0x060035D2 RID: 13778 RVA: 0x0011E61E File Offset: 0x0011C81E
		public void Clear(bool errorIfNotEmpty)
		{
			if (errorIfNotEmpty)
			{
				this.loadIDs.ConfirmClear();
			}
			else
			{
				this.loadIDs.Clear();
			}
			this.crossReferencingExposables.Clear();
			this.loadedObjectDirectory.Clear();
		}

		// Token: 0x0400288F RID: 10383
		private LoadedObjectDirectory loadedObjectDirectory = new LoadedObjectDirectory();

		// Token: 0x04002890 RID: 10384
		public LoadIDsWantedBank loadIDs = new LoadIDsWantedBank();

		// Token: 0x04002891 RID: 10385
		public List<IExposable> crossReferencingExposables = new List<IExposable>();
	}
}
