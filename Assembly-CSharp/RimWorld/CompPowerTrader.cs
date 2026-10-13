using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002143 RID: 8515
	public class CompPowerTrader : CompPower, IThingGlower
	{
		// Token: 0x17001C47 RID: 7239
		// (get) Token: 0x0600B9F3 RID: 47603 RVA: 0x0035A9BF File Offset: 0x00358BBF
		// (set) Token: 0x0600B9F4 RID: 47604 RVA: 0x0035A9D5 File Offset: 0x00358BD5
		public float PowerOutput
		{
			get
			{
				if (!this.StunnedByEMP)
				{
					return this.powerOutputInt;
				}
				return 0f;
			}
			set
			{
				this.powerOutputInt = value;
				if (this.powerOutputInt > 0f)
				{
					this.powerLastOutputted = true;
				}
				if (this.powerOutputInt < 0f)
				{
					this.powerLastOutputted = false;
				}
			}
		}

		// Token: 0x17001C48 RID: 7240
		// (get) Token: 0x0600B9F5 RID: 47605 RVA: 0x0035AA06 File Offset: 0x00358C06
		public bool Off
		{
			get
			{
				return !this.parent.Spawned || !this.PowerOn || !FlickUtility.WantsToBeOn(this.parent);
			}
		}

		// Token: 0x17001C49 RID: 7241
		// (get) Token: 0x0600B9F6 RID: 47606 RVA: 0x0035AA2D File Offset: 0x00358C2D
		public float EnergyOutputPerTick
		{
			get
			{
				return this.PowerOutput * CompPower.WattsToWattDaysPerTick;
			}
		}

		// Token: 0x17001C4A RID: 7242
		// (get) Token: 0x0600B9F7 RID: 47607 RVA: 0x0035AA3B File Offset: 0x00358C3B
		// (set) Token: 0x0600B9F8 RID: 47608 RVA: 0x0035AA44 File Offset: 0x00358C44
		public bool PowerOn
		{
			get
			{
				return this.powerOnInt;
			}
			set
			{
				if (this.powerOnInt == value)
				{
					return;
				}
				this.powerOnInt = value;
				if (this.powerOnInt)
				{
					if (!FlickUtility.WantsToBeOn(this.parent))
					{
						string str = "Tried to power on ";
						ThingWithComps parent = this.parent;
						Log.Warning(str + ((parent != null) ? parent.ToString() : null) + " which did not desire it.");
						return;
					}
					if (this.parent.IsBrokenDown())
					{
						string str2 = "Tried to power on ";
						ThingWithComps parent2 = this.parent;
						Log.Warning(str2 + ((parent2 != null) ? parent2.ToString() : null) + " which is broken down.");
						return;
					}
					if (this.powerStartedAction != null)
					{
						this.powerStartedAction();
					}
					this.parent.BroadcastCompSignal("PowerTurnedOn");
					SoundDef soundDef = ((CompProperties_Power)this.parent.def.CompDefForAssignableFrom<CompPowerTrader>()).soundPowerOn;
					if (soundDef.NullOrUndefined())
					{
						soundDef = SoundDefOf.Power_OnSmall;
					}
					if (this.parent.Spawned)
					{
						soundDef.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
					}
					this.StartSustainerPoweredIfInactive();
				}
				else
				{
					if (this.powerStoppedAction != null)
					{
						this.powerStoppedAction();
					}
					this.parent.BroadcastCompSignal("PowerTurnedOff");
					SoundDef soundDef2 = ((CompProperties_Power)this.parent.def.CompDefForAssignableFrom<CompPowerTrader>()).soundPowerOff;
					if (soundDef2.NullOrUndefined())
					{
						soundDef2 = SoundDefOf.Power_OffSmall;
					}
					if (this.parent.Spawned)
					{
						soundDef2.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
					}
					this.EndSustainerPoweredIfActive();
				}
				this.UpdateOverlays();
			}
		}

		// Token: 0x17001C4B RID: 7243
		// (get) Token: 0x0600B9F9 RID: 47609 RVA: 0x0035ABEC File Offset: 0x00358DEC
		public string DebugString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(this.parent.LabelCap + " CompPower:");
				stringBuilder.AppendLine("   PowerOn: " + this.PowerOn.ToString());
				stringBuilder.AppendLine("   energyProduction: " + this.PowerOutput.ToString());
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17001C4C RID: 7244
		// (get) Token: 0x0600B9FA RID: 47610 RVA: 0x0035AC5D File Offset: 0x00358E5D
		private bool StunnedByEMP
		{
			get
			{
				return this.stunnableComp != null && this.stunnableComp.StunHandler.Stunned && this.stunnableComp.StunHandler.StunFromEMP;
			}
		}

		// Token: 0x0600B9FB RID: 47611 RVA: 0x0035AC8D File Offset: 0x00358E8D
		public bool ShouldBeLitNow()
		{
			return this.PowerOn;
		}

		// Token: 0x0600B9FC RID: 47612 RVA: 0x0035AC98 File Offset: 0x00358E98
		public override void ReceiveCompSignal(string signal)
		{
			if (signal == "FlickedOff" || signal == "ScheduledOff" || signal == "Breakdown" || signal == "AutoPoweredWantsOff")
			{
				this.PowerOn = false;
			}
			if (signal == "RanOutOfFuel" && this.powerLastOutputted)
			{
				this.PowerOn = false;
			}
			this.UpdateOverlays();
		}

		// Token: 0x0600B9FD RID: 47613 RVA: 0x0035AD04 File Offset: 0x00358F04
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.flickableComp = this.parent.GetComp<CompFlickable>();
			this.stunnableComp = this.parent.GetComp<CompStunnable>();
			if (this.PowerOn)
			{
				LongEventHandler.ExecuteWhenFinished(new Action(this.StartSustainerPoweredIfInactive));
			}
			this.UpdateOverlays();
		}

		// Token: 0x0600B9FE RID: 47614 RVA: 0x0035AD59 File Offset: 0x00358F59
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			base.PostDeSpawn(map, mode);
			this.EndSustainerPoweredIfActive();
			if (mode != DestroyMode.WillReplace)
			{
				this.powerOutputInt = 0f;
			}
		}

		// Token: 0x0600B9FF RID: 47615 RVA: 0x0035AD78 File Offset: 0x00358F78
		public override void PostSwapMap()
		{
			if (base.PowerNet == null)
			{
				this.PowerOn = false;
			}
		}

		// Token: 0x0600BA00 RID: 47616 RVA: 0x0035AD89 File Offset: 0x00358F89
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.powerOnInt, "powerOn", true, false);
		}

		// Token: 0x0600BA01 RID: 47617 RVA: 0x0035ADA4 File Offset: 0x00358FA4
		private void UpdateOverlays()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			this.parent.Map.overlayDrawer.Disable(this.parent, ref this.overlayPowerOff);
			this.parent.Map.overlayDrawer.Disable(this.parent, ref this.overlayNeedsPower);
			if (!this.parent.IsBrokenDown())
			{
				if (this.flickableComp != null && !this.flickableComp.SwitchIsOn && this.overlayPowerOff == null)
				{
					this.overlayPowerOff = new OverlayHandle?(this.parent.Map.overlayDrawer.Enable(this.parent, OverlayTypes.PowerOff));
					return;
				}
				if (FlickUtility.WantsToBeOn(this.parent) && !this.PowerOn && this.overlayNeedsPower == null && base.Props.showPowerNeededIfOff)
				{
					this.overlayNeedsPower = new OverlayHandle?(this.parent.Map.overlayDrawer.Enable(this.parent, OverlayTypes.NeedsPower));
				}
			}
		}

		// Token: 0x0600BA02 RID: 47618 RVA: 0x0035AEB4 File Offset: 0x003590B4
		public override void SetUpPowerVars()
		{
			base.SetUpPowerVars();
			CompProperties_Power props = base.Props;
			if (!this.PowerOn && !Mathf.Approximately(props.idlePowerDraw, -1f))
			{
				this.PowerOutput = -props.idlePowerDraw;
			}
			else
			{
				this.PowerOutput = -props.PowerConsumption;
			}
			this.powerLastOutputted = (props.PowerConsumption <= 0f);
		}

		// Token: 0x0600BA03 RID: 47619 RVA: 0x0035AF1C File Offset: 0x0035911C
		public override void ResetPowerVars()
		{
			base.ResetPowerVars();
			this.powerOnInt = false;
			this.powerOutputInt = 0f;
			this.powerLastOutputted = false;
			this.sustainerPowered = null;
			if (this.flickableComp != null && !this.parent.BeingTransportedOnGravship)
			{
				this.flickableComp.ResetToOn();
			}
		}

		// Token: 0x0600BA04 RID: 47620 RVA: 0x0035AF6F File Offset: 0x0035916F
		public override void LostConnectParent()
		{
			base.LostConnectParent();
			this.PowerOn = false;
		}

		// Token: 0x0600BA05 RID: 47621 RVA: 0x0035AF80 File Offset: 0x00359180
		public override string CompInspectStringExtra()
		{
			if (this.Off && (base.Props.idlePowerDraw > 0f || base.Props.alwaysDisplayAsUsingPower))
			{
				return null;
			}
			string text;
			if (this.powerLastOutputted && !base.Props.alwaysDisplayAsUsingPower)
			{
				text = "PowerOutput".Translate() + ": " + this.PowerOutput.ToString("#####0") + " W";
			}
			else
			{
				text = "PowerNeeded".Translate() + ": " + (-this.PowerOutput).ToString("#####0") + " W";
			}
			if (base.Props.idlePowerDraw > 0f || base.Props.alwaysDisplayAsUsingPower)
			{
				text += " (" + "PowerActiveNeeded".Translate(base.Props.PowerConsumption.ToString("#####0")) + ")";
			}
			return text + "\n" + base.CompInspectStringExtra();
		}

		// Token: 0x0600BA06 RID: 47622 RVA: 0x0035B0C0 File Offset: 0x003592C0
		private void StartSustainerPoweredIfInactive()
		{
			if (!base.Props.soundAmbientPowered.NullOrUndefined() && this.sustainerPowered == null)
			{
				SoundInfo info = SoundInfo.InMap(this.parent, MaintenanceType.None);
				this.sustainerPowered = base.Props.soundAmbientPowered.TrySpawnSustainer(info);
			}
		}

		// Token: 0x0600BA07 RID: 47623 RVA: 0x0035B110 File Offset: 0x00359310
		private void EndSustainerPoweredIfActive()
		{
			if (this.sustainerPowered != null)
			{
				this.sustainerPowered.End();
				this.sustainerPowered = null;
			}
		}

		// Token: 0x0600BA08 RID: 47624 RVA: 0x0035B12C File Offset: 0x0035932C
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (!DebugSettings.ShowDevGizmos)
			{
				yield break;
			}
			yield return new Command_Action
			{
				defaultLabel = "DEV: Toggle power on",
				action = delegate()
				{
					this.PowerOn = !this.PowerOn;
				}
			};
			yield break;
			yield break;
		}

		// Token: 0x0400803E RID: 32830
		public Action powerStartedAction;

		// Token: 0x0400803F RID: 32831
		public Action powerStoppedAction;

		// Token: 0x04008040 RID: 32832
		private bool powerOnInt;

		// Token: 0x04008041 RID: 32833
		public float powerOutputInt;

		// Token: 0x04008042 RID: 32834
		private bool powerLastOutputted;

		// Token: 0x04008043 RID: 32835
		private Sustainer sustainerPowered;

		// Token: 0x04008044 RID: 32836
		protected CompFlickable flickableComp;

		// Token: 0x04008045 RID: 32837
		private CompStunnable stunnableComp;

		// Token: 0x04008046 RID: 32838
		public const string PowerTurnedOnSignal = "PowerTurnedOn";

		// Token: 0x04008047 RID: 32839
		public const string PowerTurnedOffSignal = "PowerTurnedOff";

		// Token: 0x04008048 RID: 32840
		private OverlayHandle? overlayPowerOff;

		// Token: 0x04008049 RID: 32841
		private OverlayHandle? overlayNeedsPower;
	}
}
