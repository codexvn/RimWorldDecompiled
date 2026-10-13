using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001443 RID: 5187
	public class JobGiver_GiveSpeechFacingTarget : ThinkNode_JobGiver
	{
		// Token: 0x06007DB7 RID: 32183 RVA: 0x0025DD44 File Offset: 0x0025BF44
		protected override Job TryGiveJob(Pawn pawn)
		{
			PawnDuty duty = pawn.mindState.duty;
			if (duty == null)
			{
				return null;
			}
			IntVec3 position = pawn.Position;
			if (!pawn.CanReserve(pawn.Position, 1, -1, null, false))
			{
				CellFinder.TryRandomClosewalkCellNear(position, pawn.Map, 2, out position, (IntVec3 c) => pawn.CanReserveAndReach(c, PathEndMode.OnCell, pawn.NormalMaxDanger(), 1, -1, null, false));
			}
			Pawn_MindState mindState = pawn.mindState;
			Rot4? rot;
			if (mindState == null)
			{
				rot = null;
			}
			else
			{
				PawnDuty duty2 = mindState.duty;
				rot = ((duty2 != null) ? new Rot4?(duty2.overrideFacing) : null);
			}
			Rot4? rot2 = rot;
			IntVec3 c2 = (rot2 != null && rot2.Value.IsValid) ? (position + rot2.Value.FacingCell) : duty.spectateRect.CenterCell;
			Lord lord = pawn.GetLord();
			LordJob_Ritual lordJob_Ritual = ((lord != null) ? lord.LordJob : null) as LordJob_Ritual;
			Job job = JobMaker.MakeJob(JobDefOf.GiveSpeech, position, c2);
			job.showSpeechBubbles = this.showSpeechBubbles;
			if (lordJob_Ritual != null)
			{
				LordToil_Ritual lordToil_Ritual = lordJob_Ritual.lord.CurLordToil as LordToil_Ritual;
				if (lordToil_Ritual != null)
				{
					job.interaction = lordToil_Ritual.stage.BehaviorForRole(lordJob_Ritual.RoleFor(pawn, false).id).speakerInteraction;
				}
			}
			job.speechSoundMale = (this.soundDefMale ?? SoundDefOf.Speech_Leader_Male);
			job.speechSoundFemale = (this.soundDefFemale ?? SoundDefOf.Speech_Leader_Female);
			job.speechFaceSpectatorsIfPossible = this.faceSpectatorsIfPossible;
			return job;
		}

		// Token: 0x06007DB8 RID: 32184 RVA: 0x0025DF02 File Offset: 0x0025C102
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_GiveSpeechFacingTarget jobGiver_GiveSpeechFacingTarget = (JobGiver_GiveSpeechFacingTarget)base.DeepCopy(resolve);
			jobGiver_GiveSpeechFacingTarget.soundDefMale = this.soundDefMale;
			jobGiver_GiveSpeechFacingTarget.soundDefFemale = this.soundDefFemale;
			jobGiver_GiveSpeechFacingTarget.showSpeechBubbles = this.showSpeechBubbles;
			jobGiver_GiveSpeechFacingTarget.faceSpectatorsIfPossible = this.faceSpectatorsIfPossible;
			return jobGiver_GiveSpeechFacingTarget;
		}

		// Token: 0x0400546F RID: 21615
		public SoundDef soundDefMale;

		// Token: 0x04005470 RID: 21616
		public SoundDef soundDefFemale;

		// Token: 0x04005471 RID: 21617
		public bool faceSpectatorsIfPossible;

		// Token: 0x04005472 RID: 21618
		public bool showSpeechBubbles = true;
	}
}
