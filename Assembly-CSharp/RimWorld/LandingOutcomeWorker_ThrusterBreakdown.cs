using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020025B5 RID: 9653
	public class LandingOutcomeWorker_ThrusterBreakdown : LandingOutcomeWorker
	{
		// Token: 0x0600D380 RID: 54144 RVA: 0x003E551B File Offset: 0x003E371B
		public LandingOutcomeWorker_ThrusterBreakdown(LandingOutcomeDef def) : base(def)
		{
		}

		// Token: 0x0600D381 RID: 54145 RVA: 0x003E56BC File Offset: 0x003E38BC
		public override void ApplyOutcome(Gravship gravship)
		{
			List<ThingWithComps> list = (from comp in gravship.Engine.GravshipComponents
			where comp.parent.HasComp<CompGravshipThruster>()
			select comp.parent).ToList<ThingWithComps>();
			list = list.InRandomOrder(null).Take(Rand.Range(Mathf.Min(2, list.Count), Mathf.Min(4, list.Count))).ToList<ThingWithComps>();
			using (List<ThingWithComps>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CompBreakdownable compBreakdownable;
					if (enumerator.Current.TryGetComp(out compBreakdownable))
					{
						compBreakdownable.DoBreakdown();
					}
				}
			}
			TaggedString taggedString = "ThrustersBrokeDown".Translate() + ":\n" + (from comp in list
			select comp.LabelCap).ToLineList(" - ", false);
			base.SendStandardLetter(gravship.Engine, taggedString, new LookTargets(list));
		}
	}
}
