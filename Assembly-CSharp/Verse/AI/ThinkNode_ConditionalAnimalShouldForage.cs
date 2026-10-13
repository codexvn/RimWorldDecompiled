using System;

namespace Verse.AI
{
	// Token: 0x02000FF6 RID: 4086
	public class ThinkNode_ConditionalAnimalShouldForage : ThinkNode_Conditional
	{
		// Token: 0x0600647A RID: 25722 RVA: 0x001FD62D File Offset: 0x001FB82D
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.playerSettings.animalForage;
		}
	}
}
