using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001E99 RID: 7833
	public class DeferredSpawner : IExposable
	{
		// Token: 0x0600ADDF RID: 44511 RVA: 0x003195F9 File Offset: 0x003177F9
		public DeferredSpawner(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600ADE0 RID: 44512 RVA: 0x00319614 File Offset: 0x00317814
		public void DeferredSpawnerTick()
		{
			this.requests.RemoveWhere((SpawnRequest r) => r.done);
			foreach (SpawnRequest request in this.requests)
			{
				this.TickRequest(request);
			}
		}

		// Token: 0x0600ADE1 RID: 44513 RVA: 0x00319694 File Offset: 0x00317894
		public void AddRequest(SpawnRequest request, bool spawnFirstBatchImmediately = true)
		{
			if (request.spawnPositions.Count != request.unspawnedThings.Count)
			{
				Log.Error("Spawn positions count does not match things to spawn count!");
				return;
			}
			this.requests.Add(request);
			if (spawnFirstBatchImmediately)
			{
				this.SpawnBatch(request, request.unspawnedThings.Count);
			}
		}

		// Token: 0x0600ADE2 RID: 44514 RVA: 0x003196E8 File Offset: 0x003178E8
		public SpawnRequest GetRequestByLord(Lord lord)
		{
			return this.requests.FirstOrDefault((SpawnRequest r) => r.lord == lord);
		}

		// Token: 0x0600ADE3 RID: 44515 RVA: 0x0031971C File Offset: 0x0031791C
		private void TickRequest(SpawnRequest request)
		{
			int count = request.unspawnedThings.Count;
			if (request.preSpawnEffect != null && Find.TickManager.TicksGame >= request.PreEffecterTick && this.map.IsHashIntervalTick(request.intervalTicks, request.preSpawnEffecterOffsetTicks) && count > 0)
			{
				this.SpawnPreSpawnEffecter(request, count);
			}
			if (Find.TickManager.TicksGame < request.startedTick + request.initialDelay)
			{
				return;
			}
			if (this.map.IsHashIntervalTick(request.intervalTicks) && count > 0)
			{
				this.SpawnBatch(request, count);
			}
		}

		// Token: 0x0600ADE4 RID: 44516 RVA: 0x003197B0 File Offset: 0x003179B0
		private void SpawnPreSpawnEffecter(SpawnRequest request, int spawnsLeft)
		{
			for (int i = Mathf.Min(request.batchSize, spawnsLeft) - 1; i >= 0; i--)
			{
				IntVec3 target = request.spawnPositions[i];
				EffecterDef preSpawnEffect = request.preSpawnEffect;
				if (preSpawnEffect != null)
				{
					preSpawnEffect.SpawnMaintained(target, this.map, 1f);
				}
			}
		}

		// Token: 0x0600ADE5 RID: 44517 RVA: 0x00319804 File Offset: 0x00317A04
		private void SpawnBatch(SpawnRequest request, int spawnsLeft)
		{
			for (int i = Mathf.Min(request.batchSize, spawnsLeft) - 1; i >= 0; i--)
			{
				Thing thing = request.unspawnedThings[i];
				IntVec3 loc = request.spawnPositions[i];
				Thing thing2 = GenSpawn.Spawn(thing, loc, this.map, Rot4.Random, WipeMode.Vanish, false, false);
				request.unspawnedThings.RemoveAt(i);
				request.spawnPositions.RemoveAt(i);
				request.spawnedThings.Add(thing);
				Pawn pawn;
				if ((pawn = (thing2 as Pawn)) == null)
				{
					IThingHolder thingHolder = thing2 as IThingHolder;
					pawn = ((thingHolder != null && thingHolder.GetDirectlyHeldThings().Count > 0) ? (thingHolder.GetDirectlyHeldThings()[0] as Pawn) : null);
				}
				Pawn pawn2 = pawn;
				if (pawn2 != null)
				{
					if (request.lord != null)
					{
						Lord lord = pawn2.GetLord();
						if (lord != null)
						{
							lord.RemovePawn(pawn2);
						}
						Lord lord2 = request.lord;
						if (lord2 != null)
						{
							lord2.AddPawn(pawn2);
						}
					}
					Pawn_RotationTracker rotationTracker = pawn2.rotationTracker;
					if (rotationTracker != null)
					{
						rotationTracker.FaceCell(pawn2.Map.Center);
					}
				}
				DeferredSpawnWorker spawnWorker = request.spawnWorker;
				if (spawnWorker != null)
				{
					spawnWorker.OnSpawn(thing2);
				}
				SoundDef spawnSound = request.spawnSound;
				if (spawnSound != null)
				{
					spawnSound.PlayOneShot(thing2);
				}
				EffecterDef spawnEffect = request.spawnEffect;
				if (spawnEffect != null)
				{
					spawnEffect.SpawnMaintained(thing2, thing2.Map, 1f);
				}
			}
			if (request.unspawnedThings.Count == 0)
			{
				request.done = true;
			}
		}

		// Token: 0x0600ADE6 RID: 44518 RVA: 0x0031996C File Offset: 0x00317B6C
		public void Notify_MapRemoved()
		{
			foreach (SpawnRequest spawnRequest in this.requests)
			{
				foreach (Thing thing in spawnRequest.unspawnedThings)
				{
					Pawn pawn = thing as Pawn;
					if (pawn != null)
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					}
					IThingHolder thingHolder = thing as IThingHolder;
					ThingOwner thingOwner = (thingHolder != null) ? thingHolder.GetDirectlyHeldThings() : null;
					if (thingOwner != null && thingOwner.Count > 0)
					{
						Pawn pawn2 = thingOwner[0] as Pawn;
						if (pawn2 != null)
						{
							Find.WorldPawns.PassToWorld(pawn2, PawnDiscardDecideMode.Decide);
						}
					}
				}
			}
		}

		// Token: 0x0600ADE7 RID: 44519 RVA: 0x00319A4C File Offset: 0x00317C4C
		public void ExposeData()
		{
			Scribe_Collections.Look<SpawnRequest>(ref this.requests, "requests", LookMode.Deep);
		}

		// Token: 0x04007791 RID: 30609
		public Map map;

		// Token: 0x04007792 RID: 30610
		private HashSet<SpawnRequest> requests = new HashSet<SpawnRequest>();
	}
}
