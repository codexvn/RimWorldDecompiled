using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002531 RID: 9521
	public sealed class SituationalThoughtHandler
	{
		// Token: 0x0600D041 RID: 53313 RVA: 0x003D3F9D File Offset: 0x003D219D
		public SituationalThoughtHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600D042 RID: 53314 RVA: 0x003D3FD4 File Offset: 0x003D21D4
		public void SituationalThoughtInterval()
		{
			this.UpdateAllMoodThoughts();
			this.RemoveExpiredThoughtsFromCache();
		}

		// Token: 0x0600D043 RID: 53315 RVA: 0x003D3FE4 File Offset: 0x003D21E4
		public void AppendMoodThoughts(List<Thought> outThoughts)
		{
			if (this.thoughtsDirty)
			{
				this.UpdateAllMoodThoughts();
			}
			foreach (Thought_Situational thought_Situational in this.cachedThoughts)
			{
				if (thought_Situational.Active)
				{
					outThoughts.Add(thought_Situational);
				}
			}
		}

		// Token: 0x0600D044 RID: 53316 RVA: 0x003D4050 File Offset: 0x003D2250
		public void AppendMoodThoughts(ThoughtDef def, List<Thought> thoughts)
		{
			bool flag = false;
			foreach (Thought_Situational thought_Situational in this.cachedThoughts)
			{
				if (thought_Situational.def == def && thought_Situational.Active)
				{
					thought_Situational.RecalculateState();
					if (thought_Situational.Active)
					{
						flag = true;
						thoughts.Add(thought_Situational);
					}
				}
			}
			if (flag)
			{
				return;
			}
			Thought_Situational thought_Situational2 = this.TryCreateThought(def);
			if (thought_Situational2 != null)
			{
				this.cachedThoughts.Add(thought_Situational2);
				thoughts.Add(thought_Situational2);
			}
		}

		// Token: 0x0600D045 RID: 53317 RVA: 0x003D40E8 File Offset: 0x003D22E8
		public void AppendSocialThoughts(Pawn otherPawn, List<ISocialThought> outThoughts)
		{
			this.CheckRecalculateSocialThoughts(otherPawn);
			SituationalThoughtHandler.CachedSocialThoughts cachedSocialThoughts = this.cachedSocialThoughts[otherPawn];
			cachedSocialThoughts.lastQueryTick = Find.TickManager.TicksGame;
			List<Thought_SituationalSocial> activeThoughts = cachedSocialThoughts.activeThoughts;
			for (int i = 0; i < activeThoughts.Count; i++)
			{
				outThoughts.Add(activeThoughts[i]);
			}
		}

		// Token: 0x0600D046 RID: 53318 RVA: 0x003D413C File Offset: 0x003D233C
		private void UpdateAllMoodThoughts()
		{
			this.thoughtsDirty = false;
			List<ThoughtDef> situationalNonSocialThoughtDefs = ThoughtUtility.situationalNonSocialThoughtDefs;
			foreach (Thought_Situational thought_Situational in this.cachedThoughts)
			{
				thought_Situational.RecalculateState();
			}
			foreach (ThoughtDef thoughtDef in situationalNonSocialThoughtDefs)
			{
				bool flag = false;
				using (List<Thought_Situational>.Enumerator enumerator = this.cachedThoughts.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.def == thoughtDef)
						{
							flag = true;
						}
					}
				}
				if (!flag)
				{
					Thought_Situational thought_Situational2 = this.TryCreateThought(thoughtDef);
					if (thought_Situational2 != null)
					{
						this.cachedThoughts.Add(thought_Situational2);
					}
				}
			}
			if (ModsConfig.IdeologyActive && this.pawn.Ideo != null)
			{
				foreach (Precept precept in this.pawn.Ideo.PreceptsListForReading)
				{
					foreach (Thought_Situational item in precept.SituationThoughtsToAdd(this.pawn, this.cachedThoughts))
					{
						this.cachedThoughts.Add(item);
					}
				}
			}
		}

		// Token: 0x0600D047 RID: 53319 RVA: 0x003D42E8 File Offset: 0x003D24E8
		private void CheckRecalculateSocialThoughts(Pawn otherPawn)
		{
			try
			{
				SituationalThoughtHandler.CachedSocialThoughts cachedSocialThoughts;
				if (!this.cachedSocialThoughts.TryGetValue(otherPawn, out cachedSocialThoughts))
				{
					cachedSocialThoughts = new SituationalThoughtHandler.CachedSocialThoughts();
					this.cachedSocialThoughts.Add(otherPawn, cachedSocialThoughts);
				}
				if (cachedSocialThoughts.ShouldRecalculateState)
				{
					cachedSocialThoughts.lastRecalculationTick = Find.TickManager.TicksGame;
					this.tmpCachedSocialThoughts.Clear();
					for (int i = 0; i < cachedSocialThoughts.thoughts.Count; i++)
					{
						Thought_SituationalSocial thought_SituationalSocial = cachedSocialThoughts.thoughts[i];
						thought_SituationalSocial.RecalculateState();
						this.tmpCachedSocialThoughts.Add(thought_SituationalSocial.def);
					}
					List<ThoughtDef> situationalSocialThoughtDefs = ThoughtUtility.situationalSocialThoughtDefs;
					int j = 0;
					int count = situationalSocialThoughtDefs.Count;
					while (j < count)
					{
						if (!this.tmpCachedSocialThoughts.Contains(situationalSocialThoughtDefs[j]))
						{
							Thought_SituationalSocial thought_SituationalSocial2 = this.TryCreateSocialThought(situationalSocialThoughtDefs[j], otherPawn);
							if (thought_SituationalSocial2 != null)
							{
								cachedSocialThoughts.thoughts.Add(thought_SituationalSocial2);
							}
						}
						j++;
					}
					cachedSocialThoughts.activeThoughts.Clear();
					for (int k = 0; k < cachedSocialThoughts.thoughts.Count; k++)
					{
						Thought_SituationalSocial thought_SituationalSocial3 = cachedSocialThoughts.thoughts[k];
						if (thought_SituationalSocial3.Active)
						{
							cachedSocialThoughts.activeThoughts.Add(thought_SituationalSocial3);
						}
					}
				}
			}
			catch (Exception arg)
			{
				Log.Error(string.Format("Exception when recalculating social thoughts for pawn {0}: {1}", this.pawn, arg));
			}
			finally
			{
			}
		}

		// Token: 0x0600D048 RID: 53320 RVA: 0x003D4470 File Offset: 0x003D2670
		private Thought_Situational TryCreateThought(ThoughtDef def)
		{
			Thought_Situational thought_Situational = null;
			try
			{
				if (!ThoughtUtility.CanGetThought(this.pawn, def, false))
				{
					return null;
				}
				if (!def.Worker.CurrentState(this.pawn).ActiveFor(def))
				{
					return null;
				}
				thought_Situational = (Thought_Situational)ThoughtMaker.MakeThought(def);
				thought_Situational.pawn = this.pawn;
				if (def.Worker is ThoughtWorker_Precept)
				{
					thought_Situational.sourcePrecept = this.pawn.Ideo.GetFirstPreceptAllowingSituationalThought(def);
				}
				thought_Situational.RecalculateState();
			}
			catch (Exception ex)
			{
				string[] array = new string[6];
				array[0] = "Exception while recalculating ";
				array[1] = ((def != null) ? def.ToString() : null);
				array[2] = " thought state for pawn ";
				int num = 3;
				Pawn pawn = this.pawn;
				array[num] = ((pawn != null) ? pawn.ToString() : null);
				array[4] = ": ";
				int num2 = 5;
				Exception ex2 = ex;
				array[num2] = ((ex2 != null) ? ex2.ToString() : null);
				Log.Error(string.Concat(array));
			}
			return thought_Situational;
		}

		// Token: 0x0600D049 RID: 53321 RVA: 0x003D4570 File Offset: 0x003D2770
		private Thought_SituationalSocial TryCreateSocialThought(ThoughtDef def, Pawn otherPawn)
		{
			Thought_SituationalSocial thought_SituationalSocial = null;
			try
			{
				if (!ThoughtUtility.CanGetThought(this.pawn, def, false))
				{
					return null;
				}
				if (!def.Worker.CurrentSocialState(this.pawn, otherPawn).ActiveFor(def))
				{
					return null;
				}
				if (!def.socialTargetDevelopmentalStageFilter.HasAny(otherPawn.DevelopmentalStage))
				{
					return null;
				}
				if (def.ignoreSubhumans && otherPawn.IsSubhuman)
				{
					return null;
				}
				thought_SituationalSocial = (Thought_SituationalSocial)ThoughtMaker.MakeThought(def);
				thought_SituationalSocial.pawn = this.pawn;
				thought_SituationalSocial.otherPawn = otherPawn;
				if (def.Worker is ThoughtWorker_Precept_Social)
				{
					thought_SituationalSocial.sourcePrecept = this.pawn.Ideo.GetFirstPreceptAllowingSituationalThought(def);
				}
				thought_SituationalSocial.RecalculateState();
			}
			catch (Exception ex)
			{
				string[] array = new string[6];
				array[0] = "Exception while recalculating ";
				array[1] = ((def != null) ? def.ToString() : null);
				array[2] = " thought state for pawn ";
				int num = 3;
				Pawn pawn = this.pawn;
				array[num] = ((pawn != null) ? pawn.ToString() : null);
				array[4] = ": ";
				int num2 = 5;
				Exception ex2 = ex;
				array[num2] = ((ex2 != null) ? ex2.ToString() : null);
				Log.Error(string.Concat(array));
			}
			return thought_SituationalSocial;
		}

		// Token: 0x0600D04A RID: 53322 RVA: 0x003D46A8 File Offset: 0x003D28A8
		public void Notify_SituationalThoughtsDirty()
		{
			this.cachedThoughts.Clear();
			this.cachedSocialThoughts.Clear();
			this.thoughtsDirty = true;
		}

		// Token: 0x0600D04B RID: 53323 RVA: 0x003D46C7 File Offset: 0x003D28C7
		private void RemoveExpiredThoughtsFromCache()
		{
			this.cachedSocialThoughts.RemoveAll((KeyValuePair<Pawn, SituationalThoughtHandler.CachedSocialThoughts> x) => x.Value.Expired || x.Key.Discarded);
		}

		// Token: 0x04008DB3 RID: 36275
		public Pawn pawn;

		// Token: 0x04008DB4 RID: 36276
		private readonly List<Thought_Situational> cachedThoughts = new List<Thought_Situational>();

		// Token: 0x04008DB5 RID: 36277
		private readonly Dictionary<Pawn, SituationalThoughtHandler.CachedSocialThoughts> cachedSocialThoughts = new Dictionary<Pawn, SituationalThoughtHandler.CachedSocialThoughts>();

		// Token: 0x04008DB6 RID: 36278
		private bool thoughtsDirty = true;

		// Token: 0x04008DB7 RID: 36279
		private const int RecalculateStateEveryTicks = 100;

		// Token: 0x04008DB8 RID: 36280
		private readonly HashSet<ThoughtDef> tmpCachedSocialThoughts = new HashSet<ThoughtDef>();

		// Token: 0x02002532 RID: 9522
		private class CachedSocialThoughts
		{
			// Token: 0x17001FF5 RID: 8181
			// (get) Token: 0x0600D04C RID: 53324 RVA: 0x003D46F4 File Offset: 0x003D28F4
			public bool Expired
			{
				get
				{
					return Find.TickManager.TicksGame - this.lastQueryTick >= 300;
				}
			}

			// Token: 0x17001FF6 RID: 8182
			// (get) Token: 0x0600D04D RID: 53325 RVA: 0x003D4711 File Offset: 0x003D2911
			public bool ShouldRecalculateState
			{
				get
				{
					return Find.TickManager.TicksGame - this.lastRecalculationTick >= 100;
				}
			}

			// Token: 0x04008DB9 RID: 36281
			public readonly List<Thought_SituationalSocial> thoughts = new List<Thought_SituationalSocial>();

			// Token: 0x04008DBA RID: 36282
			public readonly List<Thought_SituationalSocial> activeThoughts = new List<Thought_SituationalSocial>();

			// Token: 0x04008DBB RID: 36283
			public int lastRecalculationTick = -99999;

			// Token: 0x04008DBC RID: 36284
			public int lastQueryTick = -99999;

			// Token: 0x04008DBD RID: 36285
			private const int ExpireAfterTicks = 300;
		}
	}
}
