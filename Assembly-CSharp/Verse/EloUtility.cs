using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CC6 RID: 3270
	public static class EloUtility
	{
		// Token: 0x06005388 RID: 21384 RVA: 0x001AEC72 File Offset: 0x001ACE72
		public static void Update(ref float teamA, ref float teamB, float expectedA, float scoreA, float kfactor = 32f)
		{
			teamA += kfactor * (scoreA - expectedA);
			teamB += kfactor * (expectedA - scoreA);
		}

		// Token: 0x06005389 RID: 21385 RVA: 0x001AEC8C File Offset: 0x001ACE8C
		public static float CalculateExpectation(float teamA, float teamB)
		{
			float num = Mathf.Pow(10f, teamA / 400f) + Mathf.Pow(10f, teamB / 400f);
			return Mathf.Pow(10f, teamA / 400f) / num;
		}

		// Token: 0x0600538A RID: 21386 RVA: 0x001AECD0 File Offset: 0x001ACED0
		public static float CalculateLinearScore(float teamRating, float referenceRating, float referenceScore)
		{
			return referenceScore * Mathf.Pow(10f, (teamRating - referenceRating) / 400f);
		}

		// Token: 0x0600538B RID: 21387 RVA: 0x001AECE7 File Offset: 0x001ACEE7
		public static float CalculateRating(float teamScore, float referenceRating, float referenceScore)
		{
			return referenceRating + Mathf.Log(teamScore / referenceScore, 10f) * 400f;
		}

		// Token: 0x04003C47 RID: 15431
		private const float TenFactorRating = 400f;
	}
}
