using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C9B RID: 11419
	public class CompFleshmassNucleus : ThingComp, IActivity, IRoofCollapseAlert
	{
		// Token: 0x17002882 RID: 10370
		// (get) Token: 0x06010060 RID: 65632 RVA: 0x004A3FCD File Offset: 0x004A21CD
		public CompProperties_FleshmassNucleus Props
		{
			get
			{
				return (CompProperties_FleshmassNucleus)this.props;
			}
		}

		// Token: 0x17002883 RID: 10371
		// (get) Token: 0x06010061 RID: 65633 RVA: 0x004A3FDC File Offset: 0x004A21DC
		public CompActivity Activity
		{
			get
			{
				CompActivity result;
				if ((result = this.activityComp) == null)
				{
					result = (this.activityComp = this.Pawn.TryGetComp<CompActivity>());
				}
				return result;
			}
		}

		// Token: 0x17002884 RID: 10372
		// (get) Token: 0x06010062 RID: 65634 RVA: 0x0049B545 File Offset: 0x00499745
		public Pawn Pawn
		{
			get
			{
				return (Pawn)this.parent;
			}
		}

		// Token: 0x06010063 RID: 65635 RVA: 0x004A4007 File Offset: 0x004A2207
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.minNextFireTick, "minNextFireTick", 0, false);
		}

		// Token: 0x06010064 RID: 65636 RVA: 0x004A4024 File Offset: 0x004A2224
		public override void PostPostMake()
		{
			this.minNextFireTick = GenTicks.TicksGame + CompFleshmassNucleus.InitialMeatDelayRange.RandomInRange;
		}

		// Token: 0x06010065 RID: 65637 RVA: 0x004A404C File Offset: 0x004A224C
		public override void CompTick()
		{
			if (this.debugNextMeatTick > 0 && GenTicks.TicksGame == this.debugNextMeatTick)
			{
				this.CreateMeat();
			}
			if (this.debugActiveTick > 0 && GenTicks.TicksGame == this.debugActiveTick)
			{
				this.Activity.EnterActiveState();
			}
			if (GenTicks.TicksGame < this.minNextFireTick)
			{
				return;
			}
			if (Rand.MTBEventOccurs(1f, 60000f, 1f))
			{
				this.CreateMeat();
			}
		}

		// Token: 0x06010066 RID: 65638 RVA: 0x004A40C0 File Offset: 0x004A22C0
		public override void Notify_Downed()
		{
			this.Pawn.Kill(null, null);
		}

		// Token: 0x06010067 RID: 65639 RVA: 0x004A40E4 File Offset: 0x004A22E4
		private void CreateMeat()
		{
			this.minNextFireTick = GenTicks.TicksGame + 15000;
			int num = Mathf.CeilToInt(this.Props.activityMeatPerDayCurve.Evaluate(this.Activity.ActivityLevel));
			int randomInRange = CompFleshmassNucleus.MeatClumpRange.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				IntVec3 dropCell;
				if (CellFinder.TryRandomClosewalkCellNear(this.Pawn.PositionHeld, this.Pawn.MapHeld, 3, out dropCell, null))
				{
					Thing thing = ThingMaker.MakeThing(ThingDefOf.Meat_Twisted, null);
					thing.stackCount = Rand.RangeInclusive(20, num - 20 * (randomInRange - i - 1));
					Thing thing2;
					GenDrop.TryDropSpawn(thing, dropCell, this.Pawn.MapHeld, ThingPlaceMode.Near, out thing2, null, null, true);
				}
			}
			EffecterDefOf.MeatExplosion.Spawn(this.Pawn.PositionHeld, this.Pawn.MapHeld, 1f).Cleanup();
		}

		// Token: 0x06010068 RID: 65640 RVA: 0x004A41C3 File Offset: 0x004A23C3
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!DebugSettings.ShowDevGizmos)
			{
				yield break;
			}
			yield return new Command_Action
			{
				defaultLabel = "DEV: Create meat",
				action = delegate()
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					for (int i = 10; i >= 0; i--)
					{
						int ticks = i * 60;
						list.Add(new FloatMenuOption(i.ToString() + "s", delegate()
						{
							this.debugNextMeatTick = GenTicks.TicksGame + ticks;
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					Find.WindowStack.Add(new FloatMenu(list));
				}
			};
			yield return new Command_Action
			{
				defaultLabel = "DEV: Go active in...",
				action = delegate()
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					for (int i = 0; i <= 5; i++)
					{
						int ticks = i * 60;
						list.Add(new FloatMenuOption(i.ToString() + "s", delegate()
						{
							this.debugActiveTick = GenTicks.TicksGame + ticks;
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					Find.WindowStack.Add(new FloatMenu(list));
				}
			};
			yield break;
		}

		// Token: 0x06010069 RID: 65641 RVA: 0x004A41D4 File Offset: 0x004A23D4
		public void OnActivityActivated()
		{
			IntVec3 positionHeld = this.Pawn.PositionHeld;
			Map mapHeld = this.Pawn.MapHeld;
			FleshbeastUtility.MeatSplatter(10, positionHeld, mapHeld, FleshbeastUtility.MeatExplosionSize.Large);
			this.Pawn.Destroy(DestroyMode.Vanish);
			float threatPoints = StorytellerUtility.DefaultThreatPointsNow(mapHeld) * 0.5f;
			Building_FleshmassHeart building_FleshmassHeart = (Building_FleshmassHeart)ThingMaker.MakeThing(ThingDefOf.FleshmassHeart, null);
			building_FleshmassHeart.Comp.threatPoints = threatPoints;
			building_FleshmassHeart.SetFaction(Faction.OfEntities, null);
			GenSpawn.Spawn(building_FleshmassHeart, positionHeld, mapHeld, WipeMode.Vanish);
		}

		// Token: 0x0601006A RID: 65642 RVA: 0x000026EA File Offset: 0x000008EA
		public void OnPassive()
		{
		}

		// Token: 0x0601006B RID: 65643 RVA: 0x00002501 File Offset: 0x00000701
		public bool ShouldGoPassive()
		{
			return false;
		}

		// Token: 0x0601006C RID: 65644 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool CanBeSuppressed()
		{
			return true;
		}

		// Token: 0x0601006D RID: 65645 RVA: 0x004A4250 File Offset: 0x004A2450
		public bool CanActivate()
		{
			using (IEnumerator<IntVec3> enumerator = GenAdj.CellsOccupiedBy(this.Pawn.PositionHeld, Rot4.South, new IntVec2(2, 2)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.SupportsStructureType(this.Pawn.MapHeld, ThingDefOf.FleshmassHeart.terrainAffordanceNeeded))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0601006E RID: 65646 RVA: 0x00002C42 File Offset: 0x00000E42
		public string ActivityTooltipExtra()
		{
			return null;
		}

		// Token: 0x0601006F RID: 65647 RVA: 0x004A42D0 File Offset: 0x004A24D0
		public RoofCollapseResponse Notify_OnBeforeRoofCollapse()
		{
			IntVec3 cell;
			if (RCellFinder.TryFindRandomCellNearWith(this.Pawn.Position, (IntVec3 c) => CompFleshmassNucleus.IsValidCell(c, this.Pawn.MapHeld), this.Pawn.MapHeld, out cell, 10, 2147483647))
			{
				SkipUtility.SkipTo(this.Pawn, cell, this.Pawn.MapHeld);
				this.Activity.AdjustActivity(this.Props.activityOnRoofCollapsed);
			}
			return RoofCollapseResponse.RemoveThing;
		}

		// Token: 0x06010070 RID: 65648 RVA: 0x004A433E File Offset: 0x004A253E
		private static bool IsValidCell(IntVec3 cell, Map map)
		{
			return cell.InBounds(map) && cell.Walkable(map);
		}

		// Token: 0x0400A795 RID: 42901
		private int minNextFireTick;

		// Token: 0x0400A796 RID: 42902
		private CompActivity activityComp;

		// Token: 0x0400A797 RID: 42903
		private int debugNextMeatTick = -1;

		// Token: 0x0400A798 RID: 42904
		private int debugActiveTick = -1;

		// Token: 0x0400A799 RID: 42905
		private static readonly IntRange InitialMeatDelayRange = new IntRange(5000, 12500);

		// Token: 0x0400A79A RID: 42906
		private static readonly IntRange MeatClumpRange = new IntRange(3, 6);

		// Token: 0x0400A79B RID: 42907
		private const int MinRefireTicks = 15000;

		// Token: 0x0400A79C RID: 42908
		private const int MeatDaysMTB = 1;

		// Token: 0x0400A79D RID: 42909
		private const int MinMeatPerStack = 20;

		// Token: 0x0400A79E RID: 42910
		private const float HeartPointsFactor = 0.5f;
	}
}
