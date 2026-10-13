using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E46 RID: 11846
	public class CompSpawnPawnsOnDamaged : ThingComp
	{
		// Token: 0x17002ABC RID: 10940
		// (get) Token: 0x06010ACD RID: 68301 RVA: 0x004CE8E6 File Offset: 0x004CCAE6
		public CompProperties_SpawnPawnsOnDamaged Props
		{
			get
			{
				return (CompProperties_SpawnPawnsOnDamaged)this.props;
			}
		}

		// Token: 0x06010ACE RID: 68302 RVA: 0x004CE8F4 File Offset: 0x004CCAF4
		public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (!ModLister.CheckAnomaly("Pawn spawning pawns on hit"))
			{
				return;
			}
			if (!Rand.Chance(this.Props.chance))
			{
				return;
			}
			if (Find.TickManager.TicksGame < this.lastSpawned + this.Props.cooldownTicks)
			{
				return;
			}
			int randomInRange = this.Props.spawnPawnCountRange.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				PawnKindDef kind = this.Props.spawnPawnKindOptions.RandomElement<PawnKindDef>();
				Faction faction = this.parent.Faction;
				PawnGenerationContext context = PawnGenerationContext.NonPlayer;
				float? fixedBiologicalAge = new float?(0f);
				Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind, faction, context, null, true, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, fixedBiologicalAge, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
				this.SpawnPawn(pawn, this.parent.PositionHeld, this.parent.MapHeld);
			}
			CompSpawnPawnsOnDamaged.tmpTakenCells.Clear();
			FleshbeastUtility.MeatSplatter(CompSpawnPawnsOnDamaged.BloodFilthCountRange.RandomInRange, this.parent.PositionHeld, this.parent.MapHeld, FleshbeastUtility.MeatExplosionSize.Normal);
			FilthMaker.TryMakeFilth(this.parent.PositionHeld, this.parent.MapHeld, ThingDefOf.Filth_TwistedFlesh, 1, FilthSourceFlags.None, true);
			this.lastSpawned = Find.TickManager.TicksGame;
		}

		// Token: 0x06010ACF RID: 68303 RVA: 0x004CEA9C File Offset: 0x004CCC9C
		private void SpawnPawn(Pawn pawn, IntVec3 position, Map map)
		{
			GenSpawn.Spawn(pawn, position, map, WipeMode.VanishOrMoveAside);
			IntVec3 intVec;
			if (CellFinder.TryFindRandomSpawnCellForPawnNear(position, map, out intVec, 5, (IntVec3 c) => !CompSpawnPawnsOnDamaged.tmpTakenCells.Contains(c) && GenSight.LineOfSight(position, c, map, true, null, 0, 0)))
			{
				pawn.rotationTracker.FaceCell(intVec);
				CompSpawnPawnsOnDamaged.tmpTakenCells.Add(intVec);
				PawnFlyer pawnFlyer = PawnFlyer.MakeFlyer(ThingDefOf.PawnFlyer_Stun, pawn, intVec, null, null, false, null, null, default(LocalTargetInfo));
				if (pawnFlyer != null)
				{
					GenSpawn.Spawn(pawnFlyer, intVec, map, WipeMode.Vanish);
				}
			}
		}

		// Token: 0x06010AD0 RID: 68304 RVA: 0x004CEB40 File Offset: 0x004CCD40
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastSpawned, "lastSpawned", -99999, false);
		}

		// Token: 0x0400ADD7 RID: 44503
		private static readonly IntRange BloodFilthCountRange = new IntRange(1, 2);

		// Token: 0x0400ADD8 RID: 44504
		private const int InitialJumpDistance = 5;

		// Token: 0x0400ADD9 RID: 44505
		private int lastSpawned = -99999;

		// Token: 0x0400ADDA RID: 44506
		private static List<IntVec3> tmpTakenCells = new List<IntVec3>();
	}
}
