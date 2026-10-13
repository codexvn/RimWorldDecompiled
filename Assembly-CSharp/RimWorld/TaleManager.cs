using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02002921 RID: 10529
	public sealed class TaleManager : IExposable
	{
		// Token: 0x170022FC RID: 8956
		// (get) Token: 0x0600E652 RID: 58962 RVA: 0x00432D95 File Offset: 0x00430F95
		public List<Tale> AllTalesListForReading
		{
			get
			{
				return this.tales;
			}
		}

		// Token: 0x0600E653 RID: 58963 RVA: 0x00432DA0 File Offset: 0x00430FA0
		public void ExposeData()
		{
			Scribe_Collections.Look<Tale>(ref this.tales, "tales", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.tales.RemoveAll((Tale x) => x == null) != 0)
				{
					Log.Error("Some tales were null after loading.");
				}
				if (this.tales.RemoveAll((Tale x) => x.def == null) != 0)
				{
					Log.Error("Some tales had null def after loading.");
				}
			}
		}

		// Token: 0x0600E654 RID: 58964 RVA: 0x00432E37 File Offset: 0x00431037
		public void TaleManagerTick()
		{
			this.RemoveExpiredTales();
		}

		// Token: 0x0600E655 RID: 58965 RVA: 0x00432E3F File Offset: 0x0043103F
		public void Add(Tale tale)
		{
			this.tales.Add(tale);
			this.CheckCullTales(tale);
		}

		// Token: 0x0600E656 RID: 58966 RVA: 0x00432E54 File Offset: 0x00431054
		private void RemoveTale(Tale tale)
		{
			if (!tale.Unused)
			{
				Log.Warning("Tried to remove used tale " + ((tale != null) ? tale.ToString() : null));
				return;
			}
			this.tales.Remove(tale);
		}

		// Token: 0x0600E657 RID: 58967 RVA: 0x00432E88 File Offset: 0x00431088
		private void CheckCullTales(Tale addedTale)
		{
			this.CheckCullUnusedVolatileTales();
			this.CheckCullUnusedTalesWithMaxPerPawnLimit(addedTale);
		}

		// Token: 0x0600E658 RID: 58968 RVA: 0x00432E98 File Offset: 0x00431098
		private void CheckCullUnusedVolatileTales()
		{
			int i = 0;
			for (int j = 0; j < this.tales.Count; j++)
			{
				if (this.tales[j].def.type == TaleType.Volatile && this.tales[j].Unused)
				{
					i++;
				}
			}
			while (i > 350)
			{
				Tale tale = null;
				float num = float.MaxValue;
				for (int k = 0; k < this.tales.Count; k++)
				{
					if (this.tales[k].def.type == TaleType.Volatile && this.tales[k].Unused && this.tales[k].InterestLevel < num)
					{
						tale = this.tales[k];
						num = this.tales[k].InterestLevel;
					}
				}
				this.RemoveTale(tale);
				i--;
			}
		}

		// Token: 0x0600E659 RID: 58969 RVA: 0x00432F8C File Offset: 0x0043118C
		private void CheckCullUnusedTalesWithMaxPerPawnLimit(Tale addedTale)
		{
			if (addedTale.def.maxPerPawn < 0)
			{
				return;
			}
			if (addedTale.DominantPawn == null)
			{
				return;
			}
			int i = 0;
			for (int j = 0; j < this.tales.Count; j++)
			{
				if (this.tales[j].Unused && this.tales[j].def == addedTale.def && this.tales[j].DominantPawn == addedTale.DominantPawn)
				{
					i++;
				}
			}
			while (i > addedTale.def.maxPerPawn)
			{
				Tale tale = null;
				int num = -1;
				for (int k = 0; k < this.tales.Count; k++)
				{
					if (this.tales[k].Unused && this.tales[k].def == addedTale.def && this.tales[k].DominantPawn == addedTale.DominantPawn && this.tales[k].AgeTicks > num)
					{
						tale = this.tales[k];
						num = this.tales[k].AgeTicks;
					}
				}
				this.RemoveTale(tale);
				i--;
			}
		}

		// Token: 0x0600E65A RID: 58970 RVA: 0x004330D8 File Offset: 0x004312D8
		private void RemoveExpiredTales()
		{
			for (int i = this.tales.Count - 1; i >= 0; i--)
			{
				if (this.tales[i].Expired)
				{
					this.RemoveTale(this.tales[i]);
				}
			}
		}

		// Token: 0x0600E65B RID: 58971 RVA: 0x00433124 File Offset: 0x00431324
		public TaleReference GetRandomTaleReferenceForArt(ArtGenerationContext source)
		{
			if (source == ArtGenerationContext.Outsider)
			{
				return TaleReference.Taleless;
			}
			if (this.tales.Count == 0)
			{
				return TaleReference.Taleless;
			}
			if (Rand.Value < 0.25f)
			{
				return TaleReference.Taleless;
			}
			Tale tale;
			if (!(from x in this.tales
			where x.def.usableForArt
			select x).TryRandomElementByWeight((Tale ta) => ta.InterestLevel, out tale))
			{
				return TaleReference.Taleless;
			}
			tale.Notify_NewlyUsed();
			return new TaleReference(tale);
		}

		// Token: 0x0600E65C RID: 58972 RVA: 0x004331C4 File Offset: 0x004313C4
		public TaleReference GetRandomTaleReferenceForArtConcerning(Thing th)
		{
			if (this.tales.Count == 0)
			{
				return TaleReference.Taleless;
			}
			Tale tale;
			if (!(from x in this.tales
			where x.def.usableForArt && x.Concerns(th)
			select x).TryRandomElementByWeight((Tale x) => x.InterestLevel, out tale))
			{
				return TaleReference.Taleless;
			}
			tale.Notify_NewlyUsed();
			return new TaleReference(tale);
		}

		// Token: 0x0600E65D RID: 58973 RVA: 0x00433244 File Offset: 0x00431444
		public Tale GetLatestTale(TaleDef def, Pawn pawn)
		{
			Tale tale = null;
			int num = 0;
			for (int i = 0; i < this.tales.Count; i++)
			{
				if (this.tales[i].def == def && this.tales[i].DominantPawn == pawn && (tale == null || this.tales[i].AgeTicks < num))
				{
					tale = this.tales[i];
					num = this.tales[i].AgeTicks;
				}
			}
			return tale;
		}

		// Token: 0x0600E65E RID: 58974 RVA: 0x004332CC File Offset: 0x004314CC
		public void Notify_PawnDestroyed(Pawn pawn)
		{
			for (int i = this.tales.Count - 1; i >= 0; i--)
			{
				if (this.tales[i].Unused && !this.tales[i].def.usableForArt && this.tales[i].def.type != TaleType.PermanentHistorical && this.tales[i].DominantPawn == pawn)
				{
					this.RemoveTale(this.tales[i]);
				}
			}
		}

		// Token: 0x0600E65F RID: 58975 RVA: 0x0043335C File Offset: 0x0043155C
		public void Notify_PawnDiscarded(Pawn p, bool silentlyRemoveReferences)
		{
			for (int i = this.tales.Count - 1; i >= 0; i--)
			{
				if (this.tales[i].Concerns(p))
				{
					if (!silentlyRemoveReferences)
					{
						string[] array = new string[5];
						array[0] = "Discarding pawn ";
						array[1] = ((p != null) ? p.ToString() : null);
						array[2] = ", but he is referenced by a tale ";
						int num = 3;
						Tale tale = this.tales[i];
						array[num] = ((tale != null) ? tale.ToString() : null);
						array[4] = ".";
						Log.Warning(string.Concat(array));
					}
					else if (!this.tales[i].Unused)
					{
						string[] array2 = new string[5];
						array2[0] = "Discarding pawn ";
						array2[1] = ((p != null) ? p.ToString() : null);
						array2[2] = ", but he is referenced by an active tale ";
						int num2 = 3;
						Tale tale2 = this.tales[i];
						array2[num2] = ((tale2 != null) ? tale2.ToString() : null);
						array2[4] = ".";
						Log.Warning(string.Concat(array2));
					}
					this.RemoveTale(this.tales[i]);
				}
			}
		}

		// Token: 0x0600E660 RID: 58976 RVA: 0x00433470 File Offset: 0x00431670
		public void Notify_FactionRemoved(Faction faction)
		{
			for (int i = 0; i < this.tales.Count; i++)
			{
				this.tales[i].Notify_FactionRemoved(faction);
			}
		}

		// Token: 0x0600E661 RID: 58977 RVA: 0x004334A8 File Offset: 0x004316A8
		public bool AnyActiveTaleConcerns(Pawn p)
		{
			for (int i = 0; i < this.tales.Count; i++)
			{
				if (!this.tales[i].Unused && this.tales[i].Concerns(p))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600E662 RID: 58978 RVA: 0x004334F8 File Offset: 0x004316F8
		public bool AnyTaleConcerns(Pawn p)
		{
			for (int i = 0; i < this.tales.Count; i++)
			{
				if (this.tales[i].Concerns(p))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600E663 RID: 58979 RVA: 0x00433534 File Offset: 0x00431734
		public float GetMaxHistoricalTaleDay()
		{
			float num = 0f;
			for (int i = 0; i < this.tales.Count; i++)
			{
				Tale tale = this.tales[i];
				if (tale.def.type == TaleType.PermanentHistorical)
				{
					float num2 = (float)GenDate.TickAbsToGame(tale.date) / 60000f;
					if (num2 > num)
					{
						num = num2;
					}
				}
			}
			return num;
		}

		// Token: 0x0600E664 RID: 58980 RVA: 0x00433594 File Offset: 0x00431794
		public void LogTales()
		{
			StringBuilder stringBuilder = new StringBuilder();
			IEnumerable<Tale> enumerable = from x in this.tales
			where !x.Unused
			select x;
			IEnumerable<Tale> enumerable2 = from x in this.tales
			where x.def.type == TaleType.Volatile && x.Unused
			select x;
			IEnumerable<Tale> enumerable3 = from x in this.tales
			where x.def.type == TaleType.PermanentHistorical && x.Unused
			select x;
			IEnumerable<Tale> enumerable4 = from x in this.tales
			where x.def.type == TaleType.Expirable && x.Unused
			select x;
			stringBuilder.AppendLine("All tales count: " + this.tales.Count.ToString());
			stringBuilder.AppendLine("Used count: " + enumerable.Count<Tale>().ToString());
			stringBuilder.AppendLine(string.Concat(new string[]
			{
				"Unused volatile count: ",
				enumerable2.Count<Tale>().ToString(),
				" (max: ",
				350.ToString(),
				")"
			}));
			stringBuilder.AppendLine("Unused permanent count: " + enumerable3.Count<Tale>().ToString());
			stringBuilder.AppendLine("Unused expirable count: " + enumerable4.Count<Tale>().ToString());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("-------Used-------");
			foreach (Tale tale in enumerable)
			{
				stringBuilder.AppendLine(tale.ToString());
			}
			stringBuilder.AppendLine("-------Unused volatile-------");
			foreach (Tale tale2 in enumerable2)
			{
				stringBuilder.AppendLine(tale2.ToString());
			}
			stringBuilder.AppendLine("-------Unused permanent-------");
			foreach (Tale tale3 in enumerable3)
			{
				stringBuilder.AppendLine(tale3.ToString());
			}
			stringBuilder.AppendLine("-------Unused expirable-------");
			foreach (Tale tale4 in enumerable4)
			{
				stringBuilder.AppendLine(tale4.ToString());
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x0600E665 RID: 58981 RVA: 0x00433880 File Offset: 0x00431A80
		public void LogTaleInterestSummary()
		{
			StringBuilder stringBuilder = new StringBuilder();
			float num = (from t in this.tales
			where t.def.usableForArt
			select t).Sum((Tale t) => t.InterestLevel);
			Func<TaleDef, float> defInterest = (TaleDef def) => (from t in this.tales
			where t.def == def
			select t).Sum((Tale t) => t.InterestLevel);
			IEnumerable<TaleDef> source = from def in DefDatabase<TaleDef>.AllDefs
			where def.usableForArt
			select def;
			Func<TaleDef, float> <>9__6;
			Func<TaleDef, float> keySelector;
			if ((keySelector = <>9__6) == null)
			{
				keySelector = (<>9__6 = ((TaleDef def) => defInterest(def)));
			}
			using (IEnumerator<TaleDef> enumerator = source.OrderByDescending(keySelector).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TaleDef def = enumerator.Current;
					stringBuilder.AppendLine(string.Concat(new string[]
					{
						def.defName,
						":   [",
						(from t in this.tales
						where t.def == def
						select t).Count<Tale>().ToString(),
						"]   ",
						(defInterest(def) / num).ToStringPercent("F2")
					}));
				}
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x04009A14 RID: 39444
		private List<Tale> tales = new List<Tale>();

		// Token: 0x04009A15 RID: 39445
		private const int MaxUnusedVolatileTales = 350;
	}
}
