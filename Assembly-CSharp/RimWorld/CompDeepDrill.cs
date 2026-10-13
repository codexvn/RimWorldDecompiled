using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C50 RID: 11344
	public class CompDeepDrill : ThingComp
	{
		// Token: 0x1700280D RID: 10253
		// (get) Token: 0x0600FE5B RID: 65115 RVA: 0x0049BEF6 File Offset: 0x0049A0F6
		[Obsolete("Use WorkPerPortionBase constant directly.")]
		public static float WorkPerPortionCurrentDifficulty
		{
			get
			{
				return 10000f;
			}
		}

		// Token: 0x1700280E RID: 10254
		// (get) Token: 0x0600FE5C RID: 65116 RVA: 0x0049BEFD File Offset: 0x0049A0FD
		public float ProgressToNextPortionPercent
		{
			get
			{
				return this.portionProgress / 10000f;
			}
		}

		// Token: 0x0600FE5D RID: 65117 RVA: 0x0049BF0C File Offset: 0x0049A10C
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.powerComp = this.parent.TryGetComp<CompPowerTrader>();
		}

		// Token: 0x0600FE5E RID: 65118 RVA: 0x0049BF1F File Offset: 0x0049A11F
		public override void PostExposeData()
		{
			Scribe_Values.Look<float>(ref this.portionProgress, "portionProgress", 0f, false);
			Scribe_Values.Look<float>(ref this.portionYieldPct, "portionYieldPct", 0f, false);
			Scribe_Values.Look<int>(ref this.lastUsedTick, "lastUsedTick", 0, false);
		}

		// Token: 0x0600FE5F RID: 65119 RVA: 0x0049BF60 File Offset: 0x0049A160
		public void DrillWorkDone(Pawn driller, int delta)
		{
			float num = driller.GetStatValue(StatDefOf.DeepDrillingSpeed, true, -1) * (float)delta;
			this.portionProgress += num;
			this.portionYieldPct += num * driller.GetStatValue(StatDefOf.MiningYield, true, -1) / 10000f;
			this.lastUsedTick = Find.TickManager.TicksGame;
			if (this.portionProgress > 10000f)
			{
				this.TryProducePortion(this.portionYieldPct, driller);
				this.portionProgress = 0f;
				this.portionYieldPct = 0f;
			}
		}

		// Token: 0x0600FE60 RID: 65120 RVA: 0x0049BFEE File Offset: 0x0049A1EE
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			this.portionProgress = 0f;
			this.portionYieldPct = 0f;
			this.lastUsedTick = -99999;
		}

		// Token: 0x0600FE61 RID: 65121 RVA: 0x0049C014 File Offset: 0x0049A214
		private void TryProducePortion(float yieldPct, Pawn driller = null)
		{
			ThingDef thingDef;
			int num;
			IntVec3 intVec;
			bool nextResource = this.GetNextResource(out thingDef, out num, out intVec);
			if (thingDef == null)
			{
				return;
			}
			int num2 = Mathf.Min(num, thingDef.deepCountPerPortion);
			if (nextResource)
			{
				this.parent.Map.deepResourceGrid.SetAt(intVec, thingDef, num - num2);
			}
			int stackCount = Mathf.Max(1, GenMath.RoundRandom((float)num2 * yieldPct));
			Thing thing = ThingMaker.MakeThing(thingDef, null);
			thing.stackCount = stackCount;
			GenPlace.TryPlaceThing(thing, this.parent.InteractionCell, this.parent.Map, ThingPlaceMode.Near, null, (IntVec3 p) => p != this.parent.Position && p != this.parent.InteractionCell, null, 1);
			if (driller != null)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.Mined, driller.Named(HistoryEventArgsNames.Doer)), true);
			}
			if (nextResource && !this.ValuableResourcesPresent())
			{
				if (DeepDrillUtility.GetBaseResource(this.parent.Map, this.parent.Position) == null)
				{
					Messages.Message("DeepDrillExhaustedNoFallback".Translate(), this.parent, MessageTypeDefOf.TaskCompletion, true);
					return;
				}
				Messages.Message("DeepDrillExhausted".Translate(Find.ActiveLanguageWorker.Pluralize(DeepDrillUtility.GetBaseResource(this.parent.Map, this.parent.Position).label, -1)), this.parent, MessageTypeDefOf.TaskCompletion, true);
				for (int i = 0; i < 21; i++)
				{
					IntVec3 c = intVec + GenRadial.RadialPattern[i];
					if (c.InBounds(this.parent.Map))
					{
						ThingWithComps firstThingWithComp = c.GetFirstThingWithComp(this.parent.Map);
						if (firstThingWithComp != null && !firstThingWithComp.GetComp<CompDeepDrill>().ValuableResourcesPresent())
						{
							firstThingWithComp.SetForbidden(true, true);
						}
					}
				}
			}
		}

		// Token: 0x0600FE62 RID: 65122 RVA: 0x0049C1E8 File Offset: 0x0049A3E8
		private bool GetNextResource(out ThingDef resDef, out int countPresent, out IntVec3 cell)
		{
			return DeepDrillUtility.GetNextResource(this.parent.Position, this.parent.Map, out resDef, out countPresent, out cell);
		}

		// Token: 0x0600FE63 RID: 65123 RVA: 0x0049C208 File Offset: 0x0049A408
		public bool CanDrillNow()
		{
			return (this.powerComp == null || this.powerComp.PowerOn) && (DeepDrillUtility.GetBaseResource(this.parent.Map, this.parent.Position) != null || this.ValuableResourcesPresent());
		}

		// Token: 0x0600FE64 RID: 65124 RVA: 0x0049C248 File Offset: 0x0049A448
		public bool ValuableResourcesPresent()
		{
			ThingDef thingDef;
			int num;
			IntVec3 intVec;
			return this.GetNextResource(out thingDef, out num, out intVec);
		}

		// Token: 0x0600FE65 RID: 65125 RVA: 0x0049C261 File Offset: 0x0049A461
		public bool UsedLastTick()
		{
			return this.lastUsedTick >= Find.TickManager.TicksGame - 1;
		}

		// Token: 0x0600FE66 RID: 65126 RVA: 0x0049C27A File Offset: 0x0049A47A
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Produce portion (100% yield)",
					action = delegate()
					{
						this.TryProducePortion(1f, null);
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0600FE67 RID: 65127 RVA: 0x0049C28C File Offset: 0x0049A48C
		public override string CompInspectStringExtra()
		{
			if (!this.parent.Spawned)
			{
				return null;
			}
			ThingDef thingDef;
			int num;
			IntVec3 intVec;
			this.GetNextResource(out thingDef, out num, out intVec);
			if (thingDef == null)
			{
				return "DeepDrillNoResources".Translate();
			}
			return "ResourceBelow".Translate() + ": " + thingDef.LabelCap + "\n" + "ProgressToNextPortion".Translate() + ": " + this.ProgressToNextPortionPercent.ToStringPercent("F0");
		}

		// Token: 0x0400A6B2 RID: 42674
		private CompPowerTrader powerComp;

		// Token: 0x0400A6B3 RID: 42675
		private float portionProgress;

		// Token: 0x0400A6B4 RID: 42676
		private float portionYieldPct;

		// Token: 0x0400A6B5 RID: 42677
		private int lastUsedTick = -99999;

		// Token: 0x0400A6B6 RID: 42678
		private const float WorkPerPortionBase = 10000f;
	}
}
