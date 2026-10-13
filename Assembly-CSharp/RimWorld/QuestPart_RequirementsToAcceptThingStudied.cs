using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C96 RID: 7318
	public class QuestPart_RequirementsToAcceptThingStudied : QuestPart_RequirementsToAccept
	{
		// Token: 0x17001910 RID: 6416
		// (get) Token: 0x0600A308 RID: 41736 RVA: 0x002ED5B2 File Offset: 0x002EB7B2
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				Thing thing = this.thing;
				CompStudiable compStudiable = (thing != null) ? thing.TryGetComp<CompStudiable>() : null;
				if (compStudiable != null && !compStudiable.Completed)
				{
					yield return this.thing;
				}
				yield break;
			}
		}

		// Token: 0x0600A309 RID: 41737 RVA: 0x002ED5C4 File Offset: 0x002EB7C4
		public override AcceptanceReport CanAccept()
		{
			Thing thing = this.thing;
			CompStudiable compStudiable = (thing != null) ? thing.TryGetComp<CompStudiable>() : null;
			if (compStudiable != null && !compStudiable.Completed)
			{
				return new AcceptanceReport("QuestRequiredThingStudied".Translate(this.thing));
			}
			return true;
		}

		// Token: 0x0600A30A RID: 41738 RVA: 0x002ED615 File Offset: 0x002EB815
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.thing, "thing", false);
		}

		// Token: 0x040070B5 RID: 28853
		public Thing thing;
	}
}
