using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CF1 RID: 11505
	public class CompInspectString : ThingComp
	{
		// Token: 0x170028F1 RID: 10481
		// (get) Token: 0x0601029D RID: 66205 RVA: 0x004AD393 File Offset: 0x004AB593
		public CompProperties_InspectString Props
		{
			get
			{
				return (CompProperties_InspectString)this.props;
			}
		}

		// Token: 0x0601029E RID: 66206 RVA: 0x004AD3A0 File Offset: 0x004AB5A0
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + this.Props.inspectString.Formatted(this.parent.Named("PARENT")).Resolve();
		}
	}
}
