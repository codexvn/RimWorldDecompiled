using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D21 RID: 15649
	public class CaravanArrivalAction_Enter : CaravanArrivalAction
	{
		// Token: 0x17003519 RID: 13593
		// (get) Token: 0x060154CA RID: 87242 RVA: 0x0065EC78 File Offset: 0x0065CE78
		public override string Label
		{
			get
			{
				return "EnterMap".Translate(this.mapParent.Label);
			}
		}

		// Token: 0x1700351A RID: 13594
		// (get) Token: 0x060154CB RID: 87243 RVA: 0x0065EC99 File Offset: 0x0065CE99
		public override string ReportString
		{
			get
			{
				return "CaravanEntering".Translate(this.mapParent.Label);
			}
		}

		// Token: 0x060154CC RID: 87244 RVA: 0x0065EA59 File Offset: 0x0065CC59
		public CaravanArrivalAction_Enter()
		{
		}

		// Token: 0x060154CD RID: 87245 RVA: 0x0065ECBA File Offset: 0x0065CEBA
		public CaravanArrivalAction_Enter(MapParent mapParent)
		{
			this.mapParent = mapParent;
		}

		// Token: 0x060154CE RID: 87246 RVA: 0x0065ECCC File Offset: 0x0065CECC
		public override FloatMenuAcceptanceReport StillValid(Caravan caravan, PlanetTile destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(caravan, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.mapParent != null && this.mapParent.Tile != destinationTile)
			{
				return false;
			}
			return CaravanArrivalAction_Enter.CanEnter(caravan, this.mapParent);
		}

		// Token: 0x060154CF RID: 87247 RVA: 0x0065ED1C File Offset: 0x0065CF1C
		public override void Arrived(Caravan caravan)
		{
			Map map = this.mapParent.Map;
			if (map == null)
			{
				return;
			}
			CaravanDropInventoryMode dropInventoryMode = map.IsPlayerHome ? CaravanDropInventoryMode.UnloadIndividually : CaravanDropInventoryMode.DoNotDrop;
			bool draftColonists = this.mapParent.Faction != null && this.mapParent.Faction.HostileTo(Faction.OfPlayer);
			if (caravan.IsPlayerControlled || this.mapParent.Faction == Faction.OfPlayer)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelCaravanEnteredMap".Translate(this.mapParent), "LetterCaravanEnteredMap".Translate(caravan.Label, this.mapParent).CapitalizeFirst(), LetterDefOf.NeutralEvent, caravan.PawnsListForReading, null, null, null, null, 0, true);
			}
			CaravanEnterMapUtility.Enter(caravan, map, CaravanEnterMode.Edge, dropInventoryMode, draftColonists, null);
		}

		// Token: 0x060154D0 RID: 87248 RVA: 0x0065EDEE File Offset: 0x0065CFEE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
		}

		// Token: 0x060154D1 RID: 87249 RVA: 0x0065EE08 File Offset: 0x0065D008
		public static FloatMenuAcceptanceReport CanEnter(Caravan caravan, MapParent mapParent)
		{
			if (mapParent == null || !mapParent.Spawned || !mapParent.HasMap)
			{
				return false;
			}
			if (mapParent.EnterCooldownBlocksEntering())
			{
				return FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(mapParent.EnterCooldownTicksLeft().ToStringTicksToPeriod(true, false, true, true, false)));
			}
			return true;
		}

		// Token: 0x060154D2 RID: 87250 RVA: 0x0065EE68 File Offset: 0x0065D068
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, MapParent mapParent)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_Enter>(() => CaravanArrivalAction_Enter.CanEnter(caravan, mapParent), () => new CaravanArrivalAction_Enter(mapParent), "EnterMap".Translate(mapParent.Label), caravan, mapParent.Tile, mapParent, null);
		}

		// Token: 0x0400ED7C RID: 60796
		private MapParent mapParent;
	}
}
