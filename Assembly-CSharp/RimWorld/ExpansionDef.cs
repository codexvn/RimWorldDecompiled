using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A20 RID: 6688
	public class ExpansionDef : Def
	{
		// Token: 0x170016FE RID: 5886
		// (get) Token: 0x060096BB RID: 38587 RVA: 0x002C165B File Offset: 0x002BF85B
		public Texture2D IconFromStatus
		{
			get
			{
				if (this.Status != ExpansionStatus.NotInstalled)
				{
					return this.Icon;
				}
				return this.NotOwnedIcon;
			}
		}

		// Token: 0x170016FF RID: 5887
		// (get) Token: 0x060096BC RID: 38588 RVA: 0x002C1673 File Offset: 0x002BF873
		public Texture2D Icon
		{
			get
			{
				if (this.cachedIcon == null)
				{
					this.cachedIcon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				}
				return this.cachedIcon;
			}
		}

		// Token: 0x17001700 RID: 5888
		// (get) Token: 0x060096BD RID: 38589 RVA: 0x002C169B File Offset: 0x002BF89B
		private Texture2D NotOwnedIcon
		{
			get
			{
				if (this.notOwnedIconPath.NullOrEmpty())
				{
					return this.Icon;
				}
				if (this.cachedNotOwnedIcon == null)
				{
					this.cachedNotOwnedIcon = ContentFinder<Texture2D>.Get(this.notOwnedIconPath, true);
				}
				return this.cachedNotOwnedIcon;
			}
		}

		// Token: 0x17001701 RID: 5889
		// (get) Token: 0x060096BE RID: 38590 RVA: 0x002C16D7 File Offset: 0x002BF8D7
		public Texture2D BackgroundImage
		{
			get
			{
				if (this.cachedBG == null)
				{
					this.cachedBG = ContentFinder<Texture2D>.Get(this.backgroundPath, true);
				}
				return this.cachedBG;
			}
		}

		// Token: 0x17001702 RID: 5890
		// (get) Token: 0x060096BF RID: 38591 RVA: 0x002C16FF File Offset: 0x002BF8FF
		public List<Texture2D> PreviewImages
		{
			get
			{
				if (this.cachedPreviewImages.NullOrEmpty<Texture2D>())
				{
					if (this.previewImagesFolderPath.NullOrEmpty())
					{
						return null;
					}
					this.cachedPreviewImages = new List<Texture2D>(ContentFinder<Texture2D>.GetAllInFolder(this.previewImagesFolderPath));
				}
				return this.cachedPreviewImages;
			}
		}

		// Token: 0x17001703 RID: 5891
		// (get) Token: 0x060096C0 RID: 38592 RVA: 0x002C1739 File Offset: 0x002BF939
		public string StoreURL
		{
			get
			{
				if (!this.steamUrl.NullOrEmpty())
				{
					return this.steamUrl;
				}
				return this.siteUrl;
			}
		}

		// Token: 0x17001704 RID: 5892
		// (get) Token: 0x060096C1 RID: 38593 RVA: 0x002C1755 File Offset: 0x002BF955
		public ExpansionStatus Status
		{
			get
			{
				if (ModsConfig.IsActive(this.linkedMod))
				{
					return ExpansionStatus.Active;
				}
				if (ModLister.AllInstalledMods.Any((ModMetaData m) => m.SamePackageId(this.linkedMod, false)))
				{
					return ExpansionStatus.Installed;
				}
				return ExpansionStatus.NotInstalled;
			}
		}

		// Token: 0x17001705 RID: 5893
		// (get) Token: 0x060096C2 RID: 38594 RVA: 0x002C1784 File Offset: 0x002BF984
		public string StatusDescription
		{
			get
			{
				ExpansionStatus status = this.Status;
				if (status == ExpansionStatus.Active)
				{
					return "ContentActive".Translate();
				}
				if (status != ExpansionStatus.Installed)
				{
					return "ContentNotInstalled".Translate();
				}
				return "ContentInstalledButNotActive".Translate();
			}
		}

		// Token: 0x060096C3 RID: 38595 RVA: 0x002C17D0 File Offset: 0x002BF9D0
		public override void PostLoad()
		{
			base.PostLoad();
			this.linkedMod = this.linkedMod.ToLower();
		}

		// Token: 0x060096C4 RID: 38596 RVA: 0x002C17E9 File Offset: 0x002BF9E9
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(this.linkedMod, false);
			if (modWithIdentifier != null && !modWithIdentifier.Official)
			{
				yield return modWithIdentifier.Name + " - ExpansionDefs are used for official content. For mods, you should define ModMetaData in About.xml.";
			}
			yield break;
			yield break;
		}

		// Token: 0x04006347 RID: 25415
		[NoTranslate]
		public string iconPath;

		// Token: 0x04006348 RID: 25416
		[NoTranslate]
		public string notOwnedIconPath;

		// Token: 0x04006349 RID: 25417
		[NoTranslate]
		public string backgroundPath;

		// Token: 0x0400634A RID: 25418
		[NoTranslate]
		public string linkedMod;

		// Token: 0x0400634B RID: 25419
		[NoTranslate]
		public string steamUrl;

		// Token: 0x0400634C RID: 25420
		[NoTranslate]
		public string siteUrl;

		// Token: 0x0400634D RID: 25421
		[NoTranslate]
		public string previewImagesFolderPath;

		// Token: 0x0400634E RID: 25422
		public bool isCore;

		// Token: 0x0400634F RID: 25423
		public Color primaryColor = Color.white;

		// Token: 0x04006350 RID: 25424
		private Texture2D cachedIcon;

		// Token: 0x04006351 RID: 25425
		private Texture2D cachedNotOwnedIcon;

		// Token: 0x04006352 RID: 25426
		private Texture2D cachedBG;

		// Token: 0x04006353 RID: 25427
		private List<Texture2D> cachedPreviewImages;
	}
}
