using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200342A RID: 13354
	public class PawnColumnWorker_AnimalForage : PawnColumnWorker_Checkbox
	{
		// Token: 0x17003133 RID: 12595
		// (get) Token: 0x06012C9D RID: 76957 RVA: 0x005943C2 File Offset: 0x005925C2
		public override bool VisibleCurrently
		{
			get
			{
				return this.anyAnimalWithForage;
			}
		}

		// Token: 0x06012C9E RID: 76958 RVA: 0x005943CC File Offset: 0x005925CC
		public override void Recache()
		{
			this.anyAnimalWithForage = false;
			foreach (Pawn pawn in Find.CurrentMap.mapPawns.ColonyAnimals)
			{
				Pawn_TrainingTracker training = pawn.training;
				if (training != null && training.HasLearned(TrainableDefOf.Forage))
				{
					this.anyAnimalWithForage = true;
					break;
				}
			}
		}

		// Token: 0x06012C9F RID: 76959 RVA: 0x001FD62D File Offset: 0x001FB82D
		protected override bool GetValue(Pawn pawn)
		{
			return pawn.playerSettings.animalForage;
		}

		// Token: 0x06012CA0 RID: 76960 RVA: 0x00594448 File Offset: 0x00592648
		protected override void SetValue(Pawn pawn, bool value, PawnTable table)
		{
			pawn.playerSettings.animalForage = value;
		}

		// Token: 0x06012CA1 RID: 76961 RVA: 0x00594456 File Offset: 0x00592656
		protected override bool HasCheckbox(Pawn pawn)
		{
			Pawn_TrainingTracker training = pawn.training;
			return training != null && training.HasLearned(TrainableDefOf.Forage);
		}

		// Token: 0x0400C345 RID: 49989
		private bool anyAnimalWithForage;
	}
}
