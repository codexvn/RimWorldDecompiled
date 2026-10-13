using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020029E4 RID: 10724
	public class Building_Casket : Building, IThingHolder, IOpenable, ISearchableContents
	{
		// Token: 0x17002481 RID: 9345
		// (get) Token: 0x0600EC98 RID: 60568 RVA: 0x00222DEB File Offset: 0x00220FEB
		public virtual int OpenTicks
		{
			get
			{
				return 300;
			}
		}

		// Token: 0x0600EC99 RID: 60569 RVA: 0x0044B2F2 File Offset: 0x004494F2
		public Building_Casket()
		{
			this.innerContainer = new ThingOwner<Thing>(this, false, LookMode.Deep, true);
		}

		// Token: 0x17002482 RID: 9346
		// (get) Token: 0x0600EC9A RID: 60570 RVA: 0x0044B309 File Offset: 0x00449509
		public bool HasAnyContents
		{
			get
			{
				return this.innerContainer.Count > 0;
			}
		}

		// Token: 0x17002483 RID: 9347
		// (get) Token: 0x0600EC9B RID: 60571 RVA: 0x0044B319 File Offset: 0x00449519
		public Thing ContainedThing
		{
			get
			{
				if (this.innerContainer.Count != 0)
				{
					return this.innerContainer[0];
				}
				return null;
			}
		}

		// Token: 0x17002484 RID: 9348
		// (get) Token: 0x0600EC9C RID: 60572 RVA: 0x0044B336 File Offset: 0x00449536
		public virtual bool CanOpen
		{
			get
			{
				return this.HasAnyContents;
			}
		}

		// Token: 0x17002485 RID: 9349
		// (get) Token: 0x0600EC9D RID: 60573 RVA: 0x0044B33E File Offset: 0x0044953E
		public ThingOwner SearchableContents
		{
			get
			{
				return this.innerContainer;
			}
		}

		// Token: 0x0600EC9E RID: 60574 RVA: 0x0044B33E File Offset: 0x0044953E
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x0600EC9F RID: 60575 RVA: 0x0044B346 File Offset: 0x00449546
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600ECA0 RID: 60576 RVA: 0x0044B354 File Offset: 0x00449554
		public virtual void Open()
		{
			if (!this.HasAnyContents)
			{
				return;
			}
			this.EjectContents();
			if (!this.openedSignal.NullOrEmpty())
			{
				Find.SignalManager.SendSignal(new Signal(this.openedSignal, this.Named("SUBJECT")));
			}
			base.DirtyMapMesh(base.Map);
		}

		// Token: 0x0600ECA1 RID: 60577 RVA: 0x0044B3A9 File Offset: 0x004495A9
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			Gizmo gizmo2 = Building.SelectContainedItemGizmo(this, this.ContainedThing);
			if (gizmo2 != null)
			{
				yield return gizmo2;
			}
			if (DebugSettings.ShowDevGizmos && this.CanOpen)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Open",
					action = new Action(this.Open)
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0600ECA2 RID: 60578 RVA: 0x0044B3BC File Offset: 0x004495BC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Values.Look<bool>(ref this.contentsKnown, "contentsKnown", false, false);
			Scribe_Values.Look<string>(ref this.openedSignal, "openedSignal", null, false);
		}

		// Token: 0x0600ECA3 RID: 60579 RVA: 0x0044B40D File Offset: 0x0044960D
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (base.Faction != null && base.Faction.IsPlayer)
			{
				this.contentsKnown = true;
			}
		}

		// Token: 0x0600ECA4 RID: 60580 RVA: 0x0044B433 File Offset: 0x00449633
		public override AcceptanceReport ClaimableBy(Faction fac)
		{
			if (this.innerContainer.Any && !this.contentsKnown)
			{
				return false;
			}
			return base.ClaimableBy(fac);
		}

		// Token: 0x0600ECA5 RID: 60581 RVA: 0x0044B458 File Offset: 0x00449658
		public virtual bool Accepts(Thing thing)
		{
			return this.innerContainer.CanAcceptAnyOf(thing, true);
		}

		// Token: 0x0600ECA6 RID: 60582 RVA: 0x0044B468 File Offset: 0x00449668
		public virtual bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
		{
			if (!this.Accepts(thing))
			{
				return false;
			}
			bool flag;
			if (thing.holdingOwner != null)
			{
				thing.holdingOwner.TryTransferToContainer(thing, this.innerContainer, thing.stackCount, true);
				flag = true;
			}
			else
			{
				flag = this.innerContainer.TryAdd(thing, true);
			}
			if (flag)
			{
				if (thing.Faction != null && thing.Faction.IsPlayer)
				{
					this.contentsKnown = true;
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600ECA7 RID: 60583 RVA: 0x0044B4D8 File Offset: 0x004496D8
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.Destroy(mode);
			if (this.innerContainer.Count > 0 && (mode == DestroyMode.Deconstruct || mode == DestroyMode.KillFinalize))
			{
				if (mode != DestroyMode.Deconstruct)
				{
					List<Pawn> list = new List<Pawn>();
					foreach (Thing thing in ((IEnumerable<Thing>)this.innerContainer))
					{
						Pawn pawn = thing as Pawn;
						if (pawn != null)
						{
							list.Add(pawn);
						}
					}
					foreach (Pawn p in list)
					{
						HealthUtility.DamageUntilDowned(p, true, null, null, null);
					}
				}
				this.innerContainer.TryDropAll(base.Position, map, ThingPlaceMode.Near, null, null, true);
			}
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x0600ECA8 RID: 60584 RVA: 0x0044B5C4 File Offset: 0x004497C4
		public virtual void EjectContents()
		{
			this.innerContainer.TryDropAll(this.InteractionCell, base.Map, ThingPlaceMode.Near, null, null, true);
			this.contentsKnown = true;
		}

		// Token: 0x0600ECA9 RID: 60585 RVA: 0x0044B5EC File Offset: 0x004497EC
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			string str;
			if (!this.contentsKnown)
			{
				str = "UnknownLower".Translate();
			}
			else
			{
				str = this.innerContainer.ContentsString;
			}
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + ("CasketContains".Translate() + ": " + str.CapitalizeFirst());
		}

		// Token: 0x04009CD4 RID: 40148
		protected ThingOwner innerContainer;

		// Token: 0x04009CD5 RID: 40149
		protected bool contentsKnown;

		// Token: 0x04009CD6 RID: 40150
		public string openedSignal;
	}
}
