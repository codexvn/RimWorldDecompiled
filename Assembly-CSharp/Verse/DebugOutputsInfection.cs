using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using LudeonTK;
using RimWorld;
using UnityEngine;

namespace Verse;

public static class DebugOutputsInfection
{
	private enum InfectionLuck
	{
		Bad,
		Normal,
		Good
	}

	private struct InfectionSimRow
	{
		public HediffDef illness;

		public int skill;

		public ThingDef medicine;

		public float deathChance;

		public float recoveryTimeDays;

		public float medicineUsed;
	}

	[CompilerGenerated]
	private sealed class _003CInfectionSimulatorWorker_003Ed__6 : IEnumerable<object>, IEnumerable, IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private int _003Ctrials_003E5__2;

		private List<Pawn> _003Cdoctors_003E5__3;

		private List<int> _003CtestSkill_003E5__4;

		private List<ThingDef> _003CtestMedicine_003E5__5;

		private PawnGenerationRequest _003Cpawngen_003E5__6;

		private int _003CoriginalTicks_003E5__7;

		private List<InfectionSimRow> _003Cresults_003E5__8;

		private int _003CtotalTests_003E5__9;

		private int _003CcurrentTest_003E5__10;

		private IEnumerator<HediffDef> _003C_003E7__wrap10;

		private HediffDef _003Chediff_003E5__12;

		private List<ThingDef>.Enumerator _003C_003E7__wrap12;

		private ThingDef _003Cmeds_003E5__14;

		private List<int>.Enumerator _003C_003E7__wrap14;

		private InfectionSimRow _003Cresult_003E5__16;

		private Pawn _003Cdoctor_003E5__17;

		private int _003Ci_003E5__18;

		private Pawn _003Cpatient_003E5__19;

		private int _003CstartTicks_003E5__20;

		private Hediff _003CactiveHediff_003E5__21;

		object IEnumerator<object>.Current
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
		public _003CInfectionSimulatorWorker_003Ed__6(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -5) <= 2u || (uint)(num - 1) <= 1u)
			{
				try
				{
					if ((uint)(num - -5) <= 1u || (uint)(num - 1) <= 1u)
					{
						try
						{
							if (num == -5 || (uint)(num - 1) <= 1u)
							{
								try
								{
								}
								finally
								{
									_003C_003Em__Finally3();
								}
							}
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
			_003Cdoctors_003E5__3 = null;
			_003CtestSkill_003E5__4 = null;
			_003CtestMedicine_003E5__5 = null;
			_003Cpawngen_003E5__6 = default(PawnGenerationRequest);
			_003Cresults_003E5__8 = null;
			_003C_003E7__wrap10 = null;
			_003Chediff_003E5__12 = null;
			_003C_003E7__wrap12 = default(List<ThingDef>.Enumerator);
			_003Cmeds_003E5__14 = null;
			_003C_003E7__wrap14 = default(List<int>.Enumerator);
			_003Cresult_003E5__16 = default(InfectionSimRow);
			_003Cdoctor_003E5__17 = null;
			_003Cpatient_003E5__19 = null;
			_003CactiveHediff_003E5__21 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num;
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003Ctrials_003E5__2 = 2;
					_003Cdoctors_003E5__3 = GenerateDoctorArray();
					_003CtestSkill_003E5__4 = new List<int> { 4, 10, 16 };
					_003CtestMedicine_003E5__5 = new List<ThingDef>
					{
						null,
						ThingDefOf.MedicineHerbal,
						ThingDefOf.MedicineIndustrial,
						ThingDefOf.MedicineUltratech
					};
					_003Cpawngen_003E5__6 = new PawnGenerationRequest(PawnKindDefOf.Colonist, Faction.OfPlayer, PawnGenerationContext.NonPlayer, null, forceGenerateNewPawn: false, allowDead: false, allowDowned: false, canGeneratePawnRelations: true, mustBeCapableOfViolence: false, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: false, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, forceNoIdeo: false, forceNoBackstory: false, forbidAnyTitle: false, forceDead: false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null);
					_003CoriginalTicks_003E5__7 = Find.TickManager.TicksGame;
					_003Cresults_003E5__8 = new List<InfectionSimRow>();
					_003CtotalTests_003E5__9 = InfectionList().Count() * _003CtestMedicine_003E5__5.Count() * _003CtestSkill_003E5__4.Count() * _003Ctrials_003E5__2;
					_003CcurrentTest_003E5__10 = 0;
					_003C_003E7__wrap10 = InfectionList().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_05d0;
				case 1:
					_003C_003E1__state = -5;
					goto IL_03dd;
				case 2:
					{
						_003C_003E1__state = -5;
						_003Cpatient_003E5__19 = null;
						_003CactiveHediff_003E5__21 = null;
						num = _003Ci_003E5__18 + 1;
						_003Ci_003E5__18 = num;
						goto IL_04fb;
					}
					IL_05d0:
					if (_003C_003E7__wrap10.MoveNext())
					{
						_003Chediff_003E5__12 = _003C_003E7__wrap10.Current;
						_003C_003E7__wrap12 = _003CtestMedicine_003E5__5.GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_05a7;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap10 = null;
					DebugTables.MakeTablesDialog(_003Cresults_003E5__8, new TableDataGetter<InfectionSimRow>("defName", (InfectionSimRow isr) => isr.illness.defName), new TableDataGetter<InfectionSimRow>("meds", (InfectionSimRow isr) => (isr.medicine == null) ? "(none)" : isr.medicine.defName), new TableDataGetter<InfectionSimRow>("skill", (InfectionSimRow isr) => isr.skill.ToString()), new TableDataGetter<InfectionSimRow>("death chance", (InfectionSimRow isr) => isr.deathChance.ToStringPercent()), new TableDataGetter<InfectionSimRow>("recovery time (days)", (InfectionSimRow isr) => isr.recoveryTimeDays.ToString("F1")), new TableDataGetter<InfectionSimRow>("medicine used", (InfectionSimRow isr) => isr.medicineUsed.ToString()));
					Find.TickManager.DebugSetTicksGame(_003CoriginalTicks_003E5__7);
					return false;
					IL_057e:
					if (_003C_003E7__wrap14.MoveNext())
					{
						int current = _003C_003E7__wrap14.Current;
						_003Cresult_003E5__16 = new InfectionSimRow
						{
							illness = _003Chediff_003E5__12,
							skill = current,
							medicine = _003Cmeds_003E5__14
						};
						_003Cdoctor_003E5__17 = _003Cdoctors_003E5__3[current];
						_003Ci_003E5__18 = 0;
						goto IL_04fb;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap14 = default(List<int>.Enumerator);
					_003Cmeds_003E5__14 = null;
					goto IL_05a7;
					IL_03dd:
					while (!_003Cpatient_003E5__19.Dead && _003Cpatient_003E5__19.health.hediffSet.HasHediff(_003Cresult_003E5__16.illness))
					{
						float maxQuality = _003Cmeds_003E5__14?.GetStatValueAbstract(StatDefOf.MedicalQualityMax) ?? 0.7f;
						if (_003CactiveHediff_003E5__21.TendableNow())
						{
							_003CactiveHediff_003E5__21.Tended(TendUtility.CalculateBaseTendQuality(_003Cdoctor_003E5__17, _003Cpatient_003E5__19, _003Cmeds_003E5__14), maxQuality);
							_003Cresult_003E5__16.medicineUsed += 1f;
						}
						foreach (Hediff item in _003Cpatient_003E5__19.health.hediffSet.GetHediffsTendable())
						{
							item.Tended(TendUtility.CalculateBaseTendQuality(_003Cdoctor_003E5__17, _003Cpatient_003E5__19, _003Cmeds_003E5__14), maxQuality);
						}
						Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 1);
						_003Cpatient_003E5__19.health.HealthTickInterval(1);
						if (Find.TickManager.TicksGame % 900 == 0)
						{
							_003C_003E2__current = null;
							_003C_003E1__state = 1;
							return true;
						}
					}
					if (_003Cpatient_003E5__19.Dead)
					{
						_003Cresult_003E5__16.deathChance += 1f;
					}
					else
					{
						_003Cresult_003E5__16.recoveryTimeDays += (Find.TickManager.TicksGame - _003CstartTicks_003E5__20).TicksToDays();
					}
					num = _003CcurrentTest_003E5__10 + 1;
					_003CcurrentTest_003E5__10 = num;
					LongEventHandler.SetCurrentEventText($"Simulating ({_003CcurrentTest_003E5__10}/{_003CtotalTests_003E5__9})");
					_003C_003E2__current = null;
					_003C_003E1__state = 2;
					return true;
					IL_05a7:
					if (_003C_003E7__wrap12.MoveNext())
					{
						_003Cmeds_003E5__14 = _003C_003E7__wrap12.Current;
						_003C_003E7__wrap14 = _003CtestSkill_003E5__4.GetEnumerator();
						_003C_003E1__state = -5;
						goto IL_057e;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap12 = default(List<ThingDef>.Enumerator);
					_003Chediff_003E5__12 = null;
					goto IL_05d0;
					IL_04fb:
					if (_003Ci_003E5__18 < _003Ctrials_003E5__2)
					{
						_003Cpatient_003E5__19 = PawnGenerator.GeneratePawn(_003Cpawngen_003E5__6);
						_003CstartTicks_003E5__20 = Find.TickManager.TicksGame;
						_003Cpatient_003E5__19.health.AddHediff(_003Cresult_003E5__16.illness, null, null);
						_003CactiveHediff_003E5__21 = _003Cpatient_003E5__19.health.hediffSet.GetFirstHediffOfDef(_003Cresult_003E5__16.illness);
						goto IL_03dd;
					}
					_003Cresult_003E5__16.recoveryTimeDays /= (float)_003Ctrials_003E5__2 - _003Cresult_003E5__16.deathChance;
					_003Cresult_003E5__16.deathChance /= _003Ctrials_003E5__2;
					_003Cresult_003E5__16.medicineUsed /= _003Ctrials_003E5__2;
					_003Cresults_003E5__8.Add(_003Cresult_003E5__16);
					_003Cresult_003E5__16 = default(InfectionSimRow);
					_003Cdoctor_003E5__17 = null;
					goto IL_057e;
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
			if (_003C_003E7__wrap10 != null)
			{
				_003C_003E7__wrap10.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
			((IDisposable)_003C_003E7__wrap12/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -4;
			((IDisposable)_003C_003E7__wrap14/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<object> IEnumerable<object>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CInfectionSimulatorWorker_003Ed__6(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<object>)this).GetEnumerator();
		}
	}

	private static List<Pawn> GenerateDoctorArray()
	{
		PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.Colonist, Faction.OfPlayer, PawnGenerationContext.NonPlayer, null, forceGenerateNewPawn: false, allowDead: false, allowDowned: false, canGeneratePawnRelations: true, mustBeCapableOfViolence: false, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: false, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, 0f, 0f, null, 1f, (Pawn p) => !p.WorkTypeIsDisabled(WorkTypeDefOf.Doctor) && p.health.hediffSet.hediffs.Count == 0, null, null, null, null, null, null, null, null, null, null, null, forceNoIdeo: false, forceNoBackstory: false, forbidAnyTitle: false, forceDead: false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null);
		List<Pawn> list = new List<Pawn>();
		for (int i = 0; i <= 20; i++)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(request);
			pawn.skills.GetSkill(SkillDefOf.Medicine).Level = i;
			list.Add(pawn);
		}
		return list;
	}

	private static IEnumerable<HediffDef> InfectionList()
	{
		return DefDatabase<HediffDef>.AllDefs.Where((HediffDef hediff) => hediff.tendable && hediff.HasComp(typeof(HediffComp_TendDuration)) && hediff.HasComp(typeof(HediffComp_Immunizable)) && hediff.lethalSeverity > 0f);
	}

	[DebugOutput]
	public static void Infections()
	{
		Func<InfectionLuck, float> ilc = delegate(InfectionLuck il)
		{
			float result = 1f;
			if (il == InfectionLuck.Bad)
			{
				result = 0.8f;
			}
			if (il == InfectionLuck.Good)
			{
				result = 1.2f;
			}
			return result;
		};
		Func<Func<InfectionLuck, float>, string> stringizeWithLuck = (Func<InfectionLuck, float> func) => $"{func(InfectionLuck.Bad):F2} / {func(InfectionLuck.Good):F2}";
		Func<HediffDef, InfectionLuck, float> baseImmunityIncrease = (HediffDef d, InfectionLuck il) => d.CompProps<HediffCompProperties_Immunizable>().immunityPerDaySick * ilc(il);
		Func<HediffDef, float, float> tendedSeverityIncrease = (HediffDef d, float tend) => baseSeverityIncrease(d) + d.CompProps<HediffCompProperties_TendDuration>().severityPerDayTended * tend;
		Func<HediffDef, InfectionLuck, bool, float> immunityIncrease = delegate(HediffDef d, InfectionLuck il, bool bedridden)
		{
			float b = (isAnimal(d) ? 1f : ThingDefOf.Bed.GetStatValueAbstract(StatDefOf.ImmunityGainSpeedFactor));
			float num = Mathf.Lerp(1f, b, bedridden ? 1f : 0.3f) * StatDefOf.ImmunityGainSpeed.GetStatPart<StatPart_Resting>().factor;
			return baseImmunityIncrease(d, il) * num;
		};
		Func<HediffDef, InfectionLuck, float, float> immunityOnLethality = (HediffDef d, InfectionLuck il, float tend) => (tendedSeverityIncrease(d, tend) <= 0f) ? float.PositiveInfinity : (d.lethalSeverity / tendedSeverityIncrease(d, tend) * immunityIncrease(d, il, arg3: true));
		List<TableDataGetter<HediffDef>> list = new List<TableDataGetter<HediffDef>>();
		list.Add(new TableDataGetter<HediffDef>("defName", (HediffDef d) => d.defName + (d.stages.Any((HediffStage stage) => stage.capMods.Any((PawnCapacityModifier cap) => cap.capacity == PawnCapacityDefOf.BloodFiltration)) ? " (inaccurate)" : "")));
		list.Add(new TableDataGetter<HediffDef>("lethal\nseverity", (HediffDef d) => d.lethalSeverity.ToString("F2")));
		list.Add(new TableDataGetter<HediffDef>("base\nseverity\nincrease", (HediffDef d) => baseSeverityIncrease(d).ToString("F2")));
		list.Add(new TableDataGetter<HediffDef>("base\nimmunity\nincrease", (HediffDef d) => stringizeWithLuck((InfectionLuck il) => baseImmunityIncrease(d, il))));
		List<Pawn> source = GenerateDoctorArray();
		for (float tendquality = 0f; tendquality <= 1.01f; tendquality += 0.1f)
		{
			tendquality = Mathf.Clamp01(tendquality);
			Pawn arg = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, null) >= Mathf.Clamp01(tendquality - 0.25f));
			Pawn arg2 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineHerbal) >= Mathf.Clamp01(tendquality - 0.25f));
			Pawn arg3 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineIndustrial) >= Mathf.Clamp01(tendquality - 0.25f));
			Pawn arg4 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineUltratech) >= Mathf.Clamp01(tendquality - 0.25f));
			Pawn arg5 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, null) >= tendquality);
			Pawn arg6 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineHerbal) >= tendquality);
			Pawn arg7 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineIndustrial) >= tendquality);
			Pawn arg8 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineUltratech) >= tendquality);
			Pawn arg9 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, null) >= Mathf.Clamp01(tendquality + 0.25f));
			Pawn arg10 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineHerbal) >= Mathf.Clamp01(tendquality + 0.25f));
			Pawn arg11 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineIndustrial) >= Mathf.Clamp01(tendquality + 0.25f));
			Pawn arg12 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineUltratech) >= Mathf.Clamp01(tendquality + 0.25f));
			Func<Pawn, Pawn, Pawn, string> obj = delegate(Pawn low, Pawn exp, Pawn high)
			{
				string arg13 = ((low != null) ? low.skills.GetSkill(SkillDefOf.Medicine).Level.ToString() : "X");
				string arg14 = ((exp != null) ? exp.skills.GetSkill(SkillDefOf.Medicine).Level.ToString() : "X");
				string arg15 = ((high != null) ? high.skills.GetSkill(SkillDefOf.Medicine).Level.ToString() : "X");
				return $"{arg13}-{arg14}-{arg15}";
			};
			string text = obj(arg, arg5, arg9);
			string text2 = obj(arg2, arg6, arg10);
			string text3 = obj(arg3, arg7, arg11);
			string text4 = obj(arg4, arg8, arg12);
			float tq = tendquality;
			list.Add(new TableDataGetter<HediffDef>($"survival chance at\ntend quality {tq.ToStringPercent()}\n\ndoc skill needed:\nno meds:  {text}\nherbal:  {text2}\nnormal:  {text3}\nglitter:  {text4}", delegate(HediffDef d)
			{
				float num2 = immunityOnLethality(d, InfectionLuck.Bad, tq);
				float num3 = immunityOnLethality(d, InfectionLuck.Good, tq);
				return (num2 == float.PositiveInfinity) ? float.PositiveInfinity.ToString() : Mathf.Clamp01((num3 - 1f) / (num3 - num2)).ToStringPercent();
			}));
		}
		DebugTables.MakeTablesDialog(InfectionList(), list.ToArray());
		static float baseSeverityIncrease(HediffDef d)
		{
			return d.CompProps<HediffCompProperties_Immunizable>().severityPerDayNotImmune;
		}
		static bool isAnimal(HediffDef d)
		{
			return d.defName.Contains("Animal");
		}
	}

	[DebugOutput]
	public static void InfectionSimulator()
	{
		LongEventHandler.QueueLongEvent(InfectionSimulatorWorker(), "Simulating . . .");
	}

	[IteratorStateMachine(typeof(_003CInfectionSimulatorWorker_003Ed__6))]
	private static IEnumerable InfectionSimulatorWorker()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CInfectionSimulatorWorker_003Ed__6(-2);
	}
}
