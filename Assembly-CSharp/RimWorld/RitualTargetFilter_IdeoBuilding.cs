using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020027D6 RID: 10198
	public class RitualTargetFilter_IdeoBuilding : RitualTargetFilter
	{
		// Token: 0x0600DF37 RID: 57143 RVA: 0x004195FF File Offset: 0x004177FF
		public RitualTargetFilter_IdeoBuilding()
		{
		}

		// Token: 0x0600DF38 RID: 57144 RVA: 0x00419612 File Offset: 0x00417812
		public RitualTargetFilter_IdeoBuilding(RitualTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x0600DF39 RID: 57145 RVA: 0x00419628 File Offset: 0x00417828
		public override bool CanStart(TargetInfo initiator, TargetInfo selectedTarget, out string rejectionReason)
		{
			TargetInfo targetInfo = this.BestTarget(initiator, selectedTarget);
			rejectionReason = "";
			if (!targetInfo.IsValid)
			{
				rejectionReason = "AbilitySpeechDisabledNoSpot".Translate();
				return false;
			}
			return true;
		}

		// Token: 0x0600DF3A RID: 57146 RVA: 0x00419664 File Offset: 0x00417864
		public override TargetInfo BestTarget(TargetInfo initiator, TargetInfo selectedTarget)
		{
			Pawn pawn = (Pawn)initiator.Thing;
			Ideo ideo = pawn.Ideo;
			this.candidateSpots.Clear();
			for (int i = 0; i < ideo.PreceptsListForReading.Count; i++)
			{
				Precept_Building precept_Building = ideo.PreceptsListForReading[i] as Precept_Building;
				if (precept_Building != null)
				{
					Thing thing = precept_Building.presenceDemand.BestBuilding(pawn.Map, false);
					if (thing != null && pawn.CanReach(thing, PathEndMode.Touch, pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn))
					{
						this.candidateSpots.Add(thing);
					}
				}
			}
			this.candidateSpots.AddRange(this.ExtraCandidates(initiator));
			if (!this.candidateSpots.NullOrEmpty<Thing>())
			{
				return this.candidateSpots.RandomElement<Thing>();
			}
			if (!this.def.fallBackToGatherSpot)
			{
				return TargetInfo.Invalid;
			}
			this.candidateSpots.Clear();
			for (int j = 0; j < pawn.Map.gatherSpotLister.activeSpots.Count; j++)
			{
				ThingWithComps parent = pawn.Map.gatherSpotLister.activeSpots[j].parent;
				if (pawn.CanReach(parent, PathEndMode.Touch, pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn))
				{
					this.candidateSpots.Add(parent);
				}
			}
			if (!this.candidateSpots.NullOrEmpty<Thing>())
			{
				return (from s in this.candidateSpots
				orderby s.Position.DistanceTo(pawn.Position)
				select s).First<Thing>();
			}
			return TargetInfo.Invalid;
		}

		// Token: 0x0600DF3B RID: 57147 RVA: 0x00419816 File Offset: 0x00417A16
		protected virtual IEnumerable<Thing> ExtraCandidates(TargetInfo initiator)
		{
			return Enumerable.Empty<Thing>();
		}

		// Token: 0x0600DF3C RID: 57148 RVA: 0x0041981D File Offset: 0x00417A1D
		public override IEnumerable<string> GetTargetInfos(TargetInfo initiator)
		{
			Ideo ideo = ((Pawn)initiator.Thing).Ideo;
			foreach (Precept_Building precept_Building in ideo.cachedPossibleBuildings)
			{
				yield return precept_Building.LabelCap;
			}
			HashSet<Precept_Building>.Enumerator enumerator = default(HashSet<Precept_Building>.Enumerator);
			yield return "RitualTargetGatherSpotInfo".Translate();
			yield break;
			yield break;
		}

		// Token: 0x04009646 RID: 38470
		private List<Thing> candidateSpots = new List<Thing>();
	}
}
