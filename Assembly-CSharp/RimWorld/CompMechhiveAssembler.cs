using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D39 RID: 11577
	public class CompMechhiveAssembler : ThingComp
	{
		// Token: 0x1700295A RID: 10586
		// (get) Token: 0x06010442 RID: 66626 RVA: 0x004B3B1A File Offset: 0x004B1D1A
		private CompProperties_MechhiveAssembler Props
		{
			get
			{
				return (CompProperties_MechhiveAssembler)this.props;
			}
		}

		// Token: 0x1700295B RID: 10587
		// (get) Token: 0x06010443 RID: 66627 RVA: 0x004B3B28 File Offset: 0x004B1D28
		private CompCanBeDormant DormancyComp
		{
			get
			{
				CompCanBeDormant result;
				if ((result = this.dormancyCompCached) == null)
				{
					result = (this.dormancyCompCached = this.parent.TryGetComp<CompCanBeDormant>());
				}
				return result;
			}
		}

		// Token: 0x1700295C RID: 10588
		// (get) Token: 0x06010444 RID: 66628 RVA: 0x004B3B53 File Offset: 0x004B1D53
		private bool Active
		{
			get
			{
				return this.core != null && this.DormancyComp.Awake;
			}
		}

		// Token: 0x06010445 RID: 66629 RVA: 0x004B3B6A File Offset: 0x004B1D6A
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastSpawnTick, "lastSpawnTick", 0, false);
			Scribe_Values.Look<float>(ref this.mapThreatPoints, "mapThreatPoints", 0f, false);
		}

		// Token: 0x06010446 RID: 66630 RVA: 0x004B3B94 File Offset: 0x004B1D94
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			List<Thing> list = this.parent.Map.listerThings.ThingsOfDef(ThingDefOf.CerebrexCore);
			if (!list.NullOrEmpty<Thing>())
			{
				Thing thing = list[0];
				this.core = ((thing != null) ? thing.TryGetComp<CompCerebrexCore>() : null);
			}
			if (!respawningAfterLoad)
			{
				Site site = this.parent.Map.Parent as Site;
				this.mapThreatPoints = ((site != null) ? site.ActualThreatPoints : -1f);
				if (this.mapThreatPoints < 0f)
				{
					Log.Error("Mechhive assembler failed to get threat points from site");
					this.mapThreatPoints = StorytellerUtility.DefaultThreatPointsNow(this.parent.Map);
				}
				this.lastSpawnTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06010447 RID: 66631 RVA: 0x004B3C48 File Offset: 0x004B1E48
		public override void CompTick()
		{
			if (!this.Active)
			{
				return;
			}
			float num = (float)this.lastSpawnTick + CompMechhiveAssembler.SpawnIntervalCurve.Evaluate(this.mapThreatPoints);
			if ((float)Find.TickManager.TicksGame > num)
			{
				this.lastSpawnTick = Find.TickManager.TicksGame;
				this.SpawnMech();
			}
		}

		// Token: 0x06010448 RID: 66632 RVA: 0x004B3C9C File Offset: 0x004B1E9C
		private void SpawnMech()
		{
			float pointsPerAssembler = CompMechhiveAssembler.MaxThreatPointsCurve.Evaluate(this.mapThreatPoints);
			float remainingPoints = pointsPerAssembler * 4f;
			foreach (Pawn pawn in this.core.AssemblerLord.ownedPawns)
			{
				remainingPoints -= pawn.kindDef.combatPower;
			}
			IEnumerable<PawnGenOption> source = from x in this.Props.options
			where x.Cost < pointsPerAssembler && x.Cost < remainingPoints
			select x;
			if (!source.Any<PawnGenOption>())
			{
				return;
			}
			PawnGenOption pawnGenOption = source.RandomElementByWeight((PawnGenOption x) => x.selectionWeight);
			if (pawnGenOption == null)
			{
				return;
			}
			Pawn pawn2 = PawnGenerator.GeneratePawn(pawnGenOption.kind, this.core.AssemblerLord.faction, null);
			IntVec3 loc = CellFinder.StandableCellNear(this.parent.Position, this.parent.Map, 5f, null);
			GenSpawn.Spawn(pawn2, loc, this.parent.Map, WipeMode.Vanish);
			this.core.AssemblerLord.AddPawn(pawn2);
			remainingPoints -= pawnGenOption.Cost;
		}

		// Token: 0x06010449 RID: 66633 RVA: 0x004B3E10 File Offset: 0x004B2010
		public override string CompInspectStringExtra()
		{
			if (this.Active)
			{
				return "ProducingMechanoids".Translate();
			}
			return null;
		}

		// Token: 0x0400A9E6 RID: 43494
		private static readonly SimpleCurve MaxThreatPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(100f, 200f),
				true
			},
			{
				new CurvePoint(1000f, 250f),
				true
			},
			{
				new CurvePoint(5000f, 750f),
				true
			},
			{
				new CurvePoint(10000f, 1250f),
				true
			}
		};

		// Token: 0x0400A9E7 RID: 43495
		private static readonly SimpleCurve SpawnIntervalCurve = new SimpleCurve
		{
			{
				new CurvePoint(100f, 35000f),
				true
			},
			{
				new CurvePoint(1000f, 30000f),
				true
			},
			{
				new CurvePoint(5000f, 20000f),
				true
			},
			{
				new CurvePoint(10000f, 15000f),
				true
			}
		};

		// Token: 0x0400A9E8 RID: 43496
		private int lastSpawnTick;

		// Token: 0x0400A9E9 RID: 43497
		private float mapThreatPoints = -1f;

		// Token: 0x0400A9EA RID: 43498
		[Unsaved(false)]
		private CompCerebrexCore core;

		// Token: 0x0400A9EB RID: 43499
		[Unsaved(false)]
		private CompCanBeDormant dormancyCompCached;
	}
}
