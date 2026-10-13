using System;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020018D5 RID: 6357
	public abstract class ThoughtWorker_RelatedChildMoodBase : ThoughtWorker
	{
		// Token: 0x060090D4 RID: 37076 RVA: 0x002AA5FA File Offset: 0x002A87FA
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.BiotechActive)
			{
				return ThoughtState.Inactive;
			}
			if (p.Suspended)
			{
				return ThoughtState.Inactive;
			}
			if (!this.Active(p))
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveDefault;
		}

		// Token: 0x060090D5 RID: 37077 RVA: 0x002AA62C File Offset: 0x002A882C
		public override string PostProcessLabel(Pawn p, string label)
		{
			Pawn pawn2 = p.relations.Children.FirstOrDefault((Pawn pawn) => ThoughtWorker_RelatedChildMoodBase.IsChildWithMood(pawn, this.MoodRange()));
			if (pawn2 != null)
			{
				return label.Formatted(p.Named("PAWN"), pawn2.Named("CHILD"));
			}
			return this.SingleChildLabel().Translate().CapitalizeFirst();
		}

		// Token: 0x060090D6 RID: 37078
		protected abstract bool Active(Pawn p);

		// Token: 0x060090D7 RID: 37079
		protected abstract string SingleChildLabel();

		// Token: 0x060090D8 RID: 37080
		protected abstract FloatRange MoodRange();

		// Token: 0x060090D9 RID: 37081 RVA: 0x002AA694 File Offset: 0x002A8894
		protected int ChildrenWithMoodCount(Pawn parent)
		{
			int num = 0;
			if (parent.relations.ChildrenCount > 0)
			{
				foreach (Pawn pawn in parent.relations.Children)
				{
					if (ThoughtWorker_RelatedChildMoodBase.InSameMapOrCaravan(parent, pawn) && ThoughtWorker_RelatedChildMoodBase.IsChildWithMood(pawn, this.MoodRange()))
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x060090DA RID: 37082 RVA: 0x002AA70C File Offset: 0x002A890C
		public static bool IsChildWithMood(Pawn child, FloatRange moodRange)
		{
			Pawn_NeedsTracker needs = child.needs;
			return ((needs != null) ? needs.mood : null) != null && child.DevelopmentalStage.Juvenile() && moodRange.Includes(child.needs.mood.CurLevelPercentage) && !(child.ParentHolder is Building_GrowthVat) && !(child.ParentHolder is Building_CryptosleepCasket);
		}

		// Token: 0x060090DB RID: 37083 RVA: 0x002AA774 File Offset: 0x002A8974
		public static bool InSameMapOrCaravan(Pawn pawn, Pawn other)
		{
			Map mapHeld = pawn.MapHeld;
			if (mapHeld != null && mapHeld == other.MapHeld)
			{
				return true;
			}
			Caravan caravan = pawn.GetCaravan();
			return caravan != null && caravan == other.GetCaravan();
		}
	}
}
