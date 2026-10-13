using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000728 RID: 1832
	public class HediffComp_SurgeryInspectable : HediffComp
	{
		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x06002C94 RID: 11412 RVA: 0x000E6E47 File Offset: 0x000E5047
		public HediffCompProperties_SurgeryInspectable Props
		{
			get
			{
				return (HediffCompProperties_SurgeryInspectable)this.props;
			}
		}

		// Token: 0x06002C95 RID: 11413 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual SurgicalInspectionOutcome DoSurgicalInspection(Pawn surgeon)
		{
			return SurgicalInspectionOutcome.Detected;
		}

		// Token: 0x06002C96 RID: 11414 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void DoSurgicalInspectionVisible(Pawn surgeon)
		{
		}
	}
}
