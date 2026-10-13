using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DE3 RID: 7651
	public class IncidentWorker_WandererJoin : IncidentWorker
	{
		// Token: 0x0600AA8B RID: 43659 RVA: 0x0030C4B4 File Offset: 0x0030A6B4
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			return this.CanSpawnJoiner(map);
		}

		// Token: 0x0600AA8C RID: 43660 RVA: 0x0030C4E0 File Offset: 0x0030A6E0
		public virtual Pawn GeneratePawn()
		{
			Gender? gender = null;
			if (this.def.pawnFixedGender != Gender.None)
			{
				gender = new Gender?(this.def.pawnFixedGender);
			}
			Ideo ideo = null;
			if (ModsConfig.IdeologyActive)
			{
				if (!(from i in Find.IdeoManager.IdeosListForReading
				where !Faction.OfPlayer.ideos.Has(i)
				select i).TryRandomElementByWeight((Ideo x) => IdeoUtility.IdeoChangeToWeight(null, x), out ideo))
				{
					(from i in Find.IdeoManager.IdeosListForReading
					where !Faction.OfPlayer.ideos.IsPrimary(i)
					select i).TryRandomElementByWeight((Ideo x) => IdeoUtility.IdeoChangeToWeight(null, x), out ideo);
				}
			}
			PawnKindDef pawnKind = this.def.pawnKind;
			Faction ofPlayer = Faction.OfPlayer;
			PawnGenerationContext context = PawnGenerationContext.NonPlayer;
			bool pawnMustBeCapableOfViolence = this.def.pawnMustBeCapableOfViolence;
			Gender? fixedGender = gender;
			Ideo fixedIdeo = ideo;
			return PawnGenerator.GeneratePawn(new PawnGenerationRequest(pawnKind, ofPlayer, context, null, true, false, false, true, pawnMustBeCapableOfViolence, 20f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, fixedGender, null, null, null, fixedIdeo, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
		}

		// Token: 0x0600AA8D RID: 43661 RVA: 0x0030C678 File Offset: 0x0030A878
		public virtual bool CanSpawnJoiner(Map map)
		{
			IntVec3 intVec;
			return this.TryFindEntryCell(map, out intVec);
		}

		// Token: 0x0600AA8E RID: 43662 RVA: 0x0030C690 File Offset: 0x0030A890
		public virtual void SpawnJoiner(Map map, Pawn pawn)
		{
			IntVec3 loc;
			this.TryFindEntryCell(map, out loc);
			GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
		}

		// Token: 0x0600AA8F RID: 43663 RVA: 0x0030C6B4 File Offset: 0x0030A8B4
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (!this.CanSpawnJoiner(map))
			{
				return false;
			}
			Pawn pawn = this.GeneratePawn();
			this.SpawnJoiner(map, pawn);
			if (this.def.pawnHediff != null)
			{
				pawn.health.AddHediff(this.def.pawnHediff, null, null, null);
			}
			TaggedString baseLetterText = (this.def.pawnHediff != null) ? this.def.letterText.Formatted(pawn.Named("PAWN"), this.def.pawnHediff.Named("HEDIFF")).AdjustedFor(pawn, "PAWN", true) : this.def.letterText.Formatted(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
			TaggedString baseLetterLabel = this.def.letterLabel.Formatted(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
			PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref baseLetterText, ref baseLetterLabel, pawn);
			base.SendStandardLetter(baseLetterLabel, baseLetterText, LetterDefOf.PositiveEvent, parms, pawn, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x0600AA90 RID: 43664 RVA: 0x0030C7E4 File Offset: 0x0030A9E4
		private bool TryFindEntryCell(Map map, out IntVec3 cell)
		{
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => map.reachability.CanReachColony(c) && !c.Fogged(map), map, CellFinder.EdgeRoadChance_Neutral, out cell);
		}

		// Token: 0x04007590 RID: 30096
		private const float RelationWithColonistWeight = 20f;
	}
}
