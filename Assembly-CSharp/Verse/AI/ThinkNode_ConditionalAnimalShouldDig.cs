using System;

namespace Verse.AI
{
	// Token: 0x02000FF5 RID: 4085
	public class ThinkNode_ConditionalAnimalShouldDig : ThinkNode_Conditional
	{
		// Token: 0x06006478 RID: 25720 RVA: 0x001FD620 File Offset: 0x001FB820
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.playerSettings.animalDig;
		}
	}
}
