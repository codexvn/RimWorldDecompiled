using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200342E RID: 13358
	public class PawnColumnWorker_FollowDrafted : PawnColumnWorker_Checkbox
	{
		// Token: 0x17003135 RID: 12597
		// (get) Token: 0x06012CBB RID: 76987 RVA: 0x00594802 File Offset: 0x00592A02
		public override bool VisibleCurrently
		{
			get
			{
				return this.anyAnimalWithObedience;
			}
		}

		// Token: 0x06012CBC RID: 76988 RVA: 0x0059480C File Offset: 0x00592A0C
		public override void Recache()
		{
			this.anyAnimalWithObedience = false;
			foreach (Pawn pawn in Find.CurrentMap.mapPawns.ColonyAnimals)
			{
				Pawn_TrainingTracker training = pawn.training;
				if (training != null && training.HasLearned(TrainableDefOf.Obedience))
				{
					this.anyAnimalWithObedience = true;
					break;
				}
			}
		}

		// Token: 0x06012CBD RID: 76989 RVA: 0x00594888 File Offset: 0x00592A88
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer && pawn.training.HasLearned(TrainableDefOf.Obedience);
		}

		// Token: 0x06012CBE RID: 76990 RVA: 0x005948B6 File Offset: 0x00592AB6
		protected override bool GetValue(Pawn pawn)
		{
			return pawn.playerSettings.followDrafted;
		}

		// Token: 0x06012CBF RID: 76991 RVA: 0x005948C3 File Offset: 0x00592AC3
		protected override void SetValue(Pawn pawn, bool value, PawnTable table)
		{
			pawn.playerSettings.followDrafted = value;
		}

		// Token: 0x0400C349 RID: 49993
		private bool anyAnimalWithObedience;
	}
}
