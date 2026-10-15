using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public abstract class CompAnalyzable : CompInteractable
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__15 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompAnalyzable _003C_003E4__this;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

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
		public _003CCompGetGizmosExtra_003Ed__15(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				CompAnalyzable CS_0024_003C_003E8__locals2 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((CompInteractable)CS_0024_003C_003E8__locals2).CompGetGizmosExtra().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_006f;
				case 1:
					_003C_003E1__state = -3;
					goto IL_006f;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_006f:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (DebugSettings.ShowDevGizmos)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Finish analysis",
							action = delegate
							{
								CS_0024_003C_003E8__locals2.OnAnalyzed(Find.CurrentMap.mapPawns.FreeColonistsSpawned.First());
							}
						};
						_003C_003E1__state = 2;
						return true;
					}
					break;
				}
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__15 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__15(0)
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

	public new CompProperties_Analyzable Props => (CompProperties_Analyzable)props;

	public Pawn Pawn => parent as Pawn;

	public abstract int AnalysisID { get; }

	public override string ExposeKey => "Analyzable";

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		base.PostSpawnSetup(respawningAfterLoad);
		if (!respawningAfterLoad && !Find.AnalysisManager.HasAnalysisWithID(AnalysisID))
		{
			Find.AnalysisManager.AddAnalysisTask(AnalysisID, Props.analysisRequiredRange.RandomInRange);
		}
	}

	public override void OrderForceTarget(LocalTargetInfo target)
	{
		Building_ResearchBench bench = null;
		if (ValidateTarget(target, showMessages: false) && (Props.canStudyInPlace || StudyUtility.TryFindResearchBench(target.Pawn, out bench)))
		{
			CompForbiddable compForbiddable = parent?.TryGetComp<CompForbiddable>();
			if (compForbiddable != null)
			{
				compForbiddable.Forbidden = false;
			}
			target.Pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.AnalyzeItem, parent, bench, bench?.Position ?? IntVec3.Invalid), JobTag.Misc);
		}
	}

	public override AcceptanceReport CanInteract(Pawn activateBy = null, bool checkOptionalItems = true)
	{
		AcceptanceReport result = base.CanInteract(activateBy, checkOptionalItems);
		if (!result.Accepted)
		{
			return result;
		}
		Find.AnalysisManager.TryGetAnalysisProgress(AnalysisID, out var details);
		if (!Props.allowRepeatAnalysis && details != null && details.Satisfied)
		{
			return "AlreadyAnalyzed".Translate();
		}
		if (activateBy != null && StatDefOf.ResearchSpeed.Worker.IsDisabledFor(activateBy))
		{
			return "Incapable".Translate();
		}
		if (activateBy != null && !Props.canStudyInPlace && !StudyUtility.TryFindResearchBench(activateBy, out var _))
		{
			return "NoResearchBench".Translate();
		}
		if (activateBy == null && !Props.canStudyInPlace && !parent.MapHeld.listerBuildings.ColonistsHaveResearchBench())
		{
			return "NoResearchBench".Translate();
		}
		return true;
	}

	public virtual void OnAnalyzed(Pawn pawn)
	{
		if (!Find.AnalysisManager.TryIncrementAnalysisProgress(AnalysisID, out var details) && Find.AnalysisManager.IsAnalysisComplete(AnalysisID))
		{
			SendLetter(Props.repeatCompletedLetterLabel, Props.repeatCompletedLetter, Props.repeatCompletedLetterDef, pawn);
		}
		else
		{
			SendAppropriateProgressLetter(pawn, details);
		}
		if (Props.destroyedOnAnalyzed)
		{
			parent.Destroy();
		}
	}

	private void SendAppropriateProgressLetter(Pawn pawn, AnalysisDetails details)
	{
		if (details.Satisfied)
		{
			SendLetter(Props.completedLetterLabel, Props.completedLetter, Props.completedLetterDef, pawn);
			return;
		}
		string text = "";
		if (Props.showProgress)
		{
			text = $" {details.timesDone}/{details.required}";
		}
		string letter = ((details.timesDone <= Props.progressedLetters.Count) ? Props.progressedLetters[details.timesDone - 1] : Props.progressedLetters.Last());
		SendLetter(Props.progressedLetterLabel + text, letter, Props.progressedLetterDef, pawn);
	}

	private void SendLetter(string label, string letter, LetterDef def, Pawn pawn)
	{
		if (!string.IsNullOrEmpty(label) && !string.IsNullOrEmpty(letter))
		{
			string formattedLetterString = GetFormattedLetterString(label, pawn);
			string formattedLetterString2 = GetFormattedLetterString(letter, pawn);
			Find.LetterStack.ReceiveLetter(formattedLetterString, formattedLetterString2, def);
		}
	}

	private string GetFormattedLetterString(string text, Pawn pawn)
	{
		if (ExtraNamedArg.HasValue)
		{
			return text.Formatted(pawn.Named("PAWN"), ExtraNamedArg.Value).Resolve();
		}
		return text.Formatted(pawn.Named("PAWN")).Resolve();
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__15))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__15(-2)
		{
			_003C_003E4__this = this
		};
	}
}
