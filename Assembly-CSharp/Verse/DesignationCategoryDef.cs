using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse;

public class DesignationCategoryDef : Def
{
	public struct BuildablePreceptBuilding
	{
		private BuildableDef buildable;

		private Precept_Building precept;

		public BuildableDef Buildable => buildable;

		public Precept_Building Precept => precept;

		public BuildablePreceptBuilding(BuildableDef buildable, Precept_Building precept)
		{
			this.buildable = buildable;
			this.precept = precept;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Cget_AllIdeoDesignators_003Eg__GetDesignatorsFromStyleCategory_007C18_2_003Ed : IEnumerable<Designator>, IEnumerable, IEnumerator<Designator>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Designator _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private StyleCategoryDef categoryDef;

		public StyleCategoryDef _003C_003E3__categoryDef;

		public DesignationCategoryDef _003C_003E4__this;

		private int _003Cj_003E5__2;

		Designator IEnumerator<Designator>.Current
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
		public _003C_003Cget_AllIdeoDesignators_003Eg__GetDesignatorsFromStyleCategory_007C18_2_003Ed(int _003C_003E1__state)
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
			DesignationCategoryDef designationCategoryDef = _003C_003E4__this;
			Designator_Dropdown designator_Dropdown;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (categoryDef.addDesignators != null)
				{
					_003Cj_003E5__2 = 0;
					goto IL_00a3;
				}
				goto IL_00bb;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0093;
			case 2:
				{
					_003C_003E1__state = -1;
					goto IL_0108;
				}
				IL_00a3:
				if (_003Cj_003E5__2 < categoryDef.addDesignators.Count)
				{
					if (categoryDef.addDesignators[_003Cj_003E5__2].designationCategory == designationCategoryDef)
					{
						_003C_003E2__current = designationCategoryDef._003Cget_AllIdeoDesignators_003Eg__GetCachedDesignator_007C18_0(categoryDef.addDesignators[_003Cj_003E5__2]);
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_0093;
				}
				goto IL_00bb;
				IL_0118:
				if (_003Cj_003E5__2 >= categoryDef.addDesignatorGroups.Count)
				{
					break;
				}
				designator_Dropdown = designationCategoryDef._003Cget_AllIdeoDesignators_003Eg__GetCachedDropdown_007C18_1(categoryDef.addDesignatorGroups[_003Cj_003E5__2]);
				if (designator_Dropdown != null)
				{
					_003C_003E2__current = designator_Dropdown;
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_0108;
				IL_0108:
				_003Cj_003E5__2++;
				goto IL_0118;
				IL_0093:
				_003Cj_003E5__2++;
				goto IL_00a3;
				IL_00bb:
				if (categoryDef.addDesignatorGroups == null)
				{
					break;
				}
				_003Cj_003E5__2 = 0;
				goto IL_0118;
			}
			return false;
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
		IEnumerator<Designator> IEnumerable<Designator>.GetEnumerator()
		{
			_003C_003Cget_AllIdeoDesignators_003Eg__GetDesignatorsFromStyleCategory_007C18_2_003Ed _003C_003Cget_AllIdeoDesignators_003Eg__GetDesignatorsFromStyleCategory_007C18_2_003Ed;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003C_003Cget_AllIdeoDesignators_003Eg__GetDesignatorsFromStyleCategory_007C18_2_003Ed = this;
			}
			else
			{
				_003C_003Cget_AllIdeoDesignators_003Eg__GetDesignatorsFromStyleCategory_007C18_2_003Ed = new _003C_003Cget_AllIdeoDesignators_003Eg__GetDesignatorsFromStyleCategory_007C18_2_003Ed(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003C_003Cget_AllIdeoDesignators_003Eg__GetDesignatorsFromStyleCategory_007C18_2_003Ed.categoryDef = _003C_003E3__categoryDef;
			return _003C_003Cget_AllIdeoDesignators_003Eg__GetDesignatorsFromStyleCategory_007C18_2_003Ed;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Designator>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_AllIdeoDesignators_003Ed__18 : IEnumerable<Designator>, IEnumerable, IEnumerator<Designator>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Designator _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public DesignationCategoryDef _003C_003E4__this;

		private IEnumerator<Ideo> _003C_003E7__wrap1;

		private Ideo _003Cideo_003E5__3;

		private int _003Ci_003E5__4;

		private IEnumerator<Designator> _003C_003E7__wrap4;

		private MemeDef _003Cmeme_003E5__6;

		private int _003Cj_003E5__7;

		private List<StyleCategoryDef>.Enumerator _003C_003E7__wrap7;

		Designator IEnumerator<Designator>.Current
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
		public _003Cget_AllIdeoDesignators_003Ed__18(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			switch (num)
			{
			case -4:
			case -3:
			case 1:
			case 2:
			case 3:
			case 4:
				try
				{
					if (num == -4 || num == 2)
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
				break;
			case -6:
			case -5:
			case 5:
				try
				{
					if (num == -6 || num == 5)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally4();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003Cideo_003E5__3 = null;
			_003C_003E7__wrap4 = null;
			_003Cmeme_003E5__6 = null;
			_003C_003E7__wrap7 = default(List<StyleCategoryDef>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				DesignationCategoryDef designationCategoryDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (!ModsConfig.IdeologyActive)
					{
						break;
					}
					if (designationCategoryDef.cachedPlayerFaction != Faction.OfPlayer)
					{
						designationCategoryDef.ideoBuildingDesignatorsCached.Clear();
						designationCategoryDef.ideoDropdownsCached.Clear();
						designationCategoryDef.cachedPlayerFaction = Faction.OfPlayer;
					}
					_003C_003E7__wrap1 = Faction.OfPlayer.ideos.AllIdeos.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0397;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0132;
				case 2:
					_003C_003E1__state = -4;
					goto IL_01d0;
				case 3:
					_003C_003E1__state = -3;
					goto IL_02af;
				case 4:
					_003C_003E1__state = -3;
					goto IL_0332;
				case 5:
					{
						_003C_003E1__state = -6;
						goto IL_0434;
					}
					IL_01fc:
					if (_003Ci_003E5__4 < _003Cideo_003E5__3.thingStyleCategories.Count)
					{
						ThingStyleCategoryWithPriority thingStyleCategoryWithPriority = _003Cideo_003E5__3.thingStyleCategories[_003Ci_003E5__4];
						_003C_003E7__wrap4 = designationCategoryDef._003Cget_AllIdeoDesignators_003Eg__GetDesignatorsFromStyleCategory_007C18_2(thingStyleCategoryWithPriority.category).GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_01d0;
					}
					_003Ci_003E5__4 = 0;
					goto IL_0375;
					IL_0375:
					if (_003Ci_003E5__4 < _003Cideo_003E5__3.memes.Count)
					{
						_003Cmeme_003E5__6 = _003Cideo_003E5__3.memes[_003Ci_003E5__4];
						if (_003Cmeme_003E5__6.addDesignators != null)
						{
							_003Cj_003E5__7 = 0;
							goto IL_02c1;
						}
						goto IL_02dc;
					}
					_003Cideo_003E5__3 = null;
					goto IL_0397;
					IL_0332:
					_003Cj_003E5__7++;
					goto IL_0344;
					IL_0397:
					if (_003C_003E7__wrap1.MoveNext())
					{
						_003Cideo_003E5__3 = _003C_003E7__wrap1.Current;
						_003Ci_003E5__4 = 0;
						goto IL_0144;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (!Find.IdeoManager.classicMode)
					{
						break;
					}
					_003C_003E7__wrap7 = Find.IdeoManager.selectedStyleCategories.GetEnumerator();
					_003C_003E1__state = -5;
					goto IL_044e;
					IL_035c:
					_003Cmeme_003E5__6 = null;
					_003Ci_003E5__4++;
					goto IL_0375;
					IL_0132:
					_003Ci_003E5__4++;
					goto IL_0144;
					IL_044e:
					if (_003C_003E7__wrap7.MoveNext())
					{
						StyleCategoryDef current = _003C_003E7__wrap7.Current;
						_003C_003E7__wrap4 = designationCategoryDef._003Cget_AllIdeoDesignators_003Eg__GetDesignatorsFromStyleCategory_007C18_2(current).GetEnumerator();
						_003C_003E1__state = -6;
						goto IL_0434;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap7 = default(List<StyleCategoryDef>.Enumerator);
					break;
					IL_02c1:
					if (_003Cj_003E5__7 < _003Cmeme_003E5__6.addDesignators.Count)
					{
						if (_003Cmeme_003E5__6.addDesignators[_003Cj_003E5__7].designationCategory == designationCategoryDef)
						{
							_003C_003E2__current = designationCategoryDef._003Cget_AllIdeoDesignators_003Eg__GetCachedDesignator_007C18_0(_003Cmeme_003E5__6.addDesignators[_003Cj_003E5__7]);
							_003C_003E1__state = 3;
							return true;
						}
						goto IL_02af;
					}
					goto IL_02dc;
					IL_0434:
					if (_003C_003E7__wrap4.MoveNext())
					{
						Designator current2 = _003C_003E7__wrap4.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 5;
						return true;
					}
					_003C_003Em__Finally4();
					_003C_003E7__wrap4 = null;
					goto IL_044e;
					IL_0344:
					if (_003Cj_003E5__7 < _003Cmeme_003E5__6.addDesignatorGroups.Count)
					{
						Designator_Dropdown designator_Dropdown = designationCategoryDef._003Cget_AllIdeoDesignators_003Eg__GetCachedDropdown_007C18_1(_003Cmeme_003E5__6.addDesignatorGroups[_003Cj_003E5__7]);
						if (designator_Dropdown != null)
						{
							_003C_003E2__current = designator_Dropdown;
							_003C_003E1__state = 4;
							return true;
						}
						goto IL_0332;
					}
					goto IL_035c;
					IL_02af:
					_003Cj_003E5__7++;
					goto IL_02c1;
					IL_01d0:
					if (_003C_003E7__wrap4.MoveNext())
					{
						Designator current3 = _003C_003E7__wrap4.Current;
						_003C_003E2__current = current3;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap4 = null;
					_003Ci_003E5__4++;
					goto IL_01fc;
					IL_0144:
					if (_003Ci_003E5__4 < _003Cideo_003E5__3.PreceptsListForReading.Count)
					{
						Precept precept = _003Cideo_003E5__3.PreceptsListForReading[_003Ci_003E5__4];
						bool flag = precept is Precept_Building;
						bool flag2 = precept is Precept_RitualSeat;
						if (flag || flag2)
						{
							Precept_ThingDef precept_ThingDef = (Precept_ThingDef)precept;
							if (precept_ThingDef.ThingDef.designationCategory == designationCategoryDef)
							{
								_003C_003E2__current = designationCategoryDef._003Cget_AllIdeoDesignators_003Eg__GetCachedDesignator_007C18_0(precept_ThingDef.ThingDef, precept_ThingDef as Precept_Building);
								_003C_003E1__state = 1;
								return true;
							}
						}
						goto IL_0132;
					}
					_003Ci_003E5__4 = 0;
					goto IL_01fc;
					IL_02dc:
					if (_003Cmeme_003E5__6.addDesignatorGroups != null)
					{
						_003Cj_003E5__7 = 0;
						goto IL_0344;
					}
					goto IL_035c;
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

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
			if (_003C_003E7__wrap4 != null)
			{
				_003C_003E7__wrap4.Dispose();
			}
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap7/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally4()
		{
			_003C_003E1__state = -5;
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
		IEnumerator<Designator> IEnumerable<Designator>.GetEnumerator()
		{
			_003Cget_AllIdeoDesignators_003Ed__18 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_AllIdeoDesignators_003Ed__18(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Designator>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_AllResolvedAndIdeoDesignators_003Ed__20 : IEnumerable<Designator>, IEnumerable, IEnumerator<Designator>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Designator _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public DesignationCategoryDef _003C_003E4__this;

		private List<Designator>.Enumerator _003C_003E7__wrap1;

		private IEnumerator<Designator> _003C_003E7__wrap2;

		Designator IEnumerator<Designator>.Current
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
		public _003Cget_AllResolvedAndIdeoDesignators_003Ed__20(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap1 = default(List<Designator>.Enumerator);
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				DesignationCategoryDef designationCategoryDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = designationCategoryDef.resolvedDesignators.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -4;
						break;
					}
					IL_0072:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Designator current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<Designator>.Enumerator);
					_003C_003E7__wrap2 = designationCategoryDef.AllIdeoDesignators.GetEnumerator();
					_003C_003E1__state = -4;
					break;
				}
				if (_003C_003E7__wrap2.MoveNext())
				{
					Designator current2 = _003C_003E7__wrap2.Current;
					_003C_003E2__current = current2;
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap2 = null;
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
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Designator> IEnumerable<Designator>.GetEnumerator()
		{
			_003Cget_AllResolvedAndIdeoDesignators_003Ed__20 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_AllResolvedAndIdeoDesignators_003Ed__20(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Designator>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_ResolvedAllowedDesignators_003Ed__14 : IEnumerable<Designator>, IEnumerable, IEnumerator<Designator>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Designator _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public DesignationCategoryDef _003C_003E4__this;

		private GameRules _003Crules_003E5__2;

		private int _003Ci_003E5__3;

		private IEnumerator<Designator> _003C_003E7__wrap3;

		Designator IEnumerator<Designator>.Current
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
		public _003Cget_ResolvedAllowedDesignators_003Ed__14(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 2)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003Crules_003E5__2 = null;
			_003C_003E7__wrap3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				DesignationCategoryDef designationCategoryDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003Crules_003E5__2 = Current.Game.Rules;
					_003Ci_003E5__3 = 0;
					goto IL_009d;
				case 1:
					_003C_003E1__state = -1;
					goto IL_008b;
				case 2:
					{
						_003C_003E1__state = -3;
						break;
					}
					IL_009d:
					if (_003Ci_003E5__3 < designationCategoryDef.resolvedDesignators.Count)
					{
						Designator d = designationCategoryDef.resolvedDesignators[_003Ci_003E5__3];
						if (_003Crules_003E5__2 == null || _003Crules_003E5__2.DesignatorAllowed(d))
						{
							_003C_003E2__current = d;
							_003C_003E1__state = 1;
							return true;
						}
						goto IL_008b;
					}
					_003C_003E7__wrap3 = designationCategoryDef.AllIdeoDesignators.GetEnumerator();
					_003C_003E1__state = -3;
					break;
					IL_008b:
					_003Ci_003E5__3++;
					goto IL_009d;
				}
				if (_003C_003E7__wrap3.MoveNext())
				{
					Designator current = _003C_003E7__wrap3.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap3 = null;
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
			if (_003C_003E7__wrap3 != null)
			{
				_003C_003E7__wrap3.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Designator> IEnumerable<Designator>.GetEnumerator()
		{
			_003Cget_ResolvedAllowedDesignators_003Ed__14 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_ResolvedAllowedDesignators_003Ed__14(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Designator>)this).GetEnumerator();
		}
	}

	public List<Type> specialDesignatorClasses = new List<Type>();

	public int order;

	public bool showPowerGrid;

	public List<ResearchProjectDef> researchPrerequisites;

	public int minMonolithLevel;

	public int? preferredColumn;

	[Unsaved(false)]
	private List<Designator> resolvedDesignators = new List<Designator>();

	[Unsaved(false)]
	public KeyBindingCategoryDef bindingCatDef;

	[Unsaved(false)]
	public string cachedHighlightClosedTag;

	[Unsaved(false)]
	private Dictionary<BuildablePreceptBuilding, Designator> ideoBuildingDesignatorsCached = new Dictionary<BuildablePreceptBuilding, Designator>();

	[Unsaved(false)]
	private Dictionary<DesignatorDropdownGroupDef, Designator_Dropdown> ideoDropdownsCached = new Dictionary<DesignatorDropdownGroupDef, Designator_Dropdown>();

	[Unsaved(false)]
	private Faction cachedPlayerFaction;

	public IEnumerable<Designator> ResolvedAllowedDesignators
	{
		[IteratorStateMachine(typeof(_003Cget_ResolvedAllowedDesignators_003Ed__14))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_ResolvedAllowedDesignators_003Ed__14(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public List<Designator> AllResolvedDesignators => resolvedDesignators;

	public IEnumerable<Designator> AllIdeoDesignators
	{
		[IteratorStateMachine(typeof(_003Cget_AllIdeoDesignators_003Ed__18))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_AllIdeoDesignators_003Ed__18(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public IEnumerable<Designator> AllResolvedAndIdeoDesignators
	{
		[IteratorStateMachine(typeof(_003Cget_AllResolvedAndIdeoDesignators_003Ed__20))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_AllResolvedAndIdeoDesignators_003Ed__20(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public bool Visible
	{
		get
		{
			if (DebugSettings.godMode)
			{
				return true;
			}
			if (ModsConfig.AnomalyActive && Find.Anomaly.HighestLevelReached < minMonolithLevel && Find.Anomaly.GenerateMonolith)
			{
				return false;
			}
			if (researchPrerequisites == null)
			{
				return true;
			}
			foreach (ResearchProjectDef researchPrerequisite in researchPrerequisites)
			{
				if (!researchPrerequisite.IsFinished)
				{
					return false;
				}
			}
			return true;
		}
	}

	public override void ResolveReferences()
	{
		base.ResolveReferences();
		LongEventHandler.ExecuteWhenFinished(delegate
		{
			ResolveDesignators();
		});
		cachedHighlightClosedTag = "DesignationCategoryButton-" + defName + "-Closed";
	}

	private void ResolveDesignators()
	{
		resolvedDesignators.Clear();
		foreach (Type specialDesignatorClass in specialDesignatorClasses)
		{
			Designator designator = null;
			try
			{
				designator = (Designator)Activator.CreateInstance(specialDesignatorClass);
				designator.isOrder = true;
			}
			catch (Exception ex)
			{
				Log.Error("DesignationCategoryDef" + defName + " could not instantiate special designator from class " + specialDesignatorClass?.ToString() + ".\n Exception: \n" + ex.ToString());
			}
			if (designator != null)
			{
				resolvedDesignators.Add(designator);
			}
		}
		IEnumerable<BuildableDef> enumerable = from tDef in DefDatabase<TerrainDef>.AllDefs.Cast<BuildableDef>().Concat(DefDatabase<ThingDef>.AllDefs.Cast<BuildableDef>())
			where tDef.designationCategory == this && tDef.canGenerateDefaultDesignator
			select tDef;
		Dictionary<DesignatorDropdownGroupDef, Designator_Dropdown> dictionary = new Dictionary<DesignatorDropdownGroupDef, Designator_Dropdown>();
		foreach (BuildableDef item in enumerable)
		{
			if (item.designatorDropdown != null)
			{
				if (!dictionary.ContainsKey(item.designatorDropdown))
				{
					dictionary[item.designatorDropdown] = new Designator_Dropdown();
					dictionary[item.designatorDropdown].Order = item.uiOrder;
					resolvedDesignators.Add(dictionary[item.designatorDropdown]);
				}
				dictionary[item.designatorDropdown].Add(new Designator_Build(item));
			}
			else
			{
				resolvedDesignators.Add(new Designator_Build(item));
			}
		}
	}

	public void DirtyCache()
	{
		ideoBuildingDesignatorsCached.Clear();
		ideoDropdownsCached.Clear();
	}

	private static bool RequirementSatisfied(string requirement)
	{
		if (!ModLister.HasActiveModWithName(requirement))
		{
			return DefDatabase<ResearchProjectDef>.GetNamedSilentFail(requirement)?.IsFinished ?? false;
		}
		return true;
	}

	[CompilerGenerated]
	private Designator _003Cget_AllIdeoDesignators_003Eg__GetCachedDesignator_007C18_0(BuildableDef def, Precept_Building buildingPrecept = null)
	{
		BuildablePreceptBuilding key = new BuildablePreceptBuilding(def, buildingPrecept);
		if (!ideoBuildingDesignatorsCached.TryGetValue(key, out var value))
		{
			Designator_Build designator_Build = new Designator_Build(def);
			value = designator_Build;
			if (buildingPrecept != null)
			{
				designator_Build.sourcePrecept = buildingPrecept;
			}
			ideoBuildingDesignatorsCached[key] = value;
		}
		return value;
	}

	[CompilerGenerated]
	private Designator_Dropdown _003Cget_AllIdeoDesignators_003Eg__GetCachedDropdown_007C18_1(DesignatorDropdownGroupDef group)
	{
		if (!ideoDropdownsCached.TryGetValue(group, out var value))
		{
			IEnumerable<BuildableDef> enumerable = from tDef in DefDatabase<TerrainDef>.AllDefs.Cast<BuildableDef>().Concat(DefDatabase<ThingDef>.AllDefs.Cast<BuildableDef>())
				where tDef.designationCategory == this && !tDef.canGenerateDefaultDesignator && tDef.designatorDropdown == @group
				select tDef;
			if (!enumerable.Any())
			{
				ideoDropdownsCached[group] = null;
				return ideoDropdownsCached[group];
			}
			foreach (BuildableDef item in enumerable)
			{
				if (!ideoDropdownsCached.ContainsKey(item.designatorDropdown))
				{
					ideoDropdownsCached[item.designatorDropdown] = new Designator_Dropdown();
					ideoDropdownsCached[item.designatorDropdown].Order = item.uiOrder;
				}
				ideoDropdownsCached[item.designatorDropdown].Add(new Designator_Build(item));
			}
			return ideoDropdownsCached[group];
		}
		return value;
	}

	[IteratorStateMachine(typeof(_003C_003Cget_AllIdeoDesignators_003Eg__GetDesignatorsFromStyleCategory_007C18_2_003Ed))]
	[CompilerGenerated]
	private IEnumerable<Designator> _003Cget_AllIdeoDesignators_003Eg__GetDesignatorsFromStyleCategory_007C18_2(StyleCategoryDef categoryDef)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003C_003Cget_AllIdeoDesignators_003Eg__GetDesignatorsFromStyleCategory_007C18_2_003Ed(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__categoryDef = categoryDef
		};
	}
}
