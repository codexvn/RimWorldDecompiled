using System;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C09 RID: 7177
	public abstract class QuestPart_AddQuest : QuestPart
	{
		// Token: 0x17001890 RID: 6288
		// (get) Token: 0x06009FDA RID: 40922
		public abstract QuestScriptDef QuestDef { get; }

		// Token: 0x17001891 RID: 6289
		// (get) Token: 0x06009FDB RID: 40923 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CanAdd
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06009FDC RID: 40924 RVA: 0x002E1C3B File Offset: 0x002DFE3B
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.CanAdd)
			{
				this.AddQuest();
				this.PostAdd();
			}
		}

		// Token: 0x06009FDD RID: 40925 RVA: 0x002E1C6C File Offset: 0x002DFE6C
		private void AddQuest()
		{
			Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(this.QuestDef, this.GetSlate());
			quest.parent = this.parent;
			quest.Accept(this.acceptee);
			if (this.sendAvailableLetter)
			{
				QuestUtility.SendLetterQuestAvailable(quest, null);
			}
		}

		// Token: 0x06009FDE RID: 40926
		public abstract Slate GetSlate();

		// Token: 0x06009FDF RID: 40927 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostAdd()
		{
		}

		// Token: 0x06009FE0 RID: 40928 RVA: 0x002E1CB4 File Offset: 0x002DFEB4
		public override void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
			Rect rect = new Rect(innerRect.x, curY, 500f, 25f);
			if (Widgets.ButtonText(rect, "AddQuest " + this.QuestDef.defName, true, true, true, null))
			{
				this.AddQuest();
			}
			curY += rect.height + 4f;
		}

		// Token: 0x06009FE1 RID: 40929 RVA: 0x002E1D1C File Offset: 0x002DFF1C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Pawn>(ref this.acceptee, "acceptee", false);
			Scribe_References.Look<Quest>(ref this.parent, "parent", false);
			Scribe_Values.Look<bool>(ref this.sendAvailableLetter, "sendAvailableLetter", false, false);
		}

		// Token: 0x04006E8D RID: 28301
		public string inSignal;

		// Token: 0x04006E8E RID: 28302
		public Pawn acceptee;

		// Token: 0x04006E8F RID: 28303
		public Quest parent;

		// Token: 0x04006E90 RID: 28304
		public bool sendAvailableLetter;
	}
}
