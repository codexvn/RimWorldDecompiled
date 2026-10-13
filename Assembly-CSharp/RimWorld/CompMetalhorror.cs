using System;
using System.Collections.Generic;
using System.Text;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002D61 RID: 11617
	public class CompMetalhorror : ThingComp
	{
		// Token: 0x1700297C RID: 10620
		// (get) Token: 0x0601051C RID: 66844 RVA: 0x004B6388 File Offset: 0x004B4588
		public CompCanBeDormant DormantComp
		{
			get
			{
				CompCanBeDormant result;
				if ((result = this.intDormantComp) == null)
				{
					result = (this.intDormantComp = this.parent.GetComp<CompCanBeDormant>());
				}
				return result;
			}
		}

		// Token: 0x1700297D RID: 10621
		// (get) Token: 0x0601051D RID: 66845 RVA: 0x004B63B4 File Offset: 0x004B45B4
		public LordJob_Metalhorror LordJob
		{
			get
			{
				LordJob_Metalhorror result;
				if ((result = this.intLordJob) == null)
				{
					result = (this.intLordJob = (LordJob_Metalhorror)this.Pawn.GetLord().LordJob);
				}
				return result;
			}
		}

		// Token: 0x1700297E RID: 10622
		// (get) Token: 0x0601051E RID: 66846 RVA: 0x004B63E9 File Offset: 0x004B45E9
		public bool Hibernating
		{
			get
			{
				return this.DormantComp.Awake;
			}
		}

		// Token: 0x1700297F RID: 10623
		// (get) Token: 0x0601051F RID: 66847 RVA: 0x004B63F6 File Offset: 0x004B45F6
		public int Biosignature
		{
			get
			{
				ImplantSource implantSource = this.implantSource;
				if (implantSource == null)
				{
					return 0;
				}
				return implantSource.Biosignature;
			}
		}

		// Token: 0x17002980 RID: 10624
		// (get) Token: 0x06010520 RID: 66848 RVA: 0x004B6409 File Offset: 0x004B4609
		public string BiosignatureName
		{
			get
			{
				ImplantSource implantSource = this.implantSource;
				return ((implantSource != null) ? implantSource.BiosignatureName : null) ?? "INVALID";
			}
		}

		// Token: 0x17002981 RID: 10625
		// (get) Token: 0x06010521 RID: 66849 RVA: 0x004B6426 File Offset: 0x004B4626
		public CompProperties_Metalhorror Props
		{
			get
			{
				return (CompProperties_Metalhorror)this.props;
			}
		}

		// Token: 0x17002982 RID: 10626
		// (get) Token: 0x06010522 RID: 66850 RVA: 0x0049B545 File Offset: 0x00499745
		public Pawn Pawn
		{
			get
			{
				return (Pawn)this.parent;
			}
		}

		// Token: 0x06010523 RID: 66851 RVA: 0x004B6434 File Offset: 0x004B4634
		public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (totalDamageDealt <= 0f)
			{
				return;
			}
			Pawn pawn = dinfo.Instigator as Pawn;
			if (pawn != null)
			{
				this.lastDamagedBy = pawn;
			}
		}

		// Token: 0x06010524 RID: 66852 RVA: 0x004B6464 File Offset: 0x004B4664
		public override void PostPostMake()
		{
			this.speedBoostTimer = CompMetalhorror.SpeedBoostTicksRange.RandomInRange;
		}

		// Token: 0x06010525 RID: 66853 RVA: 0x004B6484 File Offset: 0x004B4684
		public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
		{
			FilthMaker.TryMakeFilth(this.Pawn.PositionHeld, prevMap, ThingDefOf.Filth_MetalhorrorDebris, 1, FilthSourceFlags.None, true);
		}

		// Token: 0x06010526 RID: 66854 RVA: 0x004B64A0 File Offset: 0x004B46A0
		public void FindOrCreateEmergedLord()
		{
			foreach (Lord lord in this.Pawn.MapHeld.lordManager.lords)
			{
				LordJob_Metalhorror lordJob_Metalhorror = lord.LordJob as LordJob_Metalhorror;
				if (lordJob_Metalhorror != null && lordJob_Metalhorror.biosignature == this.implantSource.Biosignature)
				{
					lord.AddPawn(this.Pawn);
					this.intLordJob = (LordJob_Metalhorror)lord.LordJob;
					return;
				}
			}
			Lord lord2 = LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_Metalhorror(), this.Pawn.MapHeld, null);
			this.intLordJob = (LordJob_Metalhorror)lord2.LordJob;
			this.intLordJob.biosignature = this.implantSource.Biosignature;
			lord2.AddPawn(this.Pawn);
		}

		// Token: 0x06010527 RID: 66855 RVA: 0x004B658C File Offset: 0x004B478C
		public override void CompTick()
		{
			if (this.Hibernating)
			{
				return;
			}
			this.FilthTick();
			this.SpeedBoostTick();
		}

		// Token: 0x06010528 RID: 66856 RVA: 0x004B65A4 File Offset: 0x004B47A4
		public override void PostPreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			absorbed = false;
			float amount = 0.2f * dinfo.Amount;
			if (dinfo.Def == DamageDefOf.Burn)
			{
				amount = 2f * dinfo.Amount;
			}
			dinfo.SetAmount(amount);
		}

		// Token: 0x06010529 RID: 66857 RVA: 0x004B65E4 File Offset: 0x004B47E4
		public override string CompInspectStringExtra()
		{
			if (this.implantSource == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (this.emergedFrom != null)
			{
				stringBuilder.AppendLine(string.Format("{0}: {1}", "EmergedFrom".Translate(), this.emergedFrom.LabelShort));
			}
			stringBuilder.Append(this.implantSource.GetSourceDesc().CapitalizeFirst() ?? "");
			if (Prefs.DevMode && DebugSettings.godMode && this.Pawn.GetLord() != null)
			{
				stringBuilder.Append(string.Format("\nDEV: Biosignature: {0} ({1} siblings)", this.BiosignatureName, this.Pawn.GetLord().ownedPawns.Count - 1));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0601052A RID: 66858 RVA: 0x004B66A8 File Offset: 0x004B48A8
		private void SpeedBoostTick()
		{
			this.speedBoostTimer--;
			if (this.speedBoostTimer <= 0)
			{
				this.speedBoostTimer = CompMetalhorror.SpeedBoostTicksRange.RandomInRange;
				if (this.Pawn.health.hediffSet.HasHediff(HediffDefOf.MetalhorrorSpeedBoost, false))
				{
					return;
				}
				this.Pawn.health.AddHediff(HediffDefOf.MetalhorrorSpeedBoost, null, null, null);
			}
		}

		// Token: 0x0601052B RID: 66859 RVA: 0x004B6720 File Offset: 0x004B4920
		private void FilthTick()
		{
			if (!this.Pawn.Spawned)
			{
				return;
			}
			this.filthTimer--;
			if (this.filthTimer <= 0)
			{
				CompMetalhorror.filthOptions.Clear();
				this.filthTimer = CompMetalhorror.DropFilthTicksRange.RandomInRange;
				int found = 0;
				this.Pawn.Map.floodFiller.FloodFill(this.Pawn.Position, (IntVec3 x) => x.WalkableBy(this.Pawn.Map, this.Pawn) && !x.InHorDistOf(this.Pawn.Position, 4f) && !PawnUtility.AnyPawnBlockingPathAt(x, this.Pawn, true, false, false, false), delegate(IntVec3 cell)
				{
					List<Thing> thingList = cell.GetThingList(this.Pawn.Map);
					for (int i = 0; i < thingList.Count; i++)
					{
						if (thingList[i].def == ThingDefOf.Filth_GrayFleshNoticeable || thingList[i].def == ThingDefOf.Filth_GrayFlesh)
						{
							int found = found;
							found++;
							if (found >= 12)
							{
								return true;
							}
						}
						else
						{
							CompMetalhorror.filthOptions.Add(cell);
						}
					}
					return false;
				}, int.MaxValue, false, null);
				if (found < 12 && CompMetalhorror.filthOptions.Any<IntVec3>())
				{
					FilthMaker.TryMakeFilth(CompMetalhorror.filthOptions.RandomElement<IntVec3>(), this.Pawn.Map, ThingDefOf.Filth_GrayFlesh, 1, FilthSourceFlags.None, true);
				}
				CompMetalhorror.filthOptions.Clear();
			}
		}

		// Token: 0x0601052C RID: 66860 RVA: 0x004B6808 File Offset: 0x004B4A08
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.filthTimer, "filthTimer", 0, false);
			Scribe_Values.Look<int>(ref this.speedBoostTimer, "speedBoostTimer", 0, false);
			Scribe_References.Look<Pawn>(ref this.lastDamagedBy, "lastDamagedBy", false);
			Scribe_References.Look<Pawn>(ref this.emergedFrom, "emergedFrom", true);
			Scribe_Deep.Look<ImplantSource>(ref this.implantSource, "implantSource", Array.Empty<object>());
		}

		// Token: 0x0400AA40 RID: 43584
		public ImplantSource implantSource;

		// Token: 0x0400AA41 RID: 43585
		public Pawn emergedFrom;

		// Token: 0x0400AA42 RID: 43586
		private Pawn lastDamagedBy;

		// Token: 0x0400AA43 RID: 43587
		private int filthTimer;

		// Token: 0x0400AA44 RID: 43588
		private int speedBoostTimer;

		// Token: 0x0400AA45 RID: 43589
		private CompCanBeDormant intDormantComp;

		// Token: 0x0400AA46 RID: 43590
		private LordJob_Metalhorror intLordJob;

		// Token: 0x0400AA47 RID: 43591
		private static readonly IntRange DropFilthTicksRange = new IntRange(600, 1800);

		// Token: 0x0400AA48 RID: 43592
		private static readonly IntRange SpeedBoostTicksRange = new IntRange(1200, 2400);

		// Token: 0x0400AA49 RID: 43593
		private const int FilthSearchRadius = 4;

		// Token: 0x0400AA4A RID: 43594
		private const int MaxFilth = 12;

		// Token: 0x0400AA4B RID: 43595
		private static readonly List<IntVec3> filthOptions = new List<IntVec3>();
	}
}
