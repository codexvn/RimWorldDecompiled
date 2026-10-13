using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200246B RID: 9323
	public class JoyToleranceSet : IExposable
	{
		// Token: 0x17001E92 RID: 7826
		public float this[JoyKindDef d]
		{
			get
			{
				return this.tolerances[d];
			}
		}

		// Token: 0x0600CA39 RID: 51769 RVA: 0x003B381D File Offset: 0x003B1A1D
		public void ExposeData()
		{
			Scribe_Deep.Look<DefMap<JoyKindDef, float>>(ref this.tolerances, "tolerances", Array.Empty<object>());
			Scribe_Deep.Look<DefMap<JoyKindDef, bool>>(ref this.bored, "bored", Array.Empty<object>());
			if (this.bored == null)
			{
				this.bored = new DefMap<JoyKindDef, bool>();
			}
		}

		// Token: 0x0600CA3A RID: 51770 RVA: 0x003B385C File Offset: 0x003B1A5C
		public bool BoredOf(JoyKindDef def)
		{
			return this.bored[def];
		}

		// Token: 0x0600CA3B RID: 51771 RVA: 0x003B386C File Offset: 0x003B1A6C
		public void Notify_JoyGained(float amount, JoyKindDef joyKind)
		{
			float num = Mathf.Min(this.tolerances[joyKind] + amount * 0.65f, 1f);
			this.tolerances[joyKind] = num;
			if (num > 0.5f)
			{
				this.bored[joyKind] = true;
			}
		}

		// Token: 0x0600CA3C RID: 51772 RVA: 0x003B38BA File Offset: 0x003B1ABA
		public float JoyFactorFromTolerance(JoyKindDef joyKind)
		{
			return 1f - this.tolerances[joyKind];
		}

		// Token: 0x0600CA3D RID: 51773 RVA: 0x003B38D0 File Offset: 0x003B1AD0
		public void NeedInterval(Pawn pawn)
		{
			float num = ExpectationsUtility.CurrentExpectationFor(pawn).joyToleranceDropPerDay * 150f / 60000f;
			for (int i = 0; i < this.tolerances.Count; i++)
			{
				float num2 = this.tolerances[i];
				num2 -= num;
				if (num2 < 0f)
				{
					num2 = 0f;
				}
				this.tolerances[i] = num2;
				if (this.bored[i] && num2 < 0.3f)
				{
					this.bored[i] = false;
				}
			}
		}

		// Token: 0x0600CA3E RID: 51774 RVA: 0x003B395C File Offset: 0x003B1B5C
		public string TolerancesString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			List<JoyKindDef> allDefsListForReading = DefDatabase<JoyKindDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				JoyKindDef joyKindDef = allDefsListForReading[i];
				float num = this.tolerances[joyKindDef];
				if (num > 0.01f)
				{
					if (stringBuilder.Length == 0)
					{
						stringBuilder.AppendLine("JoyTolerances".Translate() + ":");
					}
					string text = "   " + joyKindDef.LabelCap + ": " + num.ToStringPercent();
					if (this.bored[joyKindDef])
					{
						text += " (" + "bored".Translate() + ")";
					}
					stringBuilder.AppendLine(text);
				}
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x0600CA3F RID: 51775 RVA: 0x003B3A54 File Offset: 0x003B1C54
		public bool BoredOfKinds(List<JoyKindDef> kindsAvailable)
		{
			for (int i = 0; i < kindsAvailable.Count; i++)
			{
				if (!this.bored[kindsAvailable[i]])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600CA40 RID: 51776 RVA: 0x003B3A8C File Offset: 0x003B1C8C
		public bool BoredOfAllAvailableJoyKinds(Pawn pawn)
		{
			List<JoyKindDef> list = JoyUtility.JoyKindsOnMapTempList(pawn.MapHeld);
			bool result = true;
			for (int i = 0; i < list.Count; i++)
			{
				if (!this.bored[list[i]])
				{
					result = false;
					break;
				}
			}
			list.Clear();
			return result;
		}

		// Token: 0x0400899D RID: 35229
		private DefMap<JoyKindDef, float> tolerances = new DefMap<JoyKindDef, float>();

		// Token: 0x0400899E RID: 35230
		private DefMap<JoyKindDef, bool> bored = new DefMap<JoyKindDef, bool>();
	}
}
