using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001DCC RID: 7628
	public class IncidentWorker_ShamblerSwarm : IncidentWorker_EntitySwarm
	{
		// Token: 0x17001A14 RID: 6676
		// (get) Token: 0x0600AA33 RID: 43571 RVA: 0x0030B066 File Offset: 0x00309266
		protected virtual IntRange ShamblerLifespanTicksRange { get; } = new IntRange(150000, 210000);

		// Token: 0x17001A15 RID: 6677
		// (get) Token: 0x0600AA34 RID: 43572 RVA: 0x0030B06E File Offset: 0x0030926E
		protected override PawnGroupKindDef GroupKindDef
		{
			get
			{
				return PawnGroupKindDefOf.Shamblers;
			}
		}

		// Token: 0x0600AA35 RID: 43573 RVA: 0x0030B075 File Offset: 0x00309275
		protected override LordJob GenerateLordJob(IntVec3 entry, IntVec3 dest)
		{
			return new LordJob_ShamblerSwarm(entry, dest);
		}

		// Token: 0x0600AA36 RID: 43574 RVA: 0x0030B080 File Offset: 0x00309280
		protected override List<Pawn> GenerateEntities(IncidentParms parms, float points)
		{
			List<Pawn> list = base.GenerateEntities(parms, points);
			this.SetupShamblerHediffs(list, this.ShamblerLifespanTicksRange);
			return list;
		}

		// Token: 0x0600AA37 RID: 43575 RVA: 0x0030B0A4 File Offset: 0x003092A4
		protected void SetupShamblerHediffs(List<Pawn> shamblers, IntRange lifespanRange)
		{
			foreach (Pawn pawn in shamblers)
			{
				Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Shambler, false);
				HediffComp_DisappearsAndKills hediffComp_DisappearsAndKills = (firstHediffOfDef != null) ? firstHediffOfDef.TryGetComp<HediffComp_DisappearsAndKills>() : null;
				if (firstHediffOfDef == null || hediffComp_DisappearsAndKills == null)
				{
					Log.ErrorOnce("ShamblerSwarm spawned pawn without Shambler hediff", 63426234);
				}
				else
				{
					hediffComp_DisappearsAndKills.disabled = false;
					hediffComp_DisappearsAndKills.ticksToDisappear = lifespanRange.RandomInRange;
				}
			}
		}

		// Token: 0x0600AA38 RID: 43576 RVA: 0x0030B138 File Offset: 0x00309338
		protected override void SendLetter(IncidentParms parms, List<Pawn> entities)
		{
			TaggedString baseLetterLabel = (entities.Count > 1) ? "LetterLabelShamblerSwarmArrived".Translate() : "LetterLabelShamblerArrived".Translate();
			TaggedString taggedString = (entities.Count > 1) ? "LetterShamblerSwarmArrived".Translate(entities.Count) : "LetterShamblerArrived".Translate();
			int num = entities.Count((Pawn e) => e.kindDef == PawnKindDefOf.ShamblerGorehulk);
			if (num == 1)
			{
				taggedString += "\n\n" + "LetterText_ShamblerGorehulk".Translate();
			}
			else if (num > 1)
			{
				taggedString += "\n\n" + "LetterText_ShamblerGorehulkPlural".Translate();
			}
			base.SendStandardLetter(baseLetterLabel, taggedString, this.def.letterDef, parms, entities, Array.Empty<NamedArgument>());
		}
	}
}
