using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002378 RID: 9080
	public class PawnGroupKindWorker_Shamblers : PawnGroupKindWorker_Normal
	{
		// Token: 0x0600C4D4 RID: 50388 RVA: 0x003941D0 File Offset: 0x003923D0
		protected override void GeneratePawns(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, List<Pawn> outPawns, bool errorOnZeroResults = true)
		{
			if (!this.CanGenerateFrom(parms, groupMaker))
			{
				if (errorOnZeroResults)
				{
					string[] array = new string[5];
					array[0] = "Cannot generate pawns for ";
					int num = 1;
					Faction faction = parms.faction;
					array[num] = ((faction != null) ? faction.ToString() : null);
					array[2] = " with ";
					array[3] = parms.points.ToString();
					array[4] = ". Defaulting to a single random cheap group.";
					Log.Error(string.Concat(array));
				}
				return;
			}
			float num2 = parms.points;
			float num3 = groupMaker.options.Min((PawnGenOption opt) => opt.Cost);
			int num4 = 0;
			while (num2 > num3 && num4 < 200)
			{
				num4++;
				PawnGenOption pawnGenOption;
				groupMaker.options.TryRandomElementByWeight((PawnGenOption gr) => gr.selectionWeight, out pawnGenOption);
				if (pawnGenOption.Cost <= num2)
				{
					num2 -= pawnGenOption.Cost;
					DevelopmentalStage developmentalStage = DevelopmentalStage.Adult;
					if (Find.Storyteller.difficulty.ChildrenAllowed && Find.Storyteller.difficulty.childShamblersAllowed)
					{
						developmentalStage |= DevelopmentalStage.Child;
					}
					PawnKindDef kind = pawnGenOption.kind;
					Faction faction2 = parms.faction;
					PawnGenerationContext context = PawnGenerationContext.NonPlayer;
					DevelopmentalStage developmentalStages = developmentalStage;
					FloatRange? excludeBiologicalAgeRange = new FloatRange?(PawnGroupKindWorker_Shamblers.ChildrenDisabledExcludedAgeRange);
					Pawn item = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind, faction2, context, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, developmentalStages, null, excludeBiologicalAgeRange, null, false, false, false, -1, 0, false));
					outPawns.Add(item);
				}
			}
		}

		// Token: 0x04008643 RID: 34371
		private static readonly FloatRange ChildrenDisabledExcludedAgeRange = new FloatRange(0f, 8f);
	}
}
