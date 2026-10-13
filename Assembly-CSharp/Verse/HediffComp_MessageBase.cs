using System;

namespace Verse
{
	// Token: 0x020006E4 RID: 1764
	public class HediffComp_MessageBase : HediffComp
	{
		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x06002BA0 RID: 11168 RVA: 0x000E3B98 File Offset: 0x000E1D98
		private HediffCompProperties_MessageBase Props
		{
			get
			{
				return (HediffCompProperties_MessageBase)this.props;
			}
		}

		// Token: 0x06002BA1 RID: 11169 RVA: 0x000E3BA8 File Offset: 0x000E1DA8
		protected virtual void Message()
		{
			if (this.Props.onlyMessageForColonistsOrPrisoners && !base.Pawn.IsColonist && !base.Pawn.IsPrisonerOfColony)
			{
				return;
			}
			Messages.Message(this.Props.message.Formatted(base.Pawn), base.Pawn, this.Props.messageType, true);
		}
	}
}
