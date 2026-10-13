using System;

namespace Verse
{
	// Token: 0x02000B82 RID: 2946
	public class Command_ActionWithLimitedUseCount : Command_Action
	{
		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x06004B7C RID: 19324 RVA: 0x00180818 File Offset: 0x0017EA18
		public override string TopRightLabel
		{
			get
			{
				return this.usesLeftGetter().ToString() + " / " + this.maxUsesGetter().ToString();
			}
		}

		// Token: 0x06004B7D RID: 19325 RVA: 0x00180855 File Offset: 0x0017EA55
		public void UpdateUsesLeft()
		{
			if (this.usesLeftGetter() == 0)
			{
				base.Disable("CommandNoUsesLeft".Translate());
			}
		}

		// Token: 0x040035CC RID: 13772
		public Func<int> usesLeftGetter;

		// Token: 0x040035CD RID: 13773
		public Func<int> maxUsesGetter;
	}
}
