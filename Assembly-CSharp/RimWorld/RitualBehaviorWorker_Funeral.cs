using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020026D3 RID: 9939
	public class RitualBehaviorWorker_Funeral : RitualBehaviorWorker
	{
		// Token: 0x0600D9DF RID: 55775 RVA: 0x00406F23 File Offset: 0x00405123
		public RitualBehaviorWorker_Funeral()
		{
		}

		// Token: 0x0600D9E0 RID: 55776 RVA: 0x00406F2B File Offset: 0x0040512B
		public RitualBehaviorWorker_Funeral(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x0600D9E1 RID: 55777 RVA: 0x004078A0 File Offset: 0x00405AA0
		public override string CanStartRitualNow(TargetInfo target, Precept_Ritual ritual, Pawn selectedPawn = null, Dictionary<string, Pawn> forcedForRole = null)
		{
			if (target.HasThing)
			{
				Building_Grave building_Grave = target.Thing as Building_Grave;
				if (building_Grave != null && building_Grave.Corpse != null && building_Grave.Corpse.InnerPawn.IsSlave)
				{
					return "CantStartFuneralForSlave".Translate(building_Grave.Corpse.InnerPawn);
				}
			}
			return base.CanStartRitualNow(target, ritual, selectedPawn, forcedForRole);
		}
	}
}
