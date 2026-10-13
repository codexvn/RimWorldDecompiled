using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020037DC RID: 14300
	public class GameComponent_PsychicRitualManager : GameComponent
	{
		// Token: 0x06013D09 RID: 81161 RVA: 0x005E16AC File Offset: 0x005DF8AC
		public GameComponent_PsychicRitualManager(Game game)
		{
		}

		// Token: 0x06013D0A RID: 81162 RVA: 0x005E16CC File Offset: 0x005DF8CC
		public override void GameComponentTick()
		{
			if (!ModsConfig.AnomalyActive)
			{
				return;
			}
			this.availableRitualDefs = (from cd in this.ritualCooldowns
			where cd.Value <= Find.TickManager.TicksGame
			select cd).ToList<KeyValuePair<PsychicRitualDef, int>>();
			foreach (KeyValuePair<PsychicRitualDef, int> keyValuePair in this.availableRitualDefs)
			{
				this.ritualCooldowns.Remove(keyValuePair.Key);
				if (keyValuePair.Key.researchPrerequisite != null && keyValuePair.Key.researchPrerequisite.IsFinished)
				{
					Messages.Message("PsychicRitualCanBeCastAgain".Translate(keyValuePair.Key.label), MessageTypeDefOf.PositiveEvent, true);
				}
			}
		}

		// Token: 0x06013D0B RID: 81163 RVA: 0x005E17B4 File Offset: 0x005DF9B4
		public void RegisterCooldown(PsychicRitualDef psychicRitualDef)
		{
			this.ritualCooldowns[psychicRitualDef] = Find.TickManager.TicksGame + psychicRitualDef.cooldownHours * 2500;
		}

		// Token: 0x06013D0C RID: 81164 RVA: 0x005E17D9 File Offset: 0x005DF9D9
		public void ClearCooldown(PsychicRitualDef psychicRitualDef)
		{
			this.ritualCooldowns.Remove(psychicRitualDef);
		}

		// Token: 0x06013D0D RID: 81165 RVA: 0x005E17E8 File Offset: 0x005DF9E8
		public int GetAvailableTick(PsychicRitualDef psychicRitualDef)
		{
			if (!this.ritualCooldowns.ContainsKey(psychicRitualDef))
			{
				return Find.TickManager.TicksGame;
			}
			return this.ritualCooldowns[psychicRitualDef];
		}

		// Token: 0x06013D0E RID: 81166 RVA: 0x005E1810 File Offset: 0x005DFA10
		public AcceptanceReport CanInvoke(PsychicRitualDef psychicRitualDef, Map map)
		{
			if (!psychicRitualDef.castableOnPocketMaps && map.IsPocketMap)
			{
				return new AcceptanceReport("CantCastOnPocketMap".Translate(psychicRitualDef.label).Resolve());
			}
			if (map.Tile.Valid && !psychicRitualDef.layerWhitelist.NullOrEmpty<PlanetLayerDef>() && !psychicRitualDef.layerWhitelist.Contains(map.Tile.LayerDef))
			{
				return new AcceptanceReport("CannotPerformPlanetLayer".Translate(map.Tile.LayerDef.gerundLabel.Named("GERUND"), map.Tile.LayerDef.label.Named("LAYER")).EndWithPeriod().Resolve());
			}
			int num;
			if (!this.ritualCooldowns.TryGetValue(psychicRitualDef, out num))
			{
				return true;
			}
			if (Find.TickManager.TicksGame >= num)
			{
				return true;
			}
			return new AcceptanceReport("PsychicRitualOnCooldown".Translate((this.GetAvailableTick(psychicRitualDef) - Find.TickManager.TicksGame).ToStringTicksToPeriod(true, false, true, true, false)).Resolve());
		}

		// Token: 0x06013D0F RID: 81167 RVA: 0x005E1948 File Offset: 0x005DFB48
		public void ClearAllCooldowns()
		{
			this.ritualCooldowns.Clear();
		}

		// Token: 0x06013D10 RID: 81168 RVA: 0x005E1958 File Offset: 0x005DFB58
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<PsychicRitualDef, int>(ref this.ritualCooldowns, "ritualCooldowns", LookMode.Def, LookMode.Value, ref this.tmpRitualDefs, ref this.tmpCooldowns, true, false, false);
		}

		// Token: 0x0400DB75 RID: 56181
		private Dictionary<PsychicRitualDef, int> ritualCooldowns = new Dictionary<PsychicRitualDef, int>();

		// Token: 0x0400DB76 RID: 56182
		private List<KeyValuePair<PsychicRitualDef, int>> availableRitualDefs = new List<KeyValuePair<PsychicRitualDef, int>>();

		// Token: 0x0400DB77 RID: 56183
		private List<PsychicRitualDef> tmpRitualDefs;

		// Token: 0x0400DB78 RID: 56184
		private List<int> tmpCooldowns;
	}
}
