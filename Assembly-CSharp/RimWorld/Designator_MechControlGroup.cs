using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020030A5 RID: 12453
	public class Designator_MechControlGroup : Designator
	{
		// Token: 0x17002D40 RID: 11584
		// (get) Token: 0x060117A8 RID: 71592 RVA: 0x0050510F File Offset: 0x0050330F
		public static IEnumerable<MechanitorControlGroup> SelectedControlGroups
		{
			get
			{
				Designator_MechControlGroup.tmpControlGroups.Clear();
				foreach (Pawn pawn in Find.Selector.SelectedPawns)
				{
					if (pawn != null)
					{
						Pawn pawn2 = pawn;
						if (pawn2.IsColonyMech)
						{
							Pawn overseer = pawn.GetOverseer();
							foreach (MechanitorControlGroup mechanitorControlGroup in overseer.mechanitor.controlGroups)
							{
								if (mechanitorControlGroup != null && Designator_MechControlGroup.tmpControlGroups.Add(mechanitorControlGroup))
								{
									yield return mechanitorControlGroup;
								}
							}
							List<MechanitorControlGroup>.Enumerator enumerator2 = default(List<MechanitorControlGroup>.Enumerator);
						}
					}
				}
				List<Pawn>.Enumerator enumerator = default(List<Pawn>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x060117A9 RID: 71593 RVA: 0x00505118 File Offset: 0x00503318
		public Designator_MechControlGroup()
		{
			this.defaultLabel = "DesignatorMechControlGroup".Translate() + "...";
			this.defaultDesc = "DesignatorMechControlGroupDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/MechControlGroup", true);
			this.showReverseDesignatorDisabledReason = true;
		}

		// Token: 0x060117AA RID: 71594 RVA: 0x00505178 File Offset: 0x00503378
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			foreach (Thing t in c.GetThingList(base.Map))
			{
				if (this.CanDesignateThing(t).Accepted)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060117AB RID: 71595 RVA: 0x00505204 File Offset: 0x00503404
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn == null || !pawn.IsColonyMech || pawn.GetOverseer() == null)
			{
				return false;
			}
			Pawn pawn2 = null;
			foreach (Pawn pawn3 in Find.Selector.SelectedPawns)
			{
				Pawn overseer = pawn3.GetOverseer();
				if (pawn2 != null && pawn2 != overseer)
				{
					return false;
				}
				pawn2 = overseer;
			}
			AcceptanceReport canControlMechs = pawn2.mechanitor.CanControlMechs;
			if (canControlMechs)
			{
				return true;
			}
			if (!canControlMechs.Reason.NullOrEmpty())
			{
				return ("DisabledCommand".Translate() + ": " + canControlMechs.Reason).Colorize(ColorLibrary.RedReadable);
			}
			return false;
		}

		// Token: 0x060117AC RID: 71596 RVA: 0x005052F4 File Offset: 0x005034F4
		public override void DesignateThing(Thing t)
		{
			this.ProcessInput(null);
		}

		// Token: 0x060117AD RID: 71597 RVA: 0x00505300 File Offset: 0x00503500
		private bool CanAssignToControlGroup(Pawn pawn, MechanitorControlGroup group)
		{
			Pawn overseer = pawn.GetOverseer();
			return overseer != null && group.Tracker.Pawn == overseer;
		}

		// Token: 0x060117AE RID: 71598 RVA: 0x00505328 File Offset: 0x00503528
		public override void ProcessInput(Event ev)
		{
			Designator_MechControlGroup.tmpSelectedControlGroups.Clear();
			Designator_MechControlGroup.tmpSelectedControlGroups.AddRange(Designator_MechControlGroup.SelectedControlGroups);
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			for (int i = 0; i < Designator_MechControlGroup.tmpSelectedControlGroups.Count; i++)
			{
				MechanitorControlGroup selected = Designator_MechControlGroup.tmpSelectedControlGroups[i];
				if (Find.Selector.SelectedPawns.All((Pawn p) => p.GetMechControlGroup() == selected))
				{
					FloatMenuOption item = new FloatMenuOption("CannotAssignMechToControlGroup".Translate(selected.Index) + " (" + selected.WorkMode.LabelCap.ToString() + ")" + ": " + "AssignMechAlreadyAssigned".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					list.Add(item);
				}
				else
				{
					FloatMenuOption item2 = new FloatMenuOption("AssignMechToControlGroup".Translate(selected.Index) + " (" + selected.WorkMode.LabelCap.ToString() + ")", delegate()
					{
						foreach (Pawn pawn in Find.Selector.SelectedPawns)
						{
							if (this.CanAssignToControlGroup(pawn, selected))
							{
								selected.Assign(pawn);
							}
						}
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					list.Add(item2);
				}
			}
			Find.WindowStack.Add(new FloatMenu(list));
			Designator_MechControlGroup.tmpSelectedControlGroups.Clear();
		}

		// Token: 0x060117AF RID: 71599 RVA: 0x000026EA File Offset: 0x000008EA
		public override void SelectedUpdate()
		{
		}

		// Token: 0x0400B4BE RID: 46270
		private static readonly HashSet<MechanitorControlGroup> tmpControlGroups = new HashSet<MechanitorControlGroup>();

		// Token: 0x0400B4BF RID: 46271
		private static List<MechanitorControlGroup> tmpSelectedControlGroups = new List<MechanitorControlGroup>();
	}
}
