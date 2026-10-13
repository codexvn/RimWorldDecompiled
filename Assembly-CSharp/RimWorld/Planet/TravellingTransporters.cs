using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E18 RID: 15896
	public class TravellingTransporters : WorldObject, IThingHolder
	{
		// Token: 0x17003601 RID: 13825
		// (get) Token: 0x06015B02 RID: 88834 RVA: 0x0066022D File Offset: 0x0065E42D
		public bool IsPlayerControlled
		{
			get
			{
				return base.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x17003602 RID: 13826
		// (get) Token: 0x06015B03 RID: 88835 RVA: 0x0067D263 File Offset: 0x0067B463
		private Vector3 Start
		{
			get
			{
				return Find.WorldGrid.GetTileCenter(this.initialTile);
			}
		}

		// Token: 0x17003603 RID: 13827
		// (get) Token: 0x06015B04 RID: 88836 RVA: 0x0067D275 File Offset: 0x0067B475
		private Vector3 End
		{
			get
			{
				return Find.WorldGrid.GetTileCenter(this.destinationTile);
			}
		}

		// Token: 0x17003604 RID: 13828
		// (get) Token: 0x06015B05 RID: 88837 RVA: 0x0067D287 File Offset: 0x0067B487
		public override Vector3 DrawPos
		{
			get
			{
				return Vector3.Slerp(this.Start, this.End, this.traveledPct);
			}
		}

		// Token: 0x17003605 RID: 13829
		// (get) Token: 0x06015B06 RID: 88838 RVA: 0x0067D2A0 File Offset: 0x0067B4A0
		public override bool ExpandingIconFlipHorizontal
		{
			get
			{
				return GenWorldUI.WorldToUIPosition(this.Start).x > GenWorldUI.WorldToUIPosition(this.End).x;
			}
		}

		// Token: 0x17003606 RID: 13830
		// (get) Token: 0x06015B07 RID: 88839 RVA: 0x0067D2C4 File Offset: 0x0067B4C4
		private Thing Shuttle
		{
			get
			{
				ActiveTransporterInfo activeTransporterInfo = this.transporters.FirstOrDefault<ActiveTransporterInfo>();
				if (activeTransporterInfo == null)
				{
					return null;
				}
				return activeTransporterInfo.GetShuttle();
			}
		}

		// Token: 0x17003607 RID: 13831
		// (get) Token: 0x06015B08 RID: 88840 RVA: 0x0067D2DC File Offset: 0x0067B4DC
		public override Color ExpandingIconColor
		{
			get
			{
				return base.Faction.Color;
			}
		}

		// Token: 0x17003608 RID: 13832
		// (get) Token: 0x06015B09 RID: 88841 RVA: 0x0067D2EC File Offset: 0x0067B4EC
		public override float ExpandingIconRotation
		{
			get
			{
				if (!this.def.rotateGraphicWhenTraveling)
				{
					return base.ExpandingIconRotation;
				}
				Vector2 vector = GenWorldUI.WorldToUIPosition(this.Start);
				Vector2 vector2 = GenWorldUI.WorldToUIPosition(this.End);
				float num = Mathf.Atan2(vector2.y - vector.y, vector2.x - vector.x) * 57.29578f;
				if (num > 180f)
				{
					num -= 180f;
				}
				return num + 90f;
			}
		}

		// Token: 0x17003609 RID: 13833
		// (get) Token: 0x06015B0A RID: 88842 RVA: 0x0067D364 File Offset: 0x0067B564
		private float TraveledPctStepPerTick
		{
			get
			{
				Vector3 start = this.Start;
				Vector3 end = this.End;
				if (start == end)
				{
					return 1f;
				}
				float num = GenMath.SphericalDistance(start.normalized, end.normalized);
				if (num == 0f)
				{
					return 1f;
				}
				return 0.00025f / num;
			}
		}

		// Token: 0x1700360A RID: 13834
		// (get) Token: 0x06015B0B RID: 88843 RVA: 0x0067D3B8 File Offset: 0x0067B5B8
		private bool PodsHaveAnyPotentialCaravanOwner
		{
			get
			{
				for (int i = 0; i < this.transporters.Count; i++)
				{
					ThingOwner innerContainer = this.transporters[i].innerContainer;
					for (int j = 0; j < innerContainer.Count; j++)
					{
						Pawn pawn = innerContainer[j] as Pawn;
						if (pawn != null && CaravanUtility.IsOwner(pawn, base.Faction))
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x1700360B RID: 13835
		// (get) Token: 0x06015B0C RID: 88844 RVA: 0x0067D420 File Offset: 0x0067B620
		public bool PodsHaveAnyFreeColonist
		{
			get
			{
				for (int i = 0; i < this.transporters.Count; i++)
				{
					ThingOwner innerContainer = this.transporters[i].innerContainer;
					for (int j = 0; j < innerContainer.Count; j++)
					{
						Pawn pawn = innerContainer[j] as Pawn;
						if (pawn != null && pawn.IsColonist && pawn.HostFaction == null)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x1700360C RID: 13836
		// (get) Token: 0x06015B0D RID: 88845 RVA: 0x0067D489 File Offset: 0x0067B689
		public IEnumerable<Pawn> Pawns
		{
			get
			{
				int num;
				for (int i = 0; i < this.transporters.Count; i = num + 1)
				{
					ThingOwner things = this.transporters[i].innerContainer;
					for (int j = 0; j < things.Count; j = num + 1)
					{
						Pawn pawn = things[j] as Pawn;
						if (pawn != null)
						{
							yield return pawn;
						}
						num = j;
					}
					things = null;
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x06015B0E RID: 88846 RVA: 0x0067D49C File Offset: 0x0067B69C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<ActiveTransporterInfo>(ref this.transporters, "pods", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<PlanetTile>(ref this.destinationTile, "destinationTile", default(PlanetTile), false);
			Scribe_Values.Look<bool>(ref this.arrived, "arrived", false, false);
			Scribe_Values.Look<PlanetTile>(ref this.initialTile, "initialTile", default(PlanetTile), false);
			Scribe_Values.Look<float>(ref this.traveledPct, "traveledPct", 0f, false);
			Scribe_Deep.Look<TransportersArrivalAction>(ref this.arrivalAction, "arrivalAction", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int i = 0; i < this.transporters.Count; i++)
				{
					this.transporters[i].parent = this;
				}
			}
		}

		// Token: 0x06015B0F RID: 88847 RVA: 0x0067D566 File Offset: 0x0067B766
		public override void PostAdd()
		{
			base.PostAdd();
			this.initialTile = base.Tile;
		}

		// Token: 0x06015B10 RID: 88848 RVA: 0x0067D57A File Offset: 0x0067B77A
		protected override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			this.traveledPct += this.TraveledPctStepPerTick * (float)delta;
			if (this.traveledPct >= 1f)
			{
				this.traveledPct = 1f;
				this.Arrived();
			}
		}

		// Token: 0x06015B11 RID: 88849 RVA: 0x0067D5B8 File Offset: 0x0067B7B8
		public void AddTransporter(ActiveTransporterInfo contents, bool justLeftTheMap)
		{
			contents.parent = this;
			this.transporters.Add(contents);
			ThingOwner innerContainer = contents.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				Pawn pawn = innerContainer[i] as Pawn;
				if (pawn != null && !pawn.IsWorldPawn())
				{
					if (!base.Spawned)
					{
						Log.Warning(string.Format("Passing pawn {0} to world, but the TravelingTransportPod is not spawned. This means that WorldPawns can discard this pawn which can cause bugs.", pawn));
					}
					if (justLeftTheMap)
					{
						pawn.ExitMap(false, Rot4.Invalid);
					}
					else
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					}
				}
			}
			contents.savePawnsWithReferenceMode = true;
		}

		// Token: 0x06015B12 RID: 88850 RVA: 0x0067D644 File Offset: 0x0067B844
		public bool ContainsPawn(Pawn p)
		{
			for (int i = 0; i < this.transporters.Count; i++)
			{
				if (this.transporters[i].innerContainer.Contains(p))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06015B13 RID: 88851 RVA: 0x0067D684 File Offset: 0x0067B884
		private void Arrived()
		{
			if (this.arrived)
			{
				return;
			}
			this.arrived = true;
			if (this.arrivalAction == null || !this.arrivalAction.StillValid(this.transporters, this.destinationTile))
			{
				this.arrivalAction = null;
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].Tile == this.destinationTile)
					{
						this.arrivalAction = new TransportersArrivalAction_LandInSpecificCell(maps[i].Parent, DropCellFinder.RandomDropSpot(maps[i], true));
						break;
					}
				}
				if (this.arrivalAction == null)
				{
					if (TransportersArrivalAction_FormCaravan.CanFormCaravanAt(this.transporters, this.destinationTile))
					{
						this.arrivalAction = new TransportersArrivalAction_FormCaravan();
					}
					else
					{
						List<Caravan> caravans = Find.WorldObjects.Caravans;
						for (int j = 0; j < caravans.Count; j++)
						{
							if (caravans[j].Tile == this.destinationTile && TransportersArrivalAction_GiveToCaravan.CanGiveTo(this.transporters, caravans[j]))
							{
								this.arrivalAction = new TransportersArrivalAction_GiveToCaravan(caravans[j]);
								break;
							}
						}
					}
				}
			}
			if (this.arrivalAction != null && this.arrivalAction.ShouldUseLongEvent(this.transporters, this.destinationTile))
			{
				LongEventHandler.QueueLongEvent(new Action(this.DoArrivalAction), "GeneratingMapForNewEncounter", false, null, true, false, null);
				return;
			}
			this.DoArrivalAction();
		}

		// Token: 0x06015B14 RID: 88852 RVA: 0x0067D7F8 File Offset: 0x0067B9F8
		private void DoArrivalAction()
		{
			for (int i = 0; i < this.transporters.Count; i++)
			{
				this.transporters[i].savePawnsWithReferenceMode = false;
				this.transporters[i].parent = null;
			}
			if (this.arrivalAction != null)
			{
				try
				{
					this.arrivalAction.Arrived(this.transporters, this.destinationTile);
				}
				catch (Exception ex)
				{
					string str = "Exception in transport pods arrival action: ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
				this.arrivalAction = null;
			}
			else
			{
				for (int j = 0; j < this.transporters.Count; j++)
				{
					for (int k = 0; k < this.transporters[j].innerContainer.Count; k++)
					{
						Pawn pawn = this.transporters[j].innerContainer[k] as Pawn;
						if (pawn != null && (pawn.Faction == Faction.OfPlayer || pawn.HostFaction == Faction.OfPlayer))
						{
							PawnBanishUtility.Banish(pawn, this.destinationTile, true);
						}
					}
				}
				bool flag = true;
				if (ModsConfig.BiotechActive)
				{
					flag = false;
					int num = 0;
					while (num < this.transporters.Count && !flag)
					{
						for (int l = 0; l < this.transporters[num].innerContainer.Count; l++)
						{
							if (this.transporters[num].innerContainer[l].def != ThingDefOf.Wastepack)
							{
								flag = true;
								break;
							}
						}
						num++;
					}
				}
				for (int m = 0; m < this.transporters.Count; m++)
				{
					for (int n = 0; n < this.transporters[m].innerContainer.Count; n++)
					{
						this.transporters[m].innerContainer[n].Notify_AbandonedAtTile(this.destinationTile);
					}
				}
				for (int num2 = 0; num2 < this.transporters.Count; num2++)
				{
					this.transporters[num2].innerContainer.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
				}
				if (flag)
				{
					string key = "MessageTransportPodsArrivedAndLost";
					if (this.def == WorldObjectDefOf.TravelingShuttle)
					{
						key = "MessageShuttleArrivedContentsLost";
					}
					Messages.Message(key.Translate(), new GlobalTargetInfo(this.destinationTile), MessageTypeDefOf.NegativeEvent, true);
				}
			}
			this.transporters.Clear();
			this.Destroy();
		}

		// Token: 0x06015B15 RID: 88853 RVA: 0x00002C42 File Offset: 0x00000E42
		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		// Token: 0x06015B16 RID: 88854 RVA: 0x0067DA8C File Offset: 0x0067BC8C
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
			for (int i = 0; i < this.transporters.Count; i++)
			{
				outChildren.Add(this.transporters[i]);
			}
		}

		// Token: 0x0400F0E6 RID: 61670
		public PlanetTile destinationTile = PlanetTile.Invalid;

		// Token: 0x0400F0E7 RID: 61671
		public TransportersArrivalAction arrivalAction;

		// Token: 0x0400F0E8 RID: 61672
		private List<ActiveTransporterInfo> transporters = new List<ActiveTransporterInfo>();

		// Token: 0x0400F0E9 RID: 61673
		private bool arrived;

		// Token: 0x0400F0EA RID: 61674
		private PlanetTile initialTile = PlanetTile.Invalid;

		// Token: 0x0400F0EB RID: 61675
		private float traveledPct;

		// Token: 0x0400F0EC RID: 61676
		private const float TravelSpeed = 0.00025f;
	}
}
