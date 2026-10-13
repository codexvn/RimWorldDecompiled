using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200212D RID: 8493
	public class CompPowerBattery : CompPower
	{
		// Token: 0x17001C1C RID: 7196
		// (get) Token: 0x0600B92F RID: 47407 RVA: 0x00357474 File Offset: 0x00355674
		public float AmountCanAccept
		{
			get
			{
				if (this.parent.IsBrokenDown() || this.StunnedByEMP)
				{
					return 0f;
				}
				CompProperties_Battery props = this.Props;
				return (props.storedEnergyMax - this.storedEnergy) / props.efficiency;
			}
		}

		// Token: 0x17001C1D RID: 7197
		// (get) Token: 0x0600B930 RID: 47408 RVA: 0x003574B7 File Offset: 0x003556B7
		public float StoredEnergy
		{
			get
			{
				return this.storedEnergy;
			}
		}

		// Token: 0x17001C1E RID: 7198
		// (get) Token: 0x0600B931 RID: 47409 RVA: 0x003574BF File Offset: 0x003556BF
		public float StoredEnergyPct
		{
			get
			{
				return this.storedEnergy / this.Props.storedEnergyMax;
			}
		}

		// Token: 0x17001C1F RID: 7199
		// (get) Token: 0x0600B932 RID: 47410 RVA: 0x003574D3 File Offset: 0x003556D3
		public new CompProperties_Battery Props
		{
			get
			{
				return (CompProperties_Battery)this.props;
			}
		}

		// Token: 0x17001C20 RID: 7200
		// (get) Token: 0x0600B933 RID: 47411 RVA: 0x003574E0 File Offset: 0x003556E0
		public bool StunnedByEMP
		{
			get
			{
				return this.stunnableComp != null && this.stunnableComp.StunHandler.Stunned && this.stunnableComp.StunHandler.StunFromEMP;
			}
		}

		// Token: 0x0600B934 RID: 47412 RVA: 0x00357510 File Offset: 0x00355710
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.stunnableComp = this.parent.GetComp<CompStunnable>();
		}

		// Token: 0x0600B935 RID: 47413 RVA: 0x0035752C File Offset: 0x0035572C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.storedEnergy, "storedPower", 0f, false);
			CompProperties_Battery props = this.Props;
			if (this.storedEnergy > props.storedEnergyMax)
			{
				this.storedEnergy = props.storedEnergyMax;
			}
		}

		// Token: 0x0600B936 RID: 47414 RVA: 0x00357576 File Offset: 0x00355776
		public override void CompTick()
		{
			base.CompTick();
			this.DrawPower(Mathf.Min(5f * CompPower.WattsToWattDaysPerTick, this.storedEnergy));
		}

		// Token: 0x0600B937 RID: 47415 RVA: 0x0035759C File Offset: 0x0035579C
		public void AddEnergy(float amount)
		{
			if (amount < 0f)
			{
				Log.Error("Cannot add negative energy " + amount.ToString());
				return;
			}
			if (this.StunnedByEMP)
			{
				return;
			}
			if (amount > this.AmountCanAccept)
			{
				amount = this.AmountCanAccept;
			}
			amount *= this.Props.efficiency;
			this.storedEnergy += amount;
		}

		// Token: 0x0600B938 RID: 47416 RVA: 0x00357600 File Offset: 0x00355800
		public void DrawPower(float amount)
		{
			this.storedEnergy -= amount;
			if (this.storedEnergy < 0f)
			{
				string str = "Drawing power we don't have from ";
				ThingWithComps parent = this.parent;
				Log.Error(str + ((parent != null) ? parent.ToString() : null));
				this.storedEnergy = 0f;
			}
		}

		// Token: 0x0600B939 RID: 47417 RVA: 0x00357654 File Offset: 0x00355854
		public void SetStoredEnergyPct(float pct)
		{
			pct = Mathf.Clamp01(pct);
			this.storedEnergy = this.Props.storedEnergyMax * pct;
		}

		// Token: 0x0600B93A RID: 47418 RVA: 0x00357671 File Offset: 0x00355871
		public override void ReceiveCompSignal(string signal)
		{
			if (signal == "Breakdown")
			{
				this.DrawPower(this.StoredEnergy);
			}
		}

		// Token: 0x0600B93B RID: 47419 RVA: 0x0035768C File Offset: 0x0035588C
		public override string CompInspectStringExtra()
		{
			CompProperties_Battery props = this.Props;
			string text = "PowerBatteryStored".Translate() + ": " + this.storedEnergy.ToString("F0") + " / " + props.storedEnergyMax.ToString("F0") + " Wd";
			text += "\n" + "PowerBatteryEfficiency".Translate() + ": " + (props.efficiency * 100f).ToString("F0") + "%";
			if (this.storedEnergy > 0f)
			{
				text += "\n" + "SelfDischarging".Translate() + ": " + 5f.ToString("F0") + " W";
			}
			return text + "\n" + base.CompInspectStringExtra();
		}

		// Token: 0x0600B93C RID: 47420 RVA: 0x003577B5 File Offset: 0x003559B5
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Fill",
					action = delegate()
					{
						this.SetStoredEnergyPct(1f);
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Empty",
					action = delegate()
					{
						this.SetStoredEnergyPct(0f);
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x04007FC6 RID: 32710
		private float storedEnergy;

		// Token: 0x04007FC7 RID: 32711
		private CompStunnable stunnableComp;

		// Token: 0x04007FC8 RID: 32712
		private const float SelfDischargingWatts = 5f;
	}
}
