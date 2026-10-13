using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200043E RID: 1086
	public static class ToxicUtility
	{
		// Token: 0x06001C15 RID: 7189 RVA: 0x0008D62C File Offset: 0x0008B82C
		public static void PawnToxicTickInterval(Pawn pawn, int delta)
		{
			if (!pawn.IsHashIntervalTick(3451, delta) || !pawn.Spawned)
			{
				return;
			}
			float num = pawn.Position.GetTerrain(pawn.Map).toxicBuildupFactor;
			if (ModsConfig.BiotechActive && pawn.Position.IsPolluted(pawn.Map))
			{
				num += 1f;
			}
			if (num > 0f)
			{
				ToxicUtility.DoPawnToxicDamage(pawn, num);
			}
		}

		// Token: 0x06001C16 RID: 7190 RVA: 0x0008D698 File Offset: 0x0008B898
		public static void DoAirbornePawnToxicDamage(Pawn p, float extraFactor = 1f)
		{
			if (p.Spawned && p.Position.Roofed(p.Map))
			{
				return;
			}
			ToxicUtility.DoPawnToxicDamage(p, extraFactor);
		}

		// Token: 0x06001C17 RID: 7191 RVA: 0x0008D6C0 File Offset: 0x0008B8C0
		public static void DoPawnToxicDamage(Pawn p, float extraFactor = 1f)
		{
			float num = 0.023006668f;
			num *= Mathf.Max(1f - p.GetStatValue(StatDefOf.ToxicResistance, true, -1), 0f);
			num *= Mathf.Max(1f - p.GetStatValue(StatDefOf.ToxicEnvironmentResistance, true, -1), 0f);
			num *= extraFactor;
			if (num != 0f)
			{
				float num2 = Mathf.Lerp(0.85f, 1.15f, Rand.ValueSeeded(p.thingIDNumber ^ 74374237));
				num *= num2;
				HealthUtility.AdjustSeverity(p, HediffDefOf.ToxicBuildup, num);
			}
		}

		// Token: 0x04001822 RID: 6178
		public const int CheckInterval = 3451;

		// Token: 0x04001823 RID: 6179
		private const float ToxicPerDay = 0.4f;
	}
}
