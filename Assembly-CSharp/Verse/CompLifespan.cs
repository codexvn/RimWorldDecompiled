using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000902 RID: 2306
	public class CompLifespan : ThingComp
	{
		// Token: 0x17000B13 RID: 2835
		// (get) Token: 0x06003B88 RID: 15240 RVA: 0x0013A4C0 File Offset: 0x001386C0
		public CompProperties_Lifespan Props
		{
			get
			{
				return (CompProperties_Lifespan)this.props;
			}
		}

		// Token: 0x06003B89 RID: 15241 RVA: 0x0013A4CD File Offset: 0x001386CD
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
		}

		// Token: 0x06003B8A RID: 15242 RVA: 0x0013A4E7 File Offset: 0x001386E7
		public override void CompTick()
		{
			this.age++;
			if (this.age >= this.Props.lifespanTicks)
			{
				this.Expire();
			}
		}

		// Token: 0x06003B8B RID: 15243 RVA: 0x0013A510 File Offset: 0x00138710
		public override void CompTickRare()
		{
			this.age += 250;
			if (this.age >= this.Props.lifespanTicks)
			{
				this.Expire();
			}
		}

		// Token: 0x06003B8C RID: 15244 RVA: 0x0013A540 File Offset: 0x00138740
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			string result = "";
			int num = this.Props.lifespanTicks - this.age;
			if (num > 0)
			{
				result = "LifespanExpiry".Translate() + " " + num.ToStringTicksToPeriod(true, false, true, true, false).Colorize(ColoredText.DateTimeColor);
				if (!text.NullOrEmpty())
				{
					result = "\n" + text;
				}
			}
			return result;
		}

		// Token: 0x06003B8D RID: 15245 RVA: 0x0013A5BC File Offset: 0x001387BC
		protected void Expire()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			if (this.Props.expireEffect != null)
			{
				this.Props.expireEffect.Spawn(this.parent.Position, this.parent.Map, 1f).Cleanup();
			}
			if (this.Props.plantDefToSpawn != null && this.Props.plantDefToSpawn.CanNowPlantAt(this.parent.Position, this.parent.Map, false))
			{
				GenSpawn.Spawn(this.Props.plantDefToSpawn, this.parent.Position, this.parent.Map, WipeMode.Vanish);
			}
			this.parent.Destroy(DestroyMode.KillFinalize);
		}

		// Token: 0x04002AF8 RID: 11000
		public int age = -1;
	}
}
