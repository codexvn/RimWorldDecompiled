using System;
using System.Text;

namespace Verse
{
	// Token: 0x02000723 RID: 1827
	public abstract class HediffComp_SeverityModifierBase : HediffComp
	{
		// Token: 0x06002C81 RID: 11393
		public abstract float SeverityChangePerDay();

		// Token: 0x06002C82 RID: 11394 RVA: 0x000E6A68 File Offset: 0x000E4C68
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			if (base.Pawn.IsHashIntervalTick(200, delta))
			{
				float num = this.SeverityChangePerDay();
				num *= 0.0033333334f;
				severityAdjustment += num;
			}
		}

		// Token: 0x06002C83 RID: 11395 RVA: 0x000E6AA0 File Offset: 0x000E4CA0
		public override string CompDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.CompDebugString());
			if (!base.Pawn.Dead)
			{
				stringBuilder.AppendLine("severity/day: " + this.SeverityChangePerDay().ToString("F3"));
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x040023AA RID: 9130
		protected const int SeverityUpdateInterval = 200;
	}
}
