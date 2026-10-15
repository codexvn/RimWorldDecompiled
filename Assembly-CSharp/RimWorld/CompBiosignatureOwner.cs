using Verse;

namespace RimWorld;

public class CompBiosignatureOwner : ThingComp
{
	public int biosignature;

	private string biosignatureName;

	public CompProperties_BiosignatureOwner Props => (CompProperties_BiosignatureOwner)props;

	public string BiosignatureName => biosignatureName ?? (biosignatureName = AnomalyUtility.GetBiosignatureName(biosignature));

	public override string CompInspectStringExtra()
	{
		string value;
		if (Props.requiresAnalysis)
		{
			Find.AnalysisManager.TryGetAnalysisProgress(biosignature, out var details);
			value = ((details == null || details.timesDone == 0) ? "Unknown".Translate().Resolve().CapitalizeFirst() : $"{BiosignatureName} ({"Analyzed".Translate()} x{details.timesDone})");
		}
		else
		{
			value = BiosignatureName;
		}
		return $"{"Biosignature".Translate()}: {value}";
	}

	public override void PostPostMake()
	{
		biosignature = Rand.Int;
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref biosignature, "biosignature", 0);
	}
}
