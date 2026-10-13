using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BE4 RID: 7140
	public class QuestPart_BetrayMTB : QuestPart_MTB
	{
		// Token: 0x17001877 RID: 6263
		// (get) Token: 0x06009F16 RID: 40726 RVA: 0x002DF46C File Offset: 0x002DD66C
		protected override float MTBDays
		{
			get
			{
				Map map = null;
				bool flag = false;
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.CanBetray(this.pawns[i]))
					{
						flag = true;
						map = this.pawns[i].MapHeld;
						break;
					}
				}
				if (!flag)
				{
					return -1f;
				}
				int num = 0;
				int num2 = 0;
				List<Pawn> list = map.mapPawns.PawnsInFaction(Faction.OfPlayer);
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].IsColonist)
					{
						num++;
						if (list[j].Downed)
						{
							num2++;
						}
					}
				}
				if (num <= 1)
				{
					return QuestPart_BetrayMTB.DownedColonistsExcept1PctToMTBDaysCurve.Evaluate(1f);
				}
				return QuestPart_BetrayMTB.DownedColonistsExcept1PctToMTBDaysCurve.Evaluate((float)num2 / (float)(num - 1));
			}
		}

		// Token: 0x06009F17 RID: 40727 RVA: 0x002DF545 File Offset: 0x002DD745
		private bool CanBetray(Pawn p)
		{
			return !p.DestroyedOrNull() && !p.Downed && p.IsFreeColonist && !p.InMentalState && !p.IsBurning() && !p.Suspended && p.SpawnedOrAnyParentSpawned;
		}

		// Token: 0x06009F18 RID: 40728 RVA: 0x002DF580 File Offset: 0x002DD780
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x04006E04 RID: 28164
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04006E05 RID: 28165
		private static readonly SimpleCurve DownedColonistsExcept1PctToMTBDaysCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 120f),
				true
			},
			{
				new CurvePoint(1f, 15f),
				true
			}
		};
	}
}
