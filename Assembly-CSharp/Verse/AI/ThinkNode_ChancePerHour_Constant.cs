using System;

namespace Verse.AI
{
	// Token: 0x02000FD7 RID: 4055
	public class ThinkNode_ChancePerHour_Constant : ThinkNode_ChancePerHour
	{
		// Token: 0x06006430 RID: 25648 RVA: 0x001FD08F File Offset: 0x001FB28F
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ChancePerHour_Constant thinkNode_ChancePerHour_Constant = (ThinkNode_ChancePerHour_Constant)base.DeepCopy(resolve);
			thinkNode_ChancePerHour_Constant.mtbHours = this.mtbHours;
			thinkNode_ChancePerHour_Constant.mtbDays = this.mtbDays;
			return thinkNode_ChancePerHour_Constant;
		}

		// Token: 0x06006431 RID: 25649 RVA: 0x001FD0B5 File Offset: 0x001FB2B5
		protected override float MtbHours(Pawn Pawn)
		{
			if (this.mtbDays > 0f)
			{
				return this.mtbDays * 24f;
			}
			return this.mtbHours;
		}

		// Token: 0x040047C9 RID: 18377
		private float mtbHours = -1f;

		// Token: 0x040047CA RID: 18378
		private float mtbDays = -1f;
	}
}
