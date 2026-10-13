using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B1A RID: 11034
	public static class MoteMaker
	{
		// Token: 0x0600F682 RID: 63106 RVA: 0x004793D6 File Offset: 0x004775D6
		public static Mote MakeStaticMote(IntVec3 cell, Map map, ThingDef moteDef, float scale = 1f)
		{
			return MoteMaker.MakeStaticMote(cell.ToVector3Shifted(), map, moteDef, scale, false, 0f);
		}

		// Token: 0x0600F683 RID: 63107 RVA: 0x004793F0 File Offset: 0x004775F0
		public static Mote MakeStaticMote(Vector3 loc, Map map, ThingDef moteDef, float scale = 1f, bool makeOffscreen = false, float exactRot = 0f)
		{
			if (!makeOffscreen)
			{
				if (!loc.ShouldSpawnMotesAt(map, true) || map.moteCounter.Saturated)
				{
					return null;
				}
			}
			else if (!loc.InBounds(map) || map.moteCounter.Saturated)
			{
				return null;
			}
			Mote mote = (Mote)ThingMaker.MakeThing(moteDef, null);
			GenSpawn.Spawn(mote, loc.ToIntVec3(), map, WipeMode.Vanish);
			mote.exactPosition = loc;
			mote.Scale = scale;
			mote.exactRotation = exactRot;
			return mote;
		}

		// Token: 0x0600F684 RID: 63108 RVA: 0x00479463 File Offset: 0x00477663
		public static void ThrowText(Vector3 loc, Map map, string text, float timeBeforeStartFadeout = -1f)
		{
			MoteMaker.ThrowText(loc, map, text, Color.white, timeBeforeStartFadeout);
		}

		// Token: 0x0600F685 RID: 63109 RVA: 0x00479474 File Offset: 0x00477674
		public static void ThrowText(Vector3 loc, Map map, string text, Color color, float timeBeforeStartFadeout = -1f)
		{
			IntVec3 intVec = loc.ToIntVec3();
			if (!intVec.InBounds(map))
			{
				return;
			}
			MoteText moteText = (MoteText)ThingMaker.MakeThing(ThingDefOf.Mote_Text, null);
			moteText.exactPosition = loc;
			moteText.SetVelocity((float)Rand.Range(5, 35), Rand.Range(0.42f, 0.45f));
			moteText.text = text;
			moteText.textColor = color;
			if (timeBeforeStartFadeout >= 0f)
			{
				moteText.overrideTimeBeforeStartFadeout = timeBeforeStartFadeout;
			}
			GenSpawn.Spawn(moteText, intVec, map, WipeMode.Vanish);
		}

		// Token: 0x0600F686 RID: 63110 RVA: 0x004794F1 File Offset: 0x004776F1
		public static Mote MakeStunOverlay(Thing stunnedThing)
		{
			Mote mote = (Mote)ThingMaker.MakeThing(ThingDefOf.Mote_Stun, null);
			mote.Attach(stunnedThing);
			GenSpawn.Spawn(mote, stunnedThing.Position, stunnedThing.Map, WipeMode.Vanish);
			return mote;
		}

		// Token: 0x0600F687 RID: 63111 RVA: 0x00479524 File Offset: 0x00477724
		public static MoteDualAttached MakeInteractionOverlay(ThingDef moteDef, TargetInfo A, TargetInfo B)
		{
			MoteDualAttached moteDualAttached = (MoteDualAttached)ThingMaker.MakeThing(moteDef, null);
			moteDualAttached.Scale = 0.5f;
			moteDualAttached.Attach(A, B);
			GenSpawn.Spawn(moteDualAttached, A.Cell, A.Map ?? B.Map, WipeMode.Vanish);
			return moteDualAttached;
		}

		// Token: 0x0600F688 RID: 63112 RVA: 0x00479574 File Offset: 0x00477774
		public static MoteDualAttached MakeInteractionOverlay(ThingDef moteDef, TargetInfo A, TargetInfo B, Vector3 offsetA, Vector3 offsetB)
		{
			MoteDualAttached moteDualAttached = (MoteDualAttached)ThingMaker.MakeThing(moteDef, null);
			moteDualAttached.Scale = 0.5f;
			moteDualAttached.Attach(A, B, offsetA, offsetB);
			GenSpawn.Spawn(moteDualAttached, A.Cell, A.Map ?? B.Map, WipeMode.Vanish);
			return moteDualAttached;
		}

		// Token: 0x0600F689 RID: 63113 RVA: 0x004795C4 File Offset: 0x004777C4
		public static Mote MakeAttachedOverlay(Thing thing, ThingDef moteDef, Vector3 offset, float scale = 1f, float solidTimeOverride = -1f)
		{
			Mote mote = (Mote)ThingMaker.MakeThing(moteDef, null);
			mote.Attach(thing, offset, false);
			mote.Scale = scale;
			mote.exactPosition = thing.DrawPos + offset;
			mote.solidTimeOverride = solidTimeOverride;
			GenSpawn.Spawn(mote, thing.Position, thing.MapHeld, WipeMode.Vanish);
			return mote;
		}

		// Token: 0x0600F68A RID: 63114 RVA: 0x00479620 File Offset: 0x00477820
		public static void MakeColonistActionOverlay(Pawn pawn, ThingDef moteDef)
		{
			MoteThrownAttached moteThrownAttached = (MoteThrownAttached)ThingMaker.MakeThing(moteDef, null);
			moteThrownAttached.Attach(pawn);
			moteThrownAttached.exactPosition = pawn.DrawPos;
			moteThrownAttached.Scale = 1.5f;
			moteThrownAttached.SetVelocity(Rand.Range(20f, 25f), 0.4f);
			GenSpawn.Spawn(moteThrownAttached, pawn.Position, pawn.Map, WipeMode.Vanish);
		}

		// Token: 0x0600F68B RID: 63115 RVA: 0x0047968C File Offset: 0x0047788C
		private static MoteBubble ExistingMoteBubbleOn(Pawn pawn)
		{
			if (!pawn.Spawned)
			{
				return null;
			}
			for (int i = 0; i < 4; i++)
			{
				if ((pawn.Position + MoteMaker.UpRightPattern[i]).InBounds(pawn.Map))
				{
					List<Thing> thingList = pawn.Position.GetThingList(pawn.Map);
					for (int j = 0; j < thingList.Count; j++)
					{
						MoteBubble moteBubble = thingList[j] as MoteBubble;
						if (moteBubble != null && moteBubble.link1.Linked && moteBubble.link1.Target.HasThing && moteBubble.link1.Target == pawn)
						{
							return moteBubble;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x0600F68C RID: 63116 RVA: 0x0047974C File Offset: 0x0047794C
		public static MoteBubble MakeMoodThoughtBubble(Pawn pawn, Thought thought)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return null;
			}
			if (!pawn.Spawned)
			{
				return null;
			}
			float num = thought.MoodOffset();
			if (num == 0f)
			{
				return null;
			}
			MoteBubble moteBubble = MoteMaker.ExistingMoteBubbleOn(pawn);
			if (moteBubble != null)
			{
				if (moteBubble.def == ThingDefOf.Mote_Speech)
				{
					return null;
				}
				if (moteBubble.def == ThingDefOf.Mote_ThoughtBad || moteBubble.def == ThingDefOf.Mote_ThoughtGood)
				{
					moteBubble.Destroy(DestroyMode.Vanish);
				}
			}
			MoteBubble moteBubble2 = (MoteBubble)ThingMaker.MakeThing((num > 0f) ? ThingDefOf.Mote_ThoughtGood : ThingDefOf.Mote_ThoughtBad, null);
			moteBubble2.SetupMoteBubble(thought.Icon, null, null);
			moteBubble2.Attach(pawn);
			GenSpawn.Spawn(moteBubble2, pawn.Position, pawn.Map, WipeMode.Vanish);
			return moteBubble2;
		}

		// Token: 0x0600F68D RID: 63117 RVA: 0x00479810 File Offset: 0x00477A10
		public static MoteBubble MakeThoughtBubble(Pawn pawn, string iconPath, bool maintain = false)
		{
			MoteBubble moteBubble = MoteMaker.ExistingMoteBubbleOn(pawn);
			if (moteBubble != null)
			{
				moteBubble.Destroy(DestroyMode.Vanish);
			}
			MoteBubble moteBubble2 = (MoteBubble)ThingMaker.MakeThing(maintain ? ThingDefOf.Mote_ForceJobMaintained : ThingDefOf.Mote_ForceJob, null);
			moteBubble2.SetupMoteBubble(ContentFinder<Texture2D>.Get(iconPath, true), null, null);
			moteBubble2.Attach(pawn);
			GenSpawn.Spawn(moteBubble2, pawn.Position, pawn.Map, WipeMode.Vanish);
			return moteBubble2;
		}

		// Token: 0x0600F68E RID: 63118 RVA: 0x00479880 File Offset: 0x00477A80
		public static MoteBubble MakeInteractionBubble(Pawn initiator, Pawn recipient, ThingDef interactionMote, Texture2D symbol, Color? iconColor = null)
		{
			MoteBubble moteBubble = MoteMaker.ExistingMoteBubbleOn(initiator);
			if (moteBubble != null)
			{
				if (moteBubble.def == ThingDefOf.Mote_Speech)
				{
					moteBubble.Destroy(DestroyMode.Vanish);
				}
				if (moteBubble.def == ThingDefOf.Mote_ThoughtBad || moteBubble.def == ThingDefOf.Mote_ThoughtGood)
				{
					moteBubble.Destroy(DestroyMode.Vanish);
				}
			}
			MoteBubble moteBubble2 = (MoteBubble)ThingMaker.MakeThing(interactionMote, null);
			moteBubble2.SetupMoteBubble(symbol, recipient, iconColor);
			moteBubble2.Attach(initiator);
			GenSpawn.Spawn(moteBubble2, initiator.Position, initiator.Map, WipeMode.Vanish);
			return moteBubble2;
		}

		// Token: 0x0600F68F RID: 63119 RVA: 0x00479904 File Offset: 0x00477B04
		public static MoteBubble MakeSpeechBubble(Pawn initiator, Texture2D symbol)
		{
			MoteBubble moteBubble = MoteMaker.ExistingMoteBubbleOn(initiator);
			if (moteBubble != null)
			{
				if (moteBubble.def == ThingDefOf.Mote_Speech)
				{
					moteBubble.Destroy(DestroyMode.Vanish);
				}
				if (moteBubble.def == ThingDefOf.Mote_ThoughtBad || moteBubble.def == ThingDefOf.Mote_ThoughtGood)
				{
					moteBubble.Destroy(DestroyMode.Vanish);
				}
			}
			MoteBubble moteBubble2 = (MoteBubble)ThingMaker.MakeThing(ThingDefOf.Mote_Speech, null);
			moteBubble2.SetupMoteBubble(symbol, null, null);
			moteBubble2.Attach(initiator);
			GenSpawn.Spawn(moteBubble2, initiator.Position, initiator.Map, WipeMode.Vanish);
			return moteBubble2;
		}

		// Token: 0x0600F690 RID: 63120 RVA: 0x00479994 File Offset: 0x00477B94
		public static void ThrowExplosionCell(IntVec3 cell, Map map, ThingDef moteDef, Color color)
		{
			if (!cell.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			Mote mote = (Mote)ThingMaker.MakeThing(moteDef, null);
			mote.exactRotation = (float)(90 * Rand.RangeInclusive(0, 3));
			mote.exactPosition = cell.ToVector3Shifted();
			mote.instanceColor = color;
			GenSpawn.Spawn(mote, cell, map, WipeMode.Vanish);
			if (Rand.Value < 0.7f)
			{
				FleckMaker.ThrowDustPuff(cell, map, 1.2f);
			}
		}

		// Token: 0x0600F691 RID: 63121 RVA: 0x00479A00 File Offset: 0x00477C00
		public static void ThrowExplosionInteriorMote(Vector3 loc, Map map, ThingDef moteDef)
		{
			if (!loc.ShouldSpawnMotesAt(map, true) || map.moteCounter.SaturatedLowPriority)
			{
				return;
			}
			MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(moteDef, null);
			moteThrown.Scale = Rand.Range(3f, 4.5f);
			moteThrown.rotationRate = Rand.Range(-30f, 30f);
			moteThrown.exactPosition = loc;
			moteThrown.SetVelocity((float)Rand.Range(0, 360), Rand.Range(0.48f, 0.72f));
			GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map, WipeMode.Vanish);
		}

		// Token: 0x0600F692 RID: 63122 RVA: 0x00479A91 File Offset: 0x00477C91
		public static void MakeBombardmentMote(IntVec3 cell, Map map, float scale)
		{
			Mote mote = (Mote)ThingMaker.MakeThing(ThingDefOf.Mote_Bombardment, null);
			mote.exactPosition = cell.ToVector3Shifted();
			mote.Scale = 150f * scale;
			mote.rotationRate = 1.2f;
			GenSpawn.Spawn(mote, cell, map, WipeMode.Vanish);
		}

		// Token: 0x0600F693 RID: 63123 RVA: 0x00479AD1 File Offset: 0x00477CD1
		public static void MakePowerBeamMote(IntVec3 cell, Map map)
		{
			Mote mote = (Mote)ThingMaker.MakeThing(ThingDefOf.Mote_PowerBeam, null);
			mote.exactPosition = cell.ToVector3Shifted();
			mote.Scale = 90f;
			mote.rotationRate = 1.2f;
			GenSpawn.Spawn(mote, cell, map, WipeMode.Vanish);
		}

		// Token: 0x0600F694 RID: 63124 RVA: 0x00479B0F File Offset: 0x00477D0F
		public static void PlaceTempRoof(IntVec3 cell, Map map)
		{
			if (!cell.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			Mote mote = (Mote)ThingMaker.MakeThing(ThingDefOf.Mote_TempRoof, null);
			mote.exactPosition = cell.ToVector3Shifted();
			GenSpawn.Spawn(mote, cell, map, WipeMode.Vanish);
		}

		// Token: 0x0600F695 RID: 63125 RVA: 0x00479B44 File Offset: 0x00477D44
		public static Mote MakeConnectingLine(Vector3 start, Vector3 end, ThingDef moteType, Map map, float width = 1f)
		{
			Vector3 vector = end - start;
			float x = vector.MagnitudeHorizontal();
			Mote mote = MoteMaker.MakeStaticMote(start + vector * 0.5f, map, moteType, 1f, false, 0f);
			if (mote != null)
			{
				mote.linearScale = new Vector3(x, 1f, width);
				mote.exactRotation = Mathf.Atan2(-vector.z, vector.x) * 57.29578f;
			}
			return mote;
		}

		// Token: 0x0400A246 RID: 41542
		private static IntVec3[] UpRightPattern = new IntVec3[]
		{
			new IntVec3(0, 0, 0),
			new IntVec3(1, 0, 0),
			new IntVec3(0, 0, 1),
			new IntVec3(1, 0, 1)
		};
	}
}
