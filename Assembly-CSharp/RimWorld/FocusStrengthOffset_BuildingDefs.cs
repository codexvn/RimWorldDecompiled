using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D59 RID: 11609
	public class FocusStrengthOffset_BuildingDefs : FocusStrengthOffset
	{
		// Token: 0x1700297A RID: 10618
		// (get) Token: 0x060104F6 RID: 66806 RVA: 0x004B59B4 File Offset: 0x004B3BB4
		public float MaxOffsetPerBuilding
		{
			get
			{
				if (this.maxOffsetPerBuilding == -3.4028235E+38f)
				{
					for (int i = 0; i < this.defs.Count; i++)
					{
						MeditationFocusOffsetPerBuilding meditationFocusOffsetPerBuilding = this.defs[i];
						if (meditationFocusOffsetPerBuilding.offset > this.maxOffsetPerBuilding)
						{
							this.maxOffsetPerBuilding = meditationFocusOffsetPerBuilding.offset;
						}
					}
				}
				return this.maxOffsetPerBuilding;
			}
		}

		// Token: 0x1700297B RID: 10619
		// (get) Token: 0x060104F7 RID: 66807 RVA: 0x004B5A14 File Offset: 0x004B3C14
		public float MinOffsetPerBuilding
		{
			get
			{
				if (this.minOffsetPerBuilding == 3.4028235E+38f)
				{
					for (int i = 0; i < this.defs.Count; i++)
					{
						MeditationFocusOffsetPerBuilding meditationFocusOffsetPerBuilding = this.defs[i];
						if (meditationFocusOffsetPerBuilding.offset < this.minOffsetPerBuilding)
						{
							this.minOffsetPerBuilding = meditationFocusOffsetPerBuilding.offset;
						}
					}
				}
				return this.minOffsetPerBuilding;
			}
		}

		// Token: 0x060104F8 RID: 66808 RVA: 0x004B5A74 File Offset: 0x004B3C74
		public override float GetOffset(Thing parent, Pawn user = null)
		{
			if (parent.Spawned)
			{
				float num = 0f;
				List<Thing> forCell = parent.Map.listerBuldingOfDefInProximity.GetForCell(parent.Position, this.radius, this.defs, parent);
				int num2 = 0;
				while (num2 < forCell.Count && num2 < this.maxBuildings)
				{
					num += this.OffsetForBuilding(forCell[num2]);
					num2++;
				}
				return num;
			}
			return 0f;
		}

		// Token: 0x060104F9 RID: 66809 RVA: 0x004B5AE4 File Offset: 0x004B3CE4
		protected virtual int BuildingCount(Thing parent)
		{
			if (parent == null || !parent.Spawned)
			{
				return 0;
			}
			return Math.Min(parent.Map.listerBuldingOfDefInProximity.GetForCell(parent.Position, this.radius, this.defs, parent).Count, this.maxBuildings);
		}

		// Token: 0x060104FA RID: 66810 RVA: 0x004B5B31 File Offset: 0x004B3D31
		public override bool CanApply(Thing parent, Pawn user = null)
		{
			return parent.Spawned && this.BuildingCount(parent) > 0;
		}

		// Token: 0x060104FB RID: 66811 RVA: 0x004B5B47 File Offset: 0x004B3D47
		protected virtual float OffsetForBuilding(Thing b)
		{
			return this.OffsetFor(b.def);
		}

		// Token: 0x060104FC RID: 66812 RVA: 0x004B5B58 File Offset: 0x004B3D58
		public override void PostDrawExtraSelectionOverlays(Thing parent, Pawn user = null)
		{
			base.PostDrawExtraSelectionOverlays(parent, user);
			if (this.drawRingRadius)
			{
				GenDraw.DrawRadiusRing(parent.Position, this.radius, PlaceWorker_MeditationOffsetBuildingsNear.RingColor, null);
			}
			List<Thing> forCell = parent.Map.listerBuldingOfDefInProximity.GetForCell(parent.Position, this.radius, this.defs, parent);
			int num = 0;
			while (num < forCell.Count && num < this.maxBuildings)
			{
				GenDraw.DrawLineBetween(parent.TrueCenter(), forCell[num].TrueCenter(), SimpleColor.Green, 0.2f);
				num++;
			}
		}

		// Token: 0x060104FD RID: 66813 RVA: 0x004B5BE8 File Offset: 0x004B3DE8
		public override string GetExplanation(Thing parent)
		{
			if (!parent.Spawned)
			{
				return this.GetExplanationAbstract(parent.def);
			}
			int value = this.BuildingCount(parent);
			return this.explanationKey.Translate(value, this.maxBuildings, this.MinOffsetPerBuilding.ToString("0%"), this.MaxOffsetPerBuilding.ToString("0%"), this.defs[0].building.Named("BUILDING")) + ": " + this.GetOffset(parent, null).ToStringWithSign("0%");
		}

		// Token: 0x060104FE RID: 66814 RVA: 0x004B5CA0 File Offset: 0x004B3EA0
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return this.explanationKeyAbstract.Translate(this.maxBuildings, this.MinOffsetPerBuilding.ToString("0%"), this.MaxOffsetPerBuilding.ToString("0%"), this.defs[0].building.Named("BUILDING")) + ": +0-" + this.MaxOffset(null).ToString("0%");
		}

		// Token: 0x060104FF RID: 66815 RVA: 0x004B5D36 File Offset: 0x004B3F36
		public override float MaxOffset(Thing parent = null)
		{
			return (float)this.maxBuildings * this.MaxOffsetPerBuilding;
		}

		// Token: 0x06010500 RID: 66816 RVA: 0x004B5D48 File Offset: 0x004B3F48
		protected float OffsetFor(ThingDef def)
		{
			MeditationFocusOffsetPerBuilding meditationFocusOffsetPerBuilding = this.defs.FirstOrDefault((MeditationFocusOffsetPerBuilding d) => d.building == def);
			if (meditationFocusOffsetPerBuilding != null)
			{
				return meditationFocusOffsetPerBuilding.offset;
			}
			return 0f;
		}

		// Token: 0x06010501 RID: 66817 RVA: 0x004B5D8C File Offset: 0x004B3F8C
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			foreach (MeditationFocusOffsetPerBuilding meditationFocusOffsetPerBuilding in this.defs)
			{
				if (meditationFocusOffsetPerBuilding.offset == -3.4028235E+38f)
				{
					meditationFocusOffsetPerBuilding.offset = this.offsetPerBuilding;
				}
			}
		}

		// Token: 0x0400AA33 RID: 43571
		public List<MeditationFocusOffsetPerBuilding> defs = new List<MeditationFocusOffsetPerBuilding>();

		// Token: 0x0400AA34 RID: 43572
		public float radius = 10f;

		// Token: 0x0400AA35 RID: 43573
		public int maxBuildings = int.MaxValue;

		// Token: 0x0400AA36 RID: 43574
		public float offsetPerBuilding;

		// Token: 0x0400AA37 RID: 43575
		public bool drawRingRadius = true;

		// Token: 0x0400AA38 RID: 43576
		[NoTranslate]
		public string explanationKey;

		// Token: 0x0400AA39 RID: 43577
		[NoTranslate]
		public string explanationKeyAbstract;

		// Token: 0x0400AA3A RID: 43578
		protected float minOffsetPerBuilding = float.MaxValue;

		// Token: 0x0400AA3B RID: 43579
		protected float maxOffsetPerBuilding = float.MinValue;
	}
}
