using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200371F RID: 14111
	public class StatPart_RoleConversionPower : StatPart
	{
		// Token: 0x060138FA RID: 80122 RVA: 0x005CCF78 File Offset: 0x005CB178
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.Ideo != null)
				{
					Precept_Role role = pawn.Ideo.GetRole(pawn);
					if (role != null)
					{
						val *= role.def.convertPowerFactor;
					}
				}
			}
		}

		// Token: 0x060138FB RID: 80123 RVA: 0x005CCFC8 File Offset: 0x005CB1C8
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.Ideo != null)
				{
					Precept_Role role = pawn.Ideo.GetRole(pawn);
					if (role != null)
					{
						return "AbilityIdeoConvertBreakdownRole".Translate((req.Thing as Pawn).Named("PAWN"), role.Named("ROLE")) + ": " + role.def.convertPowerFactor.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor);
					}
				}
			}
			return null;
		}
	}
}
