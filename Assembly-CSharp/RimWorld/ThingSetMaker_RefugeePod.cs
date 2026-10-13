using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B87 RID: 11143
	public class ThingSetMaker_RefugeePod : ThingSetMaker
	{
		// Token: 0x0600F92D RID: 63789 RVA: 0x00486FF8 File Offset: 0x004851F8
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.SpaceRefugee, DownedRefugeeQuestUtility.GetRandomFactionForRefugee(0.6f), PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 20f, false, true, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false);
			int num = 0;
			Pawn pawn = null;
			while (num < 10 && (pawn == null || !pawn.Downed))
			{
				num++;
				if (pawn != null)
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
				}
				pawn = PawnGenerator.GeneratePawn(request);
				HealthUtility.DamageUntilDowned(pawn, true, null, null, null);
			}
			outThings.Add(pawn);
		}

		// Token: 0x0600F92E RID: 63790 RVA: 0x004870E3 File Offset: 0x004852E3
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			yield return PawnKindDefOf.SpaceRefugee.race;
			yield break;
		}

		// Token: 0x0400A3DA RID: 41946
		private const float RelationWithColonistWeight = 20f;
	}
}
