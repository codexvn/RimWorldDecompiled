using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200335C RID: 13148
	public class ChoiceLetter_AcceptJoiner : ChoiceLetter
	{
		// Token: 0x1700302B RID: 12331
		// (get) Token: 0x060127AF RID: 75695 RVA: 0x00002501 File Offset: 0x00000701
		public override bool CanDismissWithRightClick
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700302C RID: 12332
		// (get) Token: 0x060127B0 RID: 75696 RVA: 0x00573C4B File Offset: 0x00571E4B
		public override bool CanShowInLetterStack
		{
			get
			{
				return base.CanShowInLetterStack && this.quest != null && (this.quest.State == QuestState.Ongoing || this.quest.State == QuestState.NotYetAccepted);
			}
		}

		// Token: 0x1700302D RID: 12333
		// (get) Token: 0x060127B1 RID: 75697 RVA: 0x0057404B File Offset: 0x0057224B
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				if (base.ArchivedOnly)
				{
					yield return base.Option_Close;
				}
				else
				{
					DiaOption diaOption = new DiaOption("AcceptButton".Translate());
					DiaOption optionReject = new DiaOption("RejectLetter".Translate());
					diaOption.action = delegate()
					{
						Find.SignalManager.SendSignal(new Signal(this.signalAccept, false));
						Find.LetterStack.RemoveLetter(this);
					};
					diaOption.resolveTree = true;
					optionReject.action = delegate()
					{
						Find.SignalManager.SendSignal(new Signal(this.signalReject, false));
						Find.LetterStack.RemoveLetter(this);
					};
					optionReject.resolveTree = true;
					if (Find.AnyPlayerHomeMap == null)
					{
						diaOption.Disable("CannotAcceptQuestNoMap".Translate());
					}
					else if (Find.AnyPlayerHomeMap.Tile.LayerDef.isSpace)
					{
						PlanetLayerDef layerDef = Find.AnyPlayerHomeMap.Tile.LayerDef;
						diaOption.Disable("CannotAcceptQuestFromLayer".Translate(layerDef.gerundLabel.Named("GERUND"), layerDef.Named("LAYER")));
					}
					yield return diaOption;
					yield return optionReject;
					if (this.lookTargets.IsValid())
					{
						yield return base.Option_JumpToLocationAndPostpone;
					}
					yield return base.Option_Postpone;
					optionReject = null;
				}
				yield break;
			}
		}

		// Token: 0x060127B2 RID: 75698 RVA: 0x0057405B File Offset: 0x0057225B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.signalAccept, "signalAccept", null, false);
			Scribe_Values.Look<string>(ref this.signalReject, "signalReject", null, false);
		}

		// Token: 0x0400C008 RID: 49160
		public string signalAccept;

		// Token: 0x0400C009 RID: 49161
		public string signalReject;
	}
}
