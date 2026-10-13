using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B1E RID: 15134
	public class QuestPart_EngineClaimed : QuestPart
	{
		// Token: 0x06014968 RID: 84328 RVA: 0x00624918 File Offset: 0x00622B18
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (!signal.tag.EndsWith("ChangedFactionToPlayer"))
			{
				return;
			}
			NamedArgument namedArgument;
			if (signal.args.TryGetArg("SUBJECT", out namedArgument))
			{
				Thing thing = namedArgument.arg as Thing;
				if (thing != null && thing.def == ThingDefOf.GravEngine)
				{
					this.quest.End(QuestEndOutcome.Success, true, true);
				}
			}
		}
	}
}
