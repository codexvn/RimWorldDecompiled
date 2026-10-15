using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using LudeonTK;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld;

public static class AgeInjuryUtility
{
	[CompilerGenerated]
	private sealed class _003CRandomHediffsToGainOnBirthday_003Ed__4 : IEnumerable<HediffGiver_Birthday>, IEnumerable, IEnumerator<HediffGiver_Birthday>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private HediffGiver_Birthday _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private ThingDef raceDef;

		public ThingDef _003C_003E3__raceDef;

		private float age;

		public float _003C_003E3__age;

		private float cancerFactor;

		public float _003C_003E3__cancerFactor;

		private List<HediffGiverSetDef> _003Csets_003E5__2;

		private int _003Ci_003E5__3;

		private List<HediffGiver> _003Cgivers_003E5__4;

		private int _003Cj_003E5__5;

		HediffGiver_Birthday IEnumerator<HediffGiver_Birthday>.Current
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
		public _003CRandomHediffsToGainOnBirthday_003Ed__4(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Csets_003E5__2 = null;
			_003Cgivers_003E5__4 = null;
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
				goto IL_00eb;
			}
			_003C_003E1__state = -1;
			_003Csets_003E5__2 = raceDef.race.hediffGiverSets;
			if (_003Csets_003E5__2 == null)
			{
				return false;
			}
			_003Ci_003E5__3 = 0;
			goto IL_0128;
			IL_00eb:
			_003Cj_003E5__5++;
			goto IL_00fb;
			IL_0118:
			_003Ci_003E5__3++;
			goto IL_0128;
			IL_00fb:
			if (_003Cj_003E5__5 < _003Cgivers_003E5__4.Count)
			{
				if (_003Cgivers_003E5__4[_003Cj_003E5__5] is HediffGiver_Birthday hediffGiver_Birthday)
				{
					float num2 = age / raceDef.race.lifeExpectancy;
					if (hediffGiver_Birthday.hediff == HediffDefOf.Carcinoma)
					{
						num2 *= cancerFactor;
					}
					if (Rand.Value < hediffGiver_Birthday.ageFractionChanceCurve.Evaluate(num2))
					{
						_003C_003E2__current = hediffGiver_Birthday;
						_003C_003E1__state = 1;
						return true;
					}
				}
				goto IL_00eb;
			}
			_003Cgivers_003E5__4 = null;
			goto IL_0118;
			IL_0128:
			if (_003Ci_003E5__3 < _003Csets_003E5__2.Count)
			{
				_003Cgivers_003E5__4 = _003Csets_003E5__2[_003Ci_003E5__3].hediffGivers;
				if (_003Cgivers_003E5__4 != null)
				{
					_003Cj_003E5__5 = 0;
					goto IL_00fb;
				}
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
		IEnumerator<HediffGiver_Birthday> IEnumerable<HediffGiver_Birthday>.GetEnumerator()
		{
			_003CRandomHediffsToGainOnBirthday_003Ed__4 _003CRandomHediffsToGainOnBirthday_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CRandomHediffsToGainOnBirthday_003Ed__ = this;
			}
			else
			{
				_003CRandomHediffsToGainOnBirthday_003Ed__ = new _003CRandomHediffsToGainOnBirthday_003Ed__4(0);
			}
			_003CRandomHediffsToGainOnBirthday_003Ed__.raceDef = _003C_003E3__raceDef;
			_003CRandomHediffsToGainOnBirthday_003Ed__.cancerFactor = _003C_003E3__cancerFactor;
			_003CRandomHediffsToGainOnBirthday_003Ed__.age = _003C_003E3__age;
			return _003CRandomHediffsToGainOnBirthday_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<HediffGiver_Birthday>)this).GetEnumerator();
		}
	}

	private const int MaxPermanentInjuryAge = 100;

	private static List<Thing> emptyIngredientsList = new List<Thing>();

	private static readonly SimpleCurve AmputationChanceFromAgeCurve = new SimpleCurve
	{
		new CurvePoint(13f, 0f),
		new CurvePoint(25f, 1f)
	};

	public static IEnumerable<HediffGiver_Birthday> RandomHediffsToGainOnBirthday(Pawn pawn, float age)
	{
		return RandomHediffsToGainOnBirthday(pawn.def, ModsConfig.BiotechActive ? pawn.GetStatValue(StatDefOf.CancerRate) : 1f, age);
	}

	[IteratorStateMachine(typeof(_003CRandomHediffsToGainOnBirthday_003Ed__4))]
	private static IEnumerable<HediffGiver_Birthday> RandomHediffsToGainOnBirthday(ThingDef raceDef, float cancerFactor, float age)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRandomHediffsToGainOnBirthday_003Ed__4(-2)
		{
			_003C_003E3__raceDef = raceDef,
			_003C_003E3__cancerFactor = cancerFactor,
			_003C_003E3__age = age
		};
	}

	public static void GenerateRandomOldAgeInjuries(Pawn pawn, bool tryNotToKillPawn)
	{
		if (!pawn.kindDef.allowOldAgeInjuries)
		{
			return;
		}
		float num = (pawn.RaceProps.IsMechanoid ? 2500f : pawn.RaceProps.lifeExpectancy);
		float num2 = num / 8f;
		float b = num * 1.5f;
		float chance = (pawn.RaceProps.Humanlike ? 0.15f : 0.03f);
		int num3 = 0;
		for (float num4 = num2; num4 < Mathf.Min(pawn.ageTracker.AgeBiologicalYears, b); num4 += num2)
		{
			if (Rand.Chance(chance))
			{
				num3++;
			}
		}
		bool flag = !pawn.RaceProps.Humanlike || Rand.Chance(AmputationChanceFromAgeCurve.Evaluate(pawn.ageTracker.AgeBiologicalYearsFloat));
		for (int i = 0; i < num3; i++)
		{
			IEnumerable<BodyPartRecord> source = from x in pawn.health.hediffSet.GetNotMissingParts()
				where x.depth == BodyPartDepth.Outside && (x.def.permanentInjuryChanceFactor != 0f || x.def.pawnGeneratorCanAmputate) && !pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(x)
				select x;
			if (!source.Any())
			{
				continue;
			}
			BodyPartRecord bodyPartRecord = source.RandomElementByWeight((BodyPartRecord x) => x.coverageAbs);
			HediffDef hediffDefFromDamage = HealthUtility.GetHediffDefFromDamage(HealthUtility.RandomPermanentInjuryDamageType(bodyPartRecord.def.frostbiteVulnerability > 0f && pawn.RaceProps.ToolUser), pawn, bodyPartRecord);
			if (bodyPartRecord.def.pawnGeneratorCanAmputate && flag && Rand.Chance(0.3f))
			{
				Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, pawn);
				hediff_MissingPart.lastInjury = hediffDefFromDamage;
				hediff_MissingPart.Part = bodyPartRecord;
				hediff_MissingPart.IsFresh = false;
				if (!tryNotToKillPawn || !pawn.health.WouldDieAfterAddingHediff(hediff_MissingPart))
				{
					pawn.health.AddHediff(hediff_MissingPart, bodyPartRecord, null);
					if (pawn.RaceProps.Humanlike && bodyPartRecord.def == BodyPartDefOf.Leg && Rand.Chance(0.5f))
					{
						RecipeDefOf.InstallPegLeg.Worker.ApplyOnPawn(pawn, bodyPartRecord, null, emptyIngredientsList, null);
					}
				}
			}
			else if (bodyPartRecord.def.permanentInjuryChanceFactor > 0f && hediffDefFromDamage.HasComp(typeof(HediffComp_GetsPermanent)))
			{
				Hediff_Injury hediff_Injury = (Hediff_Injury)HediffMaker.MakeHediff(hediffDefFromDamage, pawn);
				hediff_Injury.Severity = Rand.RangeInclusive(2, 6);
				hediff_Injury.TryGetComp<HediffComp_GetsPermanent>().IsPermanent = true;
				hediff_Injury.Part = bodyPartRecord;
				if (!tryNotToKillPawn || !pawn.health.WouldDieAfterAddingHediff(hediff_Injury))
				{
					pawn.health.AddHediff(hediff_Injury, bodyPartRecord, null);
				}
			}
		}
		for (int j = 1; j < pawn.ageTracker.AgeBiologicalYears; j++)
		{
			foreach (HediffGiver_Birthday item in RandomHediffsToGainOnBirthday(pawn, j))
			{
				item.TryApplyAndSimulateSeverityChange(pawn, j, tryNotToKillPawn);
				if (pawn.Dead)
				{
					break;
				}
			}
			if (pawn.Dead)
			{
				break;
			}
		}
	}

	[DebugOutput]
	public static void PermanentInjuryCalculations()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("=======Theoretical injuries=========");
		for (int i = 0; i < 10; i++)
		{
			stringBuilder.AppendLine("#" + i + ":");
			List<HediffDef> list = new List<HediffDef>();
			for (int j = 0; j < 100; j++)
			{
				foreach (HediffGiver_Birthday item in RandomHediffsToGainOnBirthday(ThingDefOf.Human, 1f, j))
				{
					if (!list.Contains(item.hediff))
					{
						list.Add(item.hediff);
						stringBuilder.AppendLine("  age " + j + " - " + item.hediff);
					}
				}
			}
		}
		Log.Message(stringBuilder.ToString());
		stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("=======Actual injuries=========");
		for (int k = 0; k < 200; k++)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(Faction.OfPlayer.def.basicMemberKind, Faction.OfPlayer, null);
			if (pawn.ageTracker.AgeBiologicalYears >= 40)
			{
				stringBuilder.AppendLine(pawn.Name?.ToString() + " age " + pawn.ageTracker.AgeBiologicalYears);
				foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
				{
					stringBuilder.AppendLine(" - " + hediff);
				}
			}
			Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
		}
		Log.Message(stringBuilder.ToString());
	}
}
