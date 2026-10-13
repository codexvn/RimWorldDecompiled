using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002E09 RID: 11785
	[StaticConstructorOnStartup]
	public class CompShield : ThingComp
	{
		// Token: 0x17002A67 RID: 10855
		// (get) Token: 0x06010950 RID: 67920 RVA: 0x004C76ED File Offset: 0x004C58ED
		public CompProperties_Shield Props
		{
			get
			{
				return (CompProperties_Shield)this.props;
			}
		}

		// Token: 0x17002A68 RID: 10856
		// (get) Token: 0x06010951 RID: 67921 RVA: 0x004C76FA File Offset: 0x004C58FA
		private float EnergyMax
		{
			get
			{
				return this.parent.GetStatValue(StatDefOf.EnergyShieldEnergyMax, true, -1);
			}
		}

		// Token: 0x17002A69 RID: 10857
		// (get) Token: 0x06010952 RID: 67922 RVA: 0x004C770E File Offset: 0x004C590E
		private float EnergyGainPerTick
		{
			get
			{
				return this.parent.GetStatValue(StatDefOf.EnergyShieldRechargeRate, true, -1) / 60f;
			}
		}

		// Token: 0x17002A6A RID: 10858
		// (get) Token: 0x06010953 RID: 67923 RVA: 0x004C7728 File Offset: 0x004C5928
		public float Energy
		{
			get
			{
				return this.energy;
			}
		}

		// Token: 0x17002A6B RID: 10859
		// (get) Token: 0x06010954 RID: 67924 RVA: 0x004C7730 File Offset: 0x004C5930
		public ShieldState ShieldState
		{
			get
			{
				Pawn pawn = this.parent as Pawn;
				if (pawn != null && (pawn.IsCharging() || pawn.IsSelfShutdown()))
				{
					return ShieldState.Disabled;
				}
				CompCanBeDormant comp = this.parent.GetComp<CompCanBeDormant>();
				if (comp != null && !comp.Awake)
				{
					return ShieldState.Disabled;
				}
				if (this.ticksToReset <= 0)
				{
					return ShieldState.Active;
				}
				return ShieldState.Resetting;
			}
		}

		// Token: 0x17002A6C RID: 10860
		// (get) Token: 0x06010955 RID: 67925 RVA: 0x004C7784 File Offset: 0x004C5984
		protected bool ShouldDisplay
		{
			get
			{
				Pawn pawnOwner = this.PawnOwner;
				return pawnOwner.Spawned && !pawnOwner.Dead && !pawnOwner.Downed && (pawnOwner.InAggroMentalState || pawnOwner.Drafted || (pawnOwner.Faction.HostileTo(Faction.OfPlayer) && !pawnOwner.IsPrisoner) || Find.TickManager.TicksGame < this.lastKeepDisplayTick + this.KeepDisplayingTicks || (ModsConfig.BiotechActive && pawnOwner.IsColonyMech && Find.Selector.SingleSelectedThing == pawnOwner));
			}
		}

		// Token: 0x17002A6D RID: 10861
		// (get) Token: 0x06010956 RID: 67926 RVA: 0x004C781C File Offset: 0x004C5A1C
		protected Pawn PawnOwner
		{
			get
			{
				Apparel apparel = this.parent as Apparel;
				if (apparel != null)
				{
					return apparel.Wearer;
				}
				Pawn pawn = this.parent as Pawn;
				if (pawn != null)
				{
					return pawn;
				}
				return null;
			}
		}

		// Token: 0x17002A6E RID: 10862
		// (get) Token: 0x06010957 RID: 67927 RVA: 0x004C7851 File Offset: 0x004C5A51
		public bool IsApparel
		{
			get
			{
				return this.parent is Apparel;
			}
		}

		// Token: 0x17002A6F RID: 10863
		// (get) Token: 0x06010958 RID: 67928 RVA: 0x004C7861 File Offset: 0x004C5A61
		private bool IsBuiltIn
		{
			get
			{
				return !this.IsApparel;
			}
		}

		// Token: 0x06010959 RID: 67929 RVA: 0x004C786C File Offset: 0x004C5A6C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.energy, "energy", 0f, false);
			Scribe_Values.Look<int>(ref this.ticksToReset, "ticksToReset", -1, false);
			Scribe_Values.Look<int>(ref this.lastKeepDisplayTick, "lastKeepDisplayTick", 0, false);
		}

		// Token: 0x0601095A RID: 67930 RVA: 0x004C78B9 File Offset: 0x004C5AB9
		public override IEnumerable<Gizmo> CompGetWornGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetWornGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.IsApparel)
			{
				foreach (Gizmo gizmo2 in this.GetGizmos())
				{
					yield return gizmo2;
				}
				enumerator = null;
			}
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Break",
					action = new Action(this.Break)
				};
				if (this.ticksToReset > 0)
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: Clear reset",
						action = delegate()
						{
							this.ticksToReset = 0;
						}
					};
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0601095B RID: 67931 RVA: 0x004C78C9 File Offset: 0x004C5AC9
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.IsBuiltIn)
			{
				foreach (Gizmo gizmo2 in this.GetGizmos())
				{
					yield return gizmo2;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0601095C RID: 67932 RVA: 0x004C78D9 File Offset: 0x004C5AD9
		private IEnumerable<Gizmo> GetGizmos()
		{
			if (this.PawnOwner.Faction != Faction.OfPlayer)
			{
				Pawn pawn = this.parent as Pawn;
				if (pawn == null || !pawn.RaceProps.IsMechanoid)
				{
					goto IL_82;
				}
			}
			if (Find.Selector.SingleSelectedThing == this.PawnOwner)
			{
				yield return new Gizmo_EnergyShieldStatus
				{
					shield = this
				};
			}
			IL_82:
			yield break;
		}

		// Token: 0x0601095D RID: 67933 RVA: 0x004C78E9 File Offset: 0x004C5AE9
		public override float CompGetSpecialApparelScoreOffset()
		{
			return this.EnergyMax * this.ApparelScorePerEnergyMax;
		}

		// Token: 0x0601095E RID: 67934 RVA: 0x004C78F8 File Offset: 0x004C5AF8
		public override void CompTick()
		{
			base.CompTick();
			if (this.PawnOwner == null)
			{
				this.energy = 0f;
				return;
			}
			if (this.ShieldState == ShieldState.Resetting)
			{
				this.ticksToReset--;
				if (this.ticksToReset <= 0)
				{
					this.Reset();
					return;
				}
			}
			else if (this.ShieldState == ShieldState.Active)
			{
				this.energy += this.EnergyGainPerTick;
				if (this.energy > this.EnergyMax)
				{
					this.energy = this.EnergyMax;
				}
			}
		}

		// Token: 0x0601095F RID: 67935 RVA: 0x004C797C File Offset: 0x004C5B7C
		public override void PostPreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			absorbed = false;
			if (this.ShieldState != ShieldState.Active || this.PawnOwner == null)
			{
				return;
			}
			if (dinfo.Def == DamageDefOf.EMP)
			{
				this.energy = 0f;
				this.Break();
				return;
			}
			if (dinfo.Def.ignoreShields)
			{
				return;
			}
			if (dinfo.Def.isRanged || dinfo.Def.isExplosive)
			{
				this.energy -= dinfo.Amount * this.Props.energyLossPerDamage;
				if (this.energy < 0f)
				{
					this.Break();
				}
				else
				{
					this.AbsorbedDamage(dinfo);
				}
				absorbed = true;
				return;
			}
		}

		// Token: 0x06010960 RID: 67936 RVA: 0x004C7A28 File Offset: 0x004C5C28
		public void KeepDisplaying()
		{
			this.lastKeepDisplayTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06010961 RID: 67937 RVA: 0x004C7A3C File Offset: 0x004C5C3C
		private void AbsorbedDamage(DamageInfo dinfo)
		{
			SoundDefOf.EnergyShield_AbsorbDamage.PlayOneShot(new TargetInfo(this.PawnOwner.Position, this.PawnOwner.Map, false));
			this.impactAngleVect = Vector3Utility.HorizontalVectorFromAngle(dinfo.Angle);
			Vector3 loc = this.PawnOwner.TrueCenter() + this.impactAngleVect.RotatedBy(180f) * 0.5f;
			float num = Mathf.Min(10f, 2f + dinfo.Amount / 10f);
			FleckMaker.Static(loc, this.PawnOwner.Map, FleckDefOf.ExplosionFlash, num);
			int num2 = (int)num;
			for (int i = 0; i < num2; i++)
			{
				FleckMaker.ThrowDustPuff(loc, this.PawnOwner.Map, Rand.Range(0.8f, 1.2f));
			}
			this.lastAbsorbDamageTick = Find.TickManager.TicksGame;
			this.KeepDisplaying();
		}

		// Token: 0x06010962 RID: 67938 RVA: 0x004C7B2C File Offset: 0x004C5D2C
		private void Break()
		{
			if (this.parent.Spawned)
			{
				float scale = Mathf.Lerp(this.Props.minDrawSize, this.Props.maxDrawSize, this.energy);
				EffecterDefOf.Shield_Break.SpawnAttached(this.parent, this.parent.MapHeld, scale);
				FleckMaker.Static(this.PawnOwner.TrueCenter(), this.PawnOwner.Map, FleckDefOf.ExplosionFlash, 12f);
				for (int i = 0; i < 6; i++)
				{
					FleckMaker.ThrowDustPuff(this.PawnOwner.TrueCenter() + Vector3Utility.HorizontalVectorFromAngle((float)Rand.Range(0, 360)) * Rand.Range(0.3f, 0.6f), this.PawnOwner.Map, Rand.Range(0.8f, 1.2f));
				}
			}
			this.energy = 0f;
			this.ticksToReset = this.Props.startingTicksToReset;
		}

		// Token: 0x06010963 RID: 67939 RVA: 0x004C7C2C File Offset: 0x004C5E2C
		private void Reset()
		{
			if (this.PawnOwner.Spawned)
			{
				SoundDefOf.EnergyShield_Reset.PlayOneShot(new TargetInfo(this.PawnOwner.Position, this.PawnOwner.Map, false));
				FleckMaker.ThrowLightningGlow(this.PawnOwner.TrueCenter(), this.PawnOwner.Map, 3f);
			}
			this.ticksToReset = -1;
			this.energy = this.Props.energyOnReset;
		}

		// Token: 0x06010964 RID: 67940 RVA: 0x004C7CA9 File Offset: 0x004C5EA9
		public override void CompDrawWornExtras()
		{
			base.CompDrawWornExtras();
			if (this.IsApparel)
			{
				this.Draw();
			}
		}

		// Token: 0x06010965 RID: 67941 RVA: 0x004C7CBF File Offset: 0x004C5EBF
		public override void PostDraw()
		{
			base.PostDraw();
			if (this.IsBuiltIn)
			{
				this.Draw();
			}
		}

		// Token: 0x06010966 RID: 67942 RVA: 0x004C7CD8 File Offset: 0x004C5ED8
		private void Draw()
		{
			if (this.ShieldState == ShieldState.Active && this.ShouldDisplay)
			{
				float num = Mathf.Lerp(this.Props.minDrawSize, this.Props.maxDrawSize, this.energy);
				Vector3 vector = this.PawnOwner.Drawer.DrawPos;
				vector.y = AltitudeLayer.MoteOverhead.AltitudeFor();
				int num2 = Find.TickManager.TicksGame - this.lastAbsorbDamageTick;
				if (num2 < 8)
				{
					float num3 = (float)(8 - num2) / 8f * 0.05f;
					vector += this.impactAngleVect * num3;
					num -= num3;
				}
				float angle = (float)Rand.Range(0, 360);
				Vector3 s = new Vector3(num, 1f, num);
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(vector, Quaternion.AngleAxis(angle, Vector3.up), s);
				Graphics.DrawMesh(MeshPool.plane10, matrix, CompShield.BubbleMat, 0);
			}
		}

		// Token: 0x06010967 RID: 67943 RVA: 0x004C7DC7 File Offset: 0x004C5FC7
		public override bool CompAllowVerbCast(Verb verb)
		{
			return !this.Props.blocksRangedWeapons || !(verb is Verb_LaunchProjectile);
		}

		// Token: 0x0400ACD7 RID: 44247
		protected float energy;

		// Token: 0x0400ACD8 RID: 44248
		protected int ticksToReset = -1;

		// Token: 0x0400ACD9 RID: 44249
		protected int lastKeepDisplayTick = -9999;

		// Token: 0x0400ACDA RID: 44250
		private Vector3 impactAngleVect;

		// Token: 0x0400ACDB RID: 44251
		private int lastAbsorbDamageTick = -9999;

		// Token: 0x0400ACDC RID: 44252
		private const float MaxDamagedJitterDist = 0.05f;

		// Token: 0x0400ACDD RID: 44253
		private const int JitterDurationTicks = 8;

		// Token: 0x0400ACDE RID: 44254
		private int KeepDisplayingTicks = 1000;

		// Token: 0x0400ACDF RID: 44255
		private float ApparelScorePerEnergyMax = 0.25f;

		// Token: 0x0400ACE0 RID: 44256
		private static readonly Material BubbleMat = MaterialPool.MatFrom("Other/ShieldBubble", ShaderDatabase.Transparent);
	}
}
