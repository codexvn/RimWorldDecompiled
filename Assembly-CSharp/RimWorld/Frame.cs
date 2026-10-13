using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200295D RID: 10589
	[StaticConstructorOnStartup]
	public class Frame : Building, IThingHolder, IConstructible, IStorageGroupMember, IStoreSettingsParent, IHaulEnroute, ILoadReferenceable
	{
		// Token: 0x17002358 RID: 9048
		// (get) Token: 0x0600E841 RID: 59457 RVA: 0x00437EE6 File Offset: 0x004360E6
		public ThingDef BuildDef
		{
			get
			{
				return this.def.entityDefToBuild as ThingDef;
			}
		}

		// Token: 0x17002359 RID: 9049
		// (get) Token: 0x0600E842 RID: 59458 RVA: 0x00439C7F File Offset: 0x00437E7F
		public float WorkToBuild
		{
			get
			{
				return this.def.entityDefToBuild.GetStatValueAbstract(StatDefOf.WorkToBuild, base.Stuff);
			}
		}

		// Token: 0x1700235A RID: 9050
		// (get) Token: 0x0600E843 RID: 59459 RVA: 0x00439C9C File Offset: 0x00437E9C
		public float WorkLeft
		{
			get
			{
				return this.WorkToBuild - this.workDone;
			}
		}

		// Token: 0x1700235B RID: 9051
		// (get) Token: 0x0600E844 RID: 59460 RVA: 0x00439CAB File Offset: 0x00437EAB
		public float PercentComplete
		{
			get
			{
				return this.workDone / this.WorkToBuild;
			}
		}

		// Token: 0x1700235C RID: 9052
		// (get) Token: 0x0600E845 RID: 59461 RVA: 0x00439CBA File Offset: 0x00437EBA
		public override string Label
		{
			get
			{
				return this.LabelEntityToBuild + "FrameLabelExtra".Translate();
			}
		}

		// Token: 0x1700235D RID: 9053
		// (get) Token: 0x0600E846 RID: 59462 RVA: 0x00439CD8 File Offset: 0x00437ED8
		public string LabelEntityToBuild
		{
			get
			{
				string text = this.def.entityDefToBuild.label;
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

		// Token: 0x1700235E RID: 9054
		// (get) Token: 0x0600E847 RID: 59463 RVA: 0x00439D3C File Offset: 0x00437F3C
		public override Color DrawColor
		{
			get
			{
				if (!this.def.MadeFromStuff)
				{
					List<ThingDefCountClass> costList = this.def.entityDefToBuild.CostList;
					if (costList != null)
					{
						for (int i = 0; i < costList.Count; i++)
						{
							ThingDef thingDef = costList[i].thingDef;
							if (thingDef.IsStuff && thingDef.stuffProps.color != Color.white)
							{
								return this.def.GetColorForStuff(thingDef);
							}
						}
					}
					return new Color(0.6f, 0.6f, 0.6f);
				}
				return base.DrawColor;
			}
		}

		// Token: 0x1700235F RID: 9055
		// (get) Token: 0x0600E848 RID: 59464 RVA: 0x00439DD0 File Offset: 0x00437FD0
		public EffecterDef ConstructionEffect
		{
			get
			{
				ThingDef stuff = base.Stuff;
				if (((stuff != null) ? stuff.stuffProps.constructEffect : null) != null)
				{
					return base.Stuff.stuffProps.constructEffect;
				}
				if (this.def.entityDefToBuild.constructEffect != null)
				{
					return this.def.entityDefToBuild.constructEffect;
				}
				return EffecterDefOf.ConstructMetal;
			}
		}

		// Token: 0x17002360 RID: 9056
		// (get) Token: 0x0600E849 RID: 59465 RVA: 0x00439E2F File Offset: 0x0043802F
		private Material CornerMat
		{
			get
			{
				if (this.cachedCornerMat == null)
				{
					this.cachedCornerMat = MaterialPool.MatFrom(Frame.CornerTex, ShaderDatabase.MetaOverlay, this.DrawColor);
				}
				return this.cachedCornerMat;
			}
		}

		// Token: 0x17002361 RID: 9057
		// (get) Token: 0x0600E84A RID: 59466 RVA: 0x00439E60 File Offset: 0x00438060
		private Material TileMat
		{
			get
			{
				if (this.cachedTileMat == null)
				{
					this.cachedTileMat = MaterialPool.MatFrom(Frame.TileTex, ShaderDatabase.MetaOverlay, this.DrawColor);
				}
				return this.cachedTileMat;
			}
		}

		// Token: 0x0600E84B RID: 59467 RVA: 0x00439E91 File Offset: 0x00438091
		public Frame()
		{
			this.resourceContainer = new ThingOwner<Thing>(this, false, LookMode.Deep, true);
		}

		// Token: 0x0600E84C RID: 59468 RVA: 0x00439EA8 File Offset: 0x004380A8
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.resourceContainer;
		}

		// Token: 0x0600E84D RID: 59469 RVA: 0x00439EB0 File Offset: 0x004380B0
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600E84E RID: 59470 RVA: 0x00439EC0 File Offset: 0x004380C0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workDone, "workDone", 0f, false);
			Scribe_Deep.Look<ThingOwner>(ref this.resourceContainer, "resourceContainer", new object[]
			{
				this
			});
			Scribe_Values.Look<ColorInt?>(ref this.glowerColorOverride, "glowerColorOverride", null, false);
			Scribe_References.Look<StorageGroup>(ref this.storageGroup, "storageGroup", false);
			Scribe_Deep.Look<StorageSettings>(ref this.storageSettings, "storageSettings", Array.Empty<object>());
		}

		// Token: 0x0600E84F RID: 59471 RVA: 0x00439F44 File Offset: 0x00438144
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			bool spawned = base.Spawned;
			Map map = base.Map;
			base.Destroy(mode);
			if (spawned)
			{
				Blueprint_Storage blueprint_Storage = ThingUtility.CheckAutoRebuildOnDestroyed(this, mode, map, this.def.entityDefToBuild) as Blueprint_Storage;
				if (blueprint_Storage != null)
				{
					blueprint_Storage.SetStorageGroup(this.storageGroup, true);
					blueprint_Storage.settings = new StorageSettings();
					blueprint_Storage.settings.CopyFrom(this.storageSettings);
				}
			}
		}

		// Token: 0x0600E850 RID: 59472 RVA: 0x00439FAC File Offset: 0x004381AC
		public ThingDef EntityToBuildStuff()
		{
			return base.Stuff;
		}

		// Token: 0x0600E851 RID: 59473 RVA: 0x00439FB4 File Offset: 0x004381B4
		public List<ThingDefCountClass> TotalMaterialCost()
		{
			return this.def.entityDefToBuild.CostListAdjusted(base.Stuff, true);
		}

		// Token: 0x0600E852 RID: 59474 RVA: 0x00439FD0 File Offset: 0x004381D0
		public bool IsCompleted()
		{
			foreach (ThingDefCountClass thingDefCountClass in this.TotalMaterialCost())
			{
				if (thingDefCountClass.count - this.resourceContainer.TotalStackCountOfDef(thingDefCountClass.thingDef) > 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600E853 RID: 59475 RVA: 0x0043A040 File Offset: 0x00438240
		public int ThingCountNeeded(ThingDef stuff)
		{
			foreach (ThingDefCountClass thingDefCountClass in this.TotalMaterialCost())
			{
				if (thingDefCountClass.thingDef == stuff)
				{
					return thingDefCountClass.count - this.resourceContainer.TotalStackCountOfDef(thingDefCountClass.thingDef);
				}
			}
			return 0;
		}

		// Token: 0x0600E854 RID: 59476 RVA: 0x0043A0B4 File Offset: 0x004382B4
		public int ThingCountNeededWithEnroute(ThingDef stuff, Pawn excludeEnrouteFor = null)
		{
			foreach (ThingDefCountClass thingDefCountClass in this.TotalMaterialCost())
			{
				if (thingDefCountClass.thingDef == stuff)
				{
					int num = thingDefCountClass.count - this.resourceContainer.TotalStackCountOfDef(thingDefCountClass.thingDef);
					int num2 = num - base.Map.enrouteManager.GetEnroute(this, thingDefCountClass.thingDef, excludeEnrouteFor);
					if (num2 < 0)
					{
						Log.Error(string.Format("amount of stuff for {0} is negative: {1} {2}.", this.LabelShort, stuff.label, num2));
					}
					if (num2 > num)
					{
						Log.Error(string.Format("amount of stuff for {0} was greater than could be needed: {1} {2} (needs {3}).", new object[]
						{
							this.LabelShort,
							stuff.label,
							num2,
							num
						}));
					}
					return Mathf.Min(Mathf.Max(num2, 0), num);
				}
			}
			return 0;
		}

		// Token: 0x0600E855 RID: 59477 RVA: 0x0043A1C0 File Offset: 0x004383C0
		public void CompleteConstruction(Pawn worker)
		{
			if (worker.Faction != null)
			{
				QuestUtility.SendQuestTargetSignals(worker.Faction.questTags, "BuiltBuilding", this.Named("SUBJECT"));
			}
			List<CompHasSources> list = new List<CompHasSources>();
			for (int i = 0; i < this.resourceContainer.Count; i++)
			{
				CompHasSources compHasSources = this.resourceContainer[i].TryGetComp<CompHasSources>();
				if (compHasSources != null)
				{
					list.Add(compHasSources);
				}
			}
			this.resourceContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			Map map = base.Map;
			bool flag = Find.Selector.IsSelected(this);
			this.Destroy(DestroyMode.Vanish);
			if (this.GetStatValue(StatDefOf.WorkToBuild, true, -1) > 150f)
			{
				ThingDef thingDef = this.def.entityDefToBuild as ThingDef;
				if (thingDef != null && thingDef.category == ThingCategory.Building)
				{
					SoundDefOf.Building_Complete.PlayOneShot(new TargetInfo(base.Position, map, false));
				}
			}
			ThingDef thingDef2 = this.def.entityDefToBuild as ThingDef;
			Thing thing = null;
			if (thingDef2 != null)
			{
				thing = ThingMaker.MakeThing(thingDef2, base.Stuff);
				thing.SetFactionDirect(base.Faction);
				CompQuality compQuality = thing.TryGetComp<CompQuality>();
				if (compQuality != null)
				{
					QualityCategory q = QualityUtility.GenerateQualityCreatedByPawn(worker, SkillDefOf.Construction, true);
					compQuality.SetQuality(q, new ArtGenerationContext?(ArtGenerationContext.Colony));
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
				if (compHasSources2 != null && !list.NullOrEmpty<CompHasSources>())
				{
					for (int j = 0; j < list.Count; j++)
					{
						list[j].TransferSourcesTo(compHasSources2, -1);
					}
				}
				if (this.GetIdeoForStyle(worker) != null)
				{
					thing.StyleDef = this.StyleDef;
				}
				thing.HitPoints = Mathf.CeilToInt((float)this.HitPoints / (float)base.MaxHitPoints * (float)thing.MaxHitPoints);
				GenSpawn.Spawn(thing, base.Position, map, base.Rotation, WipeMode.FullRefund, false, false);
				Building building = thing as Building;
				if (building != null)
				{
					Lord lord = worker.GetLord();
					if (lord != null)
					{
						lord.AddBuilding(building);
					}
					building.StyleSourcePrecept = base.StyleSourcePrecept;
				}
				IStorageGroupMember storageGroupMember = thing as IStorageGroupMember;
				if (storageGroupMember != null)
				{
					storageGroupMember.SetStorageGroup(this.storageGroup, true);
					storageGroupMember.ThingStoreSettings.CopyFrom(this.storageSettings);
				}
				this.SetStorageGroup(null, false);
				if (thingDef2 != null)
				{
					Color? ideoColorForBuilding = IdeoUtility.GetIdeoColorForBuilding(thingDef2, base.Faction);
					if (ideoColorForBuilding != null)
					{
						thing.SetColor(ideoColorForBuilding.Value, true);
					}
				}
				if (this.overrideGraphicIndex != null)
				{
					thing.overrideGraphicIndex = this.overrideGraphicIndex;
				}
			}
			else
			{
				TerrainDef terrainDef = map.terrainGrid.TerrainAt(base.Position);
				TerrainDef terrainDef2 = (TerrainDef)this.def.entityDefToBuild;
				if (terrainDef.tempTerrain != null && terrainDef.tempTerrain.replaceableByBridge && terrainDef2.bridge)
				{
					map.terrainGrid.RemoveTempTerrain(base.Position, false, false);
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
			if (thing != null && thing.GetStatValue(StatDefOf.WorkToBuild, true, -1) >= 9500f)
			{
				TaleRecorder.RecordTale(TaleDefOf.CompletedLongConstructionProject, new object[]
				{
					worker,
					thing.def
				});
			}
			if (thing != null && flag)
			{
				Find.Selector.Select(thing, false, false);
			}
			if (this.glowerColorOverride != null)
			{
				CompGlower compGlower = (thing != null) ? thing.TryGetComp<CompGlower>() : null;
				if (compGlower != null)
				{
					compGlower.GlowColor = this.glowerColorOverride.Value;
				}
			}
			Lord lord2 = worker.GetLord();
			if (lord2 != null)
			{
				Building building2 = thing as Building;
				if (building2 != null)
				{
					lord2.Notify_ConstructionCompleted(worker, building2);
				}
			}
		}

		// Token: 0x0600E856 RID: 59478 RVA: 0x0043A5D8 File Offset: 0x004387D8
		private Ideo GetIdeoForStyle(Pawn worker)
		{
			if (worker.Ideo != null)
			{
				return worker.Ideo;
			}
			if (ModsConfig.BiotechActive && worker.IsColonyMech)
			{
				Pawn overseer = worker.GetOverseer();
				if (((overseer != null) ? overseer.Ideo : null) != null)
				{
					return overseer.Ideo;
				}
			}
			return null;
		}

		// Token: 0x0600E857 RID: 59479 RVA: 0x0043A620 File Offset: 0x00438820
		public void FailConstruction(Pawn worker)
		{
			Map map = base.Map;
			this.Destroy(DestroyMode.FailConstruction);
			Blueprint_Build blueprint_Build = null;
			if (this.def.entityDefToBuild.blueprintDef != null)
			{
				blueprint_Build = (Blueprint_Build)ThingMaker.MakeThing(this.def.entityDefToBuild.blueprintDef, null);
				blueprint_Build.stuffToUse = base.Stuff;
				blueprint_Build.SetFactionDirect(base.Faction);
				blueprint_Build.InheritStyle(base.StyleSourcePrecept, this.StyleDef);
				IStorageGroupMember storageGroupMember = blueprint_Build as IStorageGroupMember;
				if (storageGroupMember != null)
				{
					storageGroupMember.Group = this.storageGroup;
				}
				Blueprint_Storage blueprint_Storage = blueprint_Build as Blueprint_Storage;
				if (blueprint_Storage != null && this.storageSettings != null)
				{
					blueprint_Storage.SetStorageGroup(this.storageGroup, true);
					blueprint_Storage.settings = new StorageSettings();
					blueprint_Storage.settings.CopyFrom(this.storageSettings);
				}
				GenSpawn.Spawn(blueprint_Build, base.Position, map, base.Rotation, WipeMode.FullRefund, false, false);
			}
			Lord lord = worker.GetLord();
			if (lord != null)
			{
				lord.Notify_ConstructionFailed(worker, this, blueprint_Build);
			}
			MoteMaker.ThrowText(this.DrawPos, map, "TextMote_ConstructionFail".Translate(), 6f);
			if (base.Faction == Faction.OfPlayer && this.WorkToBuild > 1400f)
			{
				Messages.Message("MessageConstructionFailed".Translate(this.LabelEntityToBuild, worker.LabelShort, worker.Named("WORKER")), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NegativeEvent, true);
			}
		}

		// Token: 0x0600E858 RID: 59480 RVA: 0x0043A7A0 File Offset: 0x004389A0
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			if (WorldComponent_GravshipController.CutsceneInProgress && !WorldComponent_GravshipController.GravshipRenderInProgess)
			{
				return;
			}
			Vector3 vector = drawLoc;
			ThingDef buildDef = this.BuildDef;
			bool? flag;
			if (buildDef == null)
			{
				flag = null;
			}
			else
			{
				BuildingProperties building = buildDef.building;
				flag = ((building != null) ? new bool?(building.isAttachment) : null);
			}
			bool? flag2 = flag;
			if (flag2.GetValueOrDefault())
			{
				vector += (base.Rotation.AsVector2 * 0.5f).ToVector3();
			}
			Vector2 vector2 = new Vector2((float)this.def.size.x, (float)this.def.size.z);
			vector2.x *= 1.15f;
			vector2.y *= 1.15f;
			Vector3 s = new Vector3(vector2.x, 1f, vector2.y);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(vector, base.Rotation.AsQuat, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, Frame.UnderfieldMat, 0);
			int num = 4;
			for (int i = 0; i < num; i++)
			{
				float num2 = (float)Mathf.Min(base.RotatedSize.x, base.RotatedSize.z) * 0.38f;
				IntVec3 intVec = default(IntVec3);
				if (i == 0)
				{
					intVec = new IntVec3(-1, 0, -1);
				}
				else if (i == 1)
				{
					intVec = new IntVec3(-1, 0, 1);
				}
				else if (i == 2)
				{
					intVec = new IntVec3(1, 0, 1);
				}
				else if (i == 3)
				{
					intVec = new IntVec3(1, 0, -1);
				}
				Vector3 b = default(Vector3);
				b.x = (float)intVec.x * ((float)base.RotatedSize.x / 2f - num2 / 2f);
				b.z = (float)intVec.z * ((float)base.RotatedSize.z / 2f - num2 / 2f);
				Vector3 s2 = new Vector3(num2, 1f, num2);
				Matrix4x4 matrix2 = default(Matrix4x4);
				matrix2.SetTRS(vector + Vector3.up * 0.03f + b, new Rot4(i).AsQuat, s2);
				Graphics.DrawMesh(MeshPool.plane10, matrix2, this.CornerMat, 0);
			}
			int num3 = Mathf.CeilToInt((this.PercentComplete - 0f) / 1f * (float)base.RotatedSize.x * (float)base.RotatedSize.z * 4f);
			IntVec2 intVec2 = base.RotatedSize * 2;
			for (int j = 0; j < num3; j++)
			{
				IntVec2 intVec3 = default(IntVec2);
				intVec3.z = j / intVec2.x;
				intVec3.x = j - intVec3.z * intVec2.x;
				Vector3 a = new Vector3((float)intVec3.x * 0.5f, 0f, (float)intVec3.z * 0.5f) + vector;
				a.x -= (float)base.RotatedSize.x * 0.5f - 0.25f;
				a.z -= (float)base.RotatedSize.z * 0.5f - 0.25f;
				Vector3 s3 = new Vector3(0.5f, 1f, 0.5f);
				Matrix4x4 matrix3 = default(Matrix4x4);
				matrix3.SetTRS(a + Vector3.up * 0.02f, Quaternion.identity, s3);
				Graphics.DrawMesh(MeshPool.plane10, matrix3, this.TileMat, 0);
			}
			base.Comps_PostDraw();
		}

		// Token: 0x0600E859 RID: 59481 RVA: 0x0043AB56 File Offset: 0x00438D56
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.DrawStorageTab)
			{
				foreach (Gizmo gizmo2 in StorageSettingsClipboard.CopyPasteGizmosFor(this.GetStoreSettings()))
				{
					yield return gizmo2;
				}
				enumerator = null;
			}
			Gizmo selectMonumentMarkerGizmo = QuestUtility.GetSelectMonumentMarkerGizmo(this);
			if (selectMonumentMarkerGizmo != null)
			{
				yield return selectMonumentMarkerGizmo;
			}
			Command command = BuildCopyCommandUtility.BuildCopyCommand(this.def.entityDefToBuild, base.Stuff, base.StyleSourcePrecept as Precept_Building, this.StyleDef, true, this.glowerColorOverride);
			if (command != null)
			{
				yield return command;
			}
			if (base.Faction == Faction.OfPlayer)
			{
				foreach (Command command2 in BuildRelatedCommandUtility.RelatedBuildCommands(this.def.entityDefToBuild))
				{
					yield return command2;
				}
				IEnumerator<Command> enumerator2 = null;
				if (!this.StorageGroupTag.NullOrEmpty())
				{
					foreach (Gizmo gizmo3 in StorageGroupUtility.StorageGroupMemberGizmos(this))
					{
						yield return gizmo3;
					}
					enumerator = null;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0600E85A RID: 59482 RVA: 0x0043AB68 File Offset: 0x00438D68
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			stringBuilder.AppendLineIfNotEmpty();
			stringBuilder.AppendLine("ContainedResources".Translate() + ":");
			List<ThingDefCountClass> list = this.def.entityDefToBuild.CostListAdjusted(base.Stuff, true);
			for (int i = 0; i < list.Count; i++)
			{
				ThingDefCountClass thingDefCountClass = list[i];
				int num = thingDefCountClass.count - this.ThingCountNeeded(thingDefCountClass.thingDef);
				stringBuilder.AppendLine(string.Format("{0}: {1} / {2}", thingDefCountClass.thingDef.LabelCap, num, thingDefCountClass.count));
			}
			stringBuilder.Append("WorkLeft".Translate() + ": " + this.WorkLeft.ToStringWorkAmount());
			ThingStyleDef styleDef = this.StyleDef;
			if (((styleDef != null) ? styleDef.Category : null) != null && base.StyleSourcePrecept == null)
			{
				stringBuilder.AppendInNewLine("Style".Translate() + ": " + this.StyleDef.Category.LabelCap);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17002362 RID: 9058
		// (get) Token: 0x0600E85B RID: 59483 RVA: 0x0043ACAF File Offset: 0x00438EAF
		// (set) Token: 0x0600E85C RID: 59484 RVA: 0x0043ACB7 File Offset: 0x00438EB7
		StorageGroup IStorageGroupMember.Group
		{
			get
			{
				return this.storageGroup;
			}
			set
			{
				this.storageGroup = value;
			}
		}

		// Token: 0x17002363 RID: 9059
		// (get) Token: 0x0600E85D RID: 59485 RVA: 0x00438FA5 File Offset: 0x004371A5
		Map IStorageGroupMember.Map
		{
			get
			{
				return base.Map;
			}
		}

		// Token: 0x0600E85E RID: 59486 RVA: 0x0043ACC0 File Offset: 0x00438EC0
		public bool Accepts(Thing t)
		{
			return this.ThingCountNeeded(t.def) > 0;
		}

		// Token: 0x0600E85F RID: 59487 RVA: 0x0043ACD1 File Offset: 0x00438ED1
		public int SpaceRemainingFor(ThingDef stuff)
		{
			return this.ThingCountNeeded(stuff);
		}

		// Token: 0x17002364 RID: 9060
		// (get) Token: 0x0600E860 RID: 59488 RVA: 0x0043ACDA File Offset: 0x00438EDA
		StorageSettings IStorageGroupMember.StoreSettings
		{
			get
			{
				return this.GetStoreSettings();
			}
		}

		// Token: 0x17002365 RID: 9061
		// (get) Token: 0x0600E861 RID: 59489 RVA: 0x0043ACE2 File Offset: 0x00438EE2
		StorageSettings IStorageGroupMember.ParentStoreSettings
		{
			get
			{
				return this.GetParentStoreSettings();
			}
		}

		// Token: 0x17002366 RID: 9062
		// (get) Token: 0x0600E862 RID: 59490 RVA: 0x0043ACEA File Offset: 0x00438EEA
		StorageSettings IStorageGroupMember.ThingStoreSettings
		{
			get
			{
				return this.storageSettings;
			}
		}

		// Token: 0x17002367 RID: 9063
		// (get) Token: 0x0600E863 RID: 59491 RVA: 0x0043ACF2 File Offset: 0x00438EF2
		public string StorageGroupTag
		{
			get
			{
				ThingDef buildDef = this.BuildDef;
				if (buildDef == null)
				{
					return null;
				}
				BuildingProperties building = buildDef.building;
				if (building == null)
				{
					return null;
				}
				return building.storageGroupTag;
			}
		}

		// Token: 0x17002368 RID: 9064
		// (get) Token: 0x0600E864 RID: 59492 RVA: 0x0043AD10 File Offset: 0x00438F10
		bool IStorageGroupMember.DrawConnectionOverlay
		{
			get
			{
				return !this.StorageGroupTag.NullOrEmpty();
			}
		}

		// Token: 0x17002369 RID: 9065
		// (get) Token: 0x0600E865 RID: 59493 RVA: 0x0043AD20 File Offset: 0x00438F20
		bool IStorageGroupMember.ShowRenameButton
		{
			get
			{
				return !this.StorageGroupTag.NullOrEmpty() && base.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x1700236A RID: 9066
		// (get) Token: 0x0600E866 RID: 59494 RVA: 0x0043AD10 File Offset: 0x00438F10
		public bool DrawStorageTab
		{
			get
			{
				return !this.StorageGroupTag.NullOrEmpty();
			}
		}

		// Token: 0x1700236B RID: 9067
		// (get) Token: 0x0600E867 RID: 59495 RVA: 0x0043AD10 File Offset: 0x00438F10
		bool IStoreSettingsParent.StorageTabVisible
		{
			get
			{
				return !this.StorageGroupTag.NullOrEmpty();
			}
		}

		// Token: 0x0600E868 RID: 59496 RVA: 0x0043AD3E File Offset: 0x00438F3E
		public StorageSettings GetStoreSettings()
		{
			if (this.storageGroup != null)
			{
				return this.storageGroup.GetStoreSettings();
			}
			return this.storageSettings;
		}

		// Token: 0x0600E869 RID: 59497 RVA: 0x0043AD5A File Offset: 0x00438F5A
		public StorageSettings GetParentStoreSettings()
		{
			ThingDef buildDef = this.BuildDef;
			if (buildDef == null)
			{
				return null;
			}
			BuildingProperties building = buildDef.building;
			if (building == null)
			{
				return null;
			}
			return building.fixedStorageSettings;
		}

		// Token: 0x0600E86A RID: 59498 RVA: 0x000026EA File Offset: 0x000008EA
		void IStoreSettingsParent.Notify_SettingsChanged()
		{
		}

		// Token: 0x0600E86B RID: 59499 RVA: 0x0043AD78 File Offset: 0x00438F78
		public override IEnumerable<InspectTabBase> GetInspectTabs()
		{
			if (!this.StorageGroupTag.NullOrEmpty() && this.BuildDef.inspectorTabsResolved != null)
			{
				foreach (InspectTabBase inspectTabBase in this.BuildDef.inspectorTabsResolved)
				{
					yield return inspectTabBase;
				}
				List<InspectTabBase>.Enumerator enumerator = default(List<InspectTabBase>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x04009AD9 RID: 39641
		public ThingOwner resourceContainer;

		// Token: 0x04009ADA RID: 39642
		public float workDone;

		// Token: 0x04009ADB RID: 39643
		public ColorInt? glowerColorOverride;

		// Token: 0x04009ADC RID: 39644
		public StorageGroup storageGroup;

		// Token: 0x04009ADD RID: 39645
		public StorageSettings storageSettings;

		// Token: 0x04009ADE RID: 39646
		private Material cachedCornerMat;

		// Token: 0x04009ADF RID: 39647
		private Material cachedTileMat;

		// Token: 0x04009AE0 RID: 39648
		protected const float UnderfieldOverdrawFactor = 1.15f;

		// Token: 0x04009AE1 RID: 39649
		private const int LongConstructionProjectThreshold = 9500;

		// Token: 0x04009AE2 RID: 39650
		private static readonly Material UnderfieldMat = MaterialPool.MatFrom("Things/Building/BuildingFrame/Underfield", ShaderDatabase.Transparent);

		// Token: 0x04009AE3 RID: 39651
		private static readonly Texture2D CornerTex = ContentFinder<Texture2D>.Get("Things/Building/BuildingFrame/Corner", true);

		// Token: 0x04009AE4 RID: 39652
		private static readonly Texture2D TileTex = ContentFinder<Texture2D>.Get("Things/Building/BuildingFrame/Tile", true);
	}
}
