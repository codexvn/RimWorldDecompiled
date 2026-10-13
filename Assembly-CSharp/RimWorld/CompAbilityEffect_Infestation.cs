using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002248 RID: 8776
	public class CompAbilityEffect_Infestation : CompAbilityEffect_WithDest
	{
		// Token: 0x17001D41 RID: 7489
		// (get) Token: 0x0600BF97 RID: 49047 RVA: 0x00378ABF File Offset: 0x00376CBF
		public new CompProperties_Infestation Props
		{
			get
			{
				return (CompProperties_Infestation)this.props;
			}
		}

		// Token: 0x0600BF98 RID: 49048 RVA: 0x00378ACC File Offset: 0x00376CCC
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = this.parent.pawn.Map;
			incidentParms.points = this.Props.points;
			incidentParms.infestationLocOverride = new IntVec3?(target.Cell);
			incidentParms.forced = true;
			IncidentDefOf.Infestation.Worker.TryExecute(incidentParms);
		}
	}
}
