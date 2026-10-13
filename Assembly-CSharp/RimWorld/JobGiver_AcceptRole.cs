using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200142D RID: 5165
	public class JobGiver_AcceptRole : ThinkNode_JobGiver
	{
		// Token: 0x06007D7C RID: 32124 RVA: 0x0025CA08 File Offset: 0x0025AC08
		protected override Job TryGiveJob(Pawn pawn)
		{
			PawnDuty duty = pawn.mindState.duty;
			if (duty == null)
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.AcceptRole, pawn.Position, duty.spectateRect.CenterCell + new IntVec3(0, 0, -1));
			job.speechSoundMale = (this.soundDefMale ?? SoundDefOf.Speech_Leader_Male);
			job.speechSoundFemale = (this.soundDefFemale ?? SoundDefOf.Speech_Leader_Female);
			job.speechFaceSpectatorsIfPossible = true;
			return job;
		}

		// Token: 0x06007D7D RID: 32125 RVA: 0x0025CA89 File Offset: 0x0025AC89
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_AcceptRole jobGiver_AcceptRole = (JobGiver_AcceptRole)base.DeepCopy(resolve);
			jobGiver_AcceptRole.soundDefMale = this.soundDefMale;
			jobGiver_AcceptRole.soundDefFemale = this.soundDefFemale;
			return jobGiver_AcceptRole;
		}

		// Token: 0x04005453 RID: 21587
		public SoundDef soundDefMale;

		// Token: 0x04005454 RID: 21588
		public SoundDef soundDefFemale;
	}
}
