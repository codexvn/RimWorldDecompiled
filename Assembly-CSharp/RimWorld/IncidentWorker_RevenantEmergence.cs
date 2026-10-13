using System;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DC7 RID: 7623
	public class IncidentWorker_RevenantEmergence : IncidentWorker
	{
		// Token: 0x0600AA22 RID: 43554 RVA: 0x0030ABB4 File Offset: 0x00308DB4
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			Caravan caravan = parms.target as Caravan;
			if (caravan != null)
			{
				Thing thing;
				Pawn pawn;
				return CaravanInventoryUtility.TryGetThingOfDef(caravan, ThingDefOf.RevenantSpine, out thing, out pawn);
			}
			Map map = parms.target as Map;
			return map != null && map.listerThings.ThingsOfDef(ThingDefOf.RevenantSpine).Count > 0;
		}

		// Token: 0x0600AA23 RID: 43555 RVA: 0x0030AC14 File Offset: 0x00308E14
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Caravan caravan = parms.target as Caravan;
			if (caravan != null)
			{
				Thing thing;
				Pawn pawn;
				CaravanInventoryUtility.TryGetThingOfDef(caravan, ThingDefOf.RevenantSpine, out thing, out pawn);
				if (thing == null)
				{
					return false;
				}
				CompSpawnsRevenant compSpawnsRevenant = thing.TryGetComp<CompSpawnsRevenant>();
				if (compSpawnsRevenant.spawnTick > 0)
				{
					return false;
				}
				compSpawnsRevenant.spawnTick = Find.TickManager.TicksGame + IncidentWorker_RevenantEmergence.EmergenceDelay.RandomInRange;
				IncidentWorker.SendIncidentLetter(this.def.letterLabel, this.def.letterText, LetterDefOf.NegativeEvent, parms, caravan, this.def, Array.Empty<NamedArgument>());
				return true;
			}
			else
			{
				Map map = parms.target as Map;
				if (map == null)
				{
					return false;
				}
				Thing thing2;
				(from s in map.listerThings.ThingsOfDef(ThingDefOf.RevenantSpine)
				where s.TryGetComp<CompSpawnsRevenant>().spawnTick < 0
				select s).TryRandomElement(out thing2);
				if (thing2 == null)
				{
					return false;
				}
				thing2.TryGetComp<CompSpawnsRevenant>().spawnTick = Find.TickManager.TicksGame + IncidentWorker_RevenantEmergence.EmergenceDelay.RandomInRange;
				IncidentWorker.SendIncidentLetter(this.def.letterLabel, this.def.letterText, LetterDefOf.NegativeEvent, parms, thing2, this.def, Array.Empty<NamedArgument>());
				return true;
			}
		}

		// Token: 0x04007570 RID: 30064
		private static readonly IntRange EmergenceDelay = new IntRange(7500, 12500);
	}
}
