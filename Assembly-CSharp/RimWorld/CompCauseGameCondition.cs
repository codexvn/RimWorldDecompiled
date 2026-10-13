using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BB4 RID: 11188
	public class CompCauseGameCondition : ThingComp
	{
		// Token: 0x17002735 RID: 10037
		// (get) Token: 0x0600FA23 RID: 64035 RVA: 0x0048B920 File Offset: 0x00489B20
		public CompProperties_CausesGameCondition Props
		{
			get
			{
				return (CompProperties_CausesGameCondition)this.props;
			}
		}

		// Token: 0x17002736 RID: 10038
		// (get) Token: 0x0600FA24 RID: 64036 RVA: 0x0048B92D File Offset: 0x00489B2D
		public GameConditionDef ConditionDef
		{
			get
			{
				return this.Props.conditionDef;
			}
		}

		// Token: 0x17002737 RID: 10039
		// (get) Token: 0x0600FA25 RID: 64037 RVA: 0x0048B93A File Offset: 0x00489B3A
		public IEnumerable<GameCondition> CausedConditions
		{
			get
			{
				return this.causedConditions.Values;
			}
		}

		// Token: 0x17002738 RID: 10040
		// (get) Token: 0x0600FA26 RID: 64038 RVA: 0x0048B947 File Offset: 0x00489B47
		public virtual bool Active
		{
			get
			{
				return this.initiatableComp == null || this.initiatableComp.Initiated;
			}
		}

		// Token: 0x17002739 RID: 10041
		// (get) Token: 0x0600FA27 RID: 64039 RVA: 0x0048B95E File Offset: 0x00489B5E
		public PlanetTile MyTile
		{
			get
			{
				if (this.siteLink != null)
				{
					return this.siteLink.Tile;
				}
				if (this.parent.SpawnedOrAnyParentSpawned)
				{
					return this.parent.Tile;
				}
				return PlanetTile.Invalid;
			}
		}

		// Token: 0x0600FA28 RID: 64040 RVA: 0x0048B992 File Offset: 0x00489B92
		public void LinkWithSite(Site site)
		{
			this.siteLink = site;
		}

		// Token: 0x0600FA29 RID: 64041 RVA: 0x0048B99B File Offset: 0x00489B9B
		public override void PostPostMake()
		{
			base.PostPostMake();
			this.CacheComps();
		}

		// Token: 0x0600FA2A RID: 64042 RVA: 0x0048B9A9 File Offset: 0x00489BA9
		private void CacheComps()
		{
			this.initiatableComp = this.parent.GetComp<CompInitiatable>();
		}

		// Token: 0x0600FA2B RID: 64043 RVA: 0x0048B9BC File Offset: 0x00489BBC
		public override void PostExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.causedConditions.RemoveAll((KeyValuePair<Map, GameCondition> x) => !Find.Maps.Contains(x.Key));
			}
			Scribe_References.Look<Site>(ref this.siteLink, "siteLink", false);
			Scribe_Collections.Look<Map, GameCondition>(ref this.causedConditions, "causedConditions", LookMode.Reference, LookMode.Reference);
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				this.causedConditions.RemoveAll((KeyValuePair<Map, GameCondition> x) => x.Value == null);
				foreach (KeyValuePair<Map, GameCondition> keyValuePair in this.causedConditions)
				{
					keyValuePair.Value.conditionCauser = this.parent;
					keyValuePair.Value.hideSource = this.Props.hideSource;
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.CacheComps();
			}
		}

		// Token: 0x0600FA2C RID: 64044 RVA: 0x0048BACC File Offset: 0x00489CCC
		public bool InAoE(PlanetTile tile)
		{
			return this.MyTile.Valid && tile.Valid && this.Active && (tile == this.MyTile || (this.Props.worldRange > 0 && tile.Layer == this.MyTile.Layer && Find.WorldGrid.ApproxDistanceInTiles(tile, this.MyTile) < (float)this.Props.worldRange));
		}

		// Token: 0x0600FA2D RID: 64045 RVA: 0x0048BB54 File Offset: 0x00489D54
		protected GameCondition GetConditionInstance(Map map)
		{
			GameCondition activeCondition;
			if (!this.causedConditions.TryGetValue(map, out activeCondition) && this.Props.preventConditionStacking)
			{
				activeCondition = map.GameConditionManager.GetActiveCondition(this.Props.conditionDef);
				if (activeCondition != null)
				{
					this.causedConditions.Add(map, activeCondition);
					this.SetupCondition(activeCondition, map);
				}
			}
			return activeCondition;
		}

		// Token: 0x0600FA2E RID: 64046 RVA: 0x0048BBB0 File Offset: 0x00489DB0
		public override void CompTick()
		{
			if (this.Active)
			{
				foreach (Map map in Find.Maps)
				{
					if (this.InAoE(map.Tile))
					{
						this.EnforceConditionOn(map);
					}
				}
			}
			CompCauseGameCondition.tmpDeadConditionMaps.Clear();
			foreach (KeyValuePair<Map, GameCondition> keyValuePair in this.causedConditions)
			{
				if (keyValuePair.Value.Expired || !keyValuePair.Key.GameConditionManager.ConditionIsActive(keyValuePair.Value.def))
				{
					CompCauseGameCondition.tmpDeadConditionMaps.Add(keyValuePair.Key);
				}
			}
			foreach (Map key in CompCauseGameCondition.tmpDeadConditionMaps)
			{
				this.causedConditions.Remove(key);
			}
		}

		// Token: 0x0600FA2F RID: 64047 RVA: 0x0048BCE8 File Offset: 0x00489EE8
		private GameCondition EnforceConditionOn(Map map)
		{
			GameCondition gameCondition = this.GetConditionInstance(map);
			if (gameCondition == null)
			{
				gameCondition = this.CreateConditionOn(map);
			}
			else
			{
				gameCondition.TicksLeft = gameCondition.TransitionTicks;
			}
			return gameCondition;
		}

		// Token: 0x0600FA30 RID: 64048 RVA: 0x0048BD18 File Offset: 0x00489F18
		protected virtual GameCondition CreateConditionOn(Map map)
		{
			GameCondition gameCondition = GameConditionMaker.MakeCondition(this.ConditionDef, -1);
			gameCondition.Duration = gameCondition.TransitionTicks;
			gameCondition.conditionCauser = this.parent;
			gameCondition.hideSource = this.Props.hideSource;
			map.gameConditionManager.RegisterCondition(gameCondition);
			this.causedConditions.Add(map, gameCondition);
			this.SetupCondition(gameCondition, map);
			return gameCondition;
		}

		// Token: 0x0600FA31 RID: 64049 RVA: 0x0048BD7D File Offset: 0x00489F7D
		protected virtual void SetupCondition(GameCondition condition, Map map)
		{
			condition.suppressEndMessage = true;
		}

		// Token: 0x0600FA32 RID: 64050 RVA: 0x0048BD88 File Offset: 0x00489F88
		protected void ReSetupAllConditions()
		{
			foreach (KeyValuePair<Map, GameCondition> keyValuePair in this.causedConditions)
			{
				this.SetupCondition(keyValuePair.Value, keyValuePair.Key);
			}
		}

		// Token: 0x0600FA33 RID: 64051 RVA: 0x0048BDE8 File Offset: 0x00489FE8
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			Messages.Message("MessageConditionCauserDespawned".Translate(this.parent.def.LabelCap), new TargetInfo(this.parent.Position, previousMap, false), MessageTypeDefOf.NeutralEvent, true);
		}

		// Token: 0x0600FA34 RID: 64052 RVA: 0x0048BE3C File Offset: 0x0048A03C
		public override string CompInspectStringExtra()
		{
			if (!DebugSettings.godMode)
			{
				return base.CompInspectStringExtra();
			}
			GameCondition gameCondition = this.parent.Map.GameConditionManager.ActiveConditions.Find((GameCondition c) => c.def == this.Props.conditionDef);
			if (gameCondition == null)
			{
				return base.CompInspectStringExtra();
			}
			return "[DEV] Current map condition\n[DEV] Ticks Passed: " + gameCondition.TicksPassed.ToString() + "\n[DEV] Ticks Left: " + gameCondition.TicksLeft.ToString();
		}

		// Token: 0x0600FA35 RID: 64053 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void RandomizeSettings(Site site)
		{
		}

		// Token: 0x0400A46B RID: 42091
		protected CompInitiatable initiatableComp;

		// Token: 0x0400A46C RID: 42092
		protected Site siteLink;

		// Token: 0x0400A46D RID: 42093
		private Dictionary<Map, GameCondition> causedConditions = new Dictionary<Map, GameCondition>();

		// Token: 0x0400A46E RID: 42094
		private static List<Map> tmpDeadConditionMaps = new List<Map>();
	}
}
