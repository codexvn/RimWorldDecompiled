using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020006CA RID: 1738
	public class HediffComp_GiveRandomSituationalThought : HediffComp
	{
		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x06002B34 RID: 11060 RVA: 0x000E2193 File Offset: 0x000E0393
		public HediffCompProperties_GiveRandomSituationalThought Props
		{
			get
			{
				return (HediffCompProperties_GiveRandomSituationalThought)this.props;
			}
		}

		// Token: 0x06002B35 RID: 11061 RVA: 0x000E21A0 File Offset: 0x000E03A0
		public override void CompPostMake()
		{
			base.CompPostMake();
			this.selectedThought = this.Props.thoughtDefs.RandomElement<ThoughtDef>();
		}

		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x06002B36 RID: 11062 RVA: 0x000E21BE File Offset: 0x000E03BE
		public override string CompLabelInBracketsExtra
		{
			get
			{
				ThoughtDef thoughtDef = this.selectedThought;
				if (thoughtDef == null)
				{
					return null;
				}
				return thoughtDef.labelInBracketsExtraForHediff;
			}
		}

		// Token: 0x06002B37 RID: 11063 RVA: 0x000E21D1 File Offset: 0x000E03D1
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Defs.Look<ThoughtDef>(ref this.selectedThought, "selectedThought");
		}

		// Token: 0x040022DA RID: 8922
		public ThoughtDef selectedThought;
	}
}
