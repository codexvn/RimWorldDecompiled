using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D19 RID: 11545
	public class CompLeaveFilthOnDestroyed : ThingComp
	{
		// Token: 0x17002934 RID: 10548
		// (get) Token: 0x06010399 RID: 66457 RVA: 0x004B1478 File Offset: 0x004AF678
		public CompProperties_LeaveFilthOnDestroyed Props
		{
			get
			{
				return (CompProperties_LeaveFilthOnDestroyed)this.props;
			}
		}

		// Token: 0x17002935 RID: 10549
		// (get) Token: 0x0601039A RID: 66458 RVA: 0x004B1485 File Offset: 0x004AF685
		private float Radius
		{
			get
			{
				return (float)this.parent.def.size.x / 2f;
			}
		}

		// Token: 0x0601039B RID: 66459 RVA: 0x004B14A4 File Offset: 0x004AF6A4
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			if (mode == DestroyMode.Vanish)
			{
				return;
			}
			foreach (IntVec3 b in GenRadial.RadialPatternInRadius(this.Radius - 0.1f))
			{
				FilthMaker.TryMakeFilth(this.parent.Position + b, previousMap, this.Props.filthDef, this.Props.thickness, FilthSourceFlags.None, true);
			}
		}
	}
}
