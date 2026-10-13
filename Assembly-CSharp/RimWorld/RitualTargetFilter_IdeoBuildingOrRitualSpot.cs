using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020027D9 RID: 10201
	public class RitualTargetFilter_IdeoBuildingOrRitualSpot : RitualTargetFilter_IdeoBuilding
	{
		// Token: 0x0600DF48 RID: 57160 RVA: 0x004199FB File Offset: 0x00417BFB
		public RitualTargetFilter_IdeoBuildingOrRitualSpot()
		{
		}

		// Token: 0x0600DF49 RID: 57161 RVA: 0x00419A03 File Offset: 0x00417C03
		public RitualTargetFilter_IdeoBuildingOrRitualSpot(RitualTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x0600DF4A RID: 57162 RVA: 0x00419A0C File Offset: 0x00417C0C
		protected override IEnumerable<Thing> ExtraCandidates(TargetInfo initiator)
		{
			Pawn pawn = (Pawn)initiator.Thing;
			return from s in initiator.Map.listerThings.ThingsOfDef(ThingDefOf.RitualSpot)
			where pawn.CanReach(s, PathEndMode.Touch, pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn)
			select s;
		}

		// Token: 0x0600DF4B RID: 57163 RVA: 0x00419A58 File Offset: 0x00417C58
		public override IEnumerable<string> GetTargetInfos(TargetInfo initiator)
		{
			foreach (string text in base.GetTargetInfos(initiator))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			yield return ThingDefOf.RitualSpot.LabelCap;
			yield break;
			yield break;
		}
	}
}
