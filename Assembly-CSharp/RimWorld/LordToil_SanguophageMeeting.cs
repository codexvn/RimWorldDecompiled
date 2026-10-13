using System;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001670 RID: 5744
	public class LordToil_SanguophageMeeting : LordToil
	{
		// Token: 0x170014BC RID: 5308
		// (get) Token: 0x06008763 RID: 34659 RVA: 0x00286EE4 File Offset: 0x002850E4
		private LordToilData_SanguophageMeeting Data
		{
			get
			{
				return (LordToilData_SanguophageMeeting)this.data;
			}
		}

		// Token: 0x06008764 RID: 34660 RVA: 0x00286EF1 File Offset: 0x002850F1
		public LordToil_SanguophageMeeting(IntVec3 meetingSpot, int meetingDurationTicks)
		{
			this.meetingSpot = meetingSpot;
			this.meetingDurationTicks = meetingDurationTicks;
			this.data = new LordToilData_SanguophageMeeting();
		}

		// Token: 0x06008765 RID: 34661 RVA: 0x00286F12 File Offset: 0x00285112
		public override void Init()
		{
			base.Init();
			Messages.Message("SanguophagesBegunMeeting".Translate(), this.lord.ownedPawns, MessageTypeDefOf.NeutralEvent, true);
		}

		// Token: 0x06008766 RID: 34662 RVA: 0x00286F44 File Offset: 0x00285144
		public override void LordToilTick()
		{
			this.Data.ticksInMeeting++;
			TargetInfo targetInfo = new TargetInfo(this.meetingSpot + new IntVec3(0, 0, -2), base.Map, false);
			if (this.progressBar == null)
			{
				this.progressBar = EffecterDefOf.ProgressBarAlwaysVisible.Spawn();
			}
			this.progressBar.EffectTick(targetInfo, TargetInfo.Invalid);
			MoteProgressBar mote = ((SubEffecter_ProgressBar)this.progressBar.children[0]).mote;
			if (mote != null)
			{
				mote.progress = Mathf.Clamp01((float)this.Data.ticksInMeeting / (float)this.meetingDurationTicks);
			}
			if (this.torchCached == null)
			{
				this.torchCached = GenClosest.ClosestThing_Regionwise_ReachablePrioritized(targetInfo.Cell, base.Map, ThingRequest.ForDef(ThingDefOf.SanguphageMeetingTorch), PathEndMode.OnCell, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false, true, false), 3f, null, null, 24, 30, false);
			}
			TargetInfo a = (this.torchCached != null) ? new TargetInfo(this.torchCached.Position, base.Map, false) : targetInfo;
			if (this.meetingEffect == null)
			{
				this.meetingEffect = EffecterDefOf.SanguophageMeeting.Spawn(a.Cell, base.Map, 1f);
			}
			this.meetingEffect.EffectTick(a, TargetInfo.Invalid);
		}

		// Token: 0x06008767 RID: 34663 RVA: 0x0028708E File Offset: 0x0028528E
		public override void Cleanup()
		{
			Effecter effecter = this.progressBar;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			Effecter effecter2 = this.meetingEffect;
			if (effecter2 != null)
			{
				effecter2.Cleanup();
			}
			this.progressBar = null;
			this.meetingEffect = null;
		}

		// Token: 0x06008768 RID: 34664 RVA: 0x002870C0 File Offset: 0x002852C0
		public override void UpdateAllDuties()
		{
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty pawnDuty = new PawnDuty(DutyDefOf.SocialMeeting, this.meetingSpot, -1f);
				pawnDuty.spectateRect = CellRect.SingleCell(this.meetingSpot);
				pawnDuty.spectateDistance = new IntRange(1, 2);
				pawnDuty.locomotion = LocomotionUrgency.Walk;
				this.lord.ownedPawns[i].mindState.duty = pawnDuty;
			}
		}

		// Token: 0x0400586D RID: 22637
		private IntVec3 meetingSpot;

		// Token: 0x0400586E RID: 22638
		private int meetingDurationTicks;

		// Token: 0x0400586F RID: 22639
		private Effecter progressBar;

		// Token: 0x04005870 RID: 22640
		private Effecter meetingEffect;

		// Token: 0x04005871 RID: 22641
		private Thing torchCached;
	}
}
