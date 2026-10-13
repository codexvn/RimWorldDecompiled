using System;

namespace RimWorld
{
	// Token: 0x02001963 RID: 6499
	public class CompHackableEffecter : CompEffecter
	{
		// Token: 0x060093A4 RID: 37796 RVA: 0x002B6E86 File Offset: 0x002B5086
		protected override bool ShouldShowEffecter()
		{
			if (!base.ShouldShowEffecter())
			{
				return false;
			}
			if (this.hackable == null)
			{
				this.hackable = this.parent.GetComp<CompHackable>();
			}
			CompHackable compHackable = this.hackable;
			return compHackable == null || compHackable.ShouldBeLitNow();
		}

		// Token: 0x04005EFF RID: 24319
		private CompHackable hackable;
	}
}
