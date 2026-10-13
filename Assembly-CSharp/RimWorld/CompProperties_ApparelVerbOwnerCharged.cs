using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BE5 RID: 11237
	public class CompProperties_ApparelVerbOwnerCharged : CompProperties_ApparelVerbOwner
	{
		// Token: 0x1700277E RID: 10110
		// (get) Token: 0x0600FB8F RID: 64399 RVA: 0x00490B05 File Offset: 0x0048ED05
		public NamedArgument ChargeNounArgument
		{
			get
			{
				return this.chargeNoun.Named("CHARGENOUN");
			}
		}

		// Token: 0x0600FB90 RID: 64400 RVA: 0x00490B17 File Offset: 0x0048ED17
		public CompProperties_ApparelVerbOwnerCharged()
		{
			this.compClass = typeof(CompApparelVerbOwner_Charged);
		}

		// Token: 0x0400A50E RID: 42254
		public int maxCharges;

		// Token: 0x0400A50F RID: 42255
		public bool destroyOnEmpty;

		// Token: 0x0400A510 RID: 42256
		[MustTranslate]
		public string chargeNoun = "charge";
	}
}
