using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002492 RID: 9362
	public class Need_Sadism : Need
	{
		// Token: 0x17001EEB RID: 7915
		// (get) Token: 0x0600CB04 RID: 51972 RVA: 0x003B7A82 File Offset: 0x003B5C82
		public bool IsHigh
		{
			get
			{
				return this.CurLevel < 0.3f;
			}
		}

		// Token: 0x17001EEC RID: 7916
		// (get) Token: 0x0600CB05 RID: 51973 RVA: 0x003B7A91 File Offset: 0x003B5C91
		public bool IsCritical
		{
			get
			{
				return this.CurLevel < 0.1f;
			}
		}

		// Token: 0x0600CB06 RID: 51974 RVA: 0x003B5049 File Offset: 0x003B3249
		public Need_Sadism(Pawn newPawn) : base(newPawn)
		{
		}

		// Token: 0x0600CB07 RID: 51975 RVA: 0x003B7AA0 File Offset: 0x003B5CA0
		public override void NeedInterval()
		{
			if (this.IsFrozen)
			{
				return;
			}
			this.CurLevel -= this.def.fallPerDay * 0.0025f;
		}
	}
}
