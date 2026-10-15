using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld;

public class LordToil_Siege : LordToil
{
	[CompilerGenerated]
	private sealed class _003Cget_Frames_003Ed__17 : IEnumerable<Frame>, IEnumerable, IEnumerator<Frame>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Frame _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public LordToil_Siege _003C_003E4__this;

		private LordToilData_Siege _003Cdata_003E5__2;

		private float _003CradSquared_003E5__3;

		private List<Thing> _003CframesList_003E5__4;

		private int _003Ci_003E5__5;

		Frame IEnumerator<Frame>.Current
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
		public _003Cget_Frames_003Ed__17(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cdata_003E5__2 = null;
			_003CframesList_003E5__4 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			LordToil_Siege lordToil_Siege = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_00ee;
			}
			_003C_003E1__state = -1;
			_003Cdata_003E5__2 = lordToil_Siege.Data;
			_003CradSquared_003E5__3 = (_003Cdata_003E5__2.baseRadius + 10f) * (_003Cdata_003E5__2.baseRadius + 10f);
			_003CframesList_003E5__4 = lordToil_Siege.Map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingFrame);
			if (_003CframesList_003E5__4.Count == 0)
			{
				return false;
			}
			_003Ci_003E5__5 = 0;
			goto IL_0100;
			IL_0100:
			if (_003Ci_003E5__5 < _003CframesList_003E5__4.Count)
			{
				Frame frame = (Frame)_003CframesList_003E5__4[_003Ci_003E5__5];
				if (frame.Faction == lordToil_Siege.lord.faction && (float)(frame.Position - _003Cdata_003E5__2.siegeCenter).LengthHorizontalSquared < _003CradSquared_003E5__3)
				{
					_003C_003E2__current = frame;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_00ee;
			}
			return false;
			IL_00ee:
			_003Ci_003E5__5++;
			goto IL_0100;
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
		IEnumerator<Frame> IEnumerable<Frame>.GetEnumerator()
		{
			_003Cget_Frames_003Ed__17 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_Frames_003Ed__17(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Frame>)this).GetEnumerator();
		}
	}

	public Dictionary<Pawn, DutyDef> rememberedDuties = new Dictionary<Pawn, DutyDef>();

	private const float BaseRadiusMin = 14f;

	private const float BaseRadiusMax = 25f;

	private static readonly FloatRange MealCountRangePerRaider = new FloatRange(1f, 3f);

	private const int StartBuildingDelay = 450;

	private static readonly FloatRange BuilderCountFraction = new FloatRange(0.25f, 0.4f);

	private const float FractionLossesToAssault = 0.4f;

	private const int InitalShellsPerCannon = 5;

	private const int ReplenishAtShells = 4;

	private const int ShellReplenishCount = 6;

	private const int ReplenishAtMeals = 5;

	private const int MealReplenishCount = 12;

	public override IntVec3 FlagLoc => Data.siegeCenter;

	private LordToilData_Siege Data => (LordToilData_Siege)data;

	private IEnumerable<Frame> Frames
	{
		[IteratorStateMachine(typeof(_003Cget_Frames_003Ed__17))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Frames_003Ed__17(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override bool ForceHighStoryDanger => true;

	public LordToil_Siege(IntVec3 siegeCenter, float blueprintPoints)
	{
		data = new LordToilData_Siege();
		Data.siegeCenter = siegeCenter;
		Data.blueprintPoints = blueprintPoints;
	}

	public override void Init()
	{
		base.Init();
		LordToilData_Siege lordToilData_Siege = Data;
		lordToilData_Siege.baseRadius = Mathf.InverseLerp(14f, 25f, (float)lord.ownedPawns.Count / 50f);
		lordToilData_Siege.baseRadius = Mathf.Clamp(lordToilData_Siege.baseRadius, 14f, 25f);
		List<Thing> list = new List<Thing>();
		foreach (Blueprint_Build item2 in SiegeBlueprintPlacer.PlaceBlueprints(lordToilData_Siege.siegeCenter, base.Map, lord.faction, lordToilData_Siege.blueprintPoints))
		{
			lordToilData_Siege.blueprints.Add(item2);
			foreach (ThingDefCountClass cost in item2.TotalMaterialCost())
			{
				Thing thing = list.FirstOrDefault((Thing t) => t.def == cost.thingDef);
				if (thing != null)
				{
					thing.stackCount += cost.count;
					continue;
				}
				Thing thing2 = ThingMaker.MakeThing(cost.thingDef);
				thing2.stackCount = cost.count;
				list.Add(thing2);
			}
			if (item2.def.entityDefToBuild is ThingDef turret)
			{
				ThingDef thingDef = TurretGunUtility.TryFindRandomShellDef(turret, allowEMP: false, allowToxGas: false, mustHarmHealth: true, lord.faction.def.techLevel, allowAntigrainWarhead: false, 250f, lord.faction);
				if (thingDef != null)
				{
					Thing thing3 = ThingMaker.MakeThing(thingDef);
					thing3.stackCount = 5;
					list.Add(thing3);
				}
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			list[i].stackCount = Mathf.CeilToInt((float)list[i].stackCount * Rand.Range(1f, 1.2f));
		}
		List<List<Thing>> list2 = new List<List<Thing>>();
		for (int j = 0; j < list.Count; j++)
		{
			while (list[j].stackCount > list[j].def.stackLimit)
			{
				int num = Mathf.CeilToInt((float)list[j].def.stackLimit * Rand.Range(0.9f, 0.999f));
				Thing thing4 = ThingMaker.MakeThing(list[j].def);
				thing4.stackCount = num;
				list[j].stackCount -= num;
				list.Add(thing4);
			}
		}
		List<Thing> list3 = new List<Thing>();
		for (int k = 0; k < list.Count; k++)
		{
			list3.Add(list[k]);
			if (k % 2 == 1 || k == list.Count - 1)
			{
				list2.Add(list3);
				list3 = new List<Thing>();
			}
		}
		List<Thing> list4 = new List<Thing>();
		int num2 = Mathf.RoundToInt(MealCountRangePerRaider.RandomInRange * (float)lord.ownedPawns.Count);
		for (int l = 0; l < num2; l++)
		{
			Thing item = ThingMaker.MakeThing(ThingDefOf.MealSurvivalPack);
			list4.Add(item);
		}
		list2.Add(list4);
		DropPodUtility.DropThingGroupsNear(lordToilData_Siege.siegeCenter, base.Map, list2);
		lordToilData_Siege.desiredBuilderFraction = BuilderCountFraction.RandomInRange;
	}

	public override void UpdateAllDuties()
	{
		LordToilData_Siege data = Data;
		if (lord.ticksInToil < 450)
		{
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				SetAsDefender(lord.ownedPawns[i]);
			}
			return;
		}
		rememberedDuties.Clear();
		int num = Mathf.RoundToInt((float)lord.ownedPawns.Count * data.desiredBuilderFraction);
		if (num <= 0)
		{
			num = 1;
		}
		int num2 = (from b in base.Map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial)
			where b.def.hasInteractionCell && b.Faction == lord.faction && b.Position.InHorDistOf(FlagLoc, data.baseRadius)
			select b).Count();
		if (num < num2)
		{
			num = num2;
		}
		int num3 = 0;
		for (int j = 0; j < lord.ownedPawns.Count; j++)
		{
			Pawn pawn = lord.ownedPawns[j];
			if (pawn.mindState.duty.def == DutyDefOf.Build)
			{
				rememberedDuties.Add(pawn, DutyDefOf.Build);
				SetAsBuilder(pawn);
				num3++;
			}
		}
		int num4 = num - num3;
		for (int k = 0; k < num4; k++)
		{
			if (lord.ownedPawns.Where((Pawn pa) => !rememberedDuties.ContainsKey(pa) && CanBeBuilder(pa)).TryRandomElement(out var result))
			{
				rememberedDuties.Add(result, DutyDefOf.Build);
				SetAsBuilder(result);
				num3++;
			}
		}
		for (int l = 0; l < lord.ownedPawns.Count; l++)
		{
			Pawn pawn2 = lord.ownedPawns[l];
			if (!rememberedDuties.ContainsKey(pawn2))
			{
				SetAsDefender(pawn2);
				rememberedDuties.Add(pawn2, DutyDefOf.Defend);
			}
		}
		if (num3 == 0)
		{
			lord.ReceiveMemo("NoBuilders");
		}
	}

	public override void Notify_PawnLost(Pawn victim, PawnLostCondition cond)
	{
		UpdateAllDuties();
		base.Notify_PawnLost(victim, cond);
	}

	public override void Notify_ConstructionFailed(Pawn pawn, Frame frame, Blueprint_Build newBlueprint)
	{
		base.Notify_ConstructionFailed(pawn, frame, newBlueprint);
		if (frame.Faction == lord.faction && newBlueprint != null)
		{
			Data.blueprints.Add(newBlueprint);
		}
	}

	private bool CanBeBuilder(Pawn p)
	{
		if (p.WorkTypeIsDisabled(WorkTypeDefOf.Construction) || p.WorkTypeIsDisabled(WorkTypeDefOf.Firefighter))
		{
			return false;
		}
		return true;
	}

	private void SetAsBuilder(Pawn p)
	{
		LordToilData_Siege lordToilData_Siege = Data;
		p.mindState.duty = new PawnDuty(DutyDefOf.Build, lordToilData_Siege.siegeCenter);
		p.mindState.duty.radius = lordToilData_Siege.baseRadius;
		int minLevel = Mathf.Max(ThingDefOf.Sandbags.constructionSkillPrerequisite, ThingDefOf.Turret_Mortar.constructionSkillPrerequisite);
		p.skills.GetSkill(SkillDefOf.Construction).EnsureMinLevelWithMargin(minLevel);
		p.workSettings.EnableAndInitialize();
		List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
		for (int i = 0; i < allDefsListForReading.Count; i++)
		{
			WorkTypeDef workTypeDef = allDefsListForReading[i];
			if (workTypeDef == WorkTypeDefOf.Construction)
			{
				p.workSettings.SetPriority(workTypeDef, 1);
			}
			else
			{
				p.workSettings.Disable(workTypeDef);
			}
		}
	}

	private void SetAsDefender(Pawn p)
	{
		LordToilData_Siege lordToilData_Siege = Data;
		p.mindState.duty = new PawnDuty(DutyDefOf.Defend, lordToilData_Siege.siegeCenter);
		p.mindState.duty.radius = lordToilData_Siege.baseRadius;
	}

	public override void LordToilTick()
	{
		base.LordToilTick();
		LordToilData_Siege lordToilData_Siege = Data;
		if (lord.ticksInToil == 450)
		{
			lord.CurLordToil.UpdateAllDuties();
		}
		if (lord.ticksInToil > 450 && lord.ticksInToil % 500 == 0)
		{
			UpdateAllDuties();
		}
		if (Find.TickManager.TicksGame % 500 != 0)
		{
			return;
		}
		if (!Frames.Where((Frame frame) => !frame.Destroyed).Any() && !lordToilData_Siege.blueprints.Where((Blueprint blue) => !blue.Destroyed).Any() && !base.Map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial).Any((Thing b) => b.Faction == lord.faction && b.def.building.buildingTags.Contains("Artillery")))
		{
			lord.ReceiveMemo("NoArtillery");
			return;
		}
		int num = GenRadial.NumCellsInRadius(20f);
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		for (int i = 0; i < num; i++)
		{
			IntVec3 c = lordToilData_Siege.siegeCenter + GenRadial.RadialPattern[i];
			if (!c.InBounds(base.Map))
			{
				continue;
			}
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int j = 0; j < thingList.Count; j++)
			{
				Thing thing = thingList[j];
				if (thing.def.IsShell && thing.def.projectileWhenLoaded?.projectile != null && thing.def.projectileWhenLoaded.projectile.damageDef.harmsHealth)
				{
					num2 += thing.stackCount;
				}
				if (thing.def == ThingDefOf.MealSurvivalPack)
				{
					num3 += thing.stackCount;
				}
				if (thing.def == ThingDefOf.ReinforcedBarrel)
				{
					num5 += thing.stackCount;
				}
				if (Find.Storyteller.difficulty.classicMortars || thing.def.building == null || !thing.def.building.IsMortar || thing.Faction != lord.faction)
				{
					continue;
				}
				CompRefuelable compRefuelable = thing.TryGetComp<CompRefuelable>();
				if (compRefuelable != null)
				{
					num4++;
					if (compRefuelable.Props.fuelFilter.Allows(ThingDefOf.ReinforcedBarrel) && compRefuelable.HasFuel)
					{
						num5++;
					}
				}
			}
		}
		if (num2 < 4)
		{
			ThingDef thingDef = TurretGunUtility.TryFindRandomShellDef(ThingDefOf.Turret_Mortar, allowEMP: false, allowToxGas: false, mustHarmHealth: true, lord.faction.def.techLevel, allowAntigrainWarhead: false, 250f, lord.faction);
			if (thingDef != null)
			{
				DropSupplies(thingDef, 6);
			}
		}
		if (num3 < 5)
		{
			DropSupplies(ThingDefOf.MealSurvivalPack, 12);
		}
		if (!Find.Storyteller.difficulty.classicMortars && num5 < num4)
		{
			DropSupplies(ThingDefOf.ReinforcedBarrel, num4 - num5);
		}
	}

	private void DropSupplies(ThingDef thingDef, int count)
	{
		List<Thing> list = new List<Thing>();
		Thing thing = ThingMaker.MakeThing(thingDef);
		thing.stackCount = count;
		list.Add(thing);
		DropPodUtility.DropThingsNear(Data.siegeCenter, base.Map, list);
	}

	public override void Cleanup()
	{
		LordToilData_Siege lordToilData_Siege = Data;
		lordToilData_Siege.blueprints.RemoveAll((Blueprint blue) => blue.Destroyed);
		for (int i = 0; i < lordToilData_Siege.blueprints.Count; i++)
		{
			lordToilData_Siege.blueprints[i].Destroy(DestroyMode.Cancel);
		}
		foreach (Frame item in Frames.ToList())
		{
			item.Destroy(DestroyMode.Cancel);
		}
		foreach (Building ownedBuilding in lord.ownedBuildings)
		{
			ownedBuilding.SetFaction(null);
		}
	}
}
