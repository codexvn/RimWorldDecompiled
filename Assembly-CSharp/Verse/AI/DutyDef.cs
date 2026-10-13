using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02001045 RID: 4165
	public class DutyDef : Def
	{
		// Token: 0x040048A2 RID: 18594
		public ThinkNode thinkNode;

		// Token: 0x040048A3 RID: 18595
		public ThinkNode constantThinkNode;

		// Token: 0x040048A4 RID: 18596
		public bool alwaysShowWeapon;

		// Token: 0x040048A5 RID: 18597
		public ThinkTreeDutyHook hook = ThinkTreeDutyHook.HighPriority;

		// Token: 0x040048A6 RID: 18598
		public RandomSocialMode socialModeMax = RandomSocialMode.SuperActive;

		// Token: 0x040048A7 RID: 18599
		public bool threatDisabled;

		// Token: 0x040048A8 RID: 18600
		public bool ritualSpectateTarget;

		// Token: 0x040048A9 RID: 18601
		public bool forceFaceUpPosture;

		// Token: 0x040048AA RID: 18602
		public bool? drawBodyOverride;
	}
}
