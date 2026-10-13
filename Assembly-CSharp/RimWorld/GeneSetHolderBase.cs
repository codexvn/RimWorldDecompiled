using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002AE7 RID: 10983
	public abstract class GeneSetHolderBase : ThingWithComps
	{
		// Token: 0x1700264D RID: 9805
		// (get) Token: 0x0600F4F4 RID: 62708 RVA: 0x00472F07 File Offset: 0x00471107
		public GeneSet GeneSet
		{
			get
			{
				return this.geneSet;
			}
		}

		// Token: 0x1700264E RID: 9806
		// (get) Token: 0x0600F4F5 RID: 62709 RVA: 0x00472F0F File Offset: 0x0047110F
		protected virtual string InspectGeneDescription
		{
			get
			{
				return "InspectGenesDesc".Translate(this);
			}
		}

		// Token: 0x1700264F RID: 9807
		// (get) Token: 0x0600F4F6 RID: 62710 RVA: 0x00472F28 File Offset: 0x00471128
		public override string DescriptionDetailed
		{
			get
			{
				this.tmpGeneLabelsDesc.Clear();
				string text = base.DescriptionDetailed;
				if (this.geneSet == null || !this.geneSet.GenesListForReading.Any<GeneDef>())
				{
					return text;
				}
				if (!text.NullOrEmpty())
				{
					text += "\n\n";
				}
				for (int i = 0; i < this.geneSet.GenesListForReading.Count; i++)
				{
					this.tmpGeneLabelsDesc.Add(this.geneSet.GenesListForReading[i].label);
				}
				return text + ("Genes".Translate().CapitalizeFirst() + ":\n" + this.tmpGeneLabelsDesc.ToLineList("  - ", true));
			}
		}

		// Token: 0x0600F4F7 RID: 62711 RVA: 0x00472FF2 File Offset: 0x004711F2
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats())
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			if (this.geneSet != null)
			{
				Dialog_InfoCard.Hyperlink? inspectGenesHyperlink = null;
				if (ThingSelectionUtility.SelectableByMapClick(this))
				{
					inspectGenesHyperlink = new Dialog_InfoCard.Hyperlink?(new Dialog_InfoCard.Hyperlink(this, -1, true));
				}
				foreach (StatDrawEntry statDrawEntry2 in this.geneSet.SpecialDisplayStats(inspectGenesHyperlink))
				{
					yield return statDrawEntry2;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600F4F8 RID: 62712 RVA: 0x00473004 File Offset: 0x00471204
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			this.tmpGeneLabels.Clear();
			if (this.geneSet != null && this.geneSet.GenesListForReading.Any<GeneDef>())
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				List<GeneDef> genesListForReading = this.geneSet.GenesListForReading;
				int num = Mathf.Min(5, genesListForReading.Count);
				for (int i = 0; i < num; i++)
				{
					string text2 = genesListForReading[i].label;
					if (this.geneSet.IsOverridden(genesListForReading[i]))
					{
						text2 += " (" + "Overridden".Translate() + ")";
					}
					this.tmpGeneLabels.Add(text2);
				}
				if (genesListForReading.Count > num)
				{
					this.tmpGeneLabels.Add("Etc".Translate() + "...");
				}
				text += "Genes".Translate().CapitalizeFirst() + ":\n" + this.tmpGeneLabels.ToLineList("  - ", true);
			}
			return text;
		}

		// Token: 0x0600F4F9 RID: 62713 RVA: 0x00473144 File Offset: 0x00471344
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.geneSet != null)
			{
				Command_Action command_Action = new Command_Action();
				command_Action.defaultLabel = "InspectGenes".Translate() + "...";
				command_Action.defaultDesc = this.InspectGeneDescription;
				command_Action.icon = GeneSetHolderBase.GeneticInfoTex.Texture;
				command_Action.action = delegate()
				{
					InspectPaneUtility.OpenTab(typeof(ITab_Genes));
				};
				yield return command_Action;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600F4FA RID: 62714 RVA: 0x00473154 File Offset: 0x00471354
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<GeneSet>(ref this.geneSet, "geneSet", Array.Empty<object>());
		}

		// Token: 0x0400A164 RID: 41316
		protected GeneSet geneSet;

		// Token: 0x0400A165 RID: 41317
		private const int MaxGeneLabels = 5;

		// Token: 0x0400A166 RID: 41318
		public static readonly CachedTexture GeneticInfoTex = new CachedTexture("UI/Gizmos/ViewGenes");

		// Token: 0x0400A167 RID: 41319
		private List<string> tmpGeneLabelsDesc = new List<string>();

		// Token: 0x0400A168 RID: 41320
		private List<string> tmpGeneLabels = new List<string>();
	}
}
