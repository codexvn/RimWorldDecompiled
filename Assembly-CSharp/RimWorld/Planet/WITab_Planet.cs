using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E70 RID: 15984
	public class WITab_Planet : WITab
	{
		// Token: 0x170036A1 RID: 13985
		// (get) Token: 0x06015DDB RID: 89563 RVA: 0x0068A338 File Offset: 0x00688538
		public override bool IsVisible
		{
			get
			{
				return base.SelPlanetTile.Valid;
			}
		}

		// Token: 0x170036A2 RID: 13986
		// (get) Token: 0x06015DDC RID: 89564 RVA: 0x0068A354 File Offset: 0x00688554
		private string Desc
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("PlanetSeed".Translate());
				stringBuilder.Append(": ");
				stringBuilder.AppendLine(Find.World.info.seedString);
				stringBuilder.Append("PlanetCoverageShort".Translate());
				stringBuilder.Append(": ");
				stringBuilder.AppendLine(Find.World.info.planetCoverage.ToStringPercent());
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06015DDD RID: 89565 RVA: 0x0068A3E0 File Offset: 0x006885E0
		public WITab_Planet()
		{
			this.size = WITab_Planet.WinSize;
			this.labelKey = "TabPlanet";
		}

		// Token: 0x06015DDE RID: 89566 RVA: 0x0068A400 File Offset: 0x00688600
		protected override void FillTab()
		{
			Rect rect = new Rect(0f, 0f, WITab_Planet.WinSize.x, WITab_Planet.WinSize.y).ContractedBy(10f);
			Text.Font = GameFont.Medium;
			Widgets.Label(rect, Find.World.info.name);
			Rect rect2 = rect;
			rect2.yMin += 35f;
			Text.Font = GameFont.Small;
			Widgets.Label(rect2, this.Desc);
		}

		// Token: 0x0400F29D RID: 62109
		private static readonly Vector2 WinSize = new Vector2(400f, 150f);
	}
}
