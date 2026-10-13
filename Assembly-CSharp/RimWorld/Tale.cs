using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200292A RID: 10538
	public class Tale : IExposable, ILoadReferenceable
	{
		// Token: 0x170022FF RID: 8959
		// (get) Token: 0x0600E68B RID: 59019 RVA: 0x00433DEB File Offset: 0x00431FEB
		public int AgeTicks
		{
			get
			{
				return Find.TickManager.TicksAbs - this.date;
			}
		}

		// Token: 0x17002300 RID: 8960
		// (get) Token: 0x0600E68C RID: 59020 RVA: 0x00433DFE File Offset: 0x00431FFE
		public int Uses
		{
			get
			{
				return this.uses;
			}
		}

		// Token: 0x17002301 RID: 8961
		// (get) Token: 0x0600E68D RID: 59021 RVA: 0x00433E06 File Offset: 0x00432006
		public bool Unused
		{
			get
			{
				return this.uses == 0;
			}
		}

		// Token: 0x17002302 RID: 8962
		// (get) Token: 0x0600E68E RID: 59022 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual Pawn DominantPawn
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17002303 RID: 8963
		// (get) Token: 0x0600E68F RID: 59023 RVA: 0x00433E14 File Offset: 0x00432014
		public float InterestLevel
		{
			get
			{
				float num = this.def.baseInterest;
				num /= (float)(1 + this.uses * 3);
				float a = 0f;
				switch (this.def.type)
				{
				case TaleType.Volatile:
					a = 50f;
					break;
				case TaleType.Expirable:
					a = this.def.expireDays;
					break;
				case TaleType.PermanentHistorical:
					a = 50f;
					break;
				}
				float value = (float)(this.AgeTicks / 60000);
				num *= Mathf.InverseLerp(a, 0f, value);
				if (num < 0.01f)
				{
					num = 0.01f;
				}
				return num;
			}
		}

		// Token: 0x17002304 RID: 8964
		// (get) Token: 0x0600E690 RID: 59024 RVA: 0x00433EA9 File Offset: 0x004320A9
		public bool Expired
		{
			get
			{
				return this.Unused && this.def.type == TaleType.Expirable && (float)this.AgeTicks > this.def.expireDays * 60000f;
			}
		}

		// Token: 0x17002305 RID: 8965
		// (get) Token: 0x0600E691 RID: 59025 RVA: 0x00433EDF File Offset: 0x004320DF
		public virtual string ShortSummary
		{
			get
			{
				if (!this.customLabel.NullOrEmpty())
				{
					return this.customLabel.CapitalizeFirst();
				}
				return this.def.LabelCap;
			}
		}

		// Token: 0x0600E693 RID: 59027 RVA: 0x00433F19 File Offset: 0x00432119
		public virtual void GenerateTestData()
		{
			if (Find.CurrentMap == null)
			{
				Log.Error("Can't generate test data because there is no map.");
			}
			this.date = Rand.Range(-108000000, -7200000);
			this.surroundings = TaleData_Surroundings.GenerateRandom(Find.CurrentMap);
		}

		// Token: 0x0600E694 RID: 59028 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool Concerns(Thing th)
		{
			return false;
		}

		// Token: 0x0600E695 RID: 59029 RVA: 0x00433F54 File Offset: 0x00432154
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<TaleDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.id, "id", 0, false);
			Scribe_Values.Look<int>(ref this.uses, "uses", 0, false);
			Scribe_Values.Look<int>(ref this.date, "date", 0, false);
			Scribe_Deep.Look<TaleData_Surroundings>(ref this.surroundings, "surroundings", Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.customLabel, "customLabel", null, false);
			Scribe_Values.Look<bool>(ref this.hidden, "hidden", false, false);
		}

		// Token: 0x0600E696 RID: 59030 RVA: 0x00433FE0 File Offset: 0x004321E0
		public void Notify_NewlyUsed()
		{
			this.uses++;
		}

		// Token: 0x0600E697 RID: 59031 RVA: 0x00433FF0 File Offset: 0x004321F0
		public void Notify_ReferenceDestroyed()
		{
			if (this.uses == 0)
			{
				Log.Warning("Called reference destroyed method on tale " + ((this != null) ? this.ToString() : null) + " but uses count is 0.");
				return;
			}
			this.uses--;
		}

		// Token: 0x0600E698 RID: 59032 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_FactionRemoved(Faction faction)
		{
		}

		// Token: 0x0600E699 RID: 59033 RVA: 0x0043402A File Offset: 0x0043222A
		public IEnumerable<RulePack> GetTextGenerationIncludes()
		{
			if (this.def.rulePack != null)
			{
				yield return this.def.rulePack;
			}
			yield break;
		}

		// Token: 0x0600E69A RID: 59034 RVA: 0x0043403A File Offset: 0x0043223A
		public IEnumerable<Rule> GetTextGenerationRules(Dictionary<string, string> outConstants = null)
		{
			Vector2 location = Vector2.zero;
			if (this.surroundings != null && this.surroundings.tile.Valid)
			{
				location = Find.WorldGrid.LongLatOf(this.surroundings.tile);
			}
			yield return new Rule_String("DATE", GenDate.DateFullStringAt((long)this.date, location));
			IEnumerator<Rule> enumerator;
			if (this.surroundings != null)
			{
				foreach (Rule rule in this.surroundings.GetRules(null))
				{
					yield return rule;
				}
				enumerator = null;
			}
			foreach (Rule rule2 in this.SpecialTextGenerationRules(outConstants))
			{
				yield return rule2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600E69B RID: 59035 RVA: 0x00434051 File Offset: 0x00432251
		protected virtual IEnumerable<Rule> SpecialTextGenerationRules(Dictionary<string, string> outConstants)
		{
			return Enumerable.Empty<Rule>();
		}

		// Token: 0x0600E69C RID: 59036 RVA: 0x00434058 File Offset: 0x00432258
		public string GetUniqueLoadID()
		{
			return "Tale_" + this.id.ToString();
		}

		// Token: 0x0600E69D RID: 59037 RVA: 0x0043406F File Offset: 0x0043226F
		public override int GetHashCode()
		{
			return this.id;
		}

		// Token: 0x0600E69E RID: 59038 RVA: 0x00434078 File Offset: 0x00432278
		public override string ToString()
		{
			string str = string.Concat(new string[]
			{
				"(#",
				this.id.ToString(),
				": ",
				this.ShortSummary,
				"(age=",
				((float)this.AgeTicks / 60000f).ToString("F2"),
				" interest=",
				this.InterestLevel.ToString()
			});
			if (this.Unused && this.def.type == TaleType.Expirable)
			{
				str = str + ", expireDays=" + this.def.expireDays.ToString("F2");
			}
			return str + ")";
		}

		// Token: 0x04009A2E RID: 39470
		public TaleDef def;

		// Token: 0x04009A2F RID: 39471
		public int id;

		// Token: 0x04009A30 RID: 39472
		private int uses;

		// Token: 0x04009A31 RID: 39473
		public int date = -1;

		// Token: 0x04009A32 RID: 39474
		public TaleData_Surroundings surroundings;

		// Token: 0x04009A33 RID: 39475
		public string customLabel;

		// Token: 0x04009A34 RID: 39476
		public bool hidden;
	}
}
