using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

// Token: 0x02000045 RID: 69
public class Command_CallBossgroup : Command
{
	// Token: 0x17000034 RID: 52
	// (get) Token: 0x06000166 RID: 358 RVA: 0x00009812 File Offset: 0x00007A12
	private IEnumerable<FloatMenuOption> FloatMenuOptions
	{
		get
		{
			IEnumerable<BossgroupDef> allDefs = DefDatabase<BossgroupDef>.AllDefs;
			using (IEnumerator<BossgroupDef> enumerator = allDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BossgroupDef bg = enumerator.Current;
					AcceptanceReport report = CallBossgroupUtility.BossgroupEverCallable(this.mechanitor.Pawn, bg, true);
					if (!report)
					{
						yield return new FloatMenuOption("CannotSummon".Translate(bg.boss.kindDef.label) + ": " + report.Reason, null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
					else
					{
						yield return new FloatMenuOption("Summon".Translate(bg.boss.kindDef.label), delegate()
						{
							CallBossgroupUtility.TryStartSummonBossgroupJob(bg, this.mechanitor.Pawn, true);
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
				}
			}
			IEnumerator<BossgroupDef> enumerator = null;
			yield break;
			yield break;
		}
	}

	// Token: 0x17000035 RID: 53
	// (get) Token: 0x06000167 RID: 359 RVA: 0x00009824 File Offset: 0x00007A24
	public override string DescPostfix
	{
		get
		{
			string text = "";
			Dictionary<BossgroupDef, AcceptanceReport> source = DefDatabase<BossgroupDef>.AllDefs.ToDictionary((BossgroupDef b) => b, (BossgroupDef b) => CallBossgroupUtility.BossgroupEverCallable(this.mechanitor.Pawn, b, true));
			foreach (KeyValuePair<BossgroupDef, AcceptanceReport> keyValuePair in from b in source
			where b.Value
			select b)
			{
				text = text + "\n\n" + "ReadyToSummonThreat".Translate(keyValuePair.Key.boss.kindDef.label).Colorize(ColorLibrary.Green).CapitalizeFirst();
			}
			foreach (KeyValuePair<BossgroupDef, AcceptanceReport> keyValuePair2 in from b in source
			where !b.Value
			select b)
			{
				text = text + "\n\n" + ("CannotSummon".Translate(keyValuePair2.Key.boss.kindDef.label) + ": " + keyValuePair2.Value.Reason).Colorize(ColorLibrary.RedReadable);
			}
			return text;
		}
	}

	// Token: 0x06000168 RID: 360 RVA: 0x000099B8 File Offset: 0x00007BB8
	public Command_CallBossgroup(Pawn_MechanitorTracker mechanitor)
	{
		this.mechanitor = mechanitor;
		this.defaultLabel = "CommandCallBossgroup".Translate();
		this.defaultDesc = "CommandCallBossgroupDesc".Translate();
		this.icon = Command_CallBossgroup.BossgroupTex.Texture;
	}

	// Token: 0x06000169 RID: 361 RVA: 0x00009A0C File Offset: 0x00007C0C
	public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
	{
		this.disabled = this.IsDisabled(out this.disabledReason);
		return base.GizmoOnGUI(topLeft, maxWidth, parms);
	}

	// Token: 0x0600016A RID: 362 RVA: 0x00009A2C File Offset: 0x00007C2C
	private bool IsDisabled(out string reason)
	{
		int lastBossgroupCalled = Find.BossgroupManager.lastBossgroupCalled;
		int num = Find.TickManager.TicksGame - lastBossgroupCalled;
		PlanetTile tile = this.mechanitor.Pawn.MapHeld.Tile;
		if (Faction.OfMechanoids == null || Faction.OfMechanoids.deactivated)
		{
			reason = "MechsDisabled".Translate();
			return true;
		}
		if (tile.Valid && tile.LayerDef.isSpace)
		{
			reason = "CannotPerformPlanetLayer".Translate(tile.LayerDef.gerundLabel.Named("GERUND"), tile.LayerDef.label.Named("LAYER")).Resolve();
			return true;
		}
		if (!this.mechanitor.Pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
		{
			reason = "Incapable".Translate().CapitalizeFirst();
			return true;
		}
		if (num < 120000)
		{
			reason = "BossgroupAvailableIn".Translate((120000 - num).ToStringTicksToPeriod(true, false, true, true, false)).CapitalizeFirst();
			return true;
		}
		PawnKindDef pendingBossgroup = CallBossgroupUtility.GetPendingBossgroup();
		if (pendingBossgroup != null)
		{
			reason = "BossgroupIncoming".Translate(pendingBossgroup.label).CapitalizeFirst();
			return true;
		}
		if (this.FloatMenuOptions.All((FloatMenuOption f) => f.action == null))
		{
			reason = null;
			return true;
		}
		reason = null;
		return false;
	}

	// Token: 0x0600016B RID: 363 RVA: 0x00009BC4 File Offset: 0x00007DC4
	public override void ProcessInput(Event ev)
	{
		base.ProcessInput(ev);
		List<FloatMenuOption> list = new List<FloatMenuOption>();
		list.AddRange(this.FloatMenuOptions);
		Find.WindowStack.Add(new FloatMenu(list));
	}

	// Token: 0x040000B0 RID: 176
	private static readonly CachedTexture BossgroupTex = new CachedTexture("UI/Icons/SummonMechThreat");

	// Token: 0x040000B1 RID: 177
	private Pawn_MechanitorTracker mechanitor;
}
