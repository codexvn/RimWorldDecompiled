using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020023D1 RID: 9169
	public class RoyalTitlePermitWorker
	{
		// Token: 0x0600C6D0 RID: 50896 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual IEnumerable<Gizmo> GetPawnGizmos(Pawn pawn, Faction faction)
		{
			return null;
		}

		// Token: 0x0600C6D1 RID: 50897 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual IEnumerable<Gizmo> GetCaravanGizmos(Pawn pawn, Faction faction)
		{
			return null;
		}

		// Token: 0x0600C6D2 RID: 50898 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual IEnumerable<DiaOption> GetFactionCommDialogOptions(Map map, Pawn pawn, Faction factionInFavor)
		{
			return null;
		}

		// Token: 0x0600C6D3 RID: 50899 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual IEnumerable<FloatMenuOption> GetRoyalAidOptions(Map map, Pawn pawn, Faction faction)
		{
			return null;
		}

		// Token: 0x0600C6D4 RID: 50900 RVA: 0x0039EE50 File Offset: 0x0039D050
		[Obsolete]
		protected virtual bool AidDisabled(Map map, Pawn pawn, Faction faction, out string reason)
		{
			return this.AidDisabled_NewTemp(map, pawn, faction, out reason, true);
		}

		// Token: 0x0600C6D5 RID: 50901 RVA: 0x0039EE60 File Offset: 0x0039D060
		protected virtual bool AidDisabled_NewTemp(Map map, Pawn pawn, Faction faction, out string reason, bool temperatureMatters = true)
		{
			if (faction.HostileTo(Faction.OfPlayer))
			{
				reason = "CommandCallRoyalAidFactionHostile".Translate(faction.Named("FACTION"));
				return true;
			}
			if (map.generatorDef.isUnderground)
			{
				reason = "CommandCallRoyalAidMapUnreachable".Translate(faction.Named("FACTION"));
				return true;
			}
			if (this.def.layerBlacklist.Contains(pawn.MapHeld.Tile.LayerDef))
			{
				reason = "CommandCallRoyalAidMapUnreachable".Translate(faction.Named("FACTION"));
				return true;
			}
			if (temperatureMatters && !this.TemperatureIsAcceptable(map, faction))
			{
				reason = "BadTemperature".Translate();
				return true;
			}
			reason = null;
			return false;
		}

		// Token: 0x0600C6D6 RID: 50902 RVA: 0x0039EF30 File Offset: 0x0039D130
		protected bool FillAidOption(Pawn pawn, Faction faction, ref string description, out bool free)
		{
			int lastUsedTick = pawn.royalty.GetPermit(this.def, faction).LastUsedTick;
			int num = Math.Max(GenTicks.TicksGame - lastUsedTick, 0);
			if (lastUsedTick < 0 || num >= this.def.CooldownTicks)
			{
				description += "CommandCallRoyalAidFreeOption".Translate();
				free = true;
				return true;
			}
			int numTicks = (lastUsedTick > 0) ? Math.Max(this.def.CooldownTicks - num, 0) : 0;
			description += "CommandCallRoyalAidFavorOption".Translate(numTicks.TicksToDays().ToString("0.0"), this.def.royalAid.favorCost, faction.Named("FACTION"));
			if (pawn.royalty.GetFavor(faction) >= this.def.royalAid.favorCost)
			{
				free = false;
				return true;
			}
			free = false;
			return false;
		}

		// Token: 0x0600C6D7 RID: 50903 RVA: 0x0039F030 File Offset: 0x0039D230
		protected bool FillCaravanAidOption(Pawn pawn, Faction faction, out string description, out bool free, out bool disableNotEnoughFavor)
		{
			description = this.def.description;
			disableNotEnoughFavor = false;
			free = false;
			if (!this.def.usableOnWorldMap)
			{
				free = false;
				return false;
			}
			int lastUsedTick = pawn.royalty.GetPermit(this.def, faction).LastUsedTick;
			int num = Math.Max(GenTicks.TicksGame - lastUsedTick, 0);
			if (lastUsedTick < 0 || num >= this.def.CooldownTicks)
			{
				description += " (" + "CommandCallRoyalAidFreeOption".Translate() + ")";
				free = true;
			}
			else
			{
				int numTicks = (lastUsedTick > 0) ? Math.Max(this.def.CooldownTicks - num, 0) : 0;
				description += " (" + "CommandCallRoyalAidFavorOption".Translate(numTicks.TicksToDays().ToString("0.0"), this.def.royalAid.favorCost, faction.Named("FACTION")) + ")";
				if (pawn.royalty.GetFavor(faction) < this.def.royalAid.favorCost)
				{
					disableNotEnoughFavor = true;
				}
			}
			return true;
		}

		// Token: 0x0600C6D8 RID: 50904 RVA: 0x0039F17C File Offset: 0x0039D37C
		protected virtual bool TemperatureIsAcceptable(Map map, Faction faction)
		{
			return faction.def.allowedArrivalTemperatureRange.ExpandedBy(-4f).Includes(map.mapTemperature.SeasonalTemp);
		}

		// Token: 0x04008776 RID: 34678
		public RoyalTitlePermitDef def;
	}
}
