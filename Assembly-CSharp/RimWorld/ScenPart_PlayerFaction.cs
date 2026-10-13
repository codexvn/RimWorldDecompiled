using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020028D3 RID: 10451
	public class ScenPart_PlayerFaction : ScenPart
	{
		// Token: 0x0600E489 RID: 58505 RVA: 0x0042C02C File Offset: 0x0042A22C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<FactionDef>(ref this.factionDef, "factionDef");
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.factionDef == null)
			{
				this.Randomize();
				Log.Error("ScenPart had null faction after loading. Changing to " + this.factionDef.ToStringSafe<FactionDef>());
			}
		}

		// Token: 0x0600E48A RID: 58506 RVA: 0x0042C080 File Offset: 0x0042A280
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			if (Widgets.ButtonText(listing.GetScenPartRect(this, ScenPart.RowHeight), this.factionDef.LabelCap, true, true, true, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (FactionDef localFd2 in from d in DefDatabase<FactionDef>.AllDefs
				where d.isPlayer
				select d)
				{
					FactionDef localFd = localFd2;
					list.Add(new FloatMenuOption(localFd.LabelCap, delegate()
					{
						this.factionDef = localFd;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x0600E48B RID: 58507 RVA: 0x0042C180 File Offset: 0x0042A380
		public override string Summary(Scenario scen)
		{
			return "ScenPart_PlayerFaction".Translate(this.factionDef.label);
		}

		// Token: 0x0600E48C RID: 58508 RVA: 0x0042C1A1 File Offset: 0x0042A3A1
		public override void Randomize()
		{
			this.factionDef = (from fd in DefDatabase<FactionDef>.AllDefs
			where fd.isPlayer
			select fd).RandomElement<FactionDef>();
		}

		// Token: 0x0600E48D RID: 58509 RVA: 0x0042C1D8 File Offset: 0x0042A3D8
		public override void PostWorldGenerate()
		{
			Find.GameInitData.playerFaction = FactionGenerator.NewGeneratedFaction(new FactionGeneratorParms(this.factionDef, default(IdeoGenerationParms), null));
			Find.FactionManager.Add(Find.GameInitData.playerFaction);
		}

		// Token: 0x0600E48E RID: 58510 RVA: 0x0042C228 File Offset: 0x0042A428
		public override void PreMapGenerate()
		{
			Settlement settlement = (Settlement)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Settlement);
			settlement.SetFaction(Find.GameInitData.playerFaction);
			settlement.Tile = Find.GameInitData.startingTile;
			settlement.Name = SettlementNameGenerator.GenerateSettlementName(settlement, Find.GameInitData.playerFaction.def.playerInitialSettlementNameMaker);
			Find.WorldObjects.Add(settlement);
		}

		// Token: 0x0600E48F RID: 58511 RVA: 0x0042C290 File Offset: 0x0042A490
		public override void PostGameStart()
		{
			Find.GameInitData.playerFaction = null;
			TaleRecorder.RecordTale(TaleDefOf.TileSettled, Array.Empty<object>()).customLabel = "NewSettlement".Translate();
		}

		// Token: 0x0600E490 RID: 58512 RVA: 0x0042C2C0 File Offset: 0x0042A4C0
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.factionDef == null)
			{
				yield return "factionDef is null";
			}
			yield break;
		}

		// Token: 0x0600E491 RID: 58513 RVA: 0x0042C2D0 File Offset: 0x0042A4D0
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ ((this.factionDef != null) ? this.factionDef.GetHashCode() : 0);
		}

		// Token: 0x040098F9 RID: 39161
		internal FactionDef factionDef;
	}
}
