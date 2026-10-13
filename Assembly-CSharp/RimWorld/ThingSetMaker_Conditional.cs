using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B5A RID: 11098
	public abstract class ThingSetMaker_Conditional : ThingSetMaker
	{
		// Token: 0x0600F82E RID: 63534 RVA: 0x004831A4 File Offset: 0x004813A4
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			return this.Condition(parms) && this.thingSetMaker.CanGenerate(parms);
		}

		// Token: 0x0600F82F RID: 63535 RVA: 0x004831BD File Offset: 0x004813BD
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			outThings.AddRange(this.thingSetMaker.Generate(parms));
		}

		// Token: 0x0600F830 RID: 63536 RVA: 0x004831D1 File Offset: 0x004813D1
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return this.thingSetMaker.AllGeneratableThingsDebug(parms);
		}

		// Token: 0x0600F831 RID: 63537 RVA: 0x004831DF File Offset: 0x004813DF
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			this.thingSetMaker.ResolveReferences();
		}

		// Token: 0x0600F832 RID: 63538
		protected abstract bool Condition(ThingSetMakerParams parms);

		// Token: 0x0600F833 RID: 63539 RVA: 0x004831F2 File Offset: 0x004813F2
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.thingSetMaker == null)
			{
				yield return "thingSetMaker is null.";
			}
			else
			{
				foreach (string text in this.thingSetMaker.ConfigErrors())
				{
					yield return text;
				}
				IEnumerator<string> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0400A332 RID: 41778
		public ThingSetMaker thingSetMaker;
	}
}
