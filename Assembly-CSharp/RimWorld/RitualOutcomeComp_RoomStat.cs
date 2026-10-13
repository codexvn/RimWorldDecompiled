using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200273B RID: 10043
	public class RitualOutcomeComp_RoomStat : RitualOutcomeComp_Quality
	{
		// Token: 0x170021C4 RID: 8644
		// (get) Token: 0x0600DC66 RID: 56422 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool DataRequired
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600DC67 RID: 56423 RVA: 0x0040E139 File Offset: 0x0040C339
		public override RitualOutcomeComp_Data MakeData()
		{
			return new RitualOutcomeComp_DataRoomStatCached();
		}

		// Token: 0x0600DC68 RID: 56424 RVA: 0x0040E140 File Offset: 0x0040C340
		public override void Tick(LordJob_Ritual ritual, RitualOutcomeComp_Data data, float progressAmount)
		{
			base.Tick(ritual, data, progressAmount);
			RitualOutcomeComp_DataRoomStatCached ritualOutcomeComp_DataRoomStatCached = (RitualOutcomeComp_DataRoomStatCached)data;
			if (ritualOutcomeComp_DataRoomStatCached != null && ritualOutcomeComp_DataRoomStatCached.startingVal == null)
			{
				Room room = ritual.Spot.GetRoom(ritual.Map);
				if (room != null)
				{
					ritualOutcomeComp_DataRoomStatCached.startingVal = new float?(this.PostProcessedRoomStat(room));
				}
			}
		}

		// Token: 0x0600DC69 RID: 56425 RVA: 0x0040E194 File Offset: 0x0040C394
		private float PostProcessedRoomStat(Room room)
		{
			return GenMath.RoundTo(Math.Min(room.GetStat(this.statDef), this.curve.Points[this.curve.PointsCount - 1].x), 0.1f);
		}

		// Token: 0x0600DC6A RID: 56426 RVA: 0x0040E1E1 File Offset: 0x0040C3E1
		public override bool Applies(LordJob_Ritual ritual)
		{
			return ritual.Spot.GetRoom(ritual.Map) != null;
		}

		// Token: 0x0600DC6B RID: 56427 RVA: 0x0040E1F8 File Offset: 0x0040C3F8
		public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			RitualOutcomeComp_DataRoomStatCached ritualOutcomeComp_DataRoomStatCached = (RitualOutcomeComp_DataRoomStatCached)data;
			if (ritualOutcomeComp_DataRoomStatCached != null && ritualOutcomeComp_DataRoomStatCached.startingVal != null)
			{
				return ritualOutcomeComp_DataRoomStatCached.startingVal.Value;
			}
			Room room = ritual.Spot.GetRoom(ritual.Map);
			if (room != null)
			{
				return this.PostProcessedRoomStat(room);
			}
			return 0f;
		}

		// Token: 0x0600DC6C RID: 56428 RVA: 0x0040E24C File Offset: 0x0040C44C
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			float x = 0f;
			if (ritualTarget.Map != null)
			{
				Room room = ritualTarget.Cell.GetRoom(ritualTarget.Map);
				RitualOutcomeComp_DataRoomStatCached ritualOutcomeComp_DataRoomStatCached = (RitualOutcomeComp_DataRoomStatCached)data;
				x = ((room == null) ? 0f : (((ritualOutcomeComp_DataRoomStatCached != null) ? ritualOutcomeComp_DataRoomStatCached.startingVal : null) ?? this.PostProcessedRoomStat(room)));
				if (room != null)
				{
					bool flag = !room.PsychologicallyOutdoors;
				}
			}
			float num = this.curve.Evaluate(x);
			return new QualityFactor
			{
				label = this.label.CapitalizeFirst(),
				count = x.ToString() + " / " + base.MaxValue.ToString(),
				qualityChange = this.ExpectedOffsetDesc(true, num),
				quality = num,
				positive = (num > 0f),
				priority = 0f
			};
		}

		// Token: 0x040094C6 RID: 38086
		public RoomStatDef statDef;
	}
}
