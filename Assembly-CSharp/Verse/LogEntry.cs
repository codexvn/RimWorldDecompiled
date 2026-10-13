using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x0200033F RID: 831
	[StaticConstructorOnStartup]
	public abstract class LogEntry : IExposable, ILoadReferenceable
	{
		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06001515 RID: 5397 RVA: 0x00063E7A File Offset: 0x0006207A
		public int Age
		{
			get
			{
				return Find.TickManager.TicksAbs - this.ticksAbs;
			}
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06001516 RID: 5398 RVA: 0x00063E8D File Offset: 0x0006208D
		public int Tick
		{
			get
			{
				return this.ticksAbs;
			}
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06001517 RID: 5399 RVA: 0x00063E95 File Offset: 0x00062095
		public int LogID
		{
			get
			{
				return this.logID;
			}
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06001518 RID: 5400 RVA: 0x00063E8D File Offset: 0x0006208D
		public int Timestamp
		{
			get
			{
				return this.ticksAbs;
			}
		}

		// Token: 0x06001519 RID: 5401 RVA: 0x00063E9D File Offset: 0x0006209D
		public LogEntry(LogEntryDef def = null)
		{
			this.ticksAbs = Find.TickManager.TicksAbs;
			this.def = def;
			if (Scribe.mode == LoadSaveMode.Inactive)
			{
				this.logID = Find.UniqueIDsManager.GetNextLogID();
			}
		}

		// Token: 0x0600151A RID: 5402 RVA: 0x00063EDA File Offset: 0x000620DA
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksAbs, "ticksAbs", 0, false);
			Scribe_Values.Look<int>(ref this.logID, "logID", 0, false);
			Scribe_Defs.Look<LogEntryDef>(ref this.def, "def");
		}

		// Token: 0x0600151B RID: 5403 RVA: 0x00063F10 File Offset: 0x00062110
		public string ToGameStringFromPOV(Thing pov, bool forceLog = false)
		{
			if (this.cachedString == null || pov == null != (this.cachedStringPov == null) || (this.cachedStringPov != null && pov != this.cachedStringPov.Target) || DebugViewSettings.logGrammarResolution || forceLog)
			{
				Rand.PushState();
				try
				{
					Rand.Seed = this.logID;
					this.cachedStringPov = ((pov != null) ? new WeakReference<Thing>(pov) : null);
					this.cachedString = this.ToGameStringFromPOV_Worker(pov, forceLog);
					this.cachedHeightWidth = 0f;
					this.cachedHeight = 0f;
				}
				finally
				{
					Rand.PopState();
				}
			}
			return this.cachedString;
		}

		// Token: 0x0600151C RID: 5404 RVA: 0x00063FBC File Offset: 0x000621BC
		protected virtual string ToGameStringFromPOV_Worker(Thing pov, bool forceLog)
		{
			return GrammarResolver.Resolve("r_logentry", this.GenerateGrammarRequest(), null, forceLog, null, null, null, true);
		}

		// Token: 0x0600151D RID: 5405 RVA: 0x00063FD4 File Offset: 0x000621D4
		protected virtual GrammarRequest GenerateGrammarRequest()
		{
			return default(GrammarRequest);
		}

		// Token: 0x0600151E RID: 5406 RVA: 0x00063FEC File Offset: 0x000621EC
		public float GetTextHeight(Thing pov, float width)
		{
			string text = this.ToGameStringFromPOV(pov, false);
			if (this.cachedHeightWidth != width)
			{
				this.cachedHeightWidth = width;
				this.cachedHeight = Text.CalcHeight(text, width);
			}
			return this.cachedHeight;
		}

		// Token: 0x0600151F RID: 5407 RVA: 0x00064025 File Offset: 0x00062225
		protected void ResetCache()
		{
			this.cachedStringPov = null;
			this.cachedString = null;
			this.cachedHeightWidth = 0f;
			this.cachedHeight = 0f;
		}

		// Token: 0x06001520 RID: 5408
		public abstract bool Concerns(Thing t);

		// Token: 0x06001521 RID: 5409
		public abstract IEnumerable<Thing> GetConcerns();

		// Token: 0x06001522 RID: 5410 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool CanBeClickedFromPOV(Thing pov)
		{
			return false;
		}

		// Token: 0x06001523 RID: 5411 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void ClickedFromPOV(Thing pov)
		{
		}

		// Token: 0x06001524 RID: 5412 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual Texture2D IconFromPOV(Thing pov)
		{
			return null;
		}

		// Token: 0x06001525 RID: 5413 RVA: 0x0006404C File Offset: 0x0006224C
		public virtual Color? IconColorFromPOV(Thing pov)
		{
			return null;
		}

		// Token: 0x06001526 RID: 5414 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_FactionRemoved(Faction faction)
		{
		}

		// Token: 0x06001527 RID: 5415 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_IdeoRemoved(Ideo ideo)
		{
		}

		// Token: 0x06001528 RID: 5416 RVA: 0x00064064 File Offset: 0x00062264
		public virtual string GetTipString()
		{
			return "OccurredTimeAgo".Translate(this.Age.ToStringTicksToPeriod(true, false, true, true, false)).CapitalizeFirst() + ".";
		}

		// Token: 0x06001529 RID: 5417 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool ShowInCompactView()
		{
			return true;
		}

		// Token: 0x0600152A RID: 5418 RVA: 0x000640A7 File Offset: 0x000622A7
		public void Debug_OverrideTicks(int newTicks)
		{
			this.ticksAbs = newTicks;
		}

		// Token: 0x0600152B RID: 5419 RVA: 0x000640B0 File Offset: 0x000622B0
		public string GetUniqueLoadID()
		{
			return string.Format("LogEntry_{0}_{1}", this.ticksAbs, this.logID);
		}

		// Token: 0x040013BD RID: 5053
		protected int logID;

		// Token: 0x040013BE RID: 5054
		protected int ticksAbs = -1;

		// Token: 0x040013BF RID: 5055
		public LogEntryDef def;

		// Token: 0x040013C0 RID: 5056
		private WeakReference<Thing> cachedStringPov;

		// Token: 0x040013C1 RID: 5057
		private string cachedString;

		// Token: 0x040013C2 RID: 5058
		private float cachedHeightWidth;

		// Token: 0x040013C3 RID: 5059
		private float cachedHeight;

		// Token: 0x040013C4 RID: 5060
		public static readonly Texture2D Blood = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/Blood", true);

		// Token: 0x040013C5 RID: 5061
		public static readonly Texture2D BloodTarget = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/BloodTarget", true);

		// Token: 0x040013C6 RID: 5062
		public static readonly Texture2D Downed = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/Downed", true);

		// Token: 0x040013C7 RID: 5063
		public static readonly Texture2D DownedTarget = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/DownedTarget", true);

		// Token: 0x040013C8 RID: 5064
		public static readonly Texture2D Skull = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/Skull", true);

		// Token: 0x040013C9 RID: 5065
		public static readonly Texture2D SkullTarget = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/SkullTarget", true);
	}
}
