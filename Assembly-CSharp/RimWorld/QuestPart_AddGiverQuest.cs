using System;
using System.Linq;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C0A RID: 7178
	public class QuestPart_AddGiverQuest : QuestPart_AddQuest
	{
		// Token: 0x17001892 RID: 6290
		// (get) Token: 0x06009FE3 RID: 40931 RVA: 0x002E1D75 File Offset: 0x002DFF75
		public override QuestScriptDef QuestDef
		{
			get
			{
				return this.questScript;
			}
		}

		// Token: 0x06009FE4 RID: 40932 RVA: 0x002E1D80 File Offset: 0x002DFF80
		public override Slate GetSlate()
		{
			Slate slate = new Slate();
			NamedArgument[] args = (from arg in this.receivedSignal.args.Args
			select arg.arg.Named(arg.label)).ToArray<NamedArgument>();
			slate.Set<float>("points", this.points, false);
			slate.Set<TaggedString>("discoveryMethod", this.discoveryMethodTranslationKey.Translate(args), false);
			return slate;
		}

		// Token: 0x06009FE5 RID: 40933 RVA: 0x002E1DF6 File Offset: 0x002DFFF6
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.receivedSignal = signal;
			base.Notify_QuestSignalReceived(signal);
		}

		// Token: 0x06009FE6 RID: 40934 RVA: 0x002E1E0E File Offset: 0x002E000E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<QuestScriptDef>(ref this.questScript, "questScript");
			Scribe_Values.Look<string>(ref this.discoveryMethodTranslationKey, "discoveryMethodTranslationKey", null, false);
			Scribe_Values.Look<float>(ref this.points, "points", 0f, false);
		}

		// Token: 0x04006E91 RID: 28305
		public QuestScriptDef questScript;

		// Token: 0x04006E92 RID: 28306
		public string discoveryMethodTranslationKey;

		// Token: 0x04006E93 RID: 28307
		public float points;

		// Token: 0x04006E94 RID: 28308
		private Signal receivedSignal;
	}
}
