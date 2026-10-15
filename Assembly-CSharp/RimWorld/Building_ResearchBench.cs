using Verse;

namespace RimWorld;

public class Building_ResearchBench : Building
{
	public override string GetInspectString()
	{
		string text = base.GetInspectString();
		if (!text.NullOrEmpty())
		{
			text += "\n";
		}
		ResearchProjectDef project = Find.ResearchManager.GetProject();
		if (project != null)
		{
			return text + $"{"CurrentProject".Translate()}: {project.LabelCap}\n{"ResearchProgress".Translate()}: {project.ProgressApparent:F0} / {project.CostApparent:F0} ({project.ProgressPercent.ToStringPercent("0.#")})";
		}
		return text + $"{"CurrentProject".Translate()}: {"None".Translate()}";
	}
}
