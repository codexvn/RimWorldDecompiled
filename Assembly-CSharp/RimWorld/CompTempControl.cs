using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002E7A RID: 11898
	public class CompTempControl : ThingComp
	{
		// Token: 0x17002B01 RID: 11009
		// (get) Token: 0x06010C09 RID: 68617 RVA: 0x004D3DFB File Offset: 0x004D1FFB
		// (set) Token: 0x06010C0A RID: 68618 RVA: 0x004D3E03 File Offset: 0x004D2003
		public virtual float TargetTemperature
		{
			get
			{
				return this.targetTemperature;
			}
			set
			{
				this.targetTemperature = value;
			}
		}

		// Token: 0x17002B02 RID: 11010
		// (get) Token: 0x06010C0B RID: 68619 RVA: 0x004D3E0C File Offset: 0x004D200C
		public CompProperties_TempControl Props
		{
			get
			{
				return (CompProperties_TempControl)this.props;
			}
		}

		// Token: 0x17002B03 RID: 11011
		// (get) Token: 0x06010C0C RID: 68620 RVA: 0x004D3E1C File Offset: 0x004D201C
		public CompPowerTrader PowerTrader
		{
			get
			{
				CompPowerTrader result;
				if ((result = this.intPowerTrader) == null)
				{
					result = (this.intPowerTrader = this.parent.GetComp<CompPowerTrader>());
				}
				return result;
			}
		}

		// Token: 0x06010C0D RID: 68621 RVA: 0x004D3E47 File Offset: 0x004D2047
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (this.TargetTemperature < -2000f)
			{
				this.TargetTemperature = this.Props.defaultTargetTemperature;
			}
		}

		// Token: 0x06010C0E RID: 68622 RVA: 0x004D3E6E File Offset: 0x004D206E
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.targetTemperature, "targetTemperature", 0f, false);
		}

		// Token: 0x06010C0F RID: 68623 RVA: 0x004D3E8C File Offset: 0x004D208C
		protected float RoundedToCurrentTempModeOffset(float celsiusTemp)
		{
			return GenTemperature.ConvertTemperatureOffset((float)Mathf.RoundToInt(GenTemperature.CelsiusToOffset(celsiusTemp, Prefs.TemperatureMode)), Prefs.TemperatureMode, TemperatureDisplayMode.Celsius);
		}

		// Token: 0x06010C10 RID: 68624 RVA: 0x004D3EAA File Offset: 0x004D20AA
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			float offset2 = this.RoundedToCurrentTempModeOffset(-10f);
			yield return new Command_Action
			{
				action = delegate()
				{
					this.InterfaceChangeTargetTemperature(offset2);
				},
				defaultLabel = offset2.ToStringTemperatureOffset("F0"),
				defaultDesc = "CommandLowerTempDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc5,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempLower", true)
			};
			float offset3 = this.RoundedToCurrentTempModeOffset(-1f);
			yield return new Command_Action
			{
				action = delegate()
				{
					this.InterfaceChangeTargetTemperature(offset3);
				},
				defaultLabel = offset3.ToStringTemperatureOffset("F0"),
				defaultDesc = "CommandLowerTempDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc4,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempLower", true)
			};
			yield return new Command_Action
			{
				action = delegate()
				{
					this.TargetTemperature = 21f;
					SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
					this.ThrowCurrentTemperatureText();
				},
				defaultLabel = "CommandResetTemp".Translate(),
				defaultDesc = "CommandResetTempDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc1,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempReset", true)
			};
			float offset4 = this.RoundedToCurrentTempModeOffset(1f);
			yield return new Command_Action
			{
				action = delegate()
				{
					this.InterfaceChangeTargetTemperature(offset4);
				},
				defaultLabel = "+" + offset4.ToStringTemperatureOffset("F0"),
				defaultDesc = "CommandRaiseTempDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc2,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempRaise", true)
			};
			float offset = this.RoundedToCurrentTempModeOffset(10f);
			yield return new Command_Action
			{
				action = delegate()
				{
					this.InterfaceChangeTargetTemperature(offset);
				},
				defaultLabel = "+" + offset.ToStringTemperatureOffset("F0"),
				defaultDesc = "CommandRaiseTempDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc3,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempRaise", true)
			};
			yield break;
			yield break;
		}

		// Token: 0x06010C11 RID: 68625 RVA: 0x004D3EBA File Offset: 0x004D20BA
		protected void InterfaceChangeTargetTemperature(float offset)
		{
			SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
			this.TargetTemperature += offset;
			this.TargetTemperature = Mathf.Clamp(this.TargetTemperature, -273.15f, 1000f);
			this.ThrowCurrentTemperatureText();
		}

		// Token: 0x06010C12 RID: 68626 RVA: 0x004D3EF8 File Offset: 0x004D20F8
		protected void ThrowCurrentTemperatureText()
		{
			MoteMaker.ThrowText(this.parent.TrueCenter() + new Vector3(0.5f, 0f, 0.5f), this.parent.Map, this.TargetTemperature.ToStringTemperature("F0"), Color.white, -1f);
		}

		// Token: 0x06010C13 RID: 68627 RVA: 0x004D3F54 File Offset: 0x004D2154
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append((this.Props.inspectString ?? "TargetTemperature".Translate()) + ": ");
			stringBuilder.AppendLine(this.TargetTemperature.ToStringTemperature("F0"));
			if (this.PowerTrader != null)
			{
				string str;
				if (this.PowerTrader.Off)
				{
					str = "PowerConsumptionOff".Translate();
				}
				else if (!this.operatingAtHighPower)
				{
					str = "PowerConsumptionLow".Translate();
				}
				else
				{
					str = "PowerConsumptionHigh".Translate();
				}
				stringBuilder.Append(string.Format("{0}: {1}", "PowerConsumptionMode".Translate(), str.CapitalizeFirst()));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400AE8C RID: 44684
		protected const float DefaultTargetTemperature = 21f;

		// Token: 0x0400AE8D RID: 44685
		public bool operatingAtHighPower;

		// Token: 0x0400AE8E RID: 44686
		private CompPowerTrader intPowerTrader;

		// Token: 0x0400AE8F RID: 44687
		public float targetTemperature = -99999f;
	}
}
