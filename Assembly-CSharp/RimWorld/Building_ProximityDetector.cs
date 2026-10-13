using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002977 RID: 10615
	public class Building_ProximityDetector : Building, IThingGlower
	{
		// Token: 0x17002388 RID: 9096
		// (get) Token: 0x0600E925 RID: 59685 RVA: 0x0043DDF8 File Offset: 0x0043BFF8
		public CompPowerTrader PowerTraderComp
		{
			get
			{
				CompPowerTrader result;
				if ((result = this.powerTraderComp) == null)
				{
					result = (this.powerTraderComp = base.GetComp<CompPowerTrader>());
				}
				return result;
			}
		}

		// Token: 0x17002389 RID: 9097
		// (get) Token: 0x0600E926 RID: 59686 RVA: 0x0043DE20 File Offset: 0x0043C020
		public CompGlower Glower
		{
			get
			{
				CompGlower result;
				if ((result = this.glowerComp) == null)
				{
					result = (this.glowerComp = base.GetComp<CompGlower>());
				}
				return result;
			}
		}

		// Token: 0x1700238A RID: 9098
		// (get) Token: 0x0600E927 RID: 59687 RVA: 0x0043DE46 File Offset: 0x0043C046
		private TargetInfo TgtInfo
		{
			get
			{
				return new TargetInfo(base.Position, base.Map, false);
			}
		}

		// Token: 0x0600E928 RID: 59688 RVA: 0x0043DE5A File Offset: 0x0043C05A
		public bool ShouldBeLitNow()
		{
			return this.triggered;
		}

		// Token: 0x0600E929 RID: 59689 RVA: 0x0043DE62 File Offset: 0x0043C062
		protected override void Tick()
		{
			base.Tick();
			Effecter effecter = this.detectionEffecter;
			if (effecter != null)
			{
				effecter.EffectTick(this.TgtInfo, this.TgtInfo);
			}
			if (!base.Spawned || !this.IsHashIntervalTick(30))
			{
				return;
			}
			this.RunDetection();
		}

		// Token: 0x0600E92A RID: 59690 RVA: 0x0043DEA0 File Offset: 0x0043C0A0
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (!ModLister.CheckAnomaly("Proximity detector"))
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.SpawnSetup(map, respawningAfterLoad);
			if (respawningAfterLoad)
			{
				this.TickDetectionEffect();
			}
		}

		// Token: 0x0600E92B RID: 59691 RVA: 0x0043DEC8 File Offset: 0x0043C0C8
		private void TickDetectionEffect()
		{
			if (this.triggered && this.detectionEffecter == null)
			{
				this.detectionEffecter = EffecterDefOf.ProximityDetectorAlert.Spawn(base.Position, base.Map, 1f);
				return;
			}
			if (!this.triggered && this.detectionEffecter != null)
			{
				this.detectionEffecter.Cleanup();
				this.detectionEffecter = null;
			}
		}

		// Token: 0x0600E92C RID: 59692 RVA: 0x0043DF2C File Offset: 0x0043C12C
		private void RunDetection()
		{
			bool flag = false;
			if (this.PowerTraderComp.PowerOn)
			{
				IReadOnlyList<Pawn> allPawnsSpawned = base.Map.mapPawns.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].Position.InHorDistOf(base.Position, 19.9f) && allPawnsSpawned[i].IsPsychologicallyInvisible())
					{
						flag = true;
						break;
					}
				}
			}
			if (flag != this.triggered)
			{
				this.triggered = flag;
				this.Glower.UpdateLit(base.Map);
				if (this.triggered)
				{
					Messages.Message("MessageProximityDetectorTriggered".Translate(), this, MessageTypeDefOf.ThreatSmall, false);
				}
			}
			this.TickDetectionEffect();
		}

		// Token: 0x0600E92D RID: 59693 RVA: 0x0043DFEC File Offset: 0x0043C1EC
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (this.triggered)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += "ProximityDetectorCreatureDetected".Translate().Colorize(ColorLibrary.RedReadable);
			}
			return text;
		}

		// Token: 0x0600E92E RID: 59694 RVA: 0x0043E038 File Offset: 0x0043C238
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.triggered, "triggered", false, false);
		}

		// Token: 0x04009B49 RID: 39753
		private bool triggered;

		// Token: 0x04009B4A RID: 39754
		private Effecter detectionEffecter;

		// Token: 0x04009B4B RID: 39755
		public const float DetectionRadius = 19.9f;

		// Token: 0x04009B4C RID: 39756
		private const int DetectInterval = 30;

		// Token: 0x04009B4D RID: 39757
		private CompPowerTrader powerTraderComp;

		// Token: 0x04009B4E RID: 39758
		private CompGlower glowerComp;
	}
}
