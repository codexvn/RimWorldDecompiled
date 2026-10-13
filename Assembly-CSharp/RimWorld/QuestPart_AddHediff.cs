using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C03 RID: 7171
	public class QuestPart_AddHediff : QuestPart
	{
		// Token: 0x17001887 RID: 6279
		// (get) Token: 0x06009FAC RID: 40876 RVA: 0x002E13AB File Offset: 0x002DF5AB
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				int num;
				for (int i = 0; i < this.pawns.Count; i = num + 1)
				{
					yield return this.pawns[i];
					num = i;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17001888 RID: 6280
		// (get) Token: 0x06009FAD RID: 40877 RVA: 0x002E13BB File Offset: 0x002DF5BB
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				foreach (Dialog_InfoCard.Hyperlink hyperlink in base.Hyperlinks)
				{
					yield return hyperlink;
				}
				IEnumerator<Dialog_InfoCard.Hyperlink> enumerator = null;
				if (this.addToHyperlinks)
				{
					yield return new Dialog_InfoCard.Hyperlink(this.hediffDef, -1);
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06009FAE RID: 40878 RVA: 0x002E13CC File Offset: 0x002DF5CC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (!this.pawns[i].DestroyedOrNull() && (!this.checkDiseaseContractChance || Rand.Chance(this.pawns[i].health.immunity.DiseaseContractChanceFactor(this.hediffDef, null))))
					{
						HediffGiverUtility.TryApply(this.pawns[i], this.hediffDef, this.partsToAffect, false, 1, null, true);
					}
				}
			}
		}

		// Token: 0x06009FAF RID: 40879 RVA: 0x002E1470 File Offset: 0x002DF670
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<BodyPartDef>(ref this.partsToAffect, "partsToAffect", LookMode.Def, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Defs.Look<HediffDef>(ref this.hediffDef, "hediffDef");
			Scribe_Values.Look<bool>(ref this.checkDiseaseContractChance, "checkDiseaseContractChance", false, false);
			Scribe_Values.Look<bool>(ref this.addToHyperlinks, "addToHyperlinks", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06009FB0 RID: 40880 RVA: 0x002E1528 File Offset: 0x002DF728
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			this.hediffDef = HediffDefOf.Anesthetic;
			this.pawns.Add(PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>());
		}

		// Token: 0x06009FB1 RID: 40881 RVA: 0x002E1578 File Offset: 0x002DF778
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x04006E70 RID: 28272
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04006E71 RID: 28273
		public List<BodyPartDef> partsToAffect;

		// Token: 0x04006E72 RID: 28274
		public string inSignal;

		// Token: 0x04006E73 RID: 28275
		public HediffDef hediffDef;

		// Token: 0x04006E74 RID: 28276
		public bool checkDiseaseContractChance;

		// Token: 0x04006E75 RID: 28277
		public bool addToHyperlinks;
	}
}
