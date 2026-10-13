using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002630 RID: 9776
	public abstract class PreceptComp
	{
		// Token: 0x170020C1 RID: 8385
		// (get) Token: 0x0600D640 RID: 54848 RVA: 0x003F9817 File Offset: 0x003F7A17
		public virtual IEnumerable<TraitRequirement> TraitsAffecting
		{
			get
			{
				return Enumerable.Empty<TraitRequirement>();
			}
		}

		// Token: 0x0600D641 RID: 54849 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_MemberTookAction(HistoryEvent ev, Precept precept, bool canApplySelfTookThoughts)
		{
		}

		// Token: 0x0600D642 RID: 54850 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_MemberWitnessedAction(HistoryEvent ev, Precept precept, Pawn member)
		{
		}

		// Token: 0x0600D643 RID: 54851 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_HistoryEvent(HistoryEvent ev, Precept precept)
		{
		}

		// Token: 0x0600D644 RID: 54852 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_MemberGenerated(Pawn pawn, Precept precept, bool newborn, bool ignoreApparel = false)
		{
		}

		// Token: 0x0600D645 RID: 54853 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_AddBedThoughts(Pawn pawn, Precept precept)
		{
		}

		// Token: 0x0600D646 RID: 54854 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool MemberWillingToDo(HistoryEvent ev)
		{
			return true;
		}

		// Token: 0x0600D647 RID: 54855 RVA: 0x003F981E File Offset: 0x003F7A1E
		public virtual IEnumerable<string> GetDescriptions()
		{
			yield return this.description.CapitalizeFirst();
			yield break;
		}

		// Token: 0x0600D648 RID: 54856 RVA: 0x00035AE5 File Offset: 0x00033CE5
		public virtual IEnumerable<string> ConfigErrors(PreceptDef parent)
		{
			return Enumerable.Empty<string>();
		}

		// Token: 0x040091AA RID: 37290
		[MustTranslate]
		public string description;

		// Token: 0x040091AB RID: 37291
		public PreceptDef preceptDef;
	}
}
