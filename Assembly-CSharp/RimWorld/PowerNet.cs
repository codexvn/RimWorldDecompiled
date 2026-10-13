using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200214F RID: 8527
	public class PowerNet
	{
		// Token: 0x17001C5F RID: 7263
		// (get) Token: 0x0600BA51 RID: 47697 RVA: 0x0035BE1B File Offset: 0x0035A01B
		public Map Map
		{
			get
			{
				return this.powerNetManager.map;
			}
		}

		// Token: 0x17001C60 RID: 7264
		// (get) Token: 0x0600BA52 RID: 47698 RVA: 0x0035BE28 File Offset: 0x0035A028
		public bool HasActivePowerSource
		{
			get
			{
				if (!this.hasPowerSource)
				{
					return false;
				}
				for (int i = 0; i < this.transmitters.Count; i++)
				{
					if (this.IsActivePowerSource(this.transmitters[i]))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x0600BA53 RID: 47699 RVA: 0x0035BE6C File Offset: 0x0035A06C
		public PowerNet(IEnumerable<CompPower> newTransmitters)
		{
			foreach (CompPower compPower in newTransmitters)
			{
				this.transmitters.Add(compPower);
				compPower.transNet = this;
				this.RegisterAllComponentsOf(compPower.parent);
				if (compPower.connectChildren != null)
				{
					List<CompPower> connectChildren = compPower.connectChildren;
					for (int i = 0; i < connectChildren.Count; i++)
					{
						this.RegisterConnector(connectChildren[i]);
					}
				}
			}
			this.hasPowerSource = false;
			for (int j = 0; j < this.transmitters.Count; j++)
			{
				if (this.IsPowerSource(this.transmitters[j]))
				{
					this.hasPowerSource = true;
					return;
				}
			}
		}

		// Token: 0x0600BA54 RID: 47700 RVA: 0x0035BF74 File Offset: 0x0035A174
		private bool IsPowerSource(CompPower cp)
		{
			return cp is CompPowerBattery || (cp is CompPowerTrader && cp.Props.PowerConsumption < 0f);
		}

		// Token: 0x0600BA55 RID: 47701 RVA: 0x0035BFA0 File Offset: 0x0035A1A0
		private bool IsActivePowerSource(CompPower cp)
		{
			CompPowerBattery compPowerBattery = cp as CompPowerBattery;
			if (compPowerBattery != null && compPowerBattery.StoredEnergy > 0f && !compPowerBattery.StunnedByEMP)
			{
				return true;
			}
			CompPowerTrader compPowerTrader = cp as CompPowerTrader;
			return compPowerTrader != null && compPowerTrader.PowerOutput > 0f;
		}

		// Token: 0x0600BA56 RID: 47702 RVA: 0x0035BFE8 File Offset: 0x0035A1E8
		public void RegisterConnector(CompPower b)
		{
			if (this.connectors.Contains(b))
			{
				Log.Error("PowerNet registered connector it already had: " + ((b != null) ? b.ToString() : null));
				return;
			}
			this.connectors.Add(b);
			this.RegisterAllComponentsOf(b.parent);
		}

		// Token: 0x0600BA57 RID: 47703 RVA: 0x0035C038 File Offset: 0x0035A238
		public void DeregisterConnector(CompPower b)
		{
			this.connectors.Remove(b);
			this.DeregisterAllComponentsOf(b.parent);
		}

		// Token: 0x0600BA58 RID: 47704 RVA: 0x0035C054 File Offset: 0x0035A254
		private void RegisterAllComponentsOf(ThingWithComps parentThing)
		{
			CompPowerTrader comp = parentThing.GetComp<CompPowerTrader>();
			if (comp != null)
			{
				if (this.powerComps.Contains(comp))
				{
					string str = "PowerNet adding powerComp ";
					CompPowerTrader compPowerTrader = comp;
					Log.Error(str + ((compPowerTrader != null) ? compPowerTrader.ToString() : null) + " which it already has.");
				}
				else
				{
					this.powerComps.Add(comp);
				}
			}
			CompPowerBattery comp2 = parentThing.GetComp<CompPowerBattery>();
			if (comp2 != null)
			{
				if (this.batteryComps.Contains(comp2))
				{
					string str2 = "PowerNet adding batteryComp ";
					CompPowerBattery compPowerBattery = comp2;
					Log.Error(str2 + ((compPowerBattery != null) ? compPowerBattery.ToString() : null) + " which it already has.");
					return;
				}
				this.batteryComps.Add(comp2);
			}
		}

		// Token: 0x0600BA59 RID: 47705 RVA: 0x0035C0F0 File Offset: 0x0035A2F0
		private void DeregisterAllComponentsOf(ThingWithComps parentThing)
		{
			CompPowerTrader comp = parentThing.GetComp<CompPowerTrader>();
			if (comp != null)
			{
				this.powerComps.Remove(comp);
			}
			CompPowerBattery comp2 = parentThing.GetComp<CompPowerBattery>();
			if (comp2 != null)
			{
				this.batteryComps.Remove(comp2);
			}
		}

		// Token: 0x0600BA5A RID: 47706 RVA: 0x0035C12C File Offset: 0x0035A32C
		public float CurrentEnergyGainRate()
		{
			if (DebugSettings.unlimitedPower)
			{
				return 100000f;
			}
			float num = 0f;
			for (int i = 0; i < this.powerComps.Count; i++)
			{
				if (this.powerComps[i].PowerOn)
				{
					num += this.powerComps[i].EnergyOutputPerTick;
				}
			}
			return num;
		}

		// Token: 0x0600BA5B RID: 47707 RVA: 0x0035C18C File Offset: 0x0035A38C
		public float CurrentStoredEnergy()
		{
			float num = 0f;
			for (int i = 0; i < this.batteryComps.Count; i++)
			{
				num += (this.batteryComps[i].StunnedByEMP ? 0f : this.batteryComps[i].StoredEnergy);
			}
			return num;
		}

		// Token: 0x0600BA5C RID: 47708 RVA: 0x0035C1E4 File Offset: 0x0035A3E4
		public void PowerNetTick()
		{
			float num = this.CurrentEnergyGainRate();
			float num2 = this.CurrentStoredEnergy();
			if (num2 + num >= -1E-07f && !this.Map.gameConditionManager.ElectricityDisabled(this.Map))
			{
				float num3;
				if (this.batteryComps.Count > 0 && num2 >= 0.1f)
				{
					num3 = num2 - 5f;
				}
				else
				{
					num3 = num2;
				}
				if (num3 + num >= 0f)
				{
					PowerNet.partsWantingPowerOn.Clear();
					for (int i = 0; i < this.powerComps.Count; i++)
					{
						if (!this.powerComps[i].PowerOn && FlickUtility.WantsToBeOn(this.powerComps[i].parent) && !this.powerComps[i].parent.IsBrokenDown())
						{
							PowerNet.partsWantingPowerOn.Add(this.powerComps[i]);
						}
					}
					if (PowerNet.partsWantingPowerOn.Count > 0)
					{
						int num4 = 200 / PowerNet.partsWantingPowerOn.Count;
						if (num4 < 30)
						{
							num4 = 30;
						}
						if (Find.TickManager.TicksGame % num4 == 0)
						{
							int num5 = Mathf.Max(1, Mathf.RoundToInt((float)PowerNet.partsWantingPowerOn.Count * 0.05f));
							for (int j = 0; j < num5; j++)
							{
								CompPowerTrader compPowerTrader = PowerNet.partsWantingPowerOn.RandomElement<CompPowerTrader>();
								if (!compPowerTrader.PowerOn && num + num2 >= -(compPowerTrader.EnergyOutputPerTick + 1E-07f))
								{
									compPowerTrader.PowerOn = true;
									num += compPowerTrader.EnergyOutputPerTick;
								}
							}
						}
					}
				}
				this.ChangeStoredEnergy(num);
				return;
			}
			if (Find.TickManager.TicksGame % 20 == 0)
			{
				PowerNet.potentialShutdownParts.Clear();
				for (int k = 0; k < this.powerComps.Count; k++)
				{
					if (this.powerComps[k].PowerOn && this.powerComps[k].EnergyOutputPerTick < 0f)
					{
						PowerNet.potentialShutdownParts.Add(this.powerComps[k]);
					}
				}
				if (PowerNet.potentialShutdownParts.Count > 0)
				{
					int num6 = Mathf.Max(1, Mathf.RoundToInt((float)PowerNet.potentialShutdownParts.Count * 0.05f));
					for (int l = 0; l < num6; l++)
					{
						PowerNet.potentialShutdownParts.RandomElement<CompPowerTrader>().PowerOn = false;
					}
				}
			}
		}

		// Token: 0x0600BA5D RID: 47709 RVA: 0x0035C444 File Offset: 0x0035A644
		public bool CanPowerNow(CompPowerTrader powerTrader)
		{
			float num = this.CurrentEnergyGainRate();
			float num2 = this.CurrentStoredEnergy();
			return num + num2 >= -(powerTrader.EnergyOutputPerTick + 1E-07f);
		}

		// Token: 0x0600BA5E RID: 47710 RVA: 0x0035C474 File Offset: 0x0035A674
		private void ChangeStoredEnergy(float extra)
		{
			if (extra > 0f)
			{
				this.DistributeEnergyAmongBatteries(extra);
				return;
			}
			float num = -extra;
			this.givingBats.Clear();
			for (int i = 0; i < this.batteryComps.Count; i++)
			{
				if (this.batteryComps[i].StoredEnergy > 1E-07f)
				{
					this.givingBats.Add(this.batteryComps[i]);
				}
			}
			float a = num / (float)this.givingBats.Count;
			int num2 = 0;
			while (num > 1E-07f)
			{
				for (int j = 0; j < this.givingBats.Count; j++)
				{
					float num3 = Mathf.Min(a, this.givingBats[j].StoredEnergy);
					this.givingBats[j].DrawPower(num3);
					num -= num3;
					if (num < 1E-07f)
					{
						return;
					}
				}
				num2++;
				if (num2 > 10)
				{
					break;
				}
			}
			if (num > 1E-07f)
			{
				Log.Warning("Drew energy from a PowerNet that didn't have it.");
			}
		}

		// Token: 0x0600BA5F RID: 47711 RVA: 0x0035C570 File Offset: 0x0035A770
		private void DistributeEnergyAmongBatteries(float energy)
		{
			if (energy <= 0f || !this.batteryComps.Any<CompPowerBattery>())
			{
				return;
			}
			PowerNet.batteriesShuffled.Clear();
			PowerNet.batteriesShuffled.AddRange(this.batteryComps);
			PowerNet.batteriesShuffled.Shuffle<CompPowerBattery>();
			int num = 0;
			for (;;)
			{
				num++;
				if (num > 10000)
				{
					break;
				}
				float num2 = float.MaxValue;
				for (int i = 0; i < PowerNet.batteriesShuffled.Count; i++)
				{
					num2 = Mathf.Min(num2, PowerNet.batteriesShuffled[i].AmountCanAccept);
				}
				if (energy < num2 * (float)PowerNet.batteriesShuffled.Count)
				{
					goto IL_100;
				}
				for (int j = PowerNet.batteriesShuffled.Count - 1; j >= 0; j--)
				{
					float amountCanAccept = PowerNet.batteriesShuffled[j].AmountCanAccept;
					bool flag = amountCanAccept <= 0f || amountCanAccept == num2;
					if (num2 > 0f)
					{
						PowerNet.batteriesShuffled[j].AddEnergy(num2);
						energy -= num2;
					}
					if (flag)
					{
						PowerNet.batteriesShuffled.RemoveAt(j);
					}
				}
				if (energy < 0.0005f || !PowerNet.batteriesShuffled.Any<CompPowerBattery>())
				{
					goto IL_15B;
				}
			}
			Log.Error("Too many iterations.");
			goto IL_15B;
			IL_100:
			float amount = energy / (float)PowerNet.batteriesShuffled.Count;
			for (int k = 0; k < PowerNet.batteriesShuffled.Count; k++)
			{
				PowerNet.batteriesShuffled[k].AddEnergy(amount);
			}
			energy = 0f;
			IL_15B:
			PowerNet.batteriesShuffled.Clear();
		}

		// Token: 0x0600BA60 RID: 47712 RVA: 0x0035C6E4 File Offset: 0x0035A8E4
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("POWERNET:");
			stringBuilder.AppendLine("  Created energy: " + this.debugLastCreatedEnergy.ToString());
			stringBuilder.AppendLine("  Raw stored energy: " + this.debugLastRawStoredEnergy.ToString());
			stringBuilder.AppendLine("  Apparent stored energy: " + this.debugLastApparentStoredEnergy.ToString());
			stringBuilder.AppendLine("  hasPowerSource: " + this.hasPowerSource.ToString());
			stringBuilder.AppendLine("  Connectors: ");
			foreach (CompPower compPower in this.connectors)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				string str = "      ";
				ThingWithComps parent = compPower.parent;
				stringBuilder2.AppendLine(str + ((parent != null) ? parent.ToString() : null));
			}
			stringBuilder.AppendLine("  Transmitters: ");
			foreach (CompPower compPower2 in this.transmitters)
			{
				StringBuilder stringBuilder3 = stringBuilder;
				string str2 = "      ";
				ThingWithComps parent2 = compPower2.parent;
				stringBuilder3.AppendLine(str2 + ((parent2 != null) ? parent2.ToString() : null));
			}
			stringBuilder.AppendLine("  powerComps: ");
			foreach (CompPowerTrader compPowerTrader in this.powerComps)
			{
				StringBuilder stringBuilder4 = stringBuilder;
				string str3 = "      ";
				ThingWithComps parent3 = compPowerTrader.parent;
				stringBuilder4.AppendLine(str3 + ((parent3 != null) ? parent3.ToString() : null));
			}
			stringBuilder.AppendLine("  batteryComps: ");
			foreach (CompPowerBattery compPowerBattery in this.batteryComps)
			{
				StringBuilder stringBuilder5 = stringBuilder;
				string str4 = "      ";
				ThingWithComps parent4 = compPowerBattery.parent;
				stringBuilder5.AppendLine(str4 + ((parent4 != null) ? parent4.ToString() : null));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400806C RID: 32876
		public PowerNetManager powerNetManager;

		// Token: 0x0400806D RID: 32877
		public bool hasPowerSource;

		// Token: 0x0400806E RID: 32878
		public List<CompPower> connectors = new List<CompPower>();

		// Token: 0x0400806F RID: 32879
		public List<CompPower> transmitters = new List<CompPower>();

		// Token: 0x04008070 RID: 32880
		public List<CompPowerTrader> powerComps = new List<CompPowerTrader>();

		// Token: 0x04008071 RID: 32881
		public List<CompPowerBattery> batteryComps = new List<CompPowerBattery>();

		// Token: 0x04008072 RID: 32882
		private float debugLastCreatedEnergy;

		// Token: 0x04008073 RID: 32883
		private float debugLastRawStoredEnergy;

		// Token: 0x04008074 RID: 32884
		private float debugLastApparentStoredEnergy;

		// Token: 0x04008075 RID: 32885
		private const int MaxRestartTryInterval = 200;

		// Token: 0x04008076 RID: 32886
		private const int MinRestartTryInterval = 30;

		// Token: 0x04008077 RID: 32887
		private const float RestartMinFraction = 0.05f;

		// Token: 0x04008078 RID: 32888
		private const int ShutdownInterval = 20;

		// Token: 0x04008079 RID: 32889
		private const float ShutdownMinFraction = 0.05f;

		// Token: 0x0400807A RID: 32890
		private const float MinStoredEnergyToTurnOn = 5f;

		// Token: 0x0400807B RID: 32891
		private static List<CompPowerTrader> partsWantingPowerOn = new List<CompPowerTrader>();

		// Token: 0x0400807C RID: 32892
		private static List<CompPowerTrader> potentialShutdownParts = new List<CompPowerTrader>();

		// Token: 0x0400807D RID: 32893
		private List<CompPowerBattery> givingBats = new List<CompPowerBattery>();

		// Token: 0x0400807E RID: 32894
		private static List<CompPowerBattery> batteriesShuffled = new List<CompPowerBattery>();
	}
}
