using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020021EF RID: 8687
	public class CompProperties_AbilityEffect : AbilityCompProperties
	{
		// Token: 0x040082B9 RID: 33465
		public int goodwillImpact;

		// Token: 0x040082BA RID: 33466
		public bool psychic;

		// Token: 0x040082BB RID: 33467
		public bool applicableToMechs = true;

		// Token: 0x040082BC RID: 33468
		public bool applyGoodwillImpactToLodgers = true;

		// Token: 0x040082BD RID: 33469
		public bool canTargetBosses = true;

		// Token: 0x040082BE RID: 33470
		public ClamorDef clamorType;

		// Token: 0x040082BF RID: 33471
		public int clamorRadius;

		// Token: 0x040082C0 RID: 33472
		public float screenShakeIntensity;

		// Token: 0x040082C1 RID: 33473
		public SoundDef sound;

		// Token: 0x040082C2 RID: 33474
		public SoundDef soundMale;

		// Token: 0x040082C3 RID: 33475
		public SoundDef soundFemale;

		// Token: 0x040082C4 RID: 33476
		public string customLetterLabel;

		// Token: 0x040082C5 RID: 33477
		public string customLetterText;

		// Token: 0x040082C6 RID: 33478
		public bool sendLetter = true;

		// Token: 0x040082C7 RID: 33479
		public string message;

		// Token: 0x040082C8 RID: 33480
		public MessageTypeDef messageType;

		// Token: 0x040082C9 RID: 33481
		public float weight = 1f;

		// Token: 0x040082CA RID: 33482
		public bool availableWhenTargetIsWounded = true;

		// Token: 0x040082CB RID: 33483
		public bool canTargetBaby = true;
	}
}
