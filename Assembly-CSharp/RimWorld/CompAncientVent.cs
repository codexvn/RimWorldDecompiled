using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002BD5 RID: 11221
	public abstract class CompAncientVent : ThingComp
	{
		// Token: 0x17002754 RID: 10068
		// (get) Token: 0x0600FAF7 RID: 64247 RVA: 0x0048E995 File Offset: 0x0048CB95
		public CompProperties_AncientVentEmitter Props
		{
			get
			{
				return (CompProperties_AncientVentEmitter)this.props;
			}
		}

		// Token: 0x17002755 RID: 10069
		// (get) Token: 0x0600FAF8 RID: 64248 RVA: 0x00002501 File Offset: 0x00000701
		protected virtual bool AppliesEffectsToPawns
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002756 RID: 10070
		// (get) Token: 0x0600FAF9 RID: 64249 RVA: 0x0048E9A2 File Offset: 0x0048CBA2
		public bool VentOn
		{
			get
			{
				return this.ventOn;
			}
		}

		// Token: 0x0600FAFA RID: 64250 RVA: 0x0048E9AC File Offset: 0x0048CBAC
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.Destroyed)
			{
				return;
			}
			if (this.CheckShouldToggleAndHandleMapState())
			{
				this.ventOn = !this.ventOn;
				this.ToggleIndividualVent(this.ventOn);
			}
			if (!this.ventOn)
			{
				Sustainer sustainer = this.sustainer;
				if (sustainer != null)
				{
					sustainer.End();
				}
				this.sustainer = null;
				return;
			}
			if (this.sustainer == null)
			{
				this.sustainer = this.Props.activeSound.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.PerTick));
			}
			this.sustainer.Maintain();
			if (this.parent.IsHashIntervalTick(20) && this.Props.heatToPush != 0)
			{
				GenTemperature.PushHeat(this.parent, (float)this.Props.heatToPush);
			}
			if (!this.AppliesEffectsToPawns)
			{
				return;
			}
			Map map = this.parent.Map;
			IntVec3 position = this.parent.Position;
			int num = Mathf.FloorToInt((float)this.parent.def.size.x / 2f);
			for (int i = 0; i < map.mapPawns.AllPawnsSpawned.Count; i++)
			{
				Pawn pawn = map.mapPawns.AllPawnsSpawned[i];
				if (pawn.Position.DistanceTo(position) <= (float)num)
				{
					this.ApplyPawnEffect(pawn, 1);
				}
			}
		}

		// Token: 0x0600FAFB RID: 64251 RVA: 0x0048EB09 File Offset: 0x0048CD09
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.ventOn, "ventOn", false, false);
		}

		// Token: 0x0600FAFC RID: 64252 RVA: 0x0048EB24 File Offset: 0x0048CD24
		private bool CheckShouldToggleAndHandleMapState()
		{
			int num;
			if ((CompAncientVent.sharedLastCheckMtbTickPerMap.TryGetValue(this.parent.Map.uniqueID, out num) ? num : 0) == GenTicks.TicksGame)
			{
				bool flag;
				return CompAncientVent.sharedLastMtbCheckDidOccurPerMap.TryGetValue(this.parent.Map.uniqueID, out flag) && flag;
			}
			int num2 = this.ventOn ? this.Props.onDurationMtbDays : this.Props.offDurationMtbDays;
			CompAncientVent.sharedLastCheckMtbTickPerMap[this.parent.Map.uniqueID] = GenTicks.TicksGame;
			bool flag2 = CompAncientVent.sharedLastMtbCheckDidOccurPerMap[this.parent.Map.uniqueID] = Rand.MTBEventOccurs((float)num2, 60000f, 1f);
			if (flag2)
			{
				this.ToggleMapState();
			}
			return flag2;
		}

		// Token: 0x0600FAFD RID: 64253 RVA: 0x0048EBF0 File Offset: 0x0048CDF0
		private void ToggleMapState()
		{
			if (this.ventOn)
			{
				if (this.Props.conditionToCause != null)
				{
					GameCondition activeCondition = this.parent.Map.gameConditionManager.GetActiveCondition(this.Props.conditionToCause);
					if (activeCondition != null)
					{
						activeCondition.End();
					}
				}
				this.SendToggleAlert(false);
				return;
			}
			if (this.Props.conditionToCause != null)
			{
				if (!this.parent.Map.gameConditionManager.ConditionIsActive(this.Props.conditionToCause))
				{
					GameCondition gameCondition = GameConditionMaker.MakeConditionPermanent(this.Props.conditionToCause);
					gameCondition.suppressEndMessage = true;
					gameCondition.conditionCauser = this.parent;
					gameCondition.forceDisplayAsDuration = true;
					this.parent.Map.gameConditionManager.RegisterCondition(gameCondition);
				}
				this.SendToggleAlert(true);
			}
		}

		// Token: 0x0600FAFE RID: 64254 RVA: 0x0048ECBC File Offset: 0x0048CEBC
		public override string CompInspectStringExtra()
		{
			if (!this.ventOn)
			{
				return "DormantCompInactive".Translate();
			}
			return this.Props.activeInspectStringKey.Translate().CapitalizeFirst();
		}

		// Token: 0x0600FAFF RID: 64255 RVA: 0x0048ECFE File Offset: 0x0048CEFE
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!DebugSettings.ShowDevGizmos)
			{
				yield break;
			}
			yield return new Command_Action
			{
				action = new Action(this.DevToggleVent),
				defaultLabel = "DEV: Toggle vents on map",
				defaultDesc = "Toggles all vents on the map on or off"
			};
			yield break;
		}

		// Token: 0x0600FB00 RID: 64256 RVA: 0x0048ED10 File Offset: 0x0048CF10
		private void DevToggleVent()
		{
			this.ToggleMapState();
			bool on = !this.ventOn;
			foreach (Building thing in this.parent.Map.listerBuildings.AllBuildingsNonColonistOfDef(this.parent.def))
			{
				CompAncientVent compAncientVent = thing.TryGetComp<CompAncientVent>();
				if (compAncientVent != null)
				{
					compAncientVent.ventOn = on;
					compAncientVent.ToggleIndividualVent(on);
				}
			}
		}

		// Token: 0x0600FB01 RID: 64257 RVA: 0x0048ED98 File Offset: 0x0048CF98
		protected void SendToggleAlert(bool goingToBeOn)
		{
			Messages.Message((goingToBeOn ? this.Props.startupMessageKey : this.Props.shutdownMessageKey).Translate(), new LookTargets(this.parent.Map.listerBuildings.AllBuildingsNonColonistOfDef(this.parent.def)), MessageTypeDefOf.NeutralEvent, false);
		}

		// Token: 0x0600FB02 RID: 64258
		protected abstract void ToggleIndividualVent(bool on);

		// Token: 0x0600FB03 RID: 64259 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void ApplyPawnEffect(Pawn pawn, int delta)
		{
		}

		// Token: 0x0400A4CB RID: 42187
		private static Dictionary<int, int> sharedLastCheckMtbTickPerMap = new Dictionary<int, int>();

		// Token: 0x0400A4CC RID: 42188
		private static Dictionary<int, bool> sharedLastMtbCheckDidOccurPerMap = new Dictionary<int, bool>();

		// Token: 0x0400A4CD RID: 42189
		private const int PushHeatInterval = 20;

		// Token: 0x0400A4CE RID: 42190
		private bool ventOn;

		// Token: 0x0400A4CF RID: 42191
		private Sustainer sustainer;
	}
}
