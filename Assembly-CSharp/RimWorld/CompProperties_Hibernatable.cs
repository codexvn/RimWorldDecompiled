using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001992 RID: 6546
	public class CompProperties_Hibernatable : CompProperties
	{
		// Token: 0x06009453 RID: 37971 RVA: 0x002B917B File Offset: 0x002B737B
		public CompProperties_Hibernatable()
		{
			this.compClass = typeof(CompHibernatable);
		}

		// Token: 0x06009454 RID: 37972 RVA: 0x002B919E File Offset: 0x002B739E
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (parentDef.tickerType != TickerType.Normal)
			{
				yield return "CompHibernatable needs tickerType " + TickerType.Normal.ToString() + ", has " + parentDef.tickerType.ToString();
			}
			yield break;
			yield break;
		}

		// Token: 0x04005FF4 RID: 24564
		public float startupDays = 14f;

		// Token: 0x04005FF5 RID: 24565
		public IncidentTargetTagDef incidentTargetWhileStarting;

		// Token: 0x04005FF6 RID: 24566
		public SoundDef sustainerActive;
	}
}
