using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C64 RID: 11364
	public class CompDisruptorFlarePack : CompAIUsablePack
	{
		// Token: 0x17002828 RID: 10280
		// (get) Token: 0x0600FED2 RID: 65234 RVA: 0x0049DAE7 File Offset: 0x0049BCE7
		public new CompProperties_DisruptorFlarePack Props
		{
			get
			{
				return (CompProperties_DisruptorFlarePack)this.props;
			}
		}

		// Token: 0x0600FED3 RID: 65235 RVA: 0x00006AAC File Offset: 0x00004CAC
		protected override float ChanceToUse(Pawn wearer)
		{
			return 0f;
		}

		// Token: 0x0600FED4 RID: 65236 RVA: 0x00066B11 File Offset: 0x00064D11
		protected override void UsePack(Pawn wearer)
		{
			throw new NotImplementedException();
		}
	}
}
