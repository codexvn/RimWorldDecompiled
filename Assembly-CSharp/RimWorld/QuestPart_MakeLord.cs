using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001BF6 RID: 7158
	public abstract class QuestPart_MakeLord : QuestPart
	{
		// Token: 0x1700187C RID: 6268
		// (get) Token: 0x06009F5C RID: 40796 RVA: 0x002E029B File Offset: 0x002DE49B
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.excludeFromLookTargets)
				{
					yield break;
				}
				int num;
				for (int i = 0; i < this.pawns.Count; i = num + 1)
				{
					yield return this.pawns[i];
					num = i;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x1700187D RID: 6269
		// (get) Token: 0x06009F5D RID: 40797 RVA: 0x002E02AB File Offset: 0x002DE4AB
		public Map Map
		{
			get
			{
				if (this.mapOfPawn != null)
				{
					return this.mapOfPawn.Map;
				}
				if (this.mapParent != null)
				{
					return this.mapParent.Map;
				}
				return null;
			}
		}

		// Token: 0x06009F5E RID: 40798
		protected abstract Lord MakeLord();

		// Token: 0x06009F5F RID: 40799 RVA: 0x002E02D8 File Offset: 0x002DE4D8
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				if (this.Map == null)
				{
					this.mapOfPawn = null;
					Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
					this.mapParent = ((anyPlayerHomeMap != null) ? anyPlayerHomeMap.Parent : null);
				}
				if (this.Map != null)
				{
					this.pawns.RemoveAll((Pawn x) => x.MapHeld != this.Map);
					for (int i = 0; i < this.pawns.Count; i++)
					{
						Lord lord = this.pawns[i].GetLord();
						if (lord != null)
						{
							lord.Notify_PawnLost(this.pawns[i], PawnLostCondition.ForcedByQuest, null);
						}
					}
					Lord lord2 = this.MakeLord();
					for (int j = 0; j < this.pawns.Count; j++)
					{
						if (!this.pawns[j].Dead)
						{
							lord2.AddPawn(this.pawns[j]);
						}
					}
				}
			}
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				this.pawns.Remove(item);
			}
		}

		// Token: 0x06009F60 RID: 40800 RVA: 0x002E0420 File Offset: 0x002DE620
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.faction == f)
			{
				this.faction = null;
			}
		}

		// Token: 0x06009F61 RID: 40801 RVA: 0x002E0434 File Offset: 0x002DE634
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.inSignalRemovePawn, "inSignalRemovePawn", null, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_References.Look<Pawn>(ref this.mapOfPawn, "mapOfPawn", false);
			Scribe_Values.Look<bool>(ref this.excludeFromLookTargets, "excludeFromLookTargets", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06009F62 RID: 40802 RVA: 0x002E04FC File Offset: 0x002DE6FC
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			this.pawns.Add(PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>());
			this.mapParent = this.pawns[0].Map.Parent;
		}

		// Token: 0x04006E37 RID: 28215
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04006E38 RID: 28216
		public string inSignal;

		// Token: 0x04006E39 RID: 28217
		public Faction faction;

		// Token: 0x04006E3A RID: 28218
		public MapParent mapParent;

		// Token: 0x04006E3B RID: 28219
		public string inSignalRemovePawn;

		// Token: 0x04006E3C RID: 28220
		public Pawn mapOfPawn;

		// Token: 0x04006E3D RID: 28221
		public bool excludeFromLookTargets;
	}
}
