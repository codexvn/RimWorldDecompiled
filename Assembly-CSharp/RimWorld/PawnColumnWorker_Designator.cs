using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200342C RID: 13356
	public abstract class PawnColumnWorker_Designator : PawnColumnWorker_Checkbox
	{
		// Token: 0x17003134 RID: 12596
		// (get) Token: 0x06012CB1 RID: 76977
		protected abstract DesignationDef DesignationType { get; }

		// Token: 0x06012CB2 RID: 76978 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void Notify_DesignationAdded(Pawn pawn)
		{
		}

		// Token: 0x06012CB3 RID: 76979 RVA: 0x005946D5 File Offset: 0x005928D5
		protected override bool GetValue(Pawn pawn)
		{
			return this.GetDesignation(pawn) != null;
		}

		// Token: 0x06012CB4 RID: 76980 RVA: 0x005946E4 File Offset: 0x005928E4
		protected override void SetValue(Pawn pawn, bool value, PawnTable table)
		{
			if (value == this.GetValue(pawn))
			{
				return;
			}
			if (table.SortingBy == this.def)
			{
				table.SetDirty();
			}
			if (!value)
			{
				Designation designation = this.GetDesignation(pawn);
				if (designation != null)
				{
					pawn.MapHeld.designationManager.RemoveDesignation(designation);
				}
				return;
			}
			string title;
			if (this.ShouldConfirmDesignation(pawn, out title))
			{
				Find.WindowStack.Add(new Dialog_Confirm(title, delegate()
				{
					this.DesignationConfirmed(pawn);
				}));
				return;
			}
			this.DesignationConfirmed(pawn);
		}

		// Token: 0x06012CB5 RID: 76981 RVA: 0x0059478C File Offset: 0x0059298C
		private void DesignationConfirmed(Pawn pawn)
		{
			pawn.MapHeld.designationManager.AddDesignation(new Designation(pawn, this.DesignationType, null));
			this.Notify_DesignationAdded(pawn);
		}

		// Token: 0x06012CB6 RID: 76982 RVA: 0x005947B7 File Offset: 0x005929B7
		protected virtual bool ShouldConfirmDesignation(Pawn pawn, out string title)
		{
			title = "";
			return false;
		}

		// Token: 0x06012CB7 RID: 76983 RVA: 0x005947C4 File Offset: 0x005929C4
		private Designation GetDesignation(Pawn pawn)
		{
			Map mapHeld = pawn.MapHeld;
			if (mapHeld == null)
			{
				return null;
			}
			return mapHeld.designationManager.DesignationOn(pawn, this.DesignationType);
		}
	}
}
