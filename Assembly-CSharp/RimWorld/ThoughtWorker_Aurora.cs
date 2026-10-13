using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001886 RID: 6278
	public class ThoughtWorker_Aurora : ThoughtWorker_GameCondition
	{
		// Token: 0x06008FFB RID: 36859 RVA: 0x002A7C30 File Offset: 0x002A5E30
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			Map mapHeld = p.MapHeld;
			bool flag;
			if (mapHeld == null)
			{
				flag = false;
			}
			else
			{
				GameConditionManager gameConditionManager = mapHeld.GameConditionManager;
				bool? flag2 = (gameConditionManager != null) ? new bool?(gameConditionManager.IsAlwaysDarkOutside) : null;
				bool flag3 = true;
				flag = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
			}
			if (flag)
			{
				return false;
			}
			return base.CurrentStateInternal(p).Active && p.SpawnedOrAnyParentSpawned && !p.PositionHeld.Roofed(p.MapHeld) && !PawnUtility.IsBiologicallyOrArtificiallyBlind(p);
		}
	}
}
