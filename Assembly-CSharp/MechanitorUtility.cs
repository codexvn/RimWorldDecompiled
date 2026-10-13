using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

// Token: 0x02000032 RID: 50
public static class MechanitorUtility
{
	// Token: 0x060000F6 RID: 246 RVA: 0x00007C76 File Offset: 0x00005E76
	public static bool IsMechanitor(Pawn pawn)
	{
		return MechanitorUtility.ShouldBeMechanitor(pawn) && pawn.mechanitor != null;
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x00007C8B File Offset: 0x00005E8B
	public static bool ShouldBeMechanitor(Pawn pawn)
	{
		return ModsConfig.BiotechActive && pawn.Faction.IsPlayerSafe() && pawn.health.hediffSet.HasHediff(HediffDefOf.MechlinkImplant, false);
	}

	// Token: 0x060000F8 RID: 248 RVA: 0x00007CB9 File Offset: 0x00005EB9
	public static bool IsPlayerOverseerSubject(Pawn pawn)
	{
		return pawn.Faction.IsPlayerSafe() && pawn.OverseerSubject != null;
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x00007CD3 File Offset: 0x00005ED3
	public static MechWorkModeDef GetMechWorkMode(this Pawn pawn)
	{
		MechanitorControlGroup mechControlGroup = pawn.GetMechControlGroup();
		if (mechControlGroup == null)
		{
			return null;
		}
		return mechControlGroup.WorkMode;
	}

	// Token: 0x060000FA RID: 250 RVA: 0x00007CE6 File Offset: 0x00005EE6
	public static MechanitorControlGroup GetMechControlGroup(this Pawn pawn)
	{
		Pawn overseer = pawn.GetOverseer();
		if (overseer == null)
		{
			return null;
		}
		Pawn_MechanitorTracker mechanitor = overseer.mechanitor;
		if (mechanitor == null)
		{
			return null;
		}
		return mechanitor.GetControlGroup(pawn);
	}

	// Token: 0x060000FB RID: 251 RVA: 0x00007D05 File Offset: 0x00005F05
	public static Pawn GetOverseer(this Pawn pawn)
	{
		if (!ModsConfig.BiotechActive)
		{
			return null;
		}
		Pawn_RelationsTracker relations = pawn.relations;
		if (relations == null)
		{
			return null;
		}
		return relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Overseer, null);
	}

	// Token: 0x060000FC RID: 252 RVA: 0x00007D27 File Offset: 0x00005F27
	public static bool IsGestating(this Pawn pawn)
	{
		return pawn.ParentHolder is Building_MechGestator;
	}

	// Token: 0x060000FD RID: 253 RVA: 0x00007D38 File Offset: 0x00005F38
	public static AcceptanceReport CanControlMech(Pawn pawn, Pawn mech)
	{
		if (pawn.mechanitor == null || !mech.IsColonyMech || mech.Downed || mech.Dead || mech.IsAttacking())
		{
			return false;
		}
		if (!MechanitorUtility.EverControllable(mech))
		{
			return "CannotControlMechNeverControllable".Translate();
		}
		if (mech.GetOverseer() == pawn)
		{
			return "CannotControlMechAlreadyControlled".Translate(pawn.LabelShort);
		}
		float num = (float)(pawn.mechanitor.TotalBandwidth - pawn.mechanitor.UsedBandwidth);
		float statValue = mech.GetStatValue(StatDefOf.BandwidthCost, true, -1);
		if (num < statValue)
		{
			return "CannotControlMechNotEnoughBandwidth".Translate();
		}
		return true;
	}

	// Token: 0x060000FE RID: 254 RVA: 0x00007DEF File Offset: 0x00005FEF
	public static bool EverControllable(Pawn mech)
	{
		return mech.OverseerSubject != null;
	}

	// Token: 0x060000FF RID: 255 RVA: 0x00007DFC File Offset: 0x00005FFC
	public static bool IsColonyMechRequiringMechanitor(this Pawn mech)
	{
		if (!mech.IsColonyMech)
		{
			return false;
		}
		CompOverseerSubject overseerSubject = mech.OverseerSubject;
		return overseerSubject != null && overseerSubject.State != OverseerSubjectState.Overseen;
	}

	// Token: 0x06000100 RID: 256 RVA: 0x00007E2C File Offset: 0x0000602C
	public static void Notify_MechlinkQuestRewardAvailable(Quest quest, LookTargets lookTargets = null)
	{
		if (!Find.History.mechlinkEverAvailable)
		{
			List<Pawn> allMaps_FreeColonists = PawnsFinder.AllMaps_FreeColonists;
			bool flag = false;
			for (int i = 0; i < allMaps_FreeColonists.Count; i++)
			{
				if (MechanitorUtility.IsMechanitor(allMaps_FreeColonists[i]))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelMechlinkAvailable".Translate(), "LetterMechlinkAvailable".Translate(quest.name), LetterDefOf.PositiveEvent, lookTargets, null, quest, null, null, 0, true);
			}
			Find.History.Notify_MechlinkAvailable();
		}
	}

	// Token: 0x06000101 RID: 257 RVA: 0x00007EB4 File Offset: 0x000060B4
	public static bool TryGetBandwidthLostFromDroppingThing(Pawn pawn, Thing thing, out int bandwidthLost)
	{
		bandwidthLost = -1;
		if (thing.def.equippedStatOffsets.NullOrEmpty<StatModifier>())
		{
			return false;
		}
		bandwidthLost = Mathf.RoundToInt(thing.def.equippedStatOffsets.GetStatOffsetFromList(StatDefOf.MechBandwidth));
		if (bandwidthLost <= 0)
		{
			return false;
		}
		int num = pawn.mechanitor.TotalBandwidth - pawn.mechanitor.UsedBandwidth;
		return bandwidthLost > num;
	}

	// Token: 0x06000102 RID: 258 RVA: 0x00007F1B File Offset: 0x0000611B
	public static bool TryGetMechsLostFromDroppingThing(Pawn pawn, Thing thing, out List<Pawn> lostMechs, out int bandwidthLost)
	{
		if (!MechanitorUtility.TryGetBandwidthLostFromDroppingThing(pawn, thing, out bandwidthLost))
		{
			lostMechs = null;
			return false;
		}
		return MechanitorUtility.TryGetMechsLostFromBandwidthReduction(pawn, bandwidthLost, out lostMechs);
	}

	// Token: 0x06000103 RID: 259 RVA: 0x00007F38 File Offset: 0x00006138
	public static bool TryGetMechsLostFromBandwidthReduction(Pawn pawn, int lostBandwidth, out List<Pawn> lostMechs)
	{
		lostMechs = new List<Pawn>();
		MechanitorUtility.tmpMechs.Clear();
		MechanitorUtility.GetMechsInAssignedOrder(pawn, ref MechanitorUtility.tmpMechs);
		int num = pawn.mechanitor.TotalBandwidth - lostBandwidth;
		int num2 = pawn.mechanitor.UsedBandwidth;
		int num3 = 0;
		while (num3 < MechanitorUtility.tmpMechs.Count && num2 > num)
		{
			int num4 = Mathf.RoundToInt(MechanitorUtility.tmpMechs[num3].GetStatValue(StatDefOf.BandwidthCost, true, -1));
			num2 -= num4;
			lostMechs.Add(MechanitorUtility.tmpMechs[num3]);
			num3++;
		}
		return MechanitorUtility.tmpMechs.Count > 0;
	}

	// Token: 0x06000104 RID: 260 RVA: 0x00007FD4 File Offset: 0x000061D4
	public static bool TryConfirmBandwidthLossFromDroppingThing(Pawn pawn, Thing thing, Action confirmAct)
	{
		List<Pawn> source;
		int num;
		if (MechanitorUtility.IsMechanitor(pawn) && pawn.apparel.Wearing(thing) && MechanitorUtility.TryGetMechsLostFromDroppingThing(pawn, thing, out source, out num))
		{
			int totalBandwidth = pawn.mechanitor.TotalBandwidth;
			string bandwidthLower = "BandwidthLower".Translate().ToString();
			Dialog_MessageBox dialog_MessageBox = Dialog_MessageBox.CreateConfirmation("DropThingBandwidthApparel".Translate(thing.LabelShort, pawn, totalBandwidth, totalBandwidth - num) + (":\n\n" + (from m in source
			select string.Concat(new string[]
			{
				m.LabelShortCap,
				" (",
				m.GetStatValue(StatDefOf.BandwidthCost, true, -1).ToString(),
				" ",
				bandwidthLower,
				")"
			})).ToLineList("- ", false)), confirmAct, false, null, WindowLayer.Dialog);
			dialog_MessageBox.buttonBText = "CancelButton".Translate();
			Find.WindowStack.Add(dialog_MessageBox);
			return true;
		}
		return false;
	}

	// Token: 0x06000105 RID: 261 RVA: 0x000080C4 File Offset: 0x000062C4
	public static void GetMechsInAssignedOrder(Pawn pawn, ref List<Pawn> mechs)
	{
		MechanitorUtility.tmpAssignedMechs.Clear();
		for (int i = 0; i < pawn.mechanitor.controlGroups.Count; i++)
		{
			MechanitorUtility.tmpAssignedMechs.AddRange(pawn.mechanitor.controlGroups[i].AssignedMechs);
		}
		MechanitorUtility.tmpAssignedMechs.SortByDescending((AssignedMech m) => m.tickAssigned);
		for (int j = 0; j < MechanitorUtility.tmpAssignedMechs.Count; j++)
		{
			mechs.Add(MechanitorUtility.tmpAssignedMechs[j].pawn);
		}
	}

	// Token: 0x06000106 RID: 262 RVA: 0x0000816C File Offset: 0x0000636C
	public static void ForceDisconnectMechFromOverseer(Pawn mech)
	{
		Pawn overseer = mech.GetOverseer();
		if (overseer != null && overseer.relations.TryRemoveDirectRelation(PawnRelationDefOf.Overseer, mech))
		{
			mech.OverseerSubject.Notify_DisconnectedFromOverseer();
			SoundDefOf.DisconnectedMech.PlayOneShot(new TargetInfo(overseer));
			Messages.Message("MessageMechanitorDisconnectedFromMech".Translate(overseer, mech), new LookTargets(new Pawn[]
			{
				mech,
				overseer
			}), MessageTypeDefOf.NeutralEvent, true);
		}
	}

	// Token: 0x06000107 RID: 263 RVA: 0x000081F0 File Offset: 0x000063F0
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
				goto IL_7D;
			}
			break;
		case FormingState.Preparing:
		case FormingState.Forming:
			break;
		case FormingState.Formed:
			return "InitMechBirth".Translate() + ".";
		default:
			goto IL_7D;
		}
		return "InitMechGestationCycle".Translate() + ".";
		IL_7D:
		Log.Error("Unknown mech gestation job state.");
		return null;
	}

	// Token: 0x06000108 RID: 264 RVA: 0x00008285 File Offset: 0x00006485
	public static IEnumerable<Gizmo> GetMechGizmos(Pawn mech)
	{
		if (mech.IsColonyMech && MechanitorUtility.EverControllable(mech))
		{
			Pawn overseer = mech.GetOverseer();
			bool flag = overseer == null || !overseer.Spawned;
			yield return new Command_Action
			{
				defaultLabel = "CommandSelectOverseer".Translate(),
				defaultDesc = (flag ? "CommandSelectOverseerDisabledDesc".Translate() : "CommandSelectOverseerDesc".Translate()),
				icon = MechanitorUtility.SelectOverseerIcon.Texture,
				action = delegate()
				{
					Find.Selector.ClearSelection();
					Find.Selector.Select(overseer, true, true);
				},
				onHover = delegate()
				{
					if (overseer != null)
					{
						GenDraw.DrawArrowPointingAt(overseer.TrueCenter(), false);
					}
				},
				Disabled = flag
			};
			if (Find.Selector.IsSelected(mech))
			{
				MechanitorControlGroup mechControlGroup = mech.GetMechControlGroup();
				if (mechControlGroup != null)
				{
					yield return new MechanitorControlGroupGizmo(mechControlGroup);
				}
			}
			Gizmo activeMechShieldGizmo = RemoteShieldUtility.GetActiveMechShieldGizmo(mech);
			if (activeMechShieldGizmo != null)
			{
				yield return activeMechShieldGizmo;
			}
		}
		else if (DebugSettings.ShowDevGizmos && mech.Faction != Faction.OfPlayer)
		{
			yield return new Command_Action
			{
				defaultLabel = "DEV: Recruit",
				action = delegate()
				{
					mech.SetFaction(Faction.OfPlayer, null);
				}
			};
			yield return new Command_Action
			{
				defaultLabel = "DEV: Kill",
				action = delegate()
				{
					mech.Kill(null, null);
				}
			};
		}
		yield break;
	}

	// Token: 0x06000109 RID: 265 RVA: 0x00008298 File Offset: 0x00006498
	public static bool AnyMechanitorInPlayerFaction()
	{
		using (List<Pawn>.Enumerator enumerator = PawnsFinder.AllMaps_FreeColonists.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (MechanitorUtility.IsMechanitor(enumerator.Current))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600010A RID: 266 RVA: 0x000082F0 File Offset: 0x000064F0
	public static IEnumerable<Pawn> MechsInPlayerFaction()
	{
		List<Map> maps = Find.Maps;
		int num;
		for (int i = 0; i < maps.Count; i = num + 1)
		{
			List<Pawn> pawns = maps[i].mapPawns.PawnsInFaction(Faction.OfPlayer);
			for (int j = 0; j < pawns.Count; j = num + 1)
			{
				if (pawns[j].IsColonyMech)
				{
					yield return pawns[j];
				}
				num = j;
			}
			pawns = null;
			num = i;
		}
		yield break;
	}

	// Token: 0x0600010B RID: 267 RVA: 0x000082FC File Offset: 0x000064FC
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

	// Token: 0x0600010C RID: 268 RVA: 0x00008344 File Offset: 0x00006544
	public static List<ThingDefCountClass> IngredientsFromDisassembly(ThingDef mech)
	{
		MechanitorUtility.tmpIngredients.Clear();
		Predicate<ThingDefCountClass> <>9__0;
		foreach (RecipeDef recipeDef in DefDatabase<RecipeDef>.AllDefs)
		{
			if (!recipeDef.products.NullOrEmpty<ThingDefCountClass>())
			{
				List<ThingDefCountClass> products = recipeDef.products;
				Predicate<ThingDefCountClass> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((ThingDefCountClass x) => x.thingDef == mech));
				}
				if (products.Any(predicate))
				{
					for (int i = 0; i < recipeDef.ingredients.Count; i++)
					{
						ThingDef thingDef = recipeDef.ingredients[i].filter.AllowedThingDefs.FirstOrDefault<ThingDef>();
						int count = Mathf.Max(1, Mathf.RoundToInt(recipeDef.ingredients[i].GetBaseCount() * 0.4f));
						MechanitorUtility.tmpIngredients.Add(new ThingDefCountClass(thingDef, count));
					}
					break;
				}
			}
		}
		return MechanitorUtility.tmpIngredients;
	}

	// Token: 0x17000027 RID: 39
	// (get) Token: 0x0600010D RID: 269 RVA: 0x0000845C File Offset: 0x0000665C
	public static List<ThingDef> MechRechargers
	{
		get
		{
			if (MechanitorUtility.cachedRechargers == null)
			{
				MechanitorUtility.cachedRechargers = (from d in DefDatabase<ThingDef>.AllDefs
				where d.IsMechRecharger
				select d).ToList<ThingDef>();
			}
			return MechanitorUtility.cachedRechargers;
		}
	}

	// Token: 0x0600010E RID: 270 RVA: 0x000084A8 File Offset: 0x000066A8
	public static ThingDef RechargerForMech(ThingDef mech)
	{
		foreach (ThingDef thingDef in MechanitorUtility.MechRechargers)
		{
			if (Building_MechCharger.IsCompatibleWithCharger(thingDef, mech))
			{
				return thingDef;
			}
		}
		return null;
	}

	// Token: 0x17000028 RID: 40
	// (get) Token: 0x0600010F RID: 271 RVA: 0x00008504 File Offset: 0x00006704
	public static List<RecipeDef> MechRecipes
	{
		get
		{
			if (MechanitorUtility.cachedMechRecipes == null)
			{
				MechanitorUtility.cachedMechRecipes = DefDatabase<RecipeDef>.AllDefs.Where(delegate(RecipeDef r)
				{
					ThingDef producedThingDef = r.ProducedThingDef;
					if (producedThingDef == null)
					{
						return false;
					}
					RaceProperties race = producedThingDef.race;
					bool? flag = (race != null) ? new bool?(race.IsMechanoid) : null;
					bool flag2 = true;
					return flag.GetValueOrDefault() == flag2 & flag != null;
				}).ToList<RecipeDef>();
			}
			return MechanitorUtility.cachedMechRecipes;
		}
	}

	// Token: 0x06000110 RID: 272 RVA: 0x00008550 File Offset: 0x00006750
	public static void ClearCache()
	{
		MechanitorUtility.cachedMechRecipes = null;
		MechanitorUtility.cachedRechargers = null;
	}

	// Token: 0x06000111 RID: 273 RVA: 0x00008560 File Offset: 0x00006760
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

	// Token: 0x06000112 RID: 274 RVA: 0x00008598 File Offset: 0x00006798
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

	// Token: 0x06000113 RID: 275 RVA: 0x0000861C File Offset: 0x0000681C
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

	// Token: 0x06000114 RID: 276 RVA: 0x000086CC File Offset: 0x000068CC
	public static void Notify_PawnGotoLeftMap(Pawn pawn, Map map)
	{
		if (pawn.mechanitor == null || !pawn.IsColonist || map == null)
		{
			return;
		}
		List<Pawn> overseenPawns = pawn.mechanitor.OverseenPawns;
		MechanitorUtility.tmpLeftBehind.Clear();
		for (int i = 0; i < overseenPawns.Count; i++)
		{
			if (overseenPawns[i].Map == map && overseenPawns[i].Spawned && !overseenPawns[i].Downed && overseenPawns[i].GetMechWorkMode() != MechWorkModeDefOf.Escort)
			{
				MechanitorUtility.tmpLeftBehind.Add(overseenPawns[i]);
			}
		}
		if (MechanitorUtility.tmpLeftBehind.Count <= 0)
		{
			return;
		}
		Dialog_MessageBox window = new Dialog_MessageBox("MechanitorLeftMapWithoutMechs".Translate(pawn.Named("PAWN"), (from m in MechanitorUtility.tmpLeftBehind
		select m.LabelShortCap).ToLineList(" - ", false).Named("MECHS")), "Confirm".Translate(), delegate()
		{
			for (int j = 0; j < MechanitorUtility.tmpLeftBehind.Count; j++)
			{
				MechanitorUtility.tmpLeftBehind[j].GetMechControlGroup().SetWorkMode(MechWorkModeDefOf.Escort);
			}
		}, "Cancel".Translate(), null, null, false, null, null, WindowLayer.Dialog);
		Find.WindowStack.Add(window);
	}

	// Token: 0x0400006E RID: 110
	private static CachedTexture SelectOverseerIcon = new CachedTexture("UI/Icons/SelectOverseer");

	// Token: 0x0400006F RID: 111
	private static List<Pawn> tmpMechs = new List<Pawn>();

	// Token: 0x04000070 RID: 112
	private static List<AssignedMech> tmpAssignedMechs = new List<AssignedMech>();

	// Token: 0x04000071 RID: 113
	private static List<ThingDefCountClass> tmpIngredients = new List<ThingDefCountClass>();

	// Token: 0x04000072 RID: 114
	private static List<ThingDef> cachedRechargers;

	// Token: 0x04000073 RID: 115
	private static List<RecipeDef> cachedMechRecipes;

	// Token: 0x04000074 RID: 116
	private static readonly List<Pawn> tmpLeftBehind = new List<Pawn>();
}
