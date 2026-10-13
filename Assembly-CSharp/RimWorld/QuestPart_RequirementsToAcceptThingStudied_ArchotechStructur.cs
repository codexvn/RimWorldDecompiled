using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C98 RID: 7320
	public class QuestPart_RequirementsToAcceptThingStudied_ArchotechStructures : QuestPart_RequirementsToAcceptThingStudied
	{
		// Token: 0x17001913 RID: 6419
		// (get) Token: 0x0600A314 RID: 41748 RVA: 0x002ED713 File Offset: 0x002EB913
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.thing != null && !this.thing.Spawned)
				{
					if (this.thing.TryGetComp<CompStudiable>().Completed)
					{
						yield break;
					}
					foreach (WorldObject worldObject in Find.WorldObjects.AllWorldObjects)
					{
						AbandonedArchotechStructures abandonedArchotechStructures = worldObject as AbandonedArchotechStructures;
						if (abandonedArchotechStructures != null && !abandonedArchotechStructures.archotechStructures.NullOrEmpty<Building>() && abandonedArchotechStructures.archotechStructures.Contains(this.thing))
						{
							yield return worldObject;
							yield break;
						}
					}
					List<WorldObject>.Enumerator enumerator2 = default(List<WorldObject>.Enumerator);
				}
				yield break;
				yield break;
			}
		}
	}
}
