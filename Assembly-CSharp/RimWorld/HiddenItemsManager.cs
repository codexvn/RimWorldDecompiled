using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B51 RID: 6993
	public sealed class HiddenItemsManager : IExposable
	{
		// Token: 0x06009C2A RID: 39978 RVA: 0x002D5472 File Offset: 0x002D3672
		public void SetDiscovered(ThingDef def)
		{
			if (!this.hiddenItemDefs.ContainsKey(def))
			{
				return;
			}
			this.hiddenItemDefs[def] = false;
		}

		// Token: 0x06009C2B RID: 39979 RVA: 0x002D5490 File Offset: 0x002D3690
		public bool Hidden(ThingDef def)
		{
			bool flag;
			return this.hiddenItemDefs.TryGetValue(def, out flag) && flag;
		}

		// Token: 0x06009C2C RID: 39980 RVA: 0x002D54B0 File Offset: 0x002D36B0
		public HiddenItemsManager()
		{
			this.hiddenItemDefs = new Dictionary<ThingDef, bool>();
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.hiddenWhileUndiscovered)
				{
					this.hiddenItemDefs.Add(thingDef, true);
				}
			}
		}

		// Token: 0x06009C2D RID: 39981 RVA: 0x002D551C File Offset: 0x002D371C
		public void ClearHiddenDefs()
		{
			this.hiddenItemDefs.Clear();
		}

		// Token: 0x06009C2E RID: 39982 RVA: 0x002D552C File Offset: 0x002D372C
		public void ExposeData()
		{
			Scribe_Collections.Look<ThingDef, bool>(ref this.hiddenItemDefs, "hiddenItemDefs", LookMode.Def, LookMode.Value);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Dictionary<ThingDef, bool> dictionary = new Dictionary<ThingDef, bool>();
				foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
				{
					if (thingDef.hiddenWhileUndiscovered)
					{
						dictionary.Add(thingDef, true);
					}
				}
				foreach (KeyValuePair<ThingDef, bool> keyValuePair in this.hiddenItemDefs)
				{
					if (dictionary.ContainsKey(keyValuePair.Key))
					{
						dictionary[keyValuePair.Key] = keyValuePair.Value;
					}
				}
				this.hiddenItemDefs = dictionary;
			}
		}

		// Token: 0x04006C55 RID: 27733
		private Dictionary<ThingDef, bool> hiddenItemDefs;
	}
}
