using System;
using System.Collections.Generic;
using System.Globalization;
using RimWorld.Planet;
using Verse;
using Verse.Noise;

namespace LudeonTK
{
	// Token: 0x0200008F RID: 143
	[StaticConstructorOnStartup]
	public abstract class Dialog_DevNoiseBase : Window_DevListing
	{
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000309 RID: 777 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AutoUpdate
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600030A RID: 778
		protected abstract string Title { get; }

		// Token: 0x0600030B RID: 779
		protected abstract void OnNoiseChanged();

		// Token: 0x0600030C RID: 780
		protected abstract int GetSeed();

		// Token: 0x0600030D RID: 781 RVA: 0x00010A20 File Offset: 0x0000EC20
		public override void PreOpen()
		{
			base.PreOpen();
			this.noise = new Perlin(0.017000000923871994, 2.0, 0.5, 6, true, false, this.GetSeed(), QualityMode.Medium);
		}

		// Token: 0x0600030E RID: 782 RVA: 0x00010A64 File Offset: 0x0000EC64
		protected override void DoWindowListing()
		{
			base.PrintLabel(this.Title, GameFont.Small);
			base.Gap(4f);
			if (this.planetLayer != PlanetLayer.Selected)
			{
				this.RefreshNoiseVars();
			}
			base.Toggle("Sliders: " + (this.toggleSliders ? "Enabled" : "Disabled"), ref this.toggleSliders);
			base.Toggle("Updating: " + (this.toggleUpdate ? "Enabled" : "Disabled"), ref this.toggleUpdate);
			base.Gap(17f);
			if (this.toggleSliders)
			{
				this.DoSliderControls();
			}
			else
			{
				this.DoTextControls();
			}
			base.Gap(17f);
			if (base.SliderFieldInt(string.Format("Seed Offset: {0}", this.seedOffset), ref this.seedOffsetSlider, ref this.seedOffset, -10, 10, null))
			{
				this.noise.Seed = Gen.HashCombineInt(this.GetSeed() + this.seedOffset, this.seedKey);
			}
			if (base.TextFieldInt("Seed part", ref this.seedString, ref this.seedKey, null))
			{
				this.noise.Seed = Gen.HashCombineInt(this.GetSeed() + this.seedOffset, this.seedKey);
			}
			base.Gap(17f);
			if (DevGUI.ButtonText(base.TakeRow(), string.Format("Quality Mode: {0}", this.noise.Quality), null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (object obj in Enum.GetValues(typeof(QualityMode)))
				{
					QualityMode local2 = (QualityMode)obj;
					QualityMode local = local2;
					list.Add(new FloatMenuOption(local.ToStringSafe<QualityMode>(), delegate()
					{
						this.noise.Quality = local;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					base.Dirty = true;
				}
				FloatMenu window = new FloatMenu(list, "Select quality mode", false);
				Find.WindowStack.Add(window);
			}
			bool normalized = this.noise.Normalized;
			if (base.Toggle("Normalized: " + (normalized ? "Enabled" : "Disabled"), ref normalized))
			{
				this.noise.Normalized = normalized;
			}
			bool invert = this.noise.Invert;
			if (base.Toggle("Invert: " + (invert ? "Enabled" : "Disabled"), ref invert))
			{
				this.noise.Invert = invert;
			}
			base.Gap(17f);
			base.SliderFieldFloat(string.Format("Cutoff: {0:0.##}", this.cutoff), ref this.cutoff, ref this.cutoffEnabled, 0f, 1f, null);
			base.SliderFieldFloat(string.Format("Alpha: {0:0}%", this.alpha * 100f), ref this.alpha, 0f, 1f, null);
			base.Gap(17f);
			if (base.Dirty && this.toggleUpdate)
			{
				this.noise.Frequency = this.freq;
				this.noise.Lacunarity = this.lac;
				this.noise.Persistence = this.pers;
				this.noise.OctaveCount = this.oct;
				this.OnNoiseChanged();
				this.OnChanged();
			}
		}

		// Token: 0x0600030F RID: 783 RVA: 0x00010E04 File Offset: 0x0000F004
		private void RefreshNoiseVars()
		{
			this.freq = this.noise.Frequency;
			this.lac = this.noise.Lacunarity;
			this.oct = this.noise.OctaveCount;
			this.pers = this.noise.Persistence;
			this.freqStr = this.freq.ToString(CultureInfo.InvariantCulture);
			this.lacStr = this.lac.ToString(CultureInfo.InvariantCulture);
			this.octStr = this.oct.ToString(CultureInfo.InvariantCulture);
			this.persStr = this.pers.ToString(CultureInfo.InvariantCulture);
			this.planetLayer = PlanetLayer.Selected;
		}

		// Token: 0x06000310 RID: 784 RVA: 0x00010EB8 File Offset: 0x0000F0B8
		private void DoSliderControls()
		{
			base.SliderFieldDouble(string.Format("Frequency: {0:0.####}", this.noise.Frequency), ref this.freq, ref this.freqFine, 0f, 0.2f, "Scale of the noise-map.\n\nLarger values zoom the noise out.\nSmaller values zoom it in.");
			base.SliderFieldDouble(string.Format("Lacunarity: {0:0.##}", this.noise.Lacunarity), ref this.lac, ref this.lacFine, 0f, 5f, "Muliplies frequency at each octave (adjusts frequency)\n\n1: octaves have same frequency.\n< 1: successive octaves are smoother.\n> 1: successive octaves are rougher.\na");
			base.SliderFieldDouble(string.Format("Persistence: {0:0.##}", this.noise.Persistence), ref this.pers, ref this.persFine, 0f, 5f, "How much each octave contributes to the final value (adjusts amplitude)\n\n1: octaves contribute equally.\n< 1: successive octaves contribute less.\n> 1: successive octaves contribute more.\na");
			base.SliderFieldInt(string.Format("Octave Count: {0}", this.noise.OctaveCount), ref this.octSliderPos, ref this.oct, 0, 8, "How many octaves contribute to the final value.\n\nHigher octaves contribute according to lacunarity and persistence values.");
			if (!base.Dirty)
			{
				return;
			}
			this.freqStr = this.freq.ToString(CultureInfo.InvariantCulture);
			this.lacStr = this.lac.ToString(CultureInfo.InvariantCulture);
			this.octStr = this.oct.ToString(CultureInfo.InvariantCulture);
			this.persStr = this.pers.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x06000311 RID: 785 RVA: 0x00011010 File Offset: 0x0000F210
		private void DoTextControls()
		{
			base.TextFieldDouble("Frequency", ref this.freqStr, ref this.freq, "Scale of the noise-map.\n\nLarger values zoom the noise out.\nSmaller values zoom it in.");
			base.TextFieldDouble("Lacunarity", ref this.lacStr, ref this.lac, "Muliplies frequency at each octave (adjusts frequency)\n\n1: octaves have same frequency.\n< 1: successive octaves are smoother.\n> 1: successive octaves are rougher.\na");
			base.TextFieldDouble("Persistence", ref this.persStr, ref this.pers, "How much each octave contributes to the final value (adjusts amplitude)\n\n1: octaves contribute equally.\n< 1: successive octaves contribute less.\n> 1: successive octaves contribute more.\na");
			base.TextFieldInt("Octave Count", ref this.octStr, ref this.oct, "How many octaves contribute to the final value.\n\nHigher octaves contribute according to lacunarity and persistence values.");
		}

		// Token: 0x040001C2 RID: 450
		private int seedOffset;

		// Token: 0x040001C3 RID: 451
		private int seedKey;

		// Token: 0x040001C4 RID: 452
		private string seedString;

		// Token: 0x040001C5 RID: 453
		private string freqStr;

		// Token: 0x040001C6 RID: 454
		private string lacStr;

		// Token: 0x040001C7 RID: 455
		private string octStr;

		// Token: 0x040001C8 RID: 456
		private string persStr;

		// Token: 0x040001C9 RID: 457
		private double freq;

		// Token: 0x040001CA RID: 458
		private double lac;

		// Token: 0x040001CB RID: 459
		private double pers;

		// Token: 0x040001CC RID: 460
		private bool freqFine;

		// Token: 0x040001CD RID: 461
		private bool lacFine;

		// Token: 0x040001CE RID: 462
		private bool persFine;

		// Token: 0x040001CF RID: 463
		private float octSliderPos;

		// Token: 0x040001D0 RID: 464
		private int oct;

		// Token: 0x040001D1 RID: 465
		private float seedOffsetSlider;

		// Token: 0x040001D2 RID: 466
		private bool toggleSliders;

		// Token: 0x040001D3 RID: 467
		private bool toggleUpdate = true;

		// Token: 0x040001D4 RID: 468
		private PlanetLayer planetLayer;

		// Token: 0x040001D5 RID: 469
		public Perlin noise;

		// Token: 0x040001D6 RID: 470
		public float cutoff;

		// Token: 0x040001D7 RID: 471
		public bool cutoffEnabled = true;

		// Token: 0x040001D8 RID: 472
		public float alpha = 1f;

		// Token: 0x040001D9 RID: 473
		private const string TooltipFreq = "Scale of the noise-map.\n\nLarger values zoom the noise out.\nSmaller values zoom it in.";

		// Token: 0x040001DA RID: 474
		private const string TooltipOct = "How many octaves contribute to the final value.\n\nHigher octaves contribute according to lacunarity and persistence values.";

		// Token: 0x040001DB RID: 475
		private const string TooltipLac = "Muliplies frequency at each octave (adjusts frequency)\n\n1: octaves have same frequency.\n< 1: successive octaves are smoother.\n> 1: successive octaves are rougher.\na";

		// Token: 0x040001DC RID: 476
		private const string TooltipPers = "How much each octave contributes to the final value (adjusts amplitude)\n\n1: octaves contribute equally.\n< 1: successive octaves contribute less.\n> 1: successive octaves contribute more.\na";
	}
}
