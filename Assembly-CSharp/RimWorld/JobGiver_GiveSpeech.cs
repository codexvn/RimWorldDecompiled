using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001442 RID: 5186
	public class JobGiver_GiveSpeech : ThinkNode_JobGiver
	{
		// Token: 0x06007DB4 RID: 32180 RVA: 0x0025DC90 File Offset: 0x0025BE90
		protected override Job TryGiveJob(Pawn pawn)
		{
			PawnDuty duty = pawn.mindState.duty;
			if (duty == null)
			{
				return null;
			}
			Building_Throne building_Throne = duty.focusSecond.Thing as Building_Throne;
			if (building_Throne == null || building_Throne.AssignedPawn != pawn)
			{
				return null;
			}
			if (!pawn.CanReach(building_Throne, PathEndMode.InteractionCell, Danger.None, false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.GiveSpeech, duty.focusSecond);
			job.speechSoundMale = (this.soundDefMale ?? SoundDefOf.Speech_Leader_Male);
			job.speechSoundFemale = (this.soundDefFemale ?? SoundDefOf.Speech_Leader_Female);
			return job;
		}

		// Token: 0x06007DB5 RID: 32181 RVA: 0x0025DD1C File Offset: 0x0025BF1C
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_GiveSpeech jobGiver_GiveSpeech = (JobGiver_GiveSpeech)base.DeepCopy(resolve);
			jobGiver_GiveSpeech.soundDefMale = this.soundDefMale;
			jobGiver_GiveSpeech.soundDefFemale = this.soundDefFemale;
			return jobGiver_GiveSpeech;
		}

		// Token: 0x0400546D RID: 21613
		public SoundDef soundDefMale;

		// Token: 0x0400546E RID: 21614
		public SoundDef soundDefFemale;
	}
}
