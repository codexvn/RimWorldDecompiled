using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002648 RID: 9800
	public class PreceptComp_UnwillingToDo_WithDef : PreceptComp_UnwillingToDo
	{
		// Token: 0x0600D6B4 RID: 54964 RVA: 0x003FAEB4 File Offset: 0x003F90B4
		public override bool MemberWillingToDo(HistoryEvent ev)
		{
			if (this.eventDef != null && ev.def != this.eventDef)
			{
				return true;
			}
			Pawn pawn;
			ev.args.TryGetArg<Pawn>(HistoryEventArgsNames.Doer, out pawn);
			ThingDef thingDef;
			ev.args.TryGetArg<ThingDef>(HistoryEventArgsNames.Building, out thingDef);
			if (thingDef == null)
			{
				return true;
			}
			if (this.nullifyingTraits != null && pawn != null && pawn.story != null)
			{
				if (!this.preceptDef.enabledForNPCFactions && !pawn.Faction.IsPlayer)
				{
					return true;
				}
				for (int i = 0; i < this.nullifyingTraits.Count; i++)
				{
					if (this.nullifyingTraits[i].HasTrait(pawn))
					{
						return true;
					}
				}
			}
			return thingDef != this.buildingDef;
		}

		// Token: 0x0600D6B5 RID: 54965 RVA: 0x003FAF6D File Offset: 0x003F916D
		public override IEnumerable<string> GetDescriptions()
		{
			yield return "UnwillingToDoIdeoAction".Translate() + ": " + "BuildThing".Translate(this.buildingDef);
			yield break;
		}

		// Token: 0x040091EB RID: 37355
		public ThingDef buildingDef;
	}
}
