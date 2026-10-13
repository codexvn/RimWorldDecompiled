using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D8B RID: 11659
	public class CompObelisk_Labyrinth : CompInteractable
	{
		// Token: 0x170029C0 RID: 10688
		// (get) Token: 0x06010646 RID: 67142 RVA: 0x004BADC2 File Offset: 0x004B8FC2
		public new CompProperties_ObeliskLabyrinth Props
		{
			get
			{
				return (CompProperties_ObeliskLabyrinth)this.props;
			}
		}

		// Token: 0x06010647 RID: 67143 RVA: 0x004BADCF File Offset: 0x004B8FCF
		protected override void OnInteracted(Pawn caster)
		{
			Messages.Message(this.Props.messageActivating, this.parent, MessageTypeDefOf.NeutralEvent, false);
			this.parent.Map.GetComponent<LabyrinthMapComponent>().StartClosing();
		}
	}
}
