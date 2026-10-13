using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BBF RID: 11199
	public class CompCauseGameCondition_PsychicSuppression : CompCauseGameCondition
	{
		// Token: 0x0600FA6E RID: 64110 RVA: 0x0048C6D3 File Offset: 0x0048A8D3
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.gender = (Rand.Bool ? Gender.Male : Gender.Female);
		}

		// Token: 0x0600FA6F RID: 64111 RVA: 0x0048C6ED File Offset: 0x0048A8ED
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
		}

		// Token: 0x0600FA70 RID: 64112 RVA: 0x0048C707 File Offset: 0x0048A907
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!Prefs.DevMode || !DebugSettings.godMode)
			{
				yield break;
			}
			yield return new Command_Action
			{
				defaultLabel = this.gender.GetLabel(false),
				action = delegate()
				{
					if (this.gender == Gender.Female)
					{
						this.gender = Gender.Male;
					}
					else
					{
						this.gender = Gender.Female;
					}
					base.ReSetupAllConditions();
				},
				hotKey = KeyBindingDefOf.Misc1
			};
			yield break;
		}

		// Token: 0x0600FA71 RID: 64113 RVA: 0x0048C718 File Offset: 0x0048A918
		public override void CompTick()
		{
			base.CompTick();
			if (!this.Active || !base.MyTile.Valid)
			{
				return;
			}
			foreach (Caravan caravan in Find.World.worldObjects.Caravans)
			{
				if (Find.WorldGrid.ApproxDistanceInTiles(caravan.Tile, base.MyTile) < (float)base.Props.worldRange)
				{
					foreach (Pawn pawn in caravan.pawns)
					{
						GameCondition_PsychicSuppression.CheckPawn(pawn, this.gender);
					}
				}
			}
		}

		// Token: 0x0600FA72 RID: 64114 RVA: 0x0048C7F8 File Offset: 0x0048A9F8
		protected override void SetupCondition(GameCondition condition, Map map)
		{
			base.SetupCondition(condition, map);
			((GameCondition_PsychicSuppression)condition).gender = this.gender;
		}

		// Token: 0x0600FA73 RID: 64115 RVA: 0x0048C814 File Offset: 0x0048AA14
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + ("AffectedGender".Translate() + ": " + this.gender.GetLabel(false).CapitalizeFirst());
		}

		// Token: 0x0600FA74 RID: 64116 RVA: 0x0048C873 File Offset: 0x0048AA73
		public override void RandomizeSettings(Site site)
		{
			this.gender = (Rand.Bool ? Gender.Male : Gender.Female);
		}

		// Token: 0x0400A48A RID: 42122
		public Gender gender;
	}
}
