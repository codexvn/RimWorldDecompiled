using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002872 RID: 10354
	public class ScenPart_GameCondition : ScenPart
	{
		// Token: 0x17002288 RID: 8840
		// (get) Token: 0x0600E291 RID: 58001 RVA: 0x00425A53 File Offset: 0x00423C53
		public override string Label
		{
			get
			{
				return this.def.gameCondition.LabelCap;
			}
		}

		// Token: 0x0600E292 RID: 58002 RVA: 0x00425A6A File Offset: 0x00423C6A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.durationDays, "durationDayS", 0f, false);
		}

		// Token: 0x0600E293 RID: 58003 RVA: 0x00425A88 File Offset: 0x00423C88
		public override string Summary(Scenario scen)
		{
			return this.def.gameCondition.LabelCap + ": " + this.def.gameCondition.description + " (" + ((int)(this.durationDays * 60000f)).ToStringTicksToDays("F1") + ")";
		}

		// Token: 0x0600E294 RID: 58004 RVA: 0x00425AF9 File Offset: 0x00423CF9
		public override void Randomize()
		{
			this.durationDays = Mathf.Round(this.def.durationRandomRange.RandomInRange);
		}

		// Token: 0x0600E295 RID: 58005 RVA: 0x00425B16 File Offset: 0x00423D16
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Widgets.TextFieldNumericLabeled<float>(listing.GetScenPartRect(this, ScenPart.RowHeight), "durationDays".Translate(), ref this.durationDays, ref this.durationDaysBuf, 0f, 1E+09f);
		}

		// Token: 0x0600E296 RID: 58006 RVA: 0x00425B4E File Offset: 0x00423D4E
		public override void GenerateIntoMap(Map map)
		{
			if (!this.def.gameConditionTargetsWorld)
			{
				map.gameConditionManager.RegisterCondition(this.MakeCondition());
			}
		}

		// Token: 0x0600E297 RID: 58007 RVA: 0x00425B6E File Offset: 0x00423D6E
		public override void PostWorldGenerate()
		{
			if (this.def.gameConditionTargetsWorld)
			{
				Find.World.gameConditionManager.RegisterCondition(this.MakeCondition());
			}
		}

		// Token: 0x0600E298 RID: 58008 RVA: 0x00425B92 File Offset: 0x00423D92
		private GameCondition MakeCondition()
		{
			return GameConditionMaker.MakeCondition(this.def.gameCondition, (int)(this.durationDays * 60000f));
		}

		// Token: 0x0600E299 RID: 58009 RVA: 0x00425BB4 File Offset: 0x00423DB4
		public override bool CanCoexistWith(ScenPart other)
		{
			ScenPart_GameCondition scenPart_GameCondition = other as ScenPart_GameCondition;
			return scenPart_GameCondition == null || scenPart_GameCondition.def.gameCondition.CanCoexistWith(this.def.gameCondition);
		}

		// Token: 0x0600E29A RID: 58010 RVA: 0x00425BEB File Offset: 0x00423DEB
		public override bool HasNullDefs()
		{
			return base.HasNullDefs() || this.def.gameCondition == null;
		}

		// Token: 0x0600E29B RID: 58011 RVA: 0x00425C05 File Offset: 0x00423E05
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ this.durationDays.GetHashCode();
		}

		// Token: 0x04009804 RID: 38916
		private float durationDays;

		// Token: 0x04009805 RID: 38917
		private string durationDaysBuf;
	}
}
