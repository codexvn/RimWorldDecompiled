using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002ED8 RID: 11992
	public class CompDissolution : ThingComp
	{
		// Token: 0x17002BAC RID: 11180
		// (get) Token: 0x06010EE4 RID: 69348 RVA: 0x004DF728 File Offset: 0x004DD928
		private CompProperties_Dissolution Props
		{
			get
			{
				return (CompProperties_Dissolution)this.props;
			}
		}

		// Token: 0x17002BAD RID: 11181
		// (get) Token: 0x06010EE5 RID: 69349 RVA: 0x004DF735 File Offset: 0x004DD935
		public int DeterioarationRate
		{
			get
			{
				return (int)this.parent.GetStatValue(StatDefOf.DeteriorationRate, true, -1);
			}
		}

		// Token: 0x17002BAE RID: 11182
		// (get) Token: 0x06010EE6 RID: 69350 RVA: 0x004DF74A File Offset: 0x004DD94A
		public int DissolutionAfterDamage
		{
			get
			{
				return this.DeterioarationRate * this.Props.dissolutionAfterDays;
			}
		}

		// Token: 0x17002BAF RID: 11183
		// (get) Token: 0x06010EE7 RID: 69351 RVA: 0x004DF75E File Offset: 0x004DD95E
		public int DefaultTicksUntilDissolution
		{
			get
			{
				return 60000 * this.Props.dissolutionAfterDays;
			}
		}

		// Token: 0x17002BB0 RID: 11184
		// (get) Token: 0x06010EE8 RID: 69352 RVA: 0x004DF774 File Offset: 0x004DD974
		public bool IsOutdoors
		{
			get
			{
				Map mapHeld = this.parent.MapHeld;
				IntVec3 position = this.parent.Position;
				if (mapHeld != null)
				{
					Room room = position.GetRoom(mapHeld);
					if (room != null && room.UsesOutdoorTemperature)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17002BB1 RID: 11185
		// (get) Token: 0x06010EE9 RID: 69353 RVA: 0x004DF7B2 File Offset: 0x004DD9B2
		public bool IsFrozen
		{
			get
			{
				return this.parent.AmbientTemperature <= 0f;
			}
		}

		// Token: 0x17002BB2 RID: 11186
		// (get) Token: 0x06010EEA RID: 69354 RVA: 0x004DF7C9 File Offset: 0x004DD9C9
		public bool InAtomizer
		{
			get
			{
				return this.parent.ParentHolder != null && this.parent.ParentHolder is CompAtomizer;
			}
		}

		// Token: 0x17002BB3 RID: 11187
		// (get) Token: 0x06010EEB RID: 69355 RVA: 0x004DF7ED File Offset: 0x004DD9ED
		public bool CanDissolveNow
		{
			get
			{
				return !this.IsFrozen && !this.InAtomizer;
			}
		}

		// Token: 0x17002BB4 RID: 11188
		// (get) Token: 0x06010EEC RID: 69356 RVA: 0x004DF802 File Offset: 0x004DDA02
		public bool IsBeingRainedOn
		{
			get
			{
				return this.IsOutdoors && this.parent.MapHeld.weatherManager.curWeather.rainRate > 0.1f;
			}
		}

		// Token: 0x17002BB5 RID: 11189
		// (get) Token: 0x06010EED RID: 69357 RVA: 0x004DF830 File Offset: 0x004DDA30
		public int DissolutionIntervalTicks
		{
			get
			{
				float num = (float)this.DefaultTicksUntilDissolution;
				if (!this.IsOutdoors)
				{
					num /= this.Props.dissolutinFactorIndoors;
				}
				if (this.IsBeingRainedOn)
				{
					num /= this.Props.dissolutionFactorRain;
				}
				return (int)num;
			}
		}

		// Token: 0x06010EEE RID: 69358 RVA: 0x004DF873 File Offset: 0x004DDA73
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckBiotech("Dissolution"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostSpawnSetup(respawningAfterLoad);
		}

		// Token: 0x06010EEF RID: 69359 RVA: 0x004DF898 File Offset: 0x004DDA98
		public void TriggerDissolutionEvent(int amountToDissolve = 1)
		{
			amountToDissolve = Mathf.Min(amountToDissolve, this.parent.stackCount);
			if (this.parent.MapHeld != null)
			{
				this.DissolveMap(amountToDissolve);
			}
			else
			{
				this.DissolveWorld(amountToDissolve, this.parent.Tile);
			}
			this.parent.stackCount -= amountToDissolve;
			this.dissolveTicks = 0;
			if (this.parent.stackCount <= 0 && !this.parent.Destroyed)
			{
				this.parent.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06010EF0 RID: 69360 RVA: 0x004DF924 File Offset: 0x004DDB24
		private void DissolveMap(int amount)
		{
			List<ThingComp> allComps = this.parent.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				CompDissolutionEffect compDissolutionEffect = allComps[i] as CompDissolutionEffect;
				if (compDissolutionEffect != null)
				{
					compDissolutionEffect.DoDissolutionEffectMap(amount);
				}
			}
			Find.Storyteller.Notify_DissolutionEvent(this.parent);
		}

		// Token: 0x06010EF1 RID: 69361 RVA: 0x004DF978 File Offset: 0x004DDB78
		private void DissolveWorld(int amount, PlanetTile tile)
		{
			List<ThingComp> allComps = this.parent.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				CompDissolutionEffect compDissolutionEffect = allComps[i] as CompDissolutionEffect;
				if (compDissolutionEffect != null)
				{
					compDissolutionEffect.DoDissolutionEffectWorld(amount, tile);
				}
			}
		}

		// Token: 0x06010EF2 RID: 69362 RVA: 0x004DF9BA File Offset: 0x004DDBBA
		public override void PostPreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			base.PostPreApplyDamage(ref dinfo, out absorbed);
			if (dinfo.Def.harmsHealth && dinfo.Amount >= (float)this.parent.HitPoints)
			{
				this.DissolveMap(this.parent.stackCount);
			}
		}

		// Token: 0x06010EF3 RID: 69363 RVA: 0x004DF9F6 File Offset: 0x004DDBF6
		public override void Notify_AbandonedAtTile(PlanetTile tile)
		{
			this.DissolveWorld(this.parent.stackCount, tile);
		}

		// Token: 0x06010EF4 RID: 69364 RVA: 0x004DFA0A File Offset: 0x004DDC0A
		public override void CompTick()
		{
			base.CompTick();
			if (!this.CanDissolveNow)
			{
				return;
			}
			this.dissolveTicks++;
			if (this.dissolveTicks >= this.DissolutionIntervalTicks)
			{
				this.TriggerDissolutionEvent(1);
			}
		}

		// Token: 0x06010EF5 RID: 69365 RVA: 0x004DFA3E File Offset: 0x004DDC3E
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Dissolution event",
					action = delegate()
					{
						this.TriggerDissolutionEvent(1);
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Dissolution event until destroyed",
					action = delegate()
					{
						int num = 1000;
						while (!this.parent.Destroyed && num > 0)
						{
							this.TriggerDissolutionEvent(1);
							num--;
						}
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Dissolution progress +25%",
					action = delegate()
					{
						this.parent.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, (float)this.DissolutionAfterDamage * 0.25f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Set next dissolve time",
					action = delegate()
					{
						List<FloatMenuOption> list = new List<FloatMenuOption>();
						int[] array = new int[]
						{
							60,
							120,
							180,
							240,
							300,
							600,
							900,
							1200,
							1500,
							1800,
							3600
						};
						for (int i = 0; i < array.Length; i++)
						{
							int ticks = array[i];
							list.Add(new FloatMenuOption(ticks.ToStringSecondsFromTicks("F0"), delegate()
							{
								this.dissolveTicks = this.DissolutionIntervalTicks - ticks;
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
						}
						Find.WindowStack.Add(new FloatMenu(list));
					}
				};
			}
			yield break;
		}

		// Token: 0x06010EF6 RID: 69366 RVA: 0x004DFA50 File Offset: 0x004DDC50
		public override string CompInspectStringExtra()
		{
			if (this.InAtomizer)
			{
				return base.CompInspectStringExtra();
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.CompInspectStringExtra());
			if (stringBuilder.Length > 0)
			{
				stringBuilder.AppendLine();
			}
			if (this.IsFrozen)
			{
				stringBuilder.Append("DissolutionFrozen".Translate());
				return stringBuilder.ToString();
			}
			CompDissolution.tmpDissolutionReasons.Clear();
			if (!this.IsOutdoors)
			{
				CompDissolution.tmpDissolutionReasons.Add("DissolutionRateIndoors".Translate() + " x" + this.Props.dissolutinFactorIndoors.ToStringPercent());
			}
			if (this.IsBeingRainedOn)
			{
				CompDissolution.tmpDissolutionReasons.Add("DissolutionRain".Translate() + " x" + this.Props.dissolutionFactorRain.ToStringPercent());
			}
			stringBuilder.Append("DissolvesEvery".Translate(this.DissolutionIntervalTicks.ToStringTicksToPeriod(true, false, true, true, false)));
			if (CompDissolution.tmpDissolutionReasons.Count > 0)
			{
				string str = CompDissolution.tmpDissolutionReasons.ToLineList(null);
				stringBuilder.Append(" (" + str.CapitalizeFirst() + ")");
				CompDissolution.tmpDissolutionReasons.Clear();
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06010EF7 RID: 69367 RVA: 0x004DFBAE File Offset: 0x004DDDAE
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.dissolveTicks, "dissolveTicks", 0, false);
		}

		// Token: 0x0400B01E RID: 45086
		private int dissolveTicks;

		// Token: 0x0400B01F RID: 45087
		private static List<string> tmpDissolutionReasons = new List<string>();
	}
}
