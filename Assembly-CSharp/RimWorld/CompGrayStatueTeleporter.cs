using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002EFB RID: 12027
	public class CompGrayStatueTeleporter : CompGrayStatue
	{
		// Token: 0x17002BDA RID: 11226
		// (get) Token: 0x06010FAC RID: 69548 RVA: 0x004E2D7B File Offset: 0x004E0F7B
		private CompProperties_GrayStatueTeleporter Props
		{
			get
			{
				return (CompProperties_GrayStatueTeleporter)this.props;
			}
		}

		// Token: 0x06010FAD RID: 69549 RVA: 0x004E2D88 File Offset: 0x004E0F88
		protected override void Trigger(Pawn target)
		{
			this.parent.MapHeld.GetComponent<LabyrinthMapComponent>().TeleportToLabyrinth(target);
		}
	}
}
