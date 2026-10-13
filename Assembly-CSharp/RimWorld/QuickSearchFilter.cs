using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003490 RID: 13456
	public class QuickSearchFilter
	{
		// Token: 0x1700315E RID: 12638
		// (get) Token: 0x06012ECD RID: 77517 RVA: 0x0059A896 File Offset: 0x00598A96
		// (set) Token: 0x06012ECE RID: 77518 RVA: 0x0059A89E File Offset: 0x00598A9E
		public string Text
		{
			get
			{
				return this.inputText;
			}
			set
			{
				this.inputText = value;
				this.searchText = value.Trim();
				this.cachedMatches.Clear();
			}
		}

		// Token: 0x1700315F RID: 12639
		// (get) Token: 0x06012ECF RID: 77519 RVA: 0x0059A8BE File Offset: 0x00598ABE
		public bool Active
		{
			get
			{
				return !this.inputText.NullOrEmpty();
			}
		}

		// Token: 0x06012ED0 RID: 77520 RVA: 0x0059A8D0 File Offset: 0x00598AD0
		public bool Matches(string value)
		{
			if (!this.Active)
			{
				return true;
			}
			if (value.NullOrEmpty())
			{
				return false;
			}
			bool flag;
			if (!this.cachedMatches.TryGetValue(value, out flag))
			{
				flag = this.MatchImpl(value);
				this.cachedMatches.Add(value, flag);
			}
			return flag;
		}

		// Token: 0x06012ED1 RID: 77521 RVA: 0x0059A917 File Offset: 0x00598B17
		private bool MatchImpl(string value)
		{
			return value.IndexOf(this.searchText, StringComparison.InvariantCultureIgnoreCase) != -1;
		}

		// Token: 0x06012ED2 RID: 77522 RVA: 0x0059A92C File Offset: 0x00598B2C
		public bool Matches(ThingDef td)
		{
			return !Find.HiddenItemsManager.Hidden(td) && this.Matches(td.label);
		}

		// Token: 0x06012ED3 RID: 77523 RVA: 0x0059A949 File Offset: 0x00598B49
		public bool Matches(SpecialThingFilterDef sfDef)
		{
			return this.Matches(sfDef.label);
		}

		// Token: 0x0400C402 RID: 50178
		private string inputText = "";

		// Token: 0x0400C403 RID: 50179
		private string searchText = "";

		// Token: 0x0400C404 RID: 50180
		private readonly LRUCache<string, bool> cachedMatches = new LRUCache<string, bool>(5000);
	}
}
