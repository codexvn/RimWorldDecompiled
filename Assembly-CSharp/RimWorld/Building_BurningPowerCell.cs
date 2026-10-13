using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020029E3 RID: 10723
	public class Building_BurningPowerCell : Building
	{
		// Token: 0x0600EC96 RID: 60566 RVA: 0x0044B2C3 File Offset: 0x004494C3
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("PuncturedWillExplode".Translate());
			stringBuilder.AppendInNewLine(base.GetInspectString());
			return stringBuilder.ToString();
		}
	}
}
