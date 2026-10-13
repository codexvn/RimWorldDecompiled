using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C21 RID: 7201
	public class QuestPart_DelayedRewardDropPods : QuestPart_AddQuest
	{
		// Token: 0x170018A6 RID: 6310
		// (get) Token: 0x0600A061 RID: 41057 RVA: 0x002E3763 File Offset: 0x002E1963
		public override QuestScriptDef QuestDef
		{
			get
			{
				return QuestScriptDefOf.DelayedRewardDropPods;
			}
		}

		// Token: 0x170018A7 RID: 6311
		// (get) Token: 0x0600A062 RID: 41058 RVA: 0x002E376A File Offset: 0x002E196A
		public override bool CanAdd
		{
			get
			{
				return Rand.Chance(this.chance);
			}
		}

		// Token: 0x0600A063 RID: 41059 RVA: 0x002E3778 File Offset: 0x002E1978
		public override Slate GetSlate()
		{
			Slate slate = new Slate();
			slate.Set<List<Thing>>("rewards", this.rewards, false);
			slate.Set<Faction>("faction", this.faction, false);
			slate.Set<Pawn>("giver", this.giver, false);
			slate.Set<int>("delayTicks", this.delayTicks, false);
			slate.Set<string>("customLetterLabel", this.customLetterLabel, false);
			slate.Set<string>("customLetterText", this.customLetterText, false);
			return slate;
		}

		// Token: 0x0600A064 RID: 41060 RVA: 0x002E37F6 File Offset: 0x002E19F6
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.faction == f)
			{
				this.faction = null;
			}
		}

		// Token: 0x0600A065 RID: 41061 RVA: 0x002E3808 File Offset: 0x002E1A08
		public override void PostAdd()
		{
			this.rewards.Clear();
		}

		// Token: 0x0600A066 RID: 41062 RVA: 0x002E3818 File Offset: 0x002E1A18
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.rewardThings.Clear();
				this.rewardPawns.Clear();
				foreach (Thing thing in this.rewards)
				{
					Pawn pawn = thing as Pawn;
					if (pawn != null)
					{
						this.rewardPawns.Add(pawn);
					}
					else
					{
						this.rewardThings.Add(thing);
					}
				}
			}
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_References.Look<Pawn>(ref this.giver, "giver", false);
			Scribe_Values.Look<int>(ref this.delayTicks, "delayTicks", 0, false);
			Scribe_Values.Look<string>(ref this.customLetterLabel, "customLetterLabel", null, false);
			Scribe_Values.Look<string>(ref this.customLetterText, "customLetterText", null, false);
			Scribe_Values.Look<float>(ref this.chance, "chance", 0f, false);
			Scribe_Collections.Look<Pawn>(ref this.rewardPawns, "rewardPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.rewardThings, "rewardThings", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.rewardPawns.RemoveAll((Pawn x) => x == null);
				foreach (Pawn item in this.rewardPawns)
				{
					this.rewards.Add(item);
				}
				foreach (Thing item2 in this.rewardThings)
				{
					this.rewards.Add(item2);
				}
				this.rewardThings.Clear();
				this.rewardPawns.Clear();
			}
		}

		// Token: 0x04006EE2 RID: 28386
		public List<Thing> rewards = new List<Thing>();

		// Token: 0x04006EE3 RID: 28387
		public int delayTicks;

		// Token: 0x04006EE4 RID: 28388
		public string customLetterLabel;

		// Token: 0x04006EE5 RID: 28389
		public string customLetterText;

		// Token: 0x04006EE6 RID: 28390
		public Faction faction;

		// Token: 0x04006EE7 RID: 28391
		public Pawn giver;

		// Token: 0x04006EE8 RID: 28392
		public float chance = 1f;

		// Token: 0x04006EE9 RID: 28393
		private List<Thing> rewardThings = new List<Thing>();

		// Token: 0x04006EEA RID: 28394
		private List<Pawn> rewardPawns = new List<Pawn>();
	}
}
