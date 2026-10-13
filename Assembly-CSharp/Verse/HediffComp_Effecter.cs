using System;

namespace Verse
{
	// Token: 0x020006B1 RID: 1713
	public class HediffComp_Effecter : HediffComp
	{
		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x06002AEC RID: 10988 RVA: 0x000E10F1 File Offset: 0x000DF2F1
		public HediffCompProperties_Effecter Props
		{
			get
			{
				return (HediffCompProperties_Effecter)this.props;
			}
		}

		// Token: 0x06002AED RID: 10989 RVA: 0x000E1100 File Offset: 0x000DF300
		public EffecterDef CurrentStateEffecter()
		{
			if (this.parent.CurStageIndex >= this.Props.severityIndices.min && (this.Props.severityIndices.max < 0 || this.parent.CurStageIndex <= this.Props.severityIndices.max))
			{
				return this.Props.stateEffecter;
			}
			return null;
		}
	}
}
