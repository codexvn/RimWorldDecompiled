using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A3D RID: 14909
	public class QuestNode_GetFreeColonistsCount : QuestNode
	{
		// Token: 0x0601466F RID: 83567 RVA: 0x006184F1 File Offset: 0x006166F1
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x06014670 RID: 83568 RVA: 0x006184FB File Offset: 0x006166FB
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x06014671 RID: 83569 RVA: 0x00618508 File Offset: 0x00616708
		private void SetVars(Slate slate)
		{
			int var;
			if (this.onlyThisMap.GetValue(slate) != null)
			{
				var = this.onlyThisMap.GetValue(slate).mapPawns.FreeColonistsCount;
			}
			else
			{
				var = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists.Count;
			}
			slate.Set<int>(this.storeAs.GetValue(slate), var, false);
		}

		// Token: 0x0400E183 RID: 57731
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E184 RID: 57732
		public SlateRef<Map> onlyThisMap;
	}
}
