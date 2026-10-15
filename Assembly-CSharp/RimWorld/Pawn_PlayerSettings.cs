using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld;

public class Pawn_PlayerSettings : IExposable
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__36 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Pawn_PlayerSettings _003C_003E4__this;

		private int _003CcanAttackTargetCount_003E5__2;

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
		public _003CGetGizmos_003Ed__36(int _003C_003E1__state)
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
			Pawn_PlayerSettings CS_0024_003C_003E8__locals10 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				if (!CS_0024_003C_003E8__locals10.pawn.Drafted)
				{
					break;
				}
				int num2 = 0;
				bool flag = false;
				_003CcanAttackTargetCount_003E5__2 = 0;
				foreach (Pawn item in PawnUtility.SpawnedMasteredPawns(CS_0024_003C_003E8__locals10.pawn))
				{
					if (item.training.HasLearned(TrainableDefOf.Release))
					{
						flag = true;
						if (ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(item))
						{
							num2++;
						}
					}
					if (ModsConfig.OdysseyActive && item.training.HasLearned(TrainableDefOf.AttackTarget))
					{
						_003CcanAttackTargetCount_003E5__2++;
					}
				}
				if (flag)
				{
					Command_Toggle command_Toggle = new Command_Toggle
					{
						defaultLabel = "CommandReleaseAnimalsLabel".Translate() + ((num2 != 0) ? (" (" + num2 + ")") : ""),
						defaultDesc = "CommandReleaseAnimalsDesc".Translate(),
						icon = TexCommand.ReleaseAnimals,
						hotKey = KeyBindingDefOf.Misc7,
						isActive = () => CS_0024_003C_003E8__locals10.animalsReleased,
						toggleAction = delegate
						{
							CS_0024_003C_003E8__locals10.animalsReleased = !CS_0024_003C_003E8__locals10.animalsReleased;
							if (CS_0024_003C_003E8__locals10.animalsReleased)
							{
								foreach (Pawn item2 in PawnUtility.SpawnedMasteredPawns(CS_0024_003C_003E8__locals10.pawn))
								{
									if (item2.caller != null)
									{
										item2.caller.Notify_Released();
									}
									item2.jobs.EndCurrentJob(JobCondition.InterruptForced);
								}
							}
						}
					};
					if (num2 == 0)
					{
						command_Toggle.Disable("CommandReleaseAnimalsFail_NoAnimals".Translate());
					}
					_003C_003E2__current = command_Toggle;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0193;
			}
			case 1:
				_003C_003E1__state = -1;
				goto IL_0193;
			case 2:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0193:
				if (_003CcanAttackTargetCount_003E5__2 <= 0)
				{
					break;
				}
				_003C_003E2__current = new Command_Target
				{
					defaultLabel = "AnimalsAttackTarget".Translate() + $" ({_003CcanAttackTargetCount_003E5__2})",
					defaultDesc = "AnimalsAttackTargetDesc".Translate(),
					targetingParams = TargetingParameters.ForAttackAny(),
					hotKey = KeyBindingDefOf.Misc8,
					icon = Pawn_TrainingTracker.AttackTargetTexture,
					action = delegate(LocalTargetInfo target)
					{
						if (!target.Cell.InHorDistOf(CS_0024_003C_003E8__locals10.pawn.Position, 25.9f))
						{
							Messages.Message("MessageNotInRangeOfMaster".Translate(), MessageTypeDefOf.RejectInput, historical: false);
						}
						else
						{
							string failStr = null;
							bool flag2 = false;
							foreach (Pawn item3 in PawnUtility.SpawnedMasteredPawns(CS_0024_003C_003E8__locals10.pawn))
							{
								if (item3.training.HasLearned(TrainableDefOf.AttackTarget))
								{
									FloatMenuUtility.GetMeleeAttackAction(item3, target, out failStr, ignoreControlled: true)?.Invoke();
									if (failStr.NullOrEmpty())
									{
										flag2 = true;
										item3.training.attackTarget = target.Thing;
										item3.mindState.enemyTarget = target.Thing;
									}
								}
							}
							if (!flag2)
							{
								Messages.Message(failStr, MessageTypeDefOf.RejectInput, historical: false);
							}
						}
					},
					onUpdate = delegate
					{
						GenDraw.DrawRadiusRing(CS_0024_003C_003E8__locals10.pawn.Position, 25.9f);
					}
				};
				_003C_003E1__state = 2;
				return true;
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__36 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__36(0)
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

	public const int UnsetDisplayOrder = -9999999;

	private Pawn pawn;

	private Dictionary<Map, Area> allowedAreas = new Dictionary<Map, Area>();

	public int joinTick;

	private Pawn master;

	public bool followDrafted;

	public bool followFieldwork;

	public bool animalsReleased;

	public MedicalCareCategory medCare = MedicalCareCategory.NoMeds;

	public HostilityResponseMode hostilityResponse = HostilityResponseMode.Flee;

	public bool selfTend;

	public int displayOrder = -9999999;

	public bool animalForage = true;

	public bool animalDig = true;

	private List<Map> allowedAreasKeys;

	private List<Area> allowedAreasValues;

	public Pawn Master
	{
		get
		{
			return master;
		}
		set
		{
			if (master == value)
			{
				return;
			}
			if (value != null && !pawn.training.HasLearned(TrainableDefOf.Obedience))
			{
				Log.ErrorOnce("Attempted to set master for non-obedient pawn", 73908573);
				return;
			}
			bool flag = ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(pawn);
			master = value;
			if (pawn.Spawned && (flag || ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(pawn)))
			{
				pawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
			}
		}
	}

	public Area EffectiveAreaRestrictionInPawnCurrentMap
	{
		get
		{
			if (!RespectsAllowedArea)
			{
				return null;
			}
			if (!allowedAreas.TryGetValue(pawn.MapHeld, out var value))
			{
				return null;
			}
			return value;
		}
	}

	public Area AreaRestrictionInPawnCurrentMap
	{
		get
		{
			if (pawn.MapHeld == null || !allowedAreas.TryGetValue(pawn.MapHeld, out var value))
			{
				return null;
			}
			return value;
		}
		set
		{
			Map map = pawn.MapHeld;
			if (map == null && pawn.DevelopmentalStage == DevelopmentalStage.Baby)
			{
				map = pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Parent)?.MapHeld;
			}
			if (map != null)
			{
				allowedAreas.SetOrAdd(map, value);
				if (pawn.Spawned && !pawn.Drafted && value != null && value == EffectiveAreaRestrictionInPawnCurrentMap && value.TrueCount > 0 && pawn.jobs?.curJob != null && pawn.jobs.curJob.AnyTargetOutsideArea(value))
				{
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
				}
			}
		}
	}

	public bool RespectsAllowedArea
	{
		get
		{
			if (!SupportsAllowedAreas)
			{
				return false;
			}
			if (pawn.GetLord() != null)
			{
				return false;
			}
			if (pawn.Faction == Faction.OfPlayer)
			{
				return pawn.HostFaction == null;
			}
			return false;
		}
	}

	public bool SupportsAllowedAreas
	{
		get
		{
			if (!pawn.Roamer)
			{
				return !pawn.RaceProps.disableAreaControl;
			}
			return false;
		}
	}

	public bool RespectsMaster
	{
		get
		{
			if (Master == null)
			{
				return false;
			}
			if (pawn.Faction == Faction.OfPlayer)
			{
				return Master.Faction == pawn.Faction;
			}
			return false;
		}
	}

	public Pawn RespectedMaster
	{
		get
		{
			if (!RespectsMaster)
			{
				return null;
			}
			return Master;
		}
	}

	public bool UsesConfigurableHostilityResponse
	{
		get
		{
			if (pawn.IsColonist || (pawn.IsColonySubhuman && !pawn.mutant.Def.disableHostilityResponse))
			{
				return pawn.HostFaction == null;
			}
			return false;
		}
	}

	public Pawn_PlayerSettings(Pawn pawn)
	{
		this.pawn = pawn;
		if (Current.ProgramState == ProgramState.Playing)
		{
			joinTick = Find.TickManager.TicksGame;
		}
		else
		{
			joinTick = 0;
		}
		Notify_FactionChanged();
	}

	public void ExposeData()
	{
		Scribe_Values.Look(ref joinTick, "joinTick", 0);
		Scribe_Values.Look(ref animalsReleased, "animalsReleased", defaultValue: false);
		Scribe_Values.Look(ref medCare, "medCare", MedicalCareCategory.NoCare);
		Scribe_Collections.Look(ref allowedAreas, "allowedAreas", LookMode.Reference, LookMode.Reference, ref allowedAreasKeys, ref allowedAreasValues);
		Scribe_References.Look(ref master, "master");
		Scribe_Values.Look(ref followDrafted, "followDrafted", defaultValue: false);
		Scribe_Values.Look(ref followFieldwork, "followFieldwork", defaultValue: false);
		Scribe_Values.Look(ref hostilityResponse, "hostilityResponse", HostilityResponseMode.Flee);
		Scribe_Values.Look(ref selfTend, "selfTend", defaultValue: false);
		Scribe_Values.Look(ref displayOrder, "displayOrder", 0);
		Scribe_Values.Look(ref animalForage, "animalForage", defaultValue: true);
		Scribe_Values.Look(ref animalDig, "animalDig", defaultValue: true);
		if (Scribe.mode == LoadSaveMode.Saving && allowedAreas != null)
		{
			allowedAreas.RemoveAll((KeyValuePair<Map, Area> kvp) => kvp.Key == null || kvp.Value == null);
		}
		if (Scribe.mode == LoadSaveMode.PostLoadInit && pawn.Roamer)
		{
			allowedAreas.Clear();
		}
		if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs && allowedAreas == null)
		{
			allowedAreas = new Dictionary<Map, Area>();
		}
		if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.ResolvingCrossRefs || Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			Area refee = null;
			Scribe_References.Look(ref refee, "areaAllowed");
			if (refee != null && Find.AnyPlayerHomeMap != null)
			{
				allowedAreas.Add(Find.AnyPlayerHomeMap, refee);
			}
		}
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__36))]
	public IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__36(-2)
		{
			_003C_003E4__this = this
		};
	}

	public void Notify_FactionChanged()
	{
		ResetMedicalCare();
		allowedAreas.Clear();
	}

	public void ResetMedicalCare()
	{
		if (Scribe.mode == LoadSaveMode.LoadingVars)
		{
			return;
		}
		if (pawn.Faction == Faction.OfPlayer)
		{
			if (ModsConfig.AnomalyActive && pawn.IsGhoul)
			{
				medCare = Find.PlaySettings.defaultCareForGhouls;
			}
			else if (ModsConfig.AnomalyActive && pawn.IsEntity)
			{
				medCare = Find.PlaySettings.defaultCareForEntities;
			}
			else if (!pawn.RaceProps.Animal)
			{
				medCare = (pawn.IsSlave ? Find.PlaySettings.defaultCareForSlave : Find.PlaySettings.defaultCareForColonist);
			}
			else
			{
				medCare = Find.PlaySettings.defaultCareForTamedAnimal;
			}
		}
		else if (ModsConfig.AnomalyActive && pawn.IsEntity)
		{
			medCare = Find.PlaySettings.defaultCareForEntities;
		}
		else if (pawn.IsPrisoner)
		{
			medCare = Find.PlaySettings.defaultCareForPrisoner;
		}
		else if (pawn.Faction == null && pawn.RaceProps.Animal)
		{
			medCare = Find.PlaySettings.defaultCareForWildlife;
		}
		else if (pawn.Faction != null)
		{
			switch (pawn.Faction.RelationWith(Faction.OfPlayer).kind)
			{
			case FactionRelationKind.Ally:
				medCare = Find.PlaySettings.defaultCareForFriendlyFaction;
				break;
			case FactionRelationKind.Neutral:
				medCare = Find.PlaySettings.defaultCareForNeutralFaction;
				break;
			case FactionRelationKind.Hostile:
				medCare = Find.PlaySettings.defaultCareForHostileFaction;
				break;
			}
		}
		else if (!pawn.Faction.HostileTo(Faction.OfPlayer))
		{
			medCare = Find.PlaySettings.defaultCareForNoFaction;
		}
		else
		{
			medCare = Find.PlaySettings.defaultCareForHostileFaction;
		}
	}

	public void Notify_AreaRemoved(Area area)
	{
		foreach (Map item in allowedAreas.Keys.ToList())
		{
			if (allowedAreas[item] == area)
			{
				allowedAreas.Remove(item);
			}
		}
	}

	public void Notify_MapRemoved(Map map)
	{
		if (allowedAreas.ContainsKey(map))
		{
			allowedAreas.Remove(map);
		}
	}
}
