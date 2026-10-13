using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B17 RID: 15127
	public class QuestPart_CerebrexCore : QuestPartActivable
	{
		// Token: 0x1700334F RID: 13135
		// (get) Token: 0x0601494D RID: 84301 RVA: 0x00623FE1 File Offset: 0x006221E1
		private string StabilizerDestroyedSignal
		{
			get
			{
				return this.stabilizerTag + ".Destroyed";
			}
		}

		// Token: 0x0601494E RID: 84302 RVA: 0x002D9B28 File Offset: 0x002D7D28
		public QuestPart_CerebrexCore()
		{
		}

		// Token: 0x0601494F RID: 84303 RVA: 0x00623FF3 File Offset: 0x006221F3
		public QuestPart_CerebrexCore(Site site, string inSignalEnable, string coreTag, string stabilizerTag)
		{
			this.site = site;
			this.inSignalEnable = inSignalEnable;
			this.coreTag = coreTag;
			this.stabilizerTag = stabilizerTag;
		}

		// Token: 0x06014950 RID: 84304 RVA: 0x00624018 File Offset: 0x00622218
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Site>(ref this.site, "site", false);
			Scribe_Collections.Look<Thing>(ref this.stabilizers, "stabilizers", LookMode.Reference, Array.Empty<object>());
			Scribe_References.Look<Thing>(ref this.core, "core", false);
			Scribe_Values.Look<string>(ref this.coreTag, "coreTag", null, false);
			Scribe_Values.Look<string>(ref this.stabilizerTag, "stabilizerTag", null, false);
		}

		// Token: 0x06014951 RID: 84305 RVA: 0x00624088 File Offset: 0x00622288
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			Thing thing;
			signal.args.TryGetArg<Thing>("SUBJECT", out thing);
			if (signal.tag == this.inSignalEnable && this.SetupTags())
			{
				return;
			}
			if (signal.tag != this.StabilizerDestroyedSignal)
			{
				return;
			}
			CompCerebrexCore compCerebrexCore = this.core.TryGetComp<CompCerebrexCore>();
			MechhiveUtility.FireRaid(this.site.Map, StorytellerUtility.DefaultThreatPointsNow(this.site.Map), -1, "MechhiveDefenseResponseLetterLabel".Translate(), "MechhiveDefenseResponseLetterText".Translate());
			compCerebrexCore.Notify_StabilizerDisabled();
		}

		// Token: 0x06014952 RID: 84306 RVA: 0x00624130 File Offset: 0x00622330
		private bool SetupTags()
		{
			this.stabilizers = this.site.Map.listerThings.ThingsOfDef(ThingDefOf.CerebrexStabilizer).ToList<Thing>();
			this.core = this.site.Map.listerThings.ThingsOfDef(ThingDefOf.CerebrexCore).FirstOrDefault<Thing>();
			if (this.stabilizers.NullOrEmpty<Thing>() || this.stabilizers.Count != 3 || this.core == null)
			{
				Log.Error("Mechhive Quest: Could not find cerebrex core or stabilizers.");
				return true;
			}
			foreach (Thing thing in this.stabilizers)
			{
				QuestUtility.AddQuestTag(ref thing.questTags, this.stabilizerTag);
			}
			QuestUtility.AddQuestTag(ref this.core.questTags, this.coreTag);
			return false;
		}

		// Token: 0x0400E490 RID: 58512
		private Site site;

		// Token: 0x0400E491 RID: 58513
		private List<Thing> stabilizers;

		// Token: 0x0400E492 RID: 58514
		private Thing core;

		// Token: 0x0400E493 RID: 58515
		private string coreTag;

		// Token: 0x0400E494 RID: 58516
		private string stabilizerTag;
	}
}
