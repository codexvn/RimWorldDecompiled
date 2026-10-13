using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using Verse;

// Token: 0x02000017 RID: 23
public class LayoutWorkerComplex_Mechanitor : LayoutWorkerComplex
{
	// Token: 0x06000058 RID: 88 RVA: 0x00003750 File Offset: 0x00001950
	public LayoutWorkerComplex_Mechanitor(LayoutDef def) : base(def)
	{
	}

	// Token: 0x06000059 RID: 89 RVA: 0x00003764 File Offset: 0x00001964
	public override Faction GetFixedHostileFactionForThreats()
	{
		return Faction.OfMechanoids;
	}

	// Token: 0x0600005A RID: 90 RVA: 0x0000376C File Offset: 0x0000196C
	protected override void PreSpawnThreats(List<LayoutRoom> rooms, Map map, List<Thing> allSpawnedThings)
	{
		LayoutWorkerComplex_Mechanitor.<>c__DisplayClass6_0 CS$<>8__locals1 = new LayoutWorkerComplex_Mechanitor.<>c__DisplayClass6_0();
		base.PreSpawnThreats(rooms, map, allSpawnedThings);
		this.tmpAllRoomRects.Clear();
		this.tmpAllRoomRects.AddRange((from r in rooms
		where r.requiredDef == null
		select r).SelectMany((LayoutRoom r) => r.rects));
		CS$<>8__locals1.bounds = this.tmpAllRoomRects[0];
		for (int i = 0; i < this.tmpAllRoomRects.Count; i++)
		{
			CS$<>8__locals1.bounds = CS$<>8__locals1.bounds.Encapsulate(this.tmpAllRoomRects[i]);
		}
		bool flag = false;
		foreach (CellRect room in this.tmpAllRoomRects.OrderBy(new Func<CellRect, float>(CS$<>8__locals1.<PreSpawnThreats>g__OrderRoomsBy|2)))
		{
			Building_AncientCryptosleepPod item;
			if (this.TryPlaceDeceasedMechanitor(room, map, out item))
			{
				allSpawnedThings.Add(item);
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			Log.Error("Failed to place mechanitor in ancient mechanitor complex.");
		}
		this.tmpAllRoomRects.Clear();
	}

	// Token: 0x0600005B RID: 91 RVA: 0x000038AC File Offset: 0x00001AAC
	private bool TryPlaceDeceasedMechanitor(CellRect room, Map map, out Building_AncientCryptosleepPod casket)
	{
		LayoutWorkerComplex_Mechanitor.<>c__DisplayClass7_0 CS$<>8__locals1;
		CS$<>8__locals1.map = map;
		foreach (IntVec3 intVec in room.Cells.InRandomOrder(null))
		{
			if (LayoutWorkerComplex_Mechanitor.<TryPlaceDeceasedMechanitor>g__CanPlaceCasketAt|7_0(intVec, ref CS$<>8__locals1))
			{
				casket = (Building_AncientCryptosleepPod)GenSpawn.Spawn(ThingDefOf.AncientCryptosleepPod, intVec, CS$<>8__locals1.map, WipeMode.Vanish);
				casket.openedSignal = "MechanitorCasketOpened" + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
				Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.Mechanitor_Basic, Faction.OfAncients, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, true, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, true, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
				pawn.Corpse.Age = LayoutWorkerComplex_Mechanitor.RandomMechanitorCorpseAge.RandomInRange;
				pawn.relations.hidePawnRelations = true;
				pawn.Corpse.GetComp<CompRottable>().RotProgress += (float)pawn.Corpse.Age;
				casket.TryAcceptThing(pawn.Corpse, false);
				SignalAction_Message signalAction_Message = (SignalAction_Message)ThingMaker.MakeThing(ThingDefOf.SignalAction_Message, null);
				signalAction_Message.signalTag = casket.openedSignal;
				signalAction_Message.message = "MessageMechanitorCasketOpened".Translate(pawn, HediffDefOf.MechlinkImplant);
				signalAction_Message.messageType = MessageTypeDefOf.PositiveEvent;
				signalAction_Message.lookTargets = pawn.Corpse;
				GenSpawn.Spawn(signalAction_Message, intVec, CS$<>8__locals1.map, WipeMode.Vanish);
				SignalAction_Letter signalAction_Letter = (SignalAction_Letter)ThingMaker.MakeThing(ThingDefOf.SignalAction_Letter, null);
				signalAction_Letter.signalTag = casket.openedSignal;
				signalAction_Letter.letterDef = LetterDefOf.NeutralEvent;
				signalAction_Letter.letterLabelKey = "LetterLabelMechanitorCasketOpened";
				signalAction_Letter.letterMessageKey = "LetterMechanitorCasketOpened";
				signalAction_Letter.fixedPawnReference = pawn;
				signalAction_Letter.lookTargets = pawn.Corpse;
				GenSpawn.Spawn(signalAction_Letter, intVec, CS$<>8__locals1.map, WipeMode.Vanish);
				SignalAction_SoundOneShot signalAction_SoundOneShot = (SignalAction_SoundOneShot)ThingMaker.MakeThing(ThingDefOf.SignalAction_SoundOneShot, null);
				signalAction_SoundOneShot.signalTag = casket.openedSignal;
				signalAction_SoundOneShot.sound = SoundDefOf.MechlinkCorpseReveal;
				GenSpawn.Spawn(signalAction_SoundOneShot, intVec, CS$<>8__locals1.map, WipeMode.Vanish);
				TriggerUnfogged triggerUnfogged = (TriggerUnfogged)ThingMaker.MakeThing(ThingDefOf.TriggerUnfogged, null);
				triggerUnfogged.signalTag = "MechanitorCasketUnfogged" + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
				GenSpawn.Spawn(triggerUnfogged, casket.Position, CS$<>8__locals1.map, WipeMode.Vanish);
				SignalAction_Letter signalAction_Letter2 = (SignalAction_Letter)ThingMaker.MakeThing(ThingDefOf.SignalAction_Letter, null);
				signalAction_Letter2.signalTag = triggerUnfogged.signalTag;
				signalAction_Letter2.letterDef = LetterDefOf.NeutralEvent;
				signalAction_Letter2.letterLabelKey = "LetterLabelMechanitorCasketFound";
				signalAction_Letter2.letterMessageKey = "LetterMechanitorCasketFound";
				GenSpawn.Spawn(signalAction_Letter2, intVec, CS$<>8__locals1.map, WipeMode.Vanish);
				ScatterDebrisUtility.ScatterFilthAroundThing(casket, CS$<>8__locals1.map, ThingDefOf.Filth_MachineBits, 0.5f, 1, int.MaxValue, null);
				return true;
			}
		}
		casket = null;
		return false;
	}

	// Token: 0x0600005D RID: 93 RVA: 0x00003C30 File Offset: 0x00001E30
	[CompilerGenerated]
	internal static bool <TryPlaceDeceasedMechanitor>g__CanPlaceCasketAt|7_0(IntVec3 cell, ref LayoutWorkerComplex_Mechanitor.<>c__DisplayClass7_0 A_1)
	{
		using (CellRect.Enumerator enumerator = GenAdj.OccupiedRect(cell, Rot4.North, ThingDefOf.AncientCryptosleepPod.Size).ExpandedBy(1).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.GetEdifice(A_1.map) != null)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x04000021 RID: 33
	private const string MechanitorCasketOpenedSignal = "MechanitorCasketOpened";

	// Token: 0x04000022 RID: 34
	private const string MechanitorCasketUnfoggedSignal = "MechanitorCasketUnfogged";

	// Token: 0x04000023 RID: 35
	private static readonly IntRange RandomMechanitorCorpseAge = new IntRange(0, 360000000);

	// Token: 0x04000024 RID: 36
	private readonly List<CellRect> tmpAllRoomRects = new List<CellRect>();
}
