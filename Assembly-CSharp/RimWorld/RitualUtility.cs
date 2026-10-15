using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI.Group;

namespace RimWorld;

public static class RitualUtility
{
	[CompilerGenerated]
	private sealed class _003CAllRolesForPawn_003Ed__8 : IEnumerable<Precept_Role>, IEnumerable, IEnumerator<Precept_Role>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Precept_Role _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn p;

		public Pawn _003C_003E3__p;

		private IEnumerator<Precept_Role> _003C_003E7__wrap1;

		Precept_Role IEnumerator<Precept_Role>.Current
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
		public _003CAllRolesForPawn_003Ed__8(int _003C_003E1__state)
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
				Precept_Role precept_Role;
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (p.Ideo == null)
					{
						return false;
					}
					_003C_003E7__wrap1 = p.Ideo.RolesListForReading.Where((Precept_Role r) => !r.def.leaderRole).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00ad;
				case 1:
					_003C_003E1__state = -3;
					goto IL_00ad;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_00ad:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Precept_Role current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					precept_Role = Faction.OfPlayer.ideos.PrimaryIdeo.RolesListForReading.FirstOrDefault((Precept_Role r) => r.def.leaderRole);
					if (precept_Role != null)
					{
						_003C_003E2__current = precept_Role;
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
		IEnumerator<Precept_Role> IEnumerable<Precept_Role>.GetEnumerator()
		{
			_003CAllRolesForPawn_003Ed__8 _003CAllRolesForPawn_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CAllRolesForPawn_003Ed__ = this;
			}
			else
			{
				_003CAllRolesForPawn_003Ed__ = new _003CAllRolesForPawn_003Ed__8(0);
			}
			_003CAllRolesForPawn_003Ed__.p = _003C_003E3__p;
			return _003CAllRolesForPawn_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Precept_Role>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetActiveRitualsForPrecept_003Ed__6 : IEnumerable<LordJob_Ritual>, IEnumerable, IEnumerator<LordJob_Ritual>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private LordJob_Ritual _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Precept_Ritual precept;

		public Precept_Ritual _003C_003E3__precept;

		private List<Map> _003Cmaps_003E5__2;

		private int _003Ci_003E5__3;

		private List<Lord> _003Clords_003E5__4;

		private int _003Cj_003E5__5;

		LordJob_Ritual IEnumerator<LordJob_Ritual>.Current
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
		public _003CGetActiveRitualsForPrecept_003Ed__6(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cmaps_003E5__2 = null;
			_003Clords_003E5__4 = null;
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
				goto IL_009f;
			}
			_003C_003E1__state = -1;
			_003Cmaps_003E5__2 = Find.Maps;
			_003Ci_003E5__3 = 0;
			goto IL_00d9;
			IL_00af:
			if (_003Cj_003E5__5 < _003Clords_003E5__4.Count)
			{
				if (_003Clords_003E5__4[_003Cj_003E5__5].LordJob is LordJob_Ritual lordJob_Ritual && lordJob_Ritual.Ritual == precept)
				{
					_003C_003E2__current = lordJob_Ritual;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_009f;
			}
			_003Clords_003E5__4 = null;
			_003Ci_003E5__3++;
			goto IL_00d9;
			IL_009f:
			_003Cj_003E5__5++;
			goto IL_00af;
			IL_00d9:
			if (_003Ci_003E5__3 < _003Cmaps_003E5__2.Count)
			{
				_003Clords_003E5__4 = _003Cmaps_003E5__2[_003Ci_003E5__3].lordManager.lords;
				_003Cj_003E5__5 = 0;
				goto IL_00af;
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
		IEnumerator<LordJob_Ritual> IEnumerable<LordJob_Ritual>.GetEnumerator()
		{
			_003CGetActiveRitualsForPrecept_003Ed__6 _003CGetActiveRitualsForPrecept_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetActiveRitualsForPrecept_003Ed__ = this;
			}
			else
			{
				_003CGetActiveRitualsForPrecept_003Ed__ = new _003CGetActiveRitualsForPrecept_003Ed__6(0);
			}
			_003CGetActiveRitualsForPrecept_003Ed__.precept = _003C_003E3__precept;
			return _003CGetActiveRitualsForPrecept_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<LordJob_Ritual>)this).GetEnumerator();
		}
	}

	public static bool IsRitualTarget(this Thing thing)
	{
		return thing.TargetOfRitual() != null;
	}

	public static LordJob_Ritual TargetOfRitual(this Thing thing)
	{
		if (!thing.Spawned)
		{
			return null;
		}
		foreach (Lord lord in thing.Map.lordManager.lords)
		{
			if (lord.LordJob is LordJob_Ritual lordJob_Ritual && lordJob_Ritual.selectedTarget.Thing == thing && lordJob_Ritual.lord.CurLordToil.data is LordToilData_Gathering lordToilData_Gathering && lordToilData_Gathering.presentForTicks.Any())
			{
				return lordJob_Ritual;
			}
		}
		return null;
	}

	public static bool GoodSpectateCellForRitual(IntVec3 spot, Pawn p, Map map)
	{
		if (!spot.ContainsStaticFire(map) && !spot.GetTerrain(map).avoidWander)
		{
			return !PawnUtility.KnownDangerAt(spot, map, p);
		}
		return false;
	}

	public static float CalculateQualityAbstract(Precept_Ritual ritual, TargetInfo ritualTarget, RitualRoleAssignments assignments, RitualObligation obligation = null)
	{
		RitualOutcomeEffectDef def = ritual.outcomeEffect.def;
		float num = def.startingQuality;
		foreach (RitualOutcomeComp comp in def.comps)
		{
			QualityFactor qualityFactor = comp.GetQualityFactor(ritual, ritualTarget, obligation, assignments, null);
			if (qualityFactor != null)
			{
				if (qualityFactor.uncertainOutcome)
				{
					Log.Warning("Tried to calculate abstract quality for a ritual with uncertain quality offset, that can't be calculated without actually executing the ritual.");
				}
				else
				{
					num += qualityFactor.quality;
				}
			}
		}
		return num;
	}

	public static string QualityBreakdownAbstract(Precept_Ritual ritual, TargetInfo ritualTarget, RitualRoleAssignments assignments, RitualObligation obligation = null)
	{
		RitualOutcomeEffectDef def = ritual.outcomeEffect.def;
		float f = CalculateQualityAbstract(ritual, ritualTarget, assignments, obligation);
		TaggedString taggedString = "RitualOutcomeQualitySpecific".Translate(ritual.Label, f.ToStringPercent()).CapitalizeFirst() + ":\n";
		if (def.startingQuality > 0f)
		{
			taggedString += "\n  - " + "StartingRitualQuality".Translate(def.startingQuality.ToStringPercent()) + ".";
		}
		foreach (RitualOutcomeComp comp in def.comps)
		{
			QualityFactor qualityFactor = comp.GetQualityFactor(ritual, ritualTarget, obligation, assignments, null);
			if (qualityFactor != null)
			{
				taggedString += "\n  - " + qualityFactor.label;
				taggedString += ": " + comp.GetDescAbstract(qualityFactor.positive, qualityFactor.quality) + ".";
			}
		}
		if (ritual.RepeatPenaltyActive)
		{
			taggedString += "\n  - " + "RitualOutcomePerformedRecently".Translate() + ": " + ritual.RepeatQualityPenalty.ToStringPercent();
		}
		Tuple<ExpectationDef, float> expectationsOffset = RitualOutcomeEffectWorker_FromQuality.GetExpectationsOffset(ritualTarget.Map, ritual.def);
		if (expectationsOffset != null)
		{
			taggedString += "\n  - " + "RitualQualityExpectations".Translate(expectationsOffset.Item1.LabelCap) + ": " + expectationsOffset.Item2.ToStringPercent();
		}
		return taggedString;
	}

	public static void AddArrivalTag(Pawn pawn)
	{
		if (pawn.GetLord()?.LordJob is LordJob_Ritual lordJob_Ritual && !lordJob_Ritual.PawnTagSet(pawn, "Arrived"))
		{
			lordJob_Ritual.AddTagForPawn(pawn, "Arrived");
		}
	}

	[IteratorStateMachine(typeof(_003CGetActiveRitualsForPrecept_003Ed__6))]
	public static IEnumerable<LordJob_Ritual> GetActiveRitualsForPrecept(Precept_Ritual precept)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetActiveRitualsForPrecept_003Ed__6(-2)
		{
			_003C_003E3__precept = precept
		};
	}

	public static string RoleChangeConfirmation(Pawn pawn, Precept_Role oldRole, Precept_Role newRole)
	{
		string text = "";
		if (oldRole != null)
		{
			text += " " + "ChooseRoleConfirmAssignHasOtherRole".Translate(pawn.Named("PAWN"), oldRole.Named("ROLE"));
		}
		Pawn pawn2 = newRole.ChosenPawns().FirstOrDefault();
		if (pawn2 != null && newRole is Precept_RoleSingle)
		{
			text += " " + "ChooseRoleConfirmAssignReplace".Translate(pawn2.Named("PAWN"));
		}
		else if (newRole.def.leaderRole)
		{
			foreach (Ideo allIdeo in Faction.OfPlayer.ideos.AllIdeos)
			{
				foreach (Precept item in allIdeo.PreceptsListForReading)
				{
					if (item != oldRole && item is Precept_Role precept_Role)
					{
						Pawn pawn3 = precept_Role.ChosenPawnSingle();
						if (precept_Role.def.leaderRole && pawn3 != null && pawn3 != pawn && pawn3.IsFreeColonist)
						{
							text += " " + "ChooseRoleConfirmAssignReplaceLeader".Translate(pawn3.Named("PAWN"), newRole.Named("ROLE"), precept_Role.Named("OTHERROLE"));
							break;
						}
					}
				}
			}
		}
		IEnumerable<WorkTypeDef> disabledWorkTypes = newRole.DisabledWorkTypes;
		if (disabledWorkTypes.Any())
		{
			if (!text.NullOrEmpty())
			{
				text += "\n\n";
			}
			text += "ChooseRoleListWorkTypeRestrictions".Translate(pawn.Named("PAWN")) + ": \n" + disabledWorkTypes.Select((WorkTypeDef x) => x.labelShort.CapitalizeFirst()).ToLineList("  - ");
		}
		if (!newRole.def.grantedAbilities.NullOrEmpty())
		{
			if (!text.NullOrEmpty())
			{
				text += "\n\n";
			}
			text += "ChooseRoleListAbilities".Translate(pawn.Named("PAWN")) + ": \n" + newRole.def.grantedAbilities.Select((AbilityDef x) => x.LabelCap.ToString()).ToLineList("  - ");
		}
		if (!newRole.apparelRequirements.NullOrEmpty())
		{
			if (!text.NullOrEmpty())
			{
				text += "\n\n";
			}
			text += "ChooseRoleListApparelDemands".Translate(newRole.Named("ROLE")) + ": \n" + newRole.AllApparelRequirementLabels(pawn.gender, pawn).ToLineList("  - ");
		}
		if (newRole.def.roleRequiredWorkTags != 0)
		{
			List<string> list = new List<string>();
			foreach (WorkTags allSelectedItem in newRole.def.roleRequiredWorkTags.GetAllSelectedItems<WorkTags>())
			{
				if (pawn.WorkTagIsDisabled(allSelectedItem))
				{
					list.Add(allSelectedItem.LabelTranslated().CapitalizeFirst());
				}
			}
			if (list.Any())
			{
				if (!text.NullOrEmpty())
				{
					text += "\n\n";
				}
				text += "ChooseRoleRequiredWorkTagsDisabled".Translate(pawn.Named("PAWN"), newRole.Named("ROLE")) + ": \n" + list.ToLineList("  - ");
			}
		}
		else if (newRole.def.roleRequiredWorkTagAny != 0)
		{
			bool flag = false;
			List<string> list2 = new List<string>();
			foreach (WorkTags allSelectedItem2 in newRole.def.roleRequiredWorkTagAny.GetAllSelectedItems<WorkTags>())
			{
				if (!pawn.WorkTagIsDisabled(allSelectedItem2))
				{
					flag = true;
					break;
				}
				list2.Add(allSelectedItem2.LabelTranslated().CapitalizeFirst());
			}
			if (!flag)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n\n";
				}
				text += "ChooseRoleRequiredWorkTagsDisabled".Translate(pawn.Named("PAWN"), newRole.Named("ROLE")) + ": \n" + list2.ToLineList("  - ");
			}
		}
		if (newRole.ChosenPawnSingle() == null && newRole is Precept_RoleSingle)
		{
			if (!text.NullOrEmpty())
			{
				text += "\n\n";
			}
			text += "ChooseRoleHint".Translate();
		}
		return text;
	}

	[IteratorStateMachine(typeof(_003CAllRolesForPawn_003Ed__8))]
	public static IEnumerable<Precept_Role> AllRolesForPawn(Pawn p)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAllRolesForPawn_003Ed__8(-2)
		{
			_003C_003E3__p = p
		};
	}

	public static IntVec3 RitualCrowdCenterFor(Pawn pawn)
	{
		if (pawn.GetLord()?.LordJob is LordJob_Ritual lordJob_Ritual && pawn.CurJob != null && pawn.CurJob.speechFaceSpectatorsIfPossible)
		{
			return lordJob_Ritual.CurrentSpectatorCrowdCenter();
		}
		return IntVec3.Invalid;
	}
}
