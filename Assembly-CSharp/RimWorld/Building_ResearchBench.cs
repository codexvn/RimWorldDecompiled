using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A46 RID: 10822
	public class Building_ResearchBench : Building
	{
		// Token: 0x0600F055 RID: 61525 RVA: 0x0045B2E0 File Offset: 0x004594E0
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			ResearchProjectDef project = Find.ResearchManager.GetProject(null);
			if (project != null)
			{
				text += string.Format("{0}: {1}\n{2}: {3:F0} / {4:F0} ({5})", new object[]
				{
					"CurrentProject".Translate(),
					project.LabelCap,
					"ResearchProgress".Translate(),
					project.ProgressApparent,
					project.CostApparent,
					project.ProgressPercent.ToStringPercent("0.#")
				});
			}
			else
			{
				text += string.Format("{0}: {1}", "CurrentProject".Translate(), "None".Translate());
			}
			return text;
		}
	}
}
