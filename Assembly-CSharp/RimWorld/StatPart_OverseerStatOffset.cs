using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003714 RID: 14100
	public class StatPart_OverseerStatOffset : StatPart
	{
		// Token: 0x060138CB RID: 80075 RVA: 0x005CC36C File Offset: 0x005CA56C
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetOffset(req, out num))
			{
				val += num;
			}
		}

		// Token: 0x060138CC RID: 80076 RVA: 0x005CC38C File Offset: 0x005CA58C
		public override string ExplanationPart(StatRequest req)
		{
			float num;
			if (this.TryGetOffset(req, out num) && num != 0f)
			{
				return this.label + ": +" + num.ToStringPercent();
			}
			return null;
		}

		// Token: 0x060138CD RID: 80077 RVA: 0x005CC3C4 File Offset: 0x005CA5C4
		private bool TryGetOffset(StatRequest req, out float offset)
		{
			if (ModsConfig.BiotechActive && req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					Pawn overseer = pawn.GetOverseer();
					if (overseer != null)
					{
						offset = overseer.GetStatValue(this.stat, true, -1);
						return true;
					}
				}
			}
			offset = 0f;
			return false;
		}

		// Token: 0x0400D8DC RID: 55516
		private StatDef stat;

		// Token: 0x0400D8DD RID: 55517
		[MustTranslate]
		private string label;
	}
}
