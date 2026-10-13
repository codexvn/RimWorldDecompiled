using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003429 RID: 13353
	public class PawnColumnWorker_AnimalDig : PawnColumnWorker_Checkbox
	{
		// Token: 0x17003132 RID: 12594
		// (get) Token: 0x06012C97 RID: 76951 RVA: 0x0059430E File Offset: 0x0059250E
		public override bool VisibleCurrently
		{
			get
			{
				return this.anyAnimalWithDig;
			}
		}

		// Token: 0x06012C98 RID: 76952 RVA: 0x00594318 File Offset: 0x00592518
		public override void Recache()
		{
			this.anyAnimalWithDig = false;
			foreach (Pawn pawn in Find.CurrentMap.mapPawns.ColonyAnimals)
			{
				Pawn_TrainingTracker training = pawn.training;
				if (training != null && training.HasLearned(TrainableDefOf.Dig))
				{
					this.anyAnimalWithDig = true;
					break;
				}
			}
		}

		// Token: 0x06012C99 RID: 76953 RVA: 0x001FD620 File Offset: 0x001FB820
		protected override bool GetValue(Pawn pawn)
		{
			return pawn.playerSettings.animalDig;
		}

		// Token: 0x06012C9A RID: 76954 RVA: 0x00594394 File Offset: 0x00592594
		protected override void SetValue(Pawn pawn, bool value, PawnTable table)
		{
			pawn.playerSettings.animalDig = value;
		}

		// Token: 0x06012C9B RID: 76955 RVA: 0x005943A2 File Offset: 0x005925A2
		protected override bool HasCheckbox(Pawn pawn)
		{
			Pawn_TrainingTracker training = pawn.training;
			return training != null && training.HasLearned(TrainableDefOf.Dig);
		}

		// Token: 0x0400C344 RID: 49988
		private bool anyAnimalWithDig;
	}
}
