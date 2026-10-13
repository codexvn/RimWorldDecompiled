using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AF2 RID: 15090
	public class QuestNode_SetChildCount : QuestNode
	{
		// Token: 0x060148CF RID: 84175 RVA: 0x006215AC File Offset: 0x0061F7AC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			int num = 0;
			using (IEnumerator<Pawn> enumerator = this.pawns.GetValue(slate).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.DevelopmentalStage.Juvenile())
					{
						num++;
					}
				}
			}
			slate.Set<int>("childCount", num, false);
		}

		// Token: 0x060148D0 RID: 84176 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0400E410 RID: 58384
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
