using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A45 RID: 14917
	public class QuestNode_GetFactionOf : QuestNode
	{
		// Token: 0x0601468B RID: 83595 RVA: 0x00618BFE File Offset: 0x00616DFE
		protected override bool TestRunInt(Slate slate)
		{
			this.DoWork(slate);
			return true;
		}

		// Token: 0x0601468C RID: 83596 RVA: 0x00618C08 File Offset: 0x00616E08
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x0601468D RID: 83597 RVA: 0x00618C18 File Offset: 0x00616E18
		private void DoWork(Slate slate)
		{
			Faction var = null;
			Thing value = this.thing.GetValue(slate);
			if (value != null)
			{
				var = value.Faction;
			}
			slate.Set<Faction>(this.storeAs.GetValue(slate), var, false);
		}

		// Token: 0x0400E1A3 RID: 57763
		public SlateRef<Thing> thing;

		// Token: 0x0400E1A4 RID: 57764
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
