using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class CultureDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__17 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CultureDef _003C_003E4__this;

		private List<ThingStyleCategoryWithPriority>.Enumerator _003C_003E7__wrap1;

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
		public _003CConfigErrors_003Ed__17(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<ThingStyleCategoryWithPriority>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				CultureDef cultureDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (!cultureDef.thingStyleCategories.NullOrEmpty())
					{
						_003C_003E7__wrap1 = cultureDef.thingStyleCategories.GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_00b6;
					}
					goto IL_00d5;
				case 1:
					_003C_003E1__state = -3;
					goto IL_00b6;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00ff;
				case 3:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_00b6:
					while (_003C_003E7__wrap1.MoveNext())
					{
						ThingStyleCategoryWithPriority current = _003C_003E7__wrap1.Current;
						if (current.priority <= 0f)
						{
							_003C_003E2__current = "style category " + current.category.LabelCap + " has <= 0 priority. It must be positive.";
							_003C_003E1__state = 1;
							return true;
						}
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<ThingStyleCategoryWithPriority>.Enumerator);
					goto IL_00d5;
					IL_00ff:
					if (ModsConfig.IdeologyActive && !cultureDef.allowedPlaceTags.Any())
					{
						_003C_003E2__current = "at least one allowedPlaceTags must be defined in ideology.";
						_003C_003E1__state = 3;
						return true;
					}
					break;
					IL_00d5:
					if (cultureDef.styleItemTags.NullOrEmpty())
					{
						_003C_003E2__current = "no style item tags defined.";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00ff;
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

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__17 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__17(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public RulePackDef pawnNameMaker;

	public RulePackDef pawnNameMakerFemale;

	public RulePackDef ideoNameMaker;

	public RulePackDef deityNameMaker;

	public RulePackDef deityTypeMaker;

	public RulePackDef leaderTitleMaker;

	public RulePackDef festivalNameMaker;

	public List<ThingStyleCategoryWithPriority> thingStyleCategories;

	public List<StyleItemTagWeighted> styleItemTags;

	public IdeoWeaponClassPair preferredWeaponClasses;

	[NoTranslate]
	public List<string> allowedPlaceTags = new List<string>();

	public string iconPath;

	public Color iconColor = Color.white;

	private Texture2D icon;

	public Texture2D Icon
	{
		get
		{
			if (icon == null)
			{
				icon = ContentFinder<Texture2D>.Get(iconPath);
			}
			return icon;
		}
	}

	public RulePackDef GetPawnNameMaker(Gender gender)
	{
		if (gender == Gender.Female && pawnNameMakerFemale != null)
		{
			return pawnNameMakerFemale;
		}
		return pawnNameMaker;
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__17))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__17(-2)
		{
			_003C_003E4__this = this
		};
	}
}
