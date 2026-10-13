using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002886 RID: 10374
	public static class PawnGenerationContextUtility
	{
		// Token: 0x0600E2FE RID: 58110 RVA: 0x00426D0C File Offset: 0x00424F0C
		public static string ToStringHuman(this PawnGenerationContext context)
		{
			switch (context)
			{
			case PawnGenerationContext.All:
				return "PawnGenerationContext_All".Translate();
			case PawnGenerationContext.PlayerStarter:
				return "PawnGenerationContext_PlayerStarter".Translate();
			case PawnGenerationContext.NonPlayer:
				return "PawnGenerationContext_NonPlayer".Translate();
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x0600E2FF RID: 58111 RVA: 0x00426D62 File Offset: 0x00424F62
		public static bool Includes(this PawnGenerationContext includer, PawnGenerationContext other)
		{
			return includer == PawnGenerationContext.All || includer == other;
		}

		// Token: 0x0600E300 RID: 58112 RVA: 0x00426D70 File Offset: 0x00424F70
		public static PawnGenerationContext GetRandom()
		{
			Array values = Enum.GetValues(typeof(PawnGenerationContext));
			return (PawnGenerationContext)values.GetValue(Rand.Range(0, values.Length));
		}

		// Token: 0x0600E301 RID: 58113 RVA: 0x00426DA4 File Offset: 0x00424FA4
		public static bool OverlapsWith(this PawnGenerationContext a, PawnGenerationContext b)
		{
			return a == PawnGenerationContext.All || b == PawnGenerationContext.All || a == b;
		}
	}
}
