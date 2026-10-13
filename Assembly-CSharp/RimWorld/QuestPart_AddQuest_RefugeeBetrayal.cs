using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C0E RID: 7182
	public class QuestPart_AddQuest_RefugeeBetrayal : QuestPart_AddQuest
	{
		// Token: 0x17001894 RID: 6292
		// (get) Token: 0x06009FF4 RID: 40948 RVA: 0x002E2044 File Offset: 0x002E0244
		public override QuestScriptDef QuestDef
		{
			get
			{
				return QuestScriptDefOf.RefugeeBetrayal;
			}
		}

		// Token: 0x06009FF5 RID: 40949 RVA: 0x002E204C File Offset: 0x002E024C
		public override Slate GetSlate()
		{
			Slate slate = new Slate();
			slate.Set<List<Pawn>>("lodgers", this.lodgers, false);
			slate.Set<ExtraFaction>("refugeeFaction", this.refugeeFaction, false);
			slate.Set<Pawn>("factionOpponent", this.factionOpponent, false);
			slate.Set<Pawn>("asker", this.asker, false);
			slate.Set<int>("lodgerCount", this.lodgers.Count, false);
			slate.Set<Faction>("faction", this.refugeeFaction.faction, false);
			slate.Set<Map>("map", this.mapParent.Map, false);
			return slate;
		}

		// Token: 0x06009FF6 RID: 40950 RVA: 0x002E20EB File Offset: 0x002E02EB
		public override void Notify_FactionRemoved(Faction f)
		{
			ExtraFaction extraFaction = this.refugeeFaction;
			if (((extraFaction != null) ? extraFaction.faction : null) == f)
			{
				this.refugeeFaction = null;
			}
		}

		// Token: 0x06009FF7 RID: 40951 RVA: 0x002E210C File Offset: 0x002E030C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.lodgers.Contains(item))
			{
				this.lodgers.Remove(item);
			}
		}

		// Token: 0x06009FF8 RID: 40952 RVA: 0x002E2164 File Offset: 0x002E0364
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ExtraFaction>(ref this.refugeeFaction, "refugeeFaction", Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.lodgers, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inSignalRemovePawn, "inSignalRemovePawn", null, false);
			Scribe_References.Look<Pawn>(ref this.asker, "asker", false);
			Scribe_References.Look<Pawn>(ref this.factionOpponent, "factionOpponent", false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.lodgers.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x04006E9D RID: 28317
		public List<Pawn> lodgers = new List<Pawn>();

		// Token: 0x04006E9E RID: 28318
		public Pawn factionOpponent;

		// Token: 0x04006E9F RID: 28319
		public Pawn asker;

		// Token: 0x04006EA0 RID: 28320
		public ExtraFaction refugeeFaction;

		// Token: 0x04006EA1 RID: 28321
		public MapParent mapParent;

		// Token: 0x04006EA2 RID: 28322
		public string inSignalRemovePawn;
	}
}
