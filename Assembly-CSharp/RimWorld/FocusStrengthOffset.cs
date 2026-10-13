using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D4E RID: 11598
	public abstract class FocusStrengthOffset
	{
		// Token: 0x060104B9 RID: 66745 RVA: 0x00035ADE File Offset: 0x00033CDE
		public virtual string GetExplanation(Thing parent)
		{
			return "";
		}

		// Token: 0x060104BA RID: 66746 RVA: 0x00035ADE File Offset: 0x00033CDE
		public virtual string GetExplanationAbstract(ThingDef def = null)
		{
			return "";
		}

		// Token: 0x060104BB RID: 66747 RVA: 0x00035ADE File Offset: 0x00033CDE
		public virtual string InspectStringExtra(Thing parent, Pawn user = null)
		{
			return "";
		}

		// Token: 0x060104BC RID: 66748 RVA: 0x00006AAC File Offset: 0x00004CAC
		public virtual float GetOffset(Thing parent, Pawn user = null)
		{
			return 0f;
		}

		// Token: 0x060104BD RID: 66749 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CanApply(Thing parent, Pawn user = null)
		{
			return true;
		}

		// Token: 0x060104BE RID: 66750 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostDrawExtraSelectionOverlays(Thing parent, Pawn user = null)
		{
		}

		// Token: 0x060104BF RID: 66751 RVA: 0x00006AAC File Offset: 0x00004CAC
		public virtual float MinOffset(Thing parent = null)
		{
			return 0f;
		}

		// Token: 0x060104C0 RID: 66752 RVA: 0x004B537B File Offset: 0x004B357B
		public virtual float MaxOffset(Thing parent = null)
		{
			return this.offset;
		}

		// Token: 0x17002971 RID: 10609
		// (get) Token: 0x060104C1 RID: 66753 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool DependsOnPawn
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002972 RID: 10610
		// (get) Token: 0x060104C2 RID: 66754 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool NeedsToBeSpawned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060104C3 RID: 66755 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void ResolveReferences()
		{
		}

		// Token: 0x0400AA2C RID: 43564
		public float offset;
	}
}
