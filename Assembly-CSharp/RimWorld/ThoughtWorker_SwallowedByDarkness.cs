using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018EE RID: 6382
	public class ThoughtWorker_SwallowedByDarkness : ThoughtWorker
	{
		// Token: 0x0600912A RID: 37162 RVA: 0x002AB434 File Offset: 0x002A9634
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return ModsConfig.AnomalyActive && ThoughtWorker_SwallowedByDarkness.InDarkness(p) && p.MapHeld.GameConditionManager.MapBrightness < 0.5f;
		}

		// Token: 0x0600912B RID: 37163 RVA: 0x002AB464 File Offset: 0x002A9664
		public static bool InDarkness(Pawn pawn)
		{
			return pawn.SpawnedOrAnyParentSpawned && !pawn.Dead && !pawn.Suspended && pawn.MapHeld.glowGrid.GroundGlowAt(pawn.PositionHeld, false, false) <= 0f;
		}
	}
}
