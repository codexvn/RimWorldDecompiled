using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F26 RID: 12070
	public class CompTargetable_SingleCorpse : CompTargetable
	{
		// Token: 0x17002C08 RID: 11272
		// (get) Token: 0x060110AA RID: 69802 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060110AB RID: 69803 RVA: 0x004E6563 File Offset: 0x004E4763
		protected override TargetingParameters GetTargetingParameters()
		{
			return new TargetingParameters
			{
				canTargetPawns = false,
				canTargetBuildings = false,
				canTargetItems = false,
				canTargetCorpses = true,
				mapObjectTargetsMustBeAutoAttackable = false
			};
		}

		// Token: 0x060110AC RID: 69804 RVA: 0x004E658D File Offset: 0x004E478D
		public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
		{
			yield return targetChosenByPlayer;
			yield break;
		}

		// Token: 0x060110AD RID: 69805 RVA: 0x004E659D File Offset: 0x004E479D
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			return target.Thing is Corpse && base.ValidateTarget(target.Thing, showMessages);
		}
	}
}
