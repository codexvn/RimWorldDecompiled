using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CED RID: 7405
	public class QuestPart_GiveToCaravan : QuestPart
	{
		// Token: 0x17001980 RID: 6528
		// (get) Token: 0x0600A54F RID: 42319 RVA: 0x002F57B9 File Offset: 0x002F39B9
		// (set) Token: 0x0600A550 RID: 42320 RVA: 0x002F57D4 File Offset: 0x002F39D4
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

		// Token: 0x17001981 RID: 6529
		// (get) Token: 0x0600A551 RID: 42321 RVA: 0x002F5854 File Offset: 0x002F3A54
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.caravan != null)
				{
					yield return this.caravan;
				}
				foreach (Pawn t in PawnsArriveQuestPartUtility.GetQuestLookTargets(this.pawns))
				{
					yield return t;
				}
				IEnumerator<Pawn> enumerator2 = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17001982 RID: 6530
		// (get) Token: 0x0600A552 RID: 42322 RVA: 0x002F5864 File Offset: 0x002F3A64
		public override bool IncreasesPopulation
		{
			get
			{
				return PawnsArriveQuestPartUtility.IncreasesPopulation(this.pawns, true, false);
			}
		}

		// Token: 0x0600A553 RID: 42323 RVA: 0x002F5874 File Offset: 0x002F3A74
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				this.pawns.RemoveAll((Pawn x) => x.Destroyed);
				Caravan caravan = this.caravan;
				if (caravan == null)
				{
					signal.args.TryGetArg<Caravan>("CARAVAN", out caravan);
				}
				if (caravan != null && this.Things.Any<Thing>())
				{
					for (int i = 0; i < this.pawns.Count; i++)
					{
						if (this.pawns[i].Faction != Faction.OfPlayer)
						{
							this.pawns[i].SetFaction(Faction.OfPlayer, null);
						}
						caravan.AddPawn(this.pawns[i], true);
					}
					for (int j = 0; j < this.items.Count; j++)
					{
						CaravanInventoryUtility.GiveThing(caravan, this.items[j]);
					}
					this.items.Clear();
				}
			}
		}

		// Token: 0x0600A554 RID: 42324 RVA: 0x002F5988 File Offset: 0x002F3B88
		public override void PostQuestAdded()
		{
			base.PostQuestAdded();
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i].def == ThingDefOf.PsychicAmplifier)
				{
					Find.History.Notify_PsylinkAvailable();
					return;
				}
			}
		}

		// Token: 0x0600A555 RID: 42325 RVA: 0x002F59D4 File Offset: 0x002F3BD4
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x0600A556 RID: 42326 RVA: 0x002F59E4 File Offset: 0x002F3BE4
		public override void Cleanup()
		{
			base.Cleanup();
			for (int i = 0; i < this.items.Count; i++)
			{
				this.items[i].Destroy(DestroyMode.Vanish);
			}
			this.items.Clear();
		}

		// Token: 0x0600A557 RID: 42327 RVA: 0x002F5A2C File Offset: 0x002F3C2C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Caravan>(ref this.caravan, "caravan", false);
			Scribe_Collections.Look<Thing>(ref this.items, "items", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.items.RemoveAll((Thing x) => x == null);
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0600A558 RID: 42328 RVA: 0x002F5AEC File Offset: 0x002F3CEC
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
		}

		// Token: 0x0600A559 RID: 42329 RVA: 0x002F5B1C File Offset: 0x002F3D1C
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x04007237 RID: 29239
		public string inSignal;

		// Token: 0x04007238 RID: 29240
		public Caravan caravan;

		// Token: 0x04007239 RID: 29241
		private List<Thing> items = new List<Thing>();

		// Token: 0x0400723A RID: 29242
		private List<Pawn> pawns = new List<Pawn>();
	}
}
