using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200194F RID: 6479
	public class CompProperties_AncientVentEmitter : CompProperties
	{
		// Token: 0x0600935D RID: 37725 RVA: 0x002B5856 File Offset: 0x002B3A56
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.conditionToCause == null)
			{
				yield return parentDef.defName + " has CompProperties_AncientVentEmitter with null conditionToCause.";
			}
			if (this.onDurationMtbDays <= 0)
			{
				yield return parentDef.defName + " has CompProperties_AncientVentEmitter with onDurationMtbDays <= 0.";
			}
			if (this.offDurationMtbDays <= 0)
			{
				yield return parentDef.defName + " has CompProperties_AncientVentEmitter with offDurationMtbDays <= 0.";
			}
			yield break;
			yield break;
		}

		// Token: 0x04005EC1 RID: 24257
		public GameConditionDef conditionToCause;

		// Token: 0x04005EC2 RID: 24258
		public int onDurationMtbDays;

		// Token: 0x04005EC3 RID: 24259
		public int offDurationMtbDays;

		// Token: 0x04005EC4 RID: 24260
		public int heatToPush;

		// Token: 0x04005EC5 RID: 24261
		public string startupMessageKey;

		// Token: 0x04005EC6 RID: 24262
		public string shutdownMessageKey;

		// Token: 0x04005EC7 RID: 24263
		public string activeInspectStringKey;

		// Token: 0x04005EC8 RID: 24264
		public SoundDef activeSound;
	}
}
