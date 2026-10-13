using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020006AF RID: 1711
	public class HediffComp_DrugEffectFactor : HediffComp
	{
		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x06002AE5 RID: 10981 RVA: 0x000E100C File Offset: 0x000DF20C
		public HediffCompProperties_DrugEffectFactor Props
		{
			get
			{
				return (HediffCompProperties_DrugEffectFactor)this.props;
			}
		}

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x06002AE6 RID: 10982 RVA: 0x000E1019 File Offset: 0x000DF219
		private float CurrentFactor
		{
			get
			{
				return HediffComp_DrugEffectFactor.EffectFactorSeverityCurve.Evaluate(this.parent.Severity);
			}
		}

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x06002AE7 RID: 10983 RVA: 0x000E1030 File Offset: 0x000DF230
		public override string CompTipStringExtra
		{
			get
			{
				return "DrugEffectMultiplier".Translate(this.Props.chemical.label, this.CurrentFactor.ToStringPercent()).CapitalizeFirst();
			}
		}

		// Token: 0x06002AE8 RID: 10984 RVA: 0x000E1079 File Offset: 0x000DF279
		public override void CompModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
			if (this.Props.chemical == chem)
			{
				effect *= this.CurrentFactor;
			}
		}

		// Token: 0x040022A6 RID: 8870
		private static readonly SimpleCurve EffectFactorSeverityCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(1f, 0.25f),
				true
			}
		};
	}
}
