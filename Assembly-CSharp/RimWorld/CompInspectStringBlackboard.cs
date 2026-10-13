using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CF3 RID: 11507
	public class CompInspectStringBlackboard : CompInspectString
	{
		// Token: 0x060102A2 RID: 66210 RVA: 0x004AD444 File Offset: 0x004AB644
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.Props.inspectString);
			stringBuilder.Append(": +");
			stringBuilder.Append(0.2f.ToStringPercent());
			return stringBuilder.ToString();
		}
	}
}
