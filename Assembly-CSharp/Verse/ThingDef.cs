using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse;

public class ThingDef : BuildableDef
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass401_0
	{
		public StatModifier statBase;

		internal bool _003CConfigErrors_003Eb__4(StatModifier st)
		{
			return st.stat == statBase.stat;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass401_1
	{
		public ThingCategoryDef cat;

		internal bool _003CConfigErrors_003Eb__5(ThingCategoryDef c)
		{
			return c == cat;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass401_2
	{
		public DamageMultiplier mult;

		internal bool _003CConfigErrors_003Eb__6(DamageMultiplier m)
		{
			return m.damageDef == mult.damageDef;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass401_3
	{
		public int i;

		public ThingDef _003C_003E4__this;

		internal bool _003CConfigErrors_003Eb__7(BodyPartRecord part)
		{
			return part.groups.Contains(_003C_003E4__this.tools[i].linkedBodyPartsGroup);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass401_4
	{
		public Tool lhs;

		internal bool _003CConfigErrors_003Eb__9(Tool rhs)
		{
			if (lhs != rhs)
			{
				return lhs.id == rhs.id;
			}
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass407_0
	{
		public ThingDef p;

		internal bool _003CSpecialDisplayStats_003Eb__9(Dialog_InfoCard.Hyperlink x)
		{
			return x.def == p;
		}
	}

	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__401 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ThingDef _003C_003E4__this;

		private _003C_003Ec__DisplayClass401_3 _003C_003E8__1;

		private IEnumerator<string> _003C_003E7__wrap1;

		private List<StatModifier>.Enumerator _003C_003E7__wrap2;

		private List<ThingDefCountClass>.Enumerator _003C_003E7__wrap3;

		private List<DamageMultiplier>.Enumerator _003C_003E7__wrap4;

		private int _003Ci_003E5__6;

		private List<Tool>.Enumerator _003C_003E7__wrap6;

		private List<ThingStyleChance>.Enumerator _003C_003E7__wrap7;

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
		public _003CConfigErrors_003Ed__401(int _003C_003E1__state)
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
			case 4:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			case -5:
			case 5:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			case -6:
			case 6:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally4();
				}
				break;
			case -7:
			case 13:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally5();
				}
				break;
			case -8:
			case 21:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally6();
				}
				break;
			case -9:
			case 40:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally7();
				}
				break;
			case -10:
			case 41:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally8();
				}
				break;
			case -11:
			case 43:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally9();
				}
				break;
			case -12:
			case 44:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally10();
				}
				break;
			case -13:
			case 45:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally11();
				}
				break;
			case -14:
			case 48:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally12();
				}
				break;
			case -15:
			case 49:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally13();
				}
				break;
			case -17:
			case -16:
			case 51:
				try
				{
					if (num == -17 || num == 51)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally15();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally14();
				}
				break;
			case -18:
			case 52:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally16();
				}
				break;
			}
			_003C_003E8__1 = null;
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = default(List<StatModifier>.Enumerator);
			_003C_003E7__wrap3 = default(List<ThingDefCountClass>.Enumerator);
			_003C_003E7__wrap4 = default(List<DamageMultiplier>.Enumerator);
			_003C_003E7__wrap6 = default(List<Tool>.Enumerator);
			_003C_003E7__wrap7 = default(List<ThingStyleChance>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				ThingDef CS_0024_003C_003E8__locals164 = _003C_003E4__this;
				ThingCategoryDef thingCategoryDef;
				int num2;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((BuildableDef)CS_0024_003C_003E8__locals164).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_014c;
				case 1:
					_003C_003E1__state = -3;
					goto IL_014c;
				case 2:
					_003C_003E1__state = -1;
					goto IL_019d;
				case 3:
					_003C_003E1__state = -1;
					goto IL_0215;
				case 4:
					_003C_003E1__state = -4;
					goto IL_0269;
				case 5:
					_003C_003E1__state = -5;
					goto IL_02d7;
				case 6:
					_003C_003E1__state = -6;
					goto IL_038e;
				case 7:
					_003C_003E1__state = -1;
					goto IL_0405;
				case 8:
					_003C_003E1__state = -1;
					goto IL_045a;
				case 9:
					_003C_003E1__state = -1;
					goto IL_049a;
				case 10:
					_003C_003E1__state = -1;
					goto IL_04c9;
				case 11:
					_003C_003E1__state = -1;
					goto IL_050f;
				case 12:
					_003C_003E1__state = -1;
					goto IL_0549;
				case 13:
					_003C_003E1__state = -7;
					goto IL_05c4;
				case 14:
					_003C_003E1__state = -1;
					goto IL_063d;
				case 15:
					_003C_003E1__state = -1;
					goto IL_0670;
				case 16:
					_003C_003E1__state = -1;
					goto IL_06a9;
				case 17:
					_003C_003E1__state = -1;
					goto IL_06f6;
				case 18:
					_003C_003E1__state = -1;
					goto IL_073b;
				case 19:
					_003C_003E1__state = -1;
					goto IL_0782;
				case 20:
					_003C_003E1__state = -1;
					goto IL_07b4;
				case 21:
					_003C_003E1__state = -8;
					goto IL_085f;
				case 22:
					_003C_003E1__state = -1;
					goto IL_08a3;
				case 23:
					_003C_003E1__state = -1;
					goto IL_08d4;
				case 24:
					_003C_003E1__state = -1;
					goto IL_090a;
				case 25:
					_003C_003E1__state = -1;
					goto IL_0949;
				case 26:
					_003C_003E1__state = -1;
					goto IL_0980;
				case 27:
					_003C_003E1__state = -1;
					goto IL_09c4;
				case 28:
					_003C_003E1__state = -1;
					goto IL_09f5;
				case 29:
					_003C_003E1__state = -1;
					goto IL_0a50;
				case 30:
					_003C_003E1__state = -1;
					goto IL_0a93;
				case 31:
					_003C_003E1__state = -1;
					goto IL_0ac9;
				case 32:
					_003C_003E1__state = -1;
					goto IL_0b08;
				case 33:
					_003C_003E1__state = -1;
					goto IL_0b94;
				case 34:
					_003C_003E1__state = -1;
					goto IL_0bd0;
				case 35:
					_003C_003E1__state = -1;
					goto IL_0c17;
				case 36:
					_003C_003E1__state = -1;
					goto IL_0c59;
				case 37:
					_003C_003E1__state = -1;
					goto IL_0c8a;
				case 38:
					_003C_003E1__state = -1;
					goto IL_0cc8;
				case 39:
					_003C_003E1__state = -1;
					goto IL_0cff;
				case 40:
					_003C_003E1__state = -9;
					goto IL_0daa;
				case 41:
					_003C_003E1__state = -10;
					goto IL_0e44;
				case 42:
					_003C_003E1__state = -1;
					goto IL_0ea2;
				case 43:
					_003C_003E1__state = -11;
					goto IL_0ef7;
				case 44:
					_003C_003E1__state = -12;
					goto IL_0f80;
				case 45:
					_003C_003E1__state = -13;
					goto IL_101a;
				case 46:
					_003C_003E1__state = -1;
					goto IL_1150;
				case 47:
					_003C_003E1__state = -1;
					goto IL_11cf;
				case 48:
					_003C_003E1__state = -14;
					goto IL_1223;
				case 49:
					_003C_003E1__state = -15;
					goto IL_1291;
				case 50:
					_003C_003E1__state = -1;
					goto IL_1305;
				case 51:
					_003C_003E1__state = -17;
					goto IL_1375;
				case 52:
					_003C_003E1__state = -18;
					goto IL_1412;
				case 53:
					_003C_003E1__state = -1;
					goto IL_147e;
				case 54:
					_003C_003E1__state = -1;
					goto IL_14b5;
				case 55:
					_003C_003E1__state = -1;
					goto IL_14e8;
				case 56:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_014c:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (CS_0024_003C_003E8__locals164.category != ThingCategory.Ethereal && CS_0024_003C_003E8__locals164.label.NullOrEmpty())
					{
						_003C_003E2__current = "no label";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_019d;
					IL_116c:
					if (_003C_003E8__1.i < CS_0024_003C_003E8__locals164.tools.Count)
					{
						if (CS_0024_003C_003E8__locals164.tools[_003C_003E8__1.i].linkedBodyPartsGroup != null && !CS_0024_003C_003E8__locals164.race.body.AllParts.Any((BodyPartRecord part) => part.groups.Contains(_003C_003E8__1._003C_003E4__this.tools[_003C_003E8__1.i].linkedBodyPartsGroup)))
						{
							_003C_003E2__current = "has tool with linkedBodyPartsGroup " + CS_0024_003C_003E8__locals164.tools[_003C_003E8__1.i].linkedBodyPartsGroup?.ToString() + " but body " + CS_0024_003C_003E8__locals164.race.body?.ToString() + " has no parts with that group.";
							_003C_003E1__state = 46;
							return true;
						}
						goto IL_1150;
					}
					_003C_003E8__1 = null;
					goto IL_118e;
					IL_14e8:
					if (CS_0024_003C_003E8__locals164.Fillage != FillCategory.Full && CS_0024_003C_003E8__locals164.passability == Traversability.Impassable && !CS_0024_003C_003E8__locals164.IsDoor && CS_0024_003C_003E8__locals164.BuildableByPlayer && !CS_0024_003C_003E8__locals164.disableImpassableShotOverConfigError)
					{
						_003C_003E2__current = "impassable, player-buildable building that can be shot/seen over.";
						_003C_003E1__state = 56;
						return true;
					}
					break;
					IL_019d:
					if (CS_0024_003C_003E8__locals164.category == ThingCategory.Building && !CS_0024_003C_003E8__locals164.IsFrame && CS_0024_003C_003E8__locals164.building.IsDeconstructible && CS_0024_003C_003E8__locals164.thingClass != null && typeof(Building).IsSubclassOf(CS_0024_003C_003E8__locals164.thingClass))
					{
						_003C_003E2__current = "has building category and is marked as deconstructible, but thing class is not a subclass of building (" + CS_0024_003C_003E8__locals164.thingClass.Name + ")";
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_0215;
					IL_085f:
					_003C_003Em__Finally6();
					_003C_003E7__wrap4 = default(List<DamageMultiplier>.Enumerator);
					goto IL_0871;
					IL_0871:
					if (CS_0024_003C_003E8__locals164.Fillage == FillCategory.Full && !CS_0024_003C_003E8__locals164.IsEdifice())
					{
						_003C_003E2__current = "fillPercent is 1.00 but is not edifice";
						_003C_003E1__state = 22;
						return true;
					}
					goto IL_08a3;
					IL_123d:
					if (CS_0024_003C_003E8__locals164.plant != null)
					{
						_003C_003E7__wrap1 = CS_0024_003C_003E8__locals164.plant.ConfigErrors().GetEnumerator();
						_003C_003E1__state = -15;
						goto IL_1291;
					}
					goto IL_12ab;
					IL_14b5:
					if (CS_0024_003C_003E8__locals164.hasInteractionCell && !CS_0024_003C_003E8__locals164.multipleInteractionCellOffsets.NullOrEmpty())
					{
						_003C_003E2__current = "both single and multiple interaction cells are defined, it should be one or the other";
						_003C_003E1__state = 55;
						return true;
					}
					goto IL_14e8;
					IL_08a3:
					if (CS_0024_003C_003E8__locals164.MadeFromStuff && CS_0024_003C_003E8__locals164.constructEffect != null)
					{
						_003C_003E2__current = "madeFromStuff but has a defined constructEffect (which will always be overridden by stuff's construct animation).";
						_003C_003E1__state = 23;
						return true;
					}
					goto IL_08d4;
					IL_0215:
					if (CS_0024_003C_003E8__locals164.graphicData != null)
					{
						_003C_003E7__wrap1 = CS_0024_003C_003E8__locals164.graphicData.ConfigErrors(CS_0024_003C_003E8__locals164).GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_0269;
					}
					goto IL_0283;
					IL_0b94:
					if (CS_0024_003C_003E8__locals164.ingestible == null && CS_0024_003C_003E8__locals164.GetStatValueAbstract(StatDefOf.Nutrition) != 0f)
					{
						_003C_003E2__current = "has nutrition but ingestible properties are null";
						_003C_003E1__state = 34;
						return true;
					}
					goto IL_0bd0;
					IL_0269:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 4;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = null;
					goto IL_0283;
					IL_1375:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current3 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current3;
						_003C_003E1__state = 51;
						return true;
					}
					_003C_003Em__Finally15();
					_003C_003E7__wrap1 = null;
					goto IL_138f;
					IL_08d4:
					if (CS_0024_003C_003E8__locals164.MadeFromStuff && CS_0024_003C_003E8__locals164.stuffCategories.NullOrEmpty())
					{
						_003C_003E2__current = "madeFromStuff but has no stuffCategories.";
						_003C_003E1__state = 24;
						return true;
					}
					goto IL_090a;
					IL_0283:
					if (CS_0024_003C_003E8__locals164.projectile != null)
					{
						_003C_003E7__wrap1 = CS_0024_003C_003E8__locals164.projectile.ConfigErrors(CS_0024_003C_003E8__locals164).GetEnumerator();
						_003C_003E1__state = -5;
						goto IL_02d7;
					}
					goto IL_02f1;
					IL_0ea2:
					if (CS_0024_003C_003E8__locals164.apparel != null)
					{
						_003C_003E7__wrap1 = CS_0024_003C_003E8__locals164.apparel.ConfigErrors(CS_0024_003C_003E8__locals164).GetEnumerator();
						_003C_003E1__state = -11;
						goto IL_0ef7;
					}
					goto IL_0f11;
					IL_02d7:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current4 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current4;
						_003C_003E1__state = 5;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap1 = null;
					goto IL_02f1;
					IL_0bd0:
					if (CS_0024_003C_003E8__locals164.BaseFlammability != 0f && !CS_0024_003C_003E8__locals164.useHitPoints && CS_0024_003C_003E8__locals164.category != ThingCategory.Pawn && !CS_0024_003C_003E8__locals164.destroyOnDrop)
					{
						_003C_003E2__current = "flammable but has no hitpoints (will burn indefinitely)";
						_003C_003E1__state = 35;
						return true;
					}
					goto IL_0c17;
					IL_090a:
					if (CS_0024_003C_003E8__locals164.costList.NullOrEmpty() && CS_0024_003C_003E8__locals164.costStuffCount <= 0 && CS_0024_003C_003E8__locals164.recipeMaker != null)
					{
						_003C_003E2__current = "has a recipeMaker but no costList or costStuffCount.";
						_003C_003E1__state = 25;
						return true;
					}
					goto IL_0949;
					IL_02f1:
					if (CS_0024_003C_003E8__locals164.statBases != null)
					{
						_003C_003E7__wrap2 = CS_0024_003C_003E8__locals164.statBases.GetEnumerator();
						_003C_003E1__state = -6;
						goto IL_038e;
					}
					goto IL_03b0;
					IL_13ae:
					if (!CS_0024_003C_003E8__locals164.randomStyle.NullOrEmpty())
					{
						_003C_003E7__wrap7 = CS_0024_003C_003E8__locals164.randomStyle.GetEnumerator();
						_003C_003E1__state = -18;
						goto IL_1412;
					}
					goto IL_147e;
					IL_038e:
					while (_003C_003E7__wrap2.MoveNext())
					{
						_003C_003Ec__DisplayClass401_0 CS_0024_003C_003E8__locals55 = new _003C_003Ec__DisplayClass401_0
						{
							statBase = _003C_003E7__wrap2.Current
						};
						if (CS_0024_003C_003E8__locals164.statBases.Count((StatModifier st) => st.stat == CS_0024_003C_003E8__locals55.statBase.stat) > 1)
						{
							_003C_003E2__current = "defines the stat base " + CS_0024_003C_003E8__locals55.statBase.stat?.ToString() + " more than once.";
							_003C_003E1__state = 6;
							return true;
						}
					}
					_003C_003Em__Finally4();
					_003C_003E7__wrap2 = default(List<StatModifier>.Enumerator);
					goto IL_03b0;
					IL_0ef7:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current5 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current5;
						_003C_003E1__state = 43;
						return true;
					}
					_003C_003Em__Finally9();
					_003C_003E7__wrap1 = null;
					goto IL_0f11;
					IL_03b0:
					if (!BeautyUtility.BeautyRelevant(CS_0024_003C_003E8__locals164.category) && CS_0024_003C_003E8__locals164.StatBaseDefined(StatDefOf.Beauty))
					{
						_003C_003E2__current = "Beauty stat base is defined, but Things of category " + CS_0024_003C_003E8__locals164.category.ToString() + " cannot have beauty.";
						_003C_003E1__state = 7;
						return true;
					}
					goto IL_0405;
					IL_1291:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current6 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current6;
						_003C_003E1__state = 49;
						return true;
					}
					_003C_003Em__Finally13();
					_003C_003E7__wrap1 = null;
					goto IL_12ab;
					IL_0949:
					if (CS_0024_003C_003E8__locals164.costStuffCount > 0 && CS_0024_003C_003E8__locals164.stuffCategories.NullOrEmpty())
					{
						_003C_003E2__current = "has costStuffCount but no stuffCategories.";
						_003C_003E1__state = 26;
						return true;
					}
					goto IL_0980;
					IL_0405:
					if (!BeautyUtility.BeautyRelevant(CS_0024_003C_003E8__locals164.category) && CS_0024_003C_003E8__locals164.StatBaseDefined(StatDefOf.BeautyOutdoors))
					{
						_003C_003E2__current = "BeautyOutdoors stat base is defined, but Things of category " + CS_0024_003C_003E8__locals164.category.ToString() + " cannot have beauty.";
						_003C_003E1__state = 8;
						return true;
					}
					goto IL_045a;
					IL_147e:
					if (CS_0024_003C_003E8__locals164.relicChance > 0f && CS_0024_003C_003E8__locals164.category != ThingCategory.Item)
					{
						_003C_003E2__current = "relic chance > 0 but category != item";
						_003C_003E1__state = 54;
						return true;
					}
					goto IL_14b5;
					IL_0c17:
					if (CS_0024_003C_003E8__locals164.graphicData?.shadowData != null && CS_0024_003C_003E8__locals164.staticSunShadowHeight > 0f)
					{
						_003C_003E2__current = "graphicData defines a shadowInfo but staticSunShadowHeight > 0";
						_003C_003E1__state = 36;
						return true;
					}
					goto IL_0c59;
					IL_045a:
					if (char.IsNumber(CS_0024_003C_003E8__locals164.defName[CS_0024_003C_003E8__locals164.defName.Length - 1]))
					{
						_003C_003E2__current = "ends with a numerical digit, which is not allowed on ThingDefs.";
						_003C_003E1__state = 9;
						return true;
					}
					goto IL_049a;
					IL_0980:
					if (CS_0024_003C_003E8__locals164.GetStatValueAbstract(StatDefOf.DeteriorationRate) > 1E-05f && !CS_0024_003C_003E8__locals164.CanEverDeteriorate && !CS_0024_003C_003E8__locals164.destroyOnDrop)
					{
						_003C_003E2__current = "has >0 DeteriorationRate but can't deteriorate.";
						_003C_003E1__state = 27;
						return true;
					}
					goto IL_09c4;
					IL_049a:
					if (CS_0024_003C_003E8__locals164.thingClass == null)
					{
						_003C_003E2__current = "has null thingClass.";
						_003C_003E1__state = 10;
						return true;
					}
					goto IL_04c9;
					IL_1412:
					while (_003C_003E7__wrap7.MoveNext())
					{
						if (_003C_003E7__wrap7.Current.Chance <= 0f)
						{
							_003C_003E2__current = "style chance <= 0.";
							_003C_003E1__state = 52;
							return true;
						}
					}
					_003C_003Em__Finally16();
					_003C_003E7__wrap7 = default(List<ThingStyleChance>.Enumerator);
					if (!CS_0024_003C_003E8__locals164.comps.Any((CompProperties c) => c.compClass == typeof(CompStyleable)))
					{
						_003C_003E2__current = "random style assigned, but missing CompStyleable!";
						_003C_003E1__state = 53;
						return true;
					}
					goto IL_147e;
					IL_04c9:
					if (CS_0024_003C_003E8__locals164.comps.Count > 0 && !typeof(ThingWithComps).IsAssignableFrom(CS_0024_003C_003E8__locals164.thingClass))
					{
						_003C_003E2__current = "has components but it's thingClass is not a ThingWithComps";
						_003C_003E1__state = 11;
						return true;
					}
					goto IL_050f;
					IL_0fac:
					if (_003Ci_003E5__6 < CS_0024_003C_003E8__locals164.comps.Count)
					{
						_003C_003E7__wrap1 = CS_0024_003C_003E8__locals164.comps[_003Ci_003E5__6].ConfigErrors(CS_0024_003C_003E8__locals164).GetEnumerator();
						_003C_003E1__state = -12;
						goto IL_0f80;
					}
					goto IL_0fc2;
					IL_0f11:
					if (CS_0024_003C_003E8__locals164.comps != null)
					{
						_003Ci_003E5__6 = 0;
						goto IL_0fac;
					}
					goto IL_0fc2;
					IL_050f:
					if (CS_0024_003C_003E8__locals164.ConnectToPower && CS_0024_003C_003E8__locals164.drawerType == DrawerType.RealtimeOnly && CS_0024_003C_003E8__locals164.IsFrame)
					{
						_003C_003E2__current = "connects to power but does not add to map mesh. Will not create wire meshes.";
						_003C_003E1__state = 12;
						return true;
					}
					goto IL_0549;
					IL_09c4:
					if (CS_0024_003C_003E8__locals164.smeltProducts != null && !CS_0024_003C_003E8__locals164.smeltable)
					{
						_003C_003E2__current = "has smeltProducts but has smeltable=false";
						_003C_003E1__state = 28;
						return true;
					}
					goto IL_09f5;
					IL_12ab:
					if (CS_0024_003C_003E8__locals164.tools != null)
					{
						Tool tool = CS_0024_003C_003E8__locals164.tools.SelectMany(delegate(Tool lhs)
						{
							_003C_003Ec__DisplayClass401_4 CS_0024_003C_003E8__locals91 = new _003C_003Ec__DisplayClass401_4
							{
								lhs = lhs
							};
							return CS_0024_003C_003E8__locals164.tools.Where((Tool rhs) => CS_0024_003C_003E8__locals91.lhs != rhs && CS_0024_003C_003E8__locals91.lhs.id == rhs.id);
						}).FirstOrDefault();
						if (tool != null)
						{
							_003C_003E2__current = "duplicate thingdef tool id " + tool.id;
							_003C_003E1__state = 50;
							return true;
						}
						goto IL_1305;
					}
					goto IL_13ae;
					IL_0f80:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current7 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current7;
						_003C_003E1__state = 44;
						return true;
					}
					_003C_003Em__Finally10();
					_003C_003E7__wrap1 = null;
					_003Ci_003E5__6++;
					goto IL_0fac;
					IL_0549:
					if (CS_0024_003C_003E8__locals164.costList != null)
					{
						_003C_003E7__wrap3 = CS_0024_003C_003E8__locals164.costList.GetEnumerator();
						_003C_003E1__state = -7;
						goto IL_05c4;
					}
					goto IL_05e3;
					IL_09f5:
					if (CS_0024_003C_003E8__locals164.smeltable && CS_0024_003C_003E8__locals164.smeltProducts.NullOrEmpty() && CS_0024_003C_003E8__locals164.CostList.NullOrEmpty() && !CS_0024_003C_003E8__locals164.IsStuff && !CS_0024_003C_003E8__locals164.MadeFromStuff && !CS_0024_003C_003E8__locals164.destroyOnDrop)
					{
						_003C_003E2__current = "is smeltable but does not give anything for smelting.";
						_003C_003E1__state = 29;
						return true;
					}
					goto IL_0a50;
					IL_05c4:
					while (_003C_003E7__wrap3.MoveNext())
					{
						ThingDefCountClass current8 = _003C_003E7__wrap3.Current;
						if (current8.count == 0)
						{
							_003C_003E2__current = "cost in " + current8.thingDef?.ToString() + " is zero.";
							_003C_003E1__state = 13;
							return true;
						}
					}
					_003C_003Em__Finally5();
					_003C_003E7__wrap3 = default(List<ThingDefCountClass>.Enumerator);
					goto IL_05e3;
					IL_0c59:
					if (CS_0024_003C_003E8__locals164.saveCompressible && CS_0024_003C_003E8__locals164.Claimable)
					{
						_003C_003E2__current = "claimable item is compressible; faction will be unset after load";
						_003C_003E1__state = 37;
						return true;
					}
					goto IL_0c8a;
					IL_05e3:
					thingCategoryDef = CS_0024_003C_003E8__locals164.thingCategories?.FirstOrDefault(delegate(ThingCategoryDef cat)
					{
						_003C_003Ec__DisplayClass401_1 CS_0024_003C_003E8__locals105 = new _003C_003Ec__DisplayClass401_1
						{
							cat = cat
						};
						return CS_0024_003C_003E8__locals164.thingCategories.Count((ThingCategoryDef c) => c == CS_0024_003C_003E8__locals105.cat) > 1;
					});
					if (thingCategoryDef != null)
					{
						_003C_003E2__current = "has duplicate thingCategory " + thingCategoryDef?.ToString() + ".";
						_003C_003E1__state = 14;
						return true;
					}
					goto IL_063d;
					IL_118e:
					if (CS_0024_003C_003E8__locals164.race.Animal && CS_0024_003C_003E8__locals164.GetStatValueAbstract(StatDefOf.Wildness) < 0f)
					{
						_003C_003E2__current = "is animal but wildness is not defined";
						_003C_003E1__state = 47;
						return true;
					}
					goto IL_11cf;
					IL_0c8a:
					if (CS_0024_003C_003E8__locals164.deepCommonality > 0f != CS_0024_003C_003E8__locals164.deepLumpSizeRange.TrueMax > 0)
					{
						_003C_003E2__current = "if deepCommonality or deepLumpSizeRange is set, the other also must be set";
						_003C_003E1__state = 38;
						return true;
					}
					goto IL_0cc8;
					IL_1150:
					num2 = _003C_003E8__1.i;
					_003C_003E8__1.i = num2 + 1;
					goto IL_116c;
					IL_0fc2:
					if (CS_0024_003C_003E8__locals164.race != null)
					{
						_003C_003E7__wrap1 = CS_0024_003C_003E8__locals164.race.ConfigErrors(CS_0024_003C_003E8__locals164).GetEnumerator();
						_003C_003E1__state = -13;
						goto IL_101a;
					}
					goto IL_11cf;
					IL_0a50:
					if (CS_0024_003C_003E8__locals164.equipmentType != 0 && CS_0024_003C_003E8__locals164.verbs.NullOrEmpty() && CS_0024_003C_003E8__locals164.tools.NullOrEmpty())
					{
						_003C_003E2__current = "is equipment but has no verbs or tools";
						_003C_003E1__state = 30;
						return true;
					}
					goto IL_0a93;
					IL_1305:
					_003C_003E7__wrap6 = CS_0024_003C_003E8__locals164.tools.GetEnumerator();
					_003C_003E1__state = -16;
					goto IL_138f;
					IL_0cc8:
					if (CS_0024_003C_003E8__locals164.deepCommonality > 0f && CS_0024_003C_003E8__locals164.deepCountPerPortion <= 0)
					{
						_003C_003E2__current = "deepCommonality > 0 but deepCountPerPortion is not set";
						_003C_003E1__state = 39;
						return true;
					}
					goto IL_0cff;
					IL_063d:
					if (CS_0024_003C_003E8__locals164.Fillage == FillCategory.Full && CS_0024_003C_003E8__locals164.category != ThingCategory.Building)
					{
						_003C_003E2__current = "gives full cover but is not a building.";
						_003C_003E1__state = 15;
						return true;
					}
					goto IL_0670;
					IL_101a:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current9 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current9;
						_003C_003E1__state = 45;
						return true;
					}
					_003C_003Em__Finally11();
					_003C_003E7__wrap1 = null;
					if (CS_0024_003C_003E8__locals164.race.body != null && CS_0024_003C_003E8__locals164.race != null && CS_0024_003C_003E8__locals164.tools != null)
					{
						_003C_003E8__1 = new _003C_003Ec__DisplayClass401_3();
						_003C_003E8__1._003C_003E4__this = CS_0024_003C_003E8__locals164;
						_003C_003E8__1.i = 0;
						goto IL_116c;
					}
					goto IL_118e;
					IL_0cff:
					if (CS_0024_003C_003E8__locals164.verbs != null)
					{
						_003Ci_003E5__6 = 0;
						goto IL_0dd6;
					}
					goto IL_0dec;
					IL_0670:
					if (CS_0024_003C_003E8__locals164.equipmentType != 0)
					{
						if (CS_0024_003C_003E8__locals164.techLevel == TechLevel.Undefined && !CS_0024_003C_003E8__locals164.destroyOnDrop)
						{
							_003C_003E2__current = "is equipment but has no tech level.";
							_003C_003E1__state = 16;
							return true;
						}
						goto IL_06a9;
					}
					goto IL_06f6;
					IL_0a93:
					if (CS_0024_003C_003E8__locals164.Minifiable && CS_0024_003C_003E8__locals164.thingCategories.NullOrEmpty())
					{
						_003C_003E2__current = "is minifiable but not in any thing category";
						_003C_003E1__state = 31;
						return true;
					}
					goto IL_0ac9;
					IL_138f:
					if (_003C_003E7__wrap6.MoveNext())
					{
						Tool current10 = _003C_003E7__wrap6.Current;
						_003C_003E7__wrap1 = current10.ConfigErrors().GetEnumerator();
						_003C_003E1__state = -17;
						goto IL_1375;
					}
					_003C_003Em__Finally14();
					_003C_003E7__wrap6 = default(List<Tool>.Enumerator);
					goto IL_13ae;
					IL_0dd6:
					if (_003Ci_003E5__6 < CS_0024_003C_003E8__locals164.verbs.Count)
					{
						_003C_003E7__wrap1 = CS_0024_003C_003E8__locals164.verbs[_003Ci_003E5__6].ConfigErrors(CS_0024_003C_003E8__locals164).GetEnumerator();
						_003C_003E1__state = -9;
						goto IL_0daa;
					}
					goto IL_0dec;
					IL_06a9:
					if (!CS_0024_003C_003E8__locals164.comps.Any((CompProperties c) => typeof(CompEquippable).IsAssignableFrom(c.compClass)))
					{
						_003C_003E2__current = "is equipment but has no CompEquippable";
						_003C_003E1__state = 17;
						return true;
					}
					goto IL_06f6;
					IL_11cf:
					if (CS_0024_003C_003E8__locals164.ingestible != null)
					{
						_003C_003E7__wrap1 = CS_0024_003C_003E8__locals164.ingestible.ConfigErrors().GetEnumerator();
						_003C_003E1__state = -14;
						goto IL_1223;
					}
					goto IL_123d;
					IL_0ac9:
					if (CS_0024_003C_003E8__locals164.category == ThingCategory.Building && !CS_0024_003C_003E8__locals164.Minifiable && !CS_0024_003C_003E8__locals164.thingCategories.NullOrEmpty())
					{
						_003C_003E2__current = "is not minifiable yet has thing categories (could be confusing in thing filters because it can't be moved/stored anyway)";
						_003C_003E1__state = 32;
						return true;
					}
					goto IL_0b08;
					IL_0daa:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current11 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = $"verb {_003Ci_003E5__6}: {current11}";
						_003C_003E1__state = 40;
						return true;
					}
					_003C_003Em__Finally7();
					_003C_003E7__wrap1 = null;
					_003Ci_003E5__6++;
					goto IL_0dd6;
					IL_06f6:
					if (CS_0024_003C_003E8__locals164.thingClass == typeof(Bullet) && CS_0024_003C_003E8__locals164.projectile.damageDef == null)
					{
						_003C_003E2__current = " is a bullet but has no damageDef.";
						_003C_003E1__state = 18;
						return true;
					}
					goto IL_073b;
					IL_07b4:
					if (CS_0024_003C_003E8__locals164.damageMultipliers != null)
					{
						_003C_003E7__wrap4 = CS_0024_003C_003E8__locals164.damageMultipliers.GetEnumerator();
						_003C_003E1__state = -8;
						while (_003C_003E7__wrap4.MoveNext())
						{
							_003C_003Ec__DisplayClass401_2 CS_0024_003C_003E8__locals147 = new _003C_003Ec__DisplayClass401_2
							{
								mult = _003C_003E7__wrap4.Current
							};
							if (CS_0024_003C_003E8__locals164.damageMultipliers.Count((DamageMultiplier m) => m.damageDef == CS_0024_003C_003E8__locals147.mult.damageDef) > 1)
							{
								_003C_003E2__current = "has multiple damage multipliers for damageDef " + CS_0024_003C_003E8__locals147.mult.damageDef;
								_003C_003E1__state = 21;
								return true;
							}
						}
						goto IL_085f;
					}
					goto IL_0871;
					IL_0b08:
					if (!CS_0024_003C_003E8__locals164.destroyOnDrop && !typeof(MinifiedThing).IsAssignableFrom(CS_0024_003C_003E8__locals164.thingClass) && (CS_0024_003C_003E8__locals164.EverHaulable || CS_0024_003C_003E8__locals164.Minifiable) && (CS_0024_003C_003E8__locals164.statBases.NullOrEmpty() || !CS_0024_003C_003E8__locals164.statBases.Any((StatModifier s) => s.stat == StatDefOf.Mass)))
					{
						_003C_003E2__current = "is haulable, but does not have an authored mass value";
						_003C_003E1__state = 33;
						return true;
					}
					goto IL_0b94;
					IL_073b:
					if (CS_0024_003C_003E8__locals164.destroyOnDrop && CS_0024_003C_003E8__locals164.tradeability != 0)
					{
						_003C_003E2__current = "destroyOnDrop but tradeability is " + CS_0024_003C_003E8__locals164.tradeability;
						_003C_003E1__state = 19;
						return true;
					}
					goto IL_0782;
					IL_1223:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current12 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current12;
						_003C_003E1__state = 48;
						return true;
					}
					_003C_003Em__Finally12();
					_003C_003E7__wrap1 = null;
					goto IL_123d;
					IL_0dec:
					if (CS_0024_003C_003E8__locals164.building != null)
					{
						_003C_003E7__wrap1 = CS_0024_003C_003E8__locals164.building.ConfigErrors(CS_0024_003C_003E8__locals164).GetEnumerator();
						_003C_003E1__state = -10;
						goto IL_0e44;
					}
					goto IL_0ea2;
					IL_0782:
					if (CS_0024_003C_003E8__locals164.stackLimit > 1 && !CS_0024_003C_003E8__locals164.drawGUIOverlay)
					{
						_003C_003E2__current = "has stackLimit > 1 but also has drawGUIOverlay = false.";
						_003C_003E1__state = 20;
						return true;
					}
					goto IL_07b4;
					IL_0e44:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current13 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current13;
						_003C_003E1__state = 41;
						return true;
					}
					_003C_003Em__Finally8();
					_003C_003E7__wrap1 = null;
					if ((CS_0024_003C_003E8__locals164.building.isAirtight || CS_0024_003C_003E8__locals164.building.isStuffableAirtight) && CS_0024_003C_003E8__locals164.Fillage != FillCategory.Full)
					{
						_003C_003E2__current = "is airtight but Fillage is not Full";
						_003C_003E1__state = 42;
						return true;
					}
					goto IL_0ea2;
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
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally4()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally5()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap3/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally6()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap4/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally7()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally8()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally9()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally10()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally11()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally12()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally13()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally14()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap6/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally15()
		{
			_003C_003E1__state = -16;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally16()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap7/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__401 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__401(0)
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

	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__407 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ThingDef _003C_003E4__this;

		private StatRequest req;

		public StatRequest _003C_003E3__req;

		private IEnumerable<RecipeDef> _003Crecipes_003E5__2;

		private IEnumerator<StatDrawEntry> _003C_003E7__wrap2;

		private VerbProperties _003Cverb_003E5__4;

		private StatCategoryDef _003CverbStatCategory_003E5__5;

		private StatCategoryDef _003CstatCat_003E5__6;

		private float _003CdmgBuildingsImpassable_003E5__7;

		private float _003CdmgBuildingsPassable_003E5__8;

		private StringBuilder _003CticksBetweenBurstShotsExplanation_003E5__9;

		private StringBuilder _003CstoppingPowerExplanation_003E5__10;

		private int _003Ci_003E5__11;

		private Dialog_InfoCard.Hyperlink[] _003Chyperlinks_003E5__12;

		private ThingDef _003Cturret_003E5__13;

		private StatRequest _003Crequest_003E5__14;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
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
		public _003CSpecialDisplayStats_003Ed__407(int _003C_003E1__state)
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
			case 27:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			case -5:
			case 28:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			case -6:
			case 29:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally4();
				}
				break;
			case -7:
			case 30:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally5();
				}
				break;
			case -8:
			case 31:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally6();
				}
				break;
			case -9:
			case 32:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally7();
				}
				break;
			case -10:
			case 36:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally8();
				}
				break;
			case -11:
			case 42:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally9();
				}
				break;
			}
			_003Crecipes_003E5__2 = null;
			_003C_003E7__wrap2 = null;
			_003Cverb_003E5__4 = null;
			_003CverbStatCategory_003E5__5 = null;
			_003CstatCat_003E5__6 = null;
			_003CticksBetweenBurstShotsExplanation_003E5__9 = null;
			_003CstoppingPowerExplanation_003E5__10 = null;
			_003Chyperlinks_003E5__12 = null;
			_003Cturret_003E5__13 = null;
			_003Crequest_003E5__14 = default(StatRequest);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				ThingDef CS_0024_003C_003E8__locals66 = _003C_003E4__this;
				RecipeDef recipeDef;
				float buildingDamageFactor;
				float num7;
				StringBuilder stringBuilder7;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap2 = ((BuildableDef)CS_0024_003C_003E8__locals66).SpecialDisplayStats(req).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0118;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0118;
				case 2:
					_003C_003E1__state = -1;
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Apparel, "Layer".Translate(), CS_0024_003C_003E8__locals66.apparel.GetLayersString(), "Stat_Thing_Apparel_Layer_Desc".Translate(), 2751);
					_003C_003E1__state = 3;
					return true;
				case 3:
					_003C_003E1__state = -1;
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Apparel, "Stat_Thing_Apparel_CountsAsClothingNudity_Name".Translate(), CS_0024_003C_003E8__locals66.apparel.countsAsClothingForNudity ? "Yes".Translate() : "No".Translate(), "Stat_Thing_Apparel_CountsAsClothingNudity_Desc".Translate(), 2753);
					_003C_003E1__state = 4;
					return true;
				case 4:
					_003C_003E1__state = -1;
					if (ModsConfig.BiotechActive)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Apparel, "Stat_Thing_Apparel_ValidLifestage".Translate(), CS_0024_003C_003E8__locals66.apparel.developmentalStageFilter.ToCommaList().CapitalizeFirst(), "Stat_Thing_Apparel_ValidLifestage_Desc".Translate(), 2748);
						_003C_003E1__state = 5;
						return true;
					}
					goto IL_02d1;
				case 5:
					_003C_003E1__state = -1;
					goto IL_02d1;
				case 6:
					_003C_003E1__state = -1;
					goto IL_0340;
				case 7:
					_003C_003E1__state = -1;
					goto IL_03ac;
				case 8:
					_003C_003E1__state = -1;
					goto IL_042f;
				case 9:
					_003C_003E1__state = -1;
					goto IL_04ae;
				case 10:
					_003C_003E1__state = -1;
					goto IL_052d;
				case 11:
					_003C_003E1__state = -1;
					goto IL_0675;
				case 12:
					_003C_003E1__state = -1;
					goto IL_07f6;
				case 13:
					_003C_003E1__state = -1;
					goto IL_0873;
				case 14:
					_003C_003E1__state = -1;
					goto IL_0ab0;
				case 15:
					_003C_003E1__state = -1;
					if (_003Cverb_003E5__4.defaultProjectile.projectile.damageDef.armorCategory != null)
					{
						StringBuilder stringBuilder2 = new StringBuilder();
						float armorPenetration = _003Cverb_003E5__4.defaultProjectile.projectile.GetArmorPenetration(req.Thing, stringBuilder2);
						TaggedString taggedString = "ArmorPenetrationExplanation".Translate();
						if (stringBuilder2.Length != 0)
						{
							taggedString += "\n\n" + stringBuilder2;
						}
						_003C_003E2__current = new StatDrawEntry(_003CstatCat_003E5__6, "ArmorPenetration".Translate(), armorPenetration.ToStringPercent(), taggedString, 5400);
						_003C_003E1__state = 16;
						return true;
					}
					goto IL_0c6a;
				case 16:
					_003C_003E1__state = -1;
					goto IL_0c6a;
				case 17:
					_003C_003E1__state = -1;
					goto IL_0d24;
				case 18:
					_003C_003E1__state = -1;
					goto IL_0d8a;
				case 19:
					_003C_003E1__state = -1;
					goto IL_0df0;
				case 20:
					_003C_003E1__state = -1;
					goto IL_0e7d;
				case 21:
					_003C_003E1__state = -1;
					_003C_003E2__current = new StatDrawEntry(_003CstatCat_003E5__6, "BurstShotFireRate".Translate(), _003CdmgBuildingsImpassable_003E5__7.ToString("0.##") + " rpm", _003CticksBetweenBurstShotsExplanation_003E5__9.ToString(), 5395);
					_003C_003E1__state = 22;
					return true;
				case 22:
					_003C_003E1__state = -1;
					goto IL_1454;
				case 23:
					_003C_003E1__state = -1;
					goto IL_14bb;
				case 24:
					_003C_003E1__state = -1;
					_003CticksBetweenBurstShotsExplanation_003E5__9 = null;
					_003CstoppingPowerExplanation_003E5__10 = null;
					_003CstatCat_003E5__6 = null;
					goto IL_1736;
				case 25:
					_003C_003E1__state = -1;
					_003C_003E2__current = new StatDrawEntry(_003CstatCat_003E5__6, "DirectHitChance".Translate(), (1f / (float)GenRadial.NumCellsInRadius(_003Cverb_003E5__4.ForcedMissRadius)).ToStringPercent(), "Stat_Thing_Weapon_DirectHitChance_Desc".Translate(), 3560);
					_003C_003E1__state = 26;
					return true;
				case 26:
					_003C_003E1__state = -1;
					_003CstatCat_003E5__6 = null;
					goto IL_1838;
				case 27:
					_003C_003E1__state = -4;
					goto IL_189a;
				case 28:
					_003C_003E1__state = -5;
					goto IL_1908;
				case 29:
					_003C_003E1__state = -6;
					goto IL_197d;
				case 30:
					_003C_003E1__state = -7;
					goto IL_19f2;
				case 31:
					_003C_003E1__state = -8;
					goto IL_1a74;
				case 32:
					_003C_003E1__state = -9;
					goto IL_1af7;
				case 33:
				{
					_003C_003E1__state = -1;
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("Stat_MiningYield_Desc".Translate());
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("StatsReport_DifficultyMultiplier".Translate(Find.Storyteller.difficultyDef.label) + ": " + Find.Storyteller.difficulty.mineYieldFactor.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor));
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "Stat_MiningYield_Name".Translate(), Mathf.CeilToInt(CS_0024_003C_003E8__locals66.building.EffectiveMineableYield).ToString("F0"), stringBuilder.ToString(), 2200, null, _003Chyperlinks_003E5__12);
					_003C_003E1__state = 34;
					return true;
				}
				case 34:
					_003C_003E1__state = -1;
					_003Chyperlinks_003E5__12 = null;
					goto IL_1cc5;
				case 35:
					_003C_003E1__state = -1;
					_003Crequest_003E5__14 = StatRequest.For(_003Cturret_003E5__13, null);
					_003C_003E7__wrap2 = _003Cturret_003E5__13.SpecialDisplayStats(_003Crequest_003E5__14).GetEnumerator();
					_003C_003E1__state = -10;
					goto IL_1dce;
				case 36:
					_003C_003E1__state = -10;
					goto IL_1dce;
				case 37:
					_003C_003E1__state = -1;
					goto IL_1e65;
				case 38:
					_003C_003E1__state = -1;
					goto IL_1f46;
				case 39:
					_003C_003E1__state = -1;
					goto IL_20ba;
				case 40:
					_003C_003E1__state = -1;
					goto IL_222e;
				case 41:
					_003C_003E1__state = -1;
					_003Ci_003E5__11++;
					goto IL_2412;
				case 42:
					{
						_003C_003E1__state = -11;
						goto IL_2474;
					}
					IL_1dce:
					while (_003C_003E7__wrap2.MoveNext())
					{
						StatDrawEntry current = _003C_003E7__wrap2.Current;
						if (current.category == StatCategoryDefOf.Weapon_Ranged)
						{
							_003C_003E2__current = current;
							_003C_003E1__state = 36;
							return true;
						}
					}
					_003C_003Em__Finally8();
					_003C_003E7__wrap2 = null;
					_003Ci_003E5__11 = 0;
					goto IL_1e77;
					IL_1e77:
					if (_003Ci_003E5__11 < _003Cturret_003E5__13.statBases.Count)
					{
						StatModifier statModifier = _003Cturret_003E5__13.statBases[_003Ci_003E5__11];
						if (statModifier.stat.category == StatCategoryDefOf.Weapon_Ranged)
						{
							_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Weapon_Ranged, statModifier.stat, statModifier.value, _003Crequest_003E5__14, ToStringNumberSense.Undefined, null);
							_003C_003E1__state = 37;
							return true;
						}
						goto IL_1e65;
					}
					_003Cturret_003E5__13 = null;
					_003Crequest_003E5__14 = default(StatRequest);
					goto IL_1ea5;
					IL_1cc5:
					if (CS_0024_003C_003E8__locals66.building.IsTurret)
					{
						_003Cturret_003E5__13 = CS_0024_003C_003E8__locals66.building.turretGunDef;
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Stat_Weapon_Name".Translate(), _003Cturret_003E5__13.LabelCap, "Stat_Weapon_Desc".Translate(), 5389, null, new Dialog_InfoCard.Hyperlink[1]
						{
							new Dialog_InfoCard.Hyperlink(_003Cturret_003E5__13)
						});
						_003C_003E1__state = 35;
						return true;
					}
					goto IL_1ea5;
					IL_20ba:
					if (CS_0024_003C_003E8__locals66.IsLeather)
					{
						List<ThingDef> list = new List<ThingDef>();
						bool flag = false;
						foreach (ThingDef allDef in DefDatabase<ThingDef>.AllDefs)
						{
							if (allDef.race != null && allDef.race.leatherDef == CS_0024_003C_003E8__locals66 && !allDef.IsCorpse)
							{
								if (!Find.HiddenItemsManager.Hidden(allDef))
								{
									flag = true;
								}
								list.Add(allDef);
							}
						}
						_003C_003E2__current = new StatDrawEntry(valueString: (!flag) ? $"({"NotYetDiscovered".Translate()})" : string.Join(", ", (from x in list
							where !Find.HiddenItemsManager.Hidden(x)
							select x into p
							select p.label).ToArray()).CapitalizeFirst(), category: StatCategoryDefOf.BasicsPawn, label: "Stat_SourceSpecies_Name".Translate(), reportText: "Stat_SourceSpecies_Desc".Translate(), displayPriorityWithinCategory: 1200, overrideReportTitle: null, hyperlinks: Dialog_InfoCard.DefsToHyperlinks(list));
						_003C_003E1__state = 40;
						return true;
					}
					goto IL_222e;
					IL_0df7:
					if (_003Cverb_003E5__4.defaultProjectile == null && _003Cverb_003E5__4.beamDamageDef != null)
					{
						_003C_003E2__current = new StatDrawEntry(_003CverbStatCategory_003E5__5 ?? StatCategoryDefOf.Weapon_Ranged, "ArmorPenetration".Translate(), _003Cverb_003E5__4.beamDamageDef.defaultArmorPenetration.ToStringPercent(), "ArmorPenetrationExplanation".Translate(), 5400);
						_003C_003E1__state = 20;
						return true;
					}
					goto IL_0e7d;
					IL_1e65:
					_003Ci_003E5__11++;
					goto IL_1e77;
					IL_1838:
					_003Cverb_003E5__4 = null;
					_003CverbStatCategory_003E5__5 = null;
					goto IL_1846;
					IL_0675:
					recipeDef = _003Crecipes_003E5__2.FirstOrDefault();
					if (recipeDef != null && !recipeDef.ingredients.NullOrEmpty())
					{
						BuildableDef.tmpCostList.Clear();
						BuildableDef.tmpHyperlinks.Clear();
						for (int i = 0; i < recipeDef.ingredients.Count; i++)
						{
							IngredientCount ingredientCount = recipeDef.ingredients[i];
							if (ingredientCount.filter.Summary.NullOrEmpty())
							{
								continue;
							}
							IEnumerable<ThingDef> allowedThingDefs = ingredientCount.filter.AllowedThingDefs;
							if (allowedThingDefs.Any())
							{
								using IEnumerator<ThingDef> enumerator = allowedThingDefs.GetEnumerator();
								while (enumerator.MoveNext())
								{
									_003C_003Ec__DisplayClass407_0 CS_0024_003C_003E8__locals8 = new _003C_003Ec__DisplayClass407_0
									{
										p = enumerator.Current
									};
									if (!BuildableDef.tmpHyperlinks.Any((Dialog_InfoCard.Hyperlink x) => x.def == CS_0024_003C_003E8__locals8.p))
									{
										BuildableDef.tmpHyperlinks.Add(new Dialog_InfoCard.Hyperlink(CS_0024_003C_003E8__locals8.p));
									}
								}
							}
							BuildableDef.tmpCostList.Add(recipeDef.IngredientValueGetter.BillRequirementsDescription(recipeDef, ingredientCount));
						}
					}
					if (BuildableDef.tmpCostList.Any())
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "Ingredients".Translate(), BuildableDef.tmpCostList.ToCommaList(), "Stat_Thing_Ingredients".Translate(), 1102, null, BuildableDef.tmpHyperlinks);
						_003C_003E1__state = 12;
						return true;
					}
					goto IL_07f6;
					IL_2412:
					if (_003Ci_003E5__11 < CS_0024_003C_003E8__locals66.equippedStatOffsets.Count)
					{
						StatDef stat = CS_0024_003C_003E8__locals66.equippedStatOffsets[_003Ci_003E5__11].stat;
						float num2 = CS_0024_003C_003E8__locals66.equippedStatOffsets[_003Ci_003E5__11].value;
						StringBuilder stringBuilder3 = new StringBuilder(stat.description);
						if (req.HasThing && stat.Worker != null)
						{
							stringBuilder3.AppendLine();
							stringBuilder3.AppendLine();
							stringBuilder3.AppendLine("StatsReport_BaseValue".Translate() + ": " + stat.ValueToString(num2, ToStringNumberSense.Offset, stat.finalizeEquippedStatOffset));
							num2 = StatWorker.StatOffsetFromGear(req.Thing, stat);
							if (!stat.parts.NullOrEmpty())
							{
								stringBuilder3.AppendLine();
								for (int j = 0; j < stat.parts.Count; j++)
								{
									string text = stat.parts[j].ExplanationPart(req);
									if (!text.NullOrEmpty())
									{
										stringBuilder3.AppendLine(text);
									}
								}
							}
							stringBuilder3.AppendLine();
							stringBuilder3.AppendLine("StatsReport_FinalValue".Translate() + ": " + stat.ValueToString(num2, ToStringNumberSense.Offset, !stat.formatString.NullOrEmpty()));
						}
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.EquippedStatOffsets, CS_0024_003C_003E8__locals66.equippedStatOffsets[_003Ci_003E5__11].stat, num2, StatRequest.ForEmpty(), ToStringNumberSense.Offset, null, forceUnfinalizedMode: true).SetReportText(stringBuilder3.ToString());
						_003C_003E1__state = 41;
						return true;
					}
					goto IL_2428;
					IL_0e7d:
					if (_003Cverb_003E5__4.Ranged)
					{
						float num3 = _003Cverb_003E5__4.burstShotCount;
						float num4 = _003Cverb_003E5__4.ticksBetweenBurstShots;
						_003CdmgBuildingsPassable_003E5__8 = (_003Cverb_003E5__4?.defaultProjectile?.projectile?.stoppingPower).GetValueOrDefault();
						StringBuilder stringBuilder4 = new StringBuilder("Stat_Thing_Weapon_BurstShotCount_Desc".Translate());
						stringBuilder4.AppendLine();
						stringBuilder4.AppendLine();
						stringBuilder4.AppendLine("StatsReport_BaseValue".Translate() + ": " + _003Cverb_003E5__4.burstShotCount.ToString());
						stringBuilder4.AppendLine();
						_003CticksBetweenBurstShotsExplanation_003E5__9 = new StringBuilder("Stat_Thing_Weapon_BurstShotFireRate_Desc".Translate());
						_003CticksBetweenBurstShotsExplanation_003E5__9.AppendLine();
						_003CticksBetweenBurstShotsExplanation_003E5__9.AppendLine();
						_003CticksBetweenBurstShotsExplanation_003E5__9.AppendLine("StatsReport_BaseValue".Translate() + ": " + (60f / _003Cverb_003E5__4.ticksBetweenBurstShots.TicksToSeconds()).ToString("0.##") + " rpm");
						_003CticksBetweenBurstShotsExplanation_003E5__9.AppendLine();
						_003CstoppingPowerExplanation_003E5__10 = new StringBuilder("StoppingPowerExplanation".Translate());
						_003CstoppingPowerExplanation_003E5__10.AppendLine();
						_003CstoppingPowerExplanation_003E5__10.AppendLine();
						_003CstoppingPowerExplanation_003E5__10.AppendLine("StatsReport_BaseValue".Translate() + ": " + _003CdmgBuildingsPassable_003E5__8.ToString("F1"));
						_003CstoppingPowerExplanation_003E5__10.AppendLine();
						if (req.HasThing && req.Thing.TryGetComp(out CompUniqueWeapon comp))
						{
							bool flag2 = false;
							bool flag3 = false;
							bool flag4 = false;
							foreach (WeaponTraitDef item in comp.TraitsListForReading)
							{
								if (!Mathf.Approximately(item.burstShotCountMultiplier, 1f))
								{
									if (!flag2)
									{
										stringBuilder4.AppendLine("StatsReport_WeaponTraits".Translate() + ":");
										flag2 = true;
									}
									num3 *= item.burstShotCountMultiplier;
									stringBuilder4.AppendLine("    " + item.LabelCap + ": " + item.burstShotCountMultiplier.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor));
								}
								if (!Mathf.Approximately(item.burstShotSpeedMultiplier, 1f))
								{
									if (!flag3)
									{
										_003CticksBetweenBurstShotsExplanation_003E5__9.AppendLine("StatsReport_WeaponTraits".Translate() + ":");
										flag3 = true;
									}
									num4 /= item.burstShotSpeedMultiplier;
									_003CticksBetweenBurstShotsExplanation_003E5__9.AppendLine("    " + item.LabelCap + ": " + item.burstShotSpeedMultiplier.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor));
								}
								if (!Mathf.Approximately(item.additionalStoppingPower, 0f))
								{
									if (!flag4)
									{
										_003CstoppingPowerExplanation_003E5__10.AppendLine("StatsReport_WeaponTraits".Translate() + ":");
										flag4 = true;
									}
									_003CdmgBuildingsPassable_003E5__8 += item.additionalStoppingPower;
									_003CstoppingPowerExplanation_003E5__10.AppendLine("    " + item.LabelCap + ": " + item.additionalStoppingPower.ToStringByStyle(ToStringStyle.FloatOne, ToStringNumberSense.Offset));
								}
							}
						}
						stringBuilder4.AppendLine();
						stringBuilder4.AppendLine("StatsReport_FinalValue".Translate() + ": " + Mathf.CeilToInt(num3).ToString());
						_003CdmgBuildingsImpassable_003E5__7 = 60f / ((int)num4).TicksToSeconds();
						_003CticksBetweenBurstShotsExplanation_003E5__9.AppendLine();
						_003CticksBetweenBurstShotsExplanation_003E5__9.AppendLine("StatsReport_FinalValue".Translate() + ": " + _003CdmgBuildingsImpassable_003E5__7.ToString("0.##") + " rpm");
						_003CstoppingPowerExplanation_003E5__10.AppendLine();
						_003CstoppingPowerExplanation_003E5__10.AppendLine("StatsReport_FinalValue".Translate() + ": " + _003CdmgBuildingsPassable_003E5__8.ToString("F1"));
						_003CstatCat_003E5__6 = _003CverbStatCategory_003E5__5 ?? StatCategoryDefOf.Weapon_Ranged;
						if (_003Cverb_003E5__4.showBurstShotStats && _003Cverb_003E5__4.burstShotCount > 1)
						{
							_003C_003E2__current = new StatDrawEntry(_003CstatCat_003E5__6, "BurstShotCount".Translate(), Mathf.CeilToInt(num3).ToString(), stringBuilder4.ToString(), 5391);
							_003C_003E1__state = 21;
							return true;
						}
						goto IL_1454;
					}
					goto IL_1736;
					IL_19f2:
					if (_003C_003E7__wrap2.MoveNext())
					{
						StatDrawEntry current4 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current4;
						_003C_003E1__state = 30;
						return true;
					}
					_003C_003Em__Finally5();
					_003C_003E7__wrap2 = null;
					goto IL_1a0c;
					IL_1997:
					if (CS_0024_003C_003E8__locals66.building != null)
					{
						_003C_003E7__wrap2 = CS_0024_003C_003E8__locals66.building.SpecialDisplayStats(CS_0024_003C_003E8__locals66, req).GetEnumerator();
						_003C_003E1__state = -7;
						goto IL_19f2;
					}
					goto IL_1a0c;
					IL_0c6a:
					buildingDamageFactor = _003Cverb_003E5__4.defaultProjectile.projectile.damageDef.buildingDamageFactor;
					_003CdmgBuildingsImpassable_003E5__7 = _003Cverb_003E5__4.defaultProjectile.projectile.damageDef.buildingDamageFactorImpassable;
					_003CdmgBuildingsPassable_003E5__8 = _003Cverb_003E5__4.defaultProjectile.projectile.damageDef.buildingDamageFactorPassable;
					if (buildingDamageFactor != 1f)
					{
						_003C_003E2__current = new StatDrawEntry(_003CstatCat_003E5__6, "BuildingDamageFactor".Translate(), buildingDamageFactor.ToStringPercent(), "BuildingDamageFactorExplanation".Translate(), 5410);
						_003C_003E1__state = 17;
						return true;
					}
					goto IL_0d24;
					IL_1ea5:
					if (ModsConfig.OdysseyActive && CS_0024_003C_003E8__locals66.Fillage == FillCategory.Full)
					{
						bool b = CS_0024_003C_003E8__locals66.building.isAirtight || (CS_0024_003C_003E8__locals66.building.isStuffableAirtight && req.StuffDef.stuffProps.isAirtight);
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Building, "Stat_Airtight".Translate(), b.ToStringYesNo(), "Stat_Airtight_Desc".Translate(), 6100);
						_003C_003E1__state = 38;
						return true;
					}
					goto IL_1f46;
					IL_0d24:
					if (_003CdmgBuildingsImpassable_003E5__7 != 1f)
					{
						_003C_003E2__current = new StatDrawEntry(_003CstatCat_003E5__6, "BuildingDamageFactorImpassable".Translate(), _003CdmgBuildingsImpassable_003E5__7.ToStringPercent(), "BuildingDamageFactorImpassableExplanation".Translate(), 5420);
						_003C_003E1__state = 18;
						return true;
					}
					goto IL_0d8a;
					IL_1af7:
					if (_003C_003E7__wrap2.MoveNext())
					{
						StatDrawEntry current5 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current5;
						_003C_003E1__state = 32;
						return true;
					}
					_003C_003Em__Finally7();
					_003C_003E7__wrap2 = null;
					_003Ci_003E5__11++;
					goto IL_1b23;
					IL_0d8a:
					if (_003CdmgBuildingsPassable_003E5__8 != 1f)
					{
						_003C_003E2__current = new StatDrawEntry(_003CstatCat_003E5__6, "BuildingDamageFactorPassable".Translate(), _003CdmgBuildingsPassable_003E5__8.ToStringPercent(), "BuildingDamageFactorPassableExplanation".Translate(), 5430);
						_003C_003E1__state = 19;
						return true;
					}
					goto IL_0df0;
					IL_07f6:
					if (CS_0024_003C_003E8__locals66.thingClass != null && typeof(Building_Bed).IsAssignableFrom(CS_0024_003C_003E8__locals66.thingClass) && !CS_0024_003C_003E8__locals66.statBases.StatListContains(StatDefOf.BedRestEffectiveness))
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Building, StatDefOf.BedRestEffectiveness, StatDefOf.BedRestEffectiveness.valueIfMissing, StatRequest.ForEmpty(), ToStringNumberSense.Undefined, null);
						_003C_003E1__state = 13;
						return true;
					}
					goto IL_0873;
					IL_0df0:
					_003CstatCat_003E5__6 = null;
					goto IL_0df7;
					IL_1736:
					if (_003Cverb_003E5__4.ForcedMissRadius > 0f)
					{
						_003CstatCat_003E5__6 = _003CverbStatCategory_003E5__5 ?? StatCategoryDefOf.Weapon_Ranged;
						_003C_003E2__current = new StatDrawEntry(_003CstatCat_003E5__6, "MissRadius".Translate(), _003Cverb_003E5__4.ForcedMissRadius.ToString("0.#"), "Stat_Thing_Weapon_MissRadius_Desc".Translate(), 3557);
						_003C_003E1__state = 25;
						return true;
					}
					goto IL_1838;
					IL_02d1:
					if (CS_0024_003C_003E8__locals66.apparel.gender != 0)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Apparel, "Stat_Thing_Apparel_Gender".Translate(), CS_0024_003C_003E8__locals66.apparel.gender.GetLabel().CapitalizeFirst(), "Stat_Thing_Apparel_Gender_Desc".Translate(), 2749);
						_003C_003E1__state = 6;
						return true;
					}
					goto IL_0340;
					IL_1a0c:
					if (CS_0024_003C_003E8__locals66.isTechHediff)
					{
						IEnumerable<RecipeDef> recipes = DefDatabase<RecipeDef>.AllDefs.Where((RecipeDef x) => x.addsHediff != null && x.IsIngredient(CS_0024_003C_003E8__locals66));
						_003C_003E7__wrap2 = MedicalRecipesUtility.GetMedicalStatsFromRecipeDefs(recipes).GetEnumerator();
						_003C_003E1__state = -8;
						goto IL_1a74;
					}
					goto IL_1a8e;
					IL_0873:
					if (!CS_0024_003C_003E8__locals66.verbs.NullOrEmpty())
					{
						_003Cverb_003E5__4 = CS_0024_003C_003E8__locals66.verbs.First((VerbProperties x) => x.isPrimary);
						_003CverbStatCategory_003E5__5 = ((CS_0024_003C_003E8__locals66.category == ThingCategory.Pawn) ? StatCategoryDefOf.PawnCombat : null);
						float num5 = _003Cverb_003E5__4.warmupTime;
						StringBuilder stringBuilder5 = new StringBuilder("Stat_Thing_Weapon_RangedWarmupTime_Desc".Translate());
						stringBuilder5.AppendLine();
						stringBuilder5.AppendLine();
						stringBuilder5.AppendLine("StatsReport_BaseValue".Translate() + ": " + num5.ToString("0.##") + " " + "LetterSecond".Translate());
						if (num5 > 0f)
						{
							if (req.HasThing)
							{
								float statValue = req.Thing.GetStatValue(StatDefOf.RangedWeapon_WarmupMultiplier);
								num5 *= statValue;
								if (!Mathf.Approximately(statValue, 1f))
								{
									stringBuilder5.AppendLine();
									stringBuilder5.AppendLine("Stat_Thing_Weapon_WarmupTime_Multiplier".Translate() + ": x" + statValue.ToStringPercent());
									stringBuilder5.Append(StatUtility.GetOffsetsAndFactorsFor(StatDefOf.RangedWeapon_WarmupMultiplier, req.Thing));
								}
							}
							stringBuilder5.AppendLine();
							stringBuilder5.AppendLine("StatsReport_FinalValue".Translate() + ": " + num5.ToString("0.##") + " " + "LetterSecond".Translate());
							_003C_003E2__current = new StatDrawEntry(_003CverbStatCategory_003E5__5 ?? StatCategoryDefOf.Weapon_Ranged, "RangedWarmupTime".Translate(), num5.ToString("0.##") + " " + "LetterSecond".Translate(), stringBuilder5.ToString(), 3555);
							_003C_003E1__state = 14;
							return true;
						}
						goto IL_0ab0;
					}
					goto IL_1846;
					IL_2428:
					if (!CS_0024_003C_003E8__locals66.IsDrug)
					{
						break;
					}
					_003C_003E7__wrap2 = DrugStatsUtility.SpecialDisplayStats(CS_0024_003C_003E8__locals66).GetEnumerator();
					_003C_003E1__state = -11;
					goto IL_2474;
					IL_1a74:
					if (_003C_003E7__wrap2.MoveNext())
					{
						StatDrawEntry current6 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current6;
						_003C_003E1__state = 31;
						return true;
					}
					_003C_003Em__Finally6();
					_003C_003E7__wrap2 = null;
					goto IL_1a8e;
					IL_0118:
					if (_003C_003E7__wrap2.MoveNext())
					{
						StatDrawEntry current7 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current7;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap2 = null;
					if (CS_0024_003C_003E8__locals66.apparel != null)
					{
						string coveredOuterPartsString = CS_0024_003C_003E8__locals66.apparel.GetCoveredOuterPartsString(BodyDefOf.Human);
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Apparel, "Covers".Translate(), coveredOuterPartsString, "Stat_Thing_Apparel_Covers_Desc".Translate(), 2750);
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_0340;
					IL_1846:
					if (CS_0024_003C_003E8__locals66.plant != null)
					{
						_003C_003E7__wrap2 = CS_0024_003C_003E8__locals66.plant.SpecialDisplayStats().GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_189a;
					}
					goto IL_18b4;
					IL_189a:
					if (_003C_003E7__wrap2.MoveNext())
					{
						StatDrawEntry current8 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current8;
						_003C_003E1__state = 27;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = null;
					goto IL_18b4;
					IL_0340:
					if (CS_0024_003C_003E8__locals66.IsMedicine && CS_0024_003C_003E8__locals66.MedicineTendXpGainFactor != 1f)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "MedicineXpGainFactor".Translate(), CS_0024_003C_003E8__locals66.MedicineTendXpGainFactor.ToStringPercent(), "Stat_Thing_Drug_MedicineXpGainFactor_Desc".Translate(), 1000);
						_003C_003E1__state = 7;
						return true;
					}
					goto IL_03ac;
					IL_222e:
					if (!CS_0024_003C_003E8__locals66.equippedStatOffsets.NullOrEmpty())
					{
						_003Ci_003E5__11 = 0;
						goto IL_2412;
					}
					goto IL_2428;
					IL_1f46:
					if (CS_0024_003C_003E8__locals66.IsMeat)
					{
						List<ThingDef> list2 = new List<ThingDef>();
						bool flag5 = false;
						foreach (ThingDef allDef2 in DefDatabase<ThingDef>.AllDefs)
						{
							if (allDef2.race != null && allDef2.race.meatDef == CS_0024_003C_003E8__locals66 && !allDef2.IsCorpse)
							{
								if (!Find.HiddenItemsManager.Hidden(allDef2))
								{
									flag5 = true;
								}
								list2.Add(allDef2);
							}
						}
						_003C_003E2__current = new StatDrawEntry(valueString: (!flag5) ? $"({"NotYetDiscovered".Translate()})" : string.Join(", ", (from x in list2
							where !Find.HiddenItemsManager.Hidden(x)
							select x into p
							select p.label).ToArray()).CapitalizeFirst(), category: StatCategoryDefOf.BasicsPawn, label: "Stat_SourceSpecies_Name".Translate(), reportText: "Stat_SourceSpecies_Desc".Translate(), displayPriorityWithinCategory: 1200, overrideReportTitle: null, hyperlinks: Dialog_InfoCard.DefsToHyperlinks(list2));
						_003C_003E1__state = 39;
						return true;
					}
					goto IL_20ba;
					IL_03ac:
					if (CS_0024_003C_003E8__locals66.fillPercent > 0f && (CS_0024_003C_003E8__locals66.category == ThingCategory.Item || CS_0024_003C_003E8__locals66.category == ThingCategory.Building || CS_0024_003C_003E8__locals66.category == ThingCategory.Plant))
					{
						StatDrawEntry statDrawEntry = new StatDrawEntry(StatCategoryDefOf.Basics, "CoverEffectiveness".Translate(), CS_0024_003C_003E8__locals66.BaseBlockChance().ToStringPercent(), "CoverEffectivenessExplanation".Translate(), 2000);
						_003C_003E2__current = statDrawEntry;
						_003C_003E1__state = 8;
						return true;
					}
					goto IL_042f;
					IL_2474:
					if (_003C_003E7__wrap2.MoveNext())
					{
						StatDrawEntry current10 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current10;
						_003C_003E1__state = 42;
						return true;
					}
					_003C_003Em__Finally9();
					_003C_003E7__wrap2 = null;
					break;
					IL_1b23:
					if (_003Ci_003E5__11 < CS_0024_003C_003E8__locals66.comps.Count)
					{
						_003C_003E7__wrap2 = CS_0024_003C_003E8__locals66.comps[_003Ci_003E5__11].SpecialDisplayStats(req).GetEnumerator();
						_003C_003E1__state = -9;
						goto IL_1af7;
					}
					if (CS_0024_003C_003E8__locals66.building != null)
					{
						if (CS_0024_003C_003E8__locals66.building.mineableThing != null)
						{
							_003Chyperlinks_003E5__12 = new Dialog_InfoCard.Hyperlink[1]
							{
								new Dialog_InfoCard.Hyperlink(CS_0024_003C_003E8__locals66.building.mineableThing)
							};
							_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Stat_MineableThing_Name".Translate(), CS_0024_003C_003E8__locals66.building.mineableThing.LabelCap, "Stat_MineableThing_Desc".Translate(), 2200, null, _003Chyperlinks_003E5__12);
							_003C_003E1__state = 33;
							return true;
						}
						goto IL_1cc5;
					}
					goto IL_1f46;
					IL_18b4:
					if (CS_0024_003C_003E8__locals66.ingestible != null)
					{
						_003C_003E7__wrap2 = CS_0024_003C_003E8__locals66.ingestible.SpecialDisplayStats().GetEnumerator();
						_003C_003E1__state = -5;
						goto IL_1908;
					}
					goto IL_1922;
					IL_1a8e:
					_003Ci_003E5__11 = 0;
					goto IL_1b23;
					IL_042f:
					if (CS_0024_003C_003E8__locals66.constructionSkillPrerequisite > 0)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "SkillRequiredToBuild".Translate(SkillDefOf.Construction.LabelCap), CS_0024_003C_003E8__locals66.constructionSkillPrerequisite.ToString(), "SkillRequiredToBuildExplanation".Translate(SkillDefOf.Construction.LabelCap), 1100);
						_003C_003E1__state = 9;
						return true;
					}
					goto IL_04ae;
					IL_1454:
					if (_003CdmgBuildingsPassable_003E5__8 > 0f)
					{
						_003C_003E2__current = new StatDrawEntry(_003CstatCat_003E5__6, "StoppingPower".Translate(), _003CdmgBuildingsPassable_003E5__8.ToString("F1"), _003CstoppingPowerExplanation_003E5__10.ToString(), 5402);
						_003C_003E1__state = 23;
						return true;
					}
					goto IL_14bb;
					IL_04ae:
					if (CS_0024_003C_003E8__locals66.artisticSkillPrerequisite > 0)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "SkillRequiredToBuild".Translate(SkillDefOf.Artistic.LabelCap), CS_0024_003C_003E8__locals66.artisticSkillPrerequisite.ToString(), "SkillRequiredToBuildExplanation".Translate(SkillDefOf.Artistic.LabelCap), 1100);
						_003C_003E1__state = 10;
						return true;
					}
					goto IL_052d;
					IL_1908:
					if (_003C_003E7__wrap2.MoveNext())
					{
						StatDrawEntry current11 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current11;
						_003C_003E1__state = 28;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap2 = null;
					goto IL_1922;
					IL_052d:
					_003Crecipes_003E5__2 = DefDatabase<RecipeDef>.AllDefsListForReading.Where((RecipeDef r) => r.products.Count == 1 && r.products.Any((ThingDefCountClass p) => p.thingDef == CS_0024_003C_003E8__locals66) && !r.IsSurgery);
					if (_003Crecipes_003E5__2.Any())
					{
						IEnumerable<string> enumerable = (from u in _003Crecipes_003E5__2.Where((RecipeDef x) => x.recipeUsers != null).SelectMany((RecipeDef r) => r.recipeUsers)
							select u.label).Concat(from x in DefDatabase<ThingDef>.AllDefsListForReading
							where x.recipes != null && x.recipes.Any((RecipeDef y) => y.products.Any((ThingDefCountClass z) => z.thingDef == CS_0024_003C_003E8__locals66))
							select x.label).Distinct();
						if (enumerable.Any())
						{
							_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "CreatedAt".Translate(), enumerable.ToCommaList().CapitalizeFirst(), "Stat_Thing_CreatedAt_Desc".Translate(), 1103);
							_003C_003E1__state = 11;
							return true;
						}
						goto IL_0675;
					}
					goto IL_07f6;
					IL_0ab0:
					if (_003Cverb_003E5__4.defaultProjectile?.projectile.damageDef != null && _003Cverb_003E5__4.defaultProjectile.projectile.damageDef.harmsHealth)
					{
						_003CstatCat_003E5__6 = _003CverbStatCategory_003E5__5 ?? StatCategoryDefOf.Weapon_Ranged;
						StringBuilder stringBuilder6 = new StringBuilder();
						stringBuilder6.AppendLine("Stat_Thing_Damage_Desc".Translate());
						stringBuilder6.AppendLine();
						float num6 = _003Cverb_003E5__4.defaultProjectile.projectile.GetDamageAmount(req.Thing, stringBuilder6);
						_003C_003E2__current = new StatDrawEntry(_003CstatCat_003E5__6, "Damage".Translate(), num6.ToString(), stringBuilder6.ToString(), 5500);
						_003C_003E1__state = 15;
						return true;
					}
					goto IL_0df7;
					IL_1922:
					if (CS_0024_003C_003E8__locals66.race != null)
					{
						_003C_003E7__wrap2 = CS_0024_003C_003E8__locals66.race.SpecialDisplayStats(CS_0024_003C_003E8__locals66, req).GetEnumerator();
						_003C_003E1__state = -6;
						goto IL_197d;
					}
					goto IL_1997;
					IL_14bb:
					num7 = _003Cverb_003E5__4.range;
					stringBuilder7 = new StringBuilder("Stat_Thing_Weapon_Range_Desc".Translate());
					stringBuilder7.AppendLine();
					stringBuilder7.AppendLine();
					stringBuilder7.AppendLine("StatsReport_BaseValue".Translate() + ": " + num7.ToString("F0"));
					if (req.HasThing)
					{
						float statValue2 = req.Thing.GetStatValue(StatDefOf.RangedWeapon_RangeMultiplier);
						num7 *= statValue2;
						if (!Mathf.Approximately(statValue2, 1f))
						{
							stringBuilder7.AppendLine();
							stringBuilder7.AppendLine("Stat_Thing_Weapon_Range_Multiplier".Translate() + ": x" + statValue2.ToStringPercent());
							stringBuilder7.Append(StatUtility.GetOffsetsAndFactorsFor(StatDefOf.RangedWeapon_RangeMultiplier, req.Thing));
						}
						Map obj = req.Thing.Map ?? req.Thing.MapHeld;
						if (obj != null && obj.weatherManager.CurWeatherMaxRangeCap >= 0f)
						{
							WeatherManager weatherManager = (req.Thing.Map ?? req.Thing.MapHeld).weatherManager;
							bool num8 = num7 > weatherManager.CurWeatherMaxRangeCap;
							float num9 = num7;
							num7 = Mathf.Min(num7, weatherManager.CurWeatherMaxRangeCap);
							if (num8)
							{
								stringBuilder7.AppendLine();
								stringBuilder7.AppendLine("    " + "Stat_Thing_Weapon_Range_Clamped".Translate(num7.ToString("F0").Named("CAP"), num9.ToString("F0").Named("ORIGINAL")));
							}
						}
					}
					stringBuilder7.AppendLine();
					stringBuilder7.AppendLine("StatsReport_FinalValue".Translate() + ": " + num7.ToString("F0"));
					_003C_003E2__current = new StatDrawEntry(_003CstatCat_003E5__6, "Range".Translate(), num7.ToString("F0"), stringBuilder7.ToString(), 5390);
					_003C_003E1__state = 24;
					return true;
					IL_197d:
					if (_003C_003E7__wrap2.MoveNext())
					{
						StatDrawEntry current12 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current12;
						_003C_003E1__state = 29;
						return true;
					}
					_003C_003Em__Finally4();
					_003C_003E7__wrap2 = null;
					goto IL_1997;
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
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		private void _003C_003Em__Finally4()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		private void _003C_003Em__Finally5()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		private void _003C_003Em__Finally6()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		private void _003C_003Em__Finally7()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		private void _003C_003Em__Finally8()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		private void _003C_003Em__Finally9()
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
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__407 _003CSpecialDisplayStats_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CSpecialDisplayStats_003Ed__ = this;
			}
			else
			{
				_003CSpecialDisplayStats_003Ed__ = new _003CSpecialDisplayStats_003Ed__407(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CSpecialDisplayStats_003Ed__.req = _003C_003E3__req;
			return _003CSpecialDisplayStats_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	public Type thingClass;

	public ThingCategory category;

	public TickerType tickerType;

	public int stackLimit = 1;

	public IntVec2 size = IntVec2.One;

	public bool destroyable = true;

	public bool rotatable = true;

	public bool smallVolume;

	public bool useHitPoints = true;

	public bool receivesSignals;

	public List<CompProperties> comps = new List<CompProperties>();

	public List<ThingDef> virtualDefs = new List<ThingDef>();

	public ThingDef virtualDefParent;

	[NoTranslate]
	public string devNote;

	public List<ThingDefCountRangeClass> killedLeavingsRanges;

	public List<ThingDefCountClass> killedLeavings;

	public List<ThingDefCountClass> killedLeavingsPlayerHostile;

	public float killedLeavingsChance = 1f;

	public bool forceLeavingsAllowed;

	public List<ThingDefCountClass> butcherProducts;

	public List<ThingDefCountClass> smeltProducts;

	public bool smeltable;

	public bool burnableByRecipe;

	public bool randomizeRotationOnSpawn;

	public List<DamageMultiplier> damageMultipliers;

	public bool isTechHediff;

	public RecipeMakerProperties recipeMaker;

	public ThingDef minifiedDef;

	public bool isUnfinishedThing;

	public bool leaveResourcesWhenKilled;

	public ThingDef slagDef;

	public bool isFrameInt;

	public List<IntVec3> multipleInteractionCellOffsets;

	public IntVec3 interactionCellOffset = IntVec3.Zero;

	public bool hasInteractionCell;

	public ThingDef interactionCellIcon;

	public bool interactionCellIconReverse;

	public ThingDef filthLeaving;

	public bool forceDebugSpawnable;

	public bool intricate;

	public bool scatterableOnMapGen = true;

	public float deepCommonality;

	public int deepCountPerCell = 300;

	public int deepCountPerPortion = -1;

	public IntRange deepLumpSizeRange = IntRange.Zero;

	public float generateCommonality = 1f;

	public float generateAllowChance = 1f;

	private bool canOverlapZones = true;

	public FloatRange startingHpRange = FloatRange.One;

	[NoTranslate]
	public List<string> thingSetMakerTags;

	public bool alwaysFlee;

	public List<RecipeDef> recipes;

	public bool messageOnDeteriorateInStorage = true;

	public bool deteriorateFromEnvironmentalEffects = true;

	public bool canDeteriorateUnspawned;

	public bool canLoadIntoCaravan = true;

	public bool isMechClusterThreat;

	public FloatRange displayNumbersBetweenSameDefDistRange = FloatRange.Zero;

	public int minRewardCount = 1;

	public bool preventSkyfallersLandingOn;

	public FactionDef requiresFactionToAcquire;

	public float relicChance;

	public OrderedTakeGroupDef orderedTakeGroup;

	public int allowedArchonexusCount;

	public int possessionCount;

	public bool notifyMapRemoved;

	public bool canScatterOver = true;

	public bool genericMarketSellable = true;

	public bool drawHighlight;

	public Color? highlightColor;

	public bool drawHighlightOnlyForHostile;

	public bool autoTargetNearbyIdenticalThings;

	public bool preventDroppingThingsOn;

	public bool hiddenWhileUndiscovered;

	public bool disableImpassableShotOverConfigError;

	public bool showInSearch = true;

	public bool bringAlongOnGravship = true;

	public ThingDef dropPodFaller;

	public ThingDef dropPodActive;

	public bool preventSpawningInResourcePod;

	public bool pathfinderDangerous;

	public bool noRightClickDraftAttack;

	public int gravshipSpawnPriority = 1;

	public List<string> replaceTags;

	public bool preventGravshipLandingOn;

	public bool canInteractThroughCorners;

	public GraphicData graphicData;

	public DrawerType drawerType = DrawerType.RealtimeOnly;

	public bool drawOffscreen;

	public ColorGenerator colorGenerator;

	public float hideAtSnowOrSandDepth = 99999f;

	public bool drawDamagedOverlay = true;

	public bool castEdgeShadows;

	public float staticSunShadowHeight;

	public bool useSameGraphicForGhost;

	public bool useBlueprintGraphicAsGhost;

	public List<ThingStyleChance> randomStyle;

	public float randomStyleChance;

	public bool canEditAnyStyle;

	public bool dontPrint;

	public ThingDef defaultStuff;

	public int killedLeavingsExpandRect;

	public bool minifiedManualDraw;

	public float minifiedDrawScale = 1f;

	public Rot4 overrideMinifiedRot = Rot4.Invalid;

	public Vector3 minifiedDrawOffset = Vector3.zero;

	public float deselectedSelectionBracketFactor = 1f;

	public bool selectable;

	public bool containedPawnsSelectable;

	public bool containedItemsSelectable;

	public bool neverMultiSelect;

	public bool isAutoAttackableMapObject;

	public bool hasTooltip;

	public List<Type> inspectorTabs;

	[Unsaved(false)]
	public List<InspectTabBase> inspectorTabsResolved;

	public bool seeThroughFog;

	public bool drawGUIOverlay;

	public bool drawGUIOverlayQuality = true;

	public ResourceCountPriority resourceReadoutPriority;

	public bool resourceReadoutAlwaysShow;

	public bool drawPlaceWorkersWhileSelected;

	public bool drawPlaceWorkersWhileInstallBlueprintSelected;

	public ConceptDef storedConceptLearnOpportunity;

	public float uiIconScale = 1f;

	public bool hasCustomRectForSelector;

	public bool hideStats;

	public bool hideInspect;

	public bool onlyShowInspectString;

	public bool hideMainDesc;

	public bool alwaysHaulable;

	public bool designateHaulable;

	public List<ThingCategoryDef> thingCategories;

	public bool mineable;

	public bool socialPropernessMatters;

	public bool stealable = true;

	public SoundDef soundSpawned;

	public SoundDef soundDrop;

	public SoundDef soundPickup;

	public SoundDef soundInteract;

	public SoundDef soundImpactDefault;

	public SoundDef soundPlayInstrument;

	public SoundDef soundOpen;

	public bool saveCompressible;

	public bool isSaveable = true;

	public bool holdsRoof;

	public float fillPercent;

	public bool coversFloor;

	public bool neverOverlapFloors;

	public SurfaceType surfaceType;

	public bool wipesPlants;

	public bool blockPlants;

	public bool blockLight;

	public bool blockWind;

	public bool blockWeather;

	public Tradeability tradeability = Tradeability.All;

	[NoTranslate]
	public List<string> tradeTags;

	public bool tradeNeverStack;

	public bool tradeNeverGenerateStacked;

	public bool healthAffectsPrice = true;

	public ColorGenerator colorGeneratorInTraderStock;

	private List<VerbProperties> verbs;

	public List<Tool> tools;

	public float equippedAngleOffset;

	public float equippedDistanceOffset;

	public EquipmentType equipmentType;

	public TechLevel techLevel;

	public List<WeaponClassDef> weaponClasses;

	[NoTranslate]
	public List<string> weaponTags;

	[NoTranslate]
	public List<string> techHediffsTags;

	public bool violentTechHediff;

	public bool destroyOnDrop;

	public List<StatModifier> equippedStatOffsets;

	public SoundDef meleeHitSound;

	public float recoilPower = 1f;

	public float recoilRelaxation = 10f;

	public bool rotateInShelves = true;

	public bool mergeVerbGizmos = true;

	public BuildableDef entityDefToBuild;

	public ThingDef projectileWhenLoaded;

	public RulePackDef ideoBuildingNamerBase;

	public EntityCodexEntryDef entityCodexEntry;

	public IngestibleProperties ingestible;

	public FilthProperties filth;

	public GasProperties gas;

	public BuildingProperties building;

	public RaceProperties race;

	public ApparelProperties apparel;

	public MoteProperties mote;

	public PlantProperties plant;

	public ProjectileProperties projectile;

	public StuffProperties stuffProps;

	public SkyfallerProperties skyfaller;

	public PawnFlyerProperties pawnFlyer;

	public RitualFocusProperties ritualFocus;

	public IngredientProperties ingredient;

	public MapPortalProperties portal;

	public bool canBeUsedUnderRoof = true;

	[Unsaved(false)]
	private string descriptionDetailedCached;

	[Unsaved(false)]
	public Graphic interactionCellGraphic;

	[Unsaved(false)]
	private bool? isNaturalOrganCached;

	[Unsaved(false)]
	private bool? hasSunShadowsCached;

	[Unsaved(false)]
	private List<StyleCategoryDef> cachedRelevantStyleCategories;

	public const int SmallUnitPerVolume = 10;

	public const float SmallVolumePerUnit = 0.1f;

	public const float ArchonexusMaxItemStackMass = 5f;

	public const int ArchonexusMaxItemStackCount = 25;

	public const float ArchonexusMaxItemStackValue = 2000f;

	public const int ArchonexusAutoCalculateValue = -1;

	private List<RecipeDef> allRecipesCached;

	private static List<VerbProperties> EmptyVerbPropertiesList = new List<VerbProperties>();

	private Dictionary<ThingDef, Thing> concreteExamplesInt;

	public bool EverHaulable
	{
		get
		{
			if (!alwaysHaulable)
			{
				return designateHaulable;
			}
			return true;
		}
	}

	public bool EverPollutable => !building.isNaturalRock;

	public float VolumePerUnit
	{
		get
		{
			if (smallVolume)
			{
				return 0.1f;
			}
			return 1f;
		}
	}

	public override IntVec2 Size => size;

	public bool DiscardOnDestroyed => race == null;

	public int BaseMaxHitPoints => Mathf.RoundToInt(this.GetStatValueAbstract(StatDefOf.MaxHitPoints));

	public float BaseFlammability => this.GetStatValueAbstract(StatDefOf.Flammability);

	public float BaseMarketValue
	{
		get
		{
			return this.GetStatValueAbstract(StatDefOf.MarketValue);
		}
		set
		{
			this.SetStatBaseValue(StatDefOf.MarketValue, value);
		}
	}

	public float BaseMass => this.GetStatValueAbstract(StatDefOf.Mass);

	public int ArchonexusMaxAllowedCount
	{
		get
		{
			if (allowedArchonexusCount == -1)
			{
				return Mathf.Min(stackLimit, 25, (BaseMass > 0f) ? ((int)(5f / BaseMass)) : 0, (BaseMarketValue > 0f) ? ((int)(2000f / BaseMarketValue)) : 0);
			}
			return allowedArchonexusCount;
		}
	}

	public bool PlayerAcquirable
	{
		get
		{
			if (destroyOnDrop)
			{
				return false;
			}
			if (this == ThingDefOf.ReinforcedBarrel && Find.Storyteller != null && Find.Storyteller.difficulty.classicMortars)
			{
				return false;
			}
			if (requiresFactionToAcquire != null && Find.World != null && Find.World.factionManager != null)
			{
				return Find.FactionManager.FirstFactionOfDef(requiresFactionToAcquire) != null;
			}
			return true;
		}
	}

	public bool EverTransmitsPower
	{
		get
		{
			for (int i = 0; i < comps.Count; i++)
			{
				if (comps[i] is CompProperties_Power { transmitsPower: not false })
				{
					return true;
				}
			}
			return false;
		}
	}

	public bool Minifiable => minifiedDef != null;

	public bool HasThingIDNumber => category != ThingCategory.Mote;

	public List<RecipeDef> AllRecipes
	{
		get
		{
			if (allRecipesCached == null)
			{
				allRecipesCached = new List<RecipeDef>();
				if (recipes != null)
				{
					for (int i = 0; i < recipes.Count; i++)
					{
						allRecipesCached.Add(recipes[i]);
					}
				}
				List<RecipeDef> allDefsListForReading = DefDatabase<RecipeDef>.AllDefsListForReading;
				for (int j = 0; j < allDefsListForReading.Count; j++)
				{
					if (allDefsListForReading[j].recipeUsers != null && allDefsListForReading[j].recipeUsers.Contains(this))
					{
						allRecipesCached.Add(allDefsListForReading[j]);
					}
				}
			}
			return allRecipesCached;
		}
	}

	public bool ConnectToPower
	{
		get
		{
			if (EverTransmitsPower)
			{
				return false;
			}
			for (int i = 0; i < comps.Count; i++)
			{
				if (comps[i].compClass == typeof(CompPowerBattery))
				{
					return true;
				}
				if (comps[i].compClass == typeof(CompPowerTrader))
				{
					return true;
				}
			}
			return false;
		}
	}

	public bool CoexistsWithFloors
	{
		get
		{
			if (!neverOverlapFloors)
			{
				return !coversFloor;
			}
			return false;
		}
	}

	public FillCategory Fillage
	{
		get
		{
			if (fillPercent < 0.01f)
			{
				return FillCategory.None;
			}
			if (fillPercent > 0.99f)
			{
				return FillCategory.Full;
			}
			return FillCategory.Partial;
		}
	}

	public bool MakeFog => Fillage == FillCategory.Full;

	public bool CanOverlapZones
	{
		get
		{
			if (building != null && building.SupportsPlants)
			{
				return false;
			}
			if (passability == Traversability.Impassable && category != ThingCategory.Plant && !HasComp(typeof(CompTransporter)))
			{
				return false;
			}
			if ((int)surfaceType >= 1)
			{
				return false;
			}
			if (typeof(ISlotGroupParent).IsAssignableFrom(thingClass))
			{
				return false;
			}
			if (!canOverlapZones)
			{
				return false;
			}
			if ((IsBlueprint || IsFrame) && entityDefToBuild is ThingDef thingDef)
			{
				return thingDef.CanOverlapZones;
			}
			return true;
		}
	}

	public bool CountAsResource => resourceReadoutPriority != ResourceCountPriority.Uncounted;

	public List<VerbProperties> Verbs
	{
		get
		{
			if (verbs != null)
			{
				return verbs;
			}
			return EmptyVerbPropertiesList;
		}
	}

	public bool CanHaveFaction
	{
		get
		{
			if (IsBlueprint || IsFrame)
			{
				return true;
			}
			return category switch
			{
				ThingCategory.Pawn => true, 
				ThingCategory.Building => true, 
				_ => false, 
			};
		}
	}

	public bool Claimable
	{
		get
		{
			if (building != null && building.claimable)
			{
				return !building.isNaturalRock;
			}
			return false;
		}
	}

	public ThingCategoryDef FirstThingCategory
	{
		get
		{
			if (thingCategories.NullOrEmpty())
			{
				return null;
			}
			return thingCategories[0];
		}
	}

	public float MedicineTendXpGainFactor => Mathf.Clamp(this.GetStatValueAbstract(StatDefOf.MedicalPotency) * 0.7f, 0.5f, 1f);

	public bool CanEverDeteriorate
	{
		get
		{
			if (!useHitPoints)
			{
				return false;
			}
			if (category != ThingCategory.Item)
			{
				if (plant != null)
				{
					return plant.canDeteriorate;
				}
				return false;
			}
			return true;
		}
	}

	public bool CanInteractThroughCorners
	{
		get
		{
			if (canInteractThroughCorners)
			{
				return true;
			}
			if (category != ThingCategory.Building)
			{
				return false;
			}
			if (!holdsRoof)
			{
				return false;
			}
			if (building != null && building.isNaturalRock && !IsSmoothed)
			{
				return false;
			}
			return true;
		}
	}

	public bool AffectsRegions
	{
		get
		{
			if (passability != Traversability.Impassable && !IsDoor)
			{
				return IsFence;
			}
			return true;
		}
	}

	public bool AffectsReachability
	{
		get
		{
			if (AffectsRegions)
			{
				return true;
			}
			if (passability == Traversability.Impassable || IsDoor)
			{
				return true;
			}
			if (TouchPathEndModeUtility.MakesOccupiedCellsAlwaysReachableDiagonally(this))
			{
				return true;
			}
			return false;
		}
	}

	public string DescriptionDetailed
	{
		get
		{
			if (descriptionDetailedCached == null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(description);
				if (IsApparel)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine();
					stringBuilder.AppendLine($"{"Layer".Translate()}: {apparel.GetLayersString()}");
					stringBuilder.Append($"{"Covers".Translate()}: {apparel.GetCoveredOuterPartsString(BodyDefOf.Human)}");
					if (equippedStatOffsets != null && equippedStatOffsets.Count > 0)
					{
						stringBuilder.AppendLine();
						stringBuilder.AppendLine();
						for (int i = 0; i < equippedStatOffsets.Count; i++)
						{
							if (i > 0)
							{
								stringBuilder.AppendLine();
							}
							StatModifier statModifier = equippedStatOffsets[i];
							stringBuilder.Append($"{statModifier.stat.LabelCap}: {statModifier.ValueToStringAsOffset}");
						}
					}
				}
				descriptionDetailedCached = stringBuilder.ToString();
			}
			return descriptionDetailedCached;
		}
	}

	public bool CanBenefitFromCover
	{
		get
		{
			if (category == ThingCategory.Pawn)
			{
				return true;
			}
			if (building != null && building.IsTurret)
			{
				return true;
			}
			return false;
		}
	}

	public bool PotentiallySmeltable
	{
		get
		{
			if (!smeltable)
			{
				return false;
			}
			if (base.MadeFromStuff)
			{
				foreach (ThingDef item in GenStuff.AllowedStuffsFor(this))
				{
					if (item.smeltable)
					{
						return true;
					}
				}
				return false;
			}
			return true;
		}
	}

	public bool HasSingleOrMultipleInteractionCells
	{
		get
		{
			if (!hasInteractionCell)
			{
				return !multipleInteractionCellOffsets.NullOrEmpty();
			}
			return true;
		}
	}

	public bool IsApparel => apparel != null;

	public bool IsBed => typeof(Building_Bed).IsAssignableFrom(thingClass);

	public bool IsWall
	{
		get
		{
			if (building != null)
			{
				return building.isWall;
			}
			return false;
		}
	}

	public bool IsCorpse => typeof(Corpse).IsAssignableFrom(thingClass);

	public bool IsFrame => isFrameInt;

	public bool IsBlueprint
	{
		get
		{
			if (entityDefToBuild != null)
			{
				return category == ThingCategory.Ethereal;
			}
			return false;
		}
	}

	public bool IsStuff => stuffProps != null;

	public bool IsMedicine => statBases.StatListContains(StatDefOf.MedicalPotency);

	public bool IsDoor => typeof(Building_Door).IsAssignableFrom(thingClass);

	public bool IsFence
	{
		get
		{
			if (building != null)
			{
				return building.isFence;
			}
			return false;
		}
	}

	public bool IsFilth => filth != null;

	public bool IsIngestible => ingestible != null;

	public bool IsNutritionGivingIngestible
	{
		get
		{
			if (IsIngestible)
			{
				return ingestible.CachedNutrition > 0f;
			}
			return false;
		}
	}

	public bool IsNutritionGivingIngestibleForHumanlikeBabies
	{
		get
		{
			if (IsNutritionGivingIngestible && ingestible.HumanEdible)
			{
				return ingestible.babiesCanIngest;
			}
			return false;
		}
	}

	public bool IsWeapon
	{
		get
		{
			if (category == ThingCategory.Item && (!verbs.NullOrEmpty() || !tools.NullOrEmpty()))
			{
				return !IsApparel;
			}
			return false;
		}
	}

	public bool IsCommsConsole => typeof(Building_CommsConsole).IsAssignableFrom(thingClass);

	public bool IsOrbitalTradeBeacon => typeof(Building_OrbitalTradeBeacon).IsAssignableFrom(thingClass);

	public bool IsFoodDispenser => typeof(Building_NutrientPasteDispenser).IsAssignableFrom(thingClass);

	public bool IsDrug
	{
		get
		{
			if (ingestible != null)
			{
				return ingestible.drugCategory != DrugCategory.None;
			}
			return false;
		}
	}

	public bool IsPleasureDrug
	{
		get
		{
			if (IsDrug)
			{
				return ingestible.joy > 0f;
			}
			return false;
		}
	}

	public bool IsNonMedicalDrug
	{
		get
		{
			if (IsDrug)
			{
				return ingestible.drugCategory != DrugCategory.Medical;
			}
			return false;
		}
	}

	public bool IsTable
	{
		get
		{
			if (surfaceType == SurfaceType.Eat)
			{
				return HasComp(typeof(CompGatherSpot));
			}
			return false;
		}
	}

	public bool IsWorkTable => typeof(Building_WorkTable).IsAssignableFrom(thingClass);

	public bool IsShell => projectileWhenLoaded != null;

	public bool IsArt => IsWithinCategory(ThingCategoryDefOf.BuildingsArt);

	public bool IsSmoothable => building?.smoothedThing != null;

	public bool IsSmoothed => building?.unsmoothedThing != null;

	public bool IsMetal
	{
		get
		{
			if (stuffProps != null)
			{
				return stuffProps.categories.Contains(StuffCategoryDefOf.Metallic);
			}
			return false;
		}
	}

	public bool IsCryptosleepCasket => typeof(Building_CryptosleepCasket).IsAssignableFrom(thingClass);

	public bool IsGibbetCage => typeof(Building_GibbetCage).IsAssignableFrom(thingClass);

	public bool IsMechGestator => typeof(Building_MechGestator).IsAssignableFrom(thingClass);

	public bool IsMechRecharger => typeof(Building_MechCharger).IsAssignableFrom(thingClass);

	public bool IsAddictiveDrug
	{
		get
		{
			CompProperties_Drug compProperties = GetCompProperties<CompProperties_Drug>();
			if (compProperties != null)
			{
				return compProperties.addictiveness > 0f;
			}
			return false;
		}
	}

	public bool IsMeat
	{
		get
		{
			if (category == ThingCategory.Item && thingCategories != null)
			{
				return thingCategories.Contains(ThingCategoryDefOf.MeatRaw);
			}
			return false;
		}
	}

	public bool IsEgg
	{
		get
		{
			if (category == ThingCategory.Item && thingCategories != null)
			{
				if (!thingCategories.Contains(ThingCategoryDefOf.EggsFertilized))
				{
					return thingCategories.Contains(ThingCategoryDefOf.EggsUnfertilized);
				}
				return true;
			}
			return false;
		}
	}

	public bool IsLeather
	{
		get
		{
			if (category == ThingCategory.Item && thingCategories != null)
			{
				return thingCategories.Contains(ThingCategoryDefOf.Leathers);
			}
			return false;
		}
	}

	public bool IsWool
	{
		get
		{
			if (category == ThingCategory.Item && thingCategories != null)
			{
				return thingCategories.Contains(ThingCategoryDefOf.Wools);
			}
			return false;
		}
	}

	public bool IsRangedWeapon
	{
		get
		{
			if (!IsWeapon)
			{
				return false;
			}
			if (!verbs.NullOrEmpty())
			{
				for (int i = 0; i < verbs.Count; i++)
				{
					if (!verbs[i].IsMeleeAttack)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	public bool IsMeleeWeapon
	{
		get
		{
			if (IsWeapon)
			{
				return !IsRangedWeapon;
			}
			return false;
		}
	}

	public bool IsWeaponUsingProjectiles
	{
		get
		{
			if (!IsWeapon)
			{
				return false;
			}
			if (!verbs.NullOrEmpty())
			{
				for (int i = 0; i < verbs.Count; i++)
				{
					if (verbs[i].LaunchesProjectile)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	public bool IsShieldThatBlocksRanged
	{
		get
		{
			if (HasComp(typeof(CompShield)))
			{
				return GetCompProperties<CompProperties_Shield>().blocksRangedWeapons;
			}
			return false;
		}
	}

	public bool IsBuildingArtificial
	{
		get
		{
			if (category == ThingCategory.Building || IsFrame)
			{
				if (building != null)
				{
					if (!building.isNaturalRock)
					{
						return !building.isResourceRock;
					}
					return false;
				}
				return true;
			}
			return false;
		}
	}

	public bool IsNonResourceNaturalRock
	{
		get
		{
			if (category == ThingCategory.Building && building.isNaturalRock && !building.isResourceRock && !building.mineablePreventNaturalRockOnSurface)
			{
				return !IsSmoothed;
			}
			return false;
		}
	}

	public bool HasSunShadows
	{
		get
		{
			if (!hasSunShadowsCached.HasValue)
			{
				hasSunShadowsCached = typeof(Pawn).IsAssignableFrom(thingClass);
			}
			return hasSunShadowsCached.Value;
		}
	}

	public bool IsNaturalOrgan
	{
		get
		{
			if (!isNaturalOrganCached.HasValue)
			{
				if (category != ThingCategory.Item)
				{
					isNaturalOrganCached = false;
				}
				else
				{
					List<BodyPartDef> allDefsListForReading = DefDatabase<BodyPartDef>.AllDefsListForReading;
					isNaturalOrganCached = false;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						if (allDefsListForReading[i].spawnThingOnRemoved == this)
						{
							isNaturalOrganCached = true;
							break;
						}
					}
				}
			}
			return isNaturalOrganCached.Value;
		}
	}

	public bool IsFungus
	{
		get
		{
			if (ingestible != null)
			{
				return ingestible.foodType.HasFlag(FoodTypeFlags.Fungus);
			}
			return false;
		}
	}

	public bool IsAnimalProduct
	{
		get
		{
			if (ingestible != null)
			{
				return ingestible.foodType.HasFlag(FoodTypeFlags.AnimalProduct);
			}
			return false;
		}
	}

	public bool IsProcessedFood
	{
		get
		{
			if (ingestible != null)
			{
				return ingestible.foodType.HasFlag(FoodTypeFlags.Processed);
			}
			return false;
		}
	}

	public bool CanAffectLinker
	{
		get
		{
			if (graphicData == null || !graphicData.Linked)
			{
				return IsDoor;
			}
			return true;
		}
	}

	public bool IsNonDeconstructibleAttackableBuilding
	{
		get
		{
			if (IsBuildingArtificial && !building.IsDeconstructible && destroyable && !mineable && building.isTargetable)
			{
				return building.draftAttackNonDeconstructable;
			}
			return false;
		}
	}

	public bool IsPlant => typeof(Plant).IsAssignableFrom(thingClass);

	public bool IsDeadPlant => typeof(DeadPlant).IsAssignableFrom(thingClass);

	public bool IsStudiable => HasAssignableCompFrom(typeof(CompStudiable));

	public List<StyleCategoryDef> RelevantStyleCategories
	{
		get
		{
			if (cachedRelevantStyleCategories == null)
			{
				cachedRelevantStyleCategories = new List<StyleCategoryDef>();
				foreach (StyleCategoryDef allDef in DefDatabase<StyleCategoryDef>.AllDefs)
				{
					if (allDef.thingDefStyles.NullOrEmpty())
					{
						continue;
					}
					foreach (ThingDefStyle thingDefStyle in allDef.thingDefStyles)
					{
						if (thingDefStyle.ThingDef == this)
						{
							cachedRelevantStyleCategories.Add(allDef);
							break;
						}
					}
				}
			}
			return cachedRelevantStyleCategories;
		}
	}

	public string LabelAsStuff
	{
		get
		{
			if (!stuffProps.stuffAdjective.NullOrEmpty())
			{
				return stuffProps.stuffAdjective;
			}
			return label;
		}
	}

	public bool BlocksPlanting(bool canWipePlants = false)
	{
		if (building != null && building.SupportsPlants)
		{
			return false;
		}
		if (building != null && building.isAttachment)
		{
			return false;
		}
		if (blockPlants)
		{
			return true;
		}
		if (!canWipePlants && category == ThingCategory.Plant)
		{
			return true;
		}
		if ((int)Fillage > 0)
		{
			return true;
		}
		if (this.IsEdifice())
		{
			return true;
		}
		return false;
	}

	public virtual bool CanSpawnAt(IntVec3 pos, Rot4 rot, Map map)
	{
		return true;
	}

	public bool EverStorable(bool willMinifyIfPossible)
	{
		if (typeof(MinifiedThing).IsAssignableFrom(thingClass))
		{
			return true;
		}
		if (!thingCategories.NullOrEmpty())
		{
			if (category == ThingCategory.Item)
			{
				return true;
			}
			if (willMinifyIfPossible && Minifiable)
			{
				return true;
			}
		}
		return false;
	}

	public Thing GetConcreteExample(ThingDef stuff = null)
	{
		if (concreteExamplesInt == null)
		{
			concreteExamplesInt = new Dictionary<ThingDef, Thing>();
		}
		if (stuff == null)
		{
			stuff = ThingDefOf.Steel;
		}
		if (!concreteExamplesInt.ContainsKey(stuff))
		{
			if (race == null)
			{
				concreteExamplesInt[stuff] = ThingMaker.MakeThing(this, base.MadeFromStuff ? stuff : null);
			}
			else
			{
				concreteExamplesInt[stuff] = PawnGenerator.GeneratePawn(DefDatabase<PawnKindDef>.AllDefsListForReading.FirstOrDefault((PawnKindDef pkd) => pkd.race == this), null, null);
			}
		}
		return concreteExamplesInt[stuff];
	}

	public CompProperties CompDefFor<T>() where T : ThingComp
	{
		for (int i = 0; i < comps.Count; i++)
		{
			if (comps[i].compClass == typeof(T))
			{
				return comps[i];
			}
		}
		return null;
	}

	public CompProperties CompDefForAssignableFrom<T>() where T : ThingComp
	{
		for (int i = 0; i < comps.Count; i++)
		{
			if (typeof(T).IsAssignableFrom(comps[i].compClass))
			{
				return comps[i];
			}
		}
		return null;
	}

	public bool HasComp(Type compType)
	{
		for (int i = 0; i < comps.Count; i++)
		{
			if (comps[i].compClass == compType)
			{
				return true;
			}
		}
		return false;
	}

	public bool HasComp<T>() where T : ThingComp
	{
		for (int i = 0; i < comps.Count; i++)
		{
			if (comps[i].compClass == typeof(T) || typeof(T).IsAssignableFrom(comps[i].compClass))
			{
				return true;
			}
		}
		return false;
	}

	public bool HasAssignableCompFrom(Type compType)
	{
		for (int i = 0; i < comps.Count; i++)
		{
			if (compType.IsAssignableFrom(comps[i].compClass))
			{
				return true;
			}
		}
		return false;
	}

	public T GetCompProperties<T>() where T : CompProperties
	{
		for (int i = 0; i < comps.Count; i++)
		{
			if (comps[i] is T result)
			{
				return result;
			}
		}
		return null;
	}

	public override void PostLoad()
	{
		if (graphicData != null)
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				GraphicData graphicData = this.graphicData;
				if (graphicData.shaderType == null)
				{
					graphicData.shaderType = ShaderTypeDefOf.Cutout;
				}
				ContentFinderRequester.requester = this;
				try
				{
					graphic = this.graphicData.Graphic;
					if (drawerType != DrawerType.RealtimeOnly)
					{
						TextureAtlasGroup textureAtlasGroup = category.ToAtlasGroup();
						graphic.TryInsertIntoAtlas(textureAtlasGroup);
						if (textureAtlasGroup == TextureAtlasGroup.Building && Minifiable)
						{
							graphic.TryInsertIntoAtlas(TextureAtlasGroup.Item);
						}
					}
				}
				finally
				{
					ContentFinderRequester.requester = null;
				}
			});
		}
		if (tools != null)
		{
			for (int i = 0; i < tools.Count; i++)
			{
				tools[i].id = i.ToString();
			}
		}
		if (verbs != null && verbs.Count == 1 && verbs[0].label.NullOrEmpty())
		{
			verbs[0].label = label;
		}
		base.PostLoad();
		if (category == ThingCategory.Building && building == null)
		{
			building = new BuildingProperties();
		}
		building?.PostLoadSpecial(this);
		apparel?.PostLoadSpecial(this);
		plant?.PostLoadSpecial(this);
		if (comps == null)
		{
			return;
		}
		foreach (CompProperties comp in comps)
		{
			comp.PostLoadSpecial(this);
		}
	}

	protected override void ResolveIcon()
	{
		base.ResolveIcon();
		if (category == ThingCategory.Pawn)
		{
			if (!uiIconPath.NullOrEmpty())
			{
				uiIcon = ContentFinder<Texture2D>.Get(uiIconPath);
			}
			else
			{
				if (race.Humanlike)
				{
					return;
				}
				PawnKindDef anyPawnKind = race.AnyPawnKind;
				if (anyPawnKind != null)
				{
					Material material = ((ModsConfig.BiotechActive && anyPawnKind.RaceProps.IsMechanoid) ? anyPawnKind.lifeStages.First() : anyPawnKind.lifeStages.Last()).bodyGraphicData.Graphic.MatAt(Rot4.East);
					uiIcon = (Texture2D)material.mainTexture;
					uiIconColor = material.color;
					if (ShaderDatabase.TryGetUIShader(material.shader, out var uiShader) && MaterialPool.TryGetRequestForMat(material, out var request))
					{
						request.shader = uiShader;
						uiIconMaterial = MaterialPool.MatFrom(request);
					}
				}
			}
		}
		else
		{
			ThingDef thingDef = GenStuff.DefaultStuffFor(this);
			if (colorGenerator != null && (thingDef == null || thingDef.stuffProps.allowColorGenerators))
			{
				uiIconColor = colorGenerator.ExemplaryColor;
			}
			else if (thingDef != null)
			{
				uiIconColor = GetColorForStuff(thingDef);
			}
			else if (graphicData != null)
			{
				uiIconColor = graphicData.color;
			}
			if (rotatable && graphic != null && graphic != BaseContent.BadGraphic && graphic.ShouldDrawRotated && defaultPlacingRot == Rot4.South)
			{
				uiIconAngle = 180f + graphic.DrawRotatedExtraAngleOffset;
			}
		}
	}

	public override void ResolveReferences()
	{
		base.ResolveReferences();
		if (ingestible != null)
		{
			ingestible.parent = this;
		}
		if (stuffProps != null)
		{
			stuffProps.parent = this;
		}
		building?.ResolveReferencesSpecial();
		graphicData?.ResolveReferencesSpecial();
		race?.ResolveReferencesSpecial();
		stuffProps?.ResolveReferencesSpecial();
		apparel?.ResolveReferencesSpecial();
		if (soundImpactDefault == null)
		{
			soundImpactDefault = SoundDefOf.BulletImpact_Ground;
		}
		if (soundDrop == null)
		{
			soundDrop = SoundDefOf.Standard_Drop;
		}
		if (soundPickup == null)
		{
			soundPickup = SoundDefOf.Standard_Pickup;
		}
		if (soundInteract == null)
		{
			soundInteract = SoundDefOf.Standard_Pickup;
		}
		if (inspectorTabs != null && inspectorTabs.Any())
		{
			inspectorTabsResolved = new List<InspectTabBase>();
			for (int i = 0; i < inspectorTabs.Count; i++)
			{
				try
				{
					inspectorTabsResolved.Add(InspectTabManager.GetSharedInstance(inspectorTabs[i]));
				}
				catch (Exception ex)
				{
					Log.Error("Could not instantiate inspector tab of type " + inspectorTabs[i]?.ToString() + ": " + ex);
				}
			}
		}
		if (comps != null)
		{
			for (int j = 0; j < comps.Count; j++)
			{
				comps[j].ResolveReferences(this);
			}
		}
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__401))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__401(-2)
		{
			_003C_003E4__this = this
		};
	}

	public static ThingDef Named(string defName)
	{
		return DefDatabase<ThingDef>.GetNamed(defName);
	}

	public bool IsWithinCategory(ThingCategoryDef category)
	{
		if (thingCategories == null)
		{
			return false;
		}
		for (int i = 0; i < thingCategories.Count; i++)
		{
			for (ThingCategoryDef thingCategoryDef = thingCategories[i]; thingCategoryDef != null; thingCategoryDef = thingCategoryDef.parent)
			{
				if (thingCategoryDef == category)
				{
					return true;
				}
			}
		}
		return false;
	}

	public void Notify_UnlockedByResearch()
	{
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].Notify_PostUnlockedByResearch(this);
			}
		}
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__407))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__407(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__req = req
		};
	}
}
