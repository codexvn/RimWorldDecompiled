using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020026CA RID: 9930
	public class RitualBehaviorWorker_Duel : RitualBehaviorWorker
	{
		// Token: 0x0600D9B5 RID: 55733 RVA: 0x00406F23 File Offset: 0x00405123
		public RitualBehaviorWorker_Duel()
		{
		}

		// Token: 0x0600D9B6 RID: 55734 RVA: 0x00406F2B File Offset: 0x0040512B
		public RitualBehaviorWorker_Duel(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x17002157 RID: 8535
		// (get) Token: 0x0600D9B7 RID: 55735 RVA: 0x00406F4D File Offset: 0x0040514D
		public override Sustainer SoundPlaying
		{
			get
			{
				return this.soundPlaying;
			}
		}

		// Token: 0x0600D9B8 RID: 55736 RVA: 0x00406F58 File Offset: 0x00405158
		public override string CanStartRitualNow(TargetInfo target, Precept_Ritual ritual, Pawn selectedPawn = null, Dictionary<string, Pawn> forcedForRole = null)
		{
			RitualBehaviorWorker_Duel.<>c__DisplayClass8_0 CS$<>8__locals1;
			CS$<>8__locals1.target = target;
			string text = base.CanStartRitualNow(CS$<>8__locals1.target, ritual, selectedPawn, forcedForRole);
			if (text != null)
			{
				return text;
			}
			CS$<>8__locals1.room = CS$<>8__locals1.target.Cell.GetRoom(CS$<>8__locals1.target.Map);
			if (RitualBehaviorWorker_Duel.<CanStartRitualNow>g__StandableCellsInRange|8_0(new IntRange(RitualBehaviorWorker_Duel.RadiusRangeLeader, RitualBehaviorWorker_Duel.RadiusRangeLeader), ref CS$<>8__locals1) < 1)
			{
				return "CantStartNotEnoughSpaceDuelSpeaker".Translate(RitualBehaviorWorker_Duel.RadiusRangeLeader.Named("MINRADIUS"));
			}
			if (RitualBehaviorWorker_Duel.<CanStartRitualNow>g__StandableCellsInRange|8_0(new IntRange(RitualBehaviorWorker_Duel.RadiusRangeDuelists, RitualBehaviorWorker_Duel.RadiusRangeDuelists), ref CS$<>8__locals1) < 2)
			{
				return "CantStartNotEnoughSpaceDuelDuelists".Translate(RitualBehaviorWorker_Duel.RadiusRangeSpectators.Average.Named("MINRADIUS"), 2);
			}
			if (RitualBehaviorWorker_Duel.<CanStartRitualNow>g__StandableCellsInRange|8_0(RitualBehaviorWorker_Duel.RadiusRangeSpectators, ref CS$<>8__locals1) < 20)
			{
				return "CantStartNotEnoughSpaceDuelSpectators".Translate(RitualBehaviorWorker_Duel.RadiusRangeSpectators.Average.Named("MINRADIUS"), 20);
			}
			return null;
		}

		// Token: 0x0600D9B9 RID: 55737 RVA: 0x00407073 File Offset: 0x00405273
		protected override LordJob CreateLordJob(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			return new LordJob_Ritual_Duel(target, ritual, obligation, this.def.stages, assignments, organizer);
		}

		// Token: 0x0600D9BA RID: 55738 RVA: 0x0040708C File Offset: 0x0040528C
		public override void Tick(LordJob_Ritual ritual)
		{
			base.Tick(ritual);
			if (ritual.StageIndex == 4)
			{
				if (this.soundPlaying == null || this.soundPlaying.Ended)
				{
					TargetInfo selectedTarget = ritual.selectedTarget;
					this.soundPlaying = SoundDefOf.DuelMusic.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(selectedTarget.Cell, selectedTarget.Map, false), MaintenanceType.PerTick));
				}
				Sustainer sustainer = this.soundPlaying;
				if (sustainer == null)
				{
					return;
				}
				sustainer.Maintain();
			}
		}

		// Token: 0x0600D9BB RID: 55739 RVA: 0x004070FF File Offset: 0x004052FF
		public override void Cleanup(LordJob_Ritual ritual)
		{
			this.soundPlaying = null;
		}

		// Token: 0x0600D9BC RID: 55740 RVA: 0x00407108 File Offset: 0x00405308
		public override void DrawPreviewOnTarget(TargetInfo targetInfo)
		{
			base.DrawPreviewOnTarget(targetInfo);
			GenDraw.DrawRadiusRing(targetInfo.CenterCell, 5.9f);
		}

		// Token: 0x0600D9BE RID: 55742 RVA: 0x0040713C File Offset: 0x0040533C
		[CompilerGenerated]
		internal static int <CanStartRitualNow>g__StandableCellsInRange|8_0(IntRange range, ref RitualBehaviorWorker_Duel.<>c__DisplayClass8_0 A_1)
		{
			int num = 0;
			foreach (IntVec3 intVec in CellRect.CenteredOn(A_1.target.Cell, range.max))
			{
				float num2 = intVec.DistanceTo(A_1.target.Cell);
				if (num2 >= (float)range.min && num2 <= (float)range.max && intVec.Standable(A_1.target.Map) && intVec.GetRoom(A_1.target.Map) == A_1.room)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x04009381 RID: 37761
		private Sustainer soundPlaying;

		// Token: 0x04009382 RID: 37762
		public static readonly IntRange RadiusRangeSpectators = new IntRange(5, 7);

		// Token: 0x04009383 RID: 37763
		public static readonly int RadiusRangeLeader = 3;

		// Token: 0x04009384 RID: 37764
		public static readonly int RadiusRangeDuelists = 2;
	}
}
