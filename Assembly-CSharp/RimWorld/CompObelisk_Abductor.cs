using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D85 RID: 11653
	public class CompObelisk_Abductor : CompObelisk_ExplodingSpawner
	{
		// Token: 0x170029BC RID: 10684
		// (get) Token: 0x0601061B RID: 67099 RVA: 0x004B9BBC File Offset: 0x004B7DBC
		public LabyrinthMapComponent Labyrinth
		{
			get
			{
				LabyrinthMapComponent result;
				if ((result = this.mapComp) == null)
				{
					result = (this.mapComp = this.labyrinthMap.GetComponent<LabyrinthMapComponent>());
				}
				return result;
			}
		}

		// Token: 0x0601061C RID: 67100 RVA: 0x004B9BE8 File Offset: 0x004B7DE8
		public override void TriggerInteractionEffect(Pawn interactor, bool triggeredByPlayer = false)
		{
			if (!triggeredByPlayer)
			{
				if (GenTicks.TicksGame < this.cooldownTick)
				{
					return;
				}
				this.cooldownTick = GenTicks.TicksGame + Mathf.RoundToInt(CompObelisk_Abductor.CooldownDaysRange.RandomInRange * 60000f);
			}
			if (this.labyrinthMap != null)
			{
				this.Labyrinth.TeleportToLabyrinth(interactor);
				CompObelisk_Abductor.SendTriggeredLetter(interactor);
				return;
			}
			this.instigator = interactor;
			interactor.stances.stunner.StunFor(240, null, false, false, false);
			this.interactSkipTick = GenTicks.TicksGame + 240;
			this.effects[interactor] = EffecterDefOf.AbductWarmup_Pawn.SpawnAttached(interactor, interactor.MapHeld, 1f);
			if (this.buildingEffecter == null)
			{
				this.buildingEffecter = EffecterDefOf.AbductWarmup_Monolith.SpawnAttached(this.parent, this.parent.MapHeld, 1f);
			}
			Find.TickManager.slower.SignalForceNormalSpeedShort();
			CompObelisk_Abductor.SendDisappearingLetter(interactor);
		}

		// Token: 0x0601061D RID: 67101 RVA: 0x004B9CDC File Offset: 0x004B7EDC
		public override void OnActivityActivated()
		{
			base.OnActivityActivated();
			Find.LetterStack.ReceiveLetter("ObeliskAbductorLetterLabel".Translate(), "ObeliskAbductorLetter".Translate(), LetterDefOf.ThreatBig, this.parent, null, null, null, null, 0, true);
			this.pointsRemaining *= 0.15f;
			int num = 0;
			int num2 = 0;
			foreach (Pawn pawn in this.parent.MapHeld.mapPawns.AllPawnsSpawned)
			{
				if (CompObelisk_Abductor.IsValidColonist(pawn))
				{
					num++;
				}
				else if (CompObelisk_Abductor.IsValidOther(pawn))
				{
					num2++;
				}
			}
			this.colonists = Mathf.Max(Mathf.FloorToInt((float)num * 0.5f), 1);
			this.others = Mathf.Max(Mathf.FloorToInt((float)num2 * 0.2f), 1);
			foreach (Pawn pawn2 in this.parent.MapHeld.mapPawns.AllPawnsSpawned)
			{
				if (CompObelisk_Abductor.IsValidColonist(pawn2) && this.colonists > 0)
				{
					this.colonists--;
					this.teleporting.Add(pawn2);
					pawn2.stances.stunner.StunFor(2400, null, false, false, false);
					this.effects[pawn2] = EffecterDefOf.AbductWarmup_Pawn.SpawnAttached(pawn2, pawn2.MapHeld, 1f);
					if (this.buildingEffecter == null)
					{
						this.buildingEffecter = EffecterDefOf.AbductWarmup_Monolith.SpawnAttached(this.parent, this.parent.MapHeld, 1f);
					}
				}
				else if (CompObelisk_Abductor.IsValidOther(pawn2) && this.others > 0)
				{
					this.others--;
					this.teleporting.Add(pawn2);
				}
			}
			this.instigator = null;
			this.nextTeleportTick = GenTicks.TicksGame + 240;
			this.lastTeleportTick = this.nextTeleportTick;
		}

		// Token: 0x0601061E RID: 67102 RVA: 0x004B9F0C File Offset: 0x004B810C
		public override void CompTick()
		{
			base.CompTick();
			if (this.interactSkipTick != 0 && GenTicks.TicksGame >= this.interactSkipTick)
			{
				this.interactSkipTick = 0;
				if (this.labyrinthMap == null)
				{
					this.GenerateLabyrinth(this.instigator);
				}
				CompObelisk_Abductor.SendTriggeredLetter(this.instigator);
				this.instigator = null;
			}
			this.VFXTick();
			if (this.activated)
			{
				this.ActivatedTick();
			}
		}

		// Token: 0x0601061F RID: 67103 RVA: 0x004B9F78 File Offset: 0x004B8178
		private void VFXTick()
		{
			foreach (KeyValuePair<Pawn, Effecter> keyValuePair in this.effects)
			{
				Pawn pawn;
				Effecter effecter;
				keyValuePair.Deconstruct(out pawn, out effecter);
				Pawn pawn2 = pawn;
				Effecter effecter2 = effecter;
				if (pawn2.Map == this.parent.Map)
				{
					effecter2.EffectTick(pawn2, pawn2);
				}
				else
				{
					effecter2.Cleanup();
					CompObelisk_Abductor.tmpRemoveEffectors.Add(pawn2);
				}
			}
			this.effects.RemoveRange(CompObelisk_Abductor.tmpRemoveEffectors);
			CompObelisk_Abductor.tmpRemoveEffectors.Clear();
			if (this.effects.Count == 0 && this.buildingEffecter != null)
			{
				this.buildingEffecter.Cleanup();
				this.buildingEffecter = null;
			}
		}

		// Token: 0x06010620 RID: 67104 RVA: 0x004BA050 File Offset: 0x004B8250
		private void ActivatedTick()
		{
			if (this.explodeTick > 0)
			{
				return;
			}
			if (GenTicks.TicksGame < this.nextTeleportTick)
			{
				return;
			}
			if (this.labyrinthMap == null && !this.generating)
			{
				this.lastTeleportTick = GenTicks.TicksGame;
				this.GenerateLabyrinth(null);
				return;
			}
			if (this.labyrinthMap == null)
			{
				this.nextTeleportTick = GenTicks.TicksGame + CompObelisk_Abductor.TeleportDelayTicksRange.RandomInRange;
				this.lastTeleportTick = GenTicks.TicksGame;
				return;
			}
			this.nextTeleportTick = GenTicks.TicksGame + CompObelisk_Abductor.TeleportDelayTicksRange.RandomInRange;
			if (this.explodeTick < 0 && GenTicks.TicksGame >= this.lastTeleportTick + 180)
			{
				base.PrepareExplosion();
			}
			for (int i = this.teleporting.Count - 1; i >= 0; i--)
			{
				if (this.teleporting[i].DestroyedOrNull())
				{
					this.teleporting.RemoveAt(i);
				}
			}
			if (this.teleporting.Empty<Pawn>())
			{
				return;
			}
			Pawn pawn = this.teleporting.RandomElement<Pawn>();
			this.teleporting.Remove(pawn);
			if (CompObelisk_Abductor.IsValidColonist(pawn))
			{
				this.colonists--;
			}
			else if (CompObelisk_Abductor.IsValidHostile(pawn))
			{
				this.pointsRemaining -= pawn.kindDef.combatPower;
			}
			else
			{
				this.others--;
			}
			this.Labyrinth.TeleportToLabyrinth(pawn);
			this.lastTeleportTick = GenTicks.TicksGame;
			pawn.stances.stunner.StopStun();
		}

		// Token: 0x06010621 RID: 67105 RVA: 0x004BA1D0 File Offset: 0x004B83D0
		private void GenerateLabyrinth(Pawn interactor)
		{
			this.generating = true;
			LongEventHandler.QueueLongEvent(delegate()
			{
				this.labyrinthMap = PocketMapUtility.GeneratePocketMap(new IntVec3(90, 1, 90), MapGeneratorDefOf.Labyrinth, null, this.parent.MapHeld);
				this.labyrinthMap.GetComponent<LabyrinthMapComponent>().abductorObelisk = (Building)this.parent;
			}, "GeneratingLabyrinth", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap), false, false, delegate()
			{
				this.generating = false;
				if (interactor != null)
				{
					this.Labyrinth.TeleportToLabyrinth(interactor);
				}
				CameraJumper.TryJump(interactor, CameraJumper.MovementMode.Cut);
			});
		}

		// Token: 0x06010622 RID: 67106 RVA: 0x004BA22C File Offset: 0x004B842C
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (this.labyrinthMap == null)
			{
				return;
			}
			if (this.labyrinthMap.mapPawns.AnyColonistSpawned || this.labyrinthMap.mapPawns.SlavesOfColonySpawned.Any<Pawn>())
			{
				return;
			}
			PocketMapUtility.DestroyPocketMap(this.labyrinthMap);
			this.Notify_MapDestroyed();
		}

		// Token: 0x06010623 RID: 67107 RVA: 0x004BA285 File Offset: 0x004B8485
		public void Notify_MapDestroyed()
		{
			this.labyrinthMap = null;
			this.mapComp = null;
		}

		// Token: 0x06010624 RID: 67108 RVA: 0x004BA298 File Offset: 0x004B8498
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.cooldownTick, "cooldownTick", 0, false);
			Scribe_Values.Look<int>(ref this.nextTeleportTick, "nextTeleport", 0, false);
			Scribe_Values.Look<int>(ref this.lastTeleportTick, "lastTeleport", 0, false);
			Scribe_Values.Look<int>(ref this.interactSkipTick, "interactSendTick", 0, false);
			Scribe_Values.Look<int>(ref this.colonists, "colonists", 0, false);
			Scribe_Values.Look<int>(ref this.others, "others", 0, false);
			Scribe_Collections.Look<Pawn>(ref this.teleporting, "teleporting", LookMode.Reference, Array.Empty<object>());
			Scribe_References.Look<Map>(ref this.labyrinthMap, "labyrinthMap", false);
			Scribe_References.Look<Pawn>(ref this.instigator, "storedInstigator", false);
		}

		// Token: 0x06010625 RID: 67109 RVA: 0x004BA34F File Offset: 0x004B854F
		private static bool IsValidColonist(Pawn pawn)
		{
			return pawn.IsColonist || pawn.IsSlaveOfColony || pawn.IsPrisonerOfColony;
		}

		// Token: 0x06010626 RID: 67110 RVA: 0x004BA369 File Offset: 0x004B8569
		private static bool IsValidHostile(Pawn pawn)
		{
			return pawn.HostileTo(Faction.OfPlayer) && !pawn.IsOnHoldingPlatform;
		}

		// Token: 0x06010627 RID: 67111 RVA: 0x004BA383 File Offset: 0x004B8583
		private static bool IsValidOther(Pawn pawn)
		{
			return pawn.IsAnimal || pawn.IsColonyMech;
		}

		// Token: 0x06010628 RID: 67112 RVA: 0x004BA398 File Offset: 0x004B8598
		private static void SendDisappearingLetter(Pawn interactor)
		{
			TaggedString label = "ObeliskAbductedDisappearingLetterLabel".Translate(interactor.Named("PAWN"));
			TaggedString text = "ObeliskAbductedDisappearingLetter".Translate(interactor.Named("PAWN"));
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.ThreatBig, interactor, null, null, null, null, 0, true);
		}

		// Token: 0x06010629 RID: 67113 RVA: 0x004BA3F0 File Offset: 0x004B85F0
		private static void SendTriggeredLetter(Pawn interactor)
		{
			TaggedString label = "ObeliskAbductedLetterLabel".Translate(interactor.Named("PAWN"));
			TaggedString text = "ObeliskAbductedLetter".Translate(interactor.Named("PAWN"));
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent, interactor, null, null, null, null, 0, true);
		}

		// Token: 0x0400AAE0 RID: 43744
		private int cooldownTick;

		// Token: 0x0400AAE1 RID: 43745
		private Map labyrinthMap;

		// Token: 0x0400AAE2 RID: 43746
		private int nextTeleportTick;

		// Token: 0x0400AAE3 RID: 43747
		private int lastTeleportTick;

		// Token: 0x0400AAE4 RID: 43748
		private int interactSkipTick;

		// Token: 0x0400AAE5 RID: 43749
		private int colonists;

		// Token: 0x0400AAE6 RID: 43750
		private int others;

		// Token: 0x0400AAE7 RID: 43751
		private Pawn instigator;

		// Token: 0x0400AAE8 RID: 43752
		private Dictionary<Pawn, Effecter> effects = new Dictionary<Pawn, Effecter>();

		// Token: 0x0400AAE9 RID: 43753
		private Effecter buildingEffecter;

		// Token: 0x0400AAEA RID: 43754
		private List<Pawn> teleporting = new List<Pawn>();

		// Token: 0x0400AAEB RID: 43755
		private LabyrinthMapComponent mapComp;

		// Token: 0x0400AAEC RID: 43756
		private bool generating;

		// Token: 0x0400AAED RID: 43757
		private const int LabyrinthSize = 90;

		// Token: 0x0400AAEE RID: 43758
		private const int LastTeleportExplosionDelay = 180;

		// Token: 0x0400AAEF RID: 43759
		private const int InteractSkipDelayTicks = 240;

		// Token: 0x0400AAF0 RID: 43760
		private const int ActivityTeleportDelayTicks = 240;

		// Token: 0x0400AAF1 RID: 43761
		private const float ColonistFactor = 0.5f;

		// Token: 0x0400AAF2 RID: 43762
		private const float OtherFactor = 0.2f;

		// Token: 0x0400AAF3 RID: 43763
		private const float HostilePointsFactor = 0.15f;

		// Token: 0x0400AAF4 RID: 43764
		private static readonly FloatRange CooldownDaysRange = new FloatRange(15f, 80f);

		// Token: 0x0400AAF5 RID: 43765
		private static readonly IntRange TeleportDelayTicksRange = new IntRange(10, 30);

		// Token: 0x0400AAF6 RID: 43766
		private static readonly List<Pawn> tmpRemoveEffectors = new List<Pawn>();
	}
}
