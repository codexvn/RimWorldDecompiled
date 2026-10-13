using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F8C RID: 3980
	public class MentalState_IdeoChange : MentalState
	{
		// Token: 0x06006231 RID: 25137 RVA: 0x001F03EC File Offset: 0x001EE5EC
		public override void PreStart()
		{
			base.PreStart();
			this.oldIdeo = this.pawn.Ideo;
			this.oldRole = this.oldIdeo.GetRole(this.pawn);
			this.newIdeo = Find.IdeoManager.IdeosListForReading.RandomElementByWeight((Ideo x) => IdeoUtility.IdeoChangeToWeight(this.pawn, x));
			if (this.pawn.ideo.IdeoConversionAttempt(0.5f, this.newIdeo, false))
			{
				this.changedIdeo = true;
			}
			this.newCertainty = this.pawn.ideo.Certainty;
		}

		// Token: 0x06006232 RID: 25138 RVA: 0x001F0484 File Offset: 0x001EE684
		public override TaggedString GetBeginLetterText()
		{
			TaggedString taggedString = this.def.beginLetter.Formatted(this.pawn.LabelShort, this.pawn.Named("PAWN"), this.pawn.Ideo.Named("NEWIDEO"), this.oldIdeo.Named("OLDIDEO")).CapitalizeFirst() + "\n\n";
			if (this.changedIdeo)
			{
				taggedString += "LetterIdeoChangeConverted".Translate(this.pawn.Named("PAWN"), this.newIdeo.Named("NEWIDEO"), this.oldIdeo.Named("OLDIDEO")).CapitalizeFirst();
			}
			else
			{
				taggedString += "LetterIdeoChangeNotConverted".Translate(this.pawn.Named("PAWN"), this.oldIdeo.Named("OLDIDEO"), this.newCertainty.ToStringPercent().Named("NEWCERTAINTY")).CapitalizeFirst();
			}
			MentalStateDef wanderToOwnRoomStateOrFallback = MentalStateUtility.GetWanderToOwnRoomStateOrFallback(this.pawn);
			if (wanderToOwnRoomStateOrFallback == MentalStateDefOf.Wander_OwnRoom)
			{
				taggedString += "\n\n" + "LetterIdeoChangedWanderOwnRoom".Translate(this.pawn.Named("PAWN"));
			}
			else if (wanderToOwnRoomStateOrFallback == MentalStateDefOf.Wander_Sad)
			{
				taggedString += "\n\n" + "LetterIdeoChangedSadWander".Translate(this.pawn.Named("PAWN"));
			}
			if (this.changedIdeo && this.oldRole != null)
			{
				taggedString += "\n\n" + "LetterRoleLostLetterIdeoChangedPostfix".Translate(this.pawn.Named("PAWN"), this.oldRole.Named("ROLE"), this.oldIdeo.Named("OLDIDEO"));
			}
			return taggedString;
		}

		// Token: 0x06006233 RID: 25139 RVA: 0x001F066A File Offset: 0x001EE86A
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			MentalStateUtility.TryTransitionToWanderOwnRoom(this);
		}

		// Token: 0x06006234 RID: 25140 RVA: 0x001F067C File Offset: 0x001EE87C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Ideo>(ref this.oldIdeo, "oldIdeo", false);
			Scribe_References.Look<Ideo>(ref this.newIdeo, "newIdeo", false);
			Scribe_References.Look<Precept_Role>(ref this.oldRole, "oldRole", false);
			Scribe_Values.Look<bool>(ref this.changedIdeo, "changedIdeo", false, false);
			Scribe_Values.Look<float>(ref this.newCertainty, "newCertainty", 0f, false);
		}

		// Token: 0x04004663 RID: 18019
		private Ideo oldIdeo;

		// Token: 0x04004664 RID: 18020
		private Ideo newIdeo;

		// Token: 0x04004665 RID: 18021
		private Precept_Role oldRole;

		// Token: 0x04004666 RID: 18022
		private bool changedIdeo;

		// Token: 0x04004667 RID: 18023
		private float newCertainty;

		// Token: 0x04004668 RID: 18024
		private const float ConversionCertaintyReduction = 0.5f;
	}
}
