using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020024B9 RID: 9401
	public class Pawn_FilthTracker : IExposable
	{
		// Token: 0x17001F26 RID: 7974
		// (get) Token: 0x0600CC4E RID: 52302 RVA: 0x003BDCE0 File Offset: 0x003BBEE0
		public string FilthReport
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("FilthOnFeet".Translate());
				if (this.carriedFilth.Count == 0)
				{
					stringBuilder.Append("(" + "NoneLower".Translate() + ")");
				}
				else
				{
					for (int i = 0; i < this.carriedFilth.Count; i++)
					{
						stringBuilder.AppendLine(this.carriedFilth[i].LabelCap);
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17001F27 RID: 7975
		// (get) Token: 0x0600CC4F RID: 52303 RVA: 0x003BDD76 File Offset: 0x003BBF76
		public List<Filth> CarriedFilthListForReading
		{
			get
			{
				return this.carriedFilth;
			}
		}

		// Token: 0x17001F28 RID: 7976
		// (get) Token: 0x0600CC50 RID: 52304 RVA: 0x003BDD7E File Offset: 0x003BBF7E
		private FilthSourceFlags AdditionalFilthSourceFlags
		{
			get
			{
				if (this.pawn.Faction != null || !this.pawn.RaceProps.Animal)
				{
					return FilthSourceFlags.Unnatural;
				}
				return FilthSourceFlags.Natural;
			}
		}

		// Token: 0x0600CC51 RID: 52305 RVA: 0x003BDDA2 File Offset: 0x003BBFA2
		public Pawn_FilthTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600CC52 RID: 52306 RVA: 0x003BDDBC File Offset: 0x003BBFBC
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.lastTerrainFilthDef, "lastTerrainFilthDef");
			Scribe_Collections.Look<Filth>(ref this.carriedFilth, "carriedFilth", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.carriedFilth.RemoveAll((Filth x) => x == null) != 0)
				{
					Log.Error(this.pawn.ToStringSafe<Pawn>() + " had null carried filth after loading.");
				}
				if (this.carriedFilth.RemoveAll((Filth x) => x.def == null) != 0)
				{
					Log.Error(this.pawn.ToStringSafe<Pawn>() + " had carried filth with null def after loading.");
				}
			}
		}

		// Token: 0x0600CC53 RID: 52307 RVA: 0x003BDE88 File Offset: 0x003BC088
		public void Notify_EnteredNewCell()
		{
			if (this.pawn.Flying)
			{
				return;
			}
			if (Rand.Value < 0.05f)
			{
				this.TryDropFilth();
			}
			if (Rand.Value < 0.1f)
			{
				this.TryPickupFilth();
			}
			if (Rand.Value < this.pawn.GetStatValue(StatDefOf.FilthRate, true, -1) * 0.005f)
			{
				if (this.pawn.RaceProps.Humanlike)
				{
					ThingDef filth_Trash;
					if (this.lastTerrainFilthDef != null && Rand.Chance(0.66f))
					{
						filth_Trash = this.lastTerrainFilthDef;
					}
					else
					{
						filth_Trash = ThingDefOf.Filth_Trash;
					}
					if (FilthMaker.TryMakeFilth(this.pawn.Position, this.pawn.Map, filth_Trash, 1, this.AdditionalFilthSourceFlags | FilthSourceFlags.Pawn, true))
					{
						FilthMonitor.Notify_FilthHumanGenerated();
						return;
					}
				}
				else if (this.pawn.RaceProps.Insect)
				{
					if (FilthMaker.TryMakeFilth(this.pawn.Position, this.pawn.Map, ThingDefOf.Filth_Slime, 1, this.AdditionalFilthSourceFlags | FilthSourceFlags.Pawn, true))
					{
						FilthMonitor.Notify_FilthAnimalGenerated();
						return;
					}
				}
				else if (this.pawn.IsAnimal && FilthMaker.TryMakeFilth(this.pawn.Position, this.pawn.Map, ThingDefOf.Filth_AnimalFilth, 1, this.AdditionalFilthSourceFlags | FilthSourceFlags.Pawn, true))
				{
					FilthMonitor.Notify_FilthAnimalGenerated();
				}
			}
		}

		// Token: 0x0600CC54 RID: 52308 RVA: 0x003BDFD4 File Offset: 0x003BC1D4
		private void TryPickupFilth()
		{
			TerrainDef terrDef = this.pawn.Map.terrainGrid.TerrainAt(this.pawn.Position);
			if (terrDef.generatedFilth != null)
			{
				for (int i = this.carriedFilth.Count - 1; i >= 0; i--)
				{
					if (this.carriedFilth[i].def.filth.TerrainSourced && this.carriedFilth[i].def != terrDef.generatedFilth)
					{
						this.ThinCarriedFilth(this.carriedFilth[i]);
					}
				}
				Filth filth = this.carriedFilth.FirstOrDefault((Filth f) => f.def == terrDef.generatedFilth);
				if (filth == null || filth.thickness < 1)
				{
					this.GainFilth(terrDef.generatedFilth);
					FilthMonitor.Notify_FilthAccumulated();
				}
			}
			List<Thing> thingList = this.pawn.Position.GetThingList(this.pawn.Map);
			for (int j = thingList.Count - 1; j >= 0; j--)
			{
				Filth filth2 = thingList[j] as Filth;
				if (filth2 != null && filth2.CanFilthAttachNow)
				{
					this.GainFilth(filth2.def, filth2.sources);
					filth2.ThinFilth();
				}
			}
			if (!this.pawn.Position.Roofed(this.pawn.Map))
			{
				foreach (GameCondition gameCondition in this.pawn.Map.GameConditionManager.ActiveConditions)
				{
					if (gameCondition.def.spreadsFilth != null)
					{
						this.GainFilth(gameCondition.def.spreadsFilth, Gen.YieldSingle<string>(gameCondition.Label));
					}
				}
			}
		}

		// Token: 0x0600CC55 RID: 52309 RVA: 0x003BE1C0 File Offset: 0x003BC3C0
		private void TryDropFilth()
		{
			if (this.carriedFilth.Count == 0)
			{
				return;
			}
			for (int i = this.carriedFilth.Count - 1; i >= 0; i--)
			{
				if (this.carriedFilth[i].CanDropAt(this.pawn.Position, this.pawn.Map, FilthSourceFlags.None))
				{
					this.DropCarriedFilth(this.carriedFilth[i]);
					FilthMonitor.Notify_FilthDropped();
				}
			}
		}

		// Token: 0x0600CC56 RID: 52310 RVA: 0x003BE234 File Offset: 0x003BC434
		private void DropCarriedFilth(Filth f)
		{
			if (FilthMaker.TryMakeFilth(this.pawn.Position, this.pawn.Map, f.def, f.sources, this.AdditionalFilthSourceFlags))
			{
				this.ThinCarriedFilth(f);
			}
		}

		// Token: 0x0600CC57 RID: 52311 RVA: 0x003BE26C File Offset: 0x003BC46C
		private void ThinCarriedFilth(Filth f)
		{
			f.ThinFilth();
			if (f.thickness <= 0)
			{
				this.carriedFilth.Remove(f);
			}
		}

		// Token: 0x0600CC58 RID: 52312 RVA: 0x003BE28A File Offset: 0x003BC48A
		public void GainFilth(ThingDef filthDef)
		{
			if (filthDef.filth.TerrainSourced)
			{
				this.lastTerrainFilthDef = filthDef;
			}
			this.GainFilth(filthDef, null);
		}

		// Token: 0x0600CC59 RID: 52313 RVA: 0x003BE2A8 File Offset: 0x003BC4A8
		public void GainFilth(ThingDef filthDef, IEnumerable<string> sources)
		{
			if (filthDef.filth.TerrainSourced)
			{
				this.lastTerrainFilthDef = filthDef;
			}
			Filth filth = null;
			for (int i = 0; i < this.carriedFilth.Count; i++)
			{
				if (this.carriedFilth[i].def == filthDef)
				{
					filth = this.carriedFilth[i];
					break;
				}
			}
			if (filth != null)
			{
				if (filth.CanBeThickened)
				{
					filth.ThickenFilth();
					filth.AddSources(sources);
					return;
				}
			}
			else
			{
				Filth filth2 = (Filth)ThingMaker.MakeThing(filthDef, null);
				filth2.AddSources(sources);
				this.carriedFilth.Add(filth2);
			}
		}

		// Token: 0x04008B43 RID: 35651
		private readonly Pawn pawn;

		// Token: 0x04008B44 RID: 35652
		private List<Filth> carriedFilth = new List<Filth>();

		// Token: 0x04008B45 RID: 35653
		private ThingDef lastTerrainFilthDef;

		// Token: 0x04008B46 RID: 35654
		private const float FilthPickupChance = 0.1f;

		// Token: 0x04008B47 RID: 35655
		private const float FilthDropChance = 0.05f;

		// Token: 0x04008B48 RID: 35656
		private const int MaxCarriedTerrainFilthThickness = 1;

		// Token: 0x04008B49 RID: 35657
		private const float BaseChanceToSpreadPerCell = 0.005f;
	}
}
