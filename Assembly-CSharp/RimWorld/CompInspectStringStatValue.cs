using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CF7 RID: 11511
	public class CompInspectStringStatValue : CompInspectString
	{
		// Token: 0x170028F2 RID: 10482
		// (get) Token: 0x060102AA RID: 66218 RVA: 0x004AD540 File Offset: 0x004AB740
		private new CompProperties_InspectStringStatValue Props
		{
			get
			{
				return (CompProperties_InspectStringStatValue)this.props;
			}
		}

		// Token: 0x060102AB RID: 66219 RVA: 0x004AD550 File Offset: 0x004AB750
		public override string CompInspectStringExtra()
		{
			if (this.Props.stat == null || this.Props.stat.Worker.IsDisabledFor(this.parent))
			{
				return null;
			}
			return this.Props.inspectString.Formatted(this.Props.stat.LabelCap, this.Props.stat.Worker.ValueToString(this.parent.GetStatValue(this.Props.stat, true, -1), true, this.Props.numberSense ?? this.Props.stat.toStringNumberSense));
		}
	}
}
