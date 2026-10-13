using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200225E RID: 8798
	public class CompAbilityEffect_RandomAlliedMilitaryAid : CompAbilityEffect_WithDest
	{
		// Token: 0x17001D4D RID: 7501
		// (get) Token: 0x0600BFD8 RID: 49112 RVA: 0x003799CA File Offset: 0x00377BCA
		public new CompProperties_RandomAlliedMilitaryAid Props
		{
			get
			{
				return (CompProperties_RandomAlliedMilitaryAid)this.props;
			}
		}

		// Token: 0x0600BFD9 RID: 49113 RVA: 0x003799D8 File Offset: 0x00377BD8
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = this.parent.pawn.Map;
			incidentParms.faction = this.GetRandomAlliedFaction();
			incidentParms.raidArrivalModeForQuickMilitaryAid = true;
			incidentParms.points = this.Props.points;
			incidentParms.spawnCenter = target.Cell;
			if (!IncidentDefOf.RaidFriendly.Worker.TryExecute(incidentParms))
			{
				Log.Error("Failed to request military aid");
			}
		}

		// Token: 0x0600BFDA RID: 49114 RVA: 0x00379A56 File Offset: 0x00377C56
		private Faction GetRandomAlliedFaction()
		{
			return Find.FactionManager.RandomAlliedFaction(true, false, true, TechLevel.Undefined);
		}

		// Token: 0x0600BFDB RID: 49115 RVA: 0x00379A66 File Offset: 0x00377C66
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return this.GetRandomAlliedFaction() != null && base.CanApplyOn(target, dest);
		}
	}
}
