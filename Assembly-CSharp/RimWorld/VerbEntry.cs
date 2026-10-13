using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020024C8 RID: 9416
	public struct VerbEntry
	{
		// Token: 0x17001F4F RID: 8015
		// (get) Token: 0x0600CD05 RID: 52485 RVA: 0x003C21CC File Offset: 0x003C03CC
		public bool IsMeleeAttack
		{
			get
			{
				return this.verb.IsMeleeAttack;
			}
		}

		// Token: 0x0600CD06 RID: 52486 RVA: 0x003C21D9 File Offset: 0x003C03D9
		public VerbEntry(Verb verb, Pawn pawn, List<Verb> allVerbs, float highestSelWeight)
		{
			this.verb = verb;
			this.cachedSelectionWeight = VerbUtility.FinalSelectionWeight(verb, pawn, allVerbs, highestSelWeight);
		}

		// Token: 0x0600CD07 RID: 52487 RVA: 0x003C21F4 File Offset: 0x003C03F4
		public float GetSelectionWeight(Thing target)
		{
			if (!this.verb.IsUsableOn(target))
			{
				return 0f;
			}
			float num = this.cachedSelectionWeight;
			bool? flag;
			if (target == null)
			{
				flag = null;
			}
			else
			{
				ThingDef def = target.def;
				flag = ((def != null) ? new bool?(def.IsEdifice()) : null);
			}
			bool? flag2 = flag;
			if (flag2.GetValueOrDefault())
			{
				num *= this.verb.verbProps.commonalityVsEdificeFactor;
			}
			return num;
		}

		// Token: 0x0600CD08 RID: 52488 RVA: 0x003C2267 File Offset: 0x003C0467
		public override string ToString()
		{
			Verb verb = this.verb;
			return ((verb != null) ? verb.ToString() : null) + " - " + this.cachedSelectionWeight.ToString();
		}

		// Token: 0x04008BA3 RID: 35747
		public Verb verb;

		// Token: 0x04008BA4 RID: 35748
		private float cachedSelectionWeight;
	}
}
