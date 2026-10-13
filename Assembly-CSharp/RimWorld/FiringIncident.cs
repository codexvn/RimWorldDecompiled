using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D57 RID: 7511
	public class FiringIncident : IExposable
	{
		// Token: 0x0600A85D RID: 43101 RVA: 0x00302F99 File Offset: 0x00301199
		public FiringIncident()
		{
		}

		// Token: 0x0600A85E RID: 43102 RVA: 0x00302FAC File Offset: 0x003011AC
		public FiringIncident(IncidentDef def, StorytellerComp source, IncidentParms parms = null)
		{
			this.def = def;
			if (parms != null)
			{
				this.parms = parms;
			}
			this.source = source;
		}

		// Token: 0x0600A85F RID: 43103 RVA: 0x00302FD7 File Offset: 0x003011D7
		public void ExposeData()
		{
			Scribe_Defs.Look<IncidentDef>(ref this.def, "def");
			Scribe_Deep.Look<IncidentParms>(ref this.parms, "parms", Array.Empty<object>());
		}

		// Token: 0x0600A860 RID: 43104 RVA: 0x00303000 File Offset: 0x00301200
		public override string ToString()
		{
			string text = this.def.defName;
			if (this.parms != null)
			{
				text = text + ", parms=(" + this.parms.ToString() + ")";
			}
			if (this.source != null)
			{
				string str = text;
				string str2 = ", source=";
				StorytellerComp storytellerComp = this.source;
				text = str + str2 + ((storytellerComp != null) ? storytellerComp.ToString() : null);
			}
			if (this.sourceQuestPart != null)
			{
				string str3 = text;
				string str4 = ", sourceQuestPart=";
				QuestPart questPart = this.sourceQuestPart;
				text = str3 + str4 + ((questPart != null) ? questPart.ToString() : null);
			}
			return text;
		}

		// Token: 0x04007470 RID: 29808
		public IncidentDef def;

		// Token: 0x04007471 RID: 29809
		public IncidentParms parms = new IncidentParms();

		// Token: 0x04007472 RID: 29810
		public StorytellerComp source;

		// Token: 0x04007473 RID: 29811
		public QuestPart sourceQuestPart;
	}
}
