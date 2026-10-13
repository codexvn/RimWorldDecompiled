using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BFF RID: 7167
	public class QuestPart_AddContentsToShuttle : QuestPart
	{
		// Token: 0x17001880 RID: 6272
		// (get) Token: 0x06009F85 RID: 40837 RVA: 0x002E0B9E File Offset: 0x002DED9E
		// (set) Token: 0x06009F86 RID: 40838 RVA: 0x002E0BB8 File Offset: 0x002DEDB8
		public IEnumerable<Thing> Things
		{
			get
			{
				return this.items.Concat(this.pawns.Cast<Thing>());
			}
			set
			{
				this.items.Clear();
				this.pawns.Clear();
				if (value != null)
				{
					foreach (Thing thing in value)
					{
						if (thing.Destroyed)
						{
							Log.Error("Tried to add a destroyed thing to QuestPart_AddContentsToShuttle: " + thing.ToStringSafe<Thing>());
						}
						else
						{
							Pawn pawn = thing as Pawn;
							if (pawn != null)
							{
								this.pawns.Add(pawn);
							}
							else
							{
								this.items.Add(thing);
							}
						}
					}
				}
			}
		}

		// Token: 0x17001881 RID: 6273
		// (get) Token: 0x06009F87 RID: 40839 RVA: 0x002E0C58 File Offset: 0x002DEE58
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				foreach (Dialog_InfoCard.Hyperlink hyperlink in base.Hyperlinks)
				{
					yield return hyperlink;
				}
				IEnumerator<Dialog_InfoCard.Hyperlink> enumerator = null;
				foreach (Thing outerThing in this.items)
				{
					ThingDef def = outerThing.GetInnerIfMinified().def;
					yield return new Dialog_InfoCard.Hyperlink(def, -1);
				}
				List<Thing>.Enumerator enumerator2 = default(List<Thing>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x17001882 RID: 6274
		// (get) Token: 0x06009F88 RID: 40840 RVA: 0x002E0C68 File Offset: 0x002DEE68
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				foreach (Pawn t in PawnsArriveQuestPartUtility.GetQuestLookTargets(this.pawns))
				{
					yield return t;
				}
				IEnumerator<Pawn> enumerator2 = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x06009F89 RID: 40841 RVA: 0x002E0C78 File Offset: 0x002DEE78
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.shuttle != null)
			{
				this.pawns.RemoveAll((Pawn x) => x.Destroyed);
				this.items.RemoveAll((Thing x) => x.Destroyed);
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.pawns[i].IsWorldPawn())
					{
						Find.WorldPawns.RemovePawn(this.pawns[i]);
					}
				}
				CompTransporter compTransporter = this.shuttle.TryGetComp<CompTransporter>();
				compTransporter.innerContainer.TryAddRangeOrTransfer(this.pawns, true, false);
				compTransporter.innerContainer.TryAddRangeOrTransfer(this.items, true, false);
				this.items.Clear();
			}
		}

		// Token: 0x06009F8A RID: 40842 RVA: 0x002E0D7D File Offset: 0x002DEF7D
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x06009F8B RID: 40843 RVA: 0x002E0D8B File Offset: 0x002DEF8B
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x06009F8C RID: 40844 RVA: 0x002E0D9C File Offset: 0x002DEF9C
		public override void Cleanup()
		{
			base.Cleanup();
			for (int i = 0; i < this.items.Count; i++)
			{
				if (!this.items[i].Destroyed)
				{
					this.items[i].Destroy(DestroyMode.Vanish);
				}
			}
			this.items.Clear();
		}

		// Token: 0x06009F8D RID: 40845 RVA: 0x002E0DF8 File Offset: 0x002DEFF8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<Thing>(ref this.items, "items", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.items.RemoveAll((Thing x) => x == null);
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06009F8E RID: 40846 RVA: 0x002E0EB8 File Offset: 0x002DF0B8
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
		}

		// Token: 0x04006E5B RID: 28251
		public string inSignal;

		// Token: 0x04006E5C RID: 28252
		public Thing shuttle;

		// Token: 0x04006E5D RID: 28253
		private List<Thing> items = new List<Thing>();

		// Token: 0x04006E5E RID: 28254
		private List<Pawn> pawns = new List<Pawn>();
	}
}
