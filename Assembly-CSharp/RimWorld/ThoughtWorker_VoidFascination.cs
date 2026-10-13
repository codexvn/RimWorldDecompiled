using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018F6 RID: 6390
	public class ThoughtWorker_VoidFascination : ThoughtWorker
	{
		// Token: 0x0600913C RID: 37180 RVA: 0x002AB744 File Offset: 0x002A9944
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return ThoughtState.Inactive;
			}
			foreach (Thing thing in p.Map.listerThings.ThingsInGroup(ThingRequestGroup.EntityHolder))
			{
				bool flag;
				if (thing == null)
				{
					flag = (null != null);
				}
				else
				{
					CompEntityHolder compEntityHolder = thing.TryGetComp<CompEntityHolder>();
					flag = (((compEntityHolder != null) ? compEntityHolder.HeldPawn : null) != null);
				}
				if (flag)
				{
					return ThoughtState.ActiveAtStage(0);
				}
			}
			return ThoughtState.Inactive;
		}
	}
}
