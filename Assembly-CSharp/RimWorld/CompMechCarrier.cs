using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002D2E RID: 11566
	public class CompMechCarrier : ThingComp, IThingHolder
	{
		// Token: 0x1700294D RID: 10573
		// (get) Token: 0x060103F7 RID: 66551 RVA: 0x004B255B File Offset: 0x004B075B
		public CompProperties_MechCarrier Props
		{
			get
			{
				return (CompProperties_MechCarrier)this.props;
			}
		}

		// Token: 0x1700294E RID: 10574
		// (get) Token: 0x060103F8 RID: 66552 RVA: 0x004B2568 File Offset: 0x004B0768
		public AcceptanceReport CanSpawn
		{
			get
			{
				Pawn pawn = this.parent as Pawn;
				if (pawn != null)
				{
					if (pawn.IsSelfShutdown())
					{
						return "SelfShutdown".Translate();
					}
					if (pawn.Faction == Faction.OfPlayer && !pawn.IsColonyMechPlayerControlled)
					{
						return false;
					}
					if (!pawn.Awake() || pawn.Downed || pawn.Dead || !pawn.Spawned)
					{
						return false;
					}
				}
				if (this.MaxCanSpawn <= 0)
				{
					return "MechCarrierNotEnoughResources".Translate();
				}
				if (this.cooldownTicksRemaining > 0)
				{
					return "CooldownTime".Translate() + " " + this.cooldownTicksRemaining.ToStringSecondsFromTicks();
				}
				return true;
			}
		}

		// Token: 0x1700294F RID: 10575
		// (get) Token: 0x060103F9 RID: 66553 RVA: 0x004B2631 File Offset: 0x004B0831
		public int IngredientCount
		{
			get
			{
				return this.innerContainer.TotalStackCountOfDef(this.Props.fixedIngredient);
			}
		}

		// Token: 0x17002950 RID: 10576
		// (get) Token: 0x060103FA RID: 66554 RVA: 0x004B2649 File Offset: 0x004B0849
		public int AmountToAutofill
		{
			get
			{
				return Mathf.Max(0, this.maxToFill - this.IngredientCount);
			}
		}

		// Token: 0x17002951 RID: 10577
		// (get) Token: 0x060103FB RID: 66555 RVA: 0x004B265E File Offset: 0x004B085E
		public int MaxCanSpawn
		{
			get
			{
				return Mathf.Min(Mathf.FloorToInt((float)(this.IngredientCount / this.Props.costPerPawn)), this.Props.maxPawnsToSpawn);
			}
		}

		// Token: 0x17002952 RID: 10578
		// (get) Token: 0x060103FC RID: 66556 RVA: 0x004B2688 File Offset: 0x004B0888
		public bool LowIngredientCount
		{
			get
			{
				return this.IngredientCount < 250;
			}
		}

		// Token: 0x17002953 RID: 10579
		// (get) Token: 0x060103FD RID: 66557 RVA: 0x004B2697 File Offset: 0x004B0897
		public float PercentageFull
		{
			get
			{
				return (float)this.IngredientCount / (float)this.Props.maxIngredientCount;
			}
		}

		// Token: 0x060103FF RID: 66559 RVA: 0x004B26CC File Offset: 0x004B08CC
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckBiotech("Mech carrier"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad && !this.parent.BeingTransportedOnGravship)
			{
				this.innerContainer = new ThingOwner<Thing>(this, false, LookMode.Deep, true);
				if (this.Props.startingIngredientCount > 0)
				{
					Thing thing = ThingMaker.MakeThing(this.Props.fixedIngredient, null);
					thing.stackCount = this.Props.startingIngredientCount;
					this.innerContainer.TryAdd(thing, this.Props.startingIngredientCount, true);
				}
				this.maxToFill = this.Props.startingIngredientCount;
			}
		}

		// Token: 0x06010400 RID: 66560 RVA: 0x004B2774 File Offset: 0x004B0974
		public void TrySpawnPawns()
		{
			int maxCanSpawn = this.MaxCanSpawn;
			if (maxCanSpawn <= 0)
			{
				return;
			}
			PawnGenerationRequest request = new PawnGenerationRequest(this.Props.spawnPawnKind, this.parent.Faction, PawnGenerationContext.NonPlayer, null, true, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Newborn, null, null, null, false, false, false, -1, 0, false);
			this.tmpResources.Clear();
			this.tmpResources.AddRange(this.innerContainer);
			Pawn pawn = this.parent as Pawn;
			Lord lord = (pawn != null) ? pawn.GetLord() : null;
			for (int i = 0; i < maxCanSpawn; i++)
			{
				Pawn pawn2 = PawnGenerator.GeneratePawn(request);
				GenSpawn.Spawn(pawn2, this.parent.Position, this.parent.Map, WipeMode.Vanish);
				this.spawnedPawns.Add(pawn2);
				if (lord != null)
				{
					lord.AddPawn(pawn2);
				}
				int num = this.Props.costPerPawn;
				for (int j = 0; j < this.tmpResources.Count; j++)
				{
					Thing thing = this.innerContainer.Take(this.tmpResources[j], Mathf.Min(this.tmpResources[j].stackCount, this.Props.costPerPawn));
					num -= thing.stackCount;
					thing.Destroy(DestroyMode.Vanish);
					if (num <= 0)
					{
						break;
					}
				}
				if (this.Props.spawnedMechEffecter != null)
				{
					Effecter effecter = new Effecter(this.Props.spawnedMechEffecter);
					effecter.Trigger(this.Props.attachSpawnedMechEffecter ? pawn2 : new TargetInfo(pawn2.Position, pawn2.Map, false), TargetInfo.Invalid, -1);
					effecter.Cleanup();
				}
			}
			this.tmpResources.Clear();
			this.cooldownTicksRemaining = this.Props.cooldownTicks;
			if (this.Props.spawnEffecter != null)
			{
				Effecter effecter2 = new Effecter(this.Props.spawnEffecter);
				effecter2.Trigger(this.Props.attachSpawnedEffecter ? this.parent : new TargetInfo(this.parent.Position, this.parent.Map, false), TargetInfo.Invalid, -1);
				effecter2.Cleanup();
			}
		}

		// Token: 0x06010401 RID: 66561 RVA: 0x004B2A12 File Offset: 0x004B0C12
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			Pawn pawn = this.parent as Pawn;
			if (pawn != null && pawn.IsColonyMech && pawn.GetOverseer() != null)
			{
				foreach (Gizmo gizmo in base.CompGetGizmosExtra())
				{
					yield return gizmo;
				}
				IEnumerator<Gizmo> enumerator = null;
				if (Find.Selector.SingleSelectedThing == this.parent)
				{
					if (this.gizmo == null)
					{
						this.gizmo = new MechCarrierGizmo(this);
					}
					yield return this.gizmo;
				}
				AcceptanceReport canSpawn = this.CanSpawn;
				Command_ActionWithCooldown act = new Command_ActionWithCooldown();
				act.cooldownPercentGetter = (() => Mathf.InverseLerp((float)this.Props.cooldownTicks, 0f, (float)this.cooldownTicksRemaining));
				act.action = delegate()
				{
					this.TrySpawnPawns();
				};
				act.hotKey = KeyBindingDefOf.Misc2;
				act.Disabled = !canSpawn.Accepted;
				act.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/ReleaseWarUrchins", true);
				act.defaultLabel = "MechCarrierRelease".Translate(this.Props.spawnPawnKind.labelPlural);
				act.defaultDesc = "MechCarrierDesc".Translate(this.Props.maxPawnsToSpawn, this.Props.spawnPawnKind.labelPlural, this.Props.spawnPawnKind.label, this.Props.costPerPawn, this.Props.fixedIngredient.label);
				if (!canSpawn.Reason.NullOrEmpty())
				{
					act.Disable(canSpawn.Reason);
				}
				if (DebugSettings.ShowDevGizmos)
				{
					if (this.cooldownTicksRemaining > 0)
					{
						yield return new Command_Action
						{
							defaultLabel = "DEV: Reset cooldown",
							action = delegate()
							{
								this.cooldownTicksRemaining = 0;
							}
						};
					}
					yield return new Command_Action
					{
						defaultLabel = "DEV: Fill with " + this.Props.fixedIngredient.label,
						action = delegate()
						{
							while (this.IngredientCount < this.Props.maxIngredientCount)
							{
								int stackCount = Mathf.Min(this.Props.maxIngredientCount - this.IngredientCount, this.Props.fixedIngredient.stackLimit);
								Thing thing = ThingMaker.MakeThing(this.Props.fixedIngredient, null);
								thing.stackCount = stackCount;
								this.innerContainer.TryAdd(thing, thing.stackCount, true);
							}
						}
					};
					yield return new Command_Action
					{
						defaultLabel = "DEV: Empty " + this.Props.fixedIngredient.label,
						action = delegate()
						{
							this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
						}
					};
				}
				yield return act;
				act = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06010402 RID: 66562 RVA: 0x004B2A22 File Offset: 0x004B0C22
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06010403 RID: 66563 RVA: 0x004B2A30 File Offset: 0x004B0C30
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06010404 RID: 66564 RVA: 0x004B2A38 File Offset: 0x004B0C38
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + ("CasketContains".Translate() + ": " + this.innerContainer.ContentsString.CapitalizeFirst());
		}

		// Token: 0x06010405 RID: 66565 RVA: 0x004B2A98 File Offset: 0x004B0C98
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			ThingOwner thingOwner = this.innerContainer;
			if (thingOwner != null)
			{
				thingOwner.ClearAndDestroyContents(DestroyMode.Vanish);
			}
			for (int i = 0; i < this.spawnedPawns.Count; i++)
			{
				if (!this.spawnedPawns[i].Dead)
				{
					this.spawnedPawns[i].Kill(null, null);
				}
			}
		}

		// Token: 0x06010406 RID: 66566 RVA: 0x004B2B04 File Offset: 0x004B0D04
		public override void PostDrawExtraSelectionOverlays()
		{
			if (Find.Selector.IsSelected(this.parent))
			{
				for (int i = 0; i < this.spawnedPawns.Count; i++)
				{
					if (!this.spawnedPawns[i].Dead)
					{
						GenDraw.DrawLineBetween(this.parent.TrueCenter(), this.spawnedPawns[i].TrueCenter());
					}
				}
			}
		}

		// Token: 0x06010407 RID: 66567 RVA: 0x004B2B70 File Offset: 0x004B0D70
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Values.Look<int>(ref this.cooldownTicksRemaining, "cooldownTicksRemaining", 0, false);
			Scribe_Values.Look<int>(ref this.maxToFill, "maxToFill", 0, false);
			Scribe_Collections.Look<Pawn>(ref this.spawnedPawns, "spawnedPawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.spawnedPawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06010408 RID: 66568 RVA: 0x004B2C0A File Offset: 0x004B0E0A
		public override void CompTick()
		{
			if (this.cooldownTicksRemaining > 0)
			{
				this.cooldownTicksRemaining--;
			}
		}

		// Token: 0x0400A9B6 RID: 43446
		private const int LowIngredientCountThreshold = 250;

		// Token: 0x0400A9B7 RID: 43447
		private int cooldownTicksRemaining;

		// Token: 0x0400A9B8 RID: 43448
		private ThingOwner innerContainer;

		// Token: 0x0400A9B9 RID: 43449
		private List<Pawn> spawnedPawns = new List<Pawn>();

		// Token: 0x0400A9BA RID: 43450
		private MechCarrierGizmo gizmo;

		// Token: 0x0400A9BB RID: 43451
		public int maxToFill;

		// Token: 0x0400A9BC RID: 43452
		private List<Thing> tmpResources = new List<Thing>();
	}
}
