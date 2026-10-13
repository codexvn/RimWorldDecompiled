using System;
using System.IO;
using System.Linq;
using RimWorld;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000D92 RID: 3474
	public class WorkshopItem_Scenario : WorkshopItem
	{
		// Token: 0x17000E0D RID: 3597
		// (get) Token: 0x060058AA RID: 22698 RVA: 0x001C9A4E File Offset: 0x001C7C4E
		// (set) Token: 0x060058AB RID: 22699 RVA: 0x001C9A56 File Offset: 0x001C7C56
		public override PublishedFileId_t PublishedFileId
		{
			get
			{
				return base.PublishedFileId;
			}
			set
			{
				base.PublishedFileId = value;
				if (this.cachedScenario != null)
				{
					this.cachedScenario.SetPublishedFileId(value);
				}
			}
		}

		// Token: 0x060058AC RID: 22700 RVA: 0x001C9A73 File Offset: 0x001C7C73
		public Scenario GetScenario()
		{
			if (this.cachedScenario == null)
			{
				this.LoadScenario();
			}
			return this.cachedScenario;
		}

		// Token: 0x060058AD RID: 22701 RVA: 0x001C9A8C File Offset: 0x001C7C8C
		private void LoadScenario()
		{
			if (GameDataSaveLoader.TryLoadScenario(base.Directory.GetFiles("*.rsc").First((FileInfo fi) => fi.Extension == ".rsc").FullName, ScenarioCategory.SteamWorkshop, out this.cachedScenario))
			{
				this.cachedScenario.SetPublishedFileId(this.PublishedFileId);
			}
		}

		// Token: 0x04004058 RID: 16472
		private Scenario cachedScenario;
	}
}
