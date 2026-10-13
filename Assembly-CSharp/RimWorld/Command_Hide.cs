using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020030DB RID: 12507
	[StaticConstructorOnStartup]
	public abstract class Command_Hide : Command_Toggle
	{
		// Token: 0x17002D93 RID: 11667
		// (get) Token: 0x06011915 RID: 71957 RVA: 0x0050C0EC File Offset: 0x0050A2EC
		public IHideable Hideable
		{
			get
			{
				return this.hideable;
			}
		}

		// Token: 0x17002D94 RID: 11668
		// (get) Token: 0x06011916 RID: 71958 RVA: 0x0050C0F4 File Offset: 0x0050A2F4
		public override string Label
		{
			get
			{
				return this.Hideable.Hidden ? "CommandUnhideLabel".Translate() : "CommandHideLabel".Translate();
			}
		}

		// Token: 0x06011917 RID: 71959 RVA: 0x0050C120 File Offset: 0x0050A320
		public Command_Hide(IHideable hideable)
		{
			this.hideable = hideable;
			this.icon = ContentFinder<Texture2D>.Get("UI/Commands/HideZone", true);
			this.defaultDesc = "CommandHideZoneDesc".Translate();
			this.hotKey = KeyBindingDefOf.Misc2;
			this.isActive = (() => hideable.Hidden);
			this.toggleAction = delegate()
			{
				hideable.Hidden = !hideable.Hidden;
			};
		}

		// Token: 0x06011918 RID: 71960
		protected abstract IEnumerable<FloatMenuOption> GetOptions();

		// Token: 0x17002D95 RID: 11669
		// (get) Token: 0x06011919 RID: 71961 RVA: 0x0050C1A1 File Offset: 0x0050A3A1
		public override IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
		{
			get
			{
				foreach (FloatMenuOption floatMenuOption in this.GetOptions())
				{
					yield return floatMenuOption;
				}
				IEnumerator<FloatMenuOption> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x0601191A RID: 71962 RVA: 0x0050C1B1 File Offset: 0x0050A3B1
		protected static IEnumerable<FloatMenuOption> ZoneTypeOptions<T>(string label, Texture2D zoneIcon) where T : Zone
		{
			yield return new FloatMenuOption("ShowAllZoneTypes".Translate(label), delegate()
			{
				Command_Hide.ToggleZones<T>(false);
			}, zoneIcon, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false);
			yield return new FloatMenuOption("HideAllZoneTypes".Translate(label), delegate()
			{
				Command_Hide.ToggleZones<T>(true);
			}, zoneIcon, Color.gray, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false);
			yield break;
		}

		// Token: 0x0601191B RID: 71963 RVA: 0x0050C1C8 File Offset: 0x0050A3C8
		protected static void ToggleAll(bool hidden)
		{
			foreach (Zone zone in Find.CurrentMap.zoneManager.AllZones)
			{
				zone.Hidden = hidden;
			}
			Command_Hide.TogglePlans(hidden);
		}

		// Token: 0x0601191C RID: 71964 RVA: 0x0050C228 File Offset: 0x0050A428
		protected static void TogglePlans(bool hidden)
		{
			foreach (Plan plan in Find.CurrentMap.planManager.AllPlans)
			{
				plan.Hidden = hidden;
			}
		}

		// Token: 0x0601191D RID: 71965 RVA: 0x0050C284 File Offset: 0x0050A484
		protected static void ToggleZones<T>(bool hidden) where T : Zone
		{
			foreach (Zone zone in Find.CurrentMap.zoneManager.AllZones)
			{
				if (zone is T)
				{
					zone.Hidden = hidden;
				}
			}
		}

		// Token: 0x0400B52D RID: 46381
		protected readonly IHideable hideable;
	}
}
