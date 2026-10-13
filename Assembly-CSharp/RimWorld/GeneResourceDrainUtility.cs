using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003842 RID: 14402
	public static class GeneResourceDrainUtility
	{
		// Token: 0x06013EAA RID: 81578 RVA: 0x005E9053 File Offset: 0x005E7253
		public static void TickResourceDrainInterval(IGeneResourceDrain drain, int delta)
		{
			if (drain.CanOffset)
			{
				if (drain.Resource == null)
				{
					return;
				}
				GeneResourceDrainUtility.OffsetResource(drain, -drain.ResourceLossPerDay * (float)delta / 60000f);
			}
		}

		// Token: 0x06013EAB RID: 81579 RVA: 0x005E907C File Offset: 0x005E727C
		public static void PostResourceOffset(IGeneResourceDrain drain, float oldValue)
		{
			if (oldValue > 0f && drain.Resource.Value <= 0f)
			{
				Pawn pawn = drain.Pawn;
				if (!pawn.health.hediffSet.HasHediff(HediffDefOf.HemogenCraving, false))
				{
					pawn.health.AddHediff(HediffDefOf.HemogenCraving, null, null, null);
				}
			}
		}

		// Token: 0x06013EAC RID: 81580 RVA: 0x005E90E0 File Offset: 0x005E72E0
		public static void OffsetResource(IGeneResourceDrain drain, float amnt)
		{
			if (drain.Resource == null)
			{
				return;
			}
			float value = drain.Resource.Value;
			drain.Resource.Value += amnt;
			GeneResourceDrainUtility.PostResourceOffset(drain, value);
		}

		// Token: 0x06013EAD RID: 81581 RVA: 0x005E911C File Offset: 0x005E731C
		public static IEnumerable<Gizmo> GetResourceDrainGizmos(IGeneResourceDrain drain)
		{
			if (DebugSettings.ShowDevGizmos && drain.Resource != null)
			{
				Gene_Resource resource = drain.Resource;
				yield return new Command_Action
				{
					defaultLabel = "DEV: " + resource.ResourceLabel + " -10",
					action = delegate()
					{
						GeneResourceDrainUtility.OffsetResource(drain, -0.1f);
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: " + resource.ResourceLabel + " +10",
					action = delegate()
					{
						GeneResourceDrainUtility.OffsetResource(drain, 0.1f);
					}
				};
				resource = null;
			}
			yield break;
		}
	}
}
