using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D56 RID: 7510
	public class IncidentCycleUtility
	{
		// Token: 0x17001A01 RID: 6657
		// (get) Token: 0x0600A856 RID: 43094 RVA: 0x00302CC8 File Offset: 0x00300EC8
		private static int QueueIntervalsPassed
		{
			get
			{
				return Find.TickManager.TicksSinceSettle / 1000;
			}
		}

		// Token: 0x0600A857 RID: 43095 RVA: 0x00302CDC File Offset: 0x00300EDC
		public static int IncidentCountThisInterval(IIncidentTarget target, int randSeedSalt, float minDaysPassed, float onDays, float offDays, float minSpacingDays, float minIncidents, float maxIncidents, float acceptFraction = 1f)
		{
			int num = IncidentCycleUtility.DaysToIntervals(minDaysPassed);
			int num2 = IncidentCycleUtility.QueueIntervalsPassed - num;
			if (num2 < 0)
			{
				return 0;
			}
			int num3 = IncidentCycleUtility.DaysToIntervals(onDays);
			int num4 = IncidentCycleUtility.DaysToIntervals(offDays);
			int minSpacingIntervals = IncidentCycleUtility.DaysToIntervals(minSpacingDays);
			int num5 = num3 + num4;
			int num6 = num2 / num5;
			int fixedHit = -9999999;
			for (int i = 0; i <= num6; i++)
			{
				int seed = Gen.HashCombineInt(Find.World.info.persistentRandomValue, target.ConstantRandSeed, randSeedSalt, i);
				int start = i * num5;
				if (IncidentCycleUtility.hits.Count > 0)
				{
					List<int> list = IncidentCycleUtility.hits;
					fixedHit = list[list.Count - 1];
				}
				IncidentCycleUtility.hits.Clear();
				IncidentCycleUtility.GenerateHitList(seed, start, num3, minIncidents, maxIncidents, minSpacingIntervals, acceptFraction, fixedHit);
			}
			int num7 = 0;
			for (int j = 0; j < IncidentCycleUtility.hits.Count; j++)
			{
				if (IncidentCycleUtility.hits[j] == num2)
				{
					num7++;
				}
			}
			IncidentCycleUtility.hits.Clear();
			return num7;
		}

		// Token: 0x0600A858 RID: 43096 RVA: 0x00302DD8 File Offset: 0x00300FD8
		private static void GenerateHitList(int seed, int start, int length, float minIncidents, float maxIncidents, int minSpacingIntervals, float acceptFraction, int fixedHit)
		{
			if (IncidentCycleUtility.hits.Count > 0)
			{
				throw new Exception();
			}
			Rand.PushState();
			Rand.Seed = seed;
			int num = GenMath.RoundRandom(Rand.Range(minIncidents, maxIncidents));
			int num2 = 0;
			for (;;)
			{
				IncidentCycleUtility.hits.Clear();
				if (num2++ > 100)
				{
					break;
				}
				for (int i = 0; i < num; i++)
				{
					int item = Rand.Range(0, length) + start;
					IncidentCycleUtility.hits.Add(item);
				}
				IncidentCycleUtility.hits.Sort();
				if (IncidentCycleUtility.RelaxToSatisfyMinDiff(IncidentCycleUtility.hits, minSpacingIntervals, fixedHit, start + length))
				{
					goto IL_90;
				}
			}
			Log.ErrorOnce("Too many tries finding incident time. minSpacingDays is too high.", 12612131);
			IL_90:
			if (acceptFraction < 1f)
			{
				int num3 = GenMath.RoundRandom((float)IncidentCycleUtility.hits.Count * acceptFraction);
				IncidentCycleUtility.hits.Shuffle<int>();
				IncidentCycleUtility.hits.RemoveRange(num3, IncidentCycleUtility.hits.Count - num3);
			}
			Rand.PopState();
		}

		// Token: 0x0600A859 RID: 43097 RVA: 0x00302EBC File Offset: 0x003010BC
		private static bool RelaxToSatisfyMinDiff(List<int> values, int minDiff, int fixedValue, int max)
		{
			if (values.Count == 0)
			{
				return true;
			}
			for (int i = 0; i < values.Count; i++)
			{
				int num = (i == 0) ? Mathf.Abs(values[i] - fixedValue) : Mathf.Abs(values[i] - values[i - 1]);
				if (num < minDiff)
				{
					int index = i;
					values[index] += minDiff - num;
					for (int j = i + 1; j < values.Count; j++)
					{
						if (values[j] < values[i] + j)
						{
							values[j] = values[i] + j;
						}
					}
				}
			}
			return values[values.Count - 1] <= max;
		}

		// Token: 0x0600A85A RID: 43098 RVA: 0x00302F7F File Offset: 0x0030117F
		private static int DaysToIntervals(float days)
		{
			return Mathf.RoundToInt(days * 60f);
		}

		// Token: 0x0400746F RID: 29807
		private static List<int> hits = new List<int>();
	}
}
