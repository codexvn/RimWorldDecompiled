using System;

namespace RimWorld
{
	// Token: 0x02001D4D RID: 7501
	public class GameCondition_SmokeSpewer : GameCondition_VolcanicWinter
	{
		// Token: 0x170019F9 RID: 6649
		// (get) Token: 0x0600A811 RID: 43025 RVA: 0x00301765 File Offset: 0x002FF965
		public override int TransitionTicks
		{
			get
			{
				return 5000;
			}
		}
	}
}
