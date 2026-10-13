using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020034CE RID: 13518
	public class Instruction_BuildSandbags : Lesson_Instruction
	{
		// Token: 0x1700317C RID: 12668
		// (get) Token: 0x06012FF9 RID: 77817 RVA: 0x005A2804 File Offset: 0x005A0A04
		protected override float ProgressPercent
		{
			get
			{
				int num = 0;
				int num2 = 0;
				using (List<IntVec3>.Enumerator enumerator = this.sandbagCells.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (TutorUtility.BuildingOrBlueprintOrFrameCenterExists(enumerator.Current, base.Map, ThingDefOf.Sandbags))
						{
							num2++;
						}
						num++;
					}
				}
				return (float)num2 / (float)num;
			}
		}

		// Token: 0x06012FFA RID: 77818 RVA: 0x005A2874 File Offset: 0x005A0A74
		public override void OnActivated()
		{
			base.OnActivated();
			Find.TutorialState.sandbagsRect = TutorUtility.FindUsableRect(7, 7, base.Map, 0f, false);
			this.sandbagCells = new List<IntVec3>();
			foreach (IntVec3 intVec in Find.TutorialState.sandbagsRect.EdgeCells)
			{
				if (intVec.x != Find.TutorialState.sandbagsRect.CenterCell.x && intVec.z != Find.TutorialState.sandbagsRect.CenterCell.z)
				{
					this.sandbagCells.Add(intVec);
				}
			}
			foreach (IntVec3 c in Find.TutorialState.sandbagsRect.ContractedBy(1))
			{
				if (!Find.TutorialState.sandbagsRect.ContractedBy(2).Contains(c))
				{
					List<Thing> thingList = c.GetThingList(base.Map);
					for (int i = thingList.Count - 1; i >= 0; i--)
					{
						Thing thing = thingList[i];
						if (thing.def.passability != Traversability.Standable && (thing.def.category == ThingCategory.Plant || thing.def.category == ThingCategory.Item))
						{
							thing.Destroy(DestroyMode.Vanish);
						}
					}
				}
			}
		}

		// Token: 0x06012FFB RID: 77819 RVA: 0x005A2A08 File Offset: 0x005A0C08
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.sandbagCells, "sandbagCells", LookMode.Undefined, Array.Empty<object>());
		}

		// Token: 0x06012FFC RID: 77820 RVA: 0x005A2A26 File Offset: 0x005A0C26
		public override void LessonOnGUI()
		{
			TutorUtility.DrawLabelOnGUI(Gen.AveragePosition(this.sandbagCells), this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06012FFD RID: 77821 RVA: 0x005A2A4C File Offset: 0x005A0C4C
		public override void LessonUpdate()
		{
			GenDraw.DrawFieldEdges((from c in this.sandbagCells
			where !TutorUtility.BuildingOrBlueprintOrFrameCenterExists(c, base.Map, ThingDefOf.Sandbags)
			select c).ToList<IntVec3>(), 2900);
			GenDraw.DrawArrowPointingAt(Gen.AveragePosition(this.sandbagCells), false);
			if (this.ProgressPercent > 0.9999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x06012FFE RID: 77822 RVA: 0x005A2AA7 File Offset: 0x005A0CA7
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-Sandbags")
			{
				return TutorUtility.EventCellsAreWithin(ep, this.sandbagCells);
			}
			return base.AllowAction(ep);
		}

		// Token: 0x0400C508 RID: 50440
		private List<IntVec3> sandbagCells;
	}
}
