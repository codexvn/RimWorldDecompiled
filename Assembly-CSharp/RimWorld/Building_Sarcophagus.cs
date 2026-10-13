using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A47 RID: 10823
	public class Building_Sarcophagus : Building_Grave
	{
		// Token: 0x0600F057 RID: 61527 RVA: 0x0045B3C1 File Offset: 0x004595C1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.everNonEmpty, "everNonEmpty", false, false);
		}

		// Token: 0x0600F058 RID: 61528 RVA: 0x0045B3DB File Offset: 0x004595DB
		public override bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
		{
			if (base.TryAcceptThing(thing, allowSpecialEffects))
			{
				this.thisIsFirstBodyEver = !this.everNonEmpty;
				this.everNonEmpty = true;
				return true;
			}
			return false;
		}

		// Token: 0x0600F059 RID: 61529 RVA: 0x0045B400 File Offset: 0x00459600
		public override void Notify_HauledTo(Pawn hauler, Thing thing, int count)
		{
			base.Notify_HauledTo(hauler, thing, count);
			if (this.thisIsFirstBodyEver && hauler.IsColonist && base.Corpse.InnerPawn.def.race.Humanlike && !base.Corpse.everBuriedInSarcophagus)
			{
				base.Corpse.everBuriedInSarcophagus = true;
				foreach (Pawn pawn in base.Map.mapPawns.FreeColonists)
				{
					if (pawn.needs.mood != null)
					{
						pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.KnowBuriedInSarcophagus, null, null);
					}
				}
			}
		}

		// Token: 0x04009EA4 RID: 40612
		private bool everNonEmpty;

		// Token: 0x04009EA5 RID: 40613
		private bool thisIsFirstBodyEver;
	}
}
