using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class AbilityDef : Def
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass94_0
	{
		public StatModifier statBase;

		internal bool _003CConfigErrors_003Eb__0(StatModifier st)
		{
			return st.stat == statBase.stat;
		}
	}

	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__94 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public AbilityDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

		private List<StatModifier>.Enumerator _003C_003E7__wrap2;

		private int _003Ci_003E5__4;

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
		public _003CConfigErrors_003Ed__94(int _003C_003E1__state)
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
			case 5:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			case -5:
			case 6:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = default(List<StatModifier>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				AbilityDef abilityDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)abilityDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0082;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0082;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00ca;
				case 3:
					_003C_003E1__state = -1;
					goto IL_00f2;
				case 4:
					_003C_003E1__state = -1;
					goto IL_011f;
				case 5:
					_003C_003E1__state = -4;
					goto IL_01bc;
				case 6:
					{
						_003C_003E1__state = -5;
						goto IL_023e;
					}
					IL_0082:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (abilityDef.abilityClass == null)
					{
						_003C_003E2__current = "abilityClass is null";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00ca;
					IL_01bc:
					while (_003C_003E7__wrap2.MoveNext())
					{
						_003C_003Ec__DisplayClass94_0 CS_0024_003C_003E8__locals2 = new _003C_003Ec__DisplayClass94_0
						{
							statBase = _003C_003E7__wrap2.Current
						};
						if (abilityDef.statBases.Count((StatModifier st) => st.stat == CS_0024_003C_003E8__locals2.statBase.stat) > 1)
						{
							_003C_003E2__current = "defines the stat base " + CS_0024_003C_003E8__locals2.statBase.stat?.ToString() + " more than once.";
							_003C_003E1__state = 5;
							return true;
						}
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = default(List<StatModifier>.Enumerator);
					goto IL_01de;
					IL_01de:
					_003Ci_003E5__4 = 0;
					goto IL_026a;
					IL_00ca:
					if (abilityDef.verbProperties == null)
					{
						_003C_003E2__current = "verbProperties are null";
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_00f2;
					IL_026a:
					if (_003Ci_003E5__4 < abilityDef.comps.Count)
					{
						_003C_003E7__wrap1 = abilityDef.comps[_003Ci_003E5__4].ConfigErrors(abilityDef).GetEnumerator();
						_003C_003E1__state = -5;
						goto IL_023e;
					}
					return false;
					IL_00f2:
					if (abilityDef.label.NullOrEmpty())
					{
						_003C_003E2__current = "no label";
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_011f;
					IL_023e:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 6;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap1 = null;
					_003Ci_003E5__4++;
					goto IL_026a;
					IL_011f:
					if (abilityDef.statBases != null)
					{
						_003C_003E7__wrap2 = abilityDef.statBases.GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_01bc;
					}
					goto IL_01de;
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
			_003CConfigErrors_003Ed__94 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__94(0)
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
	private sealed class _003CSpecialDisplayStats_003Ed__90 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public AbilityDef _003C_003E4__this;

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
		public _003CSpecialDisplayStats_003Ed__90(int _003C_003E1__state)
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
			AbilityDef abilityDef = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				if (abilityDef.cachedTargets == null)
				{
					abilityDef.cachedTargets = new List<string>();
					if (abilityDef.verbProperties.targetParams.canTargetPawns && abilityDef.verbProperties.targetParams.canTargetSelf)
					{
						abilityDef.cachedTargets.Add("TargetSelf".Translate());
					}
					if (abilityDef.verbProperties.targetParams.canTargetLocations)
					{
						abilityDef.cachedTargets.Add("TargetGround".Translate());
					}
					if (abilityDef.verbProperties.targetParams.canTargetPawns && abilityDef.verbProperties.targetParams.canTargetHumans)
					{
						abilityDef.cachedTargets.Add("TargetHuman".Translate());
					}
					if (abilityDef.verbProperties.targetParams.canTargetPawns && abilityDef.verbProperties.targetParams.canTargetAnimals)
					{
						abilityDef.cachedTargets.Add("TargetAnimal".Translate());
					}
				}
				int num2 = abilityDef.comps.OfType<CompProperties_AbilityEffect>().Sum((CompProperties_AbilityEffect e) => e.goodwillImpact);
				if (num2 != 0)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Ability, StatDefOf.Ability_GoodwillImpact, num2, req, ToStringNumberSense.Undefined, null);
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_01a0;
			}
			case 1:
				_003C_003E1__state = -1;
				goto IL_01a0;
			case 2:
				_003C_003E1__state = -1;
				goto IL_01ed;
			case 3:
				_003C_003E1__state = -1;
				if (abilityDef.verbProperties.range > 0f)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Ability, StatDefOf.Ability_Range, abilityDef.verbProperties.range, req, ToStringNumberSense.Undefined, null);
					_003C_003E1__state = 4;
					return true;
				}
				goto IL_0281;
			case 4:
				_003C_003E1__state = -1;
				goto IL_0281;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Ability, "AbilityRequiresLOS".Translate(), abilityDef.verbProperties.requireLineOfSight ? "Yes".Translate() : "No".Translate(), "", 1000);
				_003C_003E1__state = 6;
				return true;
			case 6:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_01a0:
				if (abilityDef.IsPsycast && abilityDef.level != 0)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Ability, StatDefOf.Ability_RequiredPsylink, abilityDef.level, req, ToStringNumberSense.Undefined, null);
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_01ed;
				IL_01ed:
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Ability, StatDefOf.Ability_CastingTime, abilityDef.verbProperties.warmupTime, req, ToStringNumberSense.Undefined, null);
				_003C_003E1__state = 3;
				return true;
				IL_0281:
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Ability, "Target".Translate(), abilityDef.cachedTargets.ToCommaList().CapitalizeFirst(), "AbilityTargetDesc".Translate(), 1001);
				_003C_003E1__state = 5;
				return true;
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
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__90 _003CSpecialDisplayStats_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CSpecialDisplayStats_003Ed__ = this;
			}
			else
			{
				_003CSpecialDisplayStats_003Ed__ = new _003CSpecialDisplayStats_003Ed__90(0)
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
	private sealed class _003CStatSummary_003Ed__86 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public AbilityDef _003C_003E4__this;

		private Pawn forPawn;

		public Pawn _003C_003E3__forPawn;

		private int _003Ci_003E5__2;

		private IEnumerator<string> _003C_003E7__wrap2;

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
		public _003CStatSummary_003Ed__86(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 8)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				AbilityDef abilityDef = _003C_003E4__this;
				float num2;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					string text = null;
					foreach (AbilityCompProperties comp in abilityDef.comps)
					{
						if (comp.OverridesPsyfocusCost)
						{
							text = comp.PsyfocusCostExplanation;
							break;
						}
					}
					if (text == null)
					{
						if (abilityDef.PsyfocusCost > float.Epsilon)
						{
							_003C_003E2__current = "AbilityPsyfocusCost".Translate() + ": " + abilityDef.PsyfocusCost.ToStringPercent();
							_003C_003E1__state = 1;
							return true;
						}
						goto IL_00fe;
					}
					_003C_003E2__current = text;
					_003C_003E1__state = 2;
					return true;
				}
				case 1:
					_003C_003E1__state = -1;
					goto IL_00fe;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00fe;
				case 3:
					_003C_003E1__state = -1;
					goto IL_0153;
				case 4:
					_003C_003E1__state = -1;
					goto IL_01c2;
				case 5:
					_003C_003E1__state = -1;
					goto IL_0236;
				case 6:
					_003C_003E1__state = -1;
					goto IL_02c1;
				case 7:
					_003C_003E1__state = -1;
					goto IL_0316;
				case 8:
					{
						_003C_003E1__state = -3;
						goto IL_037d;
					}
					IL_037d:
					if (_003C_003E7__wrap2.MoveNext())
					{
						string current2 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 8;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap2 = null;
					_003Ci_003E5__2++;
					goto IL_03a9;
					IL_0236:
					num2 = abilityDef.EffectDuration(forPawn);
					if (num2 > float.Epsilon)
					{
						int num3 = num2.SecondsToTicks();
						_003C_003E2__current = "AbilityDuration".Translate() + ": " + ((num3 >= 2500) ? num3.ToStringTicksToPeriod() : (num2.ToString() + "LetterSecond".Translate()));
						_003C_003E1__state = 6;
						return true;
					}
					goto IL_02c1;
					IL_03a9:
					if (_003Ci_003E5__2 >= abilityDef.comps.Count)
					{
						break;
					}
					_003C_003E7__wrap2 = abilityDef.comps[_003Ci_003E5__2].ExtraStatSummary().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_037d;
					IL_00fe:
					if (abilityDef.EntropyGain > float.Epsilon)
					{
						_003C_003E2__current = string.Concat("AbilityEntropyGain".Translate() + ": ", abilityDef.EntropyGain.ToString());
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_0153;
					IL_0153:
					if (abilityDef.verbProperties.warmupTime > float.Epsilon)
					{
						_003C_003E2__current = string.Concat("AbilityCastingTime".Translate() + ": ", abilityDef.verbProperties.warmupTime.ToString()) + "LetterSecond".Translate();
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_01c2;
					IL_0316:
					if (abilityDef.comps == null)
					{
						break;
					}
					_003Ci_003E5__2 = 0;
					goto IL_03a9;
					IL_01c2:
					if (abilityDef.cooldownTicksRange.min == abilityDef.cooldownTicksRange.max && abilityDef.cooldownTicksRange.min > 0)
					{
						_003C_003E2__current = "StatsReport_Cooldown".Translate() + ": " + abilityDef.cooldownTicksRange.min.ToStringTicksToPeriod(allowSeconds: true, shortForm: false, canUseDecimals: true, allowYears: false);
						_003C_003E1__state = 5;
						return true;
					}
					goto IL_0236;
					IL_02c1:
					if (abilityDef.HasAreaOfEffect)
					{
						_003C_003E2__current = string.Concat("AbilityEffectRadius".Translate() + ": ", Mathf.Ceil(abilityDef.EffectRadius).ToString());
						_003C_003E1__state = 7;
						return true;
					}
					goto IL_0316;
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

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CStatSummary_003Ed__86 _003CStatSummary_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CStatSummary_003Ed__ = this;
			}
			else
			{
				_003CStatSummary_003Ed__ = new _003CStatSummary_003Ed__86(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CStatSummary_003Ed__.forPawn = _003C_003E3__forPawn;
			return _003CStatSummary_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public Type abilityClass = typeof(Ability);

	public Type gizmoClass = typeof(Command_Ability);

	public List<AbilityCompProperties> comps = new List<AbilityCompProperties>();

	public AbilityCategoryDef category;

	public int displayOrder;

	public List<StatModifier> statBases;

	public VerbProperties verbProperties;

	public KeyBindingDef hotKey;

	public JobDef jobDef;

	public ThingDef warmupMote;

	public EffecterDef warmupEffecter;

	public FleckDef emittedFleck;

	public int emissionInterval;

	public string warmupMoteSocialSymbol;

	public SoundDef warmupStartSound;

	public SoundDef warmupSound;

	public SoundDef warmupPreEndSound;

	public float warmupPreEndSoundSeconds;

	public Vector3 moteDrawOffset;

	public float moteOffsetAmountTowardsTarget;

	public bool canUseAoeToGetTargets = true;

	public bool useAverageTargetPositionForWarmupEffecter;

	public bool targetRequired = true;

	public bool targetWorldCell;

	public bool showGizmoOnWorldView;

	public bool aiCanUse;

	public bool ai_SearchAOEForTargets;

	public bool ai_IsOffensive = true;

	public bool ai_IsIncendiary = true;

	public bool groupAbility;

	public int level;

	public IntRange cooldownTicksRange;

	public bool cooldownPerCharge;

	public bool hasExternallyHandledCooldown;

	public int charges = -1;

	public AbilityGroupDef groupDef;

	public bool overrideGroupCooldown;

	public List<MemeDef> requiredMemes;

	public bool sendLetterOnCooldownComplete;

	public bool sendMessageOnCooldownComplete;

	public bool displayGizmoWhileUndrafted;

	public bool disableGizmoWhileUndrafted = true;

	public bool writeCombatLog;

	public bool stunTargetWhileCasting;

	public bool showPsycastEffects = true;

	public bool showCastingProgressBar;

	public float detectionChanceOverride = -1f;

	public float uiOrder;

	public bool waitForJobEnd;

	public bool showWhenDrafted = true;

	public bool showOnCharacterCard = true;

	public bool hostile = true;

	public bool casterMustBeCapableOfViolence = true;

	[MustTranslate]
	public string confirmationDialogText;

	[NoTranslate]
	public string iconPath;

	public Texture2D uiIcon = BaseContent.BadTex;

	private string cachedTooltip;

	private Pawn cachedTooltipPawn;

	private List<string> cachedTargets;

	private int requiredPsyfocusBandCached = -1;

	private bool? anyCompOverridesPsyfocusCost;

	private FloatRange psyfocusCostRange = new FloatRange(-1f, -1f);

	private string psyfocusCostPercent;

	private string psyfocusCostPercentMax;

	private Texture2D warmupMoteSocialSymbolCached;

	public float EntropyGain => statBases.GetStatValueFromList(StatDefOf.Ability_EntropyGain, 0f);

	public float PsyfocusCost => statBases.GetStatValueFromList(StatDefOf.Ability_PsyfocusCost, 0f);

	public float EffectRadius => statBases.GetStatValueFromList(StatDefOf.Ability_EffectRadius, 0f);

	public bool HasAreaOfEffect => EffectRadius > float.Epsilon;

	public float DetectionChance
	{
		get
		{
			if (!(detectionChanceOverride >= 0f))
			{
				return this.GetStatValueAbstract(StatDefOf.Ability_DetectChancePerEntropy);
			}
			return detectionChanceOverride;
		}
	}

	public bool IsPsycast => typeof(Psycast).IsAssignableFrom(abilityClass);

	public string PsyfocusCostPercent
	{
		get
		{
			if (psyfocusCostPercent.NullOrEmpty())
			{
				psyfocusCostPercent = PsyfocusCost.ToStringPercent();
			}
			return psyfocusCostPercent;
		}
	}

	public string PsyfocusCostPercentMax
	{
		get
		{
			if (psyfocusCostPercentMax.NullOrEmpty())
			{
				psyfocusCostPercentMax = PsyfocusCostRange.max.ToStringPercent();
			}
			return psyfocusCostPercentMax;
		}
	}

	public int RequiredPsyfocusBand
	{
		get
		{
			if (requiredPsyfocusBandCached == -1)
			{
				requiredPsyfocusBandCached = Pawn_PsychicEntropyTracker.MaxAbilityLevelPerPsyfocusBand.Count - 1;
				for (int i = 0; i < Pawn_PsychicEntropyTracker.MaxAbilityLevelPerPsyfocusBand.Count; i++)
				{
					int num = Pawn_PsychicEntropyTracker.MaxAbilityLevelPerPsyfocusBand[i];
					if (level <= num)
					{
						requiredPsyfocusBandCached = i;
						break;
					}
				}
			}
			return requiredPsyfocusBandCached;
		}
	}

	public bool AnyCompOverridesPsyfocusCost
	{
		get
		{
			if (!anyCompOverridesPsyfocusCost.HasValue)
			{
				anyCompOverridesPsyfocusCost = false;
				if (comps != null)
				{
					foreach (AbilityCompProperties comp in comps)
					{
						if (comp.OverridesPsyfocusCost)
						{
							anyCompOverridesPsyfocusCost = true;
							break;
						}
					}
				}
			}
			return anyCompOverridesPsyfocusCost.Value;
		}
	}

	public FloatRange PsyfocusCostRange
	{
		get
		{
			if (psyfocusCostRange.min < 0f)
			{
				if (!AnyCompOverridesPsyfocusCost)
				{
					psyfocusCostRange = new FloatRange(PsyfocusCost, PsyfocusCost);
				}
				else
				{
					foreach (AbilityCompProperties comp in comps)
					{
						if (comp.OverridesPsyfocusCost)
						{
							psyfocusCostRange = comp.PsyfocusCostRange;
							break;
						}
					}
				}
			}
			return psyfocusCostRange;
		}
	}

	public Texture2D WarmupMoteSocialSymbol
	{
		get
		{
			if (!warmupMoteSocialSymbol.NullOrEmpty() && warmupMoteSocialSymbolCached == null)
			{
				warmupMoteSocialSymbolCached = ContentFinder<Texture2D>.Get(warmupMoteSocialSymbol);
			}
			return warmupMoteSocialSymbolCached;
		}
	}

	[IteratorStateMachine(typeof(_003CStatSummary_003Ed__86))]
	public IEnumerable<string> StatSummary(Pawn forPawn = null)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CStatSummary_003Ed__86(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__forPawn = forPawn
		};
	}

	public float EffectDuration(Pawn forPawn = null)
	{
		return this.GetStatValueAbstract(StatDefOf.Ability_Duration, forPawn);
	}

	public override void PostLoad()
	{
		if (!string.IsNullOrEmpty(iconPath))
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				uiIcon = ContentFinder<Texture2D>.Get(iconPath);
			});
		}
	}

	public string GetTooltip(Pawn pawn = null)
	{
		if (cachedTooltip == null || cachedTooltipPawn != pawn)
		{
			cachedTooltip = LabelCap.Colorize(ColoredText.TipSectionTitleColor) + ((level > 0) ? string.Concat("\n" + "Level".Translate().CapitalizeFirst() + " ", level.ToString()) : "") + "\n\n" + description;
			cachedTooltipPawn = pawn;
			string text = StatSummary(pawn).ToLineList();
			if (!text.NullOrEmpty())
			{
				cachedTooltip = cachedTooltip + "\n\n" + text;
			}
		}
		if (pawn != null && ModsConfig.RoyaltyActive && IsPsycast && level > 0)
		{
			Faction first = Faction.GetMinTitleForImplantAllFactions(HediffDefOf.PsychicAmplifier).First;
			if (first != null)
			{
				RoyalTitleDef minTitleForImplant = first.GetMinTitleForImplant(HediffDefOf.PsychicAmplifier, level);
				RoyalTitleDef currentTitle = pawn.royalty.GetCurrentTitle(first);
				if (minTitleForImplant != null && (currentTitle == null || currentTitle.seniority < minTitleForImplant.seniority) && DetectionChance > 0f)
				{
					return cachedTooltip + "\n\n" + ((string)"PsycastIsIllegal".Translate(pawn.Named("PAWN"), minTitleForImplant.GetLabelCapFor(pawn).Named("TITLE"))).Colorize(ColoredText.WarningColor);
				}
			}
		}
		return cachedTooltip;
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__90))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__90(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__req = req
		};
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__94))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__94(-2)
		{
			_003C_003E4__this = this
		};
	}
}
