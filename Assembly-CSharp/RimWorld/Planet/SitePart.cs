using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003DE7 RID: 15847
	public class SitePart : IExposable, IThingHolderTickable, IThingHolder
	{
		// Token: 0x170035E5 RID: 13797
		// (get) Token: 0x06015A04 RID: 88580 RVA: 0x00679163 File Offset: 0x00677363
		public IThingHolder ParentHolder
		{
			get
			{
				return this.site;
			}
		}

		// Token: 0x06015A05 RID: 88581 RVA: 0x0067916B File Offset: 0x0067736B
		public SitePart()
		{
		}

		// Token: 0x06015A06 RID: 88582 RVA: 0x00679181 File Offset: 0x00677381
		public SitePart(Site site, SitePartDef def, SitePartParams parms)
		{
			this.site = site;
			this.def = def;
			this.parms = parms;
			this.hidden = def.defaultHidden;
		}

		// Token: 0x170035E6 RID: 13798
		// (get) Token: 0x06015A07 RID: 88583 RVA: 0x006791B8 File Offset: 0x006773B8
		public bool ShouldTickContents
		{
			get
			{
				return this.things != null && this.things.contentsLookMode == LookMode.Deep && !this.things.dontTickContents;
			}
		}

		// Token: 0x06015A08 RID: 88584 RVA: 0x006791E0 File Offset: 0x006773E0
		public void SitePartTickInterval(int delta)
		{
			if (this.things != null)
			{
				for (int i = 0; i < this.things.Count; i++)
				{
					Pawn pawn = this.things[i] as Pawn;
					if (pawn != null && !pawn.Destroyed && pawn.needs.food != null)
					{
						pawn.needs.food.CurLevelPercentage = 0.8f;
					}
				}
			}
		}

		// Token: 0x06015A09 RID: 88585 RVA: 0x0067924A File Offset: 0x0067744A
		public void PostDestroy()
		{
			if (this.things != null)
			{
				this.things.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
			}
			if (!this.relicWasSpawned)
			{
				Thing thing = this.relicThing;
				if (thing != null)
				{
					thing.Destroy(DestroyMode.Vanish);
				}
				this.relicThing = null;
			}
		}

		// Token: 0x06015A0A RID: 88586 RVA: 0x00679281 File Offset: 0x00677481
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06015A0B RID: 88587 RVA: 0x0067928F File Offset: 0x0067748F
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.things;
		}

		// Token: 0x06015A0C RID: 88588 RVA: 0x00679298 File Offset: 0x00677498
		public void ExposeData()
		{
			Scribe_Deep.Look<SitePartParams>(ref this.parms, "parms", Array.Empty<object>());
			Scribe_Deep.Look<ThingOwner>(ref this.things, "things", new object[]
			{
				this
			});
			Scribe_Collections.Look<ThingDefCount>(ref this.lootThings, "lootThings", LookMode.Deep, Array.Empty<object>());
			Scribe_Defs.Look<SitePartDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.lastRaidTick, "lastRaidTick", -1, false);
			Scribe_Values.Look<bool>(ref this.conditionCauserWasSpawned, "conditionCauserWasSpawned", false, false);
			Scribe_Values.Look<bool>(ref this.hidden, "hidden", false, false);
			Scribe_Values.Look<int>(ref this.expectedEnemyCount, "expectedEnemyCount", -1, false);
			if (this.conditionCauserWasSpawned)
			{
				Scribe_References.Look<Thing>(ref this.conditionCauser, "conditionCauser", false);
			}
			else
			{
				Scribe_Deep.Look<Thing>(ref this.conditionCauser, "conditionCauser", Array.Empty<object>());
			}
			Scribe_Values.Look<bool>(ref this.relicWasSpawned, "relicWasSpawned", false, false);
			if (this.relicWasSpawned)
			{
				Scribe_References.Look<Thing>(ref this.relicThing, "relicThing", false);
				return;
			}
			Scribe_Deep.Look<Thing>(ref this.relicThing, "relicThing", Array.Empty<object>());
		}

		// Token: 0x0400F03E RID: 61502
		public Site site;

		// Token: 0x0400F03F RID: 61503
		public SitePartDef def;

		// Token: 0x0400F040 RID: 61504
		public bool hidden;

		// Token: 0x0400F041 RID: 61505
		public SitePartParams parms;

		// Token: 0x0400F042 RID: 61506
		public ThingOwner things;

		// Token: 0x0400F043 RID: 61507
		public int lastRaidTick = -1;

		// Token: 0x0400F044 RID: 61508
		public Thing conditionCauser;

		// Token: 0x0400F045 RID: 61509
		public bool conditionCauserWasSpawned;

		// Token: 0x0400F046 RID: 61510
		public List<ThingDefCount> lootThings;

		// Token: 0x0400F047 RID: 61511
		public int expectedEnemyCount = -1;

		// Token: 0x0400F048 RID: 61512
		public Thing relicThing;

		// Token: 0x0400F049 RID: 61513
		public bool relicWasSpawned;

		// Token: 0x0400F04A RID: 61514
		private const float AutoFoodLevel = 0.8f;
	}
}
