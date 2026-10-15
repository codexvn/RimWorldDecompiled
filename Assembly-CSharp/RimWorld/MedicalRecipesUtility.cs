using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld;

public class MedicalRecipesUtility
{
	[CompilerGenerated]
	private sealed class _003C_003CGetMedicalStatsFromRecipeDefs_003Eg__PartAndAllPartsUnder_007C6_0_003Ed : IEnumerable<BodyPartRecord>, IEnumerable, IEnumerator<BodyPartRecord>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private BodyPartRecord _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private List<BodyPartDef> bpDefs;

		public List<BodyPartDef> _003C_003E3__bpDefs;

		private List<BodyPartDef>.Enumerator _003C_003E7__wrap1;

		private IEnumerator<BodyPartRecord> _003C_003E7__wrap2;

		BodyPartRecord IEnumerator<BodyPartRecord>.Current
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
		public _003C_003CGetMedicalStatsFromRecipeDefs_003Eg__PartAndAllPartsUnder_007C6_0_003Ed(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<BodyPartDef>.Enumerator);
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			bool result;
			try
			{
				int num = _003C_003E1__state;
				if (num == 0)
				{
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = bpDefs.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00be;
				}
				if (num == 1)
				{
					_003C_003E1__state = -4;
					goto IL_00a4;
				}
				result = false;
				goto end_IL_0000;
				IL_00a4:
				if (!_003C_003E7__wrap2.MoveNext())
				{
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = null;
					goto IL_00be;
				}
				BodyPartRecord current = _003C_003E7__wrap2.Current;
				_003C_003E2__current = current;
				_003C_003E1__state = 1;
				result = true;
				goto end_IL_0000;
				IL_00be:
				if (_003C_003E7__wrap1.MoveNext())
				{
					BodyPartDef current2 = _003C_003E7__wrap1.Current;
					BodyPartRecord bodyPartRecord = BodyDefOf.Human.GetPartsWithDef(current2).FirstOrDefault();
					if (bodyPartRecord != null)
					{
						_003C_003E7__wrap2 = bodyPartRecord.GetPartAndAllChildParts().GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_00a4;
					}
					result = false;
					_003C_003Em__Finally1();
				}
				else
				{
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<BodyPartDef>.Enumerator);
					result = false;
				}
				end_IL_0000:;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
			return result;
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
		IEnumerator<BodyPartRecord> IEnumerable<BodyPartRecord>.GetEnumerator()
		{
			_003C_003CGetMedicalStatsFromRecipeDefs_003Eg__PartAndAllPartsUnder_007C6_0_003Ed _003C_003CGetMedicalStatsFromRecipeDefs_003Eg__PartAndAllPartsUnder_007C6_0_003Ed;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003C_003CGetMedicalStatsFromRecipeDefs_003Eg__PartAndAllPartsUnder_007C6_0_003Ed = this;
			}
			else
			{
				_003C_003CGetMedicalStatsFromRecipeDefs_003Eg__PartAndAllPartsUnder_007C6_0_003Ed = new _003C_003CGetMedicalStatsFromRecipeDefs_003Eg__PartAndAllPartsUnder_007C6_0_003Ed(0);
			}
			_003C_003CGetMedicalStatsFromRecipeDefs_003Eg__PartAndAllPartsUnder_007C6_0_003Ed.bpDefs = _003C_003E3__bpDefs;
			return _003C_003CGetMedicalStatsFromRecipeDefs_003Eg__PartAndAllPartsUnder_007C6_0_003Ed;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<BodyPartRecord>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass6_0
	{
		public HediffDef diff;

		internal bool _003CGetMedicalStatsFromRecipeDefs_003Eb__4(ThoughtDef x)
		{
			return x.hediff == diff;
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetFixedPartsToApplyOn_003Ed__5 : IEnumerable<BodyPartRecord>, IEnumerable, IEnumerator<BodyPartRecord>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private BodyPartRecord _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private RecipeDef recipe;

		public RecipeDef _003C_003E3__recipe;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private Func<BodyPartRecord, bool> validator;

		public Func<BodyPartRecord, bool> _003C_003E3__validator;

		private int _003Ci_003E5__2;

		private BodyPartDef _003Cpart_003E5__3;

		private List<BodyPartRecord> _003CbpList_003E5__4;

		private int _003Cj_003E5__5;

		private BodyPartGroupDef _003Cgroup_003E5__6;

		BodyPartRecord IEnumerator<BodyPartRecord>.Current
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
		public _003CGetFixedPartsToApplyOn_003Ed__5(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cpart_003E5__3 = null;
			_003CbpList_003E5__4 = null;
			_003Cgroup_003E5__6 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003Ci_003E5__2 = 0;
				goto IL_00fc;
			case 1:
				_003C_003E1__state = -1;
				goto IL_00bb;
			case 2:
				{
					_003C_003E1__state = -1;
					goto IL_01bd;
				}
				IL_00fc:
				if (_003Ci_003E5__2 < recipe.appliedOnFixedBodyParts.Count)
				{
					_003Cpart_003E5__3 = recipe.appliedOnFixedBodyParts[_003Ci_003E5__2];
					_003CbpList_003E5__4 = pawn.RaceProps.body.AllParts;
					_003Cj_003E5__5 = 0;
					goto IL_00cb;
				}
				_003Ci_003E5__2 = 0;
				goto IL_01fe;
				IL_01bd:
				_003Cj_003E5__5++;
				goto IL_01cd;
				IL_01fe:
				if (_003Ci_003E5__2 < recipe.appliedOnFixedBodyPartGroups.Count)
				{
					_003Cgroup_003E5__6 = recipe.appliedOnFixedBodyPartGroups[_003Ci_003E5__2];
					_003CbpList_003E5__4 = pawn.RaceProps.body.AllParts;
					_003Cj_003E5__5 = 0;
					goto IL_01cd;
				}
				return false;
				IL_00cb:
				if (_003Cj_003E5__5 < _003CbpList_003E5__4.Count)
				{
					BodyPartRecord bodyPartRecord = _003CbpList_003E5__4[_003Cj_003E5__5];
					if (bodyPartRecord.def == _003Cpart_003E5__3 && (validator == null || validator(bodyPartRecord)))
					{
						_003C_003E2__current = bodyPartRecord;
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_00bb;
				}
				_003Cpart_003E5__3 = null;
				_003CbpList_003E5__4 = null;
				num = _003Ci_003E5__2 + 1;
				_003Ci_003E5__2 = num;
				goto IL_00fc;
				IL_01cd:
				if (_003Cj_003E5__5 < _003CbpList_003E5__4.Count)
				{
					BodyPartRecord bodyPartRecord2 = _003CbpList_003E5__4[_003Cj_003E5__5];
					if (bodyPartRecord2.groups != null && bodyPartRecord2.groups.Contains(_003Cgroup_003E5__6) && (validator == null || validator(bodyPartRecord2)))
					{
						_003C_003E2__current = bodyPartRecord2;
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_01bd;
				}
				_003Cgroup_003E5__6 = null;
				_003CbpList_003E5__4 = null;
				num = _003Ci_003E5__2 + 1;
				_003Ci_003E5__2 = num;
				goto IL_01fe;
				IL_00bb:
				_003Cj_003E5__5++;
				goto IL_00cb;
			}
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
		IEnumerator<BodyPartRecord> IEnumerable<BodyPartRecord>.GetEnumerator()
		{
			_003CGetFixedPartsToApplyOn_003Ed__5 _003CGetFixedPartsToApplyOn_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetFixedPartsToApplyOn_003Ed__ = this;
			}
			else
			{
				_003CGetFixedPartsToApplyOn_003Ed__ = new _003CGetFixedPartsToApplyOn_003Ed__5(0);
			}
			_003CGetFixedPartsToApplyOn_003Ed__.recipe = _003C_003E3__recipe;
			_003CGetFixedPartsToApplyOn_003Ed__.pawn = _003C_003E3__pawn;
			_003CGetFixedPartsToApplyOn_003Ed__.validator = _003C_003E3__validator;
			return _003CGetFixedPartsToApplyOn_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<BodyPartRecord>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetMedicalStatsFromRecipeDefs_003Ed__6 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IEnumerable<RecipeDef> recipes;

		public IEnumerable<RecipeDef> _003C_003E3__recipes;

		private _003C_003Ec__DisplayClass6_0 _003C_003E8__1;

		private bool _003Cmultiple_003E5__2;

		private IEnumerator<RecipeDef> _003C_003E7__wrap2;

		private RecipeDef _003Cdef_003E5__4;

		private string _003CextraLabelPart_003E5__5;

		private IEnumerator<StatDrawEntry> _003C_003E7__wrap5;

		private VerbProperties _003Cverb_003E5__7;

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
		public _003CGetMedicalStatsFromRecipeDefs_003Ed__6(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || (uint)(num - 1) <= 9u)
			{
				try
				{
					if (num == -4 || num == 5)
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
			_003C_003E8__1 = null;
			_003C_003E7__wrap2 = null;
			_003Cdef_003E5__4 = null;
			_003CextraLabelPart_003E5__5 = null;
			_003C_003E7__wrap5 = null;
			_003Cverb_003E5__7 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				HediffCompProperties_VerbGiver hediffCompProperties_VerbGiver;
				ThoughtDef thoughtDef;
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003Cmultiple_003E5__2 = recipes.Count() >= 2;
					_003C_003E7__wrap2 = recipes.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_079a;
				case 1:
					_003C_003E1__state = -3;
					if (_003C_003E8__1.diff.addedPartProps != null && _003C_003E8__1.diff.addedPartProps.solid)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "Stat_Thing_ReplacesParts".Translate(), (from x in _003CGetMedicalStatsFromRecipeDefs_003Eg__PartAndAllPartsUnder_007C6_0(_003Cdef_003E5__4.appliedOnFixedBodyParts)
							select x.def.label).Distinct().ToCommaList().CapitalizeFirst(), "Stat_Thing_ReplacesParts_Desc".Translate(), 3980);
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_02f2;
				case 2:
					_003C_003E1__state = -3;
					goto IL_02f2;
				case 3:
					_003C_003E1__state = -3;
					goto IL_02f2;
				case 4:
					_003C_003E1__state = -3;
					goto IL_0378;
				case 5:
					_003C_003E1__state = -4;
					goto IL_03d9;
				case 6:
					_003C_003E1__state = -3;
					if (_003Cverb_003E5__7.defaultProjectile.projectile.damageDef.armorCategory != null)
					{
						float armorPenetration = _003Cverb_003E5__7.defaultProjectile.projectile.GetArmorPenetration();
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "ArmorPenetration".Translate() + _003CextraLabelPart_003E5__5, armorPenetration.ToStringPercent(), "ArmorPenetrationExplanation".Translate(), 5400);
						_003C_003E1__state = 7;
						return true;
					}
					goto IL_0621;
				case 7:
					_003C_003E1__state = -3;
					goto IL_0621;
				case 8:
					_003C_003E1__state = -3;
					goto IL_0621;
				case 9:
					_003C_003E1__state = -3;
					goto IL_06e1;
				case 10:
					{
						_003C_003E1__state = -3;
						goto IL_0785;
					}
					IL_03d9:
					if (_003C_003E7__wrap5.MoveNext())
					{
						StatDrawEntry current = _003C_003E7__wrap5.Current;
						current.category = StatCategoryDefOf.Implant;
						_003C_003E2__current = current;
						_003C_003E1__state = 5;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap5 = null;
					hediffCompProperties_VerbGiver = _003C_003E8__1.diff.CompProps<HediffCompProperties_VerbGiver>();
					if (hediffCompProperties_VerbGiver != null)
					{
						if (!hediffCompProperties_VerbGiver.verbs.NullOrEmpty())
						{
							_003Cverb_003E5__7 = hediffCompProperties_VerbGiver.verbs[0];
							if (!_003Cverb_003E5__7.IsMeleeAttack)
							{
								if (_003Cverb_003E5__7.defaultProjectile != null)
								{
									StringBuilder stringBuilder = new StringBuilder();
									stringBuilder.AppendLine("Stat_Thing_Damage_Desc".Translate());
									stringBuilder.AppendLine();
									int damageAmount = _003Cverb_003E5__7.defaultProjectile.projectile.GetDamageAmount(null, stringBuilder);
									_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "Damage".Translate() + _003CextraLabelPart_003E5__5, damageAmount.ToString(), stringBuilder.ToString(), 5500);
									_003C_003E1__state = 6;
									return true;
								}
							}
							else
							{
								int meleeDamageBaseAmount = _003Cverb_003E5__7.meleeDamageBaseAmount;
								if (_003Cverb_003E5__7.meleeDamageDef.armorCategory != null)
								{
									float num = _003Cverb_003E5__7.meleeArmorPenetrationBase;
									if (num < 0f)
									{
										num = (float)meleeDamageBaseAmount * 0.015f;
									}
									_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Weapon_Melee, "ArmorPenetration".Translate() + _003CextraLabelPart_003E5__5, num.ToStringPercent(), "ArmorPenetrationExplanation".Translate(), 5400);
									_003C_003E1__state = 8;
									return true;
								}
							}
							goto IL_0621;
						}
						if (!hediffCompProperties_VerbGiver.tools.NullOrEmpty())
						{
							Tool tool = hediffCompProperties_VerbGiver.tools[0];
							if (ThingUtility.PrimaryMeleeWeaponDamageType(hediffCompProperties_VerbGiver.tools).armorCategory != null)
							{
								float num2 = tool.armorPenetration;
								if (num2 < 0f)
								{
									num2 = tool.power * 0.015f;
								}
								_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Weapon_Melee, "ArmorPenetration".Translate() + _003CextraLabelPart_003E5__5, num2.ToStringPercent(), "ArmorPenetrationExplanation".Translate(), 5400);
								_003C_003E1__state = 9;
								return true;
							}
						}
					}
					goto IL_06e1;
					IL_0378:
					_003C_003E7__wrap5 = _003C_003E8__1.diff.SpecialDisplayStats(StatRequest.ForEmpty()).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_03d9;
					IL_0621:
					_003Cverb_003E5__7 = null;
					goto IL_06e1;
					IL_06e1:
					thoughtDef = DefDatabase<ThoughtDef>.AllDefs.FirstOrDefault((ThoughtDef x) => x.hediff == _003C_003E8__1.diff);
					if (thoughtDef != null && thoughtDef.stages != null && thoughtDef.stages.Any())
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "MoodChange".Translate() + _003CextraLabelPart_003E5__5, thoughtDef.stages.First().baseMoodEffect.ToStringByStyle(ToStringStyle.Integer, ToStringNumberSense.Offset), "Stat_Thing_MoodChange_Desc".Translate(), 3500);
						_003C_003E1__state = 10;
						return true;
					}
					goto IL_0785;
					IL_079a:
					do
					{
						if (_003C_003E7__wrap2.MoveNext())
						{
							_003Cdef_003E5__4 = _003C_003E7__wrap2.Current;
							_003C_003E8__1 = new _003C_003Ec__DisplayClass6_0();
							_003CextraLabelPart_003E5__5 = (_003Cmultiple_003E5__2 ? (" (" + _003Cdef_003E5__4.addsHediff.label + ")") : "");
							_003C_003E8__1.diff = _003Cdef_003E5__4.addsHediff;
							continue;
						}
						_003C_003Em__Finally1();
						_003C_003E7__wrap2 = null;
						return false;
					}
					while (_003C_003E8__1.diff == null);
					if (!_003Cdef_003E5__4.appliedOnFixedBodyParts.NullOrEmpty())
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "Stat_Thing_InstallSites".Translate(), _003Cdef_003E5__4.appliedOnFixedBodyParts.Select((BodyPartDef x) => x.label).ToCommaList().CapitalizeFirst(), "Stat_Thing_InstallSites_Desc".Translate(), 3990);
						_003C_003E1__state = 1;
						return true;
					}
					if (!_003Cdef_003E5__4.appliedOnFixedBodyPartGroups.NullOrEmpty())
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "Stat_Thing_InstallSites".Translate(), _003Cdef_003E5__4.appliedOnFixedBodyPartGroups.Select((BodyPartGroupDef x) => x.label).ToCommaList().CapitalizeFirst(), "Stat_Thing_InstallSites_Desc".Translate(), 3990);
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_02f2;
					IL_0785:
					_003C_003E8__1 = null;
					_003CextraLabelPart_003E5__5 = null;
					_003Cdef_003E5__4 = null;
					goto IL_079a;
					IL_02f2:
					if (_003C_003E8__1.diff.addedPartProps != null)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "BodyPartEfficiency".Translate() + _003CextraLabelPart_003E5__5, _003C_003E8__1.diff.addedPartProps.partEfficiency.ToStringByStyle(ToStringStyle.PercentZero), "Stat_Thing_BodyPartEfficiency_Desc".Translate(), 4000);
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_0378;
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
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
			if (_003C_003E7__wrap5 != null)
			{
				_003C_003E7__wrap5.Dispose();
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
			_003CGetMedicalStatsFromRecipeDefs_003Ed__6 _003CGetMedicalStatsFromRecipeDefs_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetMedicalStatsFromRecipeDefs_003Ed__ = this;
			}
			else
			{
				_003CGetMedicalStatsFromRecipeDefs_003Ed__ = new _003CGetMedicalStatsFromRecipeDefs_003Ed__6(0);
			}
			_003CGetMedicalStatsFromRecipeDefs_003Ed__.recipes = _003C_003E3__recipes;
			return _003CGetMedicalStatsFromRecipeDefs_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	public static bool IsCleanAndDroppable(Pawn pawn, BodyPartRecord part)
	{
		if (pawn.Dead)
		{
			return false;
		}
		if (pawn.RaceProps.Animal)
		{
			return false;
		}
		if (pawn.IsMutant && !pawn.mutant.Def.partsCleanAndDroppable)
		{
			return false;
		}
		if (part.def.spawnThingOnRemoved != null)
		{
			return IsClean(pawn, part);
		}
		return false;
	}

	public static bool IsClean(Pawn pawn, BodyPartRecord part)
	{
		if (pawn.Dead)
		{
			return false;
		}
		return !pawn.health.hediffSet.hediffs.Where((Hediff x) => x.Part == part).Any();
	}

	public static void RestorePartAndSpawnAllPreviousParts(Pawn pawn, BodyPartRecord part, IntVec3 pos, Map map)
	{
		SpawnNaturalPartIfClean(pawn, part, pos, map);
		SpawnThingsFromHediffs(pawn, part, pos, map);
		pawn.health.RestorePart(part);
	}

	public static Thing SpawnNaturalPartIfClean(Pawn pawn, BodyPartRecord part, IntVec3 pos, Map map)
	{
		if (IsCleanAndDroppable(pawn, part))
		{
			return GenSpawn.Spawn(part.def.spawnThingOnRemoved, pos, map);
		}
		return null;
	}

	public static void SpawnThingsFromHediffs(Pawn pawn, BodyPartRecord part, IntVec3 pos, Map map)
	{
		if (!pawn.health.hediffSet.GetNotMissingParts().Contains(part))
		{
			return;
		}
		foreach (Hediff item in pawn.health.hediffSet.hediffs.Where((Hediff x) => x.Part == part))
		{
			if (item.def.spawnThingOnRemoved != null)
			{
				GenSpawn.Spawn(item.def.spawnThingOnRemoved, pos, map);
			}
		}
		for (int i = 0; i < part.parts.Count; i++)
		{
			SpawnThingsFromHediffs(pawn, part.parts[i], pos, map);
		}
	}

	[IteratorStateMachine(typeof(_003CGetFixedPartsToApplyOn_003Ed__5))]
	public static IEnumerable<BodyPartRecord> GetFixedPartsToApplyOn(RecipeDef recipe, Pawn pawn, Func<BodyPartRecord, bool> validator = null)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetFixedPartsToApplyOn_003Ed__5(-2)
		{
			_003C_003E3__recipe = recipe,
			_003C_003E3__pawn = pawn,
			_003C_003E3__validator = validator
		};
	}

	[IteratorStateMachine(typeof(_003CGetMedicalStatsFromRecipeDefs_003Ed__6))]
	public static IEnumerable<StatDrawEntry> GetMedicalStatsFromRecipeDefs(IEnumerable<RecipeDef> recipes)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetMedicalStatsFromRecipeDefs_003Ed__6(-2)
		{
			_003C_003E3__recipes = recipes
		};
	}

	[IteratorStateMachine(typeof(_003C_003CGetMedicalStatsFromRecipeDefs_003Eg__PartAndAllPartsUnder_007C6_0_003Ed))]
	[CompilerGenerated]
	internal static IEnumerable<BodyPartRecord> _003CGetMedicalStatsFromRecipeDefs_003Eg__PartAndAllPartsUnder_007C6_0(List<BodyPartDef> bpDefs)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003C_003CGetMedicalStatsFromRecipeDefs_003Eg__PartAndAllPartsUnder_007C6_0_003Ed(-2)
		{
			_003C_003E3__bpDefs = bpDefs
		};
	}
}
