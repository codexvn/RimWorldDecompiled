using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C2A RID: 7210
	public class QuestPart_DestroyWorldObject : QuestPart
	{
		// Token: 0x170018AF RID: 6319
		// (get) Token: 0x0600A091 RID: 41105 RVA: 0x002E3F5B File Offset: 0x002E215B
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.worldObject != null)
				{
					yield return this.worldObject;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x0600A092 RID: 41106 RVA: 0x002E3F6B File Offset: 0x002E216B
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				QuestPart_DestroyWorldObject.TryRemove(this.worldObject);
			}
		}

		// Token: 0x0600A093 RID: 41107 RVA: 0x002E3F92 File Offset: 0x002E2192
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<WorldObject>(ref this.worldObject, "worldObject", false);
		}

		// Token: 0x0600A094 RID: 41108 RVA: 0x002E3FC0 File Offset: 0x002E21C0
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			PlanetTile tile;
			if (TileFinder.TryFindNewSiteTile(out tile, 7, 27, false, null, 0.5f, true, TileFinderMode.Near, false, false, null, null))
			{
				this.worldObject = SiteMaker.MakeSite(null, tile, null, true, null, null);
			}
		}

		// Token: 0x0600A095 RID: 41109 RVA: 0x002E4024 File Offset: 0x002E2224
		public static void TryRemove(WorldObject worldObject)
		{
			if (worldObject != null && worldObject.Spawned)
			{
				MapParent mapParent = worldObject as MapParent;
				if (mapParent != null && mapParent.HasMap)
				{
					mapParent.forceRemoveWorldObjectWhenMapRemoved = true;
					return;
				}
				worldObject.Destroy();
			}
		}

		// Token: 0x04006F02 RID: 28418
		public string inSignal;

		// Token: 0x04006F03 RID: 28419
		public WorldObject worldObject;
	}
}
