using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020035D5 RID: 13781
	[DefOf]
	public static class MessageTypeDefOf
	{
		// Token: 0x060133C3 RID: 78787 RVA: 0x005B5E7E File Offset: 0x005B407E
		static MessageTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MessageTypeDefOf));
		}

		// Token: 0x0400D33F RID: 54079
		public static MessageTypeDef ThreatBig;

		// Token: 0x0400D340 RID: 54080
		public static MessageTypeDef ThreatSmall;

		// Token: 0x0400D341 RID: 54081
		public static MessageTypeDef PawnDeath;

		// Token: 0x0400D342 RID: 54082
		public static MessageTypeDef NegativeHealthEvent;

		// Token: 0x0400D343 RID: 54083
		public static MessageTypeDef NegativeEvent;

		// Token: 0x0400D344 RID: 54084
		public static MessageTypeDef NeutralEvent;

		// Token: 0x0400D345 RID: 54085
		public static MessageTypeDef TaskCompletion;

		// Token: 0x0400D346 RID: 54086
		public static MessageTypeDef PositiveEvent;

		// Token: 0x0400D347 RID: 54087
		public static MessageTypeDef SituationResolved;

		// Token: 0x0400D348 RID: 54088
		public static MessageTypeDef RejectInput;

		// Token: 0x0400D349 RID: 54089
		public static MessageTypeDef CautionInput;

		// Token: 0x0400D34A RID: 54090
		public static MessageTypeDef SilentInput;
	}
}
