using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x02003D60 RID: 15712
	[StaticConstructorOnStartup]
	public static class CaravanMergeUtility
	{
		// Token: 0x17003556 RID: 13654
		// (get) Token: 0x06015672 RID: 87666 RVA: 0x0066730B File Offset: 0x0066550B
		public static bool ShouldShowMergeCommand
		{
			get
			{
				return CaravanMergeUtility.CanMergeAnySelectedCaravans || CaravanMergeUtility.AnySelectedCaravanCloseToAnyOtherMergeableCaravan;
			}
		}

		// Token: 0x17003557 RID: 13655
		// (get) Token: 0x06015673 RID: 87667 RVA: 0x0066731C File Offset: 0x0066551C
		public static bool CanMergeAnySelectedCaravans
		{
			get
			{
				List<WorldObject> selectedObjects = Find.WorldSelector.SelectedObjects;
				for (int i = 0; i < selectedObjects.Count; i++)
				{
					Caravan caravan = selectedObjects[i] as Caravan;
					if (caravan != null && caravan.IsPlayerControlled)
					{
						for (int j = i + 1; j < selectedObjects.Count; j++)
						{
							Caravan caravan2 = selectedObjects[j] as Caravan;
							if (caravan2 != null && caravan2.IsPlayerControlled && CaravanMergeUtility.CloseToEachOther(caravan, caravan2))
							{
								return true;
							}
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17003558 RID: 13656
		// (get) Token: 0x06015674 RID: 87668 RVA: 0x00667398 File Offset: 0x00665598
		public static bool AnySelectedCaravanCloseToAnyOtherMergeableCaravan
		{
			get
			{
				List<WorldObject> selectedObjects = Find.WorldSelector.SelectedObjects;
				List<Caravan> caravans = Find.WorldObjects.Caravans;
				for (int i = 0; i < selectedObjects.Count; i++)
				{
					Caravan caravan = selectedObjects[i] as Caravan;
					if (caravan != null && caravan.IsPlayerControlled)
					{
						for (int j = 0; j < caravans.Count; j++)
						{
							Caravan caravan2 = caravans[j];
							if (caravan2 != caravan && caravan2.IsPlayerControlled && CaravanMergeUtility.CloseToEachOther(caravan, caravan2))
							{
								return true;
							}
						}
					}
				}
				return false;
			}
		}

		// Token: 0x06015675 RID: 87669 RVA: 0x00667420 File Offset: 0x00665620
		public static Command MergeCommand(Caravan caravan)
		{
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "CommandMergeCaravans".Translate();
			command_Action.defaultDesc = "CommandMergeCaravansDesc".Translate();
			command_Action.icon = CaravanMergeUtility.MergeCommandTex;
			command_Action.groupable = true;
			command_Action.action = delegate()
			{
				if (Find.WorldSelector.FirstSelectedObject != caravan)
				{
					return;
				}
				CaravanMergeUtility.TryMergeSelectedCaravans();
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			};
			if (!CaravanMergeUtility.CanMergeAnySelectedCaravans)
			{
				command_Action.Disable("CommandMergeCaravansFailCaravansNotSelected".Translate());
			}
			return command_Action;
		}

		// Token: 0x06015676 RID: 87670 RVA: 0x006674AC File Offset: 0x006656AC
		public static void TryMergeSelectedCaravans()
		{
			CaravanMergeUtility.tmpSelectedPlayerCaravans.Clear();
			List<WorldObject> selectedObjects = Find.WorldSelector.SelectedObjects;
			for (int i = 0; i < selectedObjects.Count; i++)
			{
				Caravan caravan3 = selectedObjects[i] as Caravan;
				if (caravan3 != null && caravan3.IsPlayerControlled)
				{
					CaravanMergeUtility.tmpSelectedPlayerCaravans.Add(caravan3);
				}
			}
			while (CaravanMergeUtility.tmpSelectedPlayerCaravans.Any<Caravan>())
			{
				Caravan caravan2 = CaravanMergeUtility.tmpSelectedPlayerCaravans[0];
				CaravanMergeUtility.tmpSelectedPlayerCaravans.RemoveAt(0);
				CaravanMergeUtility.tmpCaravansOnSameTile.Clear();
				CaravanMergeUtility.tmpCaravansOnSameTile.Add(caravan2);
				CaravanMergeUtility.tmpShuttleCaravans.Clear();
				if (ModsConfig.OdysseyActive && caravan2.Shuttle != null)
				{
					CaravanMergeUtility.tmpShuttleCaravans.Add(caravan2);
				}
				for (int j = CaravanMergeUtility.tmpSelectedPlayerCaravans.Count - 1; j >= 0; j--)
				{
					if (CaravanMergeUtility.CloseToEachOther(CaravanMergeUtility.tmpSelectedPlayerCaravans[j], caravan2))
					{
						if (ModsConfig.OdysseyActive && CaravanMergeUtility.tmpSelectedPlayerCaravans[j].Shuttle != null)
						{
							CaravanMergeUtility.tmpShuttleCaravans.Add(CaravanMergeUtility.tmpSelectedPlayerCaravans[j]);
						}
						CaravanMergeUtility.tmpCaravansOnSameTile.Add(CaravanMergeUtility.tmpSelectedPlayerCaravans[j]);
						CaravanMergeUtility.tmpSelectedPlayerCaravans.RemoveAt(j);
					}
				}
				if (CaravanMergeUtility.tmpCaravansOnSameTile.Count >= 2)
				{
					if (CaravanMergeUtility.tmpShuttleCaravans.Count >= 2)
					{
						string value = (from caravan in CaravanMergeUtility.tmpShuttleCaravans.Skip(1)
						select caravan.Shuttle.LabelCap).ToLineList("  - ", false);
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmMergeShuttles".Translate(value), delegate()
						{
							CaravanMergeUtility.MergeCaravans(CaravanMergeUtility.tmpCaravansOnSameTile);
						}, false, null, WindowLayer.Dialog));
					}
					else
					{
						CaravanMergeUtility.MergeCaravans(CaravanMergeUtility.tmpCaravansOnSameTile);
					}
				}
			}
		}

		// Token: 0x06015677 RID: 87671 RVA: 0x00667694 File Offset: 0x00665894
		private static bool CloseToEachOther(Caravan c1, Caravan c2)
		{
			if (c1.Tile == c2.Tile)
			{
				return true;
			}
			Vector3 drawPos = c1.DrawPos;
			Vector3 drawPos2 = c2.DrawPos;
			float num = Find.WorldGrid.AverageTileSize * 0.5f;
			return (drawPos - drawPos2).sqrMagnitude < num * num;
		}

		// Token: 0x06015678 RID: 87672 RVA: 0x006676EC File Offset: 0x006658EC
		private static void MergeCaravans(List<Caravan> caravans)
		{
			bool flag = false;
			Caravan caravan = caravans.MaxBy((Caravan x) => x.PawnsListForReading.Count);
			for (int i = 0; i < caravans.Count; i++)
			{
				Caravan caravan2 = caravans[i];
				if (ModsConfig.OdysseyActive && caravan2.Shuttle != null)
				{
					if (!flag)
					{
						flag = true;
					}
					else
					{
						Building_PassengerShuttle shuttle = caravan2.Shuttle;
						caravan2.GetDirectlyHeldThings().Remove(shuttle);
						caravan2.Shuttle.Destroy(DestroyMode.Vanish);
					}
				}
				if (caravan2 != caravan)
				{
					caravan2.pawns.TryTransferAllToContainer(caravan.pawns, true);
					caravan2.Destroy();
				}
			}
			caravan.hasShuttleDirty = true;
			caravan.Notify_Merged(caravans);
		}

		// Token: 0x0400EE4A RID: 61002
		private static readonly Texture2D MergeCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/MergeCaravans", true);

		// Token: 0x0400EE4B RID: 61003
		private static List<Caravan> tmpSelectedPlayerCaravans = new List<Caravan>();

		// Token: 0x0400EE4C RID: 61004
		private static List<Caravan> tmpCaravansOnSameTile = new List<Caravan>();

		// Token: 0x0400EE4D RID: 61005
		private static List<Caravan> tmpShuttleCaravans = new List<Caravan>();
	}
}
