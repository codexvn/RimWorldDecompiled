using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld.Planet
{
	// Token: 0x02003E2B RID: 15915
	[StaticConstructorOnStartup]
	public class FormCaravanComp : WorldObjectComp
	{
		// Token: 0x17003645 RID: 13893
		// (get) Token: 0x06015BD4 RID: 89044 RVA: 0x0068018F File Offset: 0x0067E38F
		public WorldObjectCompProperties_FormCaravan Props
		{
			get
			{
				return (WorldObjectCompProperties_FormCaravan)this.props;
			}
		}

		// Token: 0x17003646 RID: 13894
		// (get) Token: 0x06015BD5 RID: 89045 RVA: 0x0068019C File Offset: 0x0067E39C
		private MapParent MapParent
		{
			get
			{
				return (MapParent)this.parent;
			}
		}

		// Token: 0x17003647 RID: 13895
		// (get) Token: 0x06015BD6 RID: 89046 RVA: 0x006801A9 File Offset: 0x0067E3A9
		public bool Reform
		{
			get
			{
				return !this.MapParent.HasMap || !this.MapParent.Map.IsPlayerHome;
			}
		}

		// Token: 0x17003648 RID: 13896
		// (get) Token: 0x06015BD7 RID: 89047 RVA: 0x006801D0 File Offset: 0x0067E3D0
		public bool CanFormOrReformCaravanNow
		{
			get
			{
				MapParent mapParent = this.MapParent;
				if (!mapParent.HasMap)
				{
					return false;
				}
				if (this.Reform)
				{
					if (this.AnyActiveThreatNow)
					{
						return false;
					}
					List<Pawn> freeColonistsSpawned = mapParent.Map.mapPawns.FreeColonistsSpawned;
					for (int i = 0; i < freeColonistsSpawned.Count; i++)
					{
						MentalState mentalState = freeColonistsSpawned[i].MentalState;
						if (mentalState != null && mentalState.def.category == MentalStateCategory.Aggro)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x17003649 RID: 13897
		// (get) Token: 0x06015BD8 RID: 89048 RVA: 0x00680247 File Offset: 0x0067E447
		public bool AnyActiveThreatNow
		{
			get
			{
				return this.MapParent.HasMap && GenHostility.AnyHostileActiveThreatToPlayer(this.MapParent.Map, true, !this.parent.CanReformFoggedEnemies);
			}
		}

		// Token: 0x1700364A RID: 13898
		// (get) Token: 0x06015BD9 RID: 89049 RVA: 0x00680278 File Offset: 0x0067E478
		public bool AnyUnexploredFoggedRooms
		{
			get
			{
				if (this.foggedRoomsCheckRect == null)
				{
					return false;
				}
				MapParent mapParent = this.MapParent;
				if (!mapParent.HasMap)
				{
					return false;
				}
				IReadOnlyList<Room> allRooms = mapParent.Map.regionGrid.AllRooms;
				CellRect value = this.foggedRoomsCheckRect.Value;
				for (int i = 0; i < allRooms.Count; i++)
				{
					if (allRooms[i].Fogged && allRooms[i].ProperRoom)
					{
						foreach (IntVec3 c in allRooms[i].Cells)
						{
							if (value.Contains(c))
							{
								return true;
							}
						}
					}
				}
				return false;
			}
		}

		// Token: 0x06015BDA RID: 89050 RVA: 0x00680348 File Offset: 0x0067E548
		public override void CompTickInterval(int delta)
		{
			base.CompTickInterval(delta);
			bool anyActiveThreatNow = this.AnyActiveThreatNow;
			if (!anyActiveThreatNow && this.anyActiveThreatLastTick && this.Reform && this.CanFormOrReformCaravanNow)
			{
				if (this.AnyUnexploredFoggedRooms)
				{
					Messages.Message("MessageCanReformCaravanNowNoMoreEnemiesButUnexploredAreas".Translate(), new LookTargets(this.parent), MessageTypeDefOf.SituationResolved, false);
				}
				else
				{
					Messages.Message("MessageCanReformCaravanNowNoMoreEnemies".Translate(), new LookTargets(this.parent), MessageTypeDefOf.NeutralEvent, false);
				}
			}
			this.anyActiveThreatLastTick = anyActiveThreatNow;
		}

		// Token: 0x06015BDB RID: 89051 RVA: 0x006803DC File Offset: 0x0067E5DC
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.anyActiveThreatLastTick, "anyActiveThreatLastTick", false, false);
			Scribe_Values.Look<CellRect?>(ref this.foggedRoomsCheckRect, "foggedRoomsCheckRect", null, false);
		}

		// Token: 0x06015BDC RID: 89052 RVA: 0x00680418 File Offset: 0x0067E618
		public bool CanReformNow()
		{
			MapParent mapParent = (MapParent)this.parent;
			return this.MapParent.HasMap && this.Reform && this.CanFormOrReformCaravanNow && mapParent.Map.mapPawns.FreeColonistsSpawnedCount != 0;
		}

		// Token: 0x06015BDD RID: 89053 RVA: 0x00680463 File Offset: 0x0067E663
		public override IEnumerable<Gizmo> GetGizmos()
		{
			MapParent mapParent = (MapParent)this.parent;
			if (mapParent.HasMap)
			{
				if (this.Reform && this.CanFormOrReformCaravanNow && mapParent.Map.mapPawns.FreeColonistsSpawnedCount != 0)
				{
					Command_Action command_Action = new Command_Action();
					command_Action.defaultLabel = "CommandReformCaravan".Translate();
					command_Action.defaultDesc = "CommandReformCaravanDesc".Translate();
					command_Action.icon = FormCaravanComp.FormCaravanCommand;
					command_Action.hotKey = KeyBindingDefOf.Misc2;
					command_Action.tutorTag = "ReformCaravan";
					command_Action.action = delegate()
					{
						if (ModsConfig.OdysseyActive && mapParent.Map.listerThings.ThingsInGroup(ThingRequestGroup.PassengerShuttle).Any<Thing>())
						{
							Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmLoseShuttle".Translate(), new Action(base.<GetGizmos>g__Form|1), false, null, WindowLayer.Dialog));
							return;
						}
						base.<GetGizmos>g__Form|1();
					};
					if (GenHostility.AnyHostileActiveThreatToPlayer(mapParent.Map, true, false))
					{
						command_Action.Disable("CommandReformCaravanFailHostilePawns".Translate());
					}
					yield return command_Action;
				}
				if (DebugSettings.ShowDevGizmos)
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: Show available exits",
						action = delegate()
						{
							foreach (PlanetTile tile in CaravanExitMapUtility.AvailableExitTilesAt(mapParent.Map))
							{
								Find.WorldDebugDrawer.FlashTile(tile, 0f, null, 10);
							}
						}
					};
				}
			}
			yield break;
		}

		// Token: 0x0400F146 RID: 61766
		public CellRect? foggedRoomsCheckRect;

		// Token: 0x0400F147 RID: 61767
		public static readonly Texture2D FormCaravanCommand = ContentFinder<Texture2D>.Get("UI/Commands/FormCaravan", true);

		// Token: 0x0400F148 RID: 61768
		private bool anyActiveThreatLastTick;
	}
}
