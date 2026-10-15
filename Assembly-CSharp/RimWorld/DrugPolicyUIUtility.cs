using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public static class DrugPolicyUIUtility
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass2_0
	{
		public Pawn pawn;

		internal void _003CButton_GenerateMenu_003Eb__1()
		{
			Find.WindowStack.Add(new Dialog_ManageDrugPolicies(pawn.drugs.CurrentPolicy));
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass2_1
	{
		public DrugPolicy assignedDrugs;

		public _003C_003Ec__DisplayClass2_0 CS_0024_003C_003E8__locals1;

		internal void _003CButton_GenerateMenu_003Eb__0()
		{
			CS_0024_003C_003E8__locals1.pawn.drugs.CurrentPolicy = assignedDrugs;
		}
	}

	[CompilerGenerated]
	private sealed class _003CButton_GenerateMenu_003Ed__2 : IEnumerable<Widgets.DropdownMenuElement<DrugPolicy>>, IEnumerable, IEnumerator<Widgets.DropdownMenuElement<DrugPolicy>>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Widgets.DropdownMenuElement<DrugPolicy> _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private _003C_003Ec__DisplayClass2_0 _003C_003E8__1;

		private List<DrugPolicy>.Enumerator _003C_003E7__wrap1;

		Widgets.DropdownMenuElement<DrugPolicy> IEnumerator<Widgets.DropdownMenuElement<DrugPolicy>>.Current
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
		public _003CButton_GenerateMenu_003Ed__2(int _003C_003E1__state)
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
			_003C_003E8__1 = null;
			_003C_003E7__wrap1 = default(List<DrugPolicy>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass2_0();
					_003C_003E8__1.pawn = pawn;
					_003C_003E7__wrap1 = Current.Game.drugPolicyDatabase.AllPolicies.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00ee;
				case 1:
					_003C_003E1__state = -3;
					goto IL_00ee;
				case 2:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_00ee:
					if (_003C_003E7__wrap1.MoveNext())
					{
						_003C_003Ec__DisplayClass2_1 CS_0024_003C_003E8__locals4 = new _003C_003Ec__DisplayClass2_1
						{
							CS_0024_003C_003E8__locals1 = _003C_003E8__1,
							assignedDrugs = _003C_003E7__wrap1.Current
						};
						_003C_003E2__current = new Widgets.DropdownMenuElement<DrugPolicy>
						{
							option = new FloatMenuOption(CS_0024_003C_003E8__locals4.assignedDrugs.label, delegate
							{
								CS_0024_003C_003E8__locals4.CS_0024_003C_003E8__locals1.pawn.drugs.CurrentPolicy = CS_0024_003C_003E8__locals4.assignedDrugs;
							}),
							payload = CS_0024_003C_003E8__locals4.assignedDrugs
						};
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<DrugPolicy>.Enumerator);
					_003C_003E2__current = new Widgets.DropdownMenuElement<DrugPolicy>
					{
						option = new FloatMenuOption($"{"AssignTabEdit".Translate()}...", delegate
						{
							Find.WindowStack.Add(new Dialog_ManageDrugPolicies(_003C_003E8__1.pawn.drugs.CurrentPolicy));
						})
					};
					_003C_003E1__state = 2;
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
		IEnumerator<Widgets.DropdownMenuElement<DrugPolicy>> IEnumerable<Widgets.DropdownMenuElement<DrugPolicy>>.GetEnumerator()
		{
			_003CButton_GenerateMenu_003Ed__2 _003CButton_GenerateMenu_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CButton_GenerateMenu_003Ed__ = this;
			}
			else
			{
				_003CButton_GenerateMenu_003Ed__ = new _003CButton_GenerateMenu_003Ed__2(0);
			}
			_003CButton_GenerateMenu_003Ed__.pawn = _003C_003E3__pawn;
			return _003CButton_GenerateMenu_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Widgets.DropdownMenuElement<DrugPolicy>>)this).GetEnumerator();
		}
	}

	public const string AssigningDrugsTutorHighlightTag = "ButtonAssignDrugs";

	public static void DoAssignDrugPolicyButtons(Rect rect, Pawn pawn)
	{
		if (pawn.drugs.CurrentPolicy == null)
		{
			return;
		}
		Rect rect2 = rect.ContractedBy(0f, 2f);
		string text = pawn.drugs.CurrentPolicy.label;
		if (pawn.story?.traits != null)
		{
			Trait trait = pawn.story.traits.GetTrait(TraitDefOf.DrugDesire);
			Gene_ChemicalDependency gene;
			if (trait != null)
			{
				text = text + " (" + trait.Label + ")";
			}
			else if (ModsConfig.BiotechActive && PawnUtility.TryGetChemicalDependencyGene(pawn, out gene))
			{
				text = text + " (" + gene.Label + ")";
			}
		}
		Widgets.Dropdown(rect2, pawn, (Pawn p) => p.drugs.CurrentPolicy, Button_GenerateMenu, text.Truncate(rect2.width), null, pawn.drugs.CurrentPolicy.label, null, delegate
		{
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.DrugPolicies, KnowledgeAmount.Total);
		}, paintable: true);
		UIHighlighter.HighlightOpportunity(rect2, "ButtonAssignDrugs");
	}

	[IteratorStateMachine(typeof(_003CButton_GenerateMenu_003Ed__2))]
	private static IEnumerable<Widgets.DropdownMenuElement<DrugPolicy>> Button_GenerateMenu(Pawn pawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CButton_GenerateMenu_003Ed__2(-2)
		{
			_003C_003E3__pawn = pawn
		};
	}
}
