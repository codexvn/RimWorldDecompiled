using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld;

[StaticConstructorOnStartup]
public class Frame : Building, IThingHolder, IConstructible, IStorageGroupMember, IStoreSettingsParent, IHaulEnroute, ILoadReferenceable
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__46 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Frame _003C_003E4__this;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

		private IEnumerator<Command> _003C_003E7__wrap2;

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
		public _003CGetGizmos_003Ed__46(int _003C_003E1__state)
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
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			case -5:
			case 5:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			case -6:
			case 6:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally4();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Frame frame = _003C_003E4__this;
				Command command;
				Gizmo selectMonumentMarkerGizmo;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Building)frame).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0084;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0084;
				case 2:
					_003C_003E1__state = -4;
					goto IL_00f1;
				case 3:
					_003C_003E1__state = -1;
					goto IL_0131;
				case 4:
					_003C_003E1__state = -1;
					goto IL_0182;
				case 5:
					_003C_003E1__state = -5;
					goto IL_01e2;
				case 6:
					{
						_003C_003E1__state = -6;
						goto IL_024c;
					}
					IL_0084:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (frame.DrawStorageTab)
					{
						_003C_003E7__wrap1 = StorageSettingsClipboard.CopyPasteGizmosFor(frame.GetStoreSettings()).GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_00f1;
					}
					goto IL_010b;
					IL_0131:
					command = BuildCopyCommandUtility.BuildCopyCommand(frame.def.entityDefToBuild, frame.Stuff, frame.StyleSourcePrecept as Precept_Building, frame.StyleDef, styleOverridden: true, frame.glowerColorOverride);
					if (command != null)
					{
						_003C_003E2__current = command;
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_0182;
					IL_0182:
					if (frame.Faction != Faction.OfPlayer)
					{
						break;
					}
					_003C_003E7__wrap2 = BuildRelatedCommandUtility.RelatedBuildCommands(frame.def.entityDefToBuild).GetEnumerator();
					_003C_003E1__state = -5;
					goto IL_01e2;
					IL_00f1:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = null;
					goto IL_010b;
					IL_024c:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current3 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current3;
						_003C_003E1__state = 6;
						return true;
					}
					_003C_003Em__Finally4();
					_003C_003E7__wrap1 = null;
					break;
					IL_01e2:
					if (_003C_003E7__wrap2.MoveNext())
					{
						Command current4 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current4;
						_003C_003E1__state = 5;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap2 = null;
					if (frame.StorageGroupTag.NullOrEmpty())
					{
						break;
					}
					_003C_003E7__wrap1 = StorageGroupUtility.StorageGroupMemberGizmos(frame).GetEnumerator();
					_003C_003E1__state = -6;
					goto IL_024c;
					IL_010b:
					selectMonumentMarkerGizmo = QuestUtility.GetSelectMonumentMarkerGizmo(frame);
					if (selectMonumentMarkerGizmo != null)
					{
						_003C_003E2__current = selectMonumentMarkerGizmo;
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_0131;
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

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		private void _003C_003Em__Finally4()
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
			_003CGetGizmos_003Ed__46 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__46(0)
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

	[CompilerGenerated]
	private sealed class _003CGetInspectTabs_003Ed__74 : IEnumerable<InspectTabBase>, IEnumerable, IEnumerator<InspectTabBase>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private InspectTabBase _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Frame _003C_003E4__this;

		private List<InspectTabBase>.Enumerator _003C_003E7__wrap1;

		InspectTabBase IEnumerator<InspectTabBase>.Current
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
		public _003CGetInspectTabs_003Ed__74(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<InspectTabBase>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Frame frame = _003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -3;
				}
				else
				{
					_003C_003E1__state = -1;
					if (frame.StorageGroupTag.NullOrEmpty() || frame.BuildDef.inspectorTabsResolved == null)
					{
						goto IL_00a2;
					}
					_003C_003E7__wrap1 = frame.BuildDef.inspectorTabsResolved.GetEnumerator();
					_003C_003E1__state = -3;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					InspectTabBase current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<InspectTabBase>.Enumerator);
				goto IL_00a2;
				IL_00a2:
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<InspectTabBase> IEnumerable<InspectTabBase>.GetEnumerator()
		{
			_003CGetInspectTabs_003Ed__74 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetInspectTabs_003Ed__74(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<InspectTabBase>)this).GetEnumerator();
		}
	}

	public ThingOwner resourceContainer;

	public float workDone;

	public ColorInt? glowerColorOverride;

	public StorageGroup storageGroup;

	public StorageSettings storageSettings;

	private Material cachedCornerMat;

	private Material cachedTileMat;

	protected const float UnderfieldOverdrawFactor = 1.15f;

	private const int LongConstructionProjectThreshold = 9500;

	private static readonly Material UnderfieldMat = MaterialPool.MatFrom("Things/Building/BuildingFrame/Underfield", ShaderDatabase.Transparent);

	private static readonly Texture2D CornerTex = ContentFinder<Texture2D>.Get("Things/Building/BuildingFrame/Corner");

	private static readonly Texture2D TileTex = ContentFinder<Texture2D>.Get("Things/Building/BuildingFrame/Tile");

	public ThingDef BuildDef => def.entityDefToBuild as ThingDef;

	public float WorkToBuild => def.entityDefToBuild.GetStatValueAbstract(StatDefOf.WorkToBuild, base.Stuff);

	public float WorkLeft => WorkToBuild - workDone;

	public float PercentComplete => workDone / WorkToBuild;

	public override string Label => LabelEntityToBuild + "FrameLabelExtra".Translate();

	public string LabelEntityToBuild
	{
		get
		{
			string text = def.entityDefToBuild.label;
			if (base.StyleSourcePrecept != null)
			{
				text = base.StyleSourcePrecept.TransformThingLabel(text);
			}
			if (base.Stuff != null)
			{
				return "ThingMadeOfStuffLabel".Translate(base.Stuff.LabelAsStuff, text);
			}
			return text;
		}
	}

	public override Color DrawColor
	{
		get
		{
			if (!def.MadeFromStuff)
			{
				List<ThingDefCountClass> costList = def.entityDefToBuild.CostList;
				if (costList != null)
				{
					for (int i = 0; i < costList.Count; i++)
					{
						ThingDef thingDef = costList[i].thingDef;
						if (thingDef.IsStuff && thingDef.stuffProps.color != Color.white)
						{
							return def.GetColorForStuff(thingDef);
						}
					}
				}
				return new Color(0.6f, 0.6f, 0.6f);
			}
			return base.DrawColor;
		}
	}

	public EffecterDef ConstructionEffect
	{
		get
		{
			if (base.Stuff?.stuffProps.constructEffect != null)
			{
				return base.Stuff.stuffProps.constructEffect;
			}
			if (def.entityDefToBuild.constructEffect != null)
			{
				return def.entityDefToBuild.constructEffect;
			}
			return EffecterDefOf.ConstructMetal;
		}
	}

	private Material CornerMat
	{
		get
		{
			if (cachedCornerMat == null)
			{
				cachedCornerMat = MaterialPool.MatFrom(CornerTex, ShaderDatabase.MetaOverlay, DrawColor);
			}
			return cachedCornerMat;
		}
	}

	private Material TileMat
	{
		get
		{
			if (cachedTileMat == null)
			{
				cachedTileMat = MaterialPool.MatFrom(TileTex, ShaderDatabase.MetaOverlay, DrawColor);
			}
			return cachedTileMat;
		}
	}

	StorageGroup IStorageGroupMember.Group
	{
		get
		{
			return storageGroup;
		}
		set
		{
			storageGroup = value;
		}
	}

	Map IStorageGroupMember.Map => base.Map;

	StorageSettings IStorageGroupMember.StoreSettings => GetStoreSettings();

	StorageSettings IStorageGroupMember.ParentStoreSettings => GetParentStoreSettings();

	StorageSettings IStorageGroupMember.ThingStoreSettings => storageSettings;

	public string StorageGroupTag => BuildDef?.building?.storageGroupTag;

	bool IStorageGroupMember.DrawConnectionOverlay => !StorageGroupTag.NullOrEmpty();

	bool IStorageGroupMember.ShowRenameButton
	{
		get
		{
			if (!StorageGroupTag.NullOrEmpty())
			{
				return base.Faction == Faction.OfPlayer;
			}
			return false;
		}
	}

	public bool DrawStorageTab => !StorageGroupTag.NullOrEmpty();

	bool IStoreSettingsParent.StorageTabVisible => !StorageGroupTag.NullOrEmpty();

	public Frame()
	{
		resourceContainer = new ThingOwner<Thing>(this, oneStackOnly: false);
	}

	public ThingOwner GetDirectlyHeldThings()
	{
		return resourceContainer;
	}

	public void GetChildHolders(List<IThingHolder> outChildren)
	{
		ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref workDone, "workDone", 0f);
		Scribe_Deep.Look(ref resourceContainer, "resourceContainer", this);
		Scribe_Values.Look(ref glowerColorOverride, "glowerColorOverride", null);
		Scribe_References.Look(ref storageGroup, "storageGroup");
		Scribe_Deep.Look(ref storageSettings, "storageSettings");
	}

	public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
	{
		bool spawned = base.Spawned;
		Map map = base.Map;
		base.Destroy(mode);
		if (spawned && ThingUtility.CheckAutoRebuildOnDestroyed(this, mode, map, def.entityDefToBuild) is Blueprint_Storage blueprint_Storage)
		{
			blueprint_Storage.SetStorageGroup(storageGroup);
			blueprint_Storage.settings = new StorageSettings();
			blueprint_Storage.settings.CopyFrom(storageSettings);
		}
	}

	public ThingDef EntityToBuildStuff()
	{
		return base.Stuff;
	}

	public List<ThingDefCountClass> TotalMaterialCost()
	{
		return def.entityDefToBuild.CostListAdjusted(base.Stuff);
	}

	public bool IsCompleted()
	{
		foreach (ThingDefCountClass item in TotalMaterialCost())
		{
			if (item.count - resourceContainer.TotalStackCountOfDef(item.thingDef) > 0)
			{
				return false;
			}
		}
		return true;
	}

	public int ThingCountNeeded(ThingDef stuff)
	{
		foreach (ThingDefCountClass item in TotalMaterialCost())
		{
			if (item.thingDef == stuff)
			{
				return item.count - resourceContainer.TotalStackCountOfDef(item.thingDef);
			}
		}
		return 0;
	}

	public int ThingCountNeededWithEnroute(ThingDef stuff, Pawn excludeEnrouteFor = null)
	{
		foreach (ThingDefCountClass item in TotalMaterialCost())
		{
			if (item.thingDef == stuff)
			{
				int num = item.count - resourceContainer.TotalStackCountOfDef(item.thingDef);
				int num2 = num - base.Map.enrouteManager.GetEnroute(this, item.thingDef, excludeEnrouteFor);
				if (num2 < 0)
				{
					Log.Error($"amount of stuff for {LabelShort} is negative: {stuff.label} {num2}.");
				}
				if (num2 > num)
				{
					Log.Error($"amount of stuff for {LabelShort} was greater than could be needed: {stuff.label} {num2} (needs {num}).");
				}
				return Mathf.Min(Mathf.Max(num2, 0), num);
			}
		}
		return 0;
	}

	public void CompleteConstruction(Pawn worker)
	{
		if (worker.Faction != null)
		{
			QuestUtility.SendQuestTargetSignals(worker.Faction.questTags, "BuiltBuilding", this.Named("SUBJECT"));
		}
		List<CompHasSources> list = new List<CompHasSources>();
		for (int i = 0; i < resourceContainer.Count; i++)
		{
			CompHasSources compHasSources = resourceContainer[i].TryGetComp<CompHasSources>();
			if (compHasSources != null)
			{
				list.Add(compHasSources);
			}
		}
		resourceContainer.ClearAndDestroyContents();
		Map map = base.Map;
		bool flag = Find.Selector.IsSelected(this);
		Destroy();
		if (this.GetStatValue(StatDefOf.WorkToBuild) > 150f && def.entityDefToBuild is ThingDef { category: ThingCategory.Building })
		{
			SoundDefOf.Building_Complete.PlayOneShot(new TargetInfo(base.Position, map));
		}
		ThingDef thingDef2 = def.entityDefToBuild as ThingDef;
		Thing thing = null;
		if (thingDef2 != null)
		{
			thing = ThingMaker.MakeThing(thingDef2, base.Stuff);
			thing.SetFactionDirect(base.Faction);
			CompQuality compQuality = thing.TryGetComp<CompQuality>();
			if (compQuality != null)
			{
				QualityCategory q = QualityUtility.GenerateQualityCreatedByPawn(worker, SkillDefOf.Construction);
				compQuality.SetQuality(q, ArtGenerationContext.Colony);
				QualityUtility.SendCraftNotification(thing, worker);
			}
			CompArt compArt = thing.TryGetComp<CompArt>();
			if (compArt != null)
			{
				if (compQuality == null)
				{
					compArt.InitializeArt(ArtGenerationContext.Colony);
				}
				compArt.JustCreatedBy(worker);
			}
			CompHasSources compHasSources2 = thing.TryGetComp<CompHasSources>();
			if (compHasSources2 != null && !list.NullOrEmpty())
			{
				for (int j = 0; j < list.Count; j++)
				{
					list[j].TransferSourcesTo(compHasSources2);
				}
			}
			if (GetIdeoForStyle(worker) != null)
			{
				thing.StyleDef = StyleDef;
			}
			thing.HitPoints = Mathf.CeilToInt((float)HitPoints / (float)base.MaxHitPoints * (float)thing.MaxHitPoints);
			GenSpawn.Spawn(thing, base.Position, map, base.Rotation, WipeMode.FullRefund);
			if (thing is Building building)
			{
				worker.GetLord()?.AddBuilding(building);
				building.StyleSourcePrecept = base.StyleSourcePrecept;
			}
			if (thing is IStorageGroupMember storageGroupMember)
			{
				storageGroupMember.SetStorageGroup(storageGroup);
				storageGroupMember.ThingStoreSettings.CopyFrom(storageSettings);
			}
			this.SetStorageGroup(null, removeIfEmpty: false);
			if (thingDef2 != null)
			{
				Color? ideoColorForBuilding = IdeoUtility.GetIdeoColorForBuilding(thingDef2, base.Faction);
				if (ideoColorForBuilding.HasValue)
				{
					thing.SetColor(ideoColorForBuilding.Value);
				}
			}
			if (overrideGraphicIndex.HasValue)
			{
				thing.overrideGraphicIndex = overrideGraphicIndex;
			}
		}
		else
		{
			TerrainDef terrainDef = map.terrainGrid.TerrainAt(base.Position);
			TerrainDef terrainDef2 = (TerrainDef)def.entityDefToBuild;
			if (terrainDef.tempTerrain != null && terrainDef.tempTerrain.replaceableByBridge && terrainDef2.bridge)
			{
				map.terrainGrid.RemoveTempTerrain(base.Position);
			}
			if (terrainDef2.isFoundation)
			{
				map.terrainGrid.SetFoundation(base.Position, terrainDef2);
			}
			else if (terrainDef2.temporary)
			{
				map.terrainGrid.SetTempTerrain(base.Position, terrainDef2);
			}
			else
			{
				map.terrainGrid.SetTerrain(base.Position, terrainDef2);
			}
			FilthMaker.RemoveAllFilth(base.Position, map);
		}
		worker.records.Increment(RecordDefOf.ThingsConstructed);
		if (thing != null && thing.GetStatValue(StatDefOf.WorkToBuild) >= 9500f)
		{
			TaleRecorder.RecordTale(TaleDefOf.CompletedLongConstructionProject, worker, thing.def);
		}
		if (thing != null && flag)
		{
			Find.Selector.Select(thing, playSound: false, forceDesignatorDeselect: false);
		}
		if (glowerColorOverride.HasValue)
		{
			CompGlower compGlower = thing?.TryGetComp<CompGlower>();
			if (compGlower != null)
			{
				compGlower.GlowColor = glowerColorOverride.Value;
			}
		}
		Lord lord = worker.GetLord();
		if (lord != null && thing is Building building2)
		{
			lord.Notify_ConstructionCompleted(worker, building2);
		}
	}

	private Ideo GetIdeoForStyle(Pawn worker)
	{
		if (worker.Ideo != null)
		{
			return worker.Ideo;
		}
		if (ModsConfig.BiotechActive && worker.IsColonyMech)
		{
			Pawn overseer = worker.GetOverseer();
			if (overseer?.Ideo != null)
			{
				return overseer.Ideo;
			}
		}
		return null;
	}

	public void FailConstruction(Pawn worker)
	{
		Map map = base.Map;
		Destroy(DestroyMode.FailConstruction);
		Blueprint_Build blueprint_Build = null;
		if (def.entityDefToBuild.blueprintDef != null)
		{
			blueprint_Build = (Blueprint_Build)ThingMaker.MakeThing(def.entityDefToBuild.blueprintDef);
			blueprint_Build.stuffToUse = base.Stuff;
			blueprint_Build.SetFactionDirect(base.Faction);
			blueprint_Build.InheritStyle(base.StyleSourcePrecept, StyleDef);
			if (blueprint_Build is IStorageGroupMember storageGroupMember)
			{
				storageGroupMember.Group = storageGroup;
			}
			if (blueprint_Build is Blueprint_Storage blueprint_Storage && storageSettings != null)
			{
				blueprint_Storage.SetStorageGroup(storageGroup);
				blueprint_Storage.settings = new StorageSettings();
				blueprint_Storage.settings.CopyFrom(storageSettings);
			}
			GenSpawn.Spawn(blueprint_Build, base.Position, map, base.Rotation, WipeMode.FullRefund);
		}
		worker.GetLord()?.Notify_ConstructionFailed(worker, this, blueprint_Build);
		MoteMaker.ThrowText(DrawPos, map, "TextMote_ConstructionFail".Translate(), 6f);
		if (base.Faction == Faction.OfPlayer && WorkToBuild > 1400f)
		{
			Messages.Message("MessageConstructionFailed".Translate(LabelEntityToBuild, worker.LabelShort, worker.Named("WORKER")), new TargetInfo(base.Position, map), MessageTypeDefOf.NegativeEvent);
		}
	}

	protected override void DrawAt(Vector3 drawLoc, bool flip = false)
	{
		if (WorldComponent_GravshipController.CutsceneInProgress && !WorldComponent_GravshipController.GravshipRenderInProgess)
		{
			return;
		}
		Vector3 vector = drawLoc;
		if (BuildDef?.building?.isAttachment == true)
		{
			vector += (base.Rotation.AsVector2 * 0.5f).ToVector3();
		}
		Vector2 vector2 = new Vector2(def.size.x, def.size.z);
		vector2.x *= 1.15f;
		vector2.y *= 1.15f;
		Vector3 s = new Vector3(vector2.x, 1f, vector2.y);
		Matrix4x4 matrix = default(Matrix4x4);
		matrix.SetTRS(vector, base.Rotation.AsQuat, s);
		Graphics.DrawMesh(MeshPool.plane10, matrix, UnderfieldMat, 0);
		int num = 4;
		for (int i = 0; i < num; i++)
		{
			float num2 = (float)Mathf.Min(base.RotatedSize.x, base.RotatedSize.z) * 0.38f;
			IntVec3 intVec = default(IntVec3);
			switch (i)
			{
			case 0:
				intVec = new IntVec3(-1, 0, -1);
				break;
			case 1:
				intVec = new IntVec3(-1, 0, 1);
				break;
			case 2:
				intVec = new IntVec3(1, 0, 1);
				break;
			case 3:
				intVec = new IntVec3(1, 0, -1);
				break;
			}
			Vector3 vector3 = default(Vector3);
			vector3.x = (float)intVec.x * ((float)base.RotatedSize.x / 2f - num2 / 2f);
			vector3.z = (float)intVec.z * ((float)base.RotatedSize.z / 2f - num2 / 2f);
			Vector3 s2 = new Vector3(num2, 1f, num2);
			Matrix4x4 matrix2 = default(Matrix4x4);
			matrix2.SetTRS(vector + Vector3.up * 0.03f + vector3, new Rot4(i).AsQuat, s2);
			Graphics.DrawMesh(MeshPool.plane10, matrix2, CornerMat, 0);
		}
		int num3 = Mathf.CeilToInt((PercentComplete - 0f) / 1f * (float)base.RotatedSize.x * (float)base.RotatedSize.z * 4f);
		IntVec2 intVec2 = base.RotatedSize * 2;
		for (int j = 0; j < num3; j++)
		{
			IntVec2 intVec3 = default(IntVec2);
			intVec3.z = j / intVec2.x;
			intVec3.x = j - intVec3.z * intVec2.x;
			Vector3 vector4 = new Vector3((float)intVec3.x * 0.5f, 0f, (float)intVec3.z * 0.5f) + vector;
			vector4.x -= (float)base.RotatedSize.x * 0.5f - 0.25f;
			vector4.z -= (float)base.RotatedSize.z * 0.5f - 0.25f;
			Vector3 s3 = new Vector3(0.5f, 1f, 0.5f);
			Matrix4x4 matrix3 = default(Matrix4x4);
			matrix3.SetTRS(vector4 + Vector3.up * 0.02f, Quaternion.identity, s3);
			Graphics.DrawMesh(MeshPool.plane10, matrix3, TileMat, 0);
		}
		Comps_PostDraw();
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__46))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__46(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override string GetInspectString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(base.GetInspectString());
		stringBuilder.AppendLineIfNotEmpty();
		stringBuilder.AppendLine("ContainedResources".Translate() + ":");
		List<ThingDefCountClass> list = def.entityDefToBuild.CostListAdjusted(base.Stuff);
		for (int i = 0; i < list.Count; i++)
		{
			ThingDefCountClass thingDefCountClass = list[i];
			int value = thingDefCountClass.count - ThingCountNeeded(thingDefCountClass.thingDef);
			stringBuilder.AppendLine($"{thingDefCountClass.thingDef.LabelCap}: {value} / {thingDefCountClass.count}");
		}
		stringBuilder.Append("WorkLeft".Translate() + ": " + WorkLeft.ToStringWorkAmount());
		if (StyleDef?.Category != null && base.StyleSourcePrecept == null)
		{
			stringBuilder.AppendInNewLine("Style".Translate() + ": " + StyleDef.Category.LabelCap);
		}
		return stringBuilder.ToString();
	}

	public bool Accepts(Thing t)
	{
		return ThingCountNeeded(t.def) > 0;
	}

	public int SpaceRemainingFor(ThingDef stuff)
	{
		return ThingCountNeeded(stuff);
	}

	public StorageSettings GetStoreSettings()
	{
		if (storageGroup != null)
		{
			return storageGroup.GetStoreSettings();
		}
		return storageSettings;
	}

	public StorageSettings GetParentStoreSettings()
	{
		return BuildDef?.building?.fixedStorageSettings;
	}

	void IStoreSettingsParent.Notify_SettingsChanged()
	{
	}

	[IteratorStateMachine(typeof(_003CGetInspectTabs_003Ed__74))]
	public override IEnumerable<InspectTabBase> GetInspectTabs()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetInspectTabs_003Ed__74(-2)
		{
			_003C_003E4__this = this
		};
	}
}
