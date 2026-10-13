using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BC1 RID: 11201
	public class CompCauseGameCondition_TemperatureOffset : CompCauseGameCondition
	{
		// Token: 0x17002745 RID: 10053
		// (get) Token: 0x0600FA7F RID: 64127 RVA: 0x0048C99F File Offset: 0x0048AB9F
		public new CompProperties_CausesGameCondition_ClimateAdjuster Props
		{
			get
			{
				return (CompProperties_CausesGameCondition_ClimateAdjuster)this.props;
			}
		}

		// Token: 0x0600FA80 RID: 64128 RVA: 0x0048C9AC File Offset: 0x0048ABAC
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.temperatureOffset = this.Props.temperatureOffsetRange.min;
		}

		// Token: 0x0600FA81 RID: 64129 RVA: 0x0048C9CB File Offset: 0x0048ABCB
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.temperatureOffset, "temperatureOffset", 0f, false);
		}

		// Token: 0x0600FA82 RID: 64130 RVA: 0x0048C9E9 File Offset: 0x0048ABE9
		private string GetFloatStringWithSign(float val)
		{
			if (val < 0f)
			{
				return val.ToString("0");
			}
			return "+" + val.ToString("0");
		}

		// Token: 0x0600FA83 RID: 64131 RVA: 0x0048CA16 File Offset: 0x0048AC16
		public void SetTemperatureOffset(float offset)
		{
			this.temperatureOffset = this.Props.temperatureOffsetRange.ClampToRange(offset);
			base.ReSetupAllConditions();
		}

		// Token: 0x0600FA84 RID: 64132 RVA: 0x0048CA35 File Offset: 0x0048AC35
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!Prefs.DevMode || !DebugSettings.godMode)
			{
				yield break;
			}
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "-10";
			Command_Action command_Action2 = command_Action;
			command_Action2.action = (Action)Delegate.Combine(command_Action2.action, new Action(delegate()
			{
				this.SetTemperatureOffset(this.temperatureOffset - 10f);
			}));
			command_Action.hotKey = KeyBindingDefOf.Misc1;
			yield return command_Action;
			Command_Action command_Action3 = new Command_Action();
			command_Action3.defaultLabel = "-1";
			Command_Action command_Action4 = command_Action3;
			command_Action4.action = (Action)Delegate.Combine(command_Action4.action, new Action(delegate()
			{
				this.SetTemperatureOffset(this.temperatureOffset - 1f);
			}));
			command_Action3.hotKey = KeyBindingDefOf.Misc2;
			yield return command_Action3;
			Command_Action command_Action5 = new Command_Action();
			command_Action5.defaultLabel = "+1";
			Command_Action command_Action6 = command_Action5;
			command_Action6.action = (Action)Delegate.Combine(command_Action6.action, new Action(delegate()
			{
				this.SetTemperatureOffset(this.temperatureOffset + 1f);
			}));
			command_Action5.hotKey = KeyBindingDefOf.Misc3;
			yield return command_Action5;
			Command_Action command_Action7 = new Command_Action();
			command_Action7.defaultLabel = "+10";
			Command_Action command_Action8 = command_Action7;
			command_Action8.action = (Action)Delegate.Combine(command_Action8.action, new Action(delegate()
			{
				this.SetTemperatureOffset(this.temperatureOffset + 10f);
			}));
			command_Action7.hotKey = KeyBindingDefOf.Misc4;
			yield return command_Action7;
			yield break;
		}

		// Token: 0x0600FA85 RID: 64133 RVA: 0x0048CA48 File Offset: 0x0048AC48
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + ("Temperature".Translate() + ": " + this.GetFloatStringWithSign(this.temperatureOffset));
		}

		// Token: 0x0600FA86 RID: 64134 RVA: 0x0048CAA2 File Offset: 0x0048ACA2
		protected override void SetupCondition(GameCondition condition, Map map)
		{
			base.SetupCondition(condition, map);
			((GameCondition_TemperatureOffset)condition).tempOffset = this.temperatureOffset;
		}

		// Token: 0x0600FA87 RID: 64135 RVA: 0x0048CAC0 File Offset: 0x0048ACC0
		public override void RandomizeSettings(Site site)
		{
			bool flag = false;
			bool flag2 = false;
			foreach (WorldObject worldObject in Find.WorldObjects.AllWorldObjects)
			{
				Settlement settlement = worldObject as Settlement;
				if (settlement != null && settlement.Faction == Faction.OfPlayer)
				{
					if (settlement.Map != null)
					{
						bool flag3 = false;
						foreach (GameCondition gameCondition in settlement.Map.GameConditionManager.ActiveConditions)
						{
							if (gameCondition is GameCondition_TemperatureOffset)
							{
								float num = gameCondition.TemperatureOffset();
								if (num > 0f)
								{
									flag3 = true;
									flag = true;
									flag2 = false;
								}
								else if (num < 0f)
								{
									flag3 = true;
									flag2 = true;
									flag = false;
								}
								if (flag3)
								{
									break;
								}
							}
						}
						if (flag3)
						{
							break;
						}
					}
					PlanetTile tile = worldObject.Tile;
					if ((float)Find.WorldGrid.TraversalDistanceBetween(site.Tile, tile, true, this.Props.worldRange + 1, false) <= (float)this.Props.worldRange)
					{
						float num2 = GenTemperature.MinTemperatureAtTile(tile);
						float num3 = GenTemperature.MaxTemperatureAtTile(tile);
						if (num2 < -5f)
						{
							flag2 = true;
						}
						if (num3 > 20f)
						{
							flag = true;
						}
					}
				}
			}
			if (flag2 == flag)
			{
				this.temperatureOffset = (Rand.Bool ? this.Props.temperatureOffsetRange.min : this.Props.temperatureOffsetRange.max);
				return;
			}
			if (flag2)
			{
				this.temperatureOffset = this.Props.temperatureOffsetRange.min;
				return;
			}
			if (flag)
			{
				this.temperatureOffset = this.Props.temperatureOffsetRange.max;
			}
		}

		// Token: 0x0400A48F RID: 42127
		public float temperatureOffset;

		// Token: 0x0400A490 RID: 42128
		private const float MaxTempForMinOffset = -5f;

		// Token: 0x0400A491 RID: 42129
		private const float MinTempForMaxOffset = 20f;
	}
}
