using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class CompStudyUnlocks : ThingComp, IThingStudied
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__21 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompStudyUnlocks _003C_003E4__this;

		Gizmo IEnumerator<Gizmo>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CCompGetGizmosExtra_003Ed__21(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			CompStudyUnlocks CS_0024_003C_003E8__locals4 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!DebugSettings.ShowDevGizmos || CS_0024_003C_003E8__locals4.Completed)
				{
					return false;
				}
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Advance study",
					action = delegate
					{
						CS_0024_003C_003E8__locals4.RegisterStudyLevel(CS_0024_003C_003E8__locals4.parent.MapHeld.mapPawns.FreeColonists.RandomElement(), CS_0024_003C_003E8__locals4.nextIndex);
					}
				};
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				return false;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__21 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__21(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	private List<float> studyThresholds = new List<float>();

	protected List<ChoiceLetter> letters = new List<ChoiceLetter>();

	protected int nextIndex;

	protected int studyProgress;

	private CompStudiable studyInt;

	protected CompProperties_StudyUnlocks Props => (CompProperties_StudyUnlocks)props;

	protected CompStudiable StudyComp => studyInt ?? (studyInt = parent.TryGetComp<CompStudiable>());

	public IReadOnlyList<ChoiceLetter> Letters => letters;

	public int Progress => studyProgress;

	public bool Completed => nextIndex >= Props.studyNotes.Count;

	public float? StudyKnowledgeAmount
	{
		get
		{
			if (studyProgress == 0)
			{
				return Props.defaultStudyAmount;
			}
			return Props.studyNotes[studyProgress - 1].studyKnowledgeAmount;
		}
	}

	public override void PostPostMake()
	{
		SetupStudyThresholds();
	}

	public void OnStudied(Pawn studier, float amount, KnowledgeCategoryDef category = null)
	{
		if (!Completed)
		{
			float anomalyKnowledgeGained = StudyComp.anomalyKnowledgeGained;
			for (int i = nextIndex; i < Props.studyNotes.Count && !(anomalyKnowledgeGained < studyThresholds[i]); i++)
			{
				RegisterStudyLevel(studier, i);
			}
		}
	}

	private void RegisterStudyLevel(Pawn studier, int i)
	{
		if (nextIndex <= i)
		{
			StudyNote studyNote = Props.studyNotes[i];
			nextIndex = i + 1;
			studyProgress = nextIndex;
			TaggedString label = studyNote.label.Formatted(studier.Named("PAWN"));
			TaggedString text = studyNote.text.Formatted(studier.Named("PAWN"));
			ChoiceLetter let = LetterMaker.MakeLetter(label, text, LetterDefOf.NeutralEvent, parent);
			Find.LetterStack.ReceiveLetter(let);
			ChoiceLetter choiceLetter = LetterMaker.MakeLetter(label, text, LetterDefOf.NeutralEvent, parent);
			choiceLetter.arrivalTick = Find.TickManager.TicksGame;
			letters.Add(choiceLetter);
			Notify_StudyLevelChanged(choiceLetter);
		}
	}

	protected virtual void Notify_StudyLevelChanged(ChoiceLetter keptLetter)
	{
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__21))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__21(-2)
		{
			_003C_003E4__this = this
		};
	}

	private void SetupStudyThresholds()
	{
		foreach (StudyNote studyNote in Props.studyNotes)
		{
			studyThresholds.Add((studyNote.threshold != 0f) ? studyNote.threshold : studyNote.thresholdRange.RandomInRange);
		}
	}

	public override void PostExposeData()
	{
		Scribe_Values.Look(ref nextIndex, "nextIndex", 0);
		Scribe_Values.Look(ref studyProgress, "studyProgress", 0);
		Scribe_Collections.Look(ref studyThresholds, "studyThresholds", LookMode.Value);
		Scribe_Collections.Look(ref letters, "letters", LookMode.Deep);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			if (letters == null)
			{
				letters = new List<ChoiceLetter>();
			}
			if (studyThresholds == null)
			{
				studyThresholds = new List<float>();
				SetupStudyThresholds();
			}
		}
	}
}
