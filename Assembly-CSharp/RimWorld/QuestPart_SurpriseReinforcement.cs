using System;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CC0 RID: 7360
	public class QuestPart_SurpriseReinforcement : QuestPartActivable
	{
		// Token: 0x0600A3FD RID: 41981 RVA: 0x002F07AC File Offset: 0x002EE9AC
		public override void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
			if (this.reinforcementChance == 0f)
			{
				return;
			}
			if (pawn.Faction == this.faction && pawn.MapHeld != null && pawn.MapHeld.Parent != null && pawn.MapHeld.Parent == this.mapParent)
			{
				if (Rand.Chance(this.reinforcementChance))
				{
					this.ticksTillRaid = (int)QuestPart_SurpriseReinforcement.RandomDelayRange.RandomInRange;
				}
				this.reinforcementChance = 0f;
			}
		}

		// Token: 0x0600A3FE RID: 41982 RVA: 0x002F082C File Offset: 0x002EEA2C
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (this.ticksTillRaid > -1)
			{
				this.ticksTillRaid--;
				if (this.ticksTillRaid == 0)
				{
					IncidentParms incidentParms = new IncidentParms();
					incidentParms.target = this.mapParent.Map;
					incidentParms.points = StorytellerUtility.DefaultThreatPointsNow(incidentParms.target) * 2.5f;
					incidentParms.faction = this.faction;
					incidentParms.customLetterLabel = "LetterLabelSurpriseReinforcements".Translate();
					incidentParms.customLetterText = "LetterSurpriseReinforcements".Translate(incidentParms.faction.def.pawnsPlural, incidentParms.faction).Resolve();
					IncidentDefOf.RaidEnemy.Worker.TryExecute(incidentParms);
				}
			}
		}

		// Token: 0x0600A3FF RID: 41983 RVA: 0x002F08FC File Offset: 0x002EEAFC
		public override bool QuestPartReserves(Faction f)
		{
			return f == this.faction;
		}

		// Token: 0x0600A400 RID: 41984 RVA: 0x002F0907 File Offset: 0x002EEB07
		public override void Notify_FactionRemoved(Faction removedFaction)
		{
			if (removedFaction == this.faction)
			{
				this.faction = null;
			}
		}

		// Token: 0x0600A401 RID: 41985 RVA: 0x002F091C File Offset: 0x002EEB1C
		public override void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving && this.faction != null && !Find.FactionManager.AllFactions.Contains(this.faction))
			{
				this.faction = null;
			}
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<float>(ref this.reinforcementChance, "reinforcementChance", 0f, false);
			Scribe_Values.Look<int>(ref this.ticksTillRaid, "ticksTillRaid", -1, false);
		}

		// Token: 0x04007146 RID: 28998
		public MapParent mapParent;

		// Token: 0x04007147 RID: 28999
		public Faction faction;

		// Token: 0x04007148 RID: 29000
		public float reinforcementChance;

		// Token: 0x04007149 RID: 29001
		private int ticksTillRaid = -1;

		// Token: 0x0400714A RID: 29002
		public const float RaidThreatPointsMultiplier = 2.5f;

		// Token: 0x0400714B RID: 29003
		private static readonly FloatRange RandomDelayRange = new FloatRange(300f, 900f);
	}
}
