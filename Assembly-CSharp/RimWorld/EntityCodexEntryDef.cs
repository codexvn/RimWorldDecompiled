using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A1D RID: 6685
	public class EntityCodexEntryDef : Def
	{
		// Token: 0x170016F9 RID: 5881
		// (get) Token: 0x060096A9 RID: 38569 RVA: 0x002C11E2 File Offset: 0x002BF3E2
		public string Description
		{
			get
			{
				if (this.useDescriptionFrom == null)
				{
					return this.description;
				}
				return this.useDescriptionFrom.description;
			}
		}

		// Token: 0x170016FA RID: 5882
		// (get) Token: 0x060096AA RID: 38570 RVA: 0x002C11FE File Offset: 0x002BF3FE
		public bool Discovered
		{
			get
			{
				return Find.EntityCodex.Discovered(this);
			}
		}

		// Token: 0x170016FB RID: 5883
		// (get) Token: 0x060096AB RID: 38571 RVA: 0x002C120B File Offset: 0x002BF40B
		public bool Visible
		{
			get
			{
				return Current.ProgramState != ProgramState.Playing || !this.hideInPlaystyles.Contains(Find.Storyteller.difficulty.AnomalyPlaystyleDef);
			}
		}

		// Token: 0x060096AC RID: 38572 RVA: 0x002C1234 File Offset: 0x002BF434
		public override void PostLoad()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.icon = (this.uiIconPath.NullOrEmpty() ? BaseContent.BadTex : ContentFinder<Texture2D>.Get(this.uiIconPath, true));
				this.silhouette = (this.uiIconPath.NullOrEmpty() ? BaseContent.BadTex : ContentFinder<Texture2D>.Get(this.uiIconPath + "_Silhouette", true));
			});
		}

		// Token: 0x060096AD RID: 38573 RVA: 0x002C1248 File Offset: 0x002BF448
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			foreach (ThingDef thingDef in this.linkedThings)
			{
				if (thingDef.entityCodexEntry == null)
				{
					thingDef.entityCodexEntry = this;
				}
				else
				{
					Log.Error(string.Concat(new string[]
					{
						"EntityCodexEntryDef ",
						this.defName,
						" is linked to ",
						thingDef.defName,
						" but ",
						thingDef.defName,
						" is already linked to ",
						thingDef.entityCodexEntry.defName,
						"."
					}));
				}
			}
			foreach (IncidentDef incidentDef in this.linkedIncidents)
			{
				if (incidentDef.codexEntry == null)
				{
					incidentDef.codexEntry = this;
				}
				else
				{
					Log.Error(string.Concat(new string[]
					{
						"EntityCodexEntryDef ",
						this.defName,
						" is linked to ",
						incidentDef.defName,
						" but ",
						incidentDef.defName,
						" is already linked to ",
						incidentDef.codexEntry.defName,
						"."
					}));
				}
			}
		}

		// Token: 0x060096AE RID: 38574 RVA: 0x002C13C4 File Offset: 0x002BF5C4
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.category == null)
			{
				yield return "category is null.";
			}
			if (this.uiIconPath.NullOrEmpty())
			{
				yield return "missing icon.";
			}
			yield break;
			yield break;
		}

		// Token: 0x0400632F RID: 25391
		public EntityCategoryDef category;

		// Token: 0x04006330 RID: 25392
		public bool startDiscovered;

		// Token: 0x04006331 RID: 25393
		public List<ThingDef> linkedThings = new List<ThingDef>();

		// Token: 0x04006332 RID: 25394
		private List<IncidentDef> linkedIncidents = new List<IncidentDef>();

		// Token: 0x04006333 RID: 25395
		public List<ResearchProjectDef> discoveredResearchProjects = new List<ResearchProjectDef>();

		// Token: 0x04006334 RID: 25396
		public List<IncidentDef> provocationIncidents = new List<IncidentDef>();

		// Token: 0x04006335 RID: 25397
		public EntityDiscoveryType discoveryType;

		// Token: 0x04006336 RID: 25398
		public bool allowDiscoveryWhileMapGenerating;

		// Token: 0x04006337 RID: 25399
		public int orderInCategory = 9999999;

		// Token: 0x04006338 RID: 25400
		public List<AnomalyPlaystyleDef> hideInPlaystyles = new List<AnomalyPlaystyleDef>();

		// Token: 0x04006339 RID: 25401
		private ThingDef useDescriptionFrom;

		// Token: 0x0400633A RID: 25402
		[NoTranslate]
		private string uiIconPath;

		// Token: 0x0400633B RID: 25403
		public Texture2D icon;

		// Token: 0x0400633C RID: 25404
		public Texture2D silhouette;

		// Token: 0x0400633D RID: 25405
		private const string SilhouetteTexPathSuffix = "_Silhouette";
	}
}
