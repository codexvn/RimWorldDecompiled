using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E1A RID: 15898
	[StaticConstructorOnStartup]
	public class WorldObject : IExposable, ILoadReferenceable, ISelectable
	{
		// Token: 0x1700360F RID: 13839
		// (get) Token: 0x06015B20 RID: 88864 RVA: 0x0067DC43 File Offset: 0x0067BE43
		public List<WorldObjectComp> AllComps
		{
			get
			{
				return this.comps;
			}
		}

		// Token: 0x17003610 RID: 13840
		// (get) Token: 0x06015B21 RID: 88865 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool ShowRelatedQuests
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17003611 RID: 13841
		// (get) Token: 0x06015B22 RID: 88866 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool CanReformFoggedEnemies
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17003612 RID: 13842
		// (get) Token: 0x06015B23 RID: 88867 RVA: 0x0067DC4B File Offset: 0x0067BE4B
		public bool Destroyed
		{
			get
			{
				return this.destroyed;
			}
		}

		// Token: 0x17003613 RID: 13843
		// (get) Token: 0x06015B24 RID: 88868 RVA: 0x0067DC54 File Offset: 0x0067BE54
		public virtual bool VisibleInBackground
		{
			get
			{
				return this.Tile.LayerDef.isSpace && Find.WorldSelector.SelectedLayer.Def.isSpace;
			}
		}

		// Token: 0x17003614 RID: 13844
		// (get) Token: 0x06015B25 RID: 88869 RVA: 0x0067DC8C File Offset: 0x0067BE8C
		public virtual bool RequiresSignalJammerToReach
		{
			get
			{
				if (this.def.requiresSignalJammerToReach)
				{
					MapParent mapParent = this as MapParent;
					return mapParent == null || !mapParent.HasMap;
				}
				return false;
			}
		}

		// Token: 0x17003615 RID: 13845
		// (get) Token: 0x06015B26 RID: 88870 RVA: 0x0067DCBE File Offset: 0x0067BEBE
		public virtual float DrawAltitude
		{
			get
			{
				return 0.03f;
			}
		}

		// Token: 0x17003616 RID: 13846
		// (get) Token: 0x06015B27 RID: 88871 RVA: 0x0067DCC5 File Offset: 0x0067BEC5
		// (set) Token: 0x06015B28 RID: 88872 RVA: 0x0067DCD0 File Offset: 0x0067BED0
		public PlanetTile Tile
		{
			get
			{
				return this.tile;
			}
			set
			{
				if (this.tile == value)
				{
					return;
				}
				PlanetTile previous = this.tile;
				this.tile = value;
				if (this.Spawned && !this.def.useDynamicDrawer)
				{
					Find.World.renderer.Notify_StaticWorldObjectPosChanged();
				}
				if (previous.Valid)
				{
					previous.Tile.Layer.FastTileFinder.DirtyTile(previous);
				}
				this.tile.Tile.Layer.FastTileFinder.DirtyTile(this.tile);
				this.PositionChanged(previous, this.tile);
			}
		}

		// Token: 0x17003617 RID: 13847
		// (get) Token: 0x06015B29 RID: 88873 RVA: 0x0067DD6B File Offset: 0x0067BF6B
		public bool Spawned
		{
			get
			{
				return Find.WorldObjects.Contains(this);
			}
		}

		// Token: 0x17003618 RID: 13848
		// (get) Token: 0x06015B2A RID: 88874 RVA: 0x0067DD78 File Offset: 0x0067BF78
		public virtual Vector3 DrawPos
		{
			get
			{
				if (Find.TickManager.TicksGame != this.drawPosCacheTick)
				{
					this.drawPosCached = this.Tile.Layer.Origin + Find.WorldGrid.GetTileCenter(this.Tile);
					this.drawPosCacheTick = Find.TickManager.TicksGame;
				}
				return this.drawPosCached;
			}
		}

		// Token: 0x17003619 RID: 13849
		// (get) Token: 0x06015B2B RID: 88875 RVA: 0x0067DDDB File Offset: 0x0067BFDB
		public Faction Faction
		{
			get
			{
				return this.factionInt;
			}
		}

		// Token: 0x1700361A RID: 13850
		// (get) Token: 0x06015B2C RID: 88876 RVA: 0x0067DDE3 File Offset: 0x0067BFE3
		public virtual string Label
		{
			get
			{
				return this.def.label;
			}
		}

		// Token: 0x1700361B RID: 13851
		// (get) Token: 0x06015B2D RID: 88877 RVA: 0x0067DDF0 File Offset: 0x0067BFF0
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.def);
			}
		}

		// Token: 0x1700361C RID: 13852
		// (get) Token: 0x06015B2E RID: 88878 RVA: 0x0067DE03 File Offset: 0x0067C003
		public virtual string LabelShort
		{
			get
			{
				return this.Label;
			}
		}

		// Token: 0x1700361D RID: 13853
		// (get) Token: 0x06015B2F RID: 88879 RVA: 0x0067DE0B File Offset: 0x0067C00B
		public virtual string LabelShortCap
		{
			get
			{
				return this.LabelShort.CapitalizeFirst(this.def);
			}
		}

		// Token: 0x1700361E RID: 13854
		// (get) Token: 0x06015B30 RID: 88880 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool HasName
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700361F RID: 13855
		// (get) Token: 0x06015B31 RID: 88881 RVA: 0x0067DE1E File Offset: 0x0067C01E
		public virtual Material Material
		{
			get
			{
				return this.def.Material;
			}
		}

		// Token: 0x17003620 RID: 13856
		// (get) Token: 0x06015B32 RID: 88882 RVA: 0x0067DE2C File Offset: 0x0067C02C
		public virtual Material ExpandingMaterial
		{
			get
			{
				if (this.def.expandingShader == null)
				{
					return null;
				}
				if (this.expandingMaterial != null)
				{
					return this.expandingMaterial;
				}
				MaterialRequest req = new MaterialRequest
				{
					mainTex = this.def.ExpandingIconTexture,
					shader = this.def.expandingShader.Shader,
					color = Color.white,
					maskTex = this.def.ExpandingIconTextureMask
				};
				return this.expandingMaterial = MaterialPool.MatFrom(req);
			}
		}

		// Token: 0x17003621 RID: 13857
		// (get) Token: 0x06015B33 RID: 88883 RVA: 0x0067DEBD File Offset: 0x0067C0BD
		public virtual bool SelectableNow
		{
			get
			{
				return this.def.selectable;
			}
		}

		// Token: 0x17003622 RID: 13858
		// (get) Token: 0x06015B34 RID: 88884 RVA: 0x0067DECA File Offset: 0x0067C0CA
		public virtual bool NeverMultiSelect
		{
			get
			{
				return this.def.neverMultiSelect;
			}
		}

		// Token: 0x17003623 RID: 13859
		// (get) Token: 0x06015B35 RID: 88885 RVA: 0x0067DED8 File Offset: 0x0067C0D8
		public virtual Texture2D ExpandingIcon
		{
			get
			{
				if (this.def.ExpandingIconTexture != null)
				{
					return this.def.ExpandingIconTexture;
				}
				if (this.Material != null)
				{
					return (Texture2D)this.Material.mainTexture;
				}
				return BaseContent.BadTex;
			}
		}

		// Token: 0x17003624 RID: 13860
		// (get) Token: 0x06015B36 RID: 88886 RVA: 0x0067DF28 File Offset: 0x0067C128
		public virtual Color ExpandingIconColor
		{
			get
			{
				Color? expandingIconColor = this.def.expandingIconColor;
				if (expandingIconColor == null)
				{
					return this.Material.color;
				}
				return expandingIconColor.GetValueOrDefault();
			}
		}

		// Token: 0x17003625 RID: 13861
		// (get) Token: 0x06015B37 RID: 88887 RVA: 0x0067DF5D File Offset: 0x0067C15D
		public virtual float ExpandingIconPriority
		{
			get
			{
				return this.def.expandingIconPriority;
			}
		}

		// Token: 0x17003626 RID: 13862
		// (get) Token: 0x06015B38 RID: 88888 RVA: 0x0067DF6A File Offset: 0x0067C16A
		public virtual bool ExpandMore
		{
			get
			{
				return this.def.expandMore;
			}
		}

		// Token: 0x17003627 RID: 13863
		// (get) Token: 0x06015B39 RID: 88889 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool AppendFactionToInspectString
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17003628 RID: 13864
		// (get) Token: 0x06015B3A RID: 88890 RVA: 0x0067DF77 File Offset: 0x0067C177
		public IThingHolder ParentHolder
		{
			get
			{
				if (!this.Spawned)
				{
					return null;
				}
				return Find.World;
			}
		}

		// Token: 0x17003629 RID: 13865
		// (get) Token: 0x06015B3B RID: 88891 RVA: 0x000444B9 File Offset: 0x000426B9
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats
		{
			get
			{
				return Enumerable.Empty<StatDrawEntry>();
			}
		}

		// Token: 0x1700362A RID: 13866
		// (get) Token: 0x06015B3C RID: 88892 RVA: 0x0067DF88 File Offset: 0x0067C188
		public virtual BiomeDef Biome
		{
			get
			{
				if (!this.Spawned)
				{
					return null;
				}
				return Find.WorldGrid[this.Tile].PrimaryBiome;
			}
		}

		// Token: 0x1700362B RID: 13867
		// (get) Token: 0x06015B3D RID: 88893 RVA: 0x00006AAC File Offset: 0x00004CAC
		public virtual float ExpandingIconRotation
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x1700362C RID: 13868
		// (get) Token: 0x06015B3E RID: 88894 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool ExpandingIconFlipHorizontal
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700362D RID: 13869
		// (get) Token: 0x06015B3F RID: 88895 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool GravShipCanLandOn
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700362E RID: 13870
		// (get) Token: 0x06015B40 RID: 88896 RVA: 0x0067DFA9 File Offset: 0x0067C1A9
		protected virtual int UpdateRateTicks
		{
			get
			{
				if (!WorldRendererUtility.WorldSelected)
				{
					return 15;
				}
				return 1;
			}
		}

		// Token: 0x1700362F RID: 13871
		// (get) Token: 0x06015B41 RID: 88897 RVA: 0x0067DFB6 File Offset: 0x0067C1B6
		protected virtual int UpdateRateTickOffset
		{
			get
			{
				return this.HashOffset();
			}
		}

		// Token: 0x06015B42 RID: 88898 RVA: 0x0067DFBE File Offset: 0x0067C1BE
		public virtual IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
		{
			if (this.def.IncidentTargetTags != null)
			{
				foreach (IncidentTargetTagDef incidentTargetTagDef in this.def.IncidentTargetTags)
				{
					yield return incidentTargetTagDef;
				}
				List<IncidentTargetTagDef>.Enumerator enumerator = default(List<IncidentTargetTagDef>.Enumerator);
			}
			int num;
			for (int i = 0; i < this.comps.Count; i = num + 1)
			{
				foreach (IncidentTargetTagDef incidentTargetTagDef2 in this.comps[i].IncidentTargetTags())
				{
					yield return incidentTargetTagDef2;
				}
				IEnumerator<IncidentTargetTagDef> enumerator2 = null;
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x06015B43 RID: 88899 RVA: 0x0067DFD0 File Offset: 0x0067C1D0
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<WorldObjectDef>(ref this.def, "def");
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.InitializeComps();
			}
			Scribe_Values.Look<PlanetTile>(ref this.tile, "tile", default(PlanetTile), false);
			Scribe_Values.Look<int>(ref this.ID, "ID", -1, false);
			Scribe_Values.Look<int>(ref this.creationGameTicks, "creationGameTicks", 0, false);
			Scribe_Values.Look<bool>(ref this.destroyed, "destroyed", false, false);
			Scribe_Values.Look<int>(ref this.tickDelta, "tickDelta", 0, false);
			Scribe_Values.Look<bool>(ref this.isGeneratedLocation, "isGeneratedLocation", false, false);
			Scribe_References.Look<Faction>(ref this.factionInt, "faction", false);
			Scribe_Collections.Look<string>(ref this.questTags, "questTags", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				Scribe_Deep.Look<ThingOwner<Thing>>(ref this.rewards, "rewards", Array.Empty<object>());
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].PostExposeData();
			}
		}

		// Token: 0x06015B44 RID: 88900 RVA: 0x0067E0DC File Offset: 0x0067C2DC
		private void InitializeComps()
		{
			for (int i = 0; i < this.def.comps.Count; i++)
			{
				WorldObjectComp worldObjectComp = null;
				try
				{
					worldObjectComp = (WorldObjectComp)Activator.CreateInstance(this.def.comps[i].compClass);
					worldObjectComp.parent = this;
					this.comps.Add(worldObjectComp);
					worldObjectComp.Initialize(this.def.comps[i]);
				}
				catch (Exception ex)
				{
					string str = "Could not instantiate or initialize a WorldObjectComp: ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
					this.comps.Remove(worldObjectComp);
				}
			}
		}

		// Token: 0x06015B45 RID: 88901 RVA: 0x0067E194 File Offset: 0x0067C394
		public virtual void SetFaction(Faction newFaction)
		{
			if (!this.def.canHaveFaction && newFaction != null)
			{
				Log.Warning(string.Concat(new string[]
				{
					"Tried to set faction to ",
					(newFaction != null) ? newFaction.ToString() : null,
					" but this world object (",
					(this != null) ? this.ToString() : null,
					") cannot have faction."
				}));
				return;
			}
			this.factionInt = newFaction;
		}

		// Token: 0x06015B46 RID: 88902 RVA: 0x0067E204 File Offset: 0x0067C404
		public virtual string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (ModsConfig.OdysseyActive && this.def.requiresSignalJammerToReach)
			{
				stringBuilder.Append("RequiresSignalJammer".Translate());
			}
			if (this.Faction != null && this.AppendFactionToInspectString)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append(string.Format("{0}: {1}", "Faction".Translate(), this.Faction.Name));
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				string text = this.comps[i].CompInspectStringExtra();
				if (!text.NullOrEmpty())
				{
					if (Prefs.DevMode)
					{
						string text2 = text;
						if (char.IsWhiteSpace(text2[text2.Length - 1]))
						{
							Log.ErrorOnce(string.Format("{0} CompInspectStringExtra ended with whitespace: {1}", this.comps[i].GetType(), text), 25612);
							text = text.TrimEndNewlines();
						}
					}
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append(text);
				}
			}
			QuestUtility.AppendInspectStringsFromQuestParts(stringBuilder, this);
			return stringBuilder.ToString();
		}

		// Token: 0x06015B47 RID: 88903 RVA: 0x0067E32C File Offset: 0x0067C52C
		public void DoTick()
		{
			using (ProfilerBlock.Scope("DoTick()"))
			{
				using (ProfilerBlock.Scope("Tick()"))
				{
					this.Tick();
				}
				this.tickDelta++;
				if (this.tickDelta > this.UpdateRateTicks || GenTicks.IsTickInterval(this.UpdateRateTickOffset, this.UpdateRateTicks))
				{
					using (ProfilerBlock.Scope("TickInterval()"))
					{
						this.TickInterval(this.tickDelta);
					}
					this.tickDelta = 0;
				}
			}
			if (this.Destroyed)
			{
				return;
			}
			if (!this.cached)
			{
				this.cached = true;
				this.cachedHolder = (this as IThingHolder);
				this.cachedTickable = (this as IThingHolderTickable);
				this.cachedIsHolder = (this.cachedHolder != null);
			}
			if (!this.cachedIsHolder)
			{
				return;
			}
			if (this.cachedTickable == null || this.cachedTickable.ShouldTickContents)
			{
				if (this.tmpHolders == null)
				{
					this.tmpHolders = new List<IThingHolder>(8);
				}
				this.tmpHolders.Add(this.cachedHolder);
				this.cachedHolder.GetChildHolders(this.tmpHolders);
				for (int i = 0; i < this.tmpHolders.Count; i++)
				{
					ThingOwner directlyHeldThings = this.tmpHolders[i].GetDirectlyHeldThings();
					if (directlyHeldThings != null)
					{
						IThingHolder owner = directlyHeldThings.Owner;
						if (!(owner is Map) && !(owner is Caravan))
						{
							directlyHeldThings.DoTick();
							if (this.Destroyed)
							{
								break;
							}
						}
					}
				}
				this.tmpHolders.Clear();
			}
		}

		// Token: 0x06015B48 RID: 88904 RVA: 0x0067E4F0 File Offset: 0x0067C6F0
		protected virtual void Tick()
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompTick();
			}
		}

		// Token: 0x06015B49 RID: 88905 RVA: 0x0067E524 File Offset: 0x0067C724
		protected virtual void TickInterval(int delta)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompTickInterval(delta);
			}
		}

		// Token: 0x06015B4A RID: 88906 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void ExtraSelectionOverlaysOnGUI()
		{
		}

		// Token: 0x06015B4B RID: 88907 RVA: 0x0067E55C File Offset: 0x0067C75C
		public virtual void DrawExtraSelectionOverlays()
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].PostDrawExtraSelectionOverlays();
			}
		}

		// Token: 0x06015B4C RID: 88908 RVA: 0x0067E590 File Offset: 0x0067C790
		public virtual void PostMake()
		{
			this.InitializeComps();
		}

		// Token: 0x06015B4D RID: 88909 RVA: 0x0067E598 File Offset: 0x0067C798
		public virtual void PostAdd()
		{
			QuestUtility.SendQuestTargetSignals(this.questTags, "Spawned", this.Named("SUBJECT"));
		}

		// Token: 0x06015B4E RID: 88910 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void PositionChanged(PlanetTile previous, PlanetTile current)
		{
		}

		// Token: 0x06015B4F RID: 88911 RVA: 0x0067E5B8 File Offset: 0x0067C7B8
		public virtual void SpawnSetup()
		{
			if (!this.def.useDynamicDrawer)
			{
				Find.World.renderer.Notify_StaticWorldObjectPosChanged();
			}
			if (this.def.useDynamicDrawer)
			{
				Find.WorldDynamicDrawManager.RegisterDrawable(this);
			}
			this.Tile.Layer.FastTileFinder.DirtyTile(this.Tile);
		}

		// Token: 0x06015B50 RID: 88912 RVA: 0x0067E618 File Offset: 0x0067C818
		public virtual void PostRemove()
		{
			if (!this.def.useDynamicDrawer)
			{
				Find.World.renderer.Notify_StaticWorldObjectPosChanged();
			}
			if (this.def.useDynamicDrawer)
			{
				Find.WorldDynamicDrawManager.DeRegisterDrawable(this);
			}
			Find.WorldSelector.Deselect(this);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].PostPostRemove();
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "Despawned", this.Named("SUBJECT"));
			this.Tile.Layer.FastTileFinder.DirtyTile(this.Tile);
		}

		// Token: 0x06015B51 RID: 88913 RVA: 0x0067E6C4 File Offset: 0x0067C8C4
		public virtual void Destroy()
		{
			if (this.Destroyed)
			{
				Log.Error("Tried to destroy already-destroyed world object " + ((this != null) ? this.ToString() : null));
				return;
			}
			if (this.Spawned)
			{
				Find.WorldObjects.Remove(this);
			}
			this.destroyed = true;
			Find.FactionManager.Notify_WorldObjectDestroyed(this);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].PostDestroy();
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "Destroyed", this.Named("SUBJECT"));
		}

		// Token: 0x06015B52 RID: 88914 RVA: 0x0067E760 File Offset: 0x0067C960
		public virtual void Print(LayerSubMesh subMesh)
		{
			float averageTileSize = this.Tile.Layer.AverageTileSize;
			WorldRendererUtility.PrintQuadTangentialToPlanet(this.DrawPos, 0.7f * averageTileSize, 0.03f, subMesh, false, Rand.Range(0f, 360f), true);
		}

		// Token: 0x06015B53 RID: 88915 RVA: 0x0067E7AC File Offset: 0x0067C9AC
		public virtual void Draw()
		{
			float averageTileSize = this.Tile.Layer.AverageTileSize;
			float rawTransitionPct = ExpandableWorldObjectsUtility.RawTransitionPct;
			if (this.Tile.LayerDef.isSpace)
			{
				return;
			}
			if (!this.Material)
			{
				return;
			}
			if (this.def.expandingIcon && rawTransitionPct > 0f && !ExpandableWorldObjectsUtility.HiddenByRules(this))
			{
				Color color = this.Material.color;
				float num = 1f - rawTransitionPct;
				WorldObject.propertyBlock.SetColor(ShaderPropertyIDs.Color, new Color(color.r, color.g, color.b, color.a * num));
				WorldRendererUtility.DrawQuadTangentialToPlanet(this.DrawPos, 0.7f * averageTileSize, this.DrawAltitude, this.Material, 0f, false, false, WorldObject.propertyBlock);
				return;
			}
			WorldRendererUtility.DrawQuadTangentialToPlanet(this.DrawPos, 0.7f * averageTileSize, this.DrawAltitude, this.Material, 0f, false, false, null);
		}

		// Token: 0x06015B54 RID: 88916 RVA: 0x0067E8AC File Offset: 0x0067CAAC
		public T GetComponent<T>() where T : WorldObjectComp
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				T t = this.comps[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06015B55 RID: 88917 RVA: 0x0067E8F9 File Offset: 0x0067CAF9
		public bool TryGetComponent<T>(out T comp) where T : WorldObjectComp
		{
			comp = this.GetComponent<T>();
			return comp != null;
		}

		// Token: 0x06015B56 RID: 88918 RVA: 0x0067E918 File Offset: 0x0067CB18
		public WorldObjectComp GetComponent(Type type)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				if (type.IsInstanceOfType(this.comps[i]))
				{
					return this.comps[i];
				}
			}
			return null;
		}

		// Token: 0x06015B57 RID: 88919 RVA: 0x0067E95D File Offset: 0x0067CB5D
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			int num;
			if (this.ShowRelatedQuests)
			{
				List<Quest> quests = Find.QuestManager.QuestsListForReading;
				for (int i = 0; i < quests.Count; i = num + 1)
				{
					Quest quest = quests[i];
					if (!quest.hidden && !quest.Historical && !quest.dismissed && quest.QuestLookTargets.Contains(this))
					{
						yield return new Command_Action
						{
							defaultLabel = "CommandViewQuest".Translate(quest.name),
							defaultDesc = "CommandViewQuestDesc".Translate(),
							icon = WorldObject.ViewQuestCommandTex,
							Order = -1f,
							action = delegate()
							{
								Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
								((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(quest);
							}
						};
					}
					num = i;
				}
				quests = null;
			}
			if (DebugSettings.ShowDevGizmos)
			{
				MapParent mapParent = this as MapParent;
				if (mapParent != null && !mapParent.HasMap)
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: Generate",
						action = delegate()
						{
							LongEventHandler.QueueLongEvent(delegate()
							{
								this.SetFaction(Faction.OfPlayer);
								Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(this.Tile, new IntVec3(200, 1, 200), null, null, false);
								Current.Game.CurrentMap = orGenerateMap;
								CameraJumper.TryJump(orGenerateMap.Center, orGenerateMap, CameraJumper.MovementMode.Pan);
							}, "GeneratingMap", false, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap), true, false, null);
						}
					};
				}
			}
			for (int i = 0; i < this.comps.Count; i = num)
			{
				foreach (Gizmo gizmo in this.comps[i].GetGizmos())
				{
					yield return gizmo;
				}
				IEnumerator<Gizmo> enumerator = null;
				num = i + 1;
			}
			yield break;
			yield break;
		}

		// Token: 0x06015B58 RID: 88920 RVA: 0x0067E96D File Offset: 0x0067CB6D
		public virtual IEnumerable<Gizmo> GetCaravanGizmos(Caravan caravan)
		{
			int num;
			for (int i = 0; i < this.comps.Count; i = num + 1)
			{
				foreach (Gizmo gizmo in this.comps[i].GetCaravanGizmos(caravan))
				{
					yield return gizmo;
				}
				IEnumerator<Gizmo> enumerator = null;
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x06015B59 RID: 88921 RVA: 0x0067E984 File Offset: 0x0067CB84
		public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			int num;
			for (int i = 0; i < this.comps.Count; i = num)
			{
				foreach (FloatMenuOption floatMenuOption in this.comps[i].GetFloatMenuOptions(caravan))
				{
					yield return floatMenuOption;
				}
				IEnumerator<FloatMenuOption> enumerator = null;
				num = i + 1;
			}
			yield break;
			yield break;
		}

		// Token: 0x06015B5A RID: 88922 RVA: 0x000FB0F4 File Offset: 0x000F92F4
		public virtual IEnumerable<FloatMenuOption> GetTransportersFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction)
		{
			return Enumerable.Empty<FloatMenuOption>();
		}

		// Token: 0x06015B5B RID: 88923 RVA: 0x000FB0F4 File Offset: 0x000F92F4
		public virtual IEnumerable<FloatMenuOption> GetShuttleFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction)
		{
			return Enumerable.Empty<FloatMenuOption>();
		}

		// Token: 0x06015B5C RID: 88924 RVA: 0x0067E99B File Offset: 0x0067CB9B
		public virtual IEnumerable<InspectTabBase> GetInspectTabs()
		{
			return this.def.inspectorTabsResolved;
		}

		// Token: 0x06015B5D RID: 88925 RVA: 0x0067E9A8 File Offset: 0x0067CBA8
		public virtual bool AllMatchingObjectsOnScreenMatchesWith(WorldObject other)
		{
			return this.Faction == other.Faction;
		}

		// Token: 0x06015B5E RID: 88926 RVA: 0x0067E9B8 File Offset: 0x0067CBB8
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				base.GetType().Name,
				" ",
				this.LabelCap,
				" (tile=",
				this.Tile.ToString(),
				")"
			});
		}

		// Token: 0x06015B5F RID: 88927 RVA: 0x0067EA16 File Offset: 0x0067CC16
		public override int GetHashCode()
		{
			return this.ID;
		}

		// Token: 0x06015B60 RID: 88928 RVA: 0x0067EA1E File Offset: 0x0067CC1E
		public string GetUniqueLoadID()
		{
			return "WorldObject_" + this.ID.ToString();
		}

		// Token: 0x06015B61 RID: 88929 RVA: 0x0067EA38 File Offset: 0x0067CC38
		public virtual string GetDescription()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.def.description);
			for (int i = 0; i < this.comps.Count; i++)
			{
				string descriptionPart = this.comps[i].GetDescriptionPart();
				if (!descriptionPart.NullOrEmpty())
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.AppendLine();
						stringBuilder.AppendLine();
					}
					stringBuilder.Append(descriptionPart);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400F0F4 RID: 61684
		public WorldObjectDef def;

		// Token: 0x0400F0F5 RID: 61685
		public int ID = -1;

		// Token: 0x0400F0F6 RID: 61686
		private PlanetTile tile = PlanetTile.Invalid;

		// Token: 0x0400F0F7 RID: 61687
		private Faction factionInt;

		// Token: 0x0400F0F8 RID: 61688
		public int creationGameTicks = -1;

		// Token: 0x0400F0F9 RID: 61689
		public List<string> questTags;

		// Token: 0x0400F0FA RID: 61690
		private bool destroyed;

		// Token: 0x0400F0FB RID: 61691
		public ThingOwner<Thing> rewards;

		// Token: 0x0400F0FC RID: 61692
		private int tickDelta;

		// Token: 0x0400F0FD RID: 61693
		public bool isGeneratedLocation;

		// Token: 0x0400F0FE RID: 61694
		private List<WorldObjectComp> comps = new List<WorldObjectComp>();

		// Token: 0x0400F0FF RID: 61695
		private Material expandingMaterial;

		// Token: 0x0400F100 RID: 61696
		private int drawPosCacheTick = -1;

		// Token: 0x0400F101 RID: 61697
		private Vector3 drawPosCached;

		// Token: 0x0400F102 RID: 61698
		private static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

		// Token: 0x0400F103 RID: 61699
		private const float BaseDrawSize = 0.7f;

		// Token: 0x0400F104 RID: 61700
		private static readonly Texture2D ViewQuestCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/ViewQuest", true);

		// Token: 0x0400F105 RID: 61701
		private List<IThingHolder> tmpHolders;

		// Token: 0x0400F106 RID: 61702
		private bool cached;

		// Token: 0x0400F107 RID: 61703
		private bool cachedIsHolder;

		// Token: 0x0400F108 RID: 61704
		private IThingHolder cachedHolder;

		// Token: 0x0400F109 RID: 61705
		private IThingHolderTickable cachedTickable;
	}
}
