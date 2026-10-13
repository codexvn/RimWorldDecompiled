using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D2A RID: 7466
	public sealed class GameConditionManager : IExposable
	{
		// Token: 0x170019CD RID: 6605
		// (get) Token: 0x0600A724 RID: 42788 RVA: 0x002FE939 File Offset: 0x002FCB39
		public List<GameCondition> ActiveConditions
		{
			get
			{
				return this.activeConditions;
			}
		}

		// Token: 0x170019CE RID: 6606
		// (get) Token: 0x0600A725 RID: 42789 RVA: 0x002FE941 File Offset: 0x002FCB41
		public GameConditionManager Parent
		{
			get
			{
				if (this.ownerMap != null)
				{
					return Find.World.gameConditionManager;
				}
				return null;
			}
		}

		// Token: 0x170019CF RID: 6607
		// (get) Token: 0x0600A726 RID: 42790 RVA: 0x002FE957 File Offset: 0x002FCB57
		public float MapBrightness
		{
			get
			{
				if (!ModsConfig.AnomalyActive)
				{
					return 1f;
				}
				GameConditionManager.MapBrightnessTracker mapBrightnessTracker = this.mapBrightnessTracker;
				if (mapBrightnessTracker == null)
				{
					return 1f;
				}
				return mapBrightnessTracker.CurBrightness;
			}
		}

		// Token: 0x170019D0 RID: 6608
		// (get) Token: 0x0600A727 RID: 42791 RVA: 0x002FE97B File Offset: 0x002FCB7B
		public bool BrightnessChanging
		{
			get
			{
				if (ModsConfig.AnomalyActive)
				{
					GameConditionManager.MapBrightnessTracker mapBrightnessTracker = this.mapBrightnessTracker;
					return mapBrightnessTracker != null && mapBrightnessTracker.Changing;
				}
				return false;
			}
		}

		// Token: 0x170019D1 RID: 6609
		// (get) Token: 0x0600A728 RID: 42792 RVA: 0x002FE997 File Offset: 0x002FCB97
		public bool DarknessVisible
		{
			get
			{
				if (ModsConfig.AnomalyActive && DebugViewSettings.drawDarknessOverlay)
				{
					GameConditionManager.MapBrightnessTracker mapBrightnessTracker = this.mapBrightnessTracker;
					return mapBrightnessTracker != null && mapBrightnessTracker.DarknessVisible;
				}
				return false;
			}
		}

		// Token: 0x170019D2 RID: 6610
		// (get) Token: 0x0600A729 RID: 42793 RVA: 0x002FE9BA File Offset: 0x002FCBBA
		public bool IsAlwaysDarkOutside
		{
			get
			{
				return this.cachedAlwaysDark;
			}
		}

		// Token: 0x0600A72A RID: 42794 RVA: 0x002FE9C2 File Offset: 0x002FCBC2
		public GameConditionManager(Map map)
		{
			this.ownerMap = map;
			this.mapBrightnessTracker = new GameConditionManager.MapBrightnessTracker();
		}

		// Token: 0x0600A72B RID: 42795 RVA: 0x002FE9E7 File Offset: 0x002FCBE7
		public GameConditionManager(World world)
		{
		}

		// Token: 0x0600A72C RID: 42796 RVA: 0x002FE9FC File Offset: 0x002FCBFC
		public bool ElectricityDisabled(Map map)
		{
			foreach (GameCondition gameCondition in this.activeConditions)
			{
				if (gameCondition.ElectricityDisabled && gameCondition.CanApplyOnMap(map) && !gameCondition.HiddenByOtherCondition(map))
				{
					return true;
				}
			}
			GameConditionManager parent = this.Parent;
			return parent != null && parent.ElectricityDisabled(map);
		}

		// Token: 0x0600A72D RID: 42797 RVA: 0x002FEA7C File Offset: 0x002FCC7C
		public float FishPopulationOffsetFactorPerDay(Map map, out GameCondition culprit)
		{
			culprit = null;
			if (!ModsConfig.OdysseyActive)
			{
				return 0f;
			}
			foreach (GameCondition gameCondition in this.activeConditions)
			{
				GameCondition_GillRot gameCondition_GillRot = gameCondition as GameCondition_GillRot;
				if (gameCondition_GillRot != null && gameCondition.CanApplyOnMap(map) && !gameCondition.HiddenByOtherCondition(map))
				{
					culprit = gameCondition;
					return gameCondition_GillRot.fishPopulationOffsetFactorPerDay;
				}
			}
			return 0.025f;
		}

		// Token: 0x0600A72E RID: 42798 RVA: 0x002FEB08 File Offset: 0x002FCD08
		public void RegisterCondition(GameCondition cond)
		{
			this.activeConditions.Add(cond);
			cond.startTick = Mathf.Max(cond.startTick, Find.TickManager.TicksGame);
			cond.gameConditionManager = this;
			cond.Init();
			if (!this.cachedAlwaysDark)
			{
				this.cachedAlwaysDark = (cond.Permanent && cond is GameCondition_NoSunlight);
			}
			foreach (Map map in Find.Maps)
			{
				map.events.Notify_GameConditionAdded(cond);
			}
		}

		// Token: 0x0600A72F RID: 42799 RVA: 0x002FEBB4 File Offset: 0x002FCDB4
		public void OnConditionEnd(GameCondition cond)
		{
			this.activeConditions.Remove(cond);
			if (this.cachedAlwaysDark && cond.Permanent && cond is GameCondition_NoSunlight)
			{
				this.RecalcAlwaysDark();
			}
			foreach (Map map in Find.Maps)
			{
				map.events.Notify_GameConditionRemoved(cond);
			}
		}

		// Token: 0x0600A730 RID: 42800 RVA: 0x002FEC34 File Offset: 0x002FCE34
		public void ExposeData()
		{
			Scribe_Collections.Look<GameCondition>(ref this.activeConditions, "activeConditions", LookMode.Deep, Array.Empty<object>());
			Scribe_Deep.Look<GameConditionManager.MapBrightnessTracker>(ref this.mapBrightnessTracker, "mapBrightnessTracker", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				for (int i = 0; i < this.activeConditions.Count; i++)
				{
					this.activeConditions[i].gameConditionManager = this;
				}
				return;
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.mapBrightnessTracker == null && this.ownerMap != null)
				{
					this.mapBrightnessTracker = new GameConditionManager.MapBrightnessTracker();
				}
				this.RecalcAlwaysDark();
			}
		}

		// Token: 0x0600A731 RID: 42801 RVA: 0x002FECC8 File Offset: 0x002FCEC8
		public void GameConditionManagerTick()
		{
			for (int i = this.activeConditions.Count - 1; i >= 0; i--)
			{
				GameCondition gameCondition = this.activeConditions[i];
				if (gameCondition.Expired)
				{
					gameCondition.End();
				}
				else
				{
					gameCondition.GameConditionTick();
				}
			}
			GameConditionManager.MapBrightnessTracker mapBrightnessTracker = this.mapBrightnessTracker;
			if (mapBrightnessTracker == null)
			{
				return;
			}
			mapBrightnessTracker.Tick();
		}

		// Token: 0x0600A732 RID: 42802 RVA: 0x002FED20 File Offset: 0x002FCF20
		public void SetTargetBrightness(float target, float lerpSeconds = 5f)
		{
			GameConditionManager.MapBrightnessTracker mapBrightnessTracker = this.mapBrightnessTracker;
			if (mapBrightnessTracker != null)
			{
				mapBrightnessTracker.Change(target, lerpSeconds);
			}
			Map map = this.ownerMap;
			if (map == null)
			{
				return;
			}
			map.mapDrawer.WholeMapChanged(MapMeshFlagDefOf.GroundGlow);
		}

		// Token: 0x0600A733 RID: 42803 RVA: 0x002FED54 File Offset: 0x002FCF54
		public void GameConditionManagerDraw(Map map)
		{
			for (int i = this.activeConditions.Count - 1; i >= 0; i--)
			{
				this.activeConditions[i].GameConditionDraw(map);
			}
			GameConditionManager parent = this.Parent;
			if (parent == null)
			{
				return;
			}
			parent.GameConditionManagerDraw(map);
		}

		// Token: 0x0600A734 RID: 42804 RVA: 0x002FED9C File Offset: 0x002FCF9C
		public void DoSteadyEffects(IntVec3 c, Map map)
		{
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				this.activeConditions[i].DoCellSteadyEffects(c, map);
			}
			GameConditionManager parent = this.Parent;
			if (parent == null)
			{
				return;
			}
			parent.DoSteadyEffects(c, map);
		}

		// Token: 0x0600A735 RID: 42805 RVA: 0x002FEDE4 File Offset: 0x002FCFE4
		private void RecalcAlwaysDark()
		{
			this.cachedAlwaysDark = false;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				if (this.activeConditions[i].Permanent && this.activeConditions[i] is GameCondition_NoSunlight)
				{
					this.cachedAlwaysDark = true;
					return;
				}
			}
		}

		// Token: 0x0600A736 RID: 42806 RVA: 0x002FEE3C File Offset: 0x002FD03C
		public bool ConditionIsActive(GameConditionDef def)
		{
			return this.GetActiveCondition(def) != null;
		}

		// Token: 0x0600A737 RID: 42807 RVA: 0x002FEE48 File Offset: 0x002FD048
		public GameCondition GetActiveCondition(GameConditionDef def)
		{
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				if (def == this.activeConditions[i].def)
				{
					return this.activeConditions[i];
				}
			}
			GameConditionManager parent = this.Parent;
			if (parent == null)
			{
				return null;
			}
			return parent.GetActiveCondition(def);
		}

		// Token: 0x0600A738 RID: 42808 RVA: 0x002FEEA0 File Offset: 0x002FD0A0
		public T GetActiveCondition<T>() where T : GameCondition
		{
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				T t = this.activeConditions[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			GameConditionManager parent = this.Parent;
			if (parent == null)
			{
				return default(T);
			}
			return parent.GetActiveCondition<T>();
		}

		// Token: 0x0600A739 RID: 42809 RVA: 0x002FEF00 File Offset: 0x002FD100
		public PsychicDroneLevel GetHighestPsychicDroneLevelFor(Gender gender, Map map)
		{
			PsychicDroneLevel psychicDroneLevel = PsychicDroneLevel.None;
			for (int i = 0; i < this.ActiveConditions.Count; i++)
			{
				if (this.activeConditions[i].CanApplyOnMap(map))
				{
					GameCondition_PsychicEmanation gameCondition_PsychicEmanation = this.activeConditions[i] as GameCondition_PsychicEmanation;
					if (gameCondition_PsychicEmanation != null && gameCondition_PsychicEmanation.gender == gender && gameCondition_PsychicEmanation.level > psychicDroneLevel)
					{
						psychicDroneLevel = gameCondition_PsychicEmanation.level;
					}
				}
			}
			return psychicDroneLevel;
		}

		// Token: 0x0600A73A RID: 42810 RVA: 0x002FEF68 File Offset: 0x002FD168
		public void GetChildren(List<GameConditionManager> outChildren)
		{
			if (this == Find.World.gameConditionManager)
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					outChildren.Add(maps[i].gameConditionManager);
				}
			}
		}

		// Token: 0x0600A73B RID: 42811 RVA: 0x002FEFAC File Offset: 0x002FD1AC
		public float TotalHeightAt(float width)
		{
			float num = 0f;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				if ((this.ownerMap == null || !this.activeConditions[i].HiddenByOtherCondition(this.ownerMap)) && this.activeConditions[i].def.displayOnUI)
				{
					num += Text.CalcHeight(this.activeConditions[i].LabelCap, width - 6f);
				}
			}
			if (this.Parent != null)
			{
				num += this.Parent.TotalHeightAt(width);
			}
			return num;
		}

		// Token: 0x0600A73C RID: 42812 RVA: 0x002FF048 File Offset: 0x002FD248
		public void DoConditionsUI(Rect rect)
		{
			Widgets.BeginGroup(rect);
			float num = 0f;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				if (this.activeConditions[i].def.displayOnUI && (this.ownerMap == null || (this.activeConditions[i].CanApplyOnMap(this.ownerMap) && !this.activeConditions[i].HiddenByOtherCondition(this.ownerMap))))
				{
					string labelCap = this.activeConditions[i].LabelCap;
					Rect rect2 = new Rect(0f, num, rect.width, Text.CalcHeight(labelCap, rect.width - 6f));
					Text.Font = GameFont.Small;
					Text.Anchor = TextAnchor.MiddleRight;
					Widgets.DrawHighlightIfMouseover(rect2);
					Rect rect3 = rect2;
					rect3.width -= 6f;
					Widgets.Label(rect3, labelCap);
					if (Mouse.IsOver(rect2))
					{
						TooltipHandler.TipRegion(rect2, new TipSignal(this.activeConditions[i].TooltipString, 976090154 ^ i));
					}
					if (Widgets.ButtonInvisible(rect2, true))
					{
						if (this.activeConditions[i].conditionCauser != null && !this.activeConditions[i].hideSource && CameraJumper.CanJump(this.activeConditions[i].conditionCauser))
						{
							CameraJumper.TryJumpAndSelect(this.activeConditions[i].conditionCauser, CameraJumper.MovementMode.Pan);
						}
						else if (this.activeConditions[i].quest != null)
						{
							Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
							((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(this.activeConditions[i].quest);
						}
					}
					num += rect2.height;
				}
			}
			rect.yMin += num;
			GUI.EndGroup();
			Text.Anchor = TextAnchor.UpperLeft;
			GameConditionManager parent = this.Parent;
			if (parent == null)
			{
				return;
			}
			parent.DoConditionsUI(rect);
		}

		// Token: 0x0600A73D RID: 42813 RVA: 0x002FF258 File Offset: 0x002FD458
		public void GetAllGameConditionsAffectingMap(Map map, List<GameCondition> listToFill)
		{
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				if (this.activeConditions[i].CanApplyOnMap(map))
				{
					listToFill.Add(this.activeConditions[i]);
				}
			}
			if (this.Parent != null)
			{
				this.Parent.GetAllGameConditionsAffectingMap(map, listToFill);
			}
		}

		// Token: 0x0600A73E RID: 42814 RVA: 0x002FF2B8 File Offset: 0x002FD4B8
		internal float AggregateTemperatureOffset()
		{
			float num = 0f;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				if (this.activeConditions[i].CanApplyOnMap(this.ownerMap))
				{
					num += this.activeConditions[i].TemperatureOffset();
				}
			}
			if (this.Parent != null)
			{
				num += this.Parent.AggregateTemperatureOffset();
			}
			return num;
		}

		// Token: 0x0600A73F RID: 42815 RVA: 0x002FF328 File Offset: 0x002FD528
		internal float AggregateAnimalDensityFactor(Map map)
		{
			float num = 1f;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				if (this.activeConditions[i].CanApplyOnMap(this.ownerMap))
				{
					num *= this.activeConditions[i].AnimalDensityFactor(map);
				}
			}
			if (this.Parent != null)
			{
				num *= this.Parent.AggregateAnimalDensityFactor(map);
			}
			return num;
		}

		// Token: 0x0600A740 RID: 42816 RVA: 0x002FF398 File Offset: 0x002FD598
		internal float AggregatePlantDensityFactor(Map map)
		{
			float num = 1f;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				if (this.activeConditions[i].CanApplyOnMap(this.ownerMap))
				{
					num *= this.activeConditions[i].PlantDensityFactor(map);
				}
			}
			if (this.Parent != null)
			{
				num *= this.Parent.AggregatePlantDensityFactor(map);
			}
			return num;
		}

		// Token: 0x0600A741 RID: 42817 RVA: 0x002FF408 File Offset: 0x002FD608
		internal float AggregateSkyGazeJoyGainFactor(Map map)
		{
			float num = 1f;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				if (this.activeConditions[i].CanApplyOnMap(this.ownerMap))
				{
					num *= this.activeConditions[i].SkyGazeJoyGainFactor(map);
				}
			}
			if (this.Parent != null)
			{
				num *= this.Parent.AggregateSkyGazeJoyGainFactor(map);
			}
			return num;
		}

		// Token: 0x0600A742 RID: 42818 RVA: 0x002FF478 File Offset: 0x002FD678
		internal float AggregateSkyGazeChanceFactor(Map map)
		{
			float num = 1f;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				if (this.activeConditions[i].CanApplyOnMap(this.ownerMap))
				{
					num *= this.activeConditions[i].SkyGazeChanceFactor(map);
				}
			}
			if (this.Parent != null)
			{
				num *= this.Parent.AggregateSkyGazeChanceFactor(map);
			}
			return num;
		}

		// Token: 0x0600A743 RID: 42819 RVA: 0x002FF4E8 File Offset: 0x002FD6E8
		internal bool AllowEnjoyableOutsideNow(Map map)
		{
			GameConditionDef gameConditionDef;
			return this.AllowEnjoyableOutsideNow(map, out gameConditionDef);
		}

		// Token: 0x0600A744 RID: 42820 RVA: 0x002FF500 File Offset: 0x002FD700
		internal bool AllowEnjoyableOutsideNow(Map map, out GameConditionDef reason)
		{
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				GameCondition gameCondition = this.activeConditions[i];
				if (this.activeConditions[i].CanApplyOnMap(this.ownerMap) && !gameCondition.AllowEnjoyableOutsideNow(map))
				{
					reason = gameCondition.def;
					return false;
				}
			}
			reason = null;
			return this.Parent == null || this.Parent.AllowEnjoyableOutsideNow(map, out reason);
		}

		// Token: 0x0600A745 RID: 42821 RVA: 0x002FF578 File Offset: 0x002FD778
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (GameCondition saveable in this.activeConditions)
			{
				stringBuilder.AppendLine(Scribe.saver.DebugOutputFor(saveable));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040073DA RID: 29658
		public Map ownerMap;

		// Token: 0x040073DB RID: 29659
		private List<GameCondition> activeConditions = new List<GameCondition>();

		// Token: 0x040073DC RID: 29660
		private GameConditionManager.MapBrightnessTracker mapBrightnessTracker;

		// Token: 0x040073DD RID: 29661
		private bool cachedAlwaysDark;

		// Token: 0x040073DE RID: 29662
		private const float TextPadding = 6f;

		// Token: 0x02001D2B RID: 7467
		private class MapBrightnessTracker : IExposable
		{
			// Token: 0x170019D3 RID: 6611
			// (get) Token: 0x0600A746 RID: 42822 RVA: 0x002FF5E4 File Offset: 0x002FD7E4
			public float CurBrightness
			{
				get
				{
					return Mathf.Lerp(this.brightness, this.targetBrightness, this.lerp);
				}
			}

			// Token: 0x170019D4 RID: 6612
			// (get) Token: 0x0600A747 RID: 42823 RVA: 0x002FF5FD File Offset: 0x002FD7FD
			public bool Changing
			{
				get
				{
					return this.lerp < 1f;
				}
			}

			// Token: 0x170019D5 RID: 6613
			// (get) Token: 0x0600A748 RID: 42824 RVA: 0x002FF60C File Offset: 0x002FD80C
			public bool DarknessVisible
			{
				get
				{
					return this.Changing || this.CurBrightness < 1f;
				}
			}

			// Token: 0x0600A749 RID: 42825 RVA: 0x002FF625 File Offset: 0x002FD825
			public void Change(float newBrightness, float lerpSeconds = 5f)
			{
				if (!ModLister.CheckAnomaly("Map brightness"))
				{
					return;
				}
				this.brightness = this.CurBrightness;
				this.targetBrightness = newBrightness;
				this.lerp = 0f;
				this.lerpSeconds = lerpSeconds;
			}

			// Token: 0x0600A74A RID: 42826 RVA: 0x002FF659 File Offset: 0x002FD859
			public void Tick()
			{
				if (ModsConfig.AnomalyActive && this.lerp < 1f)
				{
					this.lerp += this.lerpSeconds / 60f * Time.deltaTime;
				}
			}

			// Token: 0x0600A74B RID: 42827 RVA: 0x002FF690 File Offset: 0x002FD890
			public void ExposeData()
			{
				Scribe_Values.Look<float>(ref this.brightness, "brightness", 1f, false);
				Scribe_Values.Look<float>(ref this.targetBrightness, "targetBrightness", 1f, false);
				Scribe_Values.Look<float>(ref this.lerp, "lerp", 1f, false);
				Scribe_Values.Look<float>(ref this.lerpSeconds, "lerpSeconds", 5f, false);
			}

			// Token: 0x040073DF RID: 29663
			private float brightness = 1f;

			// Token: 0x040073E0 RID: 29664
			private float targetBrightness = 1f;

			// Token: 0x040073E1 RID: 29665
			private float lerp = 1f;

			// Token: 0x040073E2 RID: 29666
			private float lerpSeconds;
		}
	}
}
