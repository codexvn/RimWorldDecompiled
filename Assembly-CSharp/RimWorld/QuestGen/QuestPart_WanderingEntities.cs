using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BB2 RID: 15282
	public class QuestPart_WanderingEntities : QuestPartActivable
	{
		// Token: 0x17003375 RID: 13173
		// (get) Token: 0x06014C07 RID: 84999 RVA: 0x00637DFC File Offset: 0x00635FFC
		private float MaxPoints
		{
			get
			{
				return QuestPart_WanderingEntities.MaxPointsByCurrentThreatPoints.Evaluate(StorytellerUtility.DefaultThreatPointsNow(this.map)) * QuestPart_WanderingEntities.MaxPointsFactorByDaysPassed.Evaluate((Find.TickManager.TicksGame - this.waveStartedTick).TicksToDays());
			}
		}

		// Token: 0x17003376 RID: 13174
		// (get) Token: 0x06014C08 RID: 85000 RVA: 0x00637E34 File Offset: 0x00636034
		private float CurPoints
		{
			get
			{
				return this.pawns.Sum((Pawn p) => p.kindDef.combatPower);
			}
		}

		// Token: 0x17003377 RID: 13175
		// (get) Token: 0x06014C09 RID: 85001 RVA: 0x00637E60 File Offset: 0x00636060
		private IEnumerable<PawnGroupKindDef> GroupKinds
		{
			get
			{
				yield return PawnGroupKindDefOf.Gorehulks;
				yield return PawnGroupKindDefOf.Noctols;
				yield break;
			}
		}

		// Token: 0x06014C0A RID: 85002 RVA: 0x00637E69 File Offset: 0x00636069
		public QuestPart_WanderingEntities()
		{
		}

		// Token: 0x06014C0B RID: 85003 RVA: 0x00637E87 File Offset: 0x00636087
		public QuestPart_WanderingEntities(Map map, string waveStartedSignal)
		{
			this.map = map;
			this.waveStartedSignal = waveStartedSignal;
			this.inSignalEnable = waveStartedSignal;
		}

		// Token: 0x06014C0C RID: 85004 RVA: 0x00637EBC File Offset: 0x006360BC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Map>(ref this.map, "map", false);
			Scribe_Values.Look<string>(ref this.waveStartedSignal, "waveStartedSignal", null, false);
			Scribe_Values.Look<int>(ref this.waveStartedTick, "waveStartedTick", 0, false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.pawns == null)
				{
					this.pawns = new List<Pawn>();
				}
				this.pawns.RemoveAll((Pawn p) => p == null);
			}
		}

		// Token: 0x06014C0D RID: 85005 RVA: 0x00637F60 File Offset: 0x00636160
		public override void Cleanup()
		{
			base.Cleanup();
			this.map = null;
		}

		// Token: 0x06014C0E RID: 85006 RVA: 0x00637F6F File Offset: 0x0063616F
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag != this.waveStartedSignal)
			{
				return;
			}
			this.waveStartedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06014C0F RID: 85007 RVA: 0x00637F9C File Offset: 0x0063619C
		public override void QuestPartTick()
		{
			if (Find.TickManager.TicksGame % 2500 != 0)
			{
				return;
			}
			this.pawns = (from p in this.pawns
			where !p.Dead && p.Spawned
			select p).ToList<Pawn>();
			if (this.CurPoints < this.MaxPoints)
			{
				this.SpawnEntities();
			}
		}

		// Token: 0x06014C10 RID: 85008 RVA: 0x00638008 File Offset: 0x00636208
		private void SpawnEntities()
		{
			PawnGroupKindDef groupKind = this.GroupKinds.RandomElement<PawnGroupKindDef>();
			float num = this.MaxPoints * 0.041666668f;
			num = Mathf.Max(num, Faction.OfEntities.def.MinPointsToGeneratePawnGroup(groupKind, null) * 1.05f);
			IncidentParms incidentParms = new IncidentParms
			{
				target = this.map,
				raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn,
				sendLetter = false,
				faction = Faction.OfEntities
			};
			if (!incidentParms.raidArrivalMode.Worker.TryResolveRaidSpawnCenter(incidentParms))
			{
				return;
			}
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			{
				groupKind = groupKind,
				points = num,
				faction = Faction.OfEntities
			}, true).ToList<Pawn>();
			PawnsArrivalModeDefOf.EdgeWalkInDistributedGroups.Worker.Arrive(list, incidentParms);
			LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_VoidAwakeningWander(), this.map, list);
			this.pawns.AddRange(list);
		}

		// Token: 0x0400E70C RID: 59148
		private const int SpawnCheckInterval = 2500;

		// Token: 0x0400E70D RID: 59149
		private const float MaxPointFractionToSpawnPerInterval = 0.041666668f;

		// Token: 0x0400E70E RID: 59150
		private static readonly SimpleCurve MaxPointsByCurrentThreatPoints = new SimpleCurve
		{
			{
				new CurvePoint(500f, 750f),
				true
			},
			{
				new CurvePoint(2500f, 1500f),
				true
			},
			{
				new CurvePoint(5000f, 2000f),
				true
			},
			{
				new CurvePoint(10000f, 3000f),
				true
			}
		};

		// Token: 0x0400E70F RID: 59151
		private static readonly SimpleCurve MaxPointsFactorByDaysPassed = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(5f, 0.2f),
				true
			}
		};

		// Token: 0x0400E710 RID: 59152
		private Map map;

		// Token: 0x0400E711 RID: 59153
		private string waveStartedSignal;

		// Token: 0x0400E712 RID: 59154
		private int waveStartedTick = -99999;

		// Token: 0x0400E713 RID: 59155
		private List<Pawn> pawns = new List<Pawn>();
	}
}
