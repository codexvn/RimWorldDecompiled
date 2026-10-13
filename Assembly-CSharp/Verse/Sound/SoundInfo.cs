using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x02000E37 RID: 3639
	public struct SoundInfo
	{
		// Token: 0x17000EC0 RID: 3776
		// (get) Token: 0x06005BD0 RID: 23504 RVA: 0x001D5CF6 File Offset: 0x001D3EF6
		// (set) Token: 0x06005BD1 RID: 23505 RVA: 0x001D5CFE File Offset: 0x001D3EFE
		public bool IsOnCamera { readonly get; private set; }

		// Token: 0x17000EC1 RID: 3777
		// (get) Token: 0x06005BD2 RID: 23506 RVA: 0x001D5D07 File Offset: 0x001D3F07
		// (set) Token: 0x06005BD3 RID: 23507 RVA: 0x001D5D0F File Offset: 0x001D3F0F
		public TargetInfo Maker { readonly get; private set; }

		// Token: 0x17000EC2 RID: 3778
		// (get) Token: 0x06005BD4 RID: 23508 RVA: 0x001D5D18 File Offset: 0x001D3F18
		// (set) Token: 0x06005BD5 RID: 23509 RVA: 0x001D5D20 File Offset: 0x001D3F20
		public MaintenanceType Maintenance { readonly get; private set; }

		// Token: 0x17000EC3 RID: 3779
		// (get) Token: 0x06005BD6 RID: 23510 RVA: 0x001D5D29 File Offset: 0x001D3F29
		public IEnumerable<KeyValuePair<string, float>> DefinedParameters
		{
			get
			{
				if (this.parameters == null)
				{
					yield break;
				}
				foreach (KeyValuePair<string, float> keyValuePair in this.parameters)
				{
					yield return keyValuePair;
				}
				Dictionary<string, float>.Enumerator enumerator = default(Dictionary<string, float>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x06005BD7 RID: 23511 RVA: 0x001D5D40 File Offset: 0x001D3F40
		public static SoundInfo OnCamera(MaintenanceType maint = MaintenanceType.None)
		{
			SoundInfo result = default(SoundInfo);
			result.IsOnCamera = true;
			result.Maintenance = maint;
			result.Maker = TargetInfo.Invalid;
			result.testPlay = false;
			result.volumeFactor = (result.pitchFactor = 1f);
			return result;
		}

		// Token: 0x06005BD8 RID: 23512 RVA: 0x001D5D90 File Offset: 0x001D3F90
		public static SoundInfo InMap(TargetInfo maker, MaintenanceType maint = MaintenanceType.None)
		{
			SoundInfo result = default(SoundInfo);
			result.IsOnCamera = false;
			result.Maintenance = maint;
			result.Maker = maker;
			result.testPlay = false;
			result.volumeFactor = (result.pitchFactor = 1f);
			return result;
		}

		// Token: 0x06005BD9 RID: 23513 RVA: 0x001D5DDC File Offset: 0x001D3FDC
		public void SetParameter(string key, float value)
		{
			if (this.parameters == null)
			{
				this.parameters = new Dictionary<string, float>();
			}
			this.parameters[key] = value;
		}

		// Token: 0x06005BDA RID: 23514 RVA: 0x001D5DFE File Offset: 0x001D3FFE
		public static implicit operator SoundInfo(TargetInfo source)
		{
			return SoundInfo.InMap(source, MaintenanceType.None);
		}

		// Token: 0x06005BDB RID: 23515 RVA: 0x001D5E07 File Offset: 0x001D4007
		public static implicit operator SoundInfo(Thing sourceThing)
		{
			return SoundInfo.InMap(sourceThing, MaintenanceType.None);
		}

		// Token: 0x06005BDC RID: 23516 RVA: 0x001D5E18 File Offset: 0x001D4018
		public override string ToString()
		{
			string text = null;
			if (this.parameters != null && this.parameters.Count > 0)
			{
				text = "parameters=";
				foreach (KeyValuePair<string, float> keyValuePair in this.parameters)
				{
					text = string.Concat(new string[]
					{
						text,
						keyValuePair.Key.ToString(),
						"-",
						keyValuePair.Value.ToString(),
						" "
					});
				}
			}
			string text2 = null;
			if (this.Maker.HasThing || this.Maker.Cell.IsValid)
			{
				text2 = this.Maker.ToString();
			}
			string text3 = null;
			if (this.Maintenance != MaintenanceType.None)
			{
				text3 = ", Maint=" + this.Maintenance.ToString();
			}
			return string.Concat(new string[]
			{
				"(",
				this.IsOnCamera ? "Camera" : "World from ",
				text2,
				text,
				text3,
				")"
			});
		}

		// Token: 0x0400428B RID: 17035
		private Dictionary<string, float> parameters;

		// Token: 0x0400428C RID: 17036
		public float volumeFactor;

		// Token: 0x0400428D RID: 17037
		public float pitchFactor;

		// Token: 0x0400428E RID: 17038
		public bool testPlay;

		// Token: 0x0400428F RID: 17039
		public bool forcedPlayOnCamera;
	}
}
