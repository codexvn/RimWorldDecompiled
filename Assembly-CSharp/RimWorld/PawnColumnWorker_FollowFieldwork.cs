using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200342F RID: 13359
	public class PawnColumnWorker_FollowFieldwork : PawnColumnWorker_Checkbox
	{
		// Token: 0x17003136 RID: 12598
		// (get) Token: 0x06012CC1 RID: 76993 RVA: 0x005948D1 File Offset: 0x00592AD1
		public override bool VisibleCurrently
		{
			get
			{
				return this.anyAnimalWithObedience;
			}
		}

		// Token: 0x06012CC2 RID: 76994 RVA: 0x005948DC File Offset: 0x00592ADC
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

		// Token: 0x06012CC3 RID: 76995 RVA: 0x00594888 File Offset: 0x00592A88
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer && pawn.training.HasLearned(TrainableDefOf.Obedience);
		}

		// Token: 0x06012CC4 RID: 76996 RVA: 0x00594958 File Offset: 0x00592B58
		protected override bool GetValue(Pawn pawn)
		{
			return pawn.playerSettings.followFieldwork;
		}

		// Token: 0x06012CC5 RID: 76997 RVA: 0x00594965 File Offset: 0x00592B65
		protected override void SetValue(Pawn pawn, bool value, PawnTable table)
		{
			pawn.playerSettings.followFieldwork = value;
		}

		// Token: 0x0400C34A RID: 49994
		private bool anyAnimalWithObedience;
	}
}
