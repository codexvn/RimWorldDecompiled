using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002640 RID: 9792
	public class PreceptComp_SituationalThought : PreceptComp_Thought
	{
		// Token: 0x170020CE RID: 8398
		// (get) Token: 0x0600D688 RID: 54920 RVA: 0x003FA1CB File Offset: 0x003F83CB
		public override IEnumerable<TraitRequirement> TraitsAffecting
		{
			get
			{
				return ThoughtUtility.GetNullifyingTraits(this.thought);
			}
		}
	}
}
