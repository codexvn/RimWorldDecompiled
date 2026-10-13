using System;
using System.Linq;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CBC RID: 7356
	public class QuestPart_SubquestGenerator_ArchonexusVictory : QuestPart_SubquestGenerator
	{
		// Token: 0x0600A3E4 RID: 41956 RVA: 0x002F027F File Offset: 0x002EE47F
		protected override Slate InitSlate()
		{
			Slate slate = new Slate();
			slate.Set<Faction>("civilOutlander", this.civilOutlander, false);
			slate.Set<Faction>("roughTribe", this.roughTribe, false);
			slate.Set<Faction>("roughOutlander", this.roughOutlander, false);
			return slate;
		}

		// Token: 0x0600A3E5 RID: 41957 RVA: 0x002F02BC File Offset: 0x002EE4BC
		protected override QuestScriptDef GetNextSubquestDef()
		{
			int index = this.quest.GetSubquests(new QuestState?(QuestState.EndedSuccess)).Count<Quest>() % this.subquestDefs.Count;
			QuestScriptDef questScriptDef = this.subquestDefs[index];
			if (!questScriptDef.CanRun(this.InitSlate(), Find.World))
			{
				return null;
			}
			return questScriptDef;
		}

		// Token: 0x0600A3E6 RID: 41958 RVA: 0x002F030F File Offset: 0x002EE50F
		public override void Notify_FactionRemoved(Faction faction)
		{
			if (this.civilOutlander == faction)
			{
				this.civilOutlander = null;
			}
			if (this.roughTribe == faction)
			{
				this.roughTribe = null;
			}
			if (this.roughOutlander == faction)
			{
				this.roughOutlander = null;
			}
		}

		// Token: 0x0600A3E7 RID: 41959 RVA: 0x002F0341 File Offset: 0x002EE541
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.civilOutlander, "civilOutlander", false);
			Scribe_References.Look<Faction>(ref this.roughTribe, "roughTribe", false);
			Scribe_References.Look<Faction>(ref this.roughOutlander, "roughOutlander", false);
		}

		// Token: 0x04007134 RID: 28980
		public Faction civilOutlander;

		// Token: 0x04007135 RID: 28981
		public Faction roughTribe;

		// Token: 0x04007136 RID: 28982
		public Faction roughOutlander;
	}
}
