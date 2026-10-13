using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002E43 RID: 11843
	public class CompSpawnPawnOnDestroyed : ThingComp
	{
		// Token: 0x17002ABA RID: 10938
		// (get) Token: 0x06010AC6 RID: 68294 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool JoinLord
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002ABB RID: 10939
		// (get) Token: 0x06010AC7 RID: 68295 RVA: 0x004CE693 File Offset: 0x004CC893
		private CompProperties_SpawnPawnOnDestroyed Props
		{
			get
			{
				return (CompProperties_SpawnPawnOnDestroyed)this.props;
			}
		}

		// Token: 0x06010AC8 RID: 68296 RVA: 0x004CE6A0 File Offset: 0x004CC8A0
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (mode != DestroyMode.KillFinalize)
			{
				return;
			}
			PawnKindDef pawnKind = this.Props.pawnKind;
			Faction faction = this.parent.Faction;
			PawnGenerationContext context = PawnGenerationContext.NonPlayer;
			float? fixedBiologicalAge = new float?(0f);
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(pawnKind, faction, context, null, true, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, fixedBiologicalAge, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
			GenSpawn.Spawn(pawn, this.parent.Position, previousMap, WipeMode.VanishOrMoveAside);
			IntVec3 intVec;
			if (CellFinder.TryFindRandomSpawnCellForPawnNear(this.parent.Position, previousMap, out intVec, 5, (IntVec3 c) => GenSight.LineOfSight(this.parent.Position, c, previousMap, true, null, 0, 0)))
			{
				pawn.rotationTracker.FaceCell(intVec);
				PawnFlyer pawnFlyer = PawnFlyer.MakeFlyer(ThingDefOf.PawnFlyer_Stun, pawn, intVec, null, null, false, null, null, default(LocalTargetInfo));
				if (pawnFlyer != null)
				{
					GenSpawn.Spawn(pawnFlyer, intVec, previousMap, WipeMode.Vanish);
				}
			}
			if (this.JoinLord && this.Props.lordJob != null)
			{
				Lord lord = CompSpawnerPawn.FindLordToJoin(this.parent, this.Props.lordJob, false, null, 2.1474836E+09f);
				if (lord == null)
				{
					lord = LordMaker.MakeNewLord(this.parent.Faction, Activator.CreateInstance(this.Props.lordJob) as LordJob, previousMap, null);
				}
				lord.AddPawn(pawn);
			}
			CompCanBeDormant compCanBeDormant = pawn.TryGetComp<CompCanBeDormant>();
			if (compCanBeDormant == null)
			{
				return;
			}
			compCanBeDormant.WakeUp();
		}

		// Token: 0x0400ADD0 RID: 44496
		private const int InitialJumpDistance = 5;
	}
}
