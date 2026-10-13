using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002D33 RID: 11571
	public class CompMechGestatorTank : ThingComp, IThingGlower
	{
		// Token: 0x17002956 RID: 10582
		// (get) Token: 0x06010422 RID: 66594 RVA: 0x004B354D File Offset: 0x004B174D
		private CompProperties_MechGestatorTank Props
		{
			get
			{
				return (CompProperties_MechGestatorTank)this.props;
			}
		}

		// Token: 0x06010423 RID: 66595 RVA: 0x004B355C File Offset: 0x004B175C
		public override string CompInspectStringExtra()
		{
			return string.Format("{0}: {1}", "Contains".Translate(), ((this.state == CompMechGestatorTank.TankState.Empty) ? "Nothing" : "Unknown").Translate().CapitalizeFirst());
		}

		// Token: 0x06010424 RID: 66596 RVA: 0x004B35A8 File Offset: 0x004B17A8
		public bool ShouldBeLitNow()
		{
			return this.state > CompMechGestatorTank.TankState.Empty;
		}

		// Token: 0x06010425 RID: 66597 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool DontDrawParent()
		{
			return true;
		}

		// Token: 0x17002957 RID: 10583
		// (get) Token: 0x06010426 RID: 66598 RVA: 0x004B35B3 File Offset: 0x004B17B3
		// (set) Token: 0x06010427 RID: 66599 RVA: 0x004B35BC File Offset: 0x004B17BC
		public CompMechGestatorTank.TankState State
		{
			get
			{
				return this.state;
			}
			set
			{
				this.state = value;
				if (this.parent.Spawned)
				{
					this.parent.DirtyMapMesh(this.parent.Map);
					CompGlower compGlower = this.parent.TryGetComp<CompGlower>();
					if (compGlower == null)
					{
						return;
					}
					compGlower.UpdateLit(this.parent.Map);
				}
			}
		}

		// Token: 0x06010428 RID: 66600 RVA: 0x004B3613 File Offset: 0x004B1813
		public override void PostPostMake()
		{
			this.triggerRadius = this.Props.triggerRadiusRange.RandomInRange;
		}

		// Token: 0x06010429 RID: 66601 RVA: 0x004B362B File Offset: 0x004B182B
		public override void CompTick()
		{
			if (this.state != CompMechGestatorTank.TankState.Proximity)
			{
				return;
			}
			if (this.parent.IsHashIntervalTick(250))
			{
				this.CheckTrigger();
			}
		}

		// Token: 0x0601042A RID: 66602 RVA: 0x004B3650 File Offset: 0x004B1850
		private void CheckTrigger()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(this.parent.Position, this.triggerRadius, false))
			{
				if (intVec.InBounds(this.parent.Map) && GenSight.LineOfSight(this.parent.Position, intVec, this.parent.Map))
				{
					foreach (Thing thing in intVec.GetThingList(this.parent.Map))
					{
						Pawn pawn = thing as Pawn;
						if (pawn != null && pawn.IsColonist)
						{
							this.Trigger(this.parent.Map);
							return;
						}
					}
				}
			}
		}

		// Token: 0x0601042B RID: 66603 RVA: 0x004B3758 File Offset: 0x004B1958
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			if (mode != DestroyMode.WillReplace && this.state != CompMechGestatorTank.TankState.Empty)
			{
				this.Trigger(previousMap);
			}
		}

		// Token: 0x0601042C RID: 66604 RVA: 0x004B3770 File Offset: 0x004B1970
		private void Trigger(Map map)
		{
			CompMechGestatorTank.<>c__DisplayClass17_0 CS$<>8__locals1 = new CompMechGestatorTank.<>c__DisplayClass17_0();
			CS$<>8__locals1.map = map;
			if (this.state == CompMechGestatorTank.TankState.Empty)
			{
				return;
			}
			this.State = CompMechGestatorTank.TankState.Empty;
			IntVec3 loc = this.parent.OccupiedRect().ExpandedBy(1).EdgeCells.Where(new Func<IntVec3, bool>(CS$<>8__locals1.<Trigger>g__Standable|0)).RandomElementWithFallback(IntVec3.Invalid);
			ScatterDebrisUtility.ScatterFilthAroundThing(this.parent, CS$<>8__locals1.map, ThingDefOf.Filth_GestationFluid, CompMechGestatorTank.GestationFluidFilthRange, 1, null);
			if (!loc.IsValid)
			{
				return;
			}
			Pawn pawn = PawnGenerator.GeneratePawn(this.Props.mechKindOptions.RandomElementByWeight((PawnKindDefWeight x) => x.weight).kindDef, Faction.OfMechanoids, null);
			GenSpawn.Spawn(pawn, loc, CS$<>8__locals1.map, WipeMode.Vanish);
			Pawn_StanceTracker stances = pawn.stances;
			if (stances != null)
			{
				StunHandler stunner = stances.stunner;
				if (stunner != null)
				{
					stunner.StunFor(180, null, false, true, false);
				}
			}
			LordJob_AssaultColony lordJob_AssaultColony;
			if (!CS$<>8__locals1.map.lordManager.TryGetLordByJob<LordJob_AssaultColony>(Faction.OfMechanoids, out lordJob_AssaultColony))
			{
				LordMaker.MakeNewLord(Faction.OfMechanoids, new LordJob_AssaultColony(Faction.OfMechanoids, false, false, false, false, false, false, false), CS$<>8__locals1.map, new List<Pawn>
				{
					pawn
				});
			}
			else
			{
				lordJob_AssaultColony.lord.AddPawn(pawn);
			}
			Messages.Message(this.Props.triggeredMessage.Formatted(pawn), pawn, MessageTypeDefOf.NegativeEvent, true);
			this.Props.triggerSound.PlayOneShot(this.parent);
		}

		// Token: 0x0601042D RID: 66605 RVA: 0x004B3914 File Offset: 0x004B1B14
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (DebugSettings.ShowDevGizmos)
			{
				if (this.State == CompMechGestatorTank.TankState.Empty)
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: Add mech",
						action = delegate()
						{
							this.State = CompMechGestatorTank.TankState.Proximity;
						}
					};
				}
				else
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: Remove mech",
						action = delegate()
						{
							this.State = CompMechGestatorTank.TankState.Empty;
						}
					};
				}
			}
			yield break;
		}

		// Token: 0x0601042E RID: 66606 RVA: 0x004B3924 File Offset: 0x004B1B24
		public override void PostPrintOnto(SectionLayer layer)
		{
			((this.state == CompMechGestatorTank.TankState.Empty) ? this.Props.emptyGraphic.Graphic : this.Props.dormantGraphic.Graphic).Print(layer, this.parent, 0f);
		}

		// Token: 0x0601042F RID: 66607 RVA: 0x004B3961 File Offset: 0x004B1B61
		public override void PostExposeData()
		{
			Scribe_Values.Look<CompMechGestatorTank.TankState>(ref this.state, "state", CompMechGestatorTank.TankState.Empty, false);
			Scribe_Values.Look<float>(ref this.triggerRadius, "triggerRadius", 0f, false);
		}

		// Token: 0x0400A9D6 RID: 43478
		private CompMechGestatorTank.TankState state;

		// Token: 0x0400A9D7 RID: 43479
		private float triggerRadius;

		// Token: 0x0400A9D8 RID: 43480
		private const int StunTicks = 180;

		// Token: 0x0400A9D9 RID: 43481
		private static readonly IntRange GestationFluidFilthRange = new IntRange(2, 4);

		// Token: 0x02002D34 RID: 11572
		public enum TankState
		{
			// Token: 0x0400A9DB RID: 43483
			Empty,
			// Token: 0x0400A9DC RID: 43484
			Dormant,
			// Token: 0x0400A9DD RID: 43485
			Proximity
		}
	}
}
