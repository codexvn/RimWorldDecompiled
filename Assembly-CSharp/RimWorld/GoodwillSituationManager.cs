using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002598 RID: 9624
	public class GoodwillSituationManager
	{
		// Token: 0x0600D2F7 RID: 54007 RVA: 0x003E25D8 File Offset: 0x003E07D8
		public List<GoodwillSituationManager.CachedSituation> GetSituations(Faction other)
		{
			if (other == null || other.IsPlayer)
			{
				Log.Error("Called GetSituations() for faction " + ((other != null) ? other.ToString() : null));
				return null;
			}
			List<GoodwillSituationManager.CachedSituation> result;
			if (this.cachedData.TryGetValue(other, out result))
			{
				return result;
			}
			this.Recalculate(other, true);
			return this.cachedData[other];
		}

		// Token: 0x0600D2F8 RID: 54008 RVA: 0x003E2634 File Offset: 0x003E0834
		public int GetMaxGoodwill(Faction other)
		{
			List<GoodwillSituationManager.CachedSituation> situations = this.GetSituations(other);
			int num = 100;
			for (int i = 0; i < situations.Count; i++)
			{
				num = Mathf.Min(num, situations[i].maxGoodwill);
			}
			return num;
		}

		// Token: 0x0600D2F9 RID: 54009 RVA: 0x003E2674 File Offset: 0x003E0874
		public int GetNaturalGoodwill(Faction other)
		{
			List<GoodwillSituationManager.CachedSituation> situations = this.GetSituations(other);
			int num = 0;
			for (int i = 0; i < situations.Count; i++)
			{
				num += situations[i].naturalGoodwillOffset;
			}
			return num;
		}

		// Token: 0x0600D2FA RID: 54010 RVA: 0x003E26AC File Offset: 0x003E08AC
		public string GetExplanation(Faction other)
		{
			if (other == null || other == Faction.OfPlayer)
			{
				Log.Error("Tried to get CachedGoodwillData explanation for faction " + ((other != null) ? other.ToString() : null));
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			List<GoodwillSituationManager.CachedSituation> situations = this.GetSituations(other);
			for (int i = 0; i < situations.Count; i++)
			{
				stringBuilder.AppendInNewLine(situations[i].def.LabelCap);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600D2FB RID: 54011 RVA: 0x003E2724 File Offset: 0x003E0924
		public void GoodwillManagerTick()
		{
			if (Find.TickManager.TicksGame % 1000 == 0)
			{
				this.RecalculateAll(true);
			}
		}

		// Token: 0x0600D2FC RID: 54012 RVA: 0x003E2740 File Offset: 0x003E0940
		public void RecalculateAll(bool canSendHostilityChangedLetter)
		{
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				if (allFactionsListForReading[i] != Faction.OfPlayer && allFactionsListForReading[i].HasGoodwill)
				{
					this.Recalculate(allFactionsListForReading[i], canSendHostilityChangedLetter);
				}
			}
		}

		// Token: 0x0600D2FD RID: 54013 RVA: 0x003E2794 File Offset: 0x003E0994
		private void Recalculate(Faction other, bool canSendHostilityChangedLetter)
		{
			List<GoodwillSituationManager.CachedSituation> outSituations;
			if (this.cachedData.TryGetValue(other, out outSituations))
			{
				this.Recalculate(other, outSituations);
			}
			else
			{
				List<GoodwillSituationManager.CachedSituation> list = new List<GoodwillSituationManager.CachedSituation>();
				this.Recalculate(other, list);
				this.cachedData.Add(other, list);
			}
			this.CheckHostilityChanged(other, canSendHostilityChangedLetter);
		}

		// Token: 0x0600D2FE RID: 54014 RVA: 0x003E27E0 File Offset: 0x003E09E0
		private void Recalculate(Faction other, List<GoodwillSituationManager.CachedSituation> outSituations)
		{
			outSituations.Clear();
			if (!other.HasGoodwill)
			{
				return;
			}
			List<GoodwillSituationDef> allDefsListForReading = DefDatabase<GoodwillSituationDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				int maxGoodwill = allDefsListForReading[i].Worker.GetMaxGoodwill(other);
				int naturalGoodwillOffset = allDefsListForReading[i].Worker.GetNaturalGoodwillOffset(other);
				if (maxGoodwill < 100 || naturalGoodwillOffset != 0)
				{
					outSituations.Add(new GoodwillSituationManager.CachedSituation
					{
						def = allDefsListForReading[i],
						maxGoodwill = maxGoodwill,
						naturalGoodwillOffset = naturalGoodwillOffset
					});
				}
			}
		}

		// Token: 0x0600D2FF RID: 54015 RVA: 0x003E2870 File Offset: 0x003E0A70
		private void CheckHostilityChanged(Faction other, bool canSendHostilityChangedLetter)
		{
			if (Current.ProgramState == ProgramState.Entry)
			{
				return;
			}
			if (!other.HasGoodwill)
			{
				return;
			}
			Faction.OfPlayer.Notify_GoodwillSituationsChanged(other, canSendHostilityChangedLetter, null, null);
		}

		// Token: 0x04008F36 RID: 36662
		private Dictionary<Faction, List<GoodwillSituationManager.CachedSituation>> cachedData = new Dictionary<Faction, List<GoodwillSituationManager.CachedSituation>>();

		// Token: 0x04008F37 RID: 36663
		private const int RecacheEveryTicks = 1000;

		// Token: 0x02002599 RID: 9625
		public struct CachedSituation
		{
			// Token: 0x04008F38 RID: 36664
			public GoodwillSituationDef def;

			// Token: 0x04008F39 RID: 36665
			public int maxGoodwill;

			// Token: 0x04008F3A RID: 36666
			public int naturalGoodwillOffset;
		}
	}
}
