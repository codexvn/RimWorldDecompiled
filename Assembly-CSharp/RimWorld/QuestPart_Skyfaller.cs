using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CF8 RID: 7416
	public class QuestPart_Skyfaller : QuestPart
	{
		// Token: 0x1700198F RID: 6543
		// (get) Token: 0x0600A5A3 RID: 42403 RVA: 0x002F6E77 File Offset: 0x002F5077
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.mapParent != null)
				{
					yield return this.mapParent;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x0600A5A4 RID: 42404 RVA: 0x002F6E88 File Offset: 0x002F5088
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (this.mapParent == null || !this.mapParent.HasMap)
			{
				Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
				this.mapParent = ((anyPlayerHomeMap != null) ? anyPlayerHomeMap.Parent : null);
			}
			if (signal.tag == this.inSignal && this.mapParent != null && this.mapParent.HasMap)
			{
				this.items.RemoveAll((Thing x) => x.Destroyed);
				Map map = this.mapParent.Map;
				SkyfallerMaker.SpawnSkyfaller(this.skyfallerDef, this.items, this.dropSpot, map);
			}
		}

		// Token: 0x0400726F RID: 29295
		public ThingDef skyfallerDef;

		// Token: 0x04007270 RID: 29296
		public string inSignal;

		// Token: 0x04007271 RID: 29297
		public IntVec3 dropSpot = IntVec3.Invalid;

		// Token: 0x04007272 RID: 29298
		public MapParent mapParent;

		// Token: 0x04007273 RID: 29299
		private List<Thing> items = new List<Thing>();
	}
}
