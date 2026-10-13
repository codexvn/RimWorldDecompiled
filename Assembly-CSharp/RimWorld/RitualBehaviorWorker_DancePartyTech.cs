using System;
using System.Collections.Generic;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020026D0 RID: 9936
	public class RitualBehaviorWorker_DancePartyTech : RitualBehaviorWorker
	{
		// Token: 0x1700215A RID: 8538
		// (get) Token: 0x0600D9D0 RID: 55760 RVA: 0x00407550 File Offset: 0x00405750
		public override Sustainer SoundPlaying
		{
			get
			{
				return this.soundPlaying;
			}
		}

		// Token: 0x0600D9D1 RID: 55761 RVA: 0x00406F23 File Offset: 0x00405123
		public RitualBehaviorWorker_DancePartyTech()
		{
		}

		// Token: 0x0600D9D2 RID: 55762 RVA: 0x00406F2B File Offset: 0x0040512B
		public RitualBehaviorWorker_DancePartyTech(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x0600D9D3 RID: 55763 RVA: 0x00407558 File Offset: 0x00405758
		public override void Tick(LordJob_Ritual ritual)
		{
			base.Tick(ritual);
			Thing thing = ritual.selectedTarget.Thing;
			if (Find.TickManager.TicksGame % 20 == 0)
			{
				SoundDef soundDef = null;
				using (List<Pawn>.Enumerator enumerator = ritual.lord.ownedPawns.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (GatheringsUtility.InGatheringArea(enumerator.Current.Position, ritual.selectedTarget.Cell, ritual.Map))
						{
							soundDef = SoundDefOf.DanceParty_NoMusic;
							break;
						}
					}
				}
				if (thing != null)
				{
					CompLightball compLightball = thing.TryGetComp<CompLightball>();
					if (compLightball != null)
					{
						soundDef = (compLightball.Playing ? compLightball.SoundToPlay : null);
					}
				}
				if (soundDef != null && (this.soundPlaying == null || this.soundPlaying.def != soundDef))
				{
					this.soundPlaying = soundDef.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(ritual.selectedTarget.Cell, ritual.selectedTarget.Map, false), MaintenanceType.PerTick));
				}
			}
			if (this.soundPlaying != null && !this.soundPlaying.Ended)
			{
				this.soundPlaying.Maintain();
			}
		}

		// Token: 0x0600D9D4 RID: 55764 RVA: 0x0040767C File Offset: 0x0040587C
		public override void Cleanup(LordJob_Ritual ritual)
		{
			base.Cleanup(ritual);
			this.soundPlaying = null;
		}

		// Token: 0x0400938C RID: 37772
		private Sustainer soundPlaying;
	}
}
