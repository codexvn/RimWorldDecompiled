using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F21 RID: 3873
	public static class Toils_Interact
	{
		// Token: 0x06006079 RID: 24697 RVA: 0x001E97A0 File Offset: 0x001E79A0
		public static Toil DestroyThing(TargetIndex ind)
		{
			Toil toil = ToilMaker.MakeToil("DestroyThing");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Thing thing = actor.jobs.curJob.GetTarget(ind).Thing;
				if (!thing.Destroyed)
				{
					if (thing.def.category == ThingCategory.Plant && thing.def.plant.IsTree && thing.def.plant.treeLoversCareIfChopped)
					{
						Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.CutTree, actor.Named(HistoryEventArgsNames.Doer)), true);
					}
					thing.Destroy(DestroyMode.Vanish);
				}
			};
			return toil;
		}
	}
}
