using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C74 RID: 7284
	public class QuestPart_PawnsKilled : QuestPartActivable
	{
		// Token: 0x170018F1 RID: 6385
		// (get) Token: 0x0600A253 RID: 41555 RVA: 0x002EAA0C File Offset: 0x002E8C0C
		public override string DescriptionPart
		{
			get
			{
				return "PawnsKilled".Translate(GenLabel.BestKindLabel(this.race.race.AnyPawnKind, Gender.None, true, -1)).CapitalizeFirst() + ": " + this.killed.ToString() + " / " + this.count.ToString();
			}
		}

		// Token: 0x170018F2 RID: 6386
		// (get) Token: 0x0600A254 RID: 41556 RVA: 0x002EAA77 File Offset: 0x002E8C77
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in base.InvolvedFactions)
				{
					yield return faction;
				}
				IEnumerator<Faction> enumerator = null;
				if (this.requiredInstigatorFaction != null)
				{
					yield return this.requiredInstigatorFaction;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x0600A255 RID: 41557 RVA: 0x002EAA87 File Offset: 0x002E8C87
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.killed = 0;
		}

		// Token: 0x0600A256 RID: 41558 RVA: 0x002EAA98 File Offset: 0x002E8C98
		public override void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
			base.Notify_PawnKilled(pawn, dinfo);
			if (base.State == QuestPartState.Enabled && pawn.def == this.race && (this.requiredInstigatorFaction == null || (dinfo != null && (dinfo.Value.Instigator == null || dinfo.Value.Instigator.Faction == this.requiredInstigatorFaction))))
			{
				this.killed++;
				Find.SignalManager.SendSignal(new Signal(this.outSignalPawnKilled, false));
				if (this.killed >= this.count)
				{
					base.Complete();
				}
			}
		}

		// Token: 0x0600A257 RID: 41559 RVA: 0x002EAB40 File Offset: 0x002E8D40
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.race, "race");
			Scribe_References.Look<Faction>(ref this.requiredInstigatorFaction, "requiredInstigatorFaction", false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<int>(ref this.count, "count", 0, false);
			Scribe_Values.Look<int>(ref this.killed, "killed", 0, false);
			Scribe_Values.Look<string>(ref this.outSignalPawnKilled, "outSignalPawnKilled", null, false);
		}

		// Token: 0x0600A258 RID: 41560 RVA: 0x002EABBB File Offset: 0x002E8DBB
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.race = ThingDefOf.Muffalo;
			this.requiredInstigatorFaction = Faction.OfPlayer;
			this.count = 10;
		}

		// Token: 0x0400702D RID: 28717
		public ThingDef race;

		// Token: 0x0400702E RID: 28718
		public Faction requiredInstigatorFaction;

		// Token: 0x0400702F RID: 28719
		public int count;

		// Token: 0x04007030 RID: 28720
		public MapParent mapParent;

		// Token: 0x04007031 RID: 28721
		public string outSignalPawnKilled;

		// Token: 0x04007032 RID: 28722
		private int killed;
	}
}
