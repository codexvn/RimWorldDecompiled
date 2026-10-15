using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class RitualOutcomeEffectDef : Def
{
	public class ExtraOutcomeChanceDescription
	{
		[MustTranslate]
		public string description;

		public Func<float, float> qualityToValue;
	}

	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__38 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public RitualOutcomeEffectDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

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
		public _003CConfigErrors_003Ed__38(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				RitualOutcomeEffectDef ritualOutcomeEffectDef = _003C_003E4__this;
				float num2;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)ritualOutcomeEffectDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0074;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0074;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0074:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					num2 = 0f;
					foreach (RitualOutcomePossibility outcomeChance in ritualOutcomeEffectDef.outcomeChances)
					{
						num2 += outcomeChance.chance;
					}
					if (ritualOutcomeEffectDef.outcomeChances.Any() && Mathf.Abs(num2 - 1f) > 0.0001f)
					{
						_003C_003E2__current = "Sum of outcome chances doesn't add up to 1.0. total=" + num2;
						_003C_003E1__state = 2;
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
			_003CConfigErrors_003Ed__38 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__38(0)
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

	public Type workerClass;

	public float startingQuality;

	public float minQuality;

	public float maxQuality = 1f;

	public bool givesDevelopmentPoints = true;

	public ThoughtDef memoryDef;

	public List<RitualOutcomePossibility> outcomeChances = new List<RitualOutcomePossibility>();

	public SimpleCurve honorFromQuality;

	[MustTranslate]
	public List<string> extraPredictedOutcomeDescriptions;

	[MustTranslate]
	public List<string> extraInfoLines;

	public List<ExtraOutcomeChanceDescription> extraOutcomeDescriptions;

	public EffecterDef effecter;

	public FleckDef fleckDef;

	public int flecksPerCell;

	public FloatRange fleckRotationRange = new FloatRange(0f, 360f);

	public FloatRange fleckScaleRange = FloatRange.One;

	public FloatRange fleckVelocityAngle = FloatRange.Zero;

	public FloatRange fleckVelocitySpeed = FloatRange.Zero;

	public ThingDef filthDefToSpawn;

	public IntRange filthCountToSpawn = IntRange.Zero;

	public List<RitualOutcomeComp> comps;

	public bool warnOnLowQuality = true;

	public bool allowAttachableOutcome = true;

	public bool allowOutcomeWithNoAttendance;

	private float minMoodCached = float.MaxValue;

	private float maxMoodCached = float.MinValue;

	private float moodDurationCached = -1f;

	private RitualOutcomePossibility bestOutcomeCached;

	private RitualOutcomePossibility worstOutcomeCached;

	public string Description
	{
		get
		{
			if (minMoodCached == float.MaxValue && !outcomeChances.NullOrEmpty())
			{
				foreach (RitualOutcomePossibility outcomeChance in outcomeChances)
				{
					if (outcomeChance.memory != null)
					{
						float baseMoodEffect = outcomeChance.memory.stages[0].baseMoodEffect;
						if (baseMoodEffect > maxMoodCached)
						{
							maxMoodCached = baseMoodEffect;
						}
						if (baseMoodEffect < minMoodCached)
						{
							minMoodCached = baseMoodEffect;
						}
						moodDurationCached = outcomeChance.memory.durationDays;
					}
				}
			}
			return description.Formatted(minMoodCached.Named("MINMOOD"), maxMoodCached.ToStringWithSign().Named("MAXMOOD"), moodDurationCached.Named("MOODDAYS"));
		}
	}

	public RitualOutcomePossibility BestOutcome
	{
		get
		{
			if (bestOutcomeCached == null)
			{
				bestOutcomeCached = outcomeChances.MaxBy((RitualOutcomePossibility o) => o.positivityIndex);
			}
			return bestOutcomeCached;
		}
	}

	public RitualOutcomePossibility WorstOutcome
	{
		get
		{
			if (worstOutcomeCached == null)
			{
				worstOutcomeCached = outcomeChances.MinBy((RitualOutcomePossibility o) => o.positivityIndex);
			}
			return worstOutcomeCached;
		}
	}

	public RitualOutcomeEffectWorker GetInstance()
	{
		return (RitualOutcomeEffectWorker)Activator.CreateInstance(workerClass, this);
	}

	public string OutcomeMoodBreakdown(RitualOutcomePossibility outcome)
	{
		if (outcome.memory != null && outcome.memory.stages[0].baseMoodEffect != 0f)
		{
			return "RitualOutcomeExtraDesc_Mood".Translate(outcome.memory.stages[0].baseMoodEffect.ToStringWithSign(), outcome.memory.durationDays);
		}
		return "";
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__38))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__38(-2)
		{
			_003C_003E4__this = this
		};
	}
}
