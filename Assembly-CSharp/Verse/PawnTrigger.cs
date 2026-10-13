using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200087B RID: 2171
	public abstract class PawnTrigger : Thing
	{
		// Token: 0x06003711 RID: 14097 RVA: 0x00127556 File Offset: 0x00125756
		protected bool TriggeredBy(Thing thing)
		{
			return thing.def.category == ThingCategory.Pawn && thing.def.race.intelligence == Intelligence.Humanlike && thing.Faction == Faction.OfPlayer;
		}

		// Token: 0x06003712 RID: 14098 RVA: 0x00127588 File Offset: 0x00125788
		public void ActivatedBy(Pawn p)
		{
			Find.SignalManager.SendSignal(new Signal(this.signalTag, p.Named("SUBJECT")));
			if (!base.Destroyed)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06003713 RID: 14099 RVA: 0x001275B9 File Offset: 0x001257B9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
		}

		// Token: 0x04002950 RID: 10576
		public string signalTag;
	}
}
