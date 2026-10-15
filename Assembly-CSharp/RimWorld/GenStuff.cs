using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public static class GenStuff
{
	[CompilerGenerated]
	private sealed class _003CAllowedStuffs_003Ed__7 : IEnumerable<ThingDef>, IEnumerable, IEnumerator<ThingDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThingDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private TechLevel maxTechLevel;

		public TechLevel _003C_003E3__maxTechLevel;

		private List<StuffCategoryDef> categories;

		public List<StuffCategoryDef> _003C_003E3__categories;

		private int _003Ci_003E5__2;

		ThingDef IEnumerator<ThingDef>.Current
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
		public _003CAllowedStuffs_003Ed__7(int _003C_003E1__state)
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
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_00d0;
			}
			_003C_003E1__state = -1;
			_003Ci_003E5__2 = 0;
			goto IL_00e2;
			IL_00d0:
			_003Ci_003E5__2++;
			goto IL_00e2;
			IL_00e2:
			if (_003Ci_003E5__2 < StuffDefs.Count)
			{
				ThingDef thingDef = StuffDefs[_003Ci_003E5__2];
				if (thingDef.IsStuff && (maxTechLevel == TechLevel.Undefined || (int)thingDef.techLevel <= (int)maxTechLevel))
				{
					bool flag = false;
					for (int i = 0; i < thingDef.stuffProps.categories.Count; i++)
					{
						for (int j = 0; j < categories.Count; j++)
						{
							if (thingDef.stuffProps.categories[i] == categories[j])
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							break;
						}
					}
					if (flag)
					{
						_003C_003E2__current = thingDef;
						_003C_003E1__state = 1;
						return true;
					}
				}
				goto IL_00d0;
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
		IEnumerator<ThingDef> IEnumerable<ThingDef>.GetEnumerator()
		{
			_003CAllowedStuffs_003Ed__7 _003CAllowedStuffs_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CAllowedStuffs_003Ed__ = this;
			}
			else
			{
				_003CAllowedStuffs_003Ed__ = new _003CAllowedStuffs_003Ed__7(0);
			}
			_003CAllowedStuffs_003Ed__.categories = _003C_003E3__categories;
			_003CAllowedStuffs_003Ed__.maxTechLevel = _003C_003E3__maxTechLevel;
			return _003CAllowedStuffs_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ThingDef>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CAllowedStuffsFor_003Ed__6 : IEnumerable<ThingDef>, IEnumerable, IEnumerator<ThingDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThingDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private BuildableDef td;

		public BuildableDef _003C_003E3__td;

		private TechLevel maxTechLevel;

		public TechLevel _003C_003E3__maxTechLevel;

		private bool checkAllowedInStuffGeneration;

		public bool _003C_003E3__checkAllowedInStuffGeneration;

		private ThingDef _003Cfound_003E5__2;

		private int _003Cyielded_003E5__3;

		private int _003Ci_003E5__4;

		ThingDef IEnumerator<ThingDef>.Current
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
		public _003CAllowedStuffsFor_003Ed__6(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cfound_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!td.MadeFromStuff)
				{
					return false;
				}
				_003Cfound_003E5__2 = null;
				_003Cyielded_003E5__3 = 0;
				_003Ci_003E5__4 = 0;
				goto IL_00e0;
			case 1:
				_003C_003E1__state = -1;
				goto IL_00d0;
			case 2:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_00d0:
				_003Ci_003E5__4++;
				goto IL_00e0;
				IL_00e0:
				if (_003Ci_003E5__4 < StuffDefs.Count)
				{
					ThingDef thingDef = StuffDefs[_003Ci_003E5__4];
					if (thingDef.IsStuff && (maxTechLevel == TechLevel.Undefined || (int)thingDef.techLevel <= (int)maxTechLevel) && thingDef.stuffProps.CanMake(td))
					{
						_003Cfound_003E5__2 = thingDef;
						if (!checkAllowedInStuffGeneration || thingDef.stuffProps.allowedInStuffGeneration)
						{
							_003Cyielded_003E5__3++;
							_003C_003E2__current = thingDef;
							_003C_003E1__state = 1;
							return true;
						}
					}
					goto IL_00d0;
				}
				if (_003Cyielded_003E5__3 == 0 && _003Cfound_003E5__2 != null)
				{
					_003C_003E2__current = _003Cfound_003E5__2;
					_003C_003E1__state = 2;
					return true;
				}
				break;
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
		IEnumerator<ThingDef> IEnumerable<ThingDef>.GetEnumerator()
		{
			_003CAllowedStuffsFor_003Ed__6 _003CAllowedStuffsFor_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CAllowedStuffsFor_003Ed__ = this;
			}
			else
			{
				_003CAllowedStuffsFor_003Ed__ = new _003CAllowedStuffsFor_003Ed__6(0);
			}
			_003CAllowedStuffsFor_003Ed__.td = _003C_003E3__td;
			_003CAllowedStuffsFor_003Ed__.maxTechLevel = _003C_003E3__maxTechLevel;
			_003CAllowedStuffsFor_003Ed__.checkAllowedInStuffGeneration = _003C_003E3__checkAllowedInStuffGeneration;
			return _003CAllowedStuffsFor_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ThingDef>)this).GetEnumerator();
		}
	}

	private static List<ThingDef> cachedStuffDefs;

	private static List<ThingDef> allowedStuffTmp = new List<ThingDef>();

	private static List<ThingDef> StuffDefs
	{
		get
		{
			if (cachedStuffDefs == null)
			{
				cachedStuffDefs = new List<ThingDef>();
				foreach (ThingDef item in DefDatabase<ThingDef>.AllDefsListForReading)
				{
					if (item.IsStuff)
					{
						cachedStuffDefs.Add(item);
					}
				}
			}
			return cachedStuffDefs;
		}
	}

	public static ThingDef DefaultStuffFor(BuildableDef bd)
	{
		if (!bd.MadeFromStuff)
		{
			return null;
		}
		if (bd is ThingDef thingDef)
		{
			if (thingDef.defaultStuff != null)
			{
				return thingDef.defaultStuff;
			}
			if (thingDef.IsMeleeWeapon)
			{
				if (ThingDefOf.Steel.stuffProps.CanMake(bd))
				{
					return ThingDefOf.Steel;
				}
				if (ThingDefOf.Plasteel.stuffProps.CanMake(bd))
				{
					return ThingDefOf.Plasteel;
				}
			}
			if (thingDef.IsApparel)
			{
				if (ThingDefOf.Cloth.stuffProps.CanMake(bd))
				{
					return ThingDefOf.Cloth;
				}
				if (ThingDefOf.Leather_Plain.stuffProps.CanMake(bd))
				{
					return ThingDefOf.Leather_Plain;
				}
				if (ThingDefOf.Steel.stuffProps.CanMake(bd))
				{
					return ThingDefOf.Steel;
				}
			}
		}
		if (ThingDefOf.WoodLog.stuffProps.CanMake(bd))
		{
			return ThingDefOf.WoodLog;
		}
		if (ThingDefOf.Steel.stuffProps.CanMake(bd))
		{
			return ThingDefOf.Steel;
		}
		if (ThingDefOf.Plasteel.stuffProps.CanMake(bd))
		{
			return ThingDefOf.Plasteel;
		}
		if (ThingDefOf.BlocksGranite.stuffProps.CanMake(bd))
		{
			return ThingDefOf.BlocksGranite;
		}
		if (ThingDefOf.Cloth.stuffProps.CanMake(bd))
		{
			return ThingDefOf.Cloth;
		}
		if (ThingDefOf.Leather_Plain.stuffProps.CanMake(bd))
		{
			return ThingDefOf.Leather_Plain;
		}
		return AllowedStuffsFor(bd).First();
	}

	public static ThingDef RandomStuffFor(ThingDef td)
	{
		if (!td.MadeFromStuff)
		{
			return null;
		}
		return AllowedStuffsFor(td, TechLevel.Undefined, checkAllowedInStuffGeneration: true).RandomElement();
	}

	public static ThingDef RandomStuffByCommonalityFor(ThingDef td, TechLevel maxTechLevel = TechLevel.Undefined)
	{
		if (!td.MadeFromStuff)
		{
			return null;
		}
		if (!TryRandomStuffByCommonalityFor(td, out var stuff, maxTechLevel))
		{
			return DefaultStuffFor(td);
		}
		return stuff;
	}

	[IteratorStateMachine(typeof(_003CAllowedStuffsFor_003Ed__6))]
	public static IEnumerable<ThingDef> AllowedStuffsFor(BuildableDef td, TechLevel maxTechLevel = TechLevel.Undefined, bool checkAllowedInStuffGeneration = false)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAllowedStuffsFor_003Ed__6(-2)
		{
			_003C_003E3__td = td,
			_003C_003E3__maxTechLevel = maxTechLevel,
			_003C_003E3__checkAllowedInStuffGeneration = checkAllowedInStuffGeneration
		};
	}

	[IteratorStateMachine(typeof(_003CAllowedStuffs_003Ed__7))]
	public static IEnumerable<ThingDef> AllowedStuffs(List<StuffCategoryDef> categories, TechLevel maxTechLevel = TechLevel.Undefined)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAllowedStuffs_003Ed__7(-2)
		{
			_003C_003E3__categories = categories,
			_003C_003E3__maxTechLevel = maxTechLevel
		};
	}

	public static bool TryRandomStuffByCommonalityFor(ThingDef td, out ThingDef stuff, TechLevel maxTechLevel = TechLevel.Undefined)
	{
		if (!td.MadeFromStuff)
		{
			stuff = null;
			return true;
		}
		return AllowedStuffsFor(td, maxTechLevel, checkAllowedInStuffGeneration: true).TryRandomElementByWeight((ThingDef x) => x.stuffProps.commonality, out stuff);
	}

	public static bool TryRandomStuffFor(ThingDef td, out ThingDef stuff, TechLevel maxTechLevel = TechLevel.Undefined, Predicate<ThingDef> validator = null)
	{
		if (!td.MadeFromStuff)
		{
			stuff = null;
			return true;
		}
		if (validator != null)
		{
			allowedStuffTmp.Clear();
			foreach (ThingDef item in AllowedStuffsFor(td, maxTechLevel, checkAllowedInStuffGeneration: true))
			{
				if (validator(item))
				{
					allowedStuffTmp.Add(item);
				}
			}
		}
		else
		{
			allowedStuffTmp.Clear();
			allowedStuffTmp.AddRange(AllowedStuffsFor(td, maxTechLevel));
		}
		return allowedStuffTmp.TryRandomElement(out stuff);
	}

	public static ThingDef RandomStuffInexpensiveFor(ThingDef thingDef, Faction faction, Predicate<ThingDef> validator = null)
	{
		return RandomStuffInexpensiveFor(thingDef, faction?.def.techLevel ?? TechLevel.Undefined, validator);
	}

	public static ThingDef RandomStuffInexpensiveFor(ThingDef thingDef, TechLevel maxTechLevel = TechLevel.Undefined, Predicate<ThingDef> validator = null)
	{
		if (!thingDef.MadeFromStuff)
		{
			return null;
		}
		IEnumerable<ThingDef> enumerable = AllowedStuffsFor(thingDef, maxTechLevel, checkAllowedInStuffGeneration: true);
		if (validator != null)
		{
			enumerable = enumerable.Where((ThingDef x) => validator(x));
		}
		float cheapestPrice = -1f;
		foreach (ThingDef item in enumerable)
		{
			float num = item.BaseMarketValue / item.VolumePerUnit;
			if (cheapestPrice == -1f || num < cheapestPrice)
			{
				cheapestPrice = num;
			}
		}
		enumerable = enumerable.Where((ThingDef x) => x.BaseMarketValue / x.VolumePerUnit <= cheapestPrice * 4f);
		if (enumerable.TryRandomElementByWeight((ThingDef x) => x.stuffProps.commonality, out var result))
		{
			return result;
		}
		return null;
	}

	public static void ResetStaticData()
	{
		cachedStuffDefs = null;
	}
}
