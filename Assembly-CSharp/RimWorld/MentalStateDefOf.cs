using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200359D RID: 13725
	[DefOf]
	public static class MentalStateDefOf
	{
		// Token: 0x0601338B RID: 78731 RVA: 0x005B5AC6 File Offset: 0x005B3CC6
		static MentalStateDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MentalStateDefOf));
		}

		// Token: 0x0400CE72 RID: 52850
		public static MentalStateDef Berserk;

		// Token: 0x0400CE73 RID: 52851
		public static MentalStateDef BerserkPermanent;

		// Token: 0x0400CE74 RID: 52852
		public static MentalStateDef BerserkMechanoid;

		// Token: 0x0400CE75 RID: 52853
		public static MentalStateDef CocoonDisturbed;

		// Token: 0x0400CE76 RID: 52854
		[MayRequireBiotech]
		public static MentalStateDef BerserkWarcall;

		// Token: 0x0400CE77 RID: 52855
		[MayRequireAnomaly]
		public static MentalStateDef HumanityBreak;

		// Token: 0x0400CE78 RID: 52856
		[MayRequireAnomaly]
		public static MentalStateDef EntityKiller;

		// Token: 0x0400CE79 RID: 52857
		public static MentalStateDef Wander_Psychotic;

		// Token: 0x0400CE7A RID: 52858
		public static MentalStateDef Wander_Sad;

		// Token: 0x0400CE7B RID: 52859
		public static MentalStateDef Wander_OwnRoom;

		// Token: 0x0400CE7C RID: 52860
		public static MentalStateDef PanicFlee;

		// Token: 0x0400CE7D RID: 52861
		public static MentalStateDef Manhunter;

		// Token: 0x0400CE7E RID: 52862
		public static MentalStateDef ManhunterPermanent;

		// Token: 0x0400CE7F RID: 52863
		public static MentalStateDef SocialFighting;

		// Token: 0x0400CE80 RID: 52864
		public static MentalStateDef Roaming;

		// Token: 0x0400CE81 RID: 52865
		[MayRequireIdeology]
		public static MentalStateDef Rebellion;

		// Token: 0x0400CE82 RID: 52866
		[MayRequireAnomaly]
		public static MentalStateDef ManhunterBloodRain;

		// Token: 0x0400CE83 RID: 52867
		[MayRequireAnomaly]
		public static MentalStateDef CubeSculpting;

		// Token: 0x0400CE84 RID: 52868
		[MayRequireOdyssey]
		public static MentalStateDef Terror;

		// Token: 0x0400CE85 RID: 52869
		[MayRequireBiotech]
		public static MentalStateDef PanicFleeFire;
	}
}
