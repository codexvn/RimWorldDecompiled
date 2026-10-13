using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200068B RID: 1675
	public class HediffComp
	{
		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x06002A2F RID: 10799 RVA: 0x000DED7F File Offset: 0x000DCF7F
		public Pawn Pawn
		{
			get
			{
				return this.parent.pawn;
			}
		}

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x06002A30 RID: 10800 RVA: 0x000DED8C File Offset: 0x000DCF8C
		public HediffDef Def
		{
			get
			{
				return this.parent.def;
			}
		}

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x06002A31 RID: 10801 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string CompLabelPrefix
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x06002A32 RID: 10802 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string CompLabelInBracketsExtra
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x06002A33 RID: 10803 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string CompDescriptionExtra
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x06002A34 RID: 10804 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string CompTipStringExtra
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06002A35 RID: 10805 RVA: 0x000DD801 File Offset: 0x000DBA01
		public virtual TextureAndColor CompStateIcon
		{
			get
			{
				return TextureAndColor.None;
			}
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x06002A36 RID: 10806 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool CompShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002A37 RID: 10807 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void CompPostMake()
		{
		}

		// Token: 0x06002A38 RID: 10808 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void CompPostTick(ref float severityAdjustment)
		{
		}

		// Token: 0x06002A39 RID: 10809 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
		}

		// Token: 0x06002A3A RID: 10810 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void CompExposeData()
		{
		}

		// Token: 0x06002A3B RID: 10811 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void CompPostPostAdd(DamageInfo? dinfo)
		{
		}

		// Token: 0x06002A3C RID: 10812 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void CompPostPostRemoved()
		{
		}

		// Token: 0x06002A3D RID: 10813 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void CompPostMerged(Hediff other)
		{
		}

		// Token: 0x06002A3E RID: 10814 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool CompDisallowVisible()
		{
			return false;
		}

		// Token: 0x06002A3F RID: 10815 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void CompModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
		}

		// Token: 0x06002A40 RID: 10816 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void CompPostInjuryHeal(float amount)
		{
		}

		// Token: 0x06002A41 RID: 10817 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void CompTended(float quality, float maxQuality, int batchPosition = 0)
		{
		}

		// Token: 0x06002A42 RID: 10818 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_ImplantUsed(string violationSourceName, float detectionChance, int violationSourceLevel = -1)
		{
		}

		// Token: 0x06002A43 RID: 10819 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_EntropyGained(float baseAmount, float finalAmount, Thing source = null)
		{
		}

		// Token: 0x06002A44 RID: 10820 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_PawnUsedVerb(Verb verb, LocalTargetInfo target)
		{
		}

		// Token: 0x06002A45 RID: 10821 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
		{
		}

		// Token: 0x06002A46 RID: 10822 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_PawnKilled()
		{
		}

		// Token: 0x06002A47 RID: 10823 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_KilledPawn(Pawn victim, DamageInfo? dinfo)
		{
		}

		// Token: 0x06002A48 RID: 10824 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
		}

		// Token: 0x06002A49 RID: 10825 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_Spawned()
		{
		}

		// Token: 0x06002A4A RID: 10826 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_SurgicallyRemoved(Pawn surgeon)
		{
		}

		// Token: 0x06002A4B RID: 10827 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_SurgicallyReplaced(Pawn surgeon)
		{
		}

		// Token: 0x06002A4C RID: 10828 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void CopyFrom(HediffComp other)
		{
		}

		// Token: 0x06002A4D RID: 10829 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual IEnumerable<Gizmo> CompGetGizmos()
		{
			return null;
		}

		// Token: 0x06002A4E RID: 10830 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string CompDebugString()
		{
			return null;
		}

		// Token: 0x0400224A RID: 8778
		public HediffWithComps parent;

		// Token: 0x0400224B RID: 8779
		public HediffCompProperties props;
	}
}
