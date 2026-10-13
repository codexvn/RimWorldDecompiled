using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002499 RID: 9369
	public class Pawn_NeedsTracker : IExposable
	{
		// Token: 0x17001EF2 RID: 7922
		// (get) Token: 0x0600CB21 RID: 52001 RVA: 0x003B80FB File Offset: 0x003B62FB
		public List<Need> AllNeeds
		{
			get
			{
				return this.needs;
			}
		}

		// Token: 0x17001EF3 RID: 7923
		// (get) Token: 0x0600CB22 RID: 52002 RVA: 0x003B8103 File Offset: 0x003B6303
		public List<Need> MiscNeeds
		{
			get
			{
				return this.needsMisc;
			}
		}

		// Token: 0x0600CB23 RID: 52003 RVA: 0x003B810B File Offset: 0x003B630B
		public Pawn_NeedsTracker()
		{
		}

		// Token: 0x0600CB24 RID: 52004 RVA: 0x003B812A File Offset: 0x003B632A
		public Pawn_NeedsTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			this.AddOrRemoveNeedsAsAppropriate();
		}

		// Token: 0x0600CB25 RID: 52005 RVA: 0x003B8158 File Offset: 0x003B6358
		public void ExposeData()
		{
			Scribe_Collections.Look<Need>(ref this.needs, "needs", LookMode.Deep, new object[]
			{
				this.pawn
			});
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.needs.RemoveAll((Need x) => ((x != null) ? x.def : null) == null) != 0)
				{
					Log.Error("Pawn " + this.pawn.ToStringSafe<Pawn>() + " had some null needs after loading.");
				}
				this.BindDirectNeedFields();
				this.CacheMiscNeeds();
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0600CB26 RID: 52006 RVA: 0x003B81F4 File Offset: 0x003B63F4
		public void BindDirectNeedFields()
		{
			this.TryGetNeed<Need_Mood>(out this.mood);
			this.TryGetNeed<Need_Food>(out this.food);
			this.TryGetNeed<Need_MechEnergy>(out this.energy);
			this.TryGetNeed<Need_Rest>(out this.rest);
			this.TryGetNeed<Need_Joy>(out this.joy);
			this.TryGetNeed<Need_Beauty>(out this.beauty);
			this.TryGetNeed<Need_Comfort>(out this.comfort);
			this.TryGetNeed<Need_RoomSize>(out this.roomsize);
			this.TryGetNeed<Need_Outdoors>(out this.outdoors);
			this.TryGetNeed<Need_Indoors>(out this.indoors);
			this.TryGetNeed<Need_Chemical_Any>(out this.drugsDesire);
			this.TryGetNeed<Need_Learning>(out this.learning);
			this.TryGetNeed<Need_Play>(out this.play);
		}

		// Token: 0x0600CB27 RID: 52007 RVA: 0x003B82AA File Offset: 0x003B64AA
		private void CacheMiscNeeds()
		{
			this.needsMisc.Clear();
		}

		// Token: 0x0600CB28 RID: 52008 RVA: 0x003B82B8 File Offset: 0x003B64B8
		public void NeedsTrackerTickInterval(int delta)
		{
			if (!this.pawn.IsHashIntervalTick(150, delta))
			{
				return;
			}
			for (int i = 0; i < this.needs.Count; i++)
			{
				this.needs[i].NeedInterval();
			}
		}

		// Token: 0x0600CB29 RID: 52009 RVA: 0x003B8300 File Offset: 0x003B6500
		public T TryGetNeed<T>() where T : Need
		{
			T result;
			this.TryGetNeed<T>(out result);
			return result;
		}

		// Token: 0x0600CB2A RID: 52010 RVA: 0x003B8318 File Offset: 0x003B6518
		public bool TryGetNeed<T>(out T need) where T : Need
		{
			for (int i = 0; i < this.needs.Count; i++)
			{
				if (this.needs[i].GetType() == typeof(T))
				{
					need = (T)((object)this.needs[i]);
					return true;
				}
			}
			need = default(T);
			return false;
		}

		// Token: 0x0600CB2B RID: 52011 RVA: 0x003B8380 File Offset: 0x003B6580
		public Need TryGetNeed(NeedDef def)
		{
			Need result;
			this.TryGetNeed(def, out result);
			return result;
		}

		// Token: 0x0600CB2C RID: 52012 RVA: 0x003B8398 File Offset: 0x003B6598
		public bool TryGetNeed(NeedDef def, out Need need)
		{
			for (int i = 0; i < this.needs.Count; i++)
			{
				if (this.needs[i].def == def)
				{
					need = this.needs[i];
					return true;
				}
			}
			need = null;
			return false;
		}

		// Token: 0x0600CB2D RID: 52013 RVA: 0x003B83E4 File Offset: 0x003B65E4
		public void SetInitialLevels()
		{
			this.pawn.GetStatValue(StatDefOf.MaxNutrition, true, 0);
			for (int i = 0; i < this.needs.Count; i++)
			{
				this.needs[i].SetInitialLevel();
			}
		}

		// Token: 0x0600CB2E RID: 52014 RVA: 0x003B842C File Offset: 0x003B662C
		public void AddOrRemoveNeedsAsAppropriate()
		{
			List<NeedDef> allDefsListForReading = DefDatabase<NeedDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				try
				{
					NeedDef needDef = allDefsListForReading[i];
					Need need;
					if (this.ShouldHaveNeed(needDef))
					{
						if (!this.TryGetNeed(needDef, out need))
						{
							this.AddNeed(needDef);
						}
					}
					else if (this.TryGetNeed(needDef, out need))
					{
						this.RemoveNeed(needDef);
					}
				}
				catch (Exception arg)
				{
					Log.Error(string.Format("Error while determining if {0} should have Need {1}: {2}", this.pawn.ToStringSafe<Pawn>(), allDefsListForReading[i].ToStringSafe<NeedDef>(), arg));
				}
			}
		}

		// Token: 0x17001EF4 RID: 7924
		// (get) Token: 0x0600CB2F RID: 52015 RVA: 0x003B84C8 File Offset: 0x003B66C8
		public bool PrefersOutdoors
		{
			get
			{
				Need need;
				return !this.TryGetNeed(NeedDefOf.Indoors, out need);
			}
		}

		// Token: 0x17001EF5 RID: 7925
		// (get) Token: 0x0600CB30 RID: 52016 RVA: 0x003B84E8 File Offset: 0x003B66E8
		public bool PrefersIndoors
		{
			get
			{
				Need need;
				return this.TryGetNeed(NeedDefOf.Indoors, out need);
			}
		}

		// Token: 0x0600CB31 RID: 52017 RVA: 0x003B8504 File Offset: 0x003B6704
		private bool ShouldHaveNeed(NeedDef nd)
		{
			if (this.pawn.RaceProps.intelligence < nd.minIntelligence)
			{
				return false;
			}
			if (!nd.developmentalStageFilter.Has(this.pawn.DevelopmentalStage))
			{
				return false;
			}
			if (nd.colonistsOnly && (this.pawn.Faction == null || !this.pawn.Faction.IsPlayer))
			{
				return false;
			}
			if (nd.playerMechsOnly && (!this.pawn.RaceProps.IsMechanoid || this.pawn.Faction != Faction.OfPlayer || this.pawn.OverseerSubject == null))
			{
				return false;
			}
			if (nd.colonistAndPrisonersOnly && (this.pawn.Faction == null || !this.pawn.Faction.IsPlayer) && (this.pawn.HostFaction == null || this.pawn.HostFaction != Faction.OfPlayer))
			{
				return false;
			}
			if (this.pawn.health.hediffSet.DisablesNeed(nd))
			{
				return false;
			}
			if (ModsConfig.BiotechActive && this.pawn.genes != null && this.pawn.genes.DisablesNeed(nd))
			{
				return false;
			}
			Pawn_StoryTracker story = this.pawn.story;
			if (((story != null) ? story.traits : null) != null && this.pawn.story.traits.DisablesNeed(nd))
			{
				return false;
			}
			if (this.pawn.Ideo != null && this.pawn.Ideo.DisablesNeed(nd))
			{
				return false;
			}
			bool flag = false;
			bool flag2 = false;
			if (nd.onlyIfCausedByHediff)
			{
				flag = true;
				if (this.pawn.health.hediffSet.EnablesNeed(nd))
				{
					flag2 = true;
				}
			}
			if (ModsConfig.BiotechActive && nd.onlyIfCausedByGene)
			{
				flag = true;
				if (this.pawn.genes != null && this.pawn.genes.EnablesNeed(nd))
				{
					flag2 = true;
				}
			}
			if (ModsConfig.IdeologyActive && nd.onlyIfCausedByIdeo)
			{
				flag = true;
				if (this.pawn.Ideo != null && this.pawn.Ideo.EnablesNeed(nd))
				{
					flag2 = true;
				}
			}
			if (nd.onlyIfCausedByTrait)
			{
				flag = true;
				Pawn_StoryTracker story2 = this.pawn.story;
				if (((story2 != null) ? story2.traits : null) != null && this.pawn.story.traits.EnablesNeed(nd))
				{
					flag2 = true;
				}
			}
			if (flag && !flag2)
			{
				return false;
			}
			if (nd.neverOnPrisoner && this.pawn.IsPrisoner)
			{
				return false;
			}
			if (nd.neverOnSlave && this.pawn.IsSlave)
			{
				return false;
			}
			if (this.pawn.IsMutant && this.pawn.mutant.Def.disableNeeds && (this.pawn.mutant.Def.needWhitelist == null || !this.pawn.mutant.Def.needWhitelist.Contains(nd)))
			{
				return false;
			}
			if (nd.titleRequiredAny != null)
			{
				if (this.pawn.royalty == null)
				{
					return false;
				}
				bool flag3 = false;
				foreach (RoyalTitle royalTitle in this.pawn.royalty.AllTitlesInEffectForReading)
				{
					if (nd.titleRequiredAny.Contains(royalTitle.def))
					{
						flag3 = true;
						break;
					}
				}
				if (!flag3)
				{
					return false;
				}
			}
			if (nd.nullifyingPrecepts != null && this.pawn.Ideo != null)
			{
				bool flag4 = false;
				foreach (PreceptDef preceptDef in nd.nullifyingPrecepts)
				{
					if (this.pawn.Ideo.HasPrecept(preceptDef))
					{
						flag4 = true;
						break;
					}
				}
				if (flag4)
				{
					return false;
				}
			}
			if (nd.hediffRequiredAny != null)
			{
				bool flag5 = false;
				foreach (HediffDef def in nd.hediffRequiredAny)
				{
					if (this.pawn.health.hediffSet.HasHediff(def, false))
					{
						flag5 = true;
						break;
					}
				}
				if (!flag5)
				{
					return false;
				}
			}
			if (nd.defName == "Authority")
			{
				return false;
			}
			if (nd.slavesOnly && !this.pawn.IsSlave)
			{
				return false;
			}
			if (ModsConfig.AnomalyActive && nd.requiredComps != null)
			{
				foreach (CompProperties properties in nd.requiredComps)
				{
					if (this.pawn.TryGetComp(properties) == null)
					{
						return false;
					}
				}
			}
			if (nd == NeedDefOf.Food)
			{
				return this.pawn.RaceProps.EatsFood;
			}
			return nd != NeedDefOf.Rest || this.pawn.RaceProps.needsRest;
		}

		// Token: 0x0600CB32 RID: 52018 RVA: 0x003B8A10 File Offset: 0x003B6C10
		private void AddNeed(NeedDef nd)
		{
			Need need = (Need)Activator.CreateInstance(nd.needClass, new object[]
			{
				this.pawn
			});
			need.def = nd;
			this.needs.Add(need);
			need.SetInitialLevel();
			this.BindDirectNeedFields();
		}

		// Token: 0x0600CB33 RID: 52019 RVA: 0x003B8A5C File Offset: 0x003B6C5C
		private void RemoveNeed(NeedDef nd)
		{
			Need need;
			if (!this.TryGetNeed(nd, out need))
			{
				return;
			}
			need.OnNeedRemoved();
			this.needs.Remove(need);
			this.BindDirectNeedFields();
		}

		// Token: 0x0600CB34 RID: 52020 RVA: 0x003B8A8E File Offset: 0x003B6C8E
		public IEnumerable<Gizmo> GetGizmos()
		{
			if (!DebugSettings.ShowDevGizmos)
			{
				yield break;
			}
			if (this.energy != null)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Mech energy +5%",
					action = delegate()
					{
						this.energy.CurLevelPercentage += 0.05f;
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Mech energy -5%",
					action = delegate()
					{
						this.energy.CurLevelPercentage -= 0.05f;
					}
				};
			}
			yield break;
		}

		// Token: 0x04008A90 RID: 35472
		private Pawn pawn;

		// Token: 0x04008A91 RID: 35473
		private List<Need> needs = new List<Need>();

		// Token: 0x04008A92 RID: 35474
		public Need_Mood mood;

		// Token: 0x04008A93 RID: 35475
		public Need_Food food;

		// Token: 0x04008A94 RID: 35476
		public Need_MechEnergy energy;

		// Token: 0x04008A95 RID: 35477
		public Need_Rest rest;

		// Token: 0x04008A96 RID: 35478
		public Need_Joy joy;

		// Token: 0x04008A97 RID: 35479
		public Need_Beauty beauty;

		// Token: 0x04008A98 RID: 35480
		public Need_RoomSize roomsize;

		// Token: 0x04008A99 RID: 35481
		public Need_Outdoors outdoors;

		// Token: 0x04008A9A RID: 35482
		public Need_Indoors indoors;

		// Token: 0x04008A9B RID: 35483
		public Need_Chemical_Any drugsDesire;

		// Token: 0x04008A9C RID: 35484
		public Need_Comfort comfort;

		// Token: 0x04008A9D RID: 35485
		public Need_Learning learning;

		// Token: 0x04008A9E RID: 35486
		public Need_Play play;

		// Token: 0x04008A9F RID: 35487
		private List<Need> needsMisc = new List<Need>(0);
	}
}
