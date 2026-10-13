using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020029E0 RID: 10720
	public class Building_BioferriteHarvester : Building
	{
		// Token: 0x17002479 RID: 9337
		// (get) Token: 0x0600EC6F RID: 60527 RVA: 0x0044AA18 File Offset: 0x00448C18
		public CompFacility FacilityComp
		{
			get
			{
				CompFacility result;
				if ((result = this.facilityComp) == null)
				{
					result = (this.facilityComp = base.GetComp<CompFacility>());
				}
				return result;
			}
		}

		// Token: 0x1700247A RID: 9338
		// (get) Token: 0x0600EC70 RID: 60528 RVA: 0x0044AA40 File Offset: 0x00448C40
		public CompPowerTrader Power
		{
			get
			{
				CompPowerTrader result;
				if ((result = this.powerComp) == null)
				{
					result = (this.powerComp = base.GetComp<CompPowerTrader>());
				}
				return result;
			}
		}

		// Token: 0x1700247B RID: 9339
		// (get) Token: 0x0600EC71 RID: 60529 RVA: 0x0044AA66 File Offset: 0x00448C66
		public List<Thing> Platforms
		{
			get
			{
				return this.FacilityComp.LinkedBuildings;
			}
		}

		// Token: 0x1700247C RID: 9340
		// (get) Token: 0x0600EC72 RID: 60530 RVA: 0x0044AA73 File Offset: 0x00448C73
		public bool ReadyForHauling
		{
			get
			{
				return Mathf.FloorToInt(this.containedBioferrite) >= 30;
			}
		}

		// Token: 0x1700247D RID: 9341
		// (get) Token: 0x0600EC73 RID: 60531 RVA: 0x0044AA88 File Offset: 0x00448C88
		private float BioferritePerDay
		{
			get
			{
				if (!this.Power.PowerOn)
				{
					return 0f;
				}
				float num = 0f;
				foreach (Thing thing in this.Platforms)
				{
					Building_HoldingPlatform building_HoldingPlatform = thing as Building_HoldingPlatform;
					if (building_HoldingPlatform != null && building_HoldingPlatform.Occupied)
					{
						num += CompProducesBioferrite.BioferritePerDay(building_HoldingPlatform.HeldPawn);
					}
				}
				return num;
			}
		}

		// Token: 0x1700247E RID: 9342
		// (get) Token: 0x0600EC74 RID: 60532 RVA: 0x0044AB0C File Offset: 0x00448D0C
		public CompCableConnection CableConnection
		{
			get
			{
				CompCableConnection result;
				if ((result = this.cableConnection) == null)
				{
					result = (this.cableConnection = base.GetComp<CompCableConnection>());
				}
				return result;
			}
		}

		// Token: 0x0600EC75 RID: 60533 RVA: 0x0044AB32 File Offset: 0x00448D32
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.Initialize();
			}
		}

		// Token: 0x0600EC76 RID: 60534 RVA: 0x0044AB48 File Offset: 0x00448D48
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			base.DeSpawn(mode);
			this.FacilityComp.OnLinkAdded -= this.OnLinkAdded;
			this.FacilityComp.OnLinkRemoved -= this.OnLinkRemoved;
			this.initalized = false;
			Sustainer sustainer = this.workingSustainer;
			if (sustainer != null)
			{
				sustainer.End();
			}
			this.workingSustainer = null;
		}

		// Token: 0x0600EC77 RID: 60535 RVA: 0x0044ABAC File Offset: 0x00448DAC
		private void Initialize()
		{
			if (this.initalized)
			{
				return;
			}
			this.initalized = true;
			this.FacilityComp.OnLinkAdded += this.OnLinkAdded;
			this.FacilityComp.OnLinkRemoved += this.OnLinkRemoved;
			foreach (Thing thing in this.Platforms)
			{
				Building_HoldingPlatform building_HoldingPlatform = thing as Building_HoldingPlatform;
				if (building_HoldingPlatform != null)
				{
					building_HoldingPlatform.innerContainer.OnContentsChanged += this.RebuildCables;
				}
			}
			this.RebuildCables();
		}

		// Token: 0x0600EC78 RID: 60536 RVA: 0x0044AC5C File Offset: 0x00448E5C
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DrawAt(drawLoc, flip);
			if (!this.initalized)
			{
				this.Initialize();
			}
		}

		// Token: 0x0600EC79 RID: 60537 RVA: 0x0044AC74 File Offset: 0x00448E74
		private void OnLinkRemoved(CompFacility facility, Thing thing)
		{
			Building_HoldingPlatform building_HoldingPlatform = thing as Building_HoldingPlatform;
			if (building_HoldingPlatform == null)
			{
				return;
			}
			building_HoldingPlatform.innerContainer.OnContentsChanged -= this.RebuildCables;
			this.RebuildCables();
		}

		// Token: 0x0600EC7A RID: 60538 RVA: 0x0044ACAC File Offset: 0x00448EAC
		private void OnLinkAdded(CompFacility facility, Thing thing)
		{
			Building_HoldingPlatform building_HoldingPlatform = thing as Building_HoldingPlatform;
			if (building_HoldingPlatform == null)
			{
				return;
			}
			building_HoldingPlatform.innerContainer.OnContentsChanged += this.RebuildCables;
			this.RebuildCables();
		}

		// Token: 0x0600EC7B RID: 60539 RVA: 0x0044ACE4 File Offset: 0x00448EE4
		protected override void Tick()
		{
			base.Tick();
			if (this.IsHashIntervalTick(250))
			{
				this.containedBioferrite = Mathf.Min(this.containedBioferrite + this.BioferritePerDay / 60000f * 250f, 60f);
			}
			if (this.IsWorking())
			{
				if (this.workingSustainer == null)
				{
					this.workingSustainer = SoundDefOf.BioferriteHarvester_Ambient.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.None));
				}
				this.workingSustainer.Maintain();
				return;
			}
			Sustainer sustainer = this.workingSustainer;
			if (sustainer != null)
			{
				sustainer.End();
			}
			this.workingSustainer = null;
		}

		// Token: 0x0600EC7C RID: 60540 RVA: 0x0044AD7D File Offset: 0x00448F7D
		public override bool IsWorking()
		{
			return this.BioferritePerDay != 0f;
		}

		// Token: 0x0600EC7D RID: 60541 RVA: 0x0044AD90 File Offset: 0x00448F90
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (stringBuilder.Length != 0)
			{
				stringBuilder.AppendLine();
			}
			stringBuilder.Append("BioferriteHarvesterContained".Translate());
			stringBuilder.Append(string.Format(": {0:F2} / {1} (+{2:F2} ", this.containedBioferrite, 60f.ToString(), this.BioferritePerDay));
			stringBuilder.Append(string.Format("{0})", "BioferriteHarvesterPerDay".Translate()));
			return stringBuilder.ToString();
		}

		// Token: 0x0600EC7E RID: 60542 RVA: 0x0044AE30 File Offset: 0x00449030
		public Thing TakeOutBioferrite()
		{
			int num = Mathf.FloorToInt(this.containedBioferrite);
			if (num == 0)
			{
				return null;
			}
			this.containedBioferrite -= (float)num;
			Thing thing = ThingMaker.MakeThing(ThingDefOf.Bioferrite, null);
			thing.stackCount = num;
			return thing;
		}

		// Token: 0x0600EC7F RID: 60543 RVA: 0x0044AE6F File Offset: 0x0044906F
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			yield return new Command_Toggle
			{
				defaultLabel = "BioferriteHarvesterToggleUnloading".Translate(),
				defaultDesc = "BioferriteHarvesterToggleUnloadingDesc".Translate(),
				isActive = (() => this.unloadingEnabled),
				toggleAction = delegate()
				{
					this.unloadingEnabled = !this.unloadingEnabled;
				},
				activateSound = SoundDefOf.Tick_Tiny,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/BioferriteUnloading", true)
			};
			if (this.containedBioferrite >= 1f)
			{
				yield return new Command_Action
				{
					defaultLabel = "BioferriteHarvesterEjectContents".Translate(),
					defaultDesc = "BioferriteHarvesterEjectContentsDesc".Translate(Find.ActiveLanguageWorker.Pluralize(ThingDefOf.Bioferrite.label, -1)),
					action = delegate()
					{
						this.EjectContents();
					},
					Disabled = (this.containedBioferrite == 0f),
					activateSound = SoundDefOf.Tick_Tiny,
					icon = ContentFinder<Texture2D>.Get("UI/Commands/EjectBioferrite", true)
				};
			}
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Add +1 bioferrite",
					action = delegate()
					{
						this.containedBioferrite = Mathf.Min(this.containedBioferrite + 1f, 60f);
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0600EC80 RID: 60544 RVA: 0x0044AE80 File Offset: 0x00449080
		private void EjectContents()
		{
			Thing thing = this.TakeOutBioferrite();
			if (thing == null)
			{
				return;
			}
			GenPlace.TryPlaceThing(thing, base.Position, base.Map, ThingPlaceMode.Near, null, null, null, 1);
		}

		// Token: 0x0600EC81 RID: 60545 RVA: 0x0044AEB8 File Offset: 0x004490B8
		public override void Notify_DefsHotReloaded()
		{
			base.Notify_DefsHotReloaded();
			this.RebuildCables();
		}

		// Token: 0x0600EC82 RID: 60546 RVA: 0x0044AEC6 File Offset: 0x004490C6
		private void RebuildCables()
		{
			this.CableConnection.RebuildCables(this.Platforms, delegate(Thing thing)
			{
				Building_HoldingPlatform building_HoldingPlatform = thing as Building_HoldingPlatform;
				return building_HoldingPlatform != null && building_HoldingPlatform.Occupied;
			});
		}

		// Token: 0x0600EC83 RID: 60547 RVA: 0x0044AEF8 File Offset: 0x004490F8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.containedBioferrite, "containedBioferrite", 0f, false);
			Scribe_Values.Look<bool>(ref this.unloadingEnabled, "unloadingEnabled", false, false);
		}

		// Token: 0x04009CC4 RID: 40132
		public float containedBioferrite;

		// Token: 0x04009CC5 RID: 40133
		public bool unloadingEnabled = true;

		// Token: 0x04009CC6 RID: 40134
		private bool initalized;

		// Token: 0x04009CC7 RID: 40135
		private const int HaulingThreshold = 30;

		// Token: 0x04009CC8 RID: 40136
		private const float MaxCapacity = 60f;

		// Token: 0x04009CC9 RID: 40137
		private CompFacility facilityComp;

		// Token: 0x04009CCA RID: 40138
		private CompPowerTrader powerComp;

		// Token: 0x04009CCB RID: 40139
		private CompCableConnection cableConnection;

		// Token: 0x04009CCC RID: 40140
		private Sustainer workingSustainer;
	}
}
