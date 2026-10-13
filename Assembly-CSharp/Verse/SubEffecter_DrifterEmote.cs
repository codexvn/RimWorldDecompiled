using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001B3 RID: 435
	public abstract class SubEffecter_DrifterEmote : SubEffecter
	{
		// Token: 0x06000C39 RID: 3129 RVA: 0x0003A14D File Offset: 0x0003834D
		public SubEffecter_DrifterEmote(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06000C3A RID: 3130 RVA: 0x0003A158 File Offset: 0x00038358
		protected void MakeMote(TargetInfo A, int overrideSpawnTick = -1)
		{
			Vector3 vector = (A.HasThing && A.Thing.DrawPosHeld != null) ? A.Thing.DrawPosHeld.Value : A.Cell.ToVector3Shifted();
			if (vector.ShouldSpawnMotesAt(A.Map, true))
			{
				int randomInRange = this.def.burstCount.RandomInRange;
				for (int i = 0; i < randomInRange; i++)
				{
					Mote mote = (Mote)ThingMaker.MakeThing(this.def.moteDef, null);
					mote.Scale = this.def.scale.RandomInRange;
					mote.exactPosition = vector + base.EffectiveOffset + Gen.RandomHorizontalVector(this.def.positionRadius);
					mote.rotationRate = this.def.rotationRate.RandomInRange;
					mote.exactRotation = this.def.rotation.RandomInRange;
					if (overrideSpawnTick != -1)
					{
						mote.ForceSpawnTick(overrideSpawnTick);
					}
					MoteThrown moteThrown = mote as MoteThrown;
					if (moteThrown != null)
					{
						moteThrown.airTimeLeft = this.def.airTime.RandomInRange;
						moteThrown.SetVelocity(this.def.angle.RandomInRange, this.def.speed.RandomInRange);
					}
					if (A.HasThing)
					{
						mote.Attach(A);
					}
					GenSpawn.Spawn(mote, vector.ToIntVec3(), A.Map, WipeMode.Vanish);
				}
			}
		}
	}
}
