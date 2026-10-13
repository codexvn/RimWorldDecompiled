using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002134 RID: 8500
	public abstract class CompPower : ThingComp
	{
		// Token: 0x17001C2E RID: 7214
		// (get) Token: 0x0600B976 RID: 47478 RVA: 0x0035834D File Offset: 0x0035654D
		public bool TransmitsPowerNow
		{
			get
			{
				return ((Building)this.parent).TransmitsPowerNow;
			}
		}

		// Token: 0x17001C2F RID: 7215
		// (get) Token: 0x0600B977 RID: 47479 RVA: 0x0035835F File Offset: 0x0035655F
		public PowerNet PowerNet
		{
			get
			{
				if (this.transNet != null)
				{
					return this.transNet;
				}
				if (this.connectParent != null)
				{
					return this.connectParent.transNet;
				}
				return null;
			}
		}

		// Token: 0x17001C30 RID: 7216
		// (get) Token: 0x0600B978 RID: 47480 RVA: 0x00358385 File Offset: 0x00356585
		public CompProperties_Power Props
		{
			get
			{
				return (CompProperties_Power)this.props;
			}
		}

		// Token: 0x0600B979 RID: 47481 RVA: 0x00358392 File Offset: 0x00356592
		public virtual void ResetPowerVars()
		{
			this.transNet = null;
			this.connectParent = null;
			this.connectChildren = null;
			CompPower.recentlyConnectedNets.Clear();
			CompPower.lastManualReconnector = null;
		}

		// Token: 0x0600B97A RID: 47482 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void SetUpPowerVars()
		{
		}

		// Token: 0x0600B97B RID: 47483 RVA: 0x003583BC File Offset: 0x003565BC
		public override void PostExposeData()
		{
			Thing thing = null;
			if (Scribe.mode == LoadSaveMode.Saving && this.connectParent != null)
			{
				thing = this.connectParent.parent;
			}
			Scribe_References.Look<Thing>(ref thing, "parentThing", false);
			if (thing != null)
			{
				this.connectParent = ((ThingWithComps)thing).GetComp<CompPower>();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.connectParent != null)
			{
				this.ConnectToTransmitter(this.connectParent, true);
			}
		}

		// Token: 0x0600B97C RID: 47484 RVA: 0x00358428 File Offset: 0x00356628
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (this.Props.transmitsPower || this.parent.def.ConnectToPower)
			{
				this.parent.Map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlagDefOf.PowerGrid, true, false);
				if (this.Props.transmitsPower)
				{
					this.parent.Map.powerNetManager.Notify_TransmitterSpawned(this);
				}
				if (this.parent.def.ConnectToPower)
				{
					this.parent.Map.powerNetManager.Notify_ConnectorWantsConnect(this);
				}
				this.SetUpPowerVars();
			}
		}

		// Token: 0x0600B97D RID: 47485 RVA: 0x003584D8 File Offset: 0x003566D8
		public override void PostSwapMap()
		{
			if (this.connectParent != null && !this.connectParent.parent.Spawned)
			{
				this.LostConnectParent();
			}
		}

		// Token: 0x0600B97E RID: 47486 RVA: 0x003584FC File Offset: 0x003566FC
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.Props.transmitsPower || this.parent.def.ConnectToPower)
			{
				if (mode != DestroyMode.WillReplace || this.parent.BeingTransportedOnGravship)
				{
					if (this.Props.transmitsPower)
					{
						if (this.connectChildren != null)
						{
							for (int i = 0; i < this.connectChildren.Count; i++)
							{
								this.connectChildren[i].LostConnectParent();
							}
						}
						map.powerNetManager.Notify_TransmitterDespawned(this);
					}
					if (this.parent.def.ConnectToPower)
					{
						map.powerNetManager.Notify_ConnectorDespawned(this);
					}
				}
				map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlagDefOf.PowerGrid, true, false);
			}
		}

		// Token: 0x0600B97F RID: 47487 RVA: 0x003585C4 File Offset: 0x003567C4
		public virtual void LostConnectParent()
		{
			this.connectParent = null;
			if (this.parent.Spawned)
			{
				this.parent.Map.powerNetManager.Notify_ConnectorWantsConnect(this);
			}
		}

		// Token: 0x0600B980 RID: 47488 RVA: 0x003585F0 File Offset: 0x003567F0
		public override void PostPrintOnto(SectionLayer layer)
		{
			base.PostPrintOnto(layer);
			if (this.connectParent != null && this.connectParent.parent.def != ThingDefOf.HiddenConduit)
			{
				PowerNetGraphics.PrintWirePieceConnecting(layer, this.parent, this.connectParent.parent, false);
			}
		}

		// Token: 0x0600B981 RID: 47489 RVA: 0x00358630 File Offset: 0x00356830
		public override void CompPrintForPowerGrid(SectionLayer layer)
		{
			if (this.TransmitsPowerNow)
			{
				PowerOverlayMats.LinkedOverlayGraphic.Print(layer, this.parent, 0f);
			}
			Thing thing = this.parent;
			Building building = this.parent as Building;
			if (building != null && building.def.building.isAttachment)
			{
				thing = (GenConstruct.GetWallAttachedTo(this.parent) ?? this.parent);
			}
			if (this.parent.def.ConnectToPower)
			{
				PowerNetGraphics.PrintOverlayConnectorBaseFor(layer, thing);
			}
			if (this.connectParent != null)
			{
				PowerNetGraphics.PrintWirePieceConnecting(layer, thing, this.connectParent.parent, true);
			}
		}

		// Token: 0x0600B982 RID: 47490 RVA: 0x003586CD File Offset: 0x003568CD
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.connectParent != null && this.parent.Faction == Faction.OfPlayer)
			{
				yield return new Command_Action
				{
					action = delegate()
					{
						SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
						this.TryManualReconnect(true);
					},
					hotKey = KeyBindingDefOf.Misc2,
					defaultDesc = "CommandTryReconnectDesc".Translate(),
					icon = ContentFinder<Texture2D>.Get("UI/Commands/TryReconnect", true),
					defaultLabel = "CommandTryReconnectLabel".Translate()
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0600B983 RID: 47491 RVA: 0x003586E0 File Offset: 0x003568E0
		public void TryManualReconnect(bool dispayVisuals = true)
		{
			if (CompPower.lastManualReconnector != this)
			{
				CompPower.recentlyConnectedNets.Clear();
				CompPower.lastManualReconnector = this;
			}
			if (this.PowerNet != null)
			{
				CompPower.recentlyConnectedNets.Add(this.PowerNet);
			}
			IntVec3 connectorPos = this.parent.def.building.isAttachment ? GenConstruct.GetWallAttachedTo(this.parent).Position : this.parent.Position;
			CompPower compPower = PowerConnectionMaker.BestTransmitterForConnector(connectorPos, this.parent.Map, CompPower.recentlyConnectedNets);
			if (compPower == null)
			{
				CompPower.recentlyConnectedNets.Clear();
				compPower = PowerConnectionMaker.BestTransmitterForConnector(connectorPos, this.parent.Map, null);
			}
			if (compPower != null)
			{
				PowerConnectionMaker.DisconnectFromPowerNet(this);
				this.ConnectToTransmitter(compPower, false);
				if (dispayVisuals)
				{
					for (int i = 0; i < 5; i++)
					{
						FleckMaker.ThrowMetaPuff(compPower.parent.Position.ToVector3Shifted(), compPower.parent.Map);
					}
				}
				this.parent.Map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlagDefOf.PowerGrid);
				this.parent.Map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlagDefOf.Things);
			}
		}

		// Token: 0x0600B984 RID: 47492 RVA: 0x00358824 File Offset: 0x00356A24
		public void ConnectToTransmitter(CompPower transmitter, bool reconnectingAfterLoading = false)
		{
			if (this.connectParent != null && (!reconnectingAfterLoading || this.connectParent != transmitter))
			{
				string[] array = new string[7];
				array[0] = "Tried to connect ";
				array[1] = ((this != null) ? this.ToString() : null);
				array[2] = " to transmitter ";
				array[3] = ((transmitter != null) ? transmitter.ToString() : null);
				array[4] = " but it's already connected to ";
				int num = 5;
				CompPower compPower = this.connectParent;
				array[num] = ((compPower != null) ? compPower.ToString() : null);
				array[6] = ".";
				Log.Error(string.Concat(array));
				return;
			}
			this.connectParent = transmitter;
			if (this.connectParent.connectChildren == null)
			{
				this.connectParent.connectChildren = new List<CompPower>();
			}
			transmitter.connectChildren.Add(this);
			PowerNet powerNet = this.PowerNet;
			if (powerNet != null)
			{
				powerNet.RegisterConnector(this);
			}
		}

		// Token: 0x0600B985 RID: 47493 RVA: 0x003588EC File Offset: 0x00356AEC
		public override string CompInspectStringExtra()
		{
			if (this.PowerNet == null)
			{
				return "PowerNotConnected".Translate();
			}
			string value = (this.PowerNet.CurrentEnergyGainRate() / CompPower.WattsToWattDaysPerTick).ToString("F0");
			string value2 = this.PowerNet.CurrentStoredEnergy().ToString("F0");
			return "PowerConnectedRateStored".Translate(value, value2);
		}

		// Token: 0x04007FE1 RID: 32737
		public PowerNet transNet;

		// Token: 0x04007FE2 RID: 32738
		public CompPower connectParent;

		// Token: 0x04007FE3 RID: 32739
		public List<CompPower> connectChildren;

		// Token: 0x04007FE4 RID: 32740
		private static List<PowerNet> recentlyConnectedNets = new List<PowerNet>();

		// Token: 0x04007FE5 RID: 32741
		private static CompPower lastManualReconnector = null;

		// Token: 0x04007FE6 RID: 32742
		public static readonly float WattsToWattDaysPerTick = 1.6666667E-05f;
	}
}
