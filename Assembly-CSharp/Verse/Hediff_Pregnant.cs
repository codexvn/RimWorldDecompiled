using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI.Group;

namespace Verse;

public class Hediff_Pregnant : HediffWithParents
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__21 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Hediff_Pregnant _003C_003E4__this;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

		Gizmo IEnumerator<Gizmo>.Current
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
		public _003CGetGizmos_003Ed__21(int _003C_003E1__state)
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
				Hediff_Pregnant CS_0024_003C_003E8__locals10 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((HediffWithParents)CS_0024_003C_003E8__locals10).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0076;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0076;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00e6;
				case 3:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0076:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (!DebugSettings.ShowDevGizmos)
					{
						break;
					}
					if (CS_0024_003C_003E8__locals10.CurStageIndex < 2)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Next trimester",
							action = delegate
							{
								HediffStage hediffStage = CS_0024_003C_003E8__locals10.def.stages[CS_0024_003C_003E8__locals10.CurStageIndex + 1];
								CS_0024_003C_003E8__locals10.severityInt = hediffStage.minSeverity;
							}
						};
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00e6;
					IL_00e6:
					if (ModsConfig.BiotechActive && CS_0024_003C_003E8__locals10.pawn.RaceProps.Humanlike && CS_0024_003C_003E8__locals10.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PregnancyLabor) == null)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Start Labor",
							action = delegate
							{
								CS_0024_003C_003E8__locals10.StartLabor();
								CS_0024_003C_003E8__locals10.pawn.health.RemoveHediff(CS_0024_003C_003E8__locals10);
							}
						};
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__21 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__21(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	private int lastStage;

	private const int MiscarryCheckInterval = 1000;

	private const float MTBMiscarryStarvingDays = 2f;

	private const float MTBMiscarryWoundedDays = 2f;

	private const float MalnutritionMinSeverityForMiscarry = 0.1f;

	public float GestationProgress
	{
		get
		{
			return Severity;
		}
		private set
		{
			Severity = value;
		}
	}

	private bool IsSeverelyWounded
	{
		get
		{
			float num = 0f;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i] is Hediff_Injury && !hediffs[i].IsPermanent())
				{
					num += hediffs[i].Severity;
				}
			}
			List<Hediff_MissingPart> missingPartsCommonAncestors = pawn.health.hediffSet.GetMissingPartsCommonAncestors();
			for (int j = 0; j < missingPartsCommonAncestors.Count; j++)
			{
				if (missingPartsCommonAncestors[j].IsFreshNonSolidExtremity)
				{
					num += missingPartsCommonAncestors[j].Part.def.GetMaxHealth(pawn);
				}
			}
			return num > 38f * pawn.RaceProps.baseHealthScale;
		}
	}

	public PregnancyAttitude? Attitude
	{
		get
		{
			if (comps == null || !pawn.RaceProps.Humanlike)
			{
				return null;
			}
			for (int i = 0; i < comps.Count; i++)
			{
				if (comps[i] is HediffComp_PregnantHuman hediffComp_PregnantHuman)
				{
					return hediffComp_PregnantHuman.Attitude;
				}
			}
			return null;
		}
	}

	public override void PostAdd(DamageInfo? dinfo)
	{
		base.PostAdd(dinfo);
		lastStage = CurStageIndex;
	}

	public override void TickInterval(int delta)
	{
		ageTicks += delta;
		if (CurStageIndex != lastStage)
		{
			NotifyPlayerOfTrimesterPassing();
			lastStage = CurStageIndex;
		}
		if ((!pawn.RaceProps.Humanlike || !Find.Storyteller.difficulty.babiesAreHealthy) && pawn.IsHashIntervalTick(1000, delta))
		{
			Need_Food food = pawn.needs.food;
			if (food != null && food.CurCategory == HungerCategory.Starving)
			{
				Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Malnutrition);
				if (firstHediffOfDef != null && firstHediffOfDef.Severity > 0.1f && Rand.MTBEventOccurs(2f, 60000f, 1000f))
				{
					if (PawnUtility.ShouldSendNotificationAbout(pawn))
					{
						string text = (pawn.Name.Numerical ? pawn.LabelShort : (pawn.LabelShort + " (" + pawn.kindDef.label + ")"));
						Messages.Message("MessageMiscarriedStarvation".Translate(text, pawn), pawn, MessageTypeDefOf.NegativeHealthEvent);
					}
					Miscarry();
					return;
				}
			}
			if (IsSeverelyWounded && Rand.MTBEventOccurs(2f, 60000f, 1000f))
			{
				if (PawnUtility.ShouldSendNotificationAbout(pawn))
				{
					string text2 = (pawn.Name.Numerical ? pawn.LabelShort : (pawn.LabelShort + " (" + pawn.kindDef.label + ")"));
					Messages.Message("MessageMiscarriedPoorHealth".Translate(text2, pawn), pawn, MessageTypeDefOf.NegativeHealthEvent);
				}
				Miscarry();
				return;
			}
		}
		float num = PawnUtility.BodyResourceGrowthSpeed(pawn) / (pawn.RaceProps.gestationPeriodDays * 60000f);
		GestationProgress += num * (float)delta;
		if (!(GestationProgress >= 1f))
		{
			return;
		}
		if (!pawn.RaceProps.Humanlike)
		{
			if (PawnUtility.ShouldSendNotificationAbout(pawn))
			{
				Messages.Message("MessageGaveBirth".Translate(pawn), pawn, MessageTypeDefOf.PositiveEvent);
			}
			DoBirthSpawn(pawn, base.Father);
		}
		else
		{
			StartLabor();
		}
		pawn.health.RemoveHediff(this);
	}

	public void Miscarry()
	{
		base.pawn.needs?.mood?.thoughts?.memories?.TryGainMemory(ThoughtDefOf.Miscarried);
		Pawn pawn = base.Mother;
		Pawn pawn2 = base.Father;
		if (pawn != base.pawn)
		{
			pawn?.needs?.mood?.thoughts?.memories?.TryGainMemory(ThoughtDefOf.PartnerMiscarried, base.pawn);
		}
		if (pawn2 != base.pawn)
		{
			pawn2?.needs?.mood?.thoughts?.memories?.TryGainMemory(ThoughtDefOf.PartnerMiscarried, base.pawn);
		}
		base.pawn.health.RemoveHediff(this);
	}

	public static void DoBirthSpawn(Pawn mother, Pawn father)
	{
		if (mother.RaceProps.Humanlike && !ModsConfig.BiotechActive)
		{
			return;
		}
		int num = ((mother.RaceProps.litterSizeCurve == null) ? 1 : Mathf.RoundToInt(Rand.ByCurve(mother.RaceProps.litterSizeCurve)));
		if (num < 1)
		{
			num = 1;
		}
		PawnGenerationRequest request = new PawnGenerationRequest(mother.kindDef, mother.Faction, PawnGenerationContext.NonPlayer, null, forceGenerateNewPawn: false, allowDead: false, allowDowned: true, canGeneratePawnRelations: true, mustBeCapableOfViolence: false, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: false, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, forceNoIdeo: false, forceNoBackstory: false, forbidAnyTitle: false, forceDead: false, null, null, null, null, null, 0f, DevelopmentalStage.Newborn, null, null, null);
		Pawn pawn = null;
		for (int i = 0; i < num; i++)
		{
			pawn = PawnGenerator.GeneratePawn(request);
			if (PawnUtility.TrySpawnHatchedOrBornPawn(pawn, mother, null))
			{
				if (pawn.playerSettings != null && mother.playerSettings != null)
				{
					pawn.playerSettings.AreaRestrictionInPawnCurrentMap = mother.playerSettings.AreaRestrictionInPawnCurrentMap;
				}
				if (pawn.RaceProps.IsFlesh)
				{
					pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, mother);
					if (father != null)
					{
						pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, father);
					}
				}
				if (mother.Spawned)
				{
					mother.GetLord()?.AddPawn(pawn);
				}
			}
			else
			{
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
			}
			TaleRecorder.RecordTale(TaleDefOf.GaveBirth, mother, pawn);
		}
		if (mother.Spawned)
		{
			FilthMaker.TryMakeFilth(mother.Position, mother.Map, ThingDefOf.Filth_AmnioticFluid, mother.LabelIndefinite(), 5);
			mother.caller?.DoCall();
			pawn.caller?.DoCall();
		}
	}

	public void StartLabor()
	{
		if (ModLister.CheckBiotech("labor"))
		{
			((Hediff_Labor)pawn.health.AddHediff(HediffDefOf.PregnancyLabor, null, null)).SetParents(base.Mother, base.Father, geneSet);
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.MorningSickness);
			if (firstHediffOfDef != null)
			{
				pawn.health.RemoveHediff(firstHediffOfDef);
			}
			Hediff firstHediffOfDef2 = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PregnancyMood);
			if (firstHediffOfDef2 != null)
			{
				pawn.health.RemoveHediff(firstHediffOfDef2);
			}
			if (PawnUtility.ShouldSendNotificationAbout(pawn))
			{
				Find.LetterStack.ReceiveLetter("LetterColonistPregnancyLaborLabel".Translate(pawn), "LetterColonistPregnancyLabor".Translate(pawn), LetterDefOf.NeutralEvent, pawn);
			}
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref lastStage, "lastStage", 0);
	}

	private void NotifyPlayerOfTrimesterPassing()
	{
		if (pawn.RaceProps.Humanlike && PawnUtility.ShouldSendNotificationAbout(pawn))
		{
			Messages.Message(((lastStage == 0) ? "MessageColonistReaching2ndTrimesterPregnancy" : "MessageColonistReaching3rdTrimesterPregnancy").Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.PositiveEvent);
			if (lastStage == 1 && !Find.History.everThirdTrimesterPregnancy)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelThirdTrimester".Translate(pawn), "LetterTextThirdTrimester".Translate(pawn), LetterDefOf.PositiveEvent, (TargetInfo)pawn);
				Find.History.everThirdTrimesterPregnancy = true;
			}
		}
	}

	public override void PostDebugAdd()
	{
		if (ModsConfig.BiotechActive && pawn.RaceProps.Humanlike)
		{
			SetParents(pawn, null, PregnancyUtility.GetInheritedGeneSet(null, pawn));
		}
	}

	public override string DebugString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(base.DebugString());
		stringBuilder.AppendLine("Gestation progress: " + GestationProgress.ToStringPercent());
		stringBuilder.AppendLine("Time left: " + ((int)((1f - GestationProgress) * pawn.RaceProps.gestationPeriodDays * 60000f)).ToStringTicksToPeriod());
		return stringBuilder.ToString();
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__21))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__21(-2)
		{
			_003C_003E4__this = this
		};
	}
}
