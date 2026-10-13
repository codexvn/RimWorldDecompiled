using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D65 RID: 7525
	public class IncidentWorker_Ambush_ManhunterPack : IncidentWorker_Ambush
	{
		// Token: 0x0600A8AA RID: 43178 RVA: 0x00304104 File Offset: 0x00302304
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			PawnKindDef pawnKindDef;
			return base.CanFireNowSub(parms) && AggressiveAnimalIncidentUtility.TryFindAggressiveAnimalKind(this.AdjustedPoints(parms.points), PlanetTile.Invalid, out pawnKindDef);
		}

		// Token: 0x0600A8AB RID: 43179 RVA: 0x00304134 File Offset: 0x00302334
		protected override List<Pawn> GeneratePawns(IncidentParms parms)
		{
			PawnKindDef animalKind;
			if (!AggressiveAnimalIncidentUtility.TryFindAggressiveAnimalKind(this.AdjustedPoints(parms.points), parms.target.Tile, out animalKind) && !AggressiveAnimalIncidentUtility.TryFindAggressiveAnimalKind(this.AdjustedPoints(parms.points), PlanetTile.Invalid, out animalKind))
			{
				string str = "Could not find any valid animal kind for ";
				IncidentDef def = this.def;
				Log.Error(str + ((def != null) ? def.ToString() : null) + " incident.");
				return new List<Pawn>();
			}
			return AggressiveAnimalIncidentUtility.GenerateAnimals(animalKind, parms.target.Tile, this.AdjustedPoints(parms.points), 0);
		}

		// Token: 0x0600A8AC RID: 43180 RVA: 0x003041C8 File Offset: 0x003023C8
		protected override void PostProcessGeneratedPawnsAfterSpawning(List<Pawn> generatedPawns)
		{
			for (int i = 0; i < generatedPawns.Count; i++)
			{
				generatedPawns[i].health.AddHediff(HediffDefOf.Scaria, null, null, null);
				generatedPawns[i].mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent, null, false, false, false, null, false, false, false);
			}
		}

		// Token: 0x0600A8AD RID: 43181 RVA: 0x0030422C File Offset: 0x0030242C
		private float AdjustedPoints(float basePoints)
		{
			return basePoints * 0.75f;
		}

		// Token: 0x0600A8AE RID: 43182 RVA: 0x00304238 File Offset: 0x00302438
		protected override string GetLetterText(Pawn anyPawn, IncidentParms parms)
		{
			Caravan caravan = parms.target as Caravan;
			return this.def.letterText.Formatted((caravan != null) ? caravan.Name : "yourCaravan".TranslateSimple(), anyPawn.GetKindLabelPlural(-1)).CapitalizeFirst();
		}

		// Token: 0x040074BF RID: 29887
		private const float ManhunterAmbushPointsFactor = 0.75f;
	}
}
