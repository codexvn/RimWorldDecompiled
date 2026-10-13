using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C27 RID: 7207
	public class QuestPart_DestroyThingsOrPassToWorldOnCleanup : QuestPart
	{
		// Token: 0x170018AC RID: 6316
		// (get) Token: 0x0600A082 RID: 41090 RVA: 0x002E3D97 File Offset: 0x002E1F97
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				if (this.questLookTargets)
				{
					int num;
					for (int i = 0; i < this.things.Count; i = num + 1)
					{
						yield return this.things[i];
						num = i;
					}
				}
				yield break;
			}
		}

		// Token: 0x0600A083 RID: 41091 RVA: 0x002E3DA7 File Offset: 0x002E1FA7
		public override void Cleanup()
		{
			base.Cleanup();
			QuestPart_DestroyThingsOrPassToWorld.Destroy(this.things);
		}

		// Token: 0x0600A084 RID: 41092 RVA: 0x002E3DBC File Offset: 0x002E1FBC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.questLookTargets, "questLookTargets", true, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.things.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x04006EF9 RID: 28409
		public List<Thing> things = new List<Thing>();

		// Token: 0x04006EFA RID: 28410
		public bool questLookTargets = true;
	}
}
