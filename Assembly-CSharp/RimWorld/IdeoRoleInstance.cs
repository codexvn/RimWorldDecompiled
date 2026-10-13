using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002808 RID: 10248
	public class IdeoRoleInstance : IExposable
	{
		// Token: 0x0600E012 RID: 57362 RVA: 0x0041C994 File Offset: 0x0041AB94
		public IdeoRoleInstance(Precept_Role sourceRole)
		{
			this.sourceRole = sourceRole;
		}

		// Token: 0x0600E013 RID: 57363 RVA: 0x0041C9A4 File Offset: 0x0041ABA4
		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_Collections.Look<Ability>(ref this.abilities, "abilities", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.abilities != null)
			{
				foreach (Ability ability in this.abilities)
				{
					ability.pawn = this.pawn;
					ability.verb.caster = this.pawn;
					ability.sourcePrecept = this.sourceRole;
				}
			}
		}

		// Token: 0x040096C4 RID: 38596
		public Precept sourceRole;

		// Token: 0x040096C5 RID: 38597
		public Pawn pawn;

		// Token: 0x040096C6 RID: 38598
		public List<Ability> abilities;
	}
}
