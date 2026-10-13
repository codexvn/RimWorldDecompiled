using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200161D RID: 5661
	public abstract class WorkGiver_EntityOnPlatform : WorkGiver_Scanner
	{
		// Token: 0x17001480 RID: 5248
		// (get) Token: 0x0600858C RID: 34188 RVA: 0x0027D86A File Offset: 0x0027BA6A
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.EntityHolder);
			}
		}

		// Token: 0x0600858D RID: 34189 RVA: 0x0027D860 File Offset: 0x0027BA60
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !ModsConfig.AnomalyActive;
		}

		// Token: 0x0600858E RID: 34190 RVA: 0x0027E824 File Offset: 0x0027CA24
		public override string PostProcessedGerund(Job job)
		{
			Pawn entity = this.GetEntity(job.targetA.Thing);
			return "DoWorkAtThing".Translate(this.def.gerund.Named("GERUND"), entity.LabelShort.Named("TARGETLABEL"));
		}

		// Token: 0x0600858F RID: 34191
		protected abstract Pawn GetEntity(Thing potentialPlatform);
	}
}
