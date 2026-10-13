using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200272D RID: 10029
	public class RitualObligationTrigger_Date : RitualObligationTrigger
	{
		// Token: 0x170021B5 RID: 8629
		// (get) Token: 0x0600DC14 RID: 56340 RVA: 0x0040D5B5 File Offset: 0x0040B7B5
		public string DateString
		{
			get
			{
				return GenDate.QuadrumDateStringAt((long)(this.triggerDaysSinceStartOfYear * 60000), 0f);
			}
		}

		// Token: 0x0600DC15 RID: 56341 RVA: 0x0040D5CE File Offset: 0x0040B7CE
		public override void Init(RitualObligationTriggerProperties props)
		{
			base.Init(props);
			this.triggerDaysSinceStartOfYear = this.RandomDate();
		}

		// Token: 0x0600DC16 RID: 56342 RVA: 0x0040D5E4 File Offset: 0x0040B7E4
		public int RandomDate()
		{
			List<int> list = new List<int>();
			foreach (Precept precept in this.ritual.ideo.PreceptsListForReading)
			{
				if (precept != this.ritual)
				{
					Precept_Ritual precept_Ritual = precept as Precept_Ritual;
					if (precept_Ritual != null)
					{
						foreach (RitualObligationTrigger ritualObligationTrigger in precept_Ritual.obligationTriggers)
						{
							RitualObligationTrigger_Date ritualObligationTrigger_Date = ritualObligationTrigger as RitualObligationTrigger_Date;
							if (ritualObligationTrigger_Date != null)
							{
								list.Add(ritualObligationTrigger_Date.triggerDaysSinceStartOfYear);
							}
						}
					}
				}
			}
			List<int> source = Enumerable.Range(0, 60).Except(list).ToList<int>();
			int num = 20;
			bool flag = false;
			int num2 = 0;
			while (num >= 5 && !flag)
			{
				for (int i = 0; i < 10; i++)
				{
					num2 = source.RandomElement<int>();
					bool flag2 = false;
					for (int j = 0; j < list.Count; j++)
					{
						int num3 = list[j];
						int num4 = Mathf.Abs(num2 - num3);
						if (Mathf.Min(num4, 59 - num4) < num)
						{
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						flag = true;
						break;
					}
				}
				num -= 5;
			}
			return num2;
		}

		// Token: 0x0600DC17 RID: 56343 RVA: 0x0040D73C File Offset: 0x0040B93C
		public override void Tick()
		{
			if (this.ritual.isAnytime)
			{
				return;
			}
			int num = this.CurrentTickRelative();
			int num2 = this.OccursOnTick();
			if ((!this.mustBePlayerIdeo || Faction.OfPlayer.ideos.Has(this.ritual.ideo)) && num == num2)
			{
				this.ritual.AddObligation(new RitualObligation(this.ritual, true));
			}
		}

		// Token: 0x0600DC18 RID: 56344 RVA: 0x0040D7A4 File Offset: 0x0040B9A4
		public int CurrentTickRelative()
		{
			Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
			long num = GenDate.LocalTicksOffsetFromLongitude((anyPlayerHomeMap != null) ? Find.WorldGrid.LongLatOf(anyPlayerHomeMap.Tile).x : 0f);
			return (int)((long)(GenTicks.TicksAbs % 3600000) + num);
		}

		// Token: 0x0600DC19 RID: 56345 RVA: 0x0040D7EC File Offset: 0x0040B9EC
		public int OccursOnTick()
		{
			int num = this.triggerDaysSinceStartOfYear * 60000;
			Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
			float longitude = (anyPlayerHomeMap != null) ? Find.WorldGrid.LongLatOf(anyPlayerHomeMap.Tile).x : 0f;
			return num + (int)GenDate.LocalTicksOffsetFromLongitude(longitude);
		}

		// Token: 0x0600DC1A RID: 56346 RVA: 0x0040D833 File Offset: 0x0040BA33
		public override void CopyTo(RitualObligationTrigger other)
		{
			base.CopyTo(other);
			((RitualObligationTrigger_Date)other).triggerDaysSinceStartOfYear = this.triggerDaysSinceStartOfYear;
		}

		// Token: 0x0600DC1B RID: 56347 RVA: 0x0040D84D File Offset: 0x0040BA4D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.triggerDaysSinceStartOfYear, "triggerDaysSinceStartOfYear", 0, false);
		}

		// Token: 0x040094A5 RID: 38053
		public int triggerDaysSinceStartOfYear;
	}
}
