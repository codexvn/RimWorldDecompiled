using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200310D RID: 12557
	public class Dialog_AddPreferredName : Window
	{
		// Token: 0x17002DDE RID: 11742
		// (get) Token: 0x06011A4B RID: 72267 RVA: 0x005101B4 File Offset: 0x0050E3B4
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(400f, 650f);
			}
		}

		// Token: 0x06011A4C RID: 72268 RVA: 0x005101C8 File Offset: 0x0050E3C8
		public Dialog_AddPreferredName() : base(null)
		{
			this.doCloseButton = true;
			this.absorbInputAroundWindow = true;
			this.lastSearch = "";
			this.cachedNames = (from n in (from b in SolidBioDatabase.allBios
			select b.name).Concat(PawnNameDatabaseSolid.AllNames())
			orderby n.Last descending
			select n).ToList<NameTriple>();
			this.CacheBlacklist();
		}

		// Token: 0x06011A4D RID: 72269 RVA: 0x00510268 File Offset: 0x0050E468
		public override void DoWindowContents(Rect inRect)
		{
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.Begin(inRect);
			listing_Standard.Label("TypeFirstNickOrLastName".Translate(), -1f, null);
			string text = listing_Standard.TextEntry(this.searchName, 1);
			if (text.Length < 20)
			{
				this.searchName = text;
				this.searchWords = this.searchName.Replace("'", "").Split(' ', StringSplitOptions.None);
			}
			listing_Standard.Gap(4f);
			int num = Mathf.FloorToInt((inRect.height - (Window.CloseButSize.y + 8f) - listing_Standard.CurHeight) / (30f + listing_Standard.verticalSpacing));
			if (this.searchName.Length > 1)
			{
				if (this.searchName != this.lastSearch)
				{
					this.lastSearch = this.searchName;
					Dialog_AddPreferredName.cachedSearch.Clear();
					foreach (NameTriple nameTriple in this.cachedNames.Where(new Func<NameTriple, bool>(this.FilterMatch)))
					{
						Dialog_AddPreferredName.cachedSearch.Add(nameTriple);
						if (listing_Standard.ButtonText(nameTriple.ToString(), null, 1f))
						{
							this.TryChooseName(nameTriple);
						}
						if (Dialog_AddPreferredName.cachedSearch.Count >= num)
						{
							break;
						}
					}
				}
				foreach (NameTriple nameTriple2 in Dialog_AddPreferredName.cachedSearch)
				{
					if (listing_Standard.ButtonText(nameTriple2.ToString(), null, 1f))
					{
						this.TryChooseName(nameTriple2);
					}
				}
			}
			listing_Standard.End();
		}

		// Token: 0x06011A4E RID: 72270 RVA: 0x00510438 File Offset: 0x0050E638
		private bool FilterMatch(NameTriple n)
		{
			if (Dialog_AddPreferredName.IsBlacklisted(n))
			{
				return false;
			}
			if (this.searchWords.Length == 0)
			{
				return false;
			}
			if (this.searchWords.Length == 1)
			{
				return n.Last.StartsWith(this.searchName, StringComparison.OrdinalIgnoreCase) || n.First.StartsWith(this.searchName, StringComparison.OrdinalIgnoreCase) || n.Nick.StartsWith(this.searchName, StringComparison.OrdinalIgnoreCase);
			}
			if (this.searchWords.Length > 1)
			{
				string value = this.searchName.Replace("'", "");
				if (n.First.IndexOf(' ') >= 0)
				{
					return n.First.StartsWith(value, StringComparison.OrdinalIgnoreCase);
				}
				if (n.Last.IndexOf(' ') >= 0)
				{
					return n.Last.StartsWith(value, StringComparison.OrdinalIgnoreCase);
				}
				if (n.Nick.IndexOf(' ') >= 0)
				{
					return n.Nick.StartsWith(value, StringComparison.OrdinalIgnoreCase);
				}
			}
			return this.searchWords.Length == 2 && n.First.EqualsIgnoreCase(this.searchWords[0]) && (n.Last.StartsWith(this.searchWords[1], StringComparison.OrdinalIgnoreCase) || n.Nick.StartsWith(this.searchWords[1], StringComparison.OrdinalIgnoreCase));
		}

		// Token: 0x06011A4F RID: 72271 RVA: 0x0051056D File Offset: 0x0050E76D
		private void TryChooseName(NameTriple name)
		{
			if (this.AlreadyPreferred(name))
			{
				Messages.Message("MessageAlreadyPreferredName".Translate(), MessageTypeDefOf.RejectInput, false);
				return;
			}
			Prefs.PreferredNames.Add(name.ToString());
			this.Close(true);
		}

		// Token: 0x06011A50 RID: 72272 RVA: 0x005105AA File Offset: 0x0050E7AA
		private bool AlreadyPreferred(NameTriple name)
		{
			return Prefs.PreferredNames.Contains(name.ToString());
		}

		// Token: 0x06011A51 RID: 72273 RVA: 0x005105BC File Offset: 0x0050E7BC
		private void CacheBlacklist()
		{
			if (Dialog_AddPreferredName.Blacklist.Any<NameTriple>())
			{
				return;
			}
			Dialog_AddPreferredName.Blacklist.AddRange((from x in SolidBioDatabase.allBios
			where x.rare
			select x into b
			select b.name).ToList<NameTriple>());
		}

		// Token: 0x06011A52 RID: 72274 RVA: 0x00510634 File Offset: 0x0050E834
		private static bool IsBlacklisted(NameTriple triple)
		{
			foreach (NameTriple nameTriple in Dialog_AddPreferredName.Blacklist)
			{
				if (nameTriple.First.EqualsIgnoreCase(triple.First) && nameTriple.Last.EqualsIgnoreCase(triple.Last))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400B5A8 RID: 46504
		private string searchName = "";

		// Token: 0x0400B5A9 RID: 46505
		private string[] searchWords;

		// Token: 0x0400B5AA RID: 46506
		private List<NameTriple> cachedNames;

		// Token: 0x0400B5AB RID: 46507
		private static readonly List<NameTriple> Blacklist = new List<NameTriple>();

		// Token: 0x0400B5AC RID: 46508
		private static readonly List<NameTriple> cachedSearch = new List<NameTriple>();

		// Token: 0x0400B5AD RID: 46509
		private string lastSearch;
	}
}
