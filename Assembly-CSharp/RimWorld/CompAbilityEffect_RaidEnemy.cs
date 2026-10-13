using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200225C RID: 8796
	public class CompAbilityEffect_RaidEnemy : CompAbilityEffect_WithDest
	{
		// Token: 0x17001D4C RID: 7500
		// (get) Token: 0x0600BFD4 RID: 49108 RVA: 0x0037990F File Offset: 0x00377B0F
		public new CompProperties_RaidEnemy Props
		{
			get
			{
				return (CompProperties_RaidEnemy)this.props;
			}
		}

		// Token: 0x0600BFD5 RID: 49109 RVA: 0x0037991C File Offset: 0x00377B1C
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = this.parent.pawn.Map;
			incidentParms.points = this.Props.points;
			incidentParms.faction = Find.FactionManager.FirstFactionOfDef(this.Props.factionDef);
			incidentParms.raidStrategy = this.Props.raidStrategyDef;
			incidentParms.raidArrivalMode = this.Props.pawnArrivalModeDef;
			incidentParms.raidStrategy = this.Props.raidStrategyDef;
			incidentParms.spawnCenter = target.Cell;
			IncidentDefOf.RaidEnemy.Worker.TryExecute(incidentParms);
		}
	}
}
