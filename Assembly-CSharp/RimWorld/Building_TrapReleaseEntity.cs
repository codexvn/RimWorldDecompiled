using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200297C RID: 10620
	public abstract class Building_TrapReleaseEntity : Building_Trap
	{
		// Token: 0x17002390 RID: 9104
		// (get) Token: 0x0600E954 RID: 59732
		protected abstract PawnKindDef PawnToSpawn { get; }

		// Token: 0x17002391 RID: 9105
		// (get) Token: 0x0600E955 RID: 59733
		protected abstract int CountToSpawn { get; }

		// Token: 0x17002392 RID: 9106
		// (get) Token: 0x0600E956 RID: 59734 RVA: 0x00002501 File Offset: 0x00000701
		public override bool ShouldShowTrapDamageStat
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600E957 RID: 59735 RVA: 0x0043E860 File Offset: 0x0043CA60
		protected override void Tick()
		{
			base.Tick();
			if (!base.Spawned || base.IsStunned || !this.IsHashIntervalTick(60))
			{
				return;
			}
			Map map = base.Map;
			IEnumerable<Pawn> enumerable = from x in map.mapPawns.AllPawnsSpawned
			where !x.IsPsychologicallyInvisible() && x.HostileTo(this) && x.Position.DistanceTo(base.Position) <= 20f
			select x;
			IntVec3 position = base.Position;
			foreach (Pawn pawn in enumerable)
			{
				if ((base.Faction != Faction.OfPlayer || (!pawn.IsPrisoner && !pawn.Downed)) && GenSight.LineOfSight(position, pawn.Position, map))
				{
					base.Spring(pawn);
					break;
				}
			}
		}

		// Token: 0x0600E958 RID: 59736 RVA: 0x0043E920 File Offset: 0x0043CB20
		public override void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
		{
			if (base.IsStunned)
			{
				base.Kill(dinfo, exactCulprit);
				return;
			}
			base.Spring(((dinfo != null) ? dinfo.GetValueOrDefault().Instigator : null) as Pawn);
		}

		// Token: 0x0600E959 RID: 59737 RVA: 0x0043E964 File Offset: 0x0043CB64
		protected override void SpringSub(Pawn p)
		{
			if (!base.Spawned)
			{
				return;
			}
			SoundDefOf.DroneTrapSpring.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
			if (base.Faction != Faction.OfPlayer)
			{
				Messages.Message("MessageTrapSprung".Translate(this.Named("TRAP")), new LookTargets(base.Position, base.Map), MessageTypeDefOf.NegativeEvent, true);
			}
			for (int i = 0; i < this.CountToSpawn; i++)
			{
				PawnKindDef pawnToSpawn = this.PawnToSpawn;
				Faction faction = base.Faction;
				PawnGenerationContext context = PawnGenerationContext.NonPlayer;
				float? fixedBiologicalAge = new float?(0f);
				float? fixedChronologicalAge = new float?(0f);
				Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(pawnToSpawn, faction, context, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, fixedBiologicalAge, fixedChronologicalAge, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
				pawn.mindState.enemyTarget = p;
				GenSpawn.Spawn(pawn, base.Position, base.Map, WipeMode.Vanish);
			}
		}

		// Token: 0x0600E95A RID: 59738 RVA: 0x00006AAC File Offset: 0x00004CAC
		protected override float SpringChance(Pawn p)
		{
			return 0f;
		}
	}
}
