using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class ChoiceLetter_EntityDiscovered : ChoiceLetter
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass2_0
	{
		public ResearchProjectDef project;

		internal void _003Cget_Choices_003Eb__0()
		{
			Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Research);
			if (MainButtonDefOf.Research.TabWindow is MainTabWindow_Research mainTabWindow_Research)
			{
				mainTabWindow_Research.Select(project);
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_Choices_003Ed__2 : IEnumerable<DiaOption>, IEnumerable, IEnumerator<DiaOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private DiaOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ChoiceLetter_EntityDiscovered _003C_003E4__this;

		private List<ResearchProjectDef>.Enumerator _003C_003E7__wrap1;

		DiaOption IEnumerator<DiaOption>.Current
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
		public _003Cget_Choices_003Ed__2(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<ResearchProjectDef>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				ChoiceLetter_EntityDiscovered choiceLetter_EntityDiscovered = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (choiceLetter_EntityDiscovered.codexEntry != null)
					{
						_003C_003E7__wrap1 = choiceLetter_EntityDiscovered.codexEntry.discoveredResearchProjects.GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_00ca;
					}
					goto IL_0107;
				case 1:
					_003C_003E1__state = -3;
					goto IL_00ca;
				case 2:
					_003C_003E1__state = -1;
					goto IL_0107;
				case 3:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_00ca:
					if (_003C_003E7__wrap1.MoveNext())
					{
						_003C_003Ec__DisplayClass2_0 CS_0024_003C_003E8__locals2 = new _003C_003Ec__DisplayClass2_0
						{
							project = _003C_003E7__wrap1.Current
						};
						_003C_003E2__current = new DiaOption("ViewHyperlink".Translate(CS_0024_003C_003E8__locals2.project.label))
						{
							action = delegate
							{
								Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Research);
								if (MainButtonDefOf.Research.TabWindow is MainTabWindow_Research mainTabWindow_Research)
								{
									mainTabWindow_Research.Select(CS_0024_003C_003E8__locals2.project);
								}
							},
							resolveTree = true
						};
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<ResearchProjectDef>.Enumerator);
					_003C_003E2__current = choiceLetter_EntityDiscovered.Option_OpenEntityCodex;
					_003C_003E1__state = 2;
					return true;
					IL_0107:
					_003C_003E2__current = choiceLetter_EntityDiscovered.Option_Close;
					_003C_003E1__state = 3;
					return true;
				}
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<DiaOption> IEnumerable<DiaOption>.GetEnumerator()
		{
			_003Cget_Choices_003Ed__2 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_Choices_003Ed__2(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<DiaOption>)this).GetEnumerator();
		}
	}

	public EntityCodexEntryDef codexEntry;

	public override IEnumerable<DiaOption> Choices
	{
		[IteratorStateMachine(typeof(_003Cget_Choices_003Ed__2))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Choices_003Ed__2(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	protected DiaOption Option_OpenEntityCodex => new DiaOption("ViewEntityCodex".Translate())
	{
		action = delegate
		{
			if (codexEntry != null)
			{
				Find.WindowStack.Add(new Dialog_EntityCodex(codexEntry));
			}
		},
		resolveTree = true
	};

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Defs.Look(ref codexEntry, "codexEntry");
	}
}
