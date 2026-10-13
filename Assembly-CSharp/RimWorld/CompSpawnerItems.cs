using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E35 RID: 11829
	public class CompSpawnerItems : ThingComp
	{
		// Token: 0x17002AAB RID: 10923
		// (get) Token: 0x06010A77 RID: 68215 RVA: 0x004CD453 File Offset: 0x004CB653
		public CompProperties_SpawnerItems Props
		{
			get
			{
				return (CompProperties_SpawnerItems)this.props;
			}
		}

		// Token: 0x17002AAC RID: 10924
		// (get) Token: 0x06010A78 RID: 68216 RVA: 0x004CD460 File Offset: 0x004CB660
		public bool Active
		{
			get
			{
				CompCanBeDormant comp = this.parent.GetComp<CompCanBeDormant>();
				return comp == null || comp.Awake;
			}
		}

		// Token: 0x06010A79 RID: 68217 RVA: 0x004CD484 File Offset: 0x004CB684
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			yield return new Command_Action
			{
				defaultLabel = "DEV: Spawn items",
				action = delegate()
				{
					this.SpawnItems();
				}
			};
			yield break;
		}

		// Token: 0x06010A7A RID: 68218 RVA: 0x004CD494 File Offset: 0x004CB694
		private void SpawnItems()
		{
			ThingDef thingDef;
			if (this.Props.MatchingItems.TryRandomElement(out thingDef))
			{
				int stackCount = Mathf.CeilToInt(this.Props.approxMarketValuePerDay / thingDef.BaseMarketValue);
				Thing thing = ThingMaker.MakeThing(thingDef, null);
				thing.stackCount = stackCount;
				GenPlace.TryPlaceThing(thing, this.parent.Position, this.parent.Map, ThingPlaceMode.Near, null, null, null, 1);
			}
		}

		// Token: 0x06010A7B RID: 68219 RVA: 0x004CD504 File Offset: 0x004CB704
		public override void CompTickRare()
		{
			if (!this.Active)
			{
				return;
			}
			this.ticksPassed += 250;
			if (this.ticksPassed >= this.Props.spawnInterval)
			{
				this.SpawnItems();
				this.ticksPassed -= this.Props.spawnInterval;
			}
		}

		// Token: 0x06010A7C RID: 68220 RVA: 0x004CD55D File Offset: 0x004CB75D
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksPassed, "ticksPassed", 0, false);
		}

		// Token: 0x06010A7D RID: 68221 RVA: 0x004CD574 File Offset: 0x004CB774
		public override string CompInspectStringExtra()
		{
			if (this.Active)
			{
				return "NextSpawnedResourceIn".Translate() + ": " + (this.Props.spawnInterval - this.ticksPassed).ToStringTicksToPeriod(true, false, true, true, false);
			}
			return null;
		}

		// Token: 0x0400AD91 RID: 44433
		private int ticksPassed;
	}
}
