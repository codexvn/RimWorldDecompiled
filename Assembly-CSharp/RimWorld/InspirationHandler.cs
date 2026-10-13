using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001110 RID: 4368
	public class InspirationHandler : IExposable
	{
		// Token: 0x1700106C RID: 4204
		// (get) Token: 0x06006A7E RID: 27262 RVA: 0x0021A091 File Offset: 0x00218291
		public bool Inspired
		{
			get
			{
				return this.curState != null;
			}
		}

		// Token: 0x1700106D RID: 4205
		// (get) Token: 0x06006A7F RID: 27263 RVA: 0x0021A09C File Offset: 0x0021829C
		public Inspiration CurState
		{
			get
			{
				return this.curState;
			}
		}

		// Token: 0x1700106E RID: 4206
		// (get) Token: 0x06006A80 RID: 27264 RVA: 0x0021A0A4 File Offset: 0x002182A4
		public InspirationDef CurStateDef
		{
			get
			{
				Inspiration inspiration = this.curState;
				if (inspiration == null)
				{
					return null;
				}
				return inspiration.def;
			}
		}

		// Token: 0x1700106F RID: 4207
		// (get) Token: 0x06006A81 RID: 27265 RVA: 0x0021A0B8 File Offset: 0x002182B8
		private float StartInspirationMTBDays
		{
			get
			{
				if (this.pawn.needs.mood == null)
				{
					return -1f;
				}
				float curLevel = this.pawn.needs.mood.CurLevel;
				if (curLevel < 0.5f)
				{
					return -1f;
				}
				return GenMath.LerpDouble(0.5f, 1f, 210f, 10f, curLevel);
			}
		}

		// Token: 0x06006A82 RID: 27266 RVA: 0x0021A11B File Offset: 0x0021831B
		public InspirationHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06006A83 RID: 27267 RVA: 0x0021A12A File Offset: 0x0021832A
		public void ExposeData()
		{
			Scribe_Deep.Look<Inspiration>(ref this.curState, "curState", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.curState != null)
			{
				this.curState.pawn = this.pawn;
			}
		}

		// Token: 0x06006A84 RID: 27268 RVA: 0x0021A162 File Offset: 0x00218362
		public void InspirationHandlerTickInterval(int delta)
		{
			if (this.curState != null)
			{
				this.curState.InspirationTick(delta);
			}
			if (this.pawn.IsHashIntervalTick(100, delta))
			{
				this.CheckStartRandomInspiration();
			}
		}

		// Token: 0x06006A85 RID: 27269 RVA: 0x0021A190 File Offset: 0x00218390
		public bool TryStartInspiration(InspirationDef def, string reason = null, bool sendLetter = true)
		{
			if (this.Inspired)
			{
				return false;
			}
			if (this.BlockedByHediff())
			{
				return false;
			}
			if (!def.Worker.InspirationCanOccur(this.pawn))
			{
				return false;
			}
			this.curState = (Inspiration)Activator.CreateInstance(def.inspirationClass);
			this.curState.def = def;
			this.curState.pawn = this.pawn;
			this.curState.reason = reason;
			this.curState.PostStart(sendLetter);
			return true;
		}

		// Token: 0x06006A86 RID: 27270 RVA: 0x0021A214 File Offset: 0x00218414
		public void EndInspiration(Inspiration inspiration)
		{
			if (inspiration == null)
			{
				return;
			}
			if (this.curState != inspiration)
			{
				Log.Error("Tried to end inspiration " + inspiration.ToStringSafe<Inspiration>() + " but current inspiration is " + this.curState.ToStringSafe<Inspiration>());
				return;
			}
			this.curState = null;
			inspiration.PostEnd();
		}

		// Token: 0x06006A87 RID: 27271 RVA: 0x0021A261 File Offset: 0x00218461
		public void EndInspiration(InspirationDef inspirationDef)
		{
			if (this.curState != null && this.curState.def == inspirationDef)
			{
				this.EndInspiration(this.curState);
			}
		}

		// Token: 0x06006A88 RID: 27272 RVA: 0x0021A285 File Offset: 0x00218485
		public void Reset()
		{
			this.curState = null;
		}

		// Token: 0x06006A89 RID: 27273 RVA: 0x0021A290 File Offset: 0x00218490
		private void CheckStartRandomInspiration()
		{
			if (this.Inspired)
			{
				return;
			}
			if (!this.pawn.health.capacities.CanBeAwake)
			{
				return;
			}
			float startInspirationMTBDays = this.StartInspirationMTBDays;
			if (startInspirationMTBDays < 0f)
			{
				return;
			}
			if (Rand.MTBEventOccurs(startInspirationMTBDays, 60000f, 100f))
			{
				InspirationDef randomAvailableInspirationDef = this.GetRandomAvailableInspirationDef();
				if (randomAvailableInspirationDef != null)
				{
					this.TryStartInspiration(randomAvailableInspirationDef, "LetterInspirationBeginThanksToHighMoodPart".Translate(), true);
				}
			}
		}

		// Token: 0x06006A8A RID: 27274 RVA: 0x0021A302 File Offset: 0x00218502
		public InspirationDef GetRandomAvailableInspirationDef()
		{
			return (from x in DefDatabase<InspirationDef>.AllDefsListForReading
			where x.Worker.InspirationCanOccur(this.pawn)
			select x).RandomElementByWeightWithFallback((InspirationDef x) => x.Worker.CommonalityFor(this.pawn), null);
		}

		// Token: 0x06006A8B RID: 27275 RVA: 0x0021A32C File Offset: 0x0021852C
		private bool BlockedByHediff()
		{
			foreach (Hediff hediff in this.pawn.health.hediffSet.hediffs)
			{
				if (hediff.CurStage != null && hediff.CurStage.blocksInspirations)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04004B33 RID: 19251
		public Pawn pawn;

		// Token: 0x04004B34 RID: 19252
		private Inspiration curState;

		// Token: 0x04004B35 RID: 19253
		private const int CheckStartInspirationIntervalTicks = 100;

		// Token: 0x04004B36 RID: 19254
		private const float StartInspirationMTBDaysAtMaxMood = 10f;

		// Token: 0x04004B37 RID: 19255
		private const float MinMood = 0.5f;
	}
}
