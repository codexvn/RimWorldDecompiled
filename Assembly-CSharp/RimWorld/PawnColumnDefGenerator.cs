using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001903 RID: 6403
	public class PawnColumnDefGenerator
	{
		// Token: 0x0600917F RID: 37247 RVA: 0x002AD2E3 File Offset: 0x002AB4E3
		public static IEnumerable<PawnColumnDef> ImpliedPawnColumnDefs(bool hotReload = false)
		{
			PawnTableDef animalsTable = PawnTableDefOf.Animals;
			foreach (TrainableDef trainableDef in from td in DefDatabase<TrainableDef>.AllDefsListForReading
			orderby td.listPriority descending
			select td)
			{
				if (!trainableDef.specialTrainable)
				{
					string defName = "Trainable_" + trainableDef.defName;
					PawnColumnDef pawnColumnDef = hotReload ? (DefDatabase<PawnColumnDef>.GetNamed(defName, false) ?? new PawnColumnDef()) : new PawnColumnDef();
					pawnColumnDef.defName = defName;
					pawnColumnDef.trainable = trainableDef;
					pawnColumnDef.headerIcon = trainableDef.icon;
					pawnColumnDef.workerClass = typeof(PawnColumnWorker_Trainable);
					pawnColumnDef.sortable = true;
					pawnColumnDef.headerTip = trainableDef.LabelCap;
					pawnColumnDef.paintable = true;
					pawnColumnDef.modContentPack = trainableDef.modContentPack;
					animalsTable.columns.Insert(animalsTable.columns.FindIndex((PawnColumnDef x) => x.Worker is PawnColumnWorker_Checkbox) - 1, pawnColumnDef);
					yield return pawnColumnDef;
				}
			}
			IEnumerator<TrainableDef> enumerator = null;
			PawnTableDef workTable = PawnTableDefOf.Work;
			bool moveWorkTypeLabelDown = false;
			foreach (WorkTypeDef workTypeDef in (from d in WorkTypeDefsUtility.WorkTypeDefsInPriorityOrder
			where d.visible
			select d).Reverse<WorkTypeDef>())
			{
				moveWorkTypeLabelDown = !moveWorkTypeLabelDown;
				string defName2 = "WorkPriority_" + workTypeDef.defName;
				PawnColumnDef pawnColumnDef2 = hotReload ? (DefDatabase<PawnColumnDef>.GetNamed(defName2, false) ?? new PawnColumnDef()) : new PawnColumnDef();
				pawnColumnDef2.defName = defName2;
				pawnColumnDef2.workType = workTypeDef;
				pawnColumnDef2.moveWorkTypeLabelDown = moveWorkTypeLabelDown;
				pawnColumnDef2.workerClass = typeof(PawnColumnWorker_WorkPriority);
				pawnColumnDef2.sortable = true;
				pawnColumnDef2.modContentPack = workTypeDef.modContentPack;
				workTable.columns.Insert(workTable.columns.FindIndex((PawnColumnDef x) => x.Worker is PawnColumnWorker_CopyPasteWorkPriorities) + 1, pawnColumnDef2);
				yield return pawnColumnDef2;
			}
			IEnumerator<WorkTypeDef> enumerator2 = null;
			yield break;
			yield break;
		}
	}
}
