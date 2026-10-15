using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class ThoughtWorker_RoyalTitleApparelRequirementNotMet : ThoughtWorker
{
	[CompilerGenerated]
	private sealed class _003CGetAllRequiredApparelPerGroup_003Ed__1 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn p;

		public Pawn _003C_003E3__p;

		private List<RoyalTitle>.Enumerator _003C_003E7__wrap1;

		private RoyalTitle _003Ct_003E5__3;

		private int _003Cj_003E5__4;

		private IEnumerator<ThingDef> _003C_003E7__wrap4;

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
		public _003CGetAllRequiredApparelPerGroup_003Ed__1(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || num == 1)
			{
				try
				{
					if (num == -4 || num == 1)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally2();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = default(List<RoyalTitle>.Enumerator);
			_003Ct_003E5__3 = null;
			_003C_003E7__wrap4 = null;
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
					if (p.royalty == null || !p.royalty.allowApparelRequirements)
					{
						return false;
					}
					_003C_003E7__wrap1 = p.royalty.AllTitlesInEffectForReading.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_01b2;
				case 1:
					_003C_003E1__state = -4;
					goto IL_015f;
				case 2:
					_003C_003E1__state = -1;
					_003C_003E2__current = "ApparelRequirementAnyPsycasterApparel".Translate();
					_003C_003E1__state = 3;
					return true;
				case 3:
					_003C_003E1__state = -1;
					if (ModsConfig.BiotechActive)
					{
						_003C_003E2__current = "ApparelRequirementAnyMechlordApparel".Translate();
						_003C_003E1__state = 4;
						return true;
					}
					break;
				case 4:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_015f:
					if (_003C_003E7__wrap4.MoveNext())
					{
						ThingDef current = _003C_003E7__wrap4.Current;
						_003C_003E2__current = current.LabelCap;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap4 = null;
					goto IL_0179;
					IL_018b:
					if (_003Cj_003E5__4 < _003Ct_003E5__3.def.requiredApparel.Count)
					{
						ApparelRequirement apparelRequirement = _003Ct_003E5__3.def.requiredApparel[_003Cj_003E5__4];
						if (apparelRequirement.IsActive(p) && !apparelRequirement.IsMet(p))
						{
							IEnumerable<ThingDef> enumerable = apparelRequirement.AllRequiredApparelForPawn(p);
							_003C_003E7__wrap4 = enumerable.GetEnumerator();
							_003C_003E1__state = -4;
							goto IL_015f;
						}
						goto IL_0179;
					}
					goto IL_01ab;
					IL_01b2:
					if (_003C_003E7__wrap1.MoveNext())
					{
						_003Ct_003E5__3 = _003C_003E7__wrap1.Current;
						if (_003Ct_003E5__3.def.requiredApparel != null && _003Ct_003E5__3.def.requiredApparel.Count > 0)
						{
							_003Cj_003E5__4 = 0;
							goto IL_018b;
						}
						goto IL_01ab;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<RoyalTitle>.Enumerator);
					_003C_003E2__current = "ApparelRequirementAnyPrestigeArmor".Translate();
					_003C_003E1__state = 2;
					return true;
					IL_0179:
					_003Cj_003E5__4++;
					goto IL_018b;
					IL_01ab:
					_003Ct_003E5__3 = null;
					goto IL_01b2;
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
			if (_003C_003E7__wrap4 != null)
			{
				_003C_003E7__wrap4.Dispose();
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
			_003CGetAllRequiredApparelPerGroup_003Ed__1 _003CGetAllRequiredApparelPerGroup_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetAllRequiredApparelPerGroup_003Ed__ = this;
			}
			else
			{
				_003CGetAllRequiredApparelPerGroup_003Ed__ = new _003CGetAllRequiredApparelPerGroup_003Ed__1(0);
			}
			_003CGetAllRequiredApparelPerGroup_003Ed__.p = _003C_003E3__p;
			return _003CGetAllRequiredApparelPerGroup_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	private static RoyalTitle Validate(Pawn p)
	{
		if (p.royalty == null || !p.royalty.allowApparelRequirements)
		{
			return null;
		}
		foreach (RoyalTitle item in p.royalty.AllTitlesInEffectForReading)
		{
			if (item.def.requiredApparel == null || item.def.requiredApparel.Count <= 0)
			{
				continue;
			}
			for (int i = 0; i < item.def.requiredApparel.Count; i++)
			{
				ApparelRequirement apparelRequirement = item.def.requiredApparel[i];
				if (apparelRequirement.IsActive(p) && !apparelRequirement.IsMet(p))
				{
					return item;
				}
			}
		}
		return null;
	}

	[IteratorStateMachine(typeof(_003CGetAllRequiredApparelPerGroup_003Ed__1))]
	private static IEnumerable<string> GetAllRequiredApparelPerGroup(Pawn p)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetAllRequiredApparelPerGroup_003Ed__1(-2)
		{
			_003C_003E3__p = p
		};
	}

	public override string PostProcessLabel(Pawn p, string label)
	{
		RoyalTitle royalTitle = Validate(p);
		if (royalTitle == null)
		{
			return string.Empty;
		}
		return label.Formatted(royalTitle.Named("TITLE"), p.Named("PAWN")).CapitalizeFirst();
	}

	public override string PostProcessDescription(Pawn p, string description)
	{
		RoyalTitle royalTitle = Validate(p);
		if (royalTitle == null)
		{
			return string.Empty;
		}
		return description.Formatted(GetAllRequiredApparelPerGroup(p).Distinct().ToLineList("- "), royalTitle.Named("TITLE"), p.Named("PAWN")).CapitalizeFirst();
	}

	protected override ThoughtState CurrentStateInternal(Pawn p)
	{
		if (Validate(p) == null)
		{
			return ThoughtState.Inactive;
		}
		return ThoughtState.ActiveDefault;
	}
}
