using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CDB RID: 11483
	public class CompHasSources : ThingComp
	{
		// Token: 0x170028D8 RID: 10456
		// (get) Token: 0x06010215 RID: 66069 RVA: 0x004AABF2 File Offset: 0x004A8DF2
		public CompProperties_HasSources Props
		{
			get
			{
				return (CompProperties_HasSources)this.props;
			}
		}

		// Token: 0x06010216 RID: 66070 RVA: 0x004AABFF File Offset: 0x004A8DFF
		public void AddSource(string sourceLabel)
		{
			if (!sourceLabel.NullOrEmpty() && !this.sources.Contains(sourceLabel))
			{
				this.sources.Add(sourceLabel);
			}
		}

		// Token: 0x06010217 RID: 66071 RVA: 0x004AAC24 File Offset: 0x004A8E24
		public void TransferSourcesTo(CompHasSources other, int count = -1)
		{
			if (count < 0)
			{
				count = this.sources.Count;
			}
			int num = 0;
			string text;
			while (num < count && this.sources.TryRandomElement(out text))
			{
				other.AddSource(text);
				this.sources.Remove(text);
				num++;
			}
		}

		// Token: 0x06010218 RID: 66072 RVA: 0x004AAC74 File Offset: 0x004A8E74
		public override string TransformLabel(string label)
		{
			if (!this.Props.affectLabel || this.sources.NullOrEmpty<string>() || this.sources.Count > 1 || this.parent.stackCount > 1)
			{
				return label;
			}
			return "ThingOfSource".Translate(label, this.sources[0]);
		}

		// Token: 0x06010219 RID: 66073 RVA: 0x004AACE0 File Offset: 0x004A8EE0
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			CompHasSources compHasSources = otherStack.TryGetComp<CompHasSources>();
			if (compHasSources == null)
			{
				return;
			}
			compHasSources.TransferSourcesTo(this, count);
		}

		// Token: 0x0601021A RID: 66074 RVA: 0x004AAD00 File Offset: 0x004A8F00
		public override void PostSplitOff(Thing piece)
		{
			CompHasSources compHasSources = piece.TryGetComp<CompHasSources>();
			if (compHasSources == null || piece == this.parent)
			{
				return;
			}
			compHasSources.sources.Clear();
			this.TransferSourcesTo(compHasSources, piece.stackCount);
		}

		// Token: 0x0601021B RID: 66075 RVA: 0x004AAD3C File Offset: 0x004A8F3C
		public override string CompInspectStringExtra()
		{
			if (this.sources.NullOrEmpty<string>())
			{
				return base.CompInspectStringExtra();
			}
			string text = string.Empty;
			if (this.sources.Count > 1 || !this.Props.affectLabel)
			{
				this.tmpSourceLabels.Clear();
				int num = Mathf.Min(this.sources.Count, 9);
				for (int i = 0; i < num; i++)
				{
					this.tmpSourceLabels.Add(this.sources[i]);
				}
				if (this.sources.Count > 9)
				{
					this.tmpSourceLabels.Add("Etc".Translate() + "...");
				}
				string str = (this.Props.inspectStringLabel ?? "ThingOf".Translate(Find.ActiveLanguageWorker.Pluralize(this.parent.LabelNoCount, -1))).CapitalizeFirst();
				text = text + str + ": " + this.tmpSourceLabels.ToCommaList(false, false);
			}
			return text;
		}

		// Token: 0x0601021C RID: 66076 RVA: 0x004AAE4F File Offset: 0x004A904F
		public override void PostExposeData()
		{
			Scribe_Collections.Look<string>(ref this.sources, "sources", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.sources == null)
			{
				this.sources = new List<string>();
			}
		}

		// Token: 0x0400A888 RID: 43144
		private List<string> sources = new List<string>();

		// Token: 0x0400A889 RID: 43145
		private const int MaxSourcesToDisplayInInspectString = 9;

		// Token: 0x0400A88A RID: 43146
		private List<string> tmpSourceLabels = new List<string>();
	}
}
