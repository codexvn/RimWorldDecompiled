using System;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020026CF RID: 9935
	public class RitualBehaviorWorker_PartyDanceDrums : RitualBehaviorWorker
	{
		// Token: 0x0600D9CA RID: 55754 RVA: 0x0040737F File Offset: 0x0040557F
		public RitualBehaviorWorker_PartyDanceDrums()
		{
		}

		// Token: 0x0600D9CB RID: 55755 RVA: 0x0040738E File Offset: 0x0040558E
		public RitualBehaviorWorker_PartyDanceDrums(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x0600D9CC RID: 55756 RVA: 0x0040739E File Offset: 0x0040559E
		protected override LordJob CreateLordJob(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			return new LordJob_PartyDanceDrums(target, ritual, obligation, this.def.stages, assignments, organizer);
		}

		// Token: 0x17002158 RID: 8536
		// (get) Token: 0x0600D9CD RID: 55757 RVA: 0x004073B7 File Offset: 0x004055B7
		public override Sustainer SoundPlaying
		{
			get
			{
				return this.soundPlaying;
			}
		}

		// Token: 0x17002159 RID: 8537
		// (get) Token: 0x0600D9CE RID: 55758 RVA: 0x00002501 File Offset: 0x00000701
		public override bool ChecksReservations
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600D9CF RID: 55759 RVA: 0x004073C0 File Offset: 0x004055C0
		public override void Tick(LordJob_Ritual ritual)
		{
			base.Tick(ritual);
			TargetInfo selectedTarget = ritual.selectedTarget;
			if (Find.TickManager.TicksGame % 20 == 0 || this.numActiveDrums == -1)
			{
				this.numActiveDrums = 0;
				foreach (Thing thing in selectedTarget.Map.listerBuldingOfDefInProximity.GetForCell(selectedTarget.Cell, (float)this.def.maxEnhancerDistance, ThingDefOf.Drum, null))
				{
					Building_MusicalInstrument building_MusicalInstrument = thing as Building_MusicalInstrument;
					if (thing.GetRoom(RegionType.Set_All) == selectedTarget.Thing.GetRoom(RegionType.Set_All) && building_MusicalInstrument != null && building_MusicalInstrument.IsBeingPlayed)
					{
						this.numActiveDrums++;
						this.anyDrumReached = true;
					}
				}
			}
			bool flag = this.numActiveDrums > 0 || this.anyDrumReached;
			SoundDef soundDef = (!this.def.soundDefsPerEnhancerCount.NullOrEmpty<SoundDef>() && flag) ? this.def.soundDefsPerEnhancerCount[Math.Min(this.numActiveDrums, this.def.soundDefsPerEnhancerCount.Count - 1)] : null;
			if (soundDef != null && (this.soundPlaying == null || this.soundPlaying.def != soundDef))
			{
				this.soundPlaying = soundDef.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(selectedTarget.Cell, selectedTarget.Map, false), MaintenanceType.PerTick));
			}
			if (flag)
			{
				Sustainer sustainer = this.soundPlaying;
				if (sustainer == null)
				{
					return;
				}
				sustainer.Maintain();
			}
		}

		// Token: 0x04009389 RID: 37769
		private Sustainer soundPlaying;

		// Token: 0x0400938A RID: 37770
		private int numActiveDrums = -1;

		// Token: 0x0400938B RID: 37771
		private bool anyDrumReached;
	}
}
