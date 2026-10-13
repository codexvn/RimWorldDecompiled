using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000582 RID: 1410
	public class ModDependency : ModRequirement
	{
		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x060024FB RID: 9467 RVA: 0x000BF029 File Offset: 0x000BD229
		public override string RequirementTypeLabel
		{
			get
			{
				return "ModDependsOn".Translate("");
			}
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x060024FC RID: 9468 RVA: 0x000BF044 File Offset: 0x000BD244
		public override bool IsSatisfied
		{
			get
			{
				if (ModLister.GetActiveModWithIdentifier(this.packageId, true) != null)
				{
					return true;
				}
				List<string> alternativePackageIds = this.alternativePackageIds;
				if (alternativePackageIds == null)
				{
					return false;
				}
				return alternativePackageIds.Any((string id) => ModLister.GetActiveModWithIdentifier(id, true) != null);
			}
		}

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x060024FD RID: 9469 RVA: 0x000BF094 File Offset: 0x000BD294
		private ModMetaData Mod
		{
			get
			{
				ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(this.packageId, true);
				if (modWithIdentifier != null)
				{
					return modWithIdentifier;
				}
				if (this.alternativePackageIds.NullOrEmpty<string>())
				{
					return null;
				}
				foreach (string identifier in this.alternativePackageIds)
				{
					modWithIdentifier = ModLister.GetModWithIdentifier(identifier, true);
					if (modWithIdentifier != null)
					{
						return modWithIdentifier;
					}
				}
				return null;
			}
		}

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x060024FE RID: 9470 RVA: 0x000BF114 File Offset: 0x000BD314
		public override Texture2D StatusIcon
		{
			get
			{
				if (this.Mod == null)
				{
					return ModRequirement.NotInstalled;
				}
				if (!this.Mod.Active)
				{
					return ModRequirement.Installed;
				}
				return ModRequirement.Resolved;
			}
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x060024FF RID: 9471 RVA: 0x000BF13C File Offset: 0x000BD33C
		public override string Tooltip
		{
			get
			{
				if (this.Mod == null)
				{
					return base.Tooltip + "\n" + "ContentNotInstalled".Translate() + "\n\n" + "ModClickToGoToWebsite".Translate();
				}
				if (!this.Mod.Active)
				{
					return base.Tooltip + "\n" + "ContentInstalledButNotActive".Translate() + "\n\n" + "ModClickToSelect".Translate();
				}
				return base.Tooltip;
			}
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x06002500 RID: 9472 RVA: 0x000BF1E0 File Offset: 0x000BD3E0
		public string Url
		{
			get
			{
				return this.steamWorkshopUrl ?? this.downloadUrl;
			}
		}

		// Token: 0x06002501 RID: 9473 RVA: 0x000BF1F2 File Offset: 0x000BD3F2
		public override void OnClicked(Page_ModsConfig window)
		{
			if (this.Mod == null)
			{
				if (!this.Url.NullOrEmpty())
				{
					SteamUtility.OpenUrl(this.Url);
					return;
				}
			}
			else if (!this.Mod.Active)
			{
				window.SelectMod(this.Mod);
			}
		}

		// Token: 0x04001E77 RID: 7799
		public string downloadUrl;

		// Token: 0x04001E78 RID: 7800
		public string steamWorkshopUrl;
	}
}
