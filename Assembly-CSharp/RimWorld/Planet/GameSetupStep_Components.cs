using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C10 RID: 15376
	public class GameSetupStep_Components : GameSetupStep
	{
		// Token: 0x17003393 RID: 13203
		// (get) Token: 0x06014DA9 RID: 85417 RVA: 0x0063ED95 File Offset: 0x0063CF95
		public override int SeedPart
		{
			get
			{
				return 508565678;
			}
		}

		// Token: 0x06014DAA RID: 85418 RVA: 0x0063ED9C File Offset: 0x0063CF9C
		public override void GenerateFresh()
		{
			Find.World.ConstructComponents();
		}

		// Token: 0x06014DAB RID: 85419 RVA: 0x0063EDA8 File Offset: 0x0063CFA8
		public override void GenerateWithoutWorldData()
		{
			this.GenerateFromScribe();
		}

		// Token: 0x06014DAC RID: 85420 RVA: 0x0063EDB0 File Offset: 0x0063CFB0
		public override void GenerateFromScribe()
		{
			Find.World.ConstructComponents();
			Find.World.ExposeComponents();
		}
	}
}
