using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

public static class MechanitorUtility
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass21_0
	{
		public Pawn mech;

		internal void _003CGetMechGizmos_003Eb__2()
		{
			mech.SetFaction(Faction.OfPlayer);
		}

		internal void _003CGetMechGizmos_003Eb__3()
		{
			mech.Kill(null);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass21_1
	{
		public Pawn overseer;

		internal void _003CGetMechGizmos_003Eb__0()
		{
			Find.Selector.ClearSelection();
			Find.Selector.Select(overseer);
		}

		internal void _003CGetMechGizmos_003Eb__1()
		{
			if (overseer != null)
			{
				GenDraw.DrawArrowPointingAt(overseer.TrueCenter());
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetMechGizmos_003Ed__21 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn mech;

		public Pawn _003C_003E3__mech;

		private _003C_003Ec__DisplayClass21_0 _003C_003E8__1;

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
		public _003CGetMechGizmos_003Ed__21(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E8__1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			Gizmo activeMechShieldGizmo;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass21_0();
				_003C_003E8__1.mech = mech;
				if (_003C_003E8__1.mech.IsColonyMech && EverControllable(_003C_003E8__1.mech))
				{
					_003C_003Ec__DisplayClass21_1 CS_0024_003C_003E8__locals5 = new _003C_003Ec__DisplayClass21_1
					{
						overseer = _003C_003E8__1.mech.GetOverseer()
					};
					bool flag = CS_0024_003C_003E8__locals5.overseer == null || !CS_0024_003C_003E8__locals5.overseer.Spawned;
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "CommandSelectOverseer".Translate(),
						defaultDesc = (flag ? "CommandSelectOverseerDisabledDesc".Translate() : "CommandSelectOverseerDesc".Translate()),
						icon = SelectOverseerIcon.Texture,
						action = delegate
						{
							Find.Selector.ClearSelection();
							Find.Selector.Select(CS_0024_003C_003E8__locals5.overseer);
						},
						onHover = delegate
						{
							if (CS_0024_003C_003E8__locals5.overseer != null)
							{
								GenDraw.DrawArrowPointingAt(CS_0024_003C_003E8__locals5.overseer.TrueCenter());
							}
						},
						Disabled = flag
					};
					_003C_003E1__state = 1;
					return true;
				}
				if (DebugSettings.ShowDevGizmos && _003C_003E8__1.mech.Faction != Faction.OfPlayer)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Recruit",
						action = delegate
						{
							_003C_003E8__1.mech.SetFaction(Faction.OfPlayer);
						}
					};
					_003C_003E1__state = 4;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				if (Find.Selector.IsSelected(_003C_003E8__1.mech))
				{
					MechanitorControlGroup mechControlGroup = _003C_003E8__1.mech.GetMechControlGroup();
					if (mechControlGroup != null)
					{
						_003C_003E2__current = new MechanitorControlGroupGizmo(mechControlGroup);
						_003C_003E1__state = 2;
						return true;
					}
				}
				goto IL_0183;
			case 2:
				_003C_003E1__state = -1;
				goto IL_0183;
			case 3:
				_003C_003E1__state = -1;
				break;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Kill",
					action = delegate
					{
						_003C_003E8__1.mech.Kill(null);
					}
				};
				_003C_003E1__state = 5;
				return true;
			case 5:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0183:
				activeMechShieldGizmo = RemoteShieldUtility.GetActiveMechShieldGizmo(_003C_003E8__1.mech);
				if (activeMechShieldGizmo != null)
				{
					_003C_003E2__current = activeMechShieldGizmo;
					_003C_003E1__state = 3;
					return true;
				}
				break;
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
			_003CGetMechGizmos_003Ed__21 _003CGetMechGizmos_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetMechGizmos_003Ed__ = this;
			}
			else
			{
				_003CGetMechGizmos_003Ed__ = new _003CGetMechGizmos_003Ed__21(0);
			}
			_003CGetMechGizmos_003Ed__.mech = _003C_003E3__mech;
			return _003CGetMechGizmos_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CMechsInPlayerFaction_003Ed__23 : IEnumerable<Pawn>, IEnumerable, IEnumerator<Pawn>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Pawn _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private List<Map> _003Cmaps_003E5__2;

		private int _003Ci_003E5__3;

		private List<Pawn> _003Cpawns_003E5__4;

		private int _003Cj_003E5__5;

		Pawn IEnumerator<Pawn>.Current
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
		public _003CMechsInPlayerFaction_003Ed__23(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cmaps_003E5__2 = null;
			_003Cpawns_003E5__4 = null;
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
			if (_003Cj_003E5__5 < _003Cpawns_003E5__4.Count)
			{
				if (_003Cpawns_003E5__4[_003Cj_003E5__5].IsColonyMech)
				{
					_003C_003E2__current = _003Cpawns_003E5__4[_003Cj_003E5__5];
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_009f;
			}
			_003Cpawns_003E5__4 = null;
			_003Ci_003E5__3++;
			goto IL_00d9;
			IL_009f:
			_003Cj_003E5__5++;
			goto IL_00af;
			IL_00d9:
			if (_003Ci_003E5__3 < _003Cmaps_003E5__2.Count)
			{
				_003Cpawns_003E5__4 = _003Cmaps_003E5__2[_003Ci_003E5__3].mapPawns.PawnsInFaction(Faction.OfPlayer);
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
		IEnumerator<Pawn> IEnumerable<Pawn>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CMechsInPlayerFaction_003Ed__23(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Pawn>)this).GetEnumerator();
		}
	}

	private static CachedTexture SelectOverseerIcon = new CachedTexture("UI/Icons/SelectOverseer");

	private static List<Pawn> tmpMechs = new List<Pawn>();

	private static List<AssignedMech> tmpAssignedMechs = new List<AssignedMech>();

	private static List<ThingDefCountClass> tmpIngredients = new List<ThingDefCountClass>();

	private static List<ThingDef> cachedRechargers;

	private static List<RecipeDef> cachedMechRecipes;

	private static readonly List<Pawn> tmpLeftBehind = new List<Pawn>();

	public static List<ThingDef> MechRechargers
	{
		get
		{
			if (cachedRechargers == null)
			{
				cachedRechargers = DefDatabase<ThingDef>.AllDefs.Where((ThingDef d) => d.IsMechRecharger).ToList();
			}
			return cachedRechargers;
		}
	}

	public static List<RecipeDef> MechRecipes
	{
		get
		{
			if (cachedMechRecipes == null)
			{
				cachedMechRecipes = DefDatabase<RecipeDef>.AllDefs.Where(delegate(RecipeDef r)
				{
					ThingDef producedThingDef = r.ProducedThingDef;
					return producedThingDef != null && producedThingDef.race?.IsMechanoid == true;
				}).ToList();
			}
			return cachedMechRecipes;
		}
	}

	public static bool IsMechanitor(Pawn pawn)
	{
		if (ShouldBeMechanitor(pawn))
		{
			return pawn.mechanitor != null;
		}
		return false;
	}

	public static bool ShouldBeMechanitor(Pawn pawn)
	{
		if (ModsConfig.BiotechActive && pawn.Faction.IsPlayerSafe())
		{
			return pawn.health.hediffSet.HasHediff(HediffDefOf.MechlinkImplant);
		}
		return false;
	}

	public static bool IsPlayerOverseerSubject(Pawn pawn)
	{
		if (pawn.Faction.IsPlayerSafe())
		{
			return pawn.OverseerSubject != null;
		}
		return false;
	}

	public static MechWorkModeDef GetMechWorkMode(this Pawn pawn)
	{
		return pawn.GetMechControlGroup()?.WorkMode;
	}

	public static MechanitorControlGroup GetMechControlGroup(this Pawn pawn)
	{
		return pawn.GetOverseer()?.mechanitor?.GetControlGroup(pawn);
	}

	public static Pawn GetOverseer(this Pawn pawn)
	{
		if (ModsConfig.BiotechActive)
		{
			return pawn.relations?.GetFirstDirectRelationPawn(PawnRelationDefOf.Overseer);
		}
		return null;
	}

	public static bool IsGestating(this Pawn pawn)
	{
		return pawn.ParentHolder is Building_MechGestator;
	}

	public static AcceptanceReport CanControlMech(Pawn pawn, Pawn mech)
	{
		if (pawn.mechanitor == null || !mech.IsColonyMech || mech.Downed || mech.Dead || mech.IsAttacking())
		{
			return false;
		}
		if (!EverControllable(mech))
		{
			return "CannotControlMechNeverControllable".Translate();
		}
		if (mech.GetOverseer() == pawn)
		{
			return "CannotControlMechAlreadyControlled".Translate(pawn.LabelShort);
		}
		int num = pawn.mechanitor.TotalBandwidth - pawn.mechanitor.UsedBandwidth;
		float statValue = mech.GetStatValue(StatDefOf.BandwidthCost);
		if ((float)num < statValue)
		{
			return "CannotControlMechNotEnoughBandwidth".Translate();
		}
		return true;
	}

	public static bool EverControllable(Pawn mech)
	{
		return mech.OverseerSubject != null;
	}

	public static bool IsColonyMechRequiringMechanitor(this Pawn mech)
	{
		if (!mech.IsColonyMech)
		{
			return false;
		}
		CompOverseerSubject overseerSubject = mech.OverseerSubject;
		if (overseerSubject == null)
		{
			return false;
		}
		return overseerSubject.State != OverseerSubjectState.Overseen;
	}

	public static void Notify_MechlinkQuestRewardAvailable(Quest quest, LookTargets lookTargets = null)
	{
		if (Find.History.mechlinkEverAvailable)
		{
			return;
		}
		List<Pawn> allMaps_FreeColonists = PawnsFinder.AllMaps_FreeColonists;
		bool flag = false;
		for (int i = 0; i < allMaps_FreeColonists.Count; i++)
		{
			if (IsMechanitor(allMaps_FreeColonists[i]))
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			Find.LetterStack.ReceiveLetter("LetterLabelMechlinkAvailable".Translate(), "LetterMechlinkAvailable".Translate(quest.name), LetterDefOf.PositiveEvent, lookTargets, null, quest);
		}
		Find.History.Notify_MechlinkAvailable();
	}

	public static bool TryGetBandwidthLostFromDroppingThing(Pawn pawn, Thing thing, out int bandwidthLost)
	{
		bandwidthLost = -1;
		if (thing.def.equippedStatOffsets.NullOrEmpty())
		{
			return false;
		}
		bandwidthLost = Mathf.RoundToInt(thing.def.equippedStatOffsets.GetStatOffsetFromList(StatDefOf.MechBandwidth));
		if (bandwidthLost <= 0)
		{
			return false;
		}
		int num = pawn.mechanitor.TotalBandwidth - pawn.mechanitor.UsedBandwidth;
		if (bandwidthLost <= num)
		{
			return false;
		}
		return true;
	}

	public static bool TryGetMechsLostFromDroppingThing(Pawn pawn, Thing thing, out List<Pawn> lostMechs, out int bandwidthLost)
	{
		if (!TryGetBandwidthLostFromDroppingThing(pawn, thing, out bandwidthLost))
		{
			lostMechs = null;
			return false;
		}
		return TryGetMechsLostFromBandwidthReduction(pawn, bandwidthLost, out lostMechs);
	}

	public static bool TryGetMechsLostFromBandwidthReduction(Pawn pawn, int lostBandwidth, out List<Pawn> lostMechs)
	{
		lostMechs = new List<Pawn>();
		tmpMechs.Clear();
		GetMechsInAssignedOrder(pawn, ref tmpMechs);
		int num = pawn.mechanitor.TotalBandwidth - lostBandwidth;
		int num2 = pawn.mechanitor.UsedBandwidth;
		for (int i = 0; i < tmpMechs.Count; i++)
		{
			if (num2 <= num)
			{
				break;
			}
			int num3 = Mathf.RoundToInt(tmpMechs[i].GetStatValue(StatDefOf.BandwidthCost));
			num2 -= num3;
			lostMechs.Add(tmpMechs[i]);
		}
		return tmpMechs.Count > 0;
	}

	public static bool TryConfirmBandwidthLossFromDroppingThing(Pawn pawn, Thing thing, Action confirmAct)
	{
		if (IsMechanitor(pawn) && pawn.apparel.Wearing(thing) && TryGetMechsLostFromDroppingThing(pawn, thing, out var lostMechs, out var bandwidthLost))
		{
			int totalBandwidth = pawn.mechanitor.TotalBandwidth;
			string bandwidthLower = "BandwidthLower".Translate().ToString();
			Dialog_MessageBox dialog_MessageBox = Dialog_MessageBox.CreateConfirmation("DropThingBandwidthApparel".Translate(thing.LabelShort, pawn, totalBandwidth, totalBandwidth - bandwidthLost) + (":\n\n" + lostMechs.Select((Pawn m) => m.LabelShortCap + " (" + m.GetStatValue(StatDefOf.BandwidthCost) + " " + bandwidthLower + ")").ToLineList("- ")), confirmAct);
			dialog_MessageBox.buttonBText = "CancelButton".Translate();
			Find.WindowStack.Add(dialog_MessageBox);
			return true;
		}
		return false;
	}

	public static void GetMechsInAssignedOrder(Pawn pawn, ref List<Pawn> mechs)
	{
		tmpAssignedMechs.Clear();
		for (int i = 0; i < pawn.mechanitor.controlGroups.Count; i++)
		{
			tmpAssignedMechs.AddRange(pawn.mechanitor.controlGroups[i].AssignedMechs);
		}
		tmpAssignedMechs.SortByDescending((AssignedMech m) => m.tickAssigned);
		for (int j = 0; j < tmpAssignedMechs.Count; j++)
		{
			mechs.Add(tmpAssignedMechs[j].pawn);
		}
	}

	public static void ForceDisconnectMechFromOverseer(Pawn mech)
	{
		Pawn overseer = mech.GetOverseer();
		if (overseer != null && overseer.relations.TryRemoveDirectRelation(PawnRelationDefOf.Overseer, mech))
		{
			mech.OverseerSubject.Notify_DisconnectedFromOverseer();
			SoundDefOf.DisconnectedMech.PlayOneShot(new TargetInfo(overseer));
			Messages.Message("MessageMechanitorDisconnectedFromMech".Translate(overseer, mech), new LookTargets(new Pawn[2] { mech, overseer }), MessageTypeDefOf.NeutralEvent);
		}
	}

	public static string GetMechGestationJobString(JobDriver_DoBill job, Pawn mechanitor, Bill_Mech bill)
	{
		switch (bill.State)
		{
		case FormingState.Gathering:
			if (job.AnyIngredientsQueued)
			{
				return "LoadingMechGestator".Translate() + ".";
			}
			if (job.AnyIngredientsQueued)
			{
				break;
			}
			goto case FormingState.Preparing;
		case FormingState.Preparing:
		case FormingState.Forming:
			return "InitMechGestationCycle".Translate() + ".";
		case FormingState.Formed:
			return "InitMechBirth".Translate() + ".";
		}
		Log.Error("Unknown mech gestation job state.");
		return null;
	}

	[IteratorStateMachine(typeof(_003CGetMechGizmos_003Ed__21))]
	public static IEnumerable<Gizmo> GetMechGizmos(Pawn mech)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetMechGizmos_003Ed__21(-2)
		{
			_003C_003E3__mech = mech
		};
	}

	public static bool AnyMechanitorInPlayerFaction()
	{
		foreach (Pawn allMaps_FreeColonist in PawnsFinder.AllMaps_FreeColonists)
		{
			if (IsMechanitor(allMaps_FreeColonist))
			{
				return true;
			}
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CMechsInPlayerFaction_003Ed__23))]
	public static IEnumerable<Pawn> MechsInPlayerFaction()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMechsInPlayerFaction_003Ed__23(-2);
	}

	public static bool AnyMechlinkInMap()
	{
		List<Map> maps = Find.Maps;
		for (int i = 0; i < maps.Count; i++)
		{
			if (maps[i].listerThings.ThingsOfDef(ThingDefOf.Mechlink).Count > 0)
			{
				return true;
			}
		}
		return false;
	}

	public static List<ThingDefCountClass> IngredientsFromDisassembly(ThingDef mech)
	{
		tmpIngredients.Clear();
		foreach (RecipeDef allDef in DefDatabase<RecipeDef>.AllDefs)
		{
			if (!allDef.products.NullOrEmpty() && allDef.products.Any((ThingDefCountClass x) => x.thingDef == mech))
			{
				for (int i = 0; i < allDef.ingredients.Count; i++)
				{
					ThingDef thingDef = allDef.ingredients[i].filter.AllowedThingDefs.FirstOrDefault();
					int count = Mathf.Max(1, Mathf.RoundToInt(allDef.ingredients[i].GetBaseCount() * 0.4f));
					tmpIngredients.Add(new ThingDefCountClass(thingDef, count));
				}
				break;
			}
		}
		return tmpIngredients;
	}

	public static ThingDef RechargerForMech(ThingDef mech)
	{
		foreach (ThingDef mechRecharger in MechRechargers)
		{
			if (Building_MechCharger.IsCompatibleWithCharger(mechRecharger, mech))
			{
				return mechRecharger;
			}
		}
		return null;
	}

	public static void ClearCache()
	{
		cachedMechRecipes = null;
		cachedRechargers = null;
	}

	public static bool InMechanitorCommandRange(Pawn mech, LocalTargetInfo target)
	{
		Pawn overseer = mech.GetOverseer();
		if (overseer != null)
		{
			if (mech.MapHeld != overseer.MapHeld)
			{
				return false;
			}
			if (overseer.mechanitor.CanCommandTo(target))
			{
				return true;
			}
		}
		return false;
	}

	public static bool AnyPlayerMechCanDoWork(WorkTypeDef workType, int skillRequired, out Pawn pawn)
	{
		if (!ModsConfig.BiotechActive)
		{
			pawn = null;
			return false;
		}
		List<Pawn> list = Find.CurrentMap.mapPawns.PawnsInFaction(Faction.OfPlayer);
		for (int i = 0; i < list.Count; i++)
		{
			Pawn pawn2 = list[i];
			if (pawn2.IsColonyMech && pawn2.GetOverseer() != null && pawn2.RaceProps.mechEnabledWorkTypes.Contains(workType) && pawn2.RaceProps.mechFixedSkillLevel >= skillRequired)
			{
				pawn = pawn2;
				return true;
			}
		}
		pawn = null;
		return false;
	}

	public static AcceptanceReport CanDraftMech(Pawn mech)
	{
		if (mech.IsColonyMech)
		{
			if (mech.needs.energy != null && mech.needs.energy.IsLowEnergySelfShutdown)
			{
				return "IsLowEnergySelfShutdown".Translate(mech.Named("PAWN"));
			}
			Pawn overseer = mech.GetOverseer();
			if (overseer != null)
			{
				AcceptanceReport canControlMechs = overseer.mechanitor.CanControlMechs;
				if (!canControlMechs)
				{
					return canControlMechs;
				}
				if (!overseer.mechanitor.ControlledPawns.Contains(mech))
				{
					return "MechControllerInsufficientBandwidth".Translate(overseer.Named("PAWN"));
				}
				return true;
			}
		}
		return false;
	}

	public static void Notify_PawnGotoLeftMap(Pawn pawn, Map map)
	{
		if (pawn.mechanitor == null || !pawn.IsColonist || map == null)
		{
			return;
		}
		List<Pawn> overseenPawns = pawn.mechanitor.OverseenPawns;
		tmpLeftBehind.Clear();
		for (int i = 0; i < overseenPawns.Count; i++)
		{
			if (overseenPawns[i].Map == map && overseenPawns[i].Spawned && !overseenPawns[i].Downed && overseenPawns[i].GetMechWorkMode() != MechWorkModeDefOf.Escort)
			{
				tmpLeftBehind.Add(overseenPawns[i]);
			}
		}
		if (tmpLeftBehind.Count <= 0)
		{
			return;
		}
		Dialog_MessageBox window = new Dialog_MessageBox("MechanitorLeftMapWithoutMechs".Translate(pawn.Named("PAWN"), tmpLeftBehind.Select((Pawn m) => m.LabelShortCap).ToLineList(" - ").Named("MECHS")), "Confirm".Translate(), delegate
		{
			for (int j = 0; j < tmpLeftBehind.Count; j++)
			{
				tmpLeftBehind[j].GetMechControlGroup().SetWorkMode(MechWorkModeDefOf.Escort);
			}
		}, "Cancel".Translate());
		Find.WindowStack.Add(window);
	}
}
