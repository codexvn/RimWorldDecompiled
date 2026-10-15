using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class CompProperties_Studiable : CompProperties
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__18 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompProperties_Studiable _003C_003E4__this;

		private ThingDef parentDef;

		public ThingDef _003C_003E3__parentDef;

		private IEnumerator<string> _003C_003E7__wrap1;

		string IEnumerator<string>.Current
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
		public _003CConfigErrors_003Ed__18(int _003C_003E1__state)
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
				CompProperties_Studiable compProperties_Studiable = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((CompProperties)compProperties_Studiable).ConfigErrors(parentDef).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0084;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0084;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00e2;
				case 3:
					_003C_003E1__state = -1;
					goto IL_0126;
				case 4:
					_003C_003E1__state = -1;
					goto IL_0183;
				case 5:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0084:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (compProperties_Studiable.requiresHoldingPlatform && !typeof(Pawn).IsAssignableFrom(parentDef.thingClass))
					{
						_003C_003E2__current = "requiresHoldingPlatform can only be used on Pawns";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00e2;
					IL_0183:
					if (compProperties_Studiable.minMonolithLevelForStudy == 0)
					{
						_003C_003E2__current = "anomalyKnowledge is set but minMonolithLevelForStudy is not";
						_003C_003E1__state = 5;
						return true;
					}
					break;
					IL_0126:
					if (!ModsConfig.AnomalyActive || !(compProperties_Studiable.anomalyKnowledge > 0f))
					{
						break;
					}
					if (compProperties_Studiable.knowledgeCategory == null && parentDef != ThingDefOf.VoidMonolith)
					{
						_003C_003E2__current = "anomalyKnowledge is set but knowledgeCategory is not";
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_0183;
					IL_00e2:
					if (compProperties_Studiable.requiresImprisonment && !typeof(Pawn).IsAssignableFrom(parentDef.thingClass))
					{
						_003C_003E2__current = "requiresImprisonment can only be used on Pawns";
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_0126;
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__18 _003CConfigErrors_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CConfigErrors_003Ed__ = this;
			}
			else
			{
				_003CConfigErrors_003Ed__ = new _003CConfigErrors_003Ed__18(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CConfigErrors_003Ed__.parentDef = _003C_003E3__parentDef;
			return _003CConfigErrors_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public int frequencyTicks = -1;

	public float studyAmountToComplete = -1f;

	public bool showToggleGizmo;

	public bool studyEnabledByDefault = true;

	public bool canBeActivityDeactivated;

	internal float? anomalyKnowledge;

	internal KnowledgeCategoryDef knowledgeCategory;

	public float knowledgeFactorOutdoors = 1f;

	public int minMonolithLevelForStudy;

	internal bool requiresHoldingPlatform;

	internal bool requiresImprisonment;

	[MustTranslate]
	public string completedLetterTitle;

	[MustTranslate]
	public string completedLetterText;

	[MustTranslate]
	public string completedMessage;

	public LetterDef completedLetterDef;

	public bool Completable => studyAmountToComplete > 0f;

	public CompProperties_Studiable()
	{
		compClass = typeof(CompStudiable);
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__18))]
	public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__18(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__parentDef = parentDef
		};
	}
}
