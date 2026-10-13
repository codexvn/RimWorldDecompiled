using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse.AI
{
	// Token: 0x02000F88 RID: 3976
	public abstract class MentalState_BabyFit : MentalState
	{
		// Token: 0x06006220 RID: 25120
		protected abstract void AuraEffect(Thing source, Pawn hearer);

		// Token: 0x06006221 RID: 25121 RVA: 0x000E96BB File Offset: 0x000E78BB
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Normal;
		}

		// Token: 0x06006222 RID: 25122 RVA: 0x001EFDC4 File Offset: 0x001EDFC4
		public override void MentalStateTick(int delta)
		{
			base.MentalStateTick(delta);
			if ((float)Find.TickManager.TicksGame <= this.lastScreamTick + 150f)
			{
				return;
			}
			if (this.pawn.IsWorldPawn())
			{
				return;
			}
			Caravan caravan = this.pawn.GetCaravan();
			if (caravan != null)
			{
				using (List<Pawn>.Enumerator enumerator = caravan.PawnsListForReading.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Pawn hearer = enumerator.Current;
						this.DoPawnHear(this.pawn, hearer);
					}
					goto IL_96;
				}
			}
			GenClamor.DoClamor(this.pawn, 9.9f, new GenClamor.ClamorEffect(this.DoPawnHear));
			IL_96:
			this.lastScreamTick = (float)Find.TickManager.TicksGame;
		}

		// Token: 0x06006223 RID: 25123 RVA: 0x001EFE88 File Offset: 0x001EE088
		private void DoPawnHear(Thing source, Pawn hearer)
		{
			if (hearer == source)
			{
				return;
			}
			if (this.alreadyHeard.Contains(hearer))
			{
				return;
			}
			this.alreadyHeard.Add(hearer);
			this.AuraEffect(source, hearer);
		}

		// Token: 0x06006224 RID: 25124 RVA: 0x001EFEB2 File Offset: 0x001EE0B2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.lastScreamTick, "lastScreamTick", 0f, false);
			Scribe_Collections.Look<Pawn>(ref this.alreadyHeard, "alreadyHeard", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x04004655 RID: 18005
		public const float BabyScreamRadius = 9.9f;

		// Token: 0x04004656 RID: 18006
		public const int ScreamInterval = 150;

		// Token: 0x04004657 RID: 18007
		private float lastScreamTick = -1f;

		// Token: 0x04004658 RID: 18008
		private List<Pawn> alreadyHeard = new List<Pawn>(32);
	}
}
