using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D00 RID: 7424
	public class QuestPart_Infestation : QuestPart
	{
		// Token: 0x17001999 RID: 6553
		// (get) Token: 0x0600A5D5 RID: 42453 RVA: 0x002F7957 File Offset: 0x002F5B57
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.mapParent != null)
				{
					yield return this.mapParent;
				}
				if (this.mapParent != null && this.mapParent.HasMap && this.loc.IsValid)
				{
					yield return new TargetInfo(this.loc, this.mapParent.Map, false);
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x1700199A RID: 6554
		// (get) Token: 0x0600A5D6 RID: 42454 RVA: 0x002F7967 File Offset: 0x002F5B67
		public override string QuestSelectTargetsLabel
		{
			get
			{
				return "SelectHiveTargets".Translate();
			}
		}

		// Token: 0x1700199B RID: 6555
		// (get) Token: 0x0600A5D7 RID: 42455 RVA: 0x002F7978 File Offset: 0x002F5B78
		public override IEnumerable<GlobalTargetInfo> QuestSelectTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestSelectTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.mapParent != null && this.mapParent.HasMap)
				{
					List<Thing> hives = this.mapParent.Map.listerThings.ThingsOfDef(ThingDefOf.Hive);
					int num;
					for (int i = 0; i < hives.Count; i = num + 1)
					{
						Hive hive = hives[i] as Hive;
						if (hive != null && !hive.questTags.NullOrEmpty<string>() && hive.questTags.Contains(this.tag))
						{
							yield return hive;
						}
						num = i;
					}
					hives = null;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x0600A5D8 RID: 42456 RVA: 0x002F7988 File Offset: 0x002F5B88
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				this.loc = IntVec3.Invalid;
				if (this.mapParent == null || !this.mapParent.HasMap)
				{
					Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
					this.mapParent = ((anyPlayerHomeMap != null) ? anyPlayerHomeMap.Parent : null);
				}
				if (this.mapParent != null && this.mapParent.HasMap)
				{
					Thing thing = InfestationUtility.SpawnTunnels(this.hivesCount, this.mapParent.Map, true, true, this.tag, this.overrideLoc, null);
					if (thing != null)
					{
						this.loc = (this.overrideLoc ?? thing.Position);
						if (this.sendStandardLetter)
						{
							TaggedString label = this.customLetterLabel.NullOrEmpty() ? IncidentDefOf.Infestation.letterLabel : this.customLetterLabel.Formatted(IncidentDefOf.Infestation.letterLabel.Named("BASELABEL"));
							TaggedString text = this.customLetterText.NullOrEmpty() ? IncidentDefOf.Infestation.letterText : this.customLetterText.Formatted(IncidentDefOf.Infestation.letterText.Named("BASETEXT"));
							Find.LetterStack.ReceiveLetter(label, text, this.customLetterDef ?? IncidentDefOf.Infestation.letterDef, new TargetInfo(this.loc, this.mapParent.Map, false), null, this.quest, null, null, 0, true);
						}
					}
				}
			}
		}

		// Token: 0x0600A5D9 RID: 42457 RVA: 0x002F7B30 File Offset: 0x002F5D30
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<int>(ref this.hivesCount, "hivesCount", 0, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<string>(ref this.customLetterLabel, "customLetterLabel", null, false);
			Scribe_Values.Look<string>(ref this.customLetterText, "customLetterText", null, false);
			Scribe_Defs.Look<LetterDef>(ref this.customLetterDef, "customLetterDef");
			Scribe_Values.Look<bool>(ref this.sendStandardLetter, "sendStandardLetter", true, false);
			Scribe_Values.Look<IntVec3>(ref this.loc, "loc", default(IntVec3), false);
			Scribe_Values.Look<string>(ref this.tag, "tag", null, false);
			Scribe_Values.Look<IntVec3?>(ref this.overrideLoc, "overrideLoc", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && !this.quest.Historical && (this.mapParent == null || !this.mapParent.HasMap))
			{
				Log.Warning(string.Format("Quest {0} loaded with quest part {1} with invalid map and no fallback map. Ending quest.", this.quest, this));
				this.quest.End(QuestEndOutcome.Unknown, false, true);
			}
		}

		// Token: 0x0600A5DA RID: 42458 RVA: 0x002F7C54 File Offset: 0x002F5E54
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			if (Find.AnyPlayerHomeMap != null)
			{
				this.mapParent = Find.RandomPlayerHomeMap.Parent;
				this.hivesCount = 5;
			}
		}

		// Token: 0x0400728E RID: 29326
		public string inSignal;

		// Token: 0x0400728F RID: 29327
		public int hivesCount;

		// Token: 0x04007290 RID: 29328
		public MapParent mapParent;

		// Token: 0x04007291 RID: 29329
		public string tag;

		// Token: 0x04007292 RID: 29330
		public IntVec3? overrideLoc;

		// Token: 0x04007293 RID: 29331
		public string customLetterText;

		// Token: 0x04007294 RID: 29332
		public string customLetterLabel;

		// Token: 0x04007295 RID: 29333
		public LetterDef customLetterDef;

		// Token: 0x04007296 RID: 29334
		public bool sendStandardLetter = true;

		// Token: 0x04007297 RID: 29335
		private IntVec3 loc = IntVec3.Invalid;
	}
}
