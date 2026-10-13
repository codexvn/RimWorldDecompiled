using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C28 RID: 11304
	public class CompBiosignatureOwner : ThingComp
	{
		// Token: 0x170027DC RID: 10204
		// (get) Token: 0x0600FD67 RID: 64871 RVA: 0x004983DC File Offset: 0x004965DC
		public CompProperties_BiosignatureOwner Props
		{
			get
			{
				return (CompProperties_BiosignatureOwner)this.props;
			}
		}

		// Token: 0x170027DD RID: 10205
		// (get) Token: 0x0600FD68 RID: 64872 RVA: 0x004983EC File Offset: 0x004965EC
		public string BiosignatureName
		{
			get
			{
				string result;
				if ((result = this.biosignatureName) == null)
				{
					result = (this.biosignatureName = AnomalyUtility.GetBiosignatureName(this.biosignature));
				}
				return result;
			}
		}

		// Token: 0x0600FD69 RID: 64873 RVA: 0x00498418 File Offset: 0x00496618
		public override string CompInspectStringExtra()
		{
			string arg;
			if (this.Props.requiresAnalysis)
			{
				AnalysisDetails analysisDetails;
				Find.AnalysisManager.TryGetAnalysisProgress(this.biosignature, out analysisDetails);
				arg = ((analysisDetails == null || analysisDetails.timesDone == 0) ? "Unknown".Translate().Resolve().CapitalizeFirst() : string.Format("{0} ({1} x{2})", this.BiosignatureName, "Analyzed".Translate(), analysisDetails.timesDone));
			}
			else
			{
				arg = this.BiosignatureName;
			}
			return string.Format("{0}: {1}", "Biosignature".Translate(), arg);
		}

		// Token: 0x0600FD6A RID: 64874 RVA: 0x004984BD File Offset: 0x004966BD
		public override void PostPostMake()
		{
			this.biosignature = Rand.Int;
		}

		// Token: 0x0600FD6B RID: 64875 RVA: 0x004984CA File Offset: 0x004966CA
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.biosignature, "biosignature", 0, false);
		}

		// Token: 0x0400A614 RID: 42516
		public int biosignature;

		// Token: 0x0400A615 RID: 42517
		private string biosignatureName;
	}
}
