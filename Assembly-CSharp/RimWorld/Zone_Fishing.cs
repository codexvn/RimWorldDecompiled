using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020021C8 RID: 8648
	public class Zone_Fishing : Zone
	{
		// Token: 0x17001CB7 RID: 7351
		// (get) Token: 0x0600BCD3 RID: 48339 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool IsMultiselectable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001CB8 RID: 7352
		// (get) Token: 0x0600BCD4 RID: 48340 RVA: 0x0036C932 File Offset: 0x0036AB32
		protected override Color NextZoneColor
		{
			get
			{
				return ZoneColorUtility.NextFishingZoneColor();
			}
		}

		// Token: 0x17001CB9 RID: 7353
		// (get) Token: 0x0600BCD5 RID: 48341 RVA: 0x0036C939 File Offset: 0x0036AB39
		public bool Allowed
		{
			get
			{
				return this.allowed;
			}
		}

		// Token: 0x17001CBA RID: 7354
		// (get) Token: 0x0600BCD6 RID: 48342 RVA: 0x0036C941 File Offset: 0x0036AB41
		public bool ShouldFishNow
		{
			get
			{
				return this.Allowed && this.OverTargetPopulation && !this.PausedDueToResourceCount && this.UnderTargetResourceCount;
			}
		}

		// Token: 0x17001CBB RID: 7355
		// (get) Token: 0x0600BCD7 RID: 48343 RVA: 0x0036C963 File Offset: 0x0036AB63
		private bool AllFishableCellsFrozenNow
		{
			get
			{
				return this.cells.FindIndex(delegate(IntVec3 c)
				{
					TerrainDef terrain = c.GetTerrain(base.Map);
					return terrain == null || !terrain.temporary || terrain != TerrainDefOf.ThinIce;
				}) < 0;
			}
		}

		// Token: 0x17001CBC RID: 7356
		// (get) Token: 0x0600BCD8 RID: 48344 RVA: 0x0036C980 File Offset: 0x0036AB80
		private bool UnderTargetResourceCount
		{
			get
			{
				bool result;
				switch (this.repeatMode)
				{
				case FishRepeatMode.RepeatCount:
					result = (this.repeatCount > 0);
					break;
				case FishRepeatMode.TargetCount:
					result = (this.OwnedFishCount < this.targetCount);
					break;
				case FishRepeatMode.DoForever:
					result = true;
					break;
				default:
					result = (this.OwnedFishCount < this.repeatCount);
					break;
				}
				return result;
			}
		}

		// Token: 0x17001CBD RID: 7357
		// (get) Token: 0x0600BCD9 RID: 48345 RVA: 0x0036C9D9 File Offset: 0x0036ABD9
		public int OwnedFishCount
		{
			get
			{
				return base.Map.listerThings.ThingsInGroup(ThingRequestGroup.Fish).Sum((Thing t) => t.stackCount) + this.CarriedFishCount();
			}
		}

		// Token: 0x17001CBE RID: 7358
		// (get) Token: 0x0600BCDA RID: 48346 RVA: 0x0036CA18 File Offset: 0x0036AC18
		private bool OverTargetPopulation
		{
			get
			{
				return base.Map.TileInfo.MaxFishPopulation > 0f && this.cells.Count != 0 && this.FishType != WaterBodyType.None && base.Map.waterBodyTracker.PopulationPercentAt(base.Cells[0]) > this.targetPopulationPct;
			}
		}

		// Token: 0x17001CBF RID: 7359
		// (get) Token: 0x0600BCDB RID: 48347 RVA: 0x0036CA77 File Offset: 0x0036AC77
		public WaterBodyType FishType
		{
			get
			{
				if (this.cells.Count == 0)
				{
					return WaterBodyType.None;
				}
				return base.Cells[0].GetWaterBodyType(base.Map);
			}
		}

		// Token: 0x17001CC0 RID: 7360
		// (get) Token: 0x0600BCDC RID: 48348 RVA: 0x0036CA9F File Offset: 0x0036AC9F
		public bool AllFishableCellsFrozen
		{
			get
			{
				if (Find.TickManager.TicksGame < this.cachedFrozenTicks + 60)
				{
					return this.cachedIsFrozen;
				}
				this.cachedFrozenTicks = Find.TickManager.TicksGame;
				this.cachedIsFrozen = this.AllFishableCellsFrozenNow;
				return this.cachedIsFrozen;
			}
		}

		// Token: 0x17001CC1 RID: 7361
		// (get) Token: 0x0600BCDD RID: 48349 RVA: 0x0036CADF File Offset: 0x0036ACDF
		public bool PausedDueToResourceCount
		{
			get
			{
				if (Find.TickManager.TicksGame < this.lastPauseCheckTicks + 60)
				{
					return this.pausedDueToResourceCount;
				}
				this.RecheckPausedDueToResourceCount();
				return this.pausedDueToResourceCount;
			}
		}

		// Token: 0x17001CC2 RID: 7362
		// (get) Token: 0x0600BCDE RID: 48350 RVA: 0x0036CB0C File Offset: 0x0036AD0C
		public bool HasAnyFishableCells
		{
			get
			{
				for (int i = 0; i < this.cells.Count; i++)
				{
					if (this.IsFishable(this.cells[i]))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17001CC3 RID: 7363
		// (get) Token: 0x0600BCDF RID: 48351 RVA: 0x0036CB48 File Offset: 0x0036AD48
		public List<IntVec3> FishbleCells
		{
			get
			{
				this.tmpFishableCells.Clear();
				for (int i = 0; i < this.cells.Count; i++)
				{
					IntVec3 intVec = this.cells[i];
					if (this.IsFishable(intVec))
					{
						this.tmpFishableCells.Add(intVec);
					}
				}
				return this.tmpFishableCells;
			}
		}

		// Token: 0x0600BCE0 RID: 48352 RVA: 0x0036CB9E File Offset: 0x0036AD9E
		public bool IsFishable(IntVec3 cell)
		{
			return cell.GetTerrain(base.Map).IsWater && cell.GetWaterBodyType(base.Map) == this.FishType;
		}

		// Token: 0x17001CC4 RID: 7364
		// (get) Token: 0x0600BCE1 RID: 48353 RVA: 0x0036CBCC File Offset: 0x0036ADCC
		public IntVec3 RandomFishableCell
		{
			get
			{
				IntVec3 result;
				if (this.FishbleCells.TryRandomElement(out result))
				{
					return result;
				}
				return IntVec3.Invalid;
			}
		}

		// Token: 0x0600BCE2 RID: 48354 RVA: 0x0036CBF0 File Offset: 0x0036ADF0
		public Zone_Fishing()
		{
		}

		// Token: 0x0600BCE3 RID: 48355 RVA: 0x0036CC60 File Offset: 0x0036AE60
		public Zone_Fishing(ZoneManager zoneManager) : base("Zone_Fishing".Translate(), zoneManager)
		{
		}

		// Token: 0x0600BCE4 RID: 48356 RVA: 0x0036CCE0 File Offset: 0x0036AEE0
		public override IEnumerable<InspectTabBase> GetInspectTabs()
		{
			return this.ITabs;
		}

		// Token: 0x0600BCE5 RID: 48357 RVA: 0x0036CCE8 File Offset: 0x0036AEE8
		public override IEnumerable<Gizmo> GetZoneAddGizmos()
		{
			yield return DesignatorUtility.FindAllowedDesignator<Designator_ZoneAdd_Fishing_Expand>();
			yield break;
		}

		// Token: 0x0600BCE6 RID: 48358 RVA: 0x0036CCF4 File Offset: 0x0036AEF4
		private int CarriedFishCount()
		{
			int num = 0;
			foreach (Pawn pawn in base.Map.mapPawns.FreeColonistsSpawned)
			{
				Thing carriedThing = pawn.carryTracker.CarriedThing;
				if (carriedThing != null && ThingRequestGroup.Fish.Includes(carriedThing.def) && !carriedThing.Spawned)
				{
					num += carriedThing.stackCount;
				}
			}
			return num;
		}

		// Token: 0x0600BCE7 RID: 48359 RVA: 0x0036CD7C File Offset: 0x0036AF7C
		public void Notify_Fished()
		{
			if (this.repeatMode == FishRepeatMode.RepeatCount)
			{
				this.repeatCount--;
			}
			this.RecheckPausedDueToResourceCount();
		}

		// Token: 0x0600BCE8 RID: 48360 RVA: 0x0036CD9C File Offset: 0x0036AF9C
		public void RecheckPausedDueToResourceCount()
		{
			this.lastPauseCheckTicks = Find.TickManager.TicksGame;
			if (this.repeatMode != FishRepeatMode.TargetCount || !this.pauseWhenSatisfied)
			{
				this.pausedDueToResourceCount = false;
				return;
			}
			if (this.pausedDueToResourceCount)
			{
				this.pausedDueToResourceCount = (this.OwnedFishCount > this.unpauseAtCount);
				return;
			}
			this.pausedDueToResourceCount = (this.OwnedFishCount >= this.targetCount);
		}

		// Token: 0x0600BCE9 RID: 48361 RVA: 0x0036CE06 File Offset: 0x0036B006
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetFloatMenuOptions(selPawn))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			if (!this.ShouldFishNow)
			{
				if (!this.OverTargetPopulation)
				{
					yield return new FloatMenuOption("CannotFish".Translate().CapitalizeFirst() + ": " + "FishingSpotUnderTargetPopulation".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					yield break;
				}
				if (!this.Allowed)
				{
					yield return new FloatMenuOption("CannotFish".Translate().CapitalizeFirst() + ": " + "ForbiddenLower".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					yield break;
				}
				if (!this.UnderTargetResourceCount)
				{
					yield return new FloatMenuOption("CannotFish".Translate().CapitalizeFirst() + ": " + "FishingSpotOverTargetResourceCount".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					yield break;
				}
				if (this.PausedDueToResourceCount)
				{
					yield return new FloatMenuOption("CannotFish".Translate().CapitalizeFirst() + ": " + "Paused".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					yield break;
				}
				yield break;
			}
			else
			{
				if (selPawn.Ideo != null && !new HistoryEvent(HistoryEventDefOf.SlaughteredFish, selPawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job())
				{
					yield return new FloatMenuOption(string.Format("{0}: {1} {2}", "CannotFish".Translate().CapitalizeFirst(), "IdeoligionForbids".Translate(), "Fishing".Translate()), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					yield break;
				}
				if (selPawn.WorkTypeIsDisabled(WorkTypeDefOf.Hunting))
				{
					yield return new FloatMenuOption("CannotFish".Translate().CapitalizeFirst() + ": " + "CannotPrioritizeWorkTypeDisabled".Translate(WorkTypeDefOf.Hunting.gerundLabel), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					yield break;
				}
				if (!this.FishbleCells.Any<IntVec3>())
				{
					if (this.AllFishableCellsFrozenNow)
					{
						yield return new FloatMenuOption("CannotFish".Translate().CapitalizeFirst() + ": " + "FishingSpotFrozen".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
					else
					{
						yield return new FloatMenuOption("CannotFish".Translate().CapitalizeFirst() + ": " + "FishingSpotBlocked".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
					yield break;
				}
				if (!selPawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
				{
					yield return new FloatMenuOption("CannotFish".Translate().CapitalizeFirst() + ": " + "IncapableOfCapacity".Translate(PawnCapacityDefOf.Manipulation.label).CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					yield break;
				}
				int i = 0;
				bool flag = false;
				while (i < 60)
				{
					IntVec3 cell = this.RandomFishableCell;
					if (cell.IsValid && cell.Standable(base.Map) && !cell.IsForbidden(selPawn) && selPawn.CanReserve(cell, 1, -1, null, false))
					{
						IntVec3 spot = WorkGiver_Fish.BestStandSpotFor(selPawn, cell, true);
						if (spot.IsValid && (flag || !spot.GetTerrain(base.Map).IsWater))
						{
							yield return new FloatMenuOption("PrioritizeGenericSimple".Translate("Fishing".Translate()), delegate()
							{
								selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Fish, cell, spot), new JobTag?(JobTag.Misc), false);
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
							yield break;
						}
					}
					i++;
					if (i >= 60 && !flag)
					{
						flag = true;
						i = 0;
					}
				}
				if (this.cells.All((IntVec3 c) => c.IsForbidden(selPawn)))
				{
					yield return new FloatMenuOption("CannotFish".Translate().CapitalizeFirst() + ": " + "CannotPrioritizeForbiddenOutsideAllowedArea".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				yield break;
			}
			yield break;
		}

		// Token: 0x0600BCEA RID: 48362 RVA: 0x0036CE1D File Offset: 0x0036B01D
		public override IEnumerable<Gizmo> GetGizmos()
		{
			yield return new Command_Hide_ZoneFishing(this);
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			yield return new Command_Toggle
			{
				defaultLabel = "CommandAllowFish".Translate(),
				defaultDesc = "CommandAllowFishDesc".Translate(),
				icon = TexCommand.ForbidOff,
				hotKey = KeyBindingDefOf.Command_ItemForbid,
				isActive = (() => this.allowed),
				toggleAction = delegate()
				{
					this.allowed = !this.allowed;
				}
			};
			yield break;
			yield break;
		}

		// Token: 0x0600BCEB RID: 48363 RVA: 0x0036CE30 File Offset: 0x0036B030
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			float num = base.Map.waterBodyTracker.FishPopulationAt(base.Cells[0]);
			text += string.Format("\n{0}: {1:F0} ({2})", "FishPopulation".Translate().CapitalizeFirst(), num, FishingUtility.FishPopulationLabel(num));
			GameCondition gameCondition;
			if (base.Map.gameConditionManager.FishPopulationOffsetFactorPerDay(base.Map, out gameCondition) < 0f && gameCondition != null)
			{
				text += string.Format("\n{0}: {1}", "FishPopulationDecline".Translate().CapitalizeFirst(), gameCondition.LabelCap);
			}
			if (!this.ShouldFishNow)
			{
				if (!this.UnderTargetResourceCount)
				{
					text += "\n" + "CannotFish".Translate().CapitalizeFirst() + " (" + "FishingSpotOverTargetResourceCount".Translate() + ")";
				}
				else if (this.PausedDueToResourceCount)
				{
					text += "\n" + "CannotFish".Translate().CapitalizeFirst() + " (" + "Paused".Translate() + ")";
				}
				else if (!this.OverTargetPopulation)
				{
					text += "\n" + "CannotFish".Translate().CapitalizeFirst() + " (" + "FishingSpotUnderTargetPopulation".Translate() + ")";
				}
				else if (!this.Allowed)
				{
					text += "\n" + "CannotFish".Translate().CapitalizeFirst() + " (" + "ForbiddenLower".Translate() + ")";
				}
			}
			else if (this.AllFishableCellsFrozen)
			{
				text += "\n" + "CannotFish".Translate().CapitalizeFirst() + " (" + "FishingSpotFrozen".Translate() + ")";
			}
			else if (!this.FishbleCells.Any<IntVec3>())
			{
				text += "\n" + "CannotFish".Translate().CapitalizeFirst() + " (" + "FishingSpotBlocked".Translate() + ")";
			}
			return text;
		}

		// Token: 0x0600BCEC RID: 48364 RVA: 0x0036D104 File Offset: 0x0036B304
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.allowed, "allowed", true, false);
			Scribe_Values.Look<FishRepeatMode>(ref this.repeatMode, "repeatMode", FishRepeatMode.TargetCount, false);
			Scribe_Values.Look<int>(ref this.repeatCount, "repeatCount", 1, false);
			Scribe_Values.Look<int>(ref this.targetCount, "targetCount", 100, false);
			Scribe_Values.Look<float>(ref this.targetPopulationPct, "targetPopulationPct", 0.5f, false);
			Scribe_Values.Look<bool>(ref this.pauseWhenSatisfied, "pauseWhenSatisfied", false, false);
			Scribe_Values.Look<int>(ref this.unpauseAtCount, "unpauseAtCount", 50, false);
			Scribe_Values.Look<bool>(ref this.pausedDueToResourceCount, "pausedDueToResourceCount", false, false);
		}

		// Token: 0x04008223 RID: 33315
		private bool allowed = true;

		// Token: 0x04008224 RID: 33316
		public int repeatCount = 1;

		// Token: 0x04008225 RID: 33317
		public int targetCount = 100;

		// Token: 0x04008226 RID: 33318
		public float targetPopulationPct = 0.5f;

		// Token: 0x04008227 RID: 33319
		public FishRepeatMode repeatMode = FishRepeatMode.TargetCount;

		// Token: 0x04008228 RID: 33320
		public bool pauseWhenSatisfied;

		// Token: 0x04008229 RID: 33321
		public int unpauseAtCount;

		// Token: 0x0400822A RID: 33322
		private bool pausedDueToResourceCount;

		// Token: 0x0400822B RID: 33323
		private bool cachedIsFrozen;

		// Token: 0x0400822C RID: 33324
		private int cachedFrozenTicks = int.MinValue;

		// Token: 0x0400822D RID: 33325
		private int lastPauseCheckTicks = int.MinValue;

		// Token: 0x0400822E RID: 33326
		private readonly ITab[] ITabs = new ITab[]
		{
			new ITab_Fishing()
		};

		// Token: 0x0400822F RID: 33327
		private const string FishTexPath = "UI/Designators/ZoneCreate_Fishing";

		// Token: 0x04008230 RID: 33328
		private const int FrozenCheckInterval = 60;

		// Token: 0x04008231 RID: 33329
		private List<IntVec3> tmpFishableCells = new List<IntVec3>();
	}
}
