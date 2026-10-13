using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C67 RID: 11367
	public class CompDreadmeld : ThingComp
	{
		// Token: 0x0600FEDA RID: 65242 RVA: 0x0049DB98 File Offset: 0x0049BD98
		public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (!ModLister.CheckAnomaly("Dreadmeld"))
			{
				return;
			}
			if (!this.parent.Spawned)
			{
				return;
			}
			float num = this.totalDamageTaken;
			this.totalDamageTaken += totalDamageDealt;
			if (num == 0f)
			{
				int randomInRange = CompDreadmeld.FingerSpikeSpawnCountRange.RandomInRange;
				for (int i = 0; i < randomInRange; i++)
				{
					PawnKindDef fingerspike = PawnKindDefOf.Fingerspike;
					Faction faction = this.parent.Faction;
					PawnGenerationContext context = PawnGenerationContext.NonPlayer;
					float? fixedBiologicalAge = new float?(0f);
					Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(fingerspike, faction, context, null, true, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, fixedBiologicalAge, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
					this.SpawnPawn(pawn, this.parent.PositionHeld, this.parent.MapHeld);
				}
				FleshbeastUtility.MeatSplatter(CompDreadmeld.BloodFilthCountRange.RandomInRange, this.parent.PositionHeld, this.parent.MapHeld, FleshbeastUtility.MeatExplosionSize.Normal);
				FilthMaker.TryMakeFilth(this.parent.PositionHeld, this.parent.MapHeld, ThingDefOf.Filth_TwistedFlesh, 1, FilthSourceFlags.None, true);
			}
			if ((int)num / 200 < (int)this.totalDamageTaken / 200)
			{
				foreach (Pawn pawn2 in FleshbeastUtility.GetFleshbeastsForPoints((float)CompDreadmeld.ThresholdSpawnPointsRange.RandomInRange, this.parent.Map, false))
				{
					this.SpawnPawn(pawn2, this.parent.PositionHeld, this.parent.MapHeld);
				}
				FleshbeastUtility.MeatSplatter(CompDreadmeld.BloodFilthCountRange.RandomInRange, this.parent.PositionHeld, this.parent.MapHeld, FleshbeastUtility.MeatExplosionSize.Normal);
				FilthMaker.TryMakeFilth(this.parent.PositionHeld, this.parent.MapHeld, ThingDefOf.Filth_TwistedFlesh, 1, FilthSourceFlags.None, true);
			}
		}

		// Token: 0x0600FEDB RID: 65243 RVA: 0x0049DDF4 File Offset: 0x0049BFF4
		private void SpawnPawn(Pawn pawn, IntVec3 position, Map map)
		{
			GenSpawn.Spawn(pawn, position, map, WipeMode.VanishOrMoveAside);
			FleshbeastUtility.SpawnPawnAsFlyer(pawn, map, position, 5, true);
		}

		// Token: 0x0600FEDC RID: 65244 RVA: 0x0049DE0C File Offset: 0x0049C00C
		public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
		{
			if (!ModLister.CheckAnomaly("Dreadmeld"))
			{
				return;
			}
			UndercaveMapComponent undercaveMapComponent = (prevMap != null) ? prevMap.GetComponent<UndercaveMapComponent>() : null;
			if (undercaveMapComponent == null)
			{
				return;
			}
			Find.LetterStack.ReceiveLetter(LetterMaker.MakeLetter("LetterLabelUndercaveCollapsing".Translate(), "LetterUndercaveCollapsing".Translate(), LetterDefOf.NeutralEvent, null, null), null, 0, true);
			PitGate pitGate = undercaveMapComponent.pitGate;
			if (pitGate == null)
			{
				return;
			}
			pitGate.BeginCollapsing();
		}

		// Token: 0x0600FEDD RID: 65245 RVA: 0x0049DE74 File Offset: 0x0049C074
		public override void PostExposeData()
		{
			Scribe_Values.Look<float>(ref this.totalDamageTaken, "totalDamageTaken", 0f, false);
		}

		// Token: 0x0400A6ED RID: 42733
		private static readonly IntRange FingerSpikeSpawnCountRange = new IntRange(1, 3);

		// Token: 0x0400A6EE RID: 42734
		private static readonly IntRange ThresholdSpawnPointsRange = new IntRange(100, 300);

		// Token: 0x0400A6EF RID: 42735
		private static readonly IntRange BloodFilthCountRange = new IntRange(1, 2);

		// Token: 0x0400A6F0 RID: 42736
		private const int InitialJumpDistance = 5;

		// Token: 0x0400A6F1 RID: 42737
		private const int DamageSpawnThresholds = 200;

		// Token: 0x0400A6F2 RID: 42738
		private float totalDamageTaken;
	}
}
