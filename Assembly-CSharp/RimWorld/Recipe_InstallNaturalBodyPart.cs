using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002309 RID: 8969
	public class Recipe_InstallNaturalBodyPart : Recipe_Surgery
	{
		// Token: 0x0600C2D5 RID: 49877 RVA: 0x0038805C File Offset: 0x0038625C
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			return MedicalRecipesUtility.GetFixedPartsToApplyOn(recipe, pawn, (BodyPartRecord record) => pawn.health.hediffSet.hediffs.Any((Hediff x) => x.Part == record) && (record.parent == null || pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Contains(record.parent)) && (!pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(record) || pawn.health.hediffSet.HasDirectlyAddedPartFor(record)));
		}

		// Token: 0x0600C2D6 RID: 49878 RVA: 0x00388090 File Offset: 0x00386290
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (billDoer != null)
			{
				if (base.CheckSurgeryFail(billDoer, pawn, ingredients, part, bill))
				{
					return;
				}
				TaleRecorder.RecordTale(TaleDefOf.DidSurgery, new object[]
				{
					billDoer,
					pawn
				});
				Hediff directlyAddedPartFor = pawn.health.hediffSet.GetDirectlyAddedPartFor(part);
				MedicalRecipesUtility.RestorePartAndSpawnAllPreviousParts(pawn, part, billDoer.Position, billDoer.Map);
				if (ModsConfig.IdeologyActive)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.InstalledOrgan, billDoer.Named(HistoryEventArgsNames.Doer)), true);
				}
				if (directlyAddedPartFor != null)
				{
					directlyAddedPartFor.Notify_SurgicallyReplaced(billDoer);
				}
			}
		}
	}
}
