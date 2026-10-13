using System;

namespace RimWorld
{
	// Token: 0x020034C1 RID: 13505
	public class TransferableComparer_Quality : TransferableComparer
	{
		// Token: 0x06012FA7 RID: 77735 RVA: 0x005A1888 File Offset: 0x0059FA88
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return this.GetValueFor(lhs).CompareTo(this.GetValueFor(rhs));
		}

		// Token: 0x06012FA8 RID: 77736 RVA: 0x005A18AC File Offset: 0x0059FAAC
		private int GetValueFor(Transferable t)
		{
			QualityCategory result;
			if (!t.AnyThing.TryGetQuality(out result))
			{
				return -1;
			}
			return (int)result;
		}
	}
}
