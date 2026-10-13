using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000694 RID: 1684
	public class HediffCompProperties_ChangeImplantLevel : HediffCompProperties
	{
		// Token: 0x06002A6E RID: 10862 RVA: 0x000DF694 File Offset: 0x000DD894
		public HediffCompProperties_ChangeImplantLevel()
		{
			this.compClass = typeof(HediffComp_ChangeImplantLevel);
		}

		// Token: 0x06002A6F RID: 10863 RVA: 0x000DF6AC File Offset: 0x000DD8AC
		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.implant == null)
			{
				yield return "implant is null";
			}
			else if (!typeof(Hediff_Level).IsAssignableFrom(this.implant.hediffClass))
			{
				yield return "implant is not Hediff_Level";
			}
			if (this.levelOffset == 0)
			{
				yield return "levelOffset is 0";
			}
			if (this.probabilityPerStage == null)
			{
				yield return "probabilityPerStage is not defined";
			}
			else if (this.probabilityPerStage.Count != parentDef.stages.Count)
			{
				yield return "probabilityPerStage count doesn't match Hediffs number of stages";
			}
			yield break;
			yield break;
		}

		// Token: 0x04002265 RID: 8805
		public HediffDef implant;

		// Token: 0x04002266 RID: 8806
		public int levelOffset;

		// Token: 0x04002267 RID: 8807
		public List<ChangeImplantLevel_Probability> probabilityPerStage;
	}
}
