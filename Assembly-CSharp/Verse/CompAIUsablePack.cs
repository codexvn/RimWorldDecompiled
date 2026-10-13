using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020008F5 RID: 2293
	public abstract class CompAIUsablePack : ThingComp
	{
		// Token: 0x17000B01 RID: 2817
		// (get) Token: 0x06003B3D RID: 15165 RVA: 0x001398EC File Offset: 0x00137AEC
		protected CompProperties_AIUSablePack Props
		{
			get
			{
				return (CompProperties_AIUSablePack)this.props;
			}
		}

		// Token: 0x06003B3E RID: 15166 RVA: 0x001398FC File Offset: 0x00137AFC
		public override void CompTick()
		{
			Pawn wearer = ((Apparel)this.parent).Wearer;
			if (this.CanOpportunisticallyUseNow(wearer))
			{
				this.UsePack(wearer);
			}
		}

		// Token: 0x06003B3F RID: 15167 RVA: 0x0013992C File Offset: 0x00137B2C
		private bool CanOpportunisticallyUseNow(Pawn wearer)
		{
			return wearer != null && !wearer.Dead && wearer.Spawned && wearer.IsHashIntervalTick(this.Props.checkInterval) && !wearer.Downed && wearer.Awake() && !wearer.IsColonistPlayerControlled && !wearer.InMentalState && Rand.Value < this.ChanceToUse(wearer);
		}

		// Token: 0x06003B40 RID: 15168
		protected abstract float ChanceToUse(Pawn wearer);

		// Token: 0x06003B41 RID: 15169
		protected abstract void UsePack(Pawn wearer);
	}
}
