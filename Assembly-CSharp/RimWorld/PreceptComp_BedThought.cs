using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002636 RID: 9782
	public class PreceptComp_BedThought : PreceptComp_Thought
	{
		// Token: 0x0600D65B RID: 54875 RVA: 0x003F9C8C File Offset: 0x003F7E8C
		public override void Notify_AddBedThoughts(Pawn pawn, Precept precept)
		{
			base.Notify_AddBedThoughts(pawn, precept);
			pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDef(this.thought);
			if (this.requireFacility != null && !this.HasActiveLinkedFacility(pawn))
			{
				return;
			}
			Thought_Memory newThought = ThoughtMaker.MakeThought(this.thought, precept);
			pawn.needs.mood.thoughts.memories.TryGainMemory(newThought, null);
		}

		// Token: 0x0600D65C RID: 54876 RVA: 0x003F9CFC File Offset: 0x003F7EFC
		private bool HasActiveLinkedFacility(Pawn pawn)
		{
			Building_Bed building_Bed = pawn.CurrentBed();
			CompAffectedByFacilities compAffectedByFacilities = (building_Bed != null) ? building_Bed.TryGetComp<CompAffectedByFacilities>() : null;
			if (compAffectedByFacilities == null)
			{
				return false;
			}
			foreach (Thing thing in compAffectedByFacilities.LinkedFacilitiesListForReading)
			{
				if (thing.def == this.requireFacility.def && compAffectedByFacilities.IsFacilityActive(thing))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600D65D RID: 54877 RVA: 0x003F9D84 File Offset: 0x003F7F84
		public override IEnumerable<string> ConfigErrors(PreceptDef parent)
		{
			foreach (string text in base.ConfigErrors(parent))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.requireFacility != null && this.requireFacility.def == null)
			{
				yield return "<requireFacility> has null <def>";
			}
			yield break;
			yield break;
		}

		// Token: 0x040091B5 RID: 37301
		public PreceptComp_BedThought.FacilityData requireFacility;

		// Token: 0x02002637 RID: 9783
		public class FacilityData
		{
			// Token: 0x040091B6 RID: 37302
			public ThingDef def;
		}
	}
}
