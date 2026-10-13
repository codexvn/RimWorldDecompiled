using System;
using RimWorld;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x02000193 RID: 403
	public class DeathActionWorker_Divide : DeathActionWorker
	{
		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000B64 RID: 2916 RVA: 0x00035EBF File Offset: 0x000340BF
		public DeathActionProperties_Divide Props
		{
			get
			{
				return (DeathActionProperties_Divide)this.props;
			}
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x00035ECC File Offset: 0x000340CC
		public override void PawnDied(Corpse corpse, Lord prevLord)
		{
			if (!ModLister.CheckAnomaly("Pawn dividing"))
			{
				return;
			}
			Pawn innerPawn = corpse.InnerPawn;
			if (innerPawn == null)
			{
				return;
			}
			int dividePawnCount = this.Props.dividePawnCount;
			for (int i = 0; i < dividePawnCount; i++)
			{
				PawnKindDef kind = this.Props.dividePawnKindOptions.RandomElement<PawnKindDef>();
				Faction faction = corpse.InnerPawn.Faction;
				PawnGenerationContext context = PawnGenerationContext.NonPlayer;
				float? fixedBiologicalAge = new float?(0f);
				Pawn child = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind, faction, context, null, true, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, fixedBiologicalAge, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
				this.SpawnPawn(child, innerPawn, corpse.PositionHeld, corpse.MapHeld, prevLord);
			}
			foreach (PawnKindDef kind2 in this.Props.dividePawnKindAdditionalForced)
			{
				Faction faction2 = corpse.InnerPawn.Faction;
				PawnGenerationContext context2 = PawnGenerationContext.NonPlayer;
				float? fixedBiologicalAge = new float?(0f);
				Pawn child2 = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind2, faction2, context2, null, true, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, fixedBiologicalAge, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
				this.SpawnPawn(child2, innerPawn, corpse.PositionHeld, corpse.MapHeld, prevLord);
			}
			FleshbeastUtility.MeatSplatter(this.Props.divideBloodFilthCountRange.RandomInRange, corpse.PositionHeld, corpse.MapHeld, FleshbeastUtility.ExplosionSizeFor(innerPawn));
			FilthMaker.TryMakeFilth(corpse.PositionHeld, corpse.MapHeld, ThingDefOf.Filth_TwistedFlesh, 1, FilthSourceFlags.None, true);
			corpse.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x00036124 File Offset: 0x00034324
		private void SpawnPawn(Pawn child, Pawn parent, IntVec3 position, Map map, Lord lord)
		{
			GenSpawn.Spawn(child, position, map, WipeMode.VanishOrMoveAside);
			if (lord != null)
			{
				lord.AddPawn(child);
			}
			CompInspectStringEmergence compInspectStringEmergence = child.TryGetComp<CompInspectStringEmergence>();
			if (compInspectStringEmergence != null)
			{
				compInspectStringEmergence.sourcePawn = parent;
			}
			FleshbeastUtility.SpawnPawnAsFlyer(child, map, position, 5, true);
		}
	}
}
