using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020034D3 RID: 13523
	public class Instruction_EquipWeapons : Lesson_Instruction
	{
		// Token: 0x1700317E RID: 12670
		// (get) Token: 0x06013011 RID: 77841 RVA: 0x005A2E98 File Offset: 0x005A1098
		protected override float ProgressPercent
		{
			get
			{
				IEnumerable<Pawn> source = from c in base.Map.mapPawns.FreeColonists
				where !LifeStageUtility.AlwaysDowned(c)
				select c;
				return (float)(from c in source
				where c.equipment.Primary != null
				select c).Count<Pawn>() / (float)source.Count<Pawn>();
			}
		}

		// Token: 0x1700317F RID: 12671
		// (get) Token: 0x06013012 RID: 77842 RVA: 0x005A2F0D File Offset: 0x005A110D
		private IEnumerable<Thing> Weapons
		{
			get
			{
				return from it in Find.TutorialState.startingItems
				where Instruction_EquipWeapons.IsWeapon(it) && it.Spawned
				select it;
			}
		}

		// Token: 0x06013013 RID: 77843 RVA: 0x005A2F3D File Offset: 0x005A113D
		public static bool IsWeapon(Thing t)
		{
			return t.def.IsWeapon && t.def.BaseMarketValue > 30f;
		}

		// Token: 0x06013014 RID: 77844 RVA: 0x005A2F60 File Offset: 0x005A1160
		public override void LessonOnGUI()
		{
			foreach (Thing t in this.Weapons)
			{
				TutorUtility.DrawLabelOnThingOnGUI(t, this.def.onMapInstruction);
			}
			base.LessonOnGUI();
		}

		// Token: 0x06013015 RID: 77845 RVA: 0x005A2FBC File Offset: 0x005A11BC
		public override void LessonUpdate()
		{
			foreach (Thing thing in this.Weapons)
			{
				GenDraw.DrawArrowPointingAt(thing.DrawPos, true);
			}
			if (this.ProgressPercent > 0.9999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
