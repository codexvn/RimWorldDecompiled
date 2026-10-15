using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class RoyalTitleDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__64 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public RoyalTitleDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

		private List<JoyKindDef>.Enumerator _003C_003E7__wrap2;

		private List<RoomRequirement>.Enumerator _003C_003E7__wrap3;

		private RoomRequirement _003Creq_003E5__5;

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
		public _003CConfigErrors_003Ed__64(int _003C_003E1__state)
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
			case -6:
			case -5:
			case 7:
				try
				{
					if (num == -6 || num == 7)
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
			_003C_003E7__wrap2 = default(List<JoyKindDef>.Enumerator);
			_003C_003E7__wrap3 = default(List<RoomRequirement>.Enumerator);
			_003Creq_003E5__5 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				RoyalTitleDef royalTitleDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)royalTitleDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0086;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0086;
				case 2:
					_003C_003E1__state = -1;
					goto IL_010a;
				case 3:
					_003C_003E1__state = -1;
					goto IL_0169;
				case 4:
					_003C_003E1__state = -4;
					goto IL_01e7;
				case 5:
					_003C_003E1__state = -1;
					goto IL_023d;
				case 6:
					_003C_003E1__state = -1;
					goto IL_026e;
				case 7:
					{
						_003C_003E1__state = -6;
						goto IL_030f;
					}
					IL_0086:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					ModLister.CheckRoyalty("Royal title");
					if (royalTitleDef.awardThought != null && !typeof(Thought_MemoryRoyalTitle).IsAssignableFrom(royalTitleDef.awardThought.thoughtClass))
					{
						_003C_003E2__current = $"Royal title {royalTitleDef.defName} has awardThought with thoughtClass {royalTitleDef.awardThought.thoughtClass.FullName} which is not deriving from Thought_MemoryRoyalTitle!";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_010a;
					IL_030f:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = $"Room requirement {_003Creq_003E5__5.GetType().Name}: {current2}";
						_003C_003E1__state = 7;
						return true;
					}
					_003C_003Em__Finally4();
					_003C_003E7__wrap1 = null;
					_003Creq_003E5__5 = null;
					goto IL_0330;
					IL_0330:
					if (_003C_003E7__wrap3.MoveNext())
					{
						_003Creq_003E5__5 = _003C_003E7__wrap3.Current;
						_003C_003E7__wrap1 = _003Creq_003E5__5.ConfigErrors().GetEnumerator();
						_003C_003E1__state = -6;
						goto IL_030f;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap3 = default(List<RoomRequirement>.Enumerator);
					break;
					IL_023d:
					if (royalTitleDef.Awardable && royalTitleDef.changeHeirQuestPoints < 0)
					{
						_003C_003E2__current = "undefined changeHeirQuestPoints, it's required for awardable titles";
						_003C_003E1__state = 6;
						return true;
					}
					goto IL_026e;
					IL_010a:
					if (royalTitleDef.lostThought != null && !typeof(Thought_MemoryRoyalTitle).IsAssignableFrom(royalTitleDef.lostThought.thoughtClass))
					{
						_003C_003E2__current = $"Royal title {royalTitleDef.defName} has awardThought with thoughtClass {royalTitleDef.awardThought.thoughtClass.FullName} which is not deriving from Thought_MemoryRoyalTitle!";
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_0169;
					IL_0206:
					if (!typeof(RoyalTitleAwardWorker).IsAssignableFrom(royalTitleDef.awardWorkerClass))
					{
						_003C_003E2__current = "awardWorkerClass does not derive from RoyalTitleAwardWorker";
						_003C_003E1__state = 5;
						return true;
					}
					goto IL_023d;
					IL_0169:
					if (royalTitleDef.disabledJoyKinds != null)
					{
						_003C_003E7__wrap2 = royalTitleDef.disabledJoyKinds.GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_01e7;
					}
					goto IL_0206;
					IL_026e:
					if (royalTitleDef.throneRoomRequirements.NullOrEmpty())
					{
						break;
					}
					_003C_003E7__wrap3 = royalTitleDef.throneRoomRequirements.GetEnumerator();
					_003C_003E1__state = -5;
					goto IL_0330;
					IL_01e7:
					while (_003C_003E7__wrap2.MoveNext())
					{
						JoyKindDef current3 = _003C_003E7__wrap2.Current;
						if (current3.titleRequiredAny != null && current3.titleRequiredAny.Contains(royalTitleDef))
						{
							_003C_003E2__current = $"Royal title {royalTitleDef.defName} disables joy kind {current3.defName} which requires the title!";
							_003C_003E1__state = 4;
							return true;
						}
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = default(List<JoyKindDef>.Enumerator);
					goto IL_0206;
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
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap3/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally4()
		{
			_003C_003E1__state = -5;
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
			_003CConfigErrors_003Ed__64 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__64(0)
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
	private sealed class _003CRequiredApparelListForGender_003Ed__62 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public RoyalTitleDef _003C_003E4__this;

		private Pawn forPawn;

		public Pawn _003C_003E3__forPawn;

		private Gender g;

		public Gender _003C_003E3__g;

		private bool _003CanyRequirementValid_003E5__2;

		private List<ApparelRequirement>.Enumerator _003C_003E7__wrap2;

		private string _003CappendDisabledRequirement_003E5__4;

		private IEnumerator<TaggedString> _003C_003E7__wrap4;

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
		public _003CRequiredApparelListForGender_003Ed__62(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || (uint)(num - 1) <= 1u)
			{
				try
				{
					if (num == -4 || (uint)(num - 1) <= 1u)
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
			_003C_003E7__wrap2 = default(List<ApparelRequirement>.Enumerator);
			_003CappendDisabledRequirement_003E5__4 = null;
			_003C_003E7__wrap4 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				RoyalTitleDef royalTitleDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003CanyRequirementValid_003E5__2 = false;
					_003C_003E7__wrap2 = royalTitleDef.requiredApparel.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_01ab;
				case 1:
					_003C_003E1__state = -4;
					goto IL_018a;
				case 2:
					_003C_003E1__state = -4;
					goto IL_018a;
				case 3:
					_003C_003E1__state = -1;
					_003C_003E2__current = "ApparelRequirementAnyPsycasterApparel".Translate();
					_003C_003E1__state = 4;
					return true;
				case 4:
					_003C_003E1__state = -1;
					if (ModsConfig.BiotechActive)
					{
						_003C_003E2__current = "ApparelRequirementAnyMechlordApparel".Translate();
						_003C_003E1__state = 5;
						return true;
					}
					break;
				case 5:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_018a:
					if (_003C_003E7__wrap4.MoveNext())
					{
						TaggedString current = _003C_003E7__wrap4.Current;
						if (_003CappendDisabledRequirement_003E5__4 == null)
						{
							_003C_003E2__current = current;
							_003C_003E1__state = 1;
							return true;
						}
						_003C_003E2__current = current + " " + _003CappendDisabledRequirement_003E5__4;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap4 = null;
					_003CappendDisabledRequirement_003E5__4 = null;
					goto IL_01ab;
					IL_01ab:
					if (_003C_003E7__wrap2.MoveNext())
					{
						ApparelRequirement current2 = _003C_003E7__wrap2.Current;
						_003CappendDisabledRequirement_003E5__4 = null;
						if (forPawn != null && !ApparelUtility.IsRequirementActive(current2, ApparelRequirementSource.Title, forPawn, out var disabledByLabel))
						{
							_003CappendDisabledRequirement_003E5__4 = " [" + "ApparelRequirementDisabledLabel".Translate() + ": " + disabledByLabel + "]";
						}
						else
						{
							_003CanyRequirementValid_003E5__2 = true;
						}
						_003C_003E7__wrap4 = (from a in current2.AllRequiredApparel(g).Distinct()
							select a.LabelCap).GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_018a;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap2 = default(List<ApparelRequirement>.Enumerator);
					if (_003CanyRequirementValid_003E5__2)
					{
						_003C_003E2__current = "ApparelRequirementAnyPrestigeArmor".Translate();
						_003C_003E1__state = 3;
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
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
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
			_003CRequiredApparelListForGender_003Ed__62 _003CRequiredApparelListForGender_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CRequiredApparelListForGender_003Ed__ = this;
			}
			else
			{
				_003CRequiredApparelListForGender_003Ed__ = new _003CRequiredApparelListForGender_003Ed__62(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CRequiredApparelListForGender_003Ed__.g = _003C_003E3__g;
			_003CRequiredApparelListForGender_003Ed__.forPawn = _003C_003E3__forPawn;
			return _003CRequiredApparelListForGender_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__61 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public RoyalTitleDef _003C_003E4__this;

		private StatRequest req;

		public StatRequest _003C_003E3__req;

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
		public _003CSpecialDisplayStats_003Ed__61(int _003C_003E1__state)
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
			RoyalTitleDef royalTitleDef = _003C_003E4__this;
			IEnumerable<string> enumerable;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!royalTitleDef.permits.NullOrEmpty())
				{
					TaggedString taggedString = "RoyalTitleTooltipPermits".Translate();
					string valueString = royalTitleDef.permits.Select((RoyalTitlePermitDef r) => r.label).ToCommaList().CapitalizeFirst();
					string reportText = royalTitleDef.permits.Select((RoyalTitlePermitDef r) => r.LabelCap.ToString()).ToLineList("  - ", capitalizeItems: true);
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.BasicsImportant, taggedString, valueString, reportText, 99999);
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_00fa;
			case 1:
				_003C_003E1__state = -1;
				goto IL_00fa;
			case 2:
				_003C_003E1__state = -1;
				goto IL_0155;
			case 3:
				_003C_003E1__state = -1;
				goto IL_0255;
			case 4:
				_003C_003E1__state = -1;
				goto IL_0314;
			case 5:
				_003C_003E1__state = -1;
				goto IL_03d7;
			case 6:
				_003C_003E1__state = -1;
				goto IL_0495;
			case 7:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0155:
				if (!royalTitleDef.requiredApparel.NullOrEmpty())
				{
					TaggedString taggedString2 = "RoyalTitleTooltipRequiredApparel".Translate();
					TaggedString taggedString3 = "Male".Translate().CapitalizeFirst() + ":\n" + royalTitleDef.RequiredApparelListForGender(Gender.Male, req.Pawn).ToLineList("  - ") + "\n\n" + "Female".Translate().CapitalizeFirst() + ":\n" + royalTitleDef.RequiredApparelListForGender(Gender.Female, req.Pawn).ToLineList("  - ");
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.BasicsImportant, taggedString2, "", "RoyalTitleRequiredApparelStatDescription".Translate() + ":\n\n" + taggedString3, 99998);
					_003C_003E1__state = 3;
					return true;
				}
				goto IL_0255;
				IL_0255:
				if (!royalTitleDef.bedroomRequirements.NullOrEmpty())
				{
					TaggedString taggedString4 = "RoyalTitleTooltipBedroomRequirements".Translate();
					string valueString2 = royalTitleDef.bedroomRequirements.Select((RoomRequirement r) => r.Label()).ToCommaList().CapitalizeFirst();
					string reportText2 = royalTitleDef.bedroomRequirements.Select((RoomRequirement r) => r.LabelCap()).ToLineList("  - ");
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.BasicsImportant, taggedString4, valueString2, reportText2, 99997);
					_003C_003E1__state = 4;
					return true;
				}
				goto IL_0314;
				IL_0495:
				if (royalTitleDef.foodRequirement.Defined && royalTitleDef.SatisfyingMeals().Any())
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "RoyalTitleRequiredMeals".Translate(), (from m in royalTitleDef.SatisfyingMeals()
						select m.label).ToCommaList().CapitalizeFirst(), "RoyalTitleRequiredMealsDesc".Translate(), 99995);
					_003C_003E1__state = 7;
					return true;
				}
				break;
				IL_03d7:
				enumerable = from t in royalTitleDef.disabledWorkTags.GetAllSelectedItems<WorkTags>()
					where t != WorkTags.None
					select t into w
					select w.LabelTranslated();
				if (enumerable.Any())
				{
					TaggedString taggedString5 = "DisabledWorkTypes".Translate();
					string valueString3 = enumerable.ToCommaList().CapitalizeFirst();
					string reportText3 = enumerable.ToLineList(" -  ", capitalizeItems: true);
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.BasicsImportant, taggedString5, valueString3, reportText3, 99994);
					_003C_003E1__state = 6;
					return true;
				}
				goto IL_0495;
				IL_00fa:
				if ((int)royalTitleDef.requiredMinimumApparelQuality > 0)
				{
					TaggedString taggedString6 = "RoyalTitleTooltipRequiredApparelQuality".Translate();
					string text = royalTitleDef.requiredMinimumApparelQuality.GetLabel().CapitalizeFirst();
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.BasicsImportant, taggedString6, text, text, 99998);
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_0155;
				IL_0314:
				if (!royalTitleDef.throneRoomRequirements.NullOrEmpty())
				{
					TaggedString taggedString7 = "RoyalTitleTooltipThroneroomRequirements".Translate();
					string valueString4 = royalTitleDef.throneRoomRequirements.Select((RoomRequirement r) => r.Label()).ToCommaList().CapitalizeFirst();
					string reportText4 = royalTitleDef.throneRoomRequirements.Select((RoomRequirement r) => r.LabelCap()).ToArray().ToLineList("  - ");
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.BasicsImportant, taggedString7, valueString4, reportText4, 99997);
					_003C_003E1__state = 5;
					return true;
				}
				goto IL_03d7;
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
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__61 _003CSpecialDisplayStats_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CSpecialDisplayStats_003Ed__ = this;
			}
			else
			{
				_003CSpecialDisplayStats_003Ed__ = new _003CSpecialDisplayStats_003Ed__61(0)
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

	[CompilerGenerated]
	private sealed class _003Cget_DisabledWorkTypes_003Ed__39 : IEnumerable<WorkTypeDef>, IEnumerable, IEnumerator<WorkTypeDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private WorkTypeDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public RoyalTitleDef _003C_003E4__this;

		private List<WorkTypeDef> _003Clist_003E5__2;

		private int _003Ci_003E5__3;

		WorkTypeDef IEnumerator<WorkTypeDef>.Current
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
		public _003Cget_DisabledWorkTypes_003Ed__39(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Clist_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			RoyalTitleDef royalTitleDef = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_0078;
			}
			_003C_003E1__state = -1;
			_003Clist_003E5__2 = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			_003Ci_003E5__3 = 0;
			goto IL_0088;
			IL_0078:
			_003Ci_003E5__3++;
			goto IL_0088;
			IL_0088:
			if (_003Ci_003E5__3 < _003Clist_003E5__2.Count)
			{
				if ((royalTitleDef.disabledWorkTags & _003Clist_003E5__2[_003Ci_003E5__3].workTags) != 0)
				{
					_003C_003E2__current = _003Clist_003E5__2[_003Ci_003E5__3];
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0078;
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
		IEnumerator<WorkTypeDef> IEnumerable<WorkTypeDef>.GetEnumerator()
		{
			_003Cget_DisabledWorkTypes_003Ed__39 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_DisabledWorkTypes_003Ed__39(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<WorkTypeDef>)this).GetEnumerator();
		}
	}

	public int seniority;

	public int favorCost;

	[MustTranslate]
	public string labelFemale;

	public int changeHeirQuestPoints = -1;

	public float commonality = 1f;

	public WorkTags disabledWorkTags;

	public Type inheritanceWorkerOverrideClass;

	public QualityCategory requiredMinimumApparelQuality;

	public List<ApparelRequirement> requiredApparel;

	public List<RoyalTitlePermitDef> permits;

	public ExpectationDef minExpectation;

	public List<JoyKindDef> disabledJoyKinds;

	[NoTranslate]
	public List<string> tags;

	public List<ThingDefCountClass> rewards;

	public bool suppressIdleAlert;

	public bool canBeInherited;

	public bool allowDignifiedMeditationFocus = true;

	public int permitPointsAwarded;

	public Type awardWorkerClass = typeof(RoyalTitleAwardWorker);

	public ThoughtDef awardThought;

	public ThoughtDef lostThought;

	public List<RoomRequirement> throneRoomRequirements;

	public List<RoomRequirement> bedroomRequirements;

	public float recruitmentResistanceOffset;

	public RoyalTitleFoodRequirement foodRequirement;

	public RoyalTitleDef replaceOnRecruited;

	public float decreeMtbDays = -1f;

	public float decreeMinIntervalDays = 2f;

	public float decreeMentalBreakCommonality;

	public List<string> decreeTags;

	public List<AbilityDef> grantedAbilities = new List<AbilityDef>();

	public IntRange speechCooldown;

	public int maxPsylinkLevel;

	[Unsaved(false)]
	private List<ThingDef> satisfyingMealsCached;

	[Unsaved(false)]
	private List<ThingDef> satisfyingMealsNoDrugsCached;

	private RoyalTitleAwardWorker awardWorker;

	private RoyalTitleInheritanceWorker inheritanceWorkerOverride;

	public bool Awardable => favorCost > 0;

	public IEnumerable<WorkTypeDef> DisabledWorkTypes
	{
		[IteratorStateMachine(typeof(_003Cget_DisabledWorkTypes_003Ed__39))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_DisabledWorkTypes_003Ed__39(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public RoyalTitleInheritanceWorker InheritanceWorkerOverride
	{
		get
		{
			if (inheritanceWorkerOverride == null && inheritanceWorkerOverrideClass != null)
			{
				inheritanceWorkerOverride = (RoyalTitleInheritanceWorker)Activator.CreateInstance(inheritanceWorkerOverrideClass);
			}
			return inheritanceWorkerOverride;
		}
	}

	public float MinThroneRoomImpressiveness
	{
		get
		{
			if (throneRoomRequirements.NullOrEmpty())
			{
				return 0f;
			}
			RoomRequirement_Impressiveness roomRequirement_Impressiveness = throneRoomRequirements.OfType<RoomRequirement_Impressiveness>().FirstOrDefault();
			if (roomRequirement_Impressiveness == null)
			{
				return 0f;
			}
			return roomRequirement_Impressiveness.impressiveness;
		}
	}

	public RoyalTitleAwardWorker AwardWorker
	{
		get
		{
			if (awardWorker == null)
			{
				awardWorker = (RoyalTitleAwardWorker)Activator.CreateInstance(awardWorkerClass);
				awardWorker.def = this;
			}
			return awardWorker;
		}
	}

	public RoyalTitleInheritanceWorker GetInheritanceWorker(Faction faction)
	{
		if (inheritanceWorkerOverrideClass == null)
		{
			return faction.def.RoyalTitleInheritanceWorker;
		}
		return InheritanceWorkerOverride;
	}

	public string GetLabelFor(Pawn p)
	{
		if (p == null)
		{
			return GetLabelForBothGenders();
		}
		return GetLabelFor(p.gender);
	}

	public string GetLabelFor(Gender g)
	{
		if (g == Gender.Female)
		{
			if (string.IsNullOrEmpty(labelFemale))
			{
				return label;
			}
			return labelFemale;
		}
		return label;
	}

	public string GetLabelForBothGenders()
	{
		if (!string.IsNullOrEmpty(labelFemale))
		{
			return label + " / " + labelFemale;
		}
		return label;
	}

	public string GetLabelCapForBothGenders()
	{
		if (!string.IsNullOrEmpty(labelFemale))
		{
			return LabelCap + " / " + labelFemale.CapitalizeFirst();
		}
		return LabelCap;
	}

	public string GetLabelCapFor(Pawn p)
	{
		return GetLabelFor(p).CapitalizeFirst(this);
	}

	public IEnumerable<RoomRequirement> GetBedroomRequirements(Pawn p)
	{
		if (p.story.traits.HasTrait(TraitDefOf.Ascetic))
		{
			return null;
		}
		return bedroomRequirements;
	}

	public string GetReportText(Faction faction)
	{
		return description + "\n\n" + RoyalTitleUtility.GetTitleProgressionInfo(faction);
	}

	public bool JoyKindDisabled(JoyKindDef joyKind)
	{
		if (disabledJoyKinds == null)
		{
			return false;
		}
		return disabledJoyKinds.Contains(joyKind);
	}

	private bool HasSameRoomRequirement(RoomRequirement otherReq, List<RoomRequirement> list)
	{
		if (list == null)
		{
			return false;
		}
		foreach (RoomRequirement item in list)
		{
			if (item.SameOrSubsetOf(otherReq))
			{
				return true;
			}
		}
		return false;
	}

	public bool HasSameThroneroomRequirement(RoomRequirement otherReq)
	{
		return HasSameRoomRequirement(otherReq, throneRoomRequirements);
	}

	public bool HasSameBedroomRequirement(RoomRequirement otherReq)
	{
		return HasSameRoomRequirement(otherReq, bedroomRequirements);
	}

	public int MaxAllowedPsylinkLevel(FactionDef faction)
	{
		int result = 0;
		for (int i = 0; i < faction.royalImplantRules.Count; i++)
		{
			RoyalImplantRule royalImplantRule = faction.royalImplantRules[i];
			if (royalImplantRule.implantHediff == HediffDefOf.PsychicAmplifier && royalImplantRule.minTitle.Awardable && royalImplantRule.minTitle.seniority <= seniority)
			{
				result = royalImplantRule.maxLevel;
			}
		}
		return result;
	}

	public IEnumerable<ThingDef> SatisfyingMeals(bool includeDrugs = true)
	{
		if (includeDrugs)
		{
			if (satisfyingMealsCached == null)
			{
				satisfyingMealsCached = (from t in DefDatabase<ThingDef>.AllDefsListForReading
					where foodRequirement.Acceptable(t)
					orderby t.ingestible.preferability descending
					select t).ToList();
			}
		}
		else if (satisfyingMealsNoDrugsCached == null)
		{
			satisfyingMealsNoDrugsCached = (from t in DefDatabase<ThingDef>.AllDefsListForReading
				where foodRequirement.Acceptable(t) && !t.IsDrug
				orderby t.ingestible.preferability descending
				select t).ToList();
		}
		if (!includeDrugs)
		{
			return satisfyingMealsNoDrugsCached;
		}
		return satisfyingMealsCached;
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__61))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__61(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__req = req
		};
	}

	[IteratorStateMachine(typeof(_003CRequiredApparelListForGender_003Ed__62))]
	private IEnumerable<string> RequiredApparelListForGender(Gender g, Pawn forPawn = null)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRequiredApparelListForGender_003Ed__62(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__g = g,
			_003C_003E3__forPawn = forPawn
		};
	}

	public IEnumerable<DefHyperlink> GetHyperlinks(Faction faction)
	{
		IEnumerable<DefHyperlink> enumerable = descriptionHyperlinks;
		return enumerable ?? (from t in faction.def.RoyalTitlesAllInSeniorityOrderForReading
			where t != this
			select new DefHyperlink(t, faction));
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__64))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__64(-2)
		{
			_003C_003E4__this = this
		};
	}
}
