using System;
using System.Collections.Generic;
using System.Text;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002DC6 RID: 11718
	public class CompPsychicRitualSpot : ThingComp
	{
		// Token: 0x17002A0D RID: 10765
		// (get) Token: 0x060107A8 RID: 67496 RVA: 0x004C0F4E File Offset: 0x004BF14E
		private bool Obstructed
		{
			get
			{
				return this.obstructingThings.Count > 0;
			}
		}

		// Token: 0x17002A0E RID: 10766
		// (get) Token: 0x060107A9 RID: 67497 RVA: 0x004C0F5E File Offset: 0x004BF15E
		public HashSet<IntVec3> OccupiedCells
		{
			get
			{
				return this.occupiedCells;
			}
		}

		// Token: 0x060107AA RID: 67498 RVA: 0x004C0F68 File Offset: 0x004BF168
		public Lord GetLord()
		{
			Building building = this.parent as Building;
			if (building != null)
			{
				return building.GetLord();
			}
			Pawn pawn = this.parent as Pawn;
			if (pawn != null)
			{
				return pawn.GetLord();
			}
			Corpse corpse = this.parent as Corpse;
			if (corpse != null)
			{
				return corpse.GetLord();
			}
			return null;
		}

		// Token: 0x060107AB RID: 67499 RVA: 0x004C0FB8 File Offset: 0x004BF1B8
		public PsychicRitual GetPsychicRitual()
		{
			Lord lord = this.GetLord();
			LordToil_PsychicRitual lordToil_PsychicRitual = ((lord != null) ? lord.CurLordToil : null) as LordToil_PsychicRitual;
			if (lordToil_PsychicRitual == null)
			{
				return null;
			}
			return lordToil_PsychicRitual.RitualData.psychicRitual;
		}

		// Token: 0x060107AC RID: 67500 RVA: 0x004C0FE4 File Offset: 0x004BF1E4
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.occupiedCells = new HashSet<IntVec3>(GenRadial.RadialCellsAround(this.parent.Position, (float)this.parent.def.Size.x + 0.9f, true));
		}

		// Token: 0x060107AD RID: 67501 RVA: 0x004C1030 File Offset: 0x004BF230
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder(base.CompInspectStringExtra());
			if (this.Obstructed)
			{
				stringBuilder.AppendInNewLine("PsychicRitualSpotObstructed".Translate());
			}
			Lord lord = this.GetLord();
			if (lord != null)
			{
				LordToil_PsychicRitual lordToil_PsychicRitual = lord.CurLordToil as LordToil_PsychicRitual;
				if (lordToil_PsychicRitual != null)
				{
					PsychicRitual psychicRitual = lordToil_PsychicRitual.RitualData.psychicRitual;
					PsychicRitualToil psychicRitualToil = lordToil_PsychicRitual.RitualData.psychicRitualToil;
					stringBuilder.AppendInNewLine(string.Format("{0}: {1}", "PsychicRitual".Translate().CapitalizeFirst(), psychicRitual.def.LabelCap));
					stringBuilder.AppendInNewLine(psychicRitualToil.GetReport(psychicRitual, null).CapitalizeFirst().EndWithPeriod());
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060107AE RID: 67502 RVA: 0x004C10F4 File Offset: 0x004BF2F4
		public override void PostDrawExtraSelectionOverlays()
		{
			base.PostDrawExtraSelectionOverlays();
			if (this.parent.Faction == Faction.OfPlayer)
			{
				foreach (Thing t in this.obstructingThings)
				{
					GenDraw.DrawLineBetween(this.parent.TrueCenter(), t.TrueCenter(), SimpleColor.Red, 0.2f);
				}
			}
		}

		// Token: 0x060107AF RID: 67503 RVA: 0x004C1174 File Offset: 0x004BF374
		public override void CompTick()
		{
			base.CompTick();
			this.obstructingThings.Clear();
			foreach (IntVec3 c in this.occupiedCells)
			{
				List<Thing> list = this.parent.Map.thingGrid.ThingsListAt(c);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].def.passability != Traversability.Standable)
					{
						this.obstructingThings.Add(list[i]);
					}
				}
			}
			PsychicRitual psychicRitual = this.GetPsychicRitual();
			if (psychicRitual != null && this.parent.Faction == Faction.OfPlayer && this.Obstructed)
			{
				psychicRitual.CancelPsychicRitual("PsychicRitualAreaObstructed".Translate());
			}
		}

		// Token: 0x060107B0 RID: 67504 RVA: 0x004C1258 File Offset: 0x004BF458
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			PsychicRitual psychicRitual = this.GetPsychicRitual();
			if (psychicRitual != null)
			{
				yield return PsychicRitualGizmo.CancelGizmo(psychicRitual);
			}
			else
			{
				foreach (Gizmo gizmo2 in PsychicRitualGizmo.GetGizmos(this.parent))
				{
					if (this.Obstructed)
					{
						gizmo2.Disabled = true;
						gizmo2.disabledReason = "PsychicRitualAreaObstructed".Translate().CapitalizeFirst();
					}
					yield return gizmo2;
				}
				enumerator = null;
			}
			if (DebugSettings.ShowDevGizmos)
			{
				Command_Action command_Action = new Command_Action();
				command_Action.defaultLabel = "DEV: Reset psychic ritual cooldowns";
				command_Action.action = delegate()
				{
					Find.PsychicRitualManager.ClearAllCooldowns();
				};
				Command_Action command_Action2 = command_Action;
				yield return command_Action2;
			}
			yield break;
			yield break;
		}

		// Token: 0x0400ABE1 RID: 44001
		private List<Thing> obstructingThings = new List<Thing>();

		// Token: 0x0400ABE2 RID: 44002
		private HashSet<IntVec3> occupiedCells;
	}
}
