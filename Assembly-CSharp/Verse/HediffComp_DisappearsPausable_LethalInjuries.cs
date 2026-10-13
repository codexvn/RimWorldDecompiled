using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020006A1 RID: 1697
	public class HediffComp_DisappearsPausable_LethalInjuries : HediffComp_DisappearsPausable
	{
		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x06002AB5 RID: 10933 RVA: 0x000E0506 File Offset: 0x000DE706
		protected override bool Paused
		{
			get
			{
				return SanguophageUtility.ShouldBeDeathrestingOrInComaInsteadOfDead(base.Pawn);
			}
		}

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x06002AB6 RID: 10934 RVA: 0x000E0513 File Offset: 0x000DE713
		public override string CompTipStringExtra
		{
			get
			{
				if (this.Paused)
				{
					return "PawnWillKeepRegeneratingLethalInjuries".Translate(base.Pawn.Named("PAWN")).Colorize(ColorLibrary.RedReadable);
				}
				return base.CompTipStringExtra;
			}
		}

		// Token: 0x06002AB7 RID: 10935 RVA: 0x000E0548 File Offset: 0x000DE748
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (base.Pawn.IsHashIntervalTick(120) && !this.Paused)
			{
				this.ticksToDisappear -= 120;
			}
		}

		// Token: 0x04002290 RID: 8848
		private const int PauseCheckInterval = 120;
	}
}
