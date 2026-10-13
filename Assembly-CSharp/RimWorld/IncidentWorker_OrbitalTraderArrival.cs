using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DBB RID: 7611
	public class IncidentWorker_OrbitalTraderArrival : IncidentWorker
	{
		// Token: 0x0600A9FA RID: 43514 RVA: 0x0030A18B File Offset: 0x0030838B
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return base.CanFireNowSub(parms) && ((Map)parms.target).passingShipManager.passingShips.Count < 5;
		}

		// Token: 0x0600A9FB RID: 43515 RVA: 0x0030A1B8 File Offset: 0x003083B8
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (parms.traderKind == null)
			{
				if (!(from x in DefDatabase<TraderKindDef>.AllDefs
				where this.CanSpawn(map, x)
				select x).TryRandomElementByWeight((TraderKindDef traderDef) => traderDef.CalculatedCommonality, out parms.traderKind))
				{
					return false;
				}
			}
			TradeShip tradeShip = new TradeShip(parms.traderKind, this.GetFaction(parms.traderKind));
			tradeShip.WasAnnounced = false;
			if (map.listerBuildings.allBuildingsColonist.Any((Building b) => b.def.IsCommsConsole && (b.GetComp<CompPowerTrader>() == null || b.GetComp<CompPowerTrader>().PowerOn)))
			{
				base.SendStandardLetter(tradeShip.def.LabelCap, "TraderArrival".Translate(tradeShip.name, tradeShip.def.label, (tradeShip.Faction == null) ? "TraderArrivalNoFaction".Translate() : "TraderArrivalFromFaction".Translate(tradeShip.Faction.Named("FACTION"))), LetterDefOf.PositiveEvent, parms, LookTargets.Invalid, Array.Empty<NamedArgument>());
				tradeShip.WasAnnounced = true;
			}
			map.passingShipManager.AddShip(tradeShip);
			tradeShip.GenerateThings();
			return true;
		}

		// Token: 0x0600A9FC RID: 43516 RVA: 0x0030A320 File Offset: 0x00308520
		private Faction GetFaction(TraderKindDef trader)
		{
			if (trader.faction == null)
			{
				return null;
			}
			Faction result;
			if (!(from f in Find.FactionManager.AllFactions
			where f.def == trader.faction
			select f).TryRandomElement(out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x0600A9FD RID: 43517 RVA: 0x0030A370 File Offset: 0x00308570
		private bool CanSpawn(Map map, TraderKindDef trader)
		{
			if (!trader.orbital)
			{
				return false;
			}
			if (trader.faction == null)
			{
				return true;
			}
			Faction faction = this.GetFaction(trader);
			if (faction == null)
			{
				return false;
			}
			using (List<Pawn>.Enumerator enumerator = map.mapPawns.FreeColonists.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.CanTradeWith(faction, trader).Accepted)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04007563 RID: 30051
		private const int MaxShips = 5;
	}
}
