using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E66 RID: 11878
	public class CompStudyUnlocks : ThingComp, IThingStudied
	{
		// Token: 0x17002AE9 RID: 10985
		// (get) Token: 0x06010BA1 RID: 68513 RVA: 0x004D2A4B File Offset: 0x004D0C4B
		protected CompProperties_StudyUnlocks Props
		{
			get
			{
				return (CompProperties_StudyUnlocks)this.props;
			}
		}

		// Token: 0x17002AEA RID: 10986
		// (get) Token: 0x06010BA2 RID: 68514 RVA: 0x004D2A58 File Offset: 0x004D0C58
		protected CompStudiable StudyComp
		{
			get
			{
				CompStudiable result;
				if ((result = this.studyInt) == null)
				{
					result = (this.studyInt = this.parent.TryGetComp<CompStudiable>());
				}
				return result;
			}
		}

		// Token: 0x17002AEB RID: 10987
		// (get) Token: 0x06010BA3 RID: 68515 RVA: 0x004D2A83 File Offset: 0x004D0C83
		public IReadOnlyList<ChoiceLetter> Letters
		{
			get
			{
				return this.letters;
			}
		}

		// Token: 0x17002AEC RID: 10988
		// (get) Token: 0x06010BA4 RID: 68516 RVA: 0x004D2A8B File Offset: 0x004D0C8B
		public int Progress
		{
			get
			{
				return this.studyProgress;
			}
		}

		// Token: 0x17002AED RID: 10989
		// (get) Token: 0x06010BA5 RID: 68517 RVA: 0x004D2A93 File Offset: 0x004D0C93
		public bool Completed
		{
			get
			{
				return this.nextIndex >= this.Props.studyNotes.Count;
			}
		}

		// Token: 0x17002AEE RID: 10990
		// (get) Token: 0x06010BA6 RID: 68518 RVA: 0x004D2AB0 File Offset: 0x004D0CB0
		public float? StudyKnowledgeAmount
		{
			get
			{
				if (this.studyProgress == 0)
				{
					return this.Props.defaultStudyAmount;
				}
				return this.Props.studyNotes[this.studyProgress - 1].studyKnowledgeAmount;
			}
		}

		// Token: 0x06010BA7 RID: 68519 RVA: 0x004D2AE3 File Offset: 0x004D0CE3
		public override void PostPostMake()
		{
			this.SetupStudyThresholds();
		}

		// Token: 0x06010BA8 RID: 68520 RVA: 0x004D2AEC File Offset: 0x004D0CEC
		public void OnStudied(Pawn studier, float amount, KnowledgeCategoryDef category = null)
		{
			if (this.Completed)
			{
				return;
			}
			float anomalyKnowledgeGained = this.StudyComp.anomalyKnowledgeGained;
			int num = this.nextIndex;
			while (num < this.Props.studyNotes.Count && anomalyKnowledgeGained >= this.studyThresholds[num])
			{
				this.RegisterStudyLevel(studier, num);
				num++;
			}
		}

		// Token: 0x06010BA9 RID: 68521 RVA: 0x004D2B48 File Offset: 0x004D0D48
		private void RegisterStudyLevel(Pawn studier, int i)
		{
			if (this.nextIndex > i)
			{
				return;
			}
			StudyNote studyNote = this.Props.studyNotes[i];
			this.nextIndex = i + 1;
			this.studyProgress = this.nextIndex;
			TaggedString label = studyNote.label.Formatted(studier.Named("PAWN"));
			TaggedString text = studyNote.text.Formatted(studier.Named("PAWN"));
			ChoiceLetter let = LetterMaker.MakeLetter(label, text, LetterDefOf.NeutralEvent, this.parent, null, null, null);
			Find.LetterStack.ReceiveLetter(let, null, 0, true);
			ChoiceLetter choiceLetter = LetterMaker.MakeLetter(label, text, LetterDefOf.NeutralEvent, this.parent, null, null, null);
			choiceLetter.arrivalTick = Find.TickManager.TicksGame;
			this.letters.Add(choiceLetter);
			this.Notify_StudyLevelChanged(choiceLetter);
		}

		// Token: 0x06010BAA RID: 68522 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void Notify_StudyLevelChanged(ChoiceLetter keptLetter)
		{
		}

		// Token: 0x06010BAB RID: 68523 RVA: 0x004D2C19 File Offset: 0x004D0E19
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!DebugSettings.ShowDevGizmos || this.Completed)
			{
				yield break;
			}
			yield return new Command_Action
			{
				defaultLabel = "DEV: Advance study",
				action = delegate()
				{
					this.RegisterStudyLevel(this.parent.MapHeld.mapPawns.FreeColonists.RandomElement<Pawn>(), this.nextIndex);
				}
			};
			yield break;
		}

		// Token: 0x06010BAC RID: 68524 RVA: 0x004D2C2C File Offset: 0x004D0E2C
		private void SetupStudyThresholds()
		{
			foreach (StudyNote studyNote in this.Props.studyNotes)
			{
				this.studyThresholds.Add((studyNote.threshold != 0f) ? studyNote.threshold : studyNote.thresholdRange.RandomInRange);
			}
		}

		// Token: 0x06010BAD RID: 68525 RVA: 0x004D2CA8 File Offset: 0x004D0EA8
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.nextIndex, "nextIndex", 0, false);
			Scribe_Values.Look<int>(ref this.studyProgress, "studyProgress", 0, false);
			Scribe_Collections.Look<float>(ref this.studyThresholds, "studyThresholds", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<ChoiceLetter>(ref this.letters, "letters", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.letters == null)
				{
					this.letters = new List<ChoiceLetter>();
				}
				if (this.studyThresholds == null)
				{
					this.studyThresholds = new List<float>();
					this.SetupStudyThresholds();
				}
			}
		}

		// Token: 0x0400AE60 RID: 44640
		private List<float> studyThresholds = new List<float>();

		// Token: 0x0400AE61 RID: 44641
		protected List<ChoiceLetter> letters = new List<ChoiceLetter>();

		// Token: 0x0400AE62 RID: 44642
		protected int nextIndex;

		// Token: 0x0400AE63 RID: 44643
		protected int studyProgress;

		// Token: 0x0400AE64 RID: 44644
		private CompStudiable studyInt;
	}
}
