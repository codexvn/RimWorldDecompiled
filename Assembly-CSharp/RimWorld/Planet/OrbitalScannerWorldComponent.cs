using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C58 RID: 15448
	public class OrbitalScannerWorldComponent : WorldComponent
	{
		// Token: 0x17003411 RID: 13329
		// (get) Token: 0x06014FC6 RID: 85958 RVA: 0x00648414 File Offset: 0x00646614
		private bool OnCooldown
		{
			get
			{
				return this.lastFoundSignal > 0 && Find.TickManager.TicksGame < this.lastFoundSignal + 1080000;
			}
		}

		// Token: 0x06014FC7 RID: 85959 RVA: 0x00648439 File Offset: 0x00646639
		public OrbitalScannerWorldComponent(World world) : base(world)
		{
		}

		// Token: 0x06014FC8 RID: 85960 RVA: 0x00648454 File Offset: 0x00646654
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastFoundSignal, "lastFoundSignal", 0, false);
		}

		// Token: 0x06014FC9 RID: 85961 RVA: 0x00648468 File Offset: 0x00646668
		public override void WorldComponentTick()
		{
			if (this.OnCooldown)
			{
				return;
			}
			if (this.workingScanners.Empty<CompOrbitalScanner>())
			{
				return;
			}
			if (Rand.MTBEventOccurs(60000f, 1f, 1f))
			{
				this.workingScanners.RandomElement<CompOrbitalScanner>().ReceiveSignal();
				this.lastFoundSignal = Find.TickManager.TicksGame;
			}
			this.workingScanners.Clear();
		}

		// Token: 0x06014FCA RID: 85962 RVA: 0x006484CD File Offset: 0x006466CD
		public void Notify_ScannerWorking(CompOrbitalScanner scanner)
		{
			if (this.OnCooldown)
			{
				return;
			}
			this.workingScanners.Add(scanner);
		}

		// Token: 0x0400E9EF RID: 59887
		private int lastFoundSignal = -1;

		// Token: 0x0400E9F0 RID: 59888
		private List<CompOrbitalScanner> workingScanners = new List<CompOrbitalScanner>();

		// Token: 0x0400E9F1 RID: 59889
		private const int FindSignalMTBTicks = 60000;

		// Token: 0x0400E9F2 RID: 59890
		private const int FindSignalCooldownTicks = 1080000;
	}
}
