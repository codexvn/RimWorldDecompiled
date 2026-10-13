using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AC6 RID: 15046
	public class QuestNode_GetPawnKindCombatPower : QuestNode
	{
		// Token: 0x06014840 RID: 84032 RVA: 0x0061F5E9 File Offset: 0x0061D7E9
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<float>(this.storeAs.GetValue(slate), this.kindDef.GetValue(slate).combatPower, false);
			return true;
		}

		// Token: 0x06014841 RID: 84033 RVA: 0x0061F610 File Offset: 0x0061D810
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<float>(this.storeAs.GetValue(slate), this.kindDef.GetValue(slate).combatPower, false);
		}

		// Token: 0x0400E35C RID: 58204
		public SlateRef<PawnKindDef> kindDef;

		// Token: 0x0400E35D RID: 58205
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
