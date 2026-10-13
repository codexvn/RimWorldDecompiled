using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BE2 RID: 7138
	public class QuestPart_WorldObjectTimeout : QuestPart_Delay
	{
		// Token: 0x17001874 RID: 6260
		// (get) Token: 0x06009F05 RID: 40709 RVA: 0x002DF173 File Offset: 0x002DD373
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

		// Token: 0x06009F06 RID: 40710 RVA: 0x002DF184 File Offset: 0x002DD384
		public override string ExtraInspectString(ISelectable target)
		{
			if (target == this.worldObject)
			{
				Site site = target as Site;
				if (site != null)
				{
					for (int i = 0; i < site.parts.Count; i++)
					{
						if (site.parts[i].def.handlesWorldObjectTimeoutInspectString)
						{
							return null;
						}
					}
				}
				return "WorldObjectTimeout".Translate(base.TicksLeft.ToStringTicksToPeriod(true, false, true, true, false));
			}
			return null;
		}

		// Token: 0x06009F07 RID: 40711 RVA: 0x002DF1FA File Offset: 0x002DD3FA
		protected override void DelayFinished()
		{
			QuestPart_DestroyWorldObject.TryRemove(this.worldObject);
			if (this.worldObject != null)
			{
				base.Complete(this.worldObject.Named("SUBJECT"));
				return;
			}
			base.Complete();
		}

		// Token: 0x06009F08 RID: 40712 RVA: 0x002DF22C File Offset: 0x002DD42C
		public override void Cleanup()
		{
			base.Cleanup();
			if (this.destroyOnCleanup)
			{
				QuestPart_DestroyWorldObject.TryRemove(this.worldObject);
			}
		}

		// Token: 0x06009F09 RID: 40713 RVA: 0x002DF247 File Offset: 0x002DD447
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<WorldObject>(ref this.worldObject, "worldObject", false);
			Scribe_Values.Look<bool>(ref this.destroyOnCleanup, "destroyOnCleanup", false, false);
		}

		// Token: 0x06009F0A RID: 40714 RVA: 0x002DF274 File Offset: 0x002DD474
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			PlanetTile tile;
			if (TileFinder.TryFindNewSiteTile(out tile, 7, 27, false, null, 0.5f, true, TileFinderMode.Near, false, false, null, null))
			{
				this.worldObject = SiteMaker.MakeSite(null, tile, null, true, null, null);
			}
		}

		// Token: 0x04006DFD RID: 28157
		public WorldObject worldObject;

		// Token: 0x04006DFE RID: 28158
		public bool destroyOnCleanup;
	}
}
