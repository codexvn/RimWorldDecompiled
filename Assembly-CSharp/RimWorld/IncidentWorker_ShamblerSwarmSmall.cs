using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DCE RID: 7630
	public class IncidentWorker_ShamblerSwarmSmall : IncidentWorker_ShamblerSwarm
	{
		// Token: 0x17001A16 RID: 6678
		// (get) Token: 0x0600AA3D RID: 43581 RVA: 0x0030B24D File Offset: 0x0030944D
		protected override IntRange ShamblerLifespanTicksRange
		{
			get
			{
				return new IntRange(25000, 45000);
			}
		}

		// Token: 0x0600AA3E RID: 43582 RVA: 0x0030B260 File Offset: 0x00309460
		protected override List<Pawn> GenerateEntities(IncidentParms parms, float points)
		{
			int randomInRange = IncidentWorker_ShamblerSwarmSmall.NumShamblersToSpawn.RandomInRange;
			List<Pawn> list = new List<Pawn>();
			for (int i = 0; i < randomInRange; i++)
			{
				Pawn item = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.ShamblerSwarmer, Faction.OfEntities, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
				list.Add(item);
			}
			base.SetupShamblerHediffs(list, this.ShamblerLifespanTicksRange);
			return list;
		}

		// Token: 0x0600AA3F RID: 43583 RVA: 0x0030B348 File Offset: 0x00309548
		protected override void SendLetter(IncidentParms parms, List<Pawn> entities)
		{
			string letterLabel = this.def.letterLabel;
			TaggedString baseLetterText = this.def.letterText.Formatted(entities.Count);
			base.SendStandardLetter(letterLabel, baseLetterText, this.def.letterDef, parms, entities, Array.Empty<NamedArgument>());
		}

		// Token: 0x04007578 RID: 30072
		private static readonly IntRange NumShamblersToSpawn = new IntRange(2, 4);
	}
}
