using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BD5 RID: 7125
	public class QuestPart_NoWorldObject : QuestPartActivable
	{
		// Token: 0x1700185D RID: 6237
		// (get) Token: 0x06009EA6 RID: 40614 RVA: 0x002DE076 File Offset: 0x002DC276
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.worldObject != null)
				{
					yield return this.worldObject;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06009EA7 RID: 40615 RVA: 0x002DE086 File Offset: 0x002DC286
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (this.worldObject == null || !this.worldObject.Spawned)
			{
				base.Complete();
			}
		}

		// Token: 0x06009EA8 RID: 40616 RVA: 0x002DE0A9 File Offset: 0x002DC2A9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<WorldObject>(ref this.worldObject, "worldObject", false);
		}

		// Token: 0x06009EA9 RID: 40617 RVA: 0x002DE0C4 File Offset: 0x002DC2C4
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			Site site = Find.WorldObjects.Sites.FirstOrDefault<Site>();
			if (site != null)
			{
				this.worldObject = site;
				return;
			}
			Map randomPlayerHomeMap = Find.RandomPlayerHomeMap;
			if (randomPlayerHomeMap != null)
			{
				this.worldObject = randomPlayerHomeMap.Parent;
			}
		}

		// Token: 0x04006DCD RID: 28109
		public WorldObject worldObject;
	}
}
