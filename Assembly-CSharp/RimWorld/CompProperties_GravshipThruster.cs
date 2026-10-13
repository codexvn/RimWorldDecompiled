using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002EEB RID: 12011
	public class CompProperties_GravshipThruster : CompProperties_GravshipFacility
	{
		// Token: 0x17002BC8 RID: 11208
		// (get) Token: 0x06010F4B RID: 69451 RVA: 0x004E0E46 File Offset: 0x004DF046
		public ShaderTypeDef FlameShaderType
		{
			get
			{
				return this.flameShaderType ?? ShaderTypeDefOf.MoteGlow;
			}
		}

		// Token: 0x06010F4C RID: 69452 RVA: 0x004E0E58 File Offset: 0x004DF058
		public CompProperties_GravshipThruster()
		{
			this.compClass = typeof(CompGravshipThruster);
		}

		// Token: 0x06010F4D RID: 69453 RVA: 0x004E0EC8 File Offset: 0x004DF0C8
		public void GetExclusionZone(IntVec3 pos, Rot4 rot, ref List<IntVec3> exclusionCells)
		{
			if (exclusionCells == null)
			{
				exclusionCells = new List<IntVec3>();
			}
			else
			{
				exclusionCells.Clear();
			}
			for (int i = 0; i < this.exclusionAreaSize.x; i++)
			{
				for (int j = 0; j < this.exclusionAreaSize.z; j++)
				{
					exclusionCells.Add(pos + new IntVec3(i, 0, j).RotatedBy(rot) + this.exclusionAreaOffset.RotatedBy(rot));
				}
			}
		}

		// Token: 0x06010F4E RID: 69454 RVA: 0x004E0F41 File Offset: 0x004DF141
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.exhaustSettings != null && this.exhaustSettings.ExhaustFleckDef != null && this.exhaustSettings.ExhaustFleckDef.fleckSystemClass != null && this.exhaustSettings.ExhaustFleckDef.fleckSystemClass != typeof(FleckSystemThrown))
			{
				yield return "exhaust fleck '" + this.exhaustSettings.ExhaustFleckDef.defName + "' uses a fleckSystemClass other than FleckSystemThrown";
			}
			yield break;
			yield break;
		}

		// Token: 0x0400B04F RID: 45135
		public IntVec3 exclusionAreaSize;

		// Token: 0x0400B050 RID: 45136
		public IntVec3 exclusionAreaOffset;

		// Token: 0x0400B051 RID: 45137
		public int directionInfluence;

		// Token: 0x0400B052 RID: 45138
		public List<Vector3> flameOffsetsPerDirection = new List<Vector3>
		{
			Vector3.zero,
			Vector3.zero,
			Vector3.zero,
			Vector3.zero
		};

		// Token: 0x0400B053 RID: 45139
		public float flameSize;

		// Token: 0x0400B054 RID: 45140
		private ShaderTypeDef flameShaderType;

		// Token: 0x0400B055 RID: 45141
		public List<ShaderParameter> flameShaderParameters = new List<ShaderParameter>();

		// Token: 0x0400B056 RID: 45142
		public CompProperties_GravshipThruster.ExhaustSettings exhaustSettings = new CompProperties_GravshipThruster.ExhaustSettings();

		// Token: 0x02002EEC RID: 12012
		public class ExhaustSettings
		{
			// Token: 0x17002BC9 RID: 11209
			// (get) Token: 0x06010F50 RID: 69456 RVA: 0x004E0F58 File Offset: 0x004DF158
			public FleckDef ExhaustFleckDef
			{
				get
				{
					return this.exhaustFleckDef ?? FleckDefOf.GravshipThrusterExhaust;
				}
			}

			// Token: 0x0400B057 RID: 45143
			public bool enabled = true;

			// Token: 0x0400B058 RID: 45144
			private FleckDef exhaustFleckDef;

			// Token: 0x0400B059 RID: 45145
			public float emissionsPerSecond = 10f;

			// Token: 0x0400B05A RID: 45146
			public Vector3 spawnOffset = new Vector3(0f, 0f, 0f);

			// Token: 0x0400B05B RID: 45147
			public FloatRange spawnRadiusRange = new FloatRange(0f);

			// Token: 0x0400B05C RID: 45148
			public bool inheritGravshipRotation = true;

			// Token: 0x0400B05D RID: 45149
			public bool inheritThrusterRotation = true;

			// Token: 0x0400B05E RID: 45150
			public Vector3 velocity = new Vector3(0f, 0f, -1f);

			// Token: 0x0400B05F RID: 45151
			public FloatRange velocityRotationRange = new FloatRange(0f);

			// Token: 0x0400B060 RID: 45152
			public FloatRange velocityMultiplierRange = new FloatRange(1f);

			// Token: 0x0400B061 RID: 45153
			public FloatRange rotationOverTimeRange = new FloatRange(0f);

			// Token: 0x0400B062 RID: 45154
			public FloatRange scaleRange = new FloatRange(1f);
		}
	}
}
