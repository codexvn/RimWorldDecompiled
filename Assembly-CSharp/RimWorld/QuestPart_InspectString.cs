using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C51 RID: 7249
	public class QuestPart_InspectString : QuestPartActivable
	{
		// Token: 0x170018D3 RID: 6355
		// (get) Token: 0x0600A16E RID: 41326 RVA: 0x002E684F File Offset: 0x002E4A4F
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				int num;
				for (int i = 0; i < this.targets.Count; i = num + 1)
				{
					ISelectable selectable = this.targets[i];
					if (selectable is Thing)
					{
						yield return (Thing)selectable;
					}
					else if (selectable is WorldObject)
					{
						yield return (WorldObject)selectable;
					}
					num = i;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x0600A16F RID: 41327 RVA: 0x002E685F File Offset: 0x002E4A5F
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.resolvedInspectString = receivedArgs.GetFormattedText(this.inspectString);
		}

		// Token: 0x0600A170 RID: 41328 RVA: 0x002E6885 File Offset: 0x002E4A85
		public override string ExtraInspectString(ISelectable target)
		{
			if (this.targets.Contains(target))
			{
				return this.resolvedInspectString;
			}
			return null;
		}

		// Token: 0x0600A171 RID: 41329 RVA: 0x002E68A0 File Offset: 0x002E4AA0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<ISelectable>(ref this.targets, "targets", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inspectString, "inspectString", null, false);
			Scribe_Values.Look<string>(ref this.resolvedInspectString, "resolvedInspectString", null, false);
		}

		// Token: 0x0600A172 RID: 41330 RVA: 0x002E68ED File Offset: 0x002E4AED
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			if (Find.AnyPlayerHomeMap != null)
			{
				this.targets.Add(Find.RandomPlayerHomeMap.mapPawns.FreeColonists.FirstOrDefault<Pawn>());
				this.inspectString = "Debug inspect string.";
			}
		}

		// Token: 0x04006F8D RID: 28557
		public List<ISelectable> targets = new List<ISelectable>();

		// Token: 0x04006F8E RID: 28558
		public string inspectString;

		// Token: 0x04006F8F RID: 28559
		private string resolvedInspectString;

		// Token: 0x04006F90 RID: 28560
		private ILoadReferenceable targetRef;
	}
}
