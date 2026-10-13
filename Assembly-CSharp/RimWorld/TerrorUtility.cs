using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003765 RID: 14181
	public static class TerrorUtility
	{
		// Token: 0x06013A78 RID: 80504 RVA: 0x005D7035 File Offset: 0x005D5235
		public static IEnumerable<Thought_MemoryObservationTerror> GetTerrorThoughts(Pawn pawn)
		{
			List<Thought_Memory> memories = pawn.needs.mood.thoughts.memories.Memories;
			int num;
			for (int i = 0; i < memories.Count; i = num + 1)
			{
				Thought_MemoryObservationTerror thought_MemoryObservationTerror = memories[i] as Thought_MemoryObservationTerror;
				if (thought_MemoryObservationTerror != null)
				{
					yield return thought_MemoryObservationTerror;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06013A79 RID: 80505 RVA: 0x005D7045 File Offset: 0x005D5245
		public static IEnumerable<Thought_MemoryObservationTerror> TakeTopTerrorThoughts(IEnumerable<Thought_MemoryObservationTerror> thoughts)
		{
			return (from t in thoughts
			orderby t.intensity descending
			select t).Take(3);
		}

		// Token: 0x06013A7A RID: 80506 RVA: 0x005D7074 File Offset: 0x005D5274
		public static float GetTerrorLevel(this Pawn pawn)
		{
			int num = 0;
			if (!pawn.IsSlave)
			{
				return (float)num;
			}
			foreach (Thought_MemoryObservationTerror thought_MemoryObservationTerror in TerrorUtility.GetTerrorThoughts(pawn))
			{
				num += thought_MemoryObservationTerror.intensity;
			}
			return (float)Mathf.Min(num, 100) / 100f;
		}

		// Token: 0x06013A7B RID: 80507 RVA: 0x005D70E0 File Offset: 0x005D52E0
		public static void RemoveAllTerrorThoughts(Pawn pawn)
		{
			foreach (Thought_MemoryObservationTerror th in TerrorUtility.GetTerrorThoughts(pawn))
			{
				pawn.needs.mood.thoughts.memories.RemoveMemory(th);
			}
		}

		// Token: 0x06013A7C RID: 80508 RVA: 0x005D7144 File Offset: 0x005D5344
		public static bool TryCreateTerrorThought(Thing thing, out Thought_MemoryObservationTerror thought)
		{
			thought = null;
			if (!ModsConfig.IdeologyActive)
			{
				return false;
			}
			float statValue = thing.GetStatValue(StatDefOf.TerrorSource, true, -1);
			if (statValue <= 0f)
			{
				return false;
			}
			thought = (Thought_MemoryObservationTerror)ThoughtMaker.MakeThought(ThoughtDefOf.ObservedTerror);
			thought.Target = thing;
			thought.intensity = (int)statValue;
			return true;
		}

		// Token: 0x0400D9BC RID: 55740
		private const int MaxTerror = 100;

		// Token: 0x0400D9BD RID: 55741
		private const int TerrorThoughtsToTake = 3;

		// Token: 0x0400D9BE RID: 55742
		public static SimpleCurve SuppressionFallRateOverTerror = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(25f, -15f),
				true
			},
			{
				new CurvePoint(50f, -25f),
				true
			},
			{
				new CurvePoint(100f, -45f),
				true
			}
		};
	}
}
