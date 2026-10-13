using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020032F7 RID: 13047
	public class FloatMenuOptionProvider_DropEquipment : FloatMenuOptionProvider
	{
		// Token: 0x17002F91 RID: 12177
		// (get) Token: 0x060125A9 RID: 75177 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F92 RID: 12178
		// (get) Token: 0x060125AA RID: 75178 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F93 RID: 12179
		// (get) Token: 0x060125AB RID: 75179 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002F94 RID: 12180
		// (get) Token: 0x060125AC RID: 75180 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool CanSelfTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060125AD RID: 75181 RVA: 0x00568D5B File Offset: 0x00566F5B
		protected override bool AppliesInt(FloatMenuContext context)
		{
			Pawn_EquipmentTracker equipment = context.FirstSelectedPawn.equipment;
			return ((equipment != null) ? equipment.Primary : null) != null;
		}

		// Token: 0x060125AE RID: 75182 RVA: 0x00568D78 File Offset: 0x00566F78
		protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
		{
			if (clickedPawn != context.FirstSelectedPawn)
			{
				return null;
			}
			if (clickedPawn.IsQuestLodger() && !EquipmentUtility.QuestLodgerCanUnequip(clickedPawn.equipment.Primary, clickedPawn))
			{
				return new FloatMenuOption("CannotDrop".Translate(clickedPawn.equipment.Primary.Label, clickedPawn.equipment.Primary) + ": " + "QuestRelated".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			Action action = delegate()
			{
				clickedPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.DropEquipment, clickedPawn.equipment.Primary), new JobTag?(JobTag.Misc), false);
			};
			return new FloatMenuOption("Drop".Translate(clickedPawn.equipment.Primary.Label, clickedPawn.equipment.Primary), action, clickedPawn.equipment.Primary, Color.white, MenuOptionPriority.Default, null, clickedPawn, 0f, null, null, true, 0);
		}
	}
}
