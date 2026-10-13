using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200336C RID: 13164
	public class ChoiceLetter_EntityDiscovered : ChoiceLetter
	{
		// Token: 0x1700304A RID: 12362
		// (get) Token: 0x06012818 RID: 75800 RVA: 0x0057586F File Offset: 0x00573A6F
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				if (this.codexEntry != null)
				{
					using (List<ResearchProjectDef>.Enumerator enumerator = this.codexEntry.discoveredResearchProjects.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ResearchProjectDef project = enumerator.Current;
							yield return new DiaOption("ViewHyperlink".Translate(project.label))
							{
								action = delegate()
								{
									Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Research, true);
									MainTabWindow_Research mainTabWindow_Research = MainButtonDefOf.Research.TabWindow as MainTabWindow_Research;
									if (mainTabWindow_Research != null)
									{
										mainTabWindow_Research.Select(project);
									}
								},
								resolveTree = true
							};
						}
					}
					List<ResearchProjectDef>.Enumerator enumerator = default(List<ResearchProjectDef>.Enumerator);
					yield return this.Option_OpenEntityCodex;
				}
				yield return base.Option_Close;
				yield break;
				yield break;
			}
		}

		// Token: 0x06012819 RID: 75801 RVA: 0x0057587F File Offset: 0x00573A7F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<EntityCodexEntryDef>(ref this.codexEntry, "codexEntry");
		}

		// Token: 0x1700304B RID: 12363
		// (get) Token: 0x0601281A RID: 75802 RVA: 0x00575897 File Offset: 0x00573A97
		protected DiaOption Option_OpenEntityCodex
		{
			get
			{
				return new DiaOption("ViewEntityCodex".Translate())
				{
					action = delegate()
					{
						if (this.codexEntry != null)
						{
							Find.WindowStack.Add(new Dialog_EntityCodex(this.codexEntry));
						}
					},
					resolveTree = true
				};
			}
		}

		// Token: 0x0400C03D RID: 49213
		public EntityCodexEntryDef codexEntry;
	}
}
