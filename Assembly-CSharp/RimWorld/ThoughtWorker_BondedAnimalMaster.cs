using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001853 RID: 6227
	public class ThoughtWorker_BondedAnimalMaster : ThoughtWorker
	{
		// Token: 0x06008F88 RID: 36744 RVA: 0x002A636C File Offset: 0x002A456C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			ThoughtWorker_BondedAnimalMaster.tmpAnimals.Clear();
			this.GetAnimals(p, ThoughtWorker_BondedAnimalMaster.tmpAnimals);
			if (!ThoughtWorker_BondedAnimalMaster.tmpAnimals.Any<string>())
			{
				return false;
			}
			if (ThoughtWorker_BondedAnimalMaster.tmpAnimals.Count == 1)
			{
				return ThoughtState.ActiveAtStage(0, ThoughtWorker_BondedAnimalMaster.tmpAnimals[0]);
			}
			return ThoughtState.ActiveAtStage(1, ThoughtWorker_BondedAnimalMaster.tmpAnimals.ToCommaList(true, false));
		}

		// Token: 0x06008F89 RID: 36745 RVA: 0x002A63D3 File Offset: 0x002A45D3
		protected virtual bool AnimalMasterCheck(Pawn p, Pawn animal)
		{
			return animal.playerSettings.RespectedMaster == p;
		}

		// Token: 0x06008F8A RID: 36746 RVA: 0x002A63E4 File Offset: 0x002A45E4
		public void GetAnimals(Pawn p, List<string> outAnimals)
		{
			outAnimals.Clear();
			List<DirectPawnRelation> directRelations = p.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				DirectPawnRelation directPawnRelation = directRelations[i];
				Pawn otherPawn = directPawnRelation.otherPawn;
				if (directPawnRelation.def == PawnRelationDefOf.Bond && !otherPawn.Dead && otherPawn.Spawned && otherPawn.Faction == Faction.OfPlayer && otherPawn.training.HasLearned(TrainableDefOf.Obedience) && this.AnimalMasterCheck(p, otherPawn))
				{
					outAnimals.Add(otherPawn.LabelShort);
				}
			}
		}

		// Token: 0x06008F8B RID: 36747 RVA: 0x002A6473 File Offset: 0x002A4673
		public int GetAnimalsCount(Pawn p)
		{
			ThoughtWorker_BondedAnimalMaster.tmpAnimals.Clear();
			this.GetAnimals(p, ThoughtWorker_BondedAnimalMaster.tmpAnimals);
			return ThoughtWorker_BondedAnimalMaster.tmpAnimals.Count;
		}

		// Token: 0x04005C30 RID: 23600
		private static List<string> tmpAnimals = new List<string>();
	}
}
