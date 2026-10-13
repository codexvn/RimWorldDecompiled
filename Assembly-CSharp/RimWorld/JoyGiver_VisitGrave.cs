using System;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014FD RID: 5373
	public class JoyGiver_VisitGrave : JoyGiver
	{
		// Token: 0x06008010 RID: 32784 RVA: 0x00268DB0 File Offset: 0x00266FB0
		public override Job TryGiveJob(Pawn pawn)
		{
			JoyGiver_VisitGrave.<>c__DisplayClass0_0 CS$<>8__locals1 = new JoyGiver_VisitGrave.<>c__DisplayClass0_0();
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.allowedOutside = JoyUtility.EnjoyableOutsideNow(CS$<>8__locals1.pawn, null);
			Thing t;
			if (!CS$<>8__locals1.pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Grave).Where(new Func<Thing, bool>(CS$<>8__locals1.<TryGiveJob>g__Validator|0)).TryRandomElementByWeight(delegate(Thing x)
			{
				float lengthHorizontal = (x.Position - CS$<>8__locals1.pawn.Position).LengthHorizontal;
				return Mathf.Max(150f - lengthHorizontal, 5f);
			}, out t))
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, t);
		}
	}
}
