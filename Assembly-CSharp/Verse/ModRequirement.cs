using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000581 RID: 1409
	[StaticConstructorOnStartup]
	public abstract class ModRequirement
	{
		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x060024F4 RID: 9460
		public abstract bool IsSatisfied { get; }

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x060024F5 RID: 9461
		public abstract string RequirementTypeLabel { get; }

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x060024F6 RID: 9462 RVA: 0x000BEF9E File Offset: 0x000BD19E
		public virtual string Tooltip
		{
			get
			{
				return "ModPackageId".Translate() + ": " + this.packageId;
			}
		}

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x060024F7 RID: 9463 RVA: 0x000BEFC4 File Offset: 0x000BD1C4
		public virtual Texture2D StatusIcon
		{
			get
			{
				if (!this.IsSatisfied)
				{
					return ModRequirement.NotResolved;
				}
				return ModRequirement.Resolved;
			}
		}

		// Token: 0x060024F8 RID: 9464
		public abstract void OnClicked(Page_ModsConfig window);

		// Token: 0x04001E70 RID: 7792
		public string packageId;

		// Token: 0x04001E71 RID: 7793
		public List<string> alternativePackageIds;

		// Token: 0x04001E72 RID: 7794
		public string displayName;

		// Token: 0x04001E73 RID: 7795
		public static Texture2D NotResolved = ContentFinder<Texture2D>.Get("UI/Icons/ModRequirements/NotResolved", true);

		// Token: 0x04001E74 RID: 7796
		public static Texture2D NotInstalled = ContentFinder<Texture2D>.Get("UI/Icons/ModRequirements/NotInstalled", true);

		// Token: 0x04001E75 RID: 7797
		public static Texture2D Installed = ContentFinder<Texture2D>.Get("UI/Icons/ModRequirements/Installed", true);

		// Token: 0x04001E76 RID: 7798
		public static Texture2D Resolved = ContentFinder<Texture2D>.Get("UI/Widgets/CheckOn", true);
	}
}
