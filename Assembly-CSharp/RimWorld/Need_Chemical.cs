using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002473 RID: 9331
	public class Need_Chemical : Need
	{
		// Token: 0x17001EA7 RID: 7847
		// (get) Token: 0x0600CA70 RID: 51824 RVA: 0x0000F9E0 File Offset: 0x0000DBE0
		public override int GUIChangeArrow
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x17001EA8 RID: 7848
		// (get) Token: 0x0600CA71 RID: 51825 RVA: 0x003B4D94 File Offset: 0x003B2F94
		public DrugDesireCategory CurCategory
		{
			get
			{
				if (this.CurLevel > 0.1f)
				{
					return DrugDesireCategory.Satisfied;
				}
				if (this.CurLevel > 0.01f)
				{
					return DrugDesireCategory.Desire;
				}
				return DrugDesireCategory.Withdrawal;
			}
		}

		// Token: 0x17001EA9 RID: 7849
		// (get) Token: 0x0600CA72 RID: 51826 RVA: 0x003B4DB5 File Offset: 0x003B2FB5
		// (set) Token: 0x0600CA73 RID: 51827 RVA: 0x003B4DC0 File Offset: 0x003B2FC0
		public override float CurLevel
		{
			get
			{
				return base.CurLevel;
			}
			set
			{
				DrugDesireCategory curCategory = this.CurCategory;
				base.CurLevel = value;
				if (this.CurCategory != curCategory)
				{
					this.CategoryChanged();
				}
			}
		}

		// Token: 0x17001EAA RID: 7850
		// (get) Token: 0x0600CA74 RID: 51828 RVA: 0x003B4DEC File Offset: 0x003B2FEC
		public Hediff_Addiction AddictionHediff
		{
			get
			{
				List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					Hediff_Addiction hediff_Addiction = hediffs[i] as Hediff_Addiction;
					if (hediff_Addiction != null && hediff_Addiction.def.chemicalNeed == this.def)
					{
						return hediff_Addiction;
					}
				}
				return null;
			}
		}

		// Token: 0x17001EAB RID: 7851
		// (get) Token: 0x0600CA75 RID: 51829 RVA: 0x003B4E46 File Offset: 0x003B3046
		private float ChemicalFallPerTick
		{
			get
			{
				return this.def.fallPerDay / 60000f;
			}
		}

		// Token: 0x0600CA76 RID: 51830 RVA: 0x003B4E59 File Offset: 0x003B3059
		public Need_Chemical(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.1f);
		}

		// Token: 0x0600CA77 RID: 51831 RVA: 0x003B4E7D File Offset: 0x003B307D
		public override void SetInitialLevel()
		{
			base.CurLevelPercentage = Rand.Range(0.8f, 1f);
		}

		// Token: 0x0600CA78 RID: 51832 RVA: 0x003B4E94 File Offset: 0x003B3094
		public override void NeedInterval()
		{
			if (!this.IsFrozen)
			{
				this.CurLevel -= this.ChemicalFallPerTick * 150f;
			}
		}

		// Token: 0x0600CA79 RID: 51833 RVA: 0x003B4EB7 File Offset: 0x003B30B7
		private void CategoryChanged()
		{
			Hediff_Addiction addictionHediff = this.AddictionHediff;
			if (addictionHediff == null)
			{
				return;
			}
			addictionHediff.Notify_NeedCategoryChanged();
		}

		// Token: 0x040089CC RID: 35276
		private const float ThreshDesire = 0.01f;

		// Token: 0x040089CD RID: 35277
		private const float ThreshSatisfied = 0.1f;
	}
}
