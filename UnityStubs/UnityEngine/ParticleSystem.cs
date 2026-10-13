using System;
using System.Threading;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityStubs.Internal;

namespace UnityEngine;

public sealed class ParticleSystem : Component
{
	public struct EmitParams
	{
		private Vector3 _position;

		private Vector3 _velocity;

		private Vector3 _startSize3D;

		private Vector3 _rotation3D;

		private Vector3 _angularVelocity3D;

		private float _startLifetime;

		private Color32 _startColor;

		private uint _randomSeed;

		internal bool _positionSet;

		internal bool _velocitySet;

		internal bool _startLifetimeSet;

		internal bool _startSizeSet;

		internal bool _rotationSet;

		internal bool _angularVelocitySet;

		internal bool _startColorSet;

		internal bool _randomSeedSet;

		public Vector3 position
		{
			get
			{
				return _position;
			}
			set
			{
				_position = value;
				_positionSet = true;
			}
		}

		public bool applyShapeToPosition { get; set; }

		public Vector3 velocity
		{
			get
			{
				return _velocity;
			}
			set
			{
				_velocity = value;
				_velocitySet = true;
			}
		}

		public float startLifetime
		{
			get
			{
				return _startLifetime;
			}
			set
			{
				_startLifetime = value;
				_startLifetimeSet = true;
			}
		}

		public float startSize
		{
			get
			{
				return _startSize3D.x;
			}
			set
			{
				_startSize3D = new Vector3(value, value, value);
				_startSizeSet = true;
			}
		}

		public Vector3 startSize3D
		{
			get
			{
				return _startSize3D;
			}
			set
			{
				_startSize3D = value;
				_startSizeSet = true;
			}
		}

		public Vector3 axisOfRotation { get; set; }

		public float rotation
		{
			get
			{
				return _rotation3D.z;
			}
			set
			{
				_rotation3D = new Vector3(0f, 0f, value);
				_rotationSet = true;
			}
		}

		public Vector3 rotation3D
		{
			get
			{
				return _rotation3D;
			}
			set
			{
				_rotation3D = value;
				_rotationSet = true;
			}
		}

		public float angularVelocity
		{
			get
			{
				return _angularVelocity3D.z;
			}
			set
			{
				_angularVelocity3D = new Vector3(0f, 0f, value);
				_angularVelocitySet = true;
			}
		}

		public Vector3 angularVelocity3D
		{
			get
			{
				return _angularVelocity3D;
			}
			set
			{
				_angularVelocity3D = value;
				_angularVelocitySet = true;
			}
		}

		public Color32 startColor
		{
			get
			{
				return _startColor;
			}
			set
			{
				_startColor = value;
				_startColorSet = true;
			}
		}

		public uint randomSeed
		{
			get
			{
				return _randomSeed;
			}
			set
			{
				_randomSeed = value;
				_randomSeedSet = true;
			}
		}

		public int meshIndex { get; set; }

		public void ResetPosition()
		{
			_position = default(Vector3);
			_positionSet = false;
		}

		public void ResetVelocity()
		{
			_velocity = default(Vector3);
			_velocitySet = false;
		}

		public void ResetStartLifetime()
		{
			_startLifetime = 0f;
			_startLifetimeSet = false;
		}

		public void ResetStartSize()
		{
			_startSize3D = default(Vector3);
			_startSizeSet = false;
		}

		public void ResetStartColor()
		{
			_startColor = default(Color32);
			_startColorSet = false;
		}

		public void ResetRandomSeed()
		{
			_randomSeed = 0u;
			_randomSeedSet = false;
		}
	}

	public struct Particle
	{
		public Vector3 position { get; set; }

		public Vector3 velocity { get; set; }

		public Vector3 angularVelocity3D { get; set; }

		public Vector3 rotation3D { get; set; }

		public Vector3 startSize3D { get; set; }

		public Color32 startColor { get; set; }

		public uint randomSeed { get; set; }

		public float remainingLifetime { get; set; }

		public float startLifetime { get; set; }

		public float angularVelocity
		{
			get
			{
				return angularVelocity3D.z;
			}
			set
			{
				angularVelocity3D = new Vector3(angularVelocity3D.x, angularVelocity3D.y, value);
			}
		}

		public float rotation
		{
			get
			{
				return rotation3D.z;
			}
			set
			{
				rotation3D = new Vector3(rotation3D.x, rotation3D.y, value);
			}
		}

		public float startSize
		{
			get
			{
				return startSize3D.x;
			}
			set
			{
				startSize3D = new Vector3(value, value, value);
			}
		}
	}

	public struct Burst
	{
		public float time { get; set; }

		public MinMaxCurve count { get; set; }

		public int cycleCount { get; set; }

		public float repeatInterval { get; set; }

		public float probability { get; set; }

		public short minCount
		{
			get
			{
				return (short)count.constantMin;
			}
			set
			{
				count = new MinMaxCurve(value, count.constantMax);
			}
		}

		public short maxCount
		{
			get
			{
				return (short)count.constantMax;
			}
			set
			{
				count = new MinMaxCurve(count.constantMin, value);
			}
		}

		public Burst(float time, short count)
			: this(time, new MinMaxCurve(count), 1, 0.01f)
		{
		}

		public Burst(float time, short minCount, short maxCount)
			: this(time, new MinMaxCurve(minCount, maxCount), 1, 0.01f)
		{
		}

		public Burst(float time, short minCount, short maxCount, int cycleCount, float repeatInterval)
			: this(time, new MinMaxCurve(minCount, maxCount), cycleCount, repeatInterval)
		{
		}

		public Burst(float time, MinMaxCurve count)
			: this(time, count, 1, 0.01f)
		{
		}

		public Burst(float time, MinMaxCurve count, int cycleCount, float repeatInterval)
		{
			this.time = time;
			this.count = count;
			this.cycleCount = cycleCount;
			this.repeatInterval = repeatInterval;
			probability = 1f;
		}
	}

	public struct MinMaxCurve
	{
		public ParticleSystemCurveMode mode { get; set; }

		public float curveMultiplier { get; set; }

		public AnimationCurve curveMin { get; set; }

		public AnimationCurve curveMax { get; set; }

		public float constantMin { get; set; }

		public float constantMax { get; set; }

		public float constant
		{
			get
			{
				return constantMax;
			}
			set
			{
				constantMax = value;
			}
		}

		public AnimationCurve curve
		{
			get
			{
				return curveMax;
			}
			set
			{
				curveMax = value;
			}
		}

		public MinMaxCurve(float constant)
		{
			mode = ParticleSystemCurveMode.Constant;
			curveMultiplier = 0f;
			curveMin = null;
			curveMax = null;
			constantMin = 0f;
			constantMax = constant;
		}

		public MinMaxCurve(float multiplier, AnimationCurve curve)
		{
			mode = ParticleSystemCurveMode.Curve;
			curveMultiplier = multiplier;
			curveMin = null;
			curveMax = curve;
			constantMin = 0f;
			constantMax = 0f;
		}

		public MinMaxCurve(float multiplier, AnimationCurve min, AnimationCurve max)
		{
			mode = ParticleSystemCurveMode.TwoCurves;
			curveMultiplier = multiplier;
			curveMin = min;
			curveMax = max;
			constantMin = 0f;
			constantMax = 0f;
		}

		public MinMaxCurve(float min, float max)
		{
			mode = ParticleSystemCurveMode.TwoConstants;
			curveMultiplier = 0f;
			curveMin = null;
			curveMax = null;
			constantMin = min;
			constantMax = max;
		}

		public float Evaluate(float time)
		{
			return Evaluate(time, 1f);
		}

		public float Evaluate(float time, float lerpFactor)
		{
			time = Mathf.Clamp01(time);
			lerpFactor = Mathf.Clamp01(lerpFactor);
			return mode switch
			{
				ParticleSystemCurveMode.Constant => constantMax, 
				ParticleSystemCurveMode.TwoConstants => Mathf.Lerp(constantMin, constantMax, lerpFactor), 
				ParticleSystemCurveMode.Curve => (curveMax?.Evaluate(time) ?? 0f) * curveMultiplier, 
				_ => Mathf.Lerp(curveMin?.Evaluate(time) ?? 0f, curveMax?.Evaluate(time) ?? 0f, lerpFactor) * curveMultiplier, 
			};
		}

		public static implicit operator MinMaxCurve(float constant)
		{
			return new MinMaxCurve(constant);
		}
	}

	public struct MinMaxGradient
	{
		public ParticleSystemGradientMode mode { get; set; }

		public Gradient gradientMin { get; set; }

		public Gradient gradientMax { get; set; }

		public Color colorMin { get; set; }

		public Color colorMax { get; set; }

		public Color color
		{
			get
			{
				return colorMax;
			}
			set
			{
				colorMax = value;
			}
		}

		public Gradient gradient
		{
			get
			{
				return gradientMax;
			}
			set
			{
				gradientMax = value;
			}
		}

		public MinMaxGradient(Color color)
		{
			mode = ParticleSystemGradientMode.Color;
			gradientMin = null;
			gradientMax = null;
			colorMin = Color.black;
			colorMax = color;
		}

		public MinMaxGradient(Gradient gradient)
		{
			mode = ParticleSystemGradientMode.Gradient;
			gradientMin = null;
			gradientMax = gradient;
			colorMin = Color.black;
			colorMax = Color.black;
		}

		public MinMaxGradient(Color min, Color max)
		{
			mode = ParticleSystemGradientMode.TwoColors;
			gradientMin = null;
			gradientMax = null;
			colorMin = min;
			colorMax = max;
		}

		public MinMaxGradient(Gradient min, Gradient max)
		{
			mode = ParticleSystemGradientMode.TwoGradients;
			gradientMin = min;
			gradientMax = max;
			colorMin = Color.black;
			colorMax = Color.black;
		}

		public Color Evaluate(float time)
		{
			return Evaluate(time, 1f);
		}

		public Color Evaluate(float time, float lerpFactor)
		{
			time = Mathf.Clamp01(time);
			lerpFactor = Mathf.Clamp01(lerpFactor);
			return mode switch
			{
				ParticleSystemGradientMode.Color => colorMax, 
				ParticleSystemGradientMode.TwoColors => Color.Lerp(colorMin, colorMax, lerpFactor), 
				ParticleSystemGradientMode.Gradient => gradientMax?.Evaluate(time) ?? Color.white, 
				ParticleSystemGradientMode.RandomColor => gradientMax?.Evaluate(lerpFactor) ?? Color.white, 
				_ => Color.Lerp(gradientMin?.Evaluate(time) ?? Color.white, gradientMax?.Evaluate(time) ?? Color.white, lerpFactor), 
			};
		}

		public static implicit operator MinMaxGradient(Color color)
		{
			return new MinMaxGradient(color);
		}

		public static implicit operator MinMaxGradient(Gradient gradient)
		{
			return new MinMaxGradient(gradient);
		}
	}

	internal sealed class MainState
	{
		public float duration = 5f;

		public float startDelayMultiplier;

		public float startLifetimeMultiplier = 5f;

		public float startSpeedMultiplier = 5f;

		public float startSizeMultiplier = 1f;

		public float startRotationMultiplier;

		public float gravityModifierMultiplier;

		public float simulationSpeed = 1f;

		public float flipRotation;

		public bool loop = true;

		public bool prewarm;

		public bool startSize3D;

		public bool startRotation3D;

		public bool playOnAwake = true;

		public bool useUnscaledTime;

		public int maxParticles = 1000;

		public MinMaxCurve startDelay;

		public MinMaxCurve startLifetime = 5f;

		public MinMaxCurve startSpeed = 5f;

		public MinMaxCurve startSize = 1f;

		public MinMaxCurve startSizeX = 1f;

		public MinMaxCurve startSizeY = 1f;

		public MinMaxCurve startSizeZ = 1f;

		public MinMaxCurve startRotation;

		public MinMaxCurve startRotationX;

		public MinMaxCurve startRotationY;

		public MinMaxCurve startRotationZ;

		public MinMaxCurve gravityModifier;

		public MinMaxGradient startColor = Color.white;

		public ParticleSystemSimulationSpace simulationSpace;

		public Transform customSimulationSpace;

		public ParticleSystemScalingMode scalingMode;

		public ParticleSystemEmitterVelocityMode emitterVelocityMode = ParticleSystemEmitterVelocityMode.Rigidbody;

		public ParticleSystemStopAction stopAction;

		public ParticleSystemCullingMode cullingMode;

		public ParticleSystemRingBufferMode ringBufferMode;
	}

	public struct MainModule
	{
		private readonly ParticleSystem _ps;

		private MainState S => _ps.mainState;

		public float duration
		{
			get
			{
				return S.duration;
			}
			set
			{
				S.duration = value;
			}
		}

		public bool loop
		{
			get
			{
				return S.loop;
			}
			set
			{
				S.loop = value;
			}
		}

		public bool prewarm
		{
			get
			{
				return S.prewarm;
			}
			set
			{
				S.prewarm = value;
			}
		}

		public MinMaxCurve startDelay
		{
			get
			{
				return S.startDelay;
			}
			set
			{
				S.startDelay = value;
			}
		}

		public float startDelayMultiplier
		{
			get
			{
				return S.startDelayMultiplier;
			}
			set
			{
				S.startDelayMultiplier = value;
			}
		}

		public MinMaxCurve startLifetime
		{
			get
			{
				return S.startLifetime;
			}
			set
			{
				S.startLifetime = value;
			}
		}

		public float startLifetimeMultiplier
		{
			get
			{
				return S.startLifetimeMultiplier;
			}
			set
			{
				S.startLifetimeMultiplier = value;
			}
		}

		public MinMaxCurve startSpeed
		{
			get
			{
				return S.startSpeed;
			}
			set
			{
				S.startSpeed = value;
			}
		}

		public float startSpeedMultiplier
		{
			get
			{
				return S.startSpeedMultiplier;
			}
			set
			{
				S.startSpeedMultiplier = value;
			}
		}

		public bool startSize3D
		{
			get
			{
				return S.startSize3D;
			}
			set
			{
				S.startSize3D = value;
			}
		}

		public MinMaxCurve startSize
		{
			get
			{
				return S.startSize;
			}
			set
			{
				S.startSize = value;
			}
		}

		public float startSizeMultiplier
		{
			get
			{
				return S.startSizeMultiplier;
			}
			set
			{
				S.startSizeMultiplier = value;
			}
		}

		public MinMaxCurve startSizeX
		{
			get
			{
				return S.startSizeX;
			}
			set
			{
				S.startSizeX = value;
			}
		}

		public MinMaxCurve startSizeY
		{
			get
			{
				return S.startSizeY;
			}
			set
			{
				S.startSizeY = value;
			}
		}

		public MinMaxCurve startSizeZ
		{
			get
			{
				return S.startSizeZ;
			}
			set
			{
				S.startSizeZ = value;
			}
		}

		public bool startRotation3D
		{
			get
			{
				return S.startRotation3D;
			}
			set
			{
				S.startRotation3D = value;
			}
		}

		public MinMaxCurve startRotation
		{
			get
			{
				return S.startRotation;
			}
			set
			{
				S.startRotation = value;
			}
		}

		public float startRotationMultiplier
		{
			get
			{
				return S.startRotationMultiplier;
			}
			set
			{
				S.startRotationMultiplier = value;
			}
		}

		public MinMaxCurve startRotationX
		{
			get
			{
				return S.startRotationX;
			}
			set
			{
				S.startRotationX = value;
			}
		}

		public MinMaxCurve startRotationY
		{
			get
			{
				return S.startRotationY;
			}
			set
			{
				S.startRotationY = value;
			}
		}

		public MinMaxCurve startRotationZ
		{
			get
			{
				return S.startRotationZ;
			}
			set
			{
				S.startRotationZ = value;
			}
		}

		public float flipRotation
		{
			get
			{
				return S.flipRotation;
			}
			set
			{
				S.flipRotation = value;
			}
		}

		public MinMaxGradient startColor
		{
			get
			{
				return S.startColor;
			}
			set
			{
				S.startColor = value;
			}
		}

		public MinMaxCurve gravityModifier
		{
			get
			{
				return S.gravityModifier;
			}
			set
			{
				S.gravityModifier = value;
			}
		}

		public float gravityModifierMultiplier
		{
			get
			{
				return S.gravityModifierMultiplier;
			}
			set
			{
				S.gravityModifierMultiplier = value;
			}
		}

		public ParticleSystemSimulationSpace simulationSpace
		{
			get
			{
				return S.simulationSpace;
			}
			set
			{
				S.simulationSpace = value;
			}
		}

		public Transform customSimulationSpace
		{
			get
			{
				return S.customSimulationSpace;
			}
			set
			{
				S.customSimulationSpace = value;
			}
		}

		public float simulationSpeed
		{
			get
			{
				return S.simulationSpeed;
			}
			set
			{
				S.simulationSpeed = value;
			}
		}

		public bool useUnscaledTime
		{
			get
			{
				return S.useUnscaledTime;
			}
			set
			{
				S.useUnscaledTime = value;
			}
		}

		public ParticleSystemScalingMode scalingMode
		{
			get
			{
				return S.scalingMode;
			}
			set
			{
				S.scalingMode = value;
			}
		}

		public bool playOnAwake
		{
			get
			{
				return S.playOnAwake;
			}
			set
			{
				S.playOnAwake = value;
			}
		}

		public int maxParticles
		{
			get
			{
				return S.maxParticles;
			}
			set
			{
				S.maxParticles = value;
			}
		}

		public ParticleSystemEmitterVelocityMode emitterVelocityMode
		{
			get
			{
				return S.emitterVelocityMode;
			}
			set
			{
				S.emitterVelocityMode = value;
			}
		}

		public ParticleSystemStopAction stopAction
		{
			get
			{
				return S.stopAction;
			}
			set
			{
				S.stopAction = value;
			}
		}

		public ParticleSystemCullingMode cullingMode
		{
			get
			{
				return S.cullingMode;
			}
			set
			{
				S.cullingMode = value;
			}
		}

		public ParticleSystemRingBufferMode ringBufferMode
		{
			get
			{
				return S.ringBufferMode;
			}
			set
			{
				S.ringBufferMode = value;
			}
		}

		internal MainModule(ParticleSystem ps)
		{
			_ps = ps;
		}
	}

	internal sealed class EmissionState
	{
		public bool enabled = true;

		public MinMaxCurve rateOverTime = 10f;

		public MinMaxCurve rateOverDistance;

		public float rateOverTimeMultiplier = 10f;

		public float rateOverDistanceMultiplier;

		public Burst[] bursts = Array.Empty<Burst>();
	}

	public struct EmissionModule
	{
		private readonly ParticleSystem _ps;

		private EmissionState S => _ps.emissionState;

		public bool enabled
		{
			get
			{
				return S.enabled;
			}
			set
			{
				S.enabled = value;
			}
		}

		public MinMaxCurve rateOverTime
		{
			get
			{
				return S.rateOverTime;
			}
			set
			{
				S.rateOverTime = value;
			}
		}

		public float rateOverTimeMultiplier
		{
			get
			{
				return S.rateOverTimeMultiplier;
			}
			set
			{
				S.rateOverTimeMultiplier = value;
			}
		}

		public MinMaxCurve rateOverDistance
		{
			get
			{
				return S.rateOverDistance;
			}
			set
			{
				S.rateOverDistance = value;
			}
		}

		public float rateOverDistanceMultiplier
		{
			get
			{
				return S.rateOverDistanceMultiplier;
			}
			set
			{
				S.rateOverDistanceMultiplier = value;
			}
		}

		public int burstCount
		{
			get
			{
				return S.bursts.Length;
			}
			set
			{
				Array.Resize(ref S.bursts, Math.Max(0, value));
			}
		}

		internal EmissionModule(ParticleSystem ps)
		{
			_ps = ps;
		}

		public void SetBursts(Burst[] bursts)
		{
			S.bursts = (Burst[])(bursts ?? Array.Empty<Burst>()).Clone();
		}

		public void SetBursts(Burst[] bursts, int size)
		{
			S.bursts = new Burst[Math.Max(0, size)];
			Array.Copy(bursts, S.bursts, S.bursts.Length);
		}

		public int GetBursts(Burst[] bursts)
		{
			int num = Math.Min(bursts.Length, S.bursts.Length);
			Array.Copy(S.bursts, bursts, num);
			return num;
		}

		public void SetBurst(int index, Burst burst)
		{
			S.bursts[index] = burst;
		}

		public Burst GetBurst(int index)
		{
			return S.bursts[index];
		}
	}

	internal sealed class ShapeState
	{
		public bool enabled = true;

		public bool alignToDirection;

		public bool useMeshMaterialIndex;

		public bool useMeshColors = true;

		public ParticleSystemShapeType shapeType = ParticleSystemShapeType.Cone;

		public ParticleSystemMeshShapeType meshShapeType;

		public float randomDirectionAmount;

		public float sphericalDirectionAmount;

		public float randomPositionAmount;

		public float radius = 1f;

		public float radiusThickness = 1f;

		public float angle = 25f;

		public float length = 5f;

		public float arc = 360f;

		public float normalOffset;

		public float meshSpawnSpread;

		public float donutRadius = 0.2f;

		public Vector3 boxThickness;

		public Vector3 position;

		public Vector3 rotation;

		public Vector3 scale = Vector3.one;

		public Mesh mesh;

		public MeshRenderer meshRenderer;

		public Texture2D texture;

		public Sprite sprite;
	}

	public struct ShapeModule
	{
		private readonly ParticleSystem _ps;

		private ShapeState S => _ps.shapeState;

		public bool enabled
		{
			get
			{
				return S.enabled;
			}
			set
			{
				S.enabled = value;
			}
		}

		public ParticleSystemShapeType shapeType
		{
			get
			{
				return S.shapeType;
			}
			set
			{
				S.shapeType = value;
			}
		}

		public ParticleSystemMeshShapeType meshShapeType
		{
			get
			{
				return S.meshShapeType;
			}
			set
			{
				S.meshShapeType = value;
			}
		}

		public float randomDirectionAmount
		{
			get
			{
				return S.randomDirectionAmount;
			}
			set
			{
				S.randomDirectionAmount = value;
			}
		}

		public float sphericalDirectionAmount
		{
			get
			{
				return S.sphericalDirectionAmount;
			}
			set
			{
				S.sphericalDirectionAmount = value;
			}
		}

		public float randomPositionAmount
		{
			get
			{
				return S.randomPositionAmount;
			}
			set
			{
				S.randomPositionAmount = value;
			}
		}

		public bool alignToDirection
		{
			get
			{
				return S.alignToDirection;
			}
			set
			{
				S.alignToDirection = value;
			}
		}

		public float radius
		{
			get
			{
				return S.radius;
			}
			set
			{
				S.radius = value;
			}
		}

		public float radiusThickness
		{
			get
			{
				return S.radiusThickness;
			}
			set
			{
				S.radiusThickness = value;
			}
		}

		public float angle
		{
			get
			{
				return S.angle;
			}
			set
			{
				S.angle = value;
			}
		}

		public float length
		{
			get
			{
				return S.length;
			}
			set
			{
				S.length = value;
			}
		}

		public Vector3 boxThickness
		{
			get
			{
				return S.boxThickness;
			}
			set
			{
				S.boxThickness = value;
			}
		}

		public float arc
		{
			get
			{
				return S.arc;
			}
			set
			{
				S.arc = value;
			}
		}

		public float normalOffset
		{
			get
			{
				return S.normalOffset;
			}
			set
			{
				S.normalOffset = value;
			}
		}

		public float donutRadius
		{
			get
			{
				return S.donutRadius;
			}
			set
			{
				S.donutRadius = value;
			}
		}

		public Vector3 position
		{
			get
			{
				return S.position;
			}
			set
			{
				S.position = value;
			}
		}

		public Vector3 rotation
		{
			get
			{
				return S.rotation;
			}
			set
			{
				S.rotation = value;
			}
		}

		public Vector3 scale
		{
			get
			{
				return S.scale;
			}
			set
			{
				S.scale = value;
			}
		}

		public Mesh mesh
		{
			get
			{
				return S.mesh;
			}
			set
			{
				S.mesh = value;
			}
		}

		public MeshRenderer meshRenderer
		{
			get
			{
				return S.meshRenderer;
			}
			set
			{
				S.meshRenderer = value;
			}
		}

		public bool useMeshMaterialIndex
		{
			get
			{
				return S.useMeshMaterialIndex;
			}
			set
			{
				S.useMeshMaterialIndex = value;
			}
		}

		public bool useMeshColors
		{
			get
			{
				return S.useMeshColors;
			}
			set
			{
				S.useMeshColors = value;
			}
		}

		public float meshSpawnSpread
		{
			get
			{
				return S.meshSpawnSpread;
			}
			set
			{
				S.meshSpawnSpread = value;
			}
		}

		public Texture2D texture
		{
			get
			{
				return S.texture;
			}
			set
			{
				S.texture = value;
			}
		}

		public Sprite sprite
		{
			get
			{
				return S.sprite;
			}
			set
			{
				S.sprite = value;
			}
		}

		internal ShapeModule(ParticleSystem ps)
		{
			_ps = ps;
		}
	}

	internal sealed class NoiseState
	{
		public bool enabled;

		public bool separateAxes;

		public bool damping = true;

		public bool remapEnabled;

		public MinMaxCurve strength = 1f;

		public MinMaxCurve strengthX = 1f;

		public MinMaxCurve strengthY = 1f;

		public MinMaxCurve strengthZ = 1f;

		public MinMaxCurve scrollSpeed;

		public MinMaxCurve remap;

		public MinMaxCurve positionAmount = 1f;

		public MinMaxCurve rotationAmount;

		public MinMaxCurve sizeAmount;

		public float strengthMultiplier = 1f;

		public float frequency = 0.5f;

		public float octaveMultiplier = 0.5f;

		public float octaveScale = 2f;

		public float scrollSpeedMultiplier;

		public int octaveCount = 1;

		public ParticleSystemNoiseQuality quality = ParticleSystemNoiseQuality.High;
	}

	public struct NoiseModule
	{
		private readonly ParticleSystem _ps;

		private NoiseState S => _ps.noiseState;

		public bool enabled
		{
			get
			{
				return S.enabled;
			}
			set
			{
				S.enabled = value;
			}
		}

		public bool separateAxes
		{
			get
			{
				return S.separateAxes;
			}
			set
			{
				S.separateAxes = value;
			}
		}

		public MinMaxCurve strength
		{
			get
			{
				return S.strength;
			}
			set
			{
				S.strength = value;
			}
		}

		public float strengthMultiplier
		{
			get
			{
				return S.strengthMultiplier;
			}
			set
			{
				S.strengthMultiplier = value;
			}
		}

		public MinMaxCurve strengthX
		{
			get
			{
				return S.strengthX;
			}
			set
			{
				S.strengthX = value;
			}
		}

		public MinMaxCurve strengthY
		{
			get
			{
				return S.strengthY;
			}
			set
			{
				S.strengthY = value;
			}
		}

		public MinMaxCurve strengthZ
		{
			get
			{
				return S.strengthZ;
			}
			set
			{
				S.strengthZ = value;
			}
		}

		public float frequency
		{
			get
			{
				return S.frequency;
			}
			set
			{
				S.frequency = value;
			}
		}

		public bool damping
		{
			get
			{
				return S.damping;
			}
			set
			{
				S.damping = value;
			}
		}

		public int octaveCount
		{
			get
			{
				return S.octaveCount;
			}
			set
			{
				S.octaveCount = value;
			}
		}

		public float octaveMultiplier
		{
			get
			{
				return S.octaveMultiplier;
			}
			set
			{
				S.octaveMultiplier = value;
			}
		}

		public float octaveScale
		{
			get
			{
				return S.octaveScale;
			}
			set
			{
				S.octaveScale = value;
			}
		}

		public ParticleSystemNoiseQuality quality
		{
			get
			{
				return S.quality;
			}
			set
			{
				S.quality = value;
			}
		}

		public MinMaxCurve scrollSpeed
		{
			get
			{
				return S.scrollSpeed;
			}
			set
			{
				S.scrollSpeed = value;
			}
		}

		public float scrollSpeedMultiplier
		{
			get
			{
				return S.scrollSpeedMultiplier;
			}
			set
			{
				S.scrollSpeedMultiplier = value;
			}
		}

		public bool remapEnabled
		{
			get
			{
				return S.remapEnabled;
			}
			set
			{
				S.remapEnabled = value;
			}
		}

		public MinMaxCurve remap
		{
			get
			{
				return S.remap;
			}
			set
			{
				S.remap = value;
			}
		}

		public MinMaxCurve positionAmount
		{
			get
			{
				return S.positionAmount;
			}
			set
			{
				S.positionAmount = value;
			}
		}

		public MinMaxCurve rotationAmount
		{
			get
			{
				return S.rotationAmount;
			}
			set
			{
				S.rotationAmount = value;
			}
		}

		public MinMaxCurve sizeAmount
		{
			get
			{
				return S.sizeAmount;
			}
			set
			{
				S.sizeAmount = value;
			}
		}

		internal NoiseModule(ParticleSystem ps)
		{
			_ps = ps;
		}
	}

	internal sealed class VelocityOverLifetimeState
	{
		public bool enabled;

		public MinMaxCurve x;

		public MinMaxCurve y;

		public MinMaxCurve z;

		public MinMaxCurve orbitalX;

		public MinMaxCurve orbitalY;

		public MinMaxCurve orbitalZ;

		public MinMaxCurve orbitalOffsetX;

		public MinMaxCurve orbitalOffsetY;

		public MinMaxCurve orbitalOffsetZ;

		public MinMaxCurve radial;

		public MinMaxCurve speedModifier = 1f;

		public float xMultiplier;

		public float yMultiplier;

		public float zMultiplier;

		public float speedModifierMultiplier = 1f;

		public ParticleSystemSimulationSpace space;
	}

	public struct VelocityOverLifetimeModule
	{
		private readonly ParticleSystem _ps;

		private VelocityOverLifetimeState S => _ps.velocityState;

		public bool enabled
		{
			get
			{
				return S.enabled;
			}
			set
			{
				S.enabled = value;
			}
		}

		public MinMaxCurve x
		{
			get
			{
				return S.x;
			}
			set
			{
				S.x = value;
			}
		}

		public MinMaxCurve y
		{
			get
			{
				return S.y;
			}
			set
			{
				S.y = value;
			}
		}

		public MinMaxCurve z
		{
			get
			{
				return S.z;
			}
			set
			{
				S.z = value;
			}
		}

		public float xMultiplier
		{
			get
			{
				return S.xMultiplier;
			}
			set
			{
				S.xMultiplier = value;
			}
		}

		public float yMultiplier
		{
			get
			{
				return S.yMultiplier;
			}
			set
			{
				S.yMultiplier = value;
			}
		}

		public float zMultiplier
		{
			get
			{
				return S.zMultiplier;
			}
			set
			{
				S.zMultiplier = value;
			}
		}

		public MinMaxCurve orbitalX
		{
			get
			{
				return S.orbitalX;
			}
			set
			{
				S.orbitalX = value;
			}
		}

		public MinMaxCurve orbitalY
		{
			get
			{
				return S.orbitalY;
			}
			set
			{
				S.orbitalY = value;
			}
		}

		public MinMaxCurve orbitalZ
		{
			get
			{
				return S.orbitalZ;
			}
			set
			{
				S.orbitalZ = value;
			}
		}

		public MinMaxCurve orbitalOffsetX
		{
			get
			{
				return S.orbitalOffsetX;
			}
			set
			{
				S.orbitalOffsetX = value;
			}
		}

		public MinMaxCurve orbitalOffsetY
		{
			get
			{
				return S.orbitalOffsetY;
			}
			set
			{
				S.orbitalOffsetY = value;
			}
		}

		public MinMaxCurve orbitalOffsetZ
		{
			get
			{
				return S.orbitalOffsetZ;
			}
			set
			{
				S.orbitalOffsetZ = value;
			}
		}

		public MinMaxCurve radial
		{
			get
			{
				return S.radial;
			}
			set
			{
				S.radial = value;
			}
		}

		public MinMaxCurve speedModifier
		{
			get
			{
				return S.speedModifier;
			}
			set
			{
				S.speedModifier = value;
			}
		}

		public float speedModifierMultiplier
		{
			get
			{
				return S.speedModifierMultiplier;
			}
			set
			{
				S.speedModifierMultiplier = value;
			}
		}

		public ParticleSystemSimulationSpace space
		{
			get
			{
				return S.space;
			}
			set
			{
				S.space = value;
			}
		}

		internal VelocityOverLifetimeModule(ParticleSystem ps)
		{
			_ps = ps;
		}
	}

	internal sealed class SizeOverLifetimeState
	{
		public bool enabled;

		public bool separateAxes;

		public MinMaxCurve size = 1f;

		public MinMaxCurve x = 1f;

		public MinMaxCurve y = 1f;

		public MinMaxCurve z = 1f;

		public float sizeMultiplier = 1f;

		public float xMultiplier = 1f;

		public float yMultiplier = 1f;

		public float zMultiplier = 1f;
	}

	public struct SizeOverLifetimeModule
	{
		private readonly ParticleSystem _ps;

		private SizeOverLifetimeState S => _ps.sizeState;

		public bool enabled
		{
			get
			{
				return S.enabled;
			}
			set
			{
				S.enabled = value;
			}
		}

		public MinMaxCurve size
		{
			get
			{
				return S.size;
			}
			set
			{
				S.size = value;
			}
		}

		public float sizeMultiplier
		{
			get
			{
				return S.sizeMultiplier;
			}
			set
			{
				S.sizeMultiplier = value;
			}
		}

		public MinMaxCurve x
		{
			get
			{
				return S.x;
			}
			set
			{
				S.x = value;
			}
		}

		public float xMultiplier
		{
			get
			{
				return S.xMultiplier;
			}
			set
			{
				S.xMultiplier = value;
			}
		}

		public MinMaxCurve y
		{
			get
			{
				return S.y;
			}
			set
			{
				S.y = value;
			}
		}

		public float yMultiplier
		{
			get
			{
				return S.yMultiplier;
			}
			set
			{
				S.yMultiplier = value;
			}
		}

		public MinMaxCurve z
		{
			get
			{
				return S.z;
			}
			set
			{
				S.z = value;
			}
		}

		public float zMultiplier
		{
			get
			{
				return S.zMultiplier;
			}
			set
			{
				S.zMultiplier = value;
			}
		}

		public bool separateAxes
		{
			get
			{
				return S.separateAxes;
			}
			set
			{
				S.separateAxes = value;
			}
		}

		internal SizeOverLifetimeModule(ParticleSystem ps)
		{
			_ps = ps;
		}
	}

	internal sealed class ColorOverLifetimeState
	{
		public bool enabled;

		public MinMaxGradient color = Color.white;
	}

	public struct ColorOverLifetimeModule
	{
		private readonly ParticleSystem _ps;

		private ColorOverLifetimeState S => _ps.colorState;

		public bool enabled
		{
			get
			{
				return S.enabled;
			}
			set
			{
				S.enabled = value;
			}
		}

		public MinMaxGradient color
		{
			get
			{
				return S.color;
			}
			set
			{
				S.color = value;
			}
		}

		internal ColorOverLifetimeModule(ParticleSystem ps)
		{
			_ps = ps;
		}
	}

	internal sealed class RotationOverLifetimeState
	{
		public bool enabled;

		public bool separateAxes;

		public MinMaxCurve x;

		public MinMaxCurve y;

		public MinMaxCurve z = (float)Math.PI / 4f;

		public float xMultiplier;

		public float yMultiplier;

		public float zMultiplier = (float)Math.PI / 4f;
	}

	public struct RotationOverLifetimeModule
	{
		private readonly ParticleSystem _ps;

		private RotationOverLifetimeState S => _ps.rotationState;

		public bool enabled
		{
			get
			{
				return S.enabled;
			}
			set
			{
				S.enabled = value;
			}
		}

		public MinMaxCurve x
		{
			get
			{
				return S.x;
			}
			set
			{
				S.x = value;
			}
		}

		public float xMultiplier
		{
			get
			{
				return S.xMultiplier;
			}
			set
			{
				S.xMultiplier = value;
			}
		}

		public MinMaxCurve y
		{
			get
			{
				return S.y;
			}
			set
			{
				S.y = value;
			}
		}

		public float yMultiplier
		{
			get
			{
				return S.yMultiplier;
			}
			set
			{
				S.yMultiplier = value;
			}
		}

		public MinMaxCurve z
		{
			get
			{
				return S.z;
			}
			set
			{
				S.z = value;
			}
		}

		public float zMultiplier
		{
			get
			{
				return S.zMultiplier;
			}
			set
			{
				S.zMultiplier = value;
			}
		}

		public bool separateAxes
		{
			get
			{
				return S.separateAxes;
			}
			set
			{
				S.separateAxes = value;
			}
		}

		internal RotationOverLifetimeModule(ParticleSystem ps)
		{
			_ps = ps;
		}
	}

	internal sealed class LimitVelocityOverLifetimeState
	{
		public bool enabled;

		public bool separateAxes;

		public bool multiplyDragByParticleSize = true;

		public bool multiplyDragByParticleVelocity = true;

		public MinMaxCurve limit = 1f;

		public MinMaxCurve limitX = 1f;

		public MinMaxCurve limitY = 1f;

		public MinMaxCurve limitZ = 1f;

		public MinMaxCurve drag;

		public float limitMultiplier = 1f;

		public float limitXMultiplier = 1f;

		public float limitYMultiplier = 1f;

		public float limitZMultiplier = 1f;

		public float dragMultiplier;

		public float dampen = 1f;

		public ParticleSystemSimulationSpace space;
	}

	public struct LimitVelocityOverLifetimeModule
	{
		private readonly ParticleSystem _ps;

		private LimitVelocityOverLifetimeState S => _ps.limitVelocityState;

		public bool enabled
		{
			get
			{
				return S.enabled;
			}
			set
			{
				S.enabled = value;
			}
		}

		public MinMaxCurve limitX
		{
			get
			{
				return S.limitX;
			}
			set
			{
				S.limitX = value;
			}
		}

		public float limitXMultiplier
		{
			get
			{
				return S.limitXMultiplier;
			}
			set
			{
				S.limitXMultiplier = value;
			}
		}

		public MinMaxCurve limitY
		{
			get
			{
				return S.limitY;
			}
			set
			{
				S.limitY = value;
			}
		}

		public float limitYMultiplier
		{
			get
			{
				return S.limitYMultiplier;
			}
			set
			{
				S.limitYMultiplier = value;
			}
		}

		public MinMaxCurve limitZ
		{
			get
			{
				return S.limitZ;
			}
			set
			{
				S.limitZ = value;
			}
		}

		public float limitZMultiplier
		{
			get
			{
				return S.limitZMultiplier;
			}
			set
			{
				S.limitZMultiplier = value;
			}
		}

		public MinMaxCurve limit
		{
			get
			{
				return S.limit;
			}
			set
			{
				S.limit = value;
			}
		}

		public float limitMultiplier
		{
			get
			{
				return S.limitMultiplier;
			}
			set
			{
				S.limitMultiplier = value;
			}
		}

		public float dampen
		{
			get
			{
				return S.dampen;
			}
			set
			{
				S.dampen = value;
			}
		}

		public bool separateAxes
		{
			get
			{
				return S.separateAxes;
			}
			set
			{
				S.separateAxes = value;
			}
		}

		public ParticleSystemSimulationSpace space
		{
			get
			{
				return S.space;
			}
			set
			{
				S.space = value;
			}
		}

		public MinMaxCurve drag
		{
			get
			{
				return S.drag;
			}
			set
			{
				S.drag = value;
			}
		}

		public float dragMultiplier
		{
			get
			{
				return S.dragMultiplier;
			}
			set
			{
				S.dragMultiplier = value;
			}
		}

		public bool multiplyDragByParticleSize
		{
			get
			{
				return S.multiplyDragByParticleSize;
			}
			set
			{
				S.multiplyDragByParticleSize = value;
			}
		}

		public bool multiplyDragByParticleVelocity
		{
			get
			{
				return S.multiplyDragByParticleVelocity;
			}
			set
			{
				S.multiplyDragByParticleVelocity = value;
			}
		}

		internal LimitVelocityOverLifetimeModule(ParticleSystem ps)
		{
			_ps = ps;
		}
	}

	internal sealed class TextureSheetAnimationState
	{
		public bool enabled;

		public ParticleSystemAnimationMode mode;

		public ParticleSystemAnimationTimeMode timeMode;

		public ParticleSystemAnimationType animation;

		public ParticleSystemAnimationRowMode rowMode = ParticleSystemAnimationRowMode.Random;

		public int numTilesX = 1;

		public int numTilesY = 1;

		public int rowIndex;

		public int cycleCount = 1;

		public int uvChannelMask = -1;

		public float fps = 30f;

		public float frameOverTimeMultiplier = 1f;

		public float startFrameMultiplier;

		public MinMaxCurve frameOverTime = new MinMaxCurve(1f, AnimationCurve.Linear(0f, 0f, 1f, 1f));

		public MinMaxCurve startFrame;

		public Vector2 speedRange = new Vector2(0f, 1f);
	}

	public struct TextureSheetAnimationModule
	{
		private readonly ParticleSystem _ps;

		private TextureSheetAnimationState S => _ps.textureSheetState;

		public bool enabled
		{
			get
			{
				return S.enabled;
			}
			set
			{
				S.enabled = value;
			}
		}

		public ParticleSystemAnimationMode mode
		{
			get
			{
				return S.mode;
			}
			set
			{
				S.mode = value;
			}
		}

		public ParticleSystemAnimationTimeMode timeMode
		{
			get
			{
				return S.timeMode;
			}
			set
			{
				S.timeMode = value;
			}
		}

		public float fps
		{
			get
			{
				return S.fps;
			}
			set
			{
				S.fps = value;
			}
		}

		public int numTilesX
		{
			get
			{
				return S.numTilesX;
			}
			set
			{
				S.numTilesX = value;
			}
		}

		public int numTilesY
		{
			get
			{
				return S.numTilesY;
			}
			set
			{
				S.numTilesY = value;
			}
		}

		public ParticleSystemAnimationType animation
		{
			get
			{
				return S.animation;
			}
			set
			{
				S.animation = value;
			}
		}

		public ParticleSystemAnimationRowMode rowMode
		{
			get
			{
				return S.rowMode;
			}
			set
			{
				S.rowMode = value;
			}
		}

		public MinMaxCurve frameOverTime
		{
			get
			{
				return S.frameOverTime;
			}
			set
			{
				S.frameOverTime = value;
			}
		}

		public float frameOverTimeMultiplier
		{
			get
			{
				return S.frameOverTimeMultiplier;
			}
			set
			{
				S.frameOverTimeMultiplier = value;
			}
		}

		public MinMaxCurve startFrame
		{
			get
			{
				return S.startFrame;
			}
			set
			{
				S.startFrame = value;
			}
		}

		public float startFrameMultiplier
		{
			get
			{
				return S.startFrameMultiplier;
			}
			set
			{
				S.startFrameMultiplier = value;
			}
		}

		public int cycleCount
		{
			get
			{
				return S.cycleCount;
			}
			set
			{
				S.cycleCount = value;
			}
		}

		public int rowIndex
		{
			get
			{
				return S.rowIndex;
			}
			set
			{
				S.rowIndex = value;
			}
		}

		public Vector2 speedRange
		{
			get
			{
				return S.speedRange;
			}
			set
			{
				S.speedRange = value;
			}
		}

		public int uvChannelMask
		{
			get
			{
				return S.uvChannelMask;
			}
			set
			{
				S.uvChannelMask = value;
			}
		}

		internal TextureSheetAnimationModule(ParticleSystem ps)
		{
			_ps = ps;
		}
	}

	internal sealed class CollisionState
	{
		public bool enabled;

		public bool sendCollisionMessages;

		public ParticleSystemCollisionType type;

		public ParticleSystemCollisionMode mode;

		public LayerMask collidesWith = -1;

		public MinMaxCurve dampen;

		public MinMaxCurve bounce = 1f;

		public MinMaxCurve lifetimeLoss;
	}

	public struct CollisionModule
	{
		private readonly ParticleSystem _ps;

		private CollisionState S => _ps.collisionState;

		public bool enabled
		{
			get
			{
				return S.enabled;
			}
			set
			{
				S.enabled = value;
			}
		}

		public ParticleSystemCollisionType type
		{
			get
			{
				return S.type;
			}
			set
			{
				S.type = value;
			}
		}

		public ParticleSystemCollisionMode mode
		{
			get
			{
				return S.mode;
			}
			set
			{
				S.mode = value;
			}
		}

		public bool sendCollisionMessages
		{
			get
			{
				return S.sendCollisionMessages;
			}
			set
			{
				S.sendCollisionMessages = value;
			}
		}

		public LayerMask collidesWith
		{
			get
			{
				return S.collidesWith;
			}
			set
			{
				S.collidesWith = value;
			}
		}

		public MinMaxCurve dampen
		{
			get
			{
				return S.dampen;
			}
			set
			{
				S.dampen = value;
			}
		}

		public MinMaxCurve bounce
		{
			get
			{
				return S.bounce;
			}
			set
			{
				S.bounce = value;
			}
		}

		public MinMaxCurve lifetimeLoss
		{
			get
			{
				return S.lifetimeLoss;
			}
			set
			{
				S.lifetimeLoss = value;
			}
		}

		internal CollisionModule(ParticleSystem ps)
		{
			_ps = ps;
		}
	}

	internal sealed class TrailState
	{
		public bool enabled;

		public bool worldSpace;

		public bool dieWithParticles = true;

		public bool sizeAffectsWidth = true;

		public bool sizeAffectsLifetime;

		public bool inheritParticleColor = true;

		public bool generateLightingData;

		public bool splitSubEmitterRibbons;

		public bool attachRibbonsToTransform;

		public ParticleSystemTrailMode mode;

		public ParticleSystemTrailTextureMode textureMode;

		public float ratio = 1f;

		public float minVertexDistance = 0.2f;

		public float lifetimeMultiplier = 1f;

		public float widthOverTrailMultiplier = 1f;

		public float shadowBias = 0.5f;

		public int ribbonCount = 1;

		public MinMaxCurve lifetime = 1f;

		public MinMaxCurve widthOverTrail = 1f;

		public MinMaxGradient colorOverLifetime = Color.white;

		public MinMaxGradient colorOverTrail = Color.white;
	}

	public struct TrailModule
	{
		private readonly ParticleSystem _ps;

		private TrailState S => _ps.trailState;

		public bool enabled
		{
			get
			{
				return S.enabled;
			}
			set
			{
				S.enabled = value;
			}
		}

		public ParticleSystemTrailMode mode
		{
			get
			{
				return S.mode;
			}
			set
			{
				S.mode = value;
			}
		}

		public float ratio
		{
			get
			{
				return S.ratio;
			}
			set
			{
				S.ratio = value;
			}
		}

		public MinMaxCurve lifetime
		{
			get
			{
				return S.lifetime;
			}
			set
			{
				S.lifetime = value;
			}
		}

		public float lifetimeMultiplier
		{
			get
			{
				return S.lifetimeMultiplier;
			}
			set
			{
				S.lifetimeMultiplier = value;
			}
		}

		public float minVertexDistance
		{
			get
			{
				return S.minVertexDistance;
			}
			set
			{
				S.minVertexDistance = value;
			}
		}

		public ParticleSystemTrailTextureMode textureMode
		{
			get
			{
				return S.textureMode;
			}
			set
			{
				S.textureMode = value;
			}
		}

		public bool worldSpace
		{
			get
			{
				return S.worldSpace;
			}
			set
			{
				S.worldSpace = value;
			}
		}

		public bool dieWithParticles
		{
			get
			{
				return S.dieWithParticles;
			}
			set
			{
				S.dieWithParticles = value;
			}
		}

		public bool sizeAffectsWidth
		{
			get
			{
				return S.sizeAffectsWidth;
			}
			set
			{
				S.sizeAffectsWidth = value;
			}
		}

		public bool sizeAffectsLifetime
		{
			get
			{
				return S.sizeAffectsLifetime;
			}
			set
			{
				S.sizeAffectsLifetime = value;
			}
		}

		public bool inheritParticleColor
		{
			get
			{
				return S.inheritParticleColor;
			}
			set
			{
				S.inheritParticleColor = value;
			}
		}

		public MinMaxGradient colorOverLifetime
		{
			get
			{
				return S.colorOverLifetime;
			}
			set
			{
				S.colorOverLifetime = value;
			}
		}

		public MinMaxCurve widthOverTrail
		{
			get
			{
				return S.widthOverTrail;
			}
			set
			{
				S.widthOverTrail = value;
			}
		}

		public float widthOverTrailMultiplier
		{
			get
			{
				return S.widthOverTrailMultiplier;
			}
			set
			{
				S.widthOverTrailMultiplier = value;
			}
		}

		public MinMaxGradient colorOverTrail
		{
			get
			{
				return S.colorOverTrail;
			}
			set
			{
				S.colorOverTrail = value;
			}
		}

		public bool generateLightingData
		{
			get
			{
				return S.generateLightingData;
			}
			set
			{
				S.generateLightingData = value;
			}
		}

		public int ribbonCount
		{
			get
			{
				return S.ribbonCount;
			}
			set
			{
				S.ribbonCount = value;
			}
		}

		public float shadowBias
		{
			get
			{
				return S.shadowBias;
			}
			set
			{
				S.shadowBias = value;
			}
		}

		public bool splitSubEmitterRibbons
		{
			get
			{
				return S.splitSubEmitterRibbons;
			}
			set
			{
				S.splitSubEmitterRibbons = value;
			}
		}

		public bool attachRibbonsToTransform
		{
			get
			{
				return S.attachRibbonsToTransform;
			}
			set
			{
				S.attachRibbonsToTransform = value;
			}
		}

		internal TrailModule(ParticleSystem ps)
		{
			_ps = ps;
		}
	}

	private readonly record struct MoveStep(float Dt, float SystemTime, bool HasGravity, Quaternion LinearSpace);

	internal readonly MainState mainState = new MainState();

	internal readonly EmissionState emissionState = new EmissionState();

	internal readonly ShapeState shapeState = new ShapeState();

	internal readonly NoiseState noiseState = new NoiseState();

	internal readonly VelocityOverLifetimeState velocityState = new VelocityOverLifetimeState();

	internal readonly SizeOverLifetimeState sizeState = new SizeOverLifetimeState();

	internal readonly ColorOverLifetimeState colorState = new ColorOverLifetimeState();

	internal readonly TrailState trailState = new TrailState();

	internal readonly RotationOverLifetimeState rotationState = new RotationOverLifetimeState();

	internal readonly LimitVelocityOverLifetimeState limitVelocityState = new LimitVelocityOverLifetimeState();

	internal readonly TextureSheetAnimationState textureSheetState = new TextureSheetAnimationState();

	internal readonly CollisionState collisionState = new CollisionState();

	private bool _playing;

	private bool _paused;

	private const float MaxFrameStep = 1f / 3f;

	private const float ChunkedStep = 1f / 30f;

	private const int ParallelMoveThreshold = 1024;

	private const int ParallelMoveBatch = 512;

	private static readonly Vector3 Gravity = new Vector3(0f, -9.81f, 0f);

	private static uint _nextSystemSeed = 2654435769u;

	private ParticleSystemRenderer _renderer;

	private uint _rng = NextSystemSeed();

	private float _emitAccumulator;

	private float _delayRemaining;

	private float _noiseScroll;

	private bool _stopActionArmed;

	private MoveStep _step;

	private Action<int> _moveBatch;

	internal Vector3[] _position = Array.Empty<Vector3>();

	private Vector3[] _velocity = Array.Empty<Vector3>();

	private float[] _age = Array.Empty<float>();

	private float[] _lifetime = Array.Empty<float>();

	internal float[] _rotation = Array.Empty<float>();

	private float[] _angularVelocity = Array.Empty<float>();

	private Vector3[] _startSize = Array.Empty<Vector3>();

	private Color32[] _startColor = Array.Empty<Color32>();

	internal uint[] _seed = Array.Empty<uint>();

	internal int[] _frame = Array.Empty<int>();

	internal Vector2[] _currentSize = Array.Empty<Vector2>();

	internal Color32[] _currentColor = Array.Empty<Color32>();

	internal int _count;

	public MainModule main => new MainModule(this);

	public EmissionModule emission => new EmissionModule(this);

	public ShapeModule shape => new ShapeModule(this);

	public NoiseModule noise => new NoiseModule(this);

	public VelocityOverLifetimeModule velocityOverLifetime => new VelocityOverLifetimeModule(this);

	public SizeOverLifetimeModule sizeOverLifetime => new SizeOverLifetimeModule(this);

	public ColorOverLifetimeModule colorOverLifetime => new ColorOverLifetimeModule(this);

	public TrailModule trails => new TrailModule(this);

	public RotationOverLifetimeModule rotationOverLifetime => new RotationOverLifetimeModule(this);

	public LimitVelocityOverLifetimeModule limitVelocityOverLifetime => new LimitVelocityOverLifetimeModule(this);

	public TextureSheetAnimationModule textureSheetAnimation => new TextureSheetAnimationModule(this);

	public CollisionModule collision => new CollisionModule(this);

	public bool isPlaying
	{
		get
		{
			if (_playing)
			{
				return !_paused;
			}
			return false;
		}
	}

	public bool isPaused => _paused;

	public bool isStopped
	{
		get
		{
			if (!_playing)
			{
				return !_paused;
			}
			return false;
		}
	}

	public bool isEmitting
	{
		get
		{
			if (_playing && !_paused && emissionState.enabled)
			{
				if (!mainState.loop)
				{
					return time < Duration;
				}
				return true;
			}
			return false;
		}
	}

	public int particleCount => _count;

	public float time { get; set; }

	public uint randomSeed { get; set; }

	public bool useAutoRandomSeed { get; set; } = true;

	public bool proceduralSimulationSupported => false;

	private float Duration => Mathf.Max(mainState.duration, 0.05f);

	private float NormalizedTime => Mathf.Clamp01(time / Duration);

	public void Play()
	{
		Play(withChildren: true);
	}

	public void Play(bool withChildren)
	{
		if (!_playing)
		{
			time = 0f;
			_emitAccumulator = 0f;
			if (!useAutoRandomSeed)
			{
				_rng = randomSeed | 1;
			}
			_delayRemaining = mainState.startDelay.Evaluate(0f, NextFloat());
			_playing = true;
			if (mainState.prewarm && mainState.loop)
			{
				AdvanceChunked(Duration);
			}
		}
		_paused = false;
		_stopActionArmed = true;
	}

	public void Pause()
	{
		Pause(withChildren: true);
	}

	public void Pause(bool withChildren)
	{
		_paused = true;
	}

	public void Stop()
	{
		Stop(withChildren: true, ParticleSystemStopBehavior.StopEmitting);
	}

	public void Stop(bool withChildren)
	{
		Stop(withChildren, ParticleSystemStopBehavior.StopEmitting);
	}

	public void Stop(bool withChildren, ParticleSystemStopBehavior stopBehavior)
	{
		_playing = false;
		_paused = false;
		if (stopBehavior == ParticleSystemStopBehavior.StopEmittingAndClear)
		{
			_count = 0;
		}
	}

	public void Clear()
	{
		Clear(withChildren: true);
	}

	public void Clear(bool withChildren)
	{
		_count = 0;
	}

	public void Emit(int count)
	{
		for (int i = 0; i < count; i++)
		{
			Spawn(default(EmitParams));
		}
	}

	public void Emit(EmitParams emitParams, int count)
	{
		for (int i = 0; i < count; i++)
		{
			Spawn(in emitParams);
		}
	}

	public void Simulate(float t)
	{
		Simulate(t, withChildren: true, restart: true, fixedTimeStep: true);
	}

	public void Simulate(float t, bool withChildren)
	{
		Simulate(t, withChildren, restart: true, fixedTimeStep: true);
	}

	public void Simulate(float t, bool withChildren, bool restart)
	{
		Simulate(t, withChildren, restart, fixedTimeStep: true);
	}

	public void Simulate(float t, bool withChildren, bool restart, bool fixedTimeStep)
	{
		if (restart)
		{
			_count = 0;
			_playing = false;
			Play(withChildren);
		}
		else
		{
			_playing = true;
		}
		AdvanceChunked(t);
		_paused = true;
	}

	public bool IsAlive()
	{
		return IsAlive(withChildren: true);
	}

	public bool IsAlive(bool withChildren)
	{
		if (_count <= 0)
		{
			if (_playing)
			{
				if (!mainState.loop)
				{
					return time < Duration;
				}
				return true;
			}
			return false;
		}
		return true;
	}

	public int GetParticles(Particle[] particles)
	{
		return GetParticles(particles, particles.Length);
	}

	public int GetParticles(Particle[] particles, int size)
	{
		int num = Math.Min(Math.Min(size, particles.Length), _count);
		for (int i = 0; i < num; i++)
		{
			particles[i] = new Particle
			{
				position = _position[i],
				velocity = _velocity[i],
				angularVelocity3D = new Vector3(0f, 0f, _angularVelocity[i] * 57.29578f),
				rotation3D = new Vector3(0f, 0f, _rotation[i] * 57.29578f),
				startSize3D = _startSize[i],
				startColor = _startColor[i],
				randomSeed = _seed[i],
				remainingLifetime = _lifetime[i] - _age[i],
				startLifetime = _lifetime[i]
			};
		}
		return num;
	}

	public void SetParticles(Particle[] particles)
	{
		SetParticles(particles, particles.Length, 0);
	}

	public void SetParticles(Particle[] particles, int size)
	{
		SetParticles(particles, size, 0);
	}

	public void SetParticles(Particle[] particles, int size, int offset)
	{
		size = Math.Min(size, particles.Length);
		EnsureCapacity(offset + size);
		for (int i = 0; i < size; i++)
		{
			Particle particle = particles[i];
			int num = offset + i;
			_position[num] = particle.position;
			_velocity[num] = particle.velocity;
			_angularVelocity[num] = particle.angularVelocity3D.z * ((float)Math.PI / 180f);
			_rotation[num] = particle.rotation3D.z * ((float)Math.PI / 180f);
			_startSize[num] = particle.startSize3D;
			_startColor[num] = particle.startColor;
			_seed[num] = particle.randomSeed;
			_lifetime[num] = Mathf.Max(particle.startLifetime, 0.0001f);
			_age[num] = _lifetime[num] - particle.remainingLifetime;
			RefreshAppearance(num, Mathf.Clamp01(_age[num] / _lifetime[num]));
		}
		_count = ((offset == 0) ? size : Math.Max(_count, offset + size));
		for (int num2 = _count - 1; num2 >= 0; num2--)
		{
			if (_age[num2] >= _lifetime[num2])
			{
				RemoveAt(num2);
			}
		}
	}

	private static uint NextSystemSeed()
	{
		return Interlocked.Add(ref _nextSystemSeed, 1831565813u) | 1;
	}

	internal void Attach()
	{
		_renderer = GetComponent<ParticleSystemRenderer>() ?? base.gameObject.AddComponent<ParticleSystemRenderer>();
		ParticleSystemDriver.Register(this);
		if (mainState.playOnAwake)
		{
			Play(withChildren: false);
		}
	}

	internal void ReleaseRendering()
	{
		_renderer?.ReleaseMesh();
	}

	internal void FrameUpdate(Quaternion viewRotation)
	{
		if (!(this == null) && base.gameObject.activeInHierarchy)
		{
			if (!_paused)
			{
				float a = (mainState.useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);
				Advance(Mathf.Min(a, 1f / 3f) * mainState.simulationSpeed);
			}
			if (_stopActionArmed && !_playing && _count == 0)
			{
				RunStopAction();
			}
			if (_renderer != null)
			{
				_renderer.Draw(this, viewRotation);
			}
		}
	}

	private void AdvanceChunked(float seconds)
	{
		while (seconds > 0f)
		{
			float num = Mathf.Min(seconds, 1f / 30f);
			Advance(num);
			seconds -= num;
		}
	}

	private void Advance(float dt)
	{
		if (dt <= 0f)
		{
			return;
		}
		UpdateParticles(dt);
		if (!_playing)
		{
			return;
		}
		if (_delayRemaining > 0f)
		{
			_delayRemaining -= dt;
			if (_delayRemaining > 0f)
			{
				return;
			}
			dt = 0f - _delayRemaining;
		}
		float duration = Duration;
		float num = time;
		float num2 = time + dt;
		if (mainState.loop || num < duration)
		{
			EmitOverTime(dt, num, Mathf.Min(num2, duration));
		}
		if (num2 < duration)
		{
			time = num2;
			return;
		}
		if (mainState.loop)
		{
			time = (num2 - duration) % duration;
			FireBursts(0f, time);
			return;
		}
		time = duration;
		if (_count == 0)
		{
			_playing = false;
		}
	}

	private void EmitOverTime(float dt, float from, float to)
	{
		if (emissionState.enabled)
		{
			_emitAccumulator += Mathf.Max(0f, emissionState.rateOverTime.Evaluate(NormalizedTime, NextFloat())) * dt;
			int num = (int)_emitAccumulator;
			_emitAccumulator -= num;
			for (int i = 0; i < num; i++)
			{
				Spawn(default(EmitParams));
			}
			FireBursts(from, to);
		}
	}

	private void FireBursts(float from, float to)
	{
		Burst[] bursts = emissionState.bursts;
		if (!emissionState.enabled || bursts.Length == 0)
		{
			return;
		}
		Burst[] array = bursts;
		for (int i = 0; i < array.Length; i++)
		{
			Burst burst = array[i];
			float num = Mathf.Max(burst.repeatInterval, 0.0001f);
			for (int j = Math.Max(0, (int)MathF.Ceiling((from - burst.time) / num)); burst.cycleCount <= 0 || j < burst.cycleCount; j++)
			{
				float num2 = burst.time + (float)j * num;
				if (num2 >= to)
				{
					break;
				}
				if (!(num2 < from) && !(NextFloat() > burst.probability))
				{
					int num3 = (int)burst.count.Evaluate(NormalizedTime, NextFloat());
					for (int k = 0; k < num3; k++)
					{
						Spawn(default(EmitParams));
					}
				}
			}
		}
	}

	private void RunStopAction()
	{
		_stopActionArmed = false;
		switch (mainState.stopAction)
		{
		case ParticleSystemStopAction.Disable:
			base.gameObject.SetActive(value: false);
			break;
		case ParticleSystemStopAction.Destroy:
			Object.Destroy(base.gameObject);
			break;
		case ParticleSystemStopAction.Callback:
			StubWarner.LogStub("RunStopAction", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\ParticleSystemSimulation.cs", 174);
			break;
		}
	}

	private void UpdateParticles(float dt)
	{
		for (int num = _count - 1; num >= 0; num--)
		{
			_age[num] += dt;
			if (_age[num] >= _lifetime[num])
			{
				RemoveAt(num);
			}
		}
		VelocityOverLifetimeState velocityOverLifetimeState = velocityState;
		NoiseState noiseState = this.noiseState;
		if (velocityOverLifetimeState.enabled && (IsNonZero(velocityOverLifetimeState.orbitalX) || IsNonZero(velocityOverLifetimeState.orbitalY) || IsNonZero(velocityOverLifetimeState.orbitalZ) || IsNonZero(velocityOverLifetimeState.radial)))
		{
			StubWarner.LogStub("UpdateParticles", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\ParticleSystemSimulation.cs", 191);
		}
		if (noiseState.enabled && (noiseState.remapEnabled || IsNonZero(noiseState.rotationAmount) || IsNonZero(noiseState.sizeAmount)))
		{
			StubWarner.LogStub("UpdateParticles", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\ParticleSystemSimulation.cs", 193);
		}
		if (collisionState.enabled)
		{
			StubWarner.LogStub("UpdateParticles", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\ParticleSystemSimulation.cs", 195);
		}
		_step = new MoveStep(dt, NormalizedTime, IsNonZero(mainState.gravityModifier), SimulationRotationFrom(velocityOverLifetimeState.space));
		if (noiseState.enabled)
		{
			_noiseScroll += noiseState.scrollSpeed.Evaluate(_step.SystemTime, NextFloat()) * dt;
		}
		if (!noiseState.enabled || _count < 1024)
		{
			MoveRange(0, _count);
			return;
		}
		if (_moveBatch == null)
		{
			_moveBatch = delegate(int batch)
			{
				MoveRange(batch * 512, Math.Min(_count, (batch + 1) * 512));
			};
		}
		Parallel.For(0, (_count + 512 - 1) / 512, _moveBatch);
	}

	private void MoveRange(int start, int end)
	{
		MoveStep step = _step;
		VelocityOverLifetimeState velocityOverLifetimeState = velocityState;
		bool enabled = noiseState.enabled;
		for (int i = start; i < end; i++)
		{
			float t = _age[i] / _lifetime[i];
			uint seed = _seed[i];
			if (step.HasGravity)
			{
				_velocity[i] += Gravity * (mainState.gravityModifier.Evaluate(step.SystemTime, SeedFloat(seed, 1u)) * step.Dt);
			}
			if (limitVelocityState.enabled)
			{
				_velocity[i] = LimitVelocity(i, _velocity[i], t, seed, step.Dt);
			}
			Vector3 vector = _velocity[i];
			if (velocityOverLifetimeState.enabled)
			{
				Vector3 vector2 = new Vector3(velocityOverLifetimeState.x.Evaluate(t, SeedFloat(seed, 2u)), velocityOverLifetimeState.y.Evaluate(t, SeedFloat(seed, 3u)), velocityOverLifetimeState.z.Evaluate(t, SeedFloat(seed, 4u)));
				vector = (vector + step.LinearSpace * vector2) * velocityOverLifetimeState.speedModifier.Evaluate(t, SeedFloat(seed, 5u));
			}
			if (enabled)
			{
				vector += NoiseVelocity(_position[i], t, seed);
			}
			_position[i] += vector * step.Dt;
			float num = _angularVelocity[i];
			if (rotationState.enabled)
			{
				num += rotationState.z.Evaluate(t, SeedFloat(seed, 16u));
			}
			_rotation[i] += num * step.Dt;
			RefreshAppearance(i, t);
		}
	}

	private Vector3 LimitVelocity(int i, Vector3 velocity, float t, uint seed, float dt)
	{
		LimitVelocityOverLifetimeState limitVelocityOverLifetimeState = limitVelocityState;
		float magnitude = velocity.magnitude;
		float num = limitVelocityOverLifetimeState.drag.Evaluate(t, SeedFloat(seed, 17u));
		if (num > 0f && magnitude > 0f)
		{
			if (limitVelocityOverLifetimeState.multiplyDragByParticleSize)
			{
				float num2 = 0.5f * (_currentSize[i].x + _currentSize[i].y);
				num *= num2 * num2;
			}
			if (limitVelocityOverLifetimeState.multiplyDragByParticleVelocity)
			{
				num *= magnitude;
			}
			velocity *= Mathf.Max(0f, 1f - num * dt);
		}
		float num3 = 1f - MathF.Pow(1f - Mathf.Clamp01(limitVelocityOverLifetimeState.dampen), dt * 30f);
		if (limitVelocityOverLifetimeState.separateAxes)
		{
			Vector3 vector = new Vector3(limitVelocityOverLifetimeState.limitX.Evaluate(t, SeedFloat(seed, 18u)), limitVelocityOverLifetimeState.limitY.Evaluate(t, SeedFloat(seed, 19u)), limitVelocityOverLifetimeState.limitZ.Evaluate(t, SeedFloat(seed, 20u)));
			for (int j = 0; j < 3; j++)
			{
				float num4 = MathF.Abs(velocity[j]) - vector[j];
				if (num4 > 0f)
				{
					velocity[j] -= (float)MathF.Sign(velocity[j]) * num4 * num3;
				}
			}
			return velocity;
		}
		magnitude = velocity.magnitude;
		float num5 = limitVelocityOverLifetimeState.limit.Evaluate(t, SeedFloat(seed, 18u));
		if (magnitude > num5 && magnitude > 0f)
		{
			velocity *= (magnitude - (magnitude - num5) * num3) / magnitude;
		}
		return velocity;
	}

	private static bool IsNonZero(MinMaxCurve curve)
	{
		if (curve.mode == ParticleSystemCurveMode.Constant)
		{
			return curve.constant != 0f;
		}
		return true;
	}

	private Quaternion SimulationRotationFrom(ParticleSystemSimulationSpace space)
	{
		ParticleSystemSimulationSpace simulationSpace = mainState.simulationSpace;
		if (space == simulationSpace)
		{
			return Quaternion.identity;
		}
		if (space != 0)
		{
			return Quaternion.Inverse(base.transform.rotation);
		}
		return base.transform.rotation;
	}

	private Vector3 NoiseVelocity(Vector3 position, float t, uint seed)
	{
		NoiseState noiseState = this.noiseState;
		Vector3 a;
		if (noiseState.separateAxes)
		{
			a = new Vector3(noiseState.strengthX.Evaluate(t, SeedFloat(seed, 6u)), noiseState.strengthY.Evaluate(t, SeedFloat(seed, 7u)), noiseState.strengthZ.Evaluate(t, SeedFloat(seed, 8u)));
		}
		else
		{
			float num = noiseState.strength.Evaluate(t, SeedFloat(seed, 6u));
			a = new Vector3(num, num, num);
		}
		float num2 = Mathf.Max(noiseState.frequency, 0.0001f);
		a *= noiseState.positionAmount.Evaluate(t, SeedFloat(seed, 9u));
		if (noiseState.damping)
		{
			a /= num2;
		}
		return Vector3.Scale(a, CurlNoise(position, num2));
	}

	private Vector3 CurlNoise(Vector3 position, float frequency)
	{
		NoiseState noiseState = this.noiseState;
		Vector3 zero = Vector3.zero;
		float num = 0.2444f;
		int num2 = Math.Clamp(noiseState.octaveCount, 1, 4);
		for (int i = 0; i < num2; i++)
		{
			Unity.Mathematics.noise.snoise(new float3(position.x * frequency, position.y * frequency + _noiseScroll, position.z * frequency), out var gradient);
			zero += new Vector3(0f - gradient.z, 0f, gradient.x) * num;
			num *= noiseState.octaveMultiplier;
			frequency *= noiseState.octaveScale;
		}
		return zero;
	}

	private void RefreshAppearance(int i, float t)
	{
		uint seed = _seed[i];
		Vector2 vector = new Vector2(_startSize[i].x, _startSize[i].y);
		SizeOverLifetimeState sizeOverLifetimeState = sizeState;
		if (sizeOverLifetimeState.enabled)
		{
			if (sizeOverLifetimeState.separateAxes)
			{
				vector = new Vector2(vector.x * sizeOverLifetimeState.x.Evaluate(t, SeedFloat(seed, 10u)), vector.y * sizeOverLifetimeState.y.Evaluate(t, SeedFloat(seed, 11u)));
			}
			else
			{
				vector *= sizeOverLifetimeState.size.Evaluate(t, SeedFloat(seed, 10u));
			}
		}
		_currentSize[i] = vector;
		ColorOverLifetimeState colorOverLifetimeState = colorState;
		_currentColor[i] = (colorOverLifetimeState.enabled ? ((Color32)(_startColor[i] * colorOverLifetimeState.color.Evaluate(t, SeedFloat(seed, 12u)))) : _startColor[i]);
		if (textureSheetState.enabled)
		{
			_frame[i] = SheetFrame(i, t, seed);
		}
	}

	private int SheetFrame(int i, float t, uint seed)
	{
		TextureSheetAnimationState textureSheetAnimationState = textureSheetState;
		int num = Math.Max(1, textureSheetAnimationState.numTilesX);
		int num2 = Math.Max(1, textureSheetAnimationState.numTilesY);
		int num3 = ((textureSheetAnimationState.animation == ParticleSystemAnimationType.WholeSheet) ? (num * num2) : num);
		float num4 = ((textureSheetAnimationState.timeMode == ParticleSystemAnimationTimeMode.FPS) ? (_age[i] * textureSheetAnimationState.fps / (float)num3) : (t * (float)Math.Max(1, textureSheetAnimationState.cycleCount)));
		float num5 = textureSheetAnimationState.frameOverTime.Evaluate(num4 - MathF.Floor(num4), SeedFloat(seed, 13u)) + textureSheetAnimationState.startFrame.Evaluate(0f, SeedFloat(seed, 14u));
		int num6 = Math.Min(num3 - 1, (int)((num5 - MathF.Floor(num5)) * (float)num3));
		if (textureSheetAnimationState.animation == ParticleSystemAnimationType.WholeSheet)
		{
			return num6;
		}
		return Math.Clamp((textureSheetAnimationState.rowMode == ParticleSystemAnimationRowMode.Random) ? ((int)(SeedFloat(seed, 15u) * (float)num2)) : textureSheetAnimationState.rowIndex, 0, num2 - 1) * num + num6;
	}

	private void Spawn(in EmitParams emit)
	{
		if (_count >= this.mainState.maxParticles)
		{
			return;
		}
		bool flag = emit._positionSet && !emit.applyShapeToPosition;
		if (SampleShape(out var position, out var direction) || flag)
		{
			MainState mainState = this.mainState;
			float normalizedTime = NormalizedTime;
			Vector3 vector = direction * mainState.startSpeed.Evaluate(normalizedTime, NextFloat());
			if (mainState.simulationSpace != 0)
			{
				Matrix4x4 localToWorldMatrix = base.transform.localToWorldMatrix;
				position = localToWorldMatrix.MultiplyPoint3x4(position);
				vector = localToWorldMatrix.MultiplyVector(vector);
			}
			if (emit._positionSet)
			{
				position = (emit.applyShapeToPosition ? (position + emit.position) : emit.position);
			}
			if (emit._velocitySet)
			{
				vector = emit.velocity;
			}
			EnsureCapacity(_count + 1);
			int num = _count++;
			_position[num] = position;
			_velocity[num] = vector;
			_age[num] = 0f;
			_lifetime[num] = Mathf.Max(emit._startLifetimeSet ? emit.startLifetime : mainState.startLifetime.Evaluate(normalizedTime, NextFloat()), 0.0001f);
			if (emit._startSizeSet)
			{
				_startSize[num] = emit.startSize3D;
			}
			else if (mainState.startSize3D)
			{
				_startSize[num] = new Vector3(mainState.startSizeX.Evaluate(normalizedTime, NextFloat()), mainState.startSizeY.Evaluate(normalizedTime, NextFloat()), mainState.startSizeZ.Evaluate(normalizedTime, NextFloat()));
			}
			else
			{
				float num2 = mainState.startSize.Evaluate(normalizedTime, NextFloat());
				_startSize[num] = new Vector3(num2, num2, num2);
			}
			if (emit._rotationSet)
			{
				_rotation[num] = emit.rotation3D.z * ((float)Math.PI / 180f);
			}
			else
			{
				_rotation[num] = (mainState.startRotation3D ? mainState.startRotationZ : mainState.startRotation).Evaluate(normalizedTime, NextFloat());
			}
			_angularVelocity[num] = (emit._angularVelocitySet ? (emit.angularVelocity3D.z * ((float)Math.PI / 180f)) : 0f);
			_startColor[num] = (emit._startColorSet ? emit.startColor : ((Color32)mainState.startColor.Evaluate(normalizedTime, NextFloat())));
			_seed[num] = (emit._randomSeedSet ? emit.randomSeed : NextUInt());
			RefreshAppearance(num, 0f);
		}
	}

	private bool SampleShape(out Vector3 position, out Vector3 direction)
	{
		ShapeState shapeState = this.shapeState;
		position = Vector3.zero;
		direction = Vector3.forward;
		if (!shapeState.enabled)
		{
			return true;
		}
		if (!SampleShapeLocal(shapeState, out position, out direction))
		{
			return false;
		}
		Quaternion quaternion = Quaternion.Euler(shapeState.rotation);
		position = Matrix4x4.TRS(shapeState.position, quaternion, shapeState.scale).MultiplyPoint3x4(position);
		direction = quaternion * direction;
		if (shapeState.randomDirectionAmount > 0f)
		{
			direction = Vector3.Lerp(direction, RandomOnUnitSphere(), Mathf.Clamp01(shapeState.randomDirectionAmount));
		}
		if (shapeState.sphericalDirectionAmount > 0f)
		{
			direction = Vector3.Lerp(direction, (position - shapeState.position).normalized, Mathf.Clamp01(shapeState.sphericalDirectionAmount));
		}
		if (shapeState.randomPositionAmount > 0f)
		{
			position += RandomOnUnitSphere() * (shapeState.randomPositionAmount * NextFloat());
		}
		direction = ((direction.sqrMagnitude > 0f) ? direction.normalized : Vector3.forward);
		return true;
	}

	private bool SampleShapeLocal(ShapeState shape, out Vector3 position, out Vector3 direction)
	{
		switch (shape.shapeType)
		{
		case ParticleSystemShapeType.Sphere:
		case ParticleSystemShapeType.SphereShell:
		case ParticleSystemShapeType.Hemisphere:
		case ParticleSystemShapeType.HemisphereShell:
		{
			direction = RandomOnUnitSphere();
			ParticleSystemShapeType shapeType = shape.shapeType;
			if ((uint)(shapeType - 2) <= 1u)
			{
				direction.z = MathF.Abs(direction.z);
			}
			shapeType = shape.shapeType;
			bool flag = ((shapeType == ParticleSystemShapeType.SphereShell || shapeType == ParticleSystemShapeType.HemisphereShell) ? true : false);
			bool flag2 = flag;
			position = direction * (shape.radius * (flag2 ? 1f : RadialFraction(shape.radiusThickness, 3)));
			return true;
		}
		case ParticleSystemShapeType.Cone:
		case ParticleSystemShapeType.ConeShell:
		case ParticleSystemShapeType.ConeVolume:
		case ParticleSystemShapeType.ConeVolumeShell:
		{
			ParticleSystemShapeType shapeType = shape.shapeType;
			bool flag = ((shapeType == ParticleSystemShapeType.ConeShell || shapeType == ParticleSystemShapeType.ConeVolumeShell) ? true : false);
			float num2 = (flag ? 1f : RadialFraction(shape.radiusThickness, 2));
			float x2 = NextFloat() * shape.arc * ((float)Math.PI / 180f);
			Vector3 vector = new Vector3(MathF.Cos(x2), MathF.Sin(x2), 0f);
			float num3 = MathF.Tan(Mathf.Clamp(shape.angle, 0f, 89.9f) * ((float)Math.PI / 180f)) * num2;
			position = vector * (shape.radius * num2);
			direction = new Vector3(vector.x * num3, vector.y * num3, 1f).normalized;
			shapeType = shape.shapeType;
			if ((uint)(shapeType - 8) <= 1u)
			{
				position += direction * (shape.length * NextFloat());
			}
			return true;
		}
		case ParticleSystemShapeType.Box:
		case ParticleSystemShapeType.BoxShell:
		case ParticleSystemShapeType.BoxEdge:
		{
			position = new Vector3(NextFloat() - 0.5f, NextFloat() - 0.5f, NextFloat() - 0.5f);
			int num4 = ((shape.shapeType == ParticleSystemShapeType.BoxShell) ? 1 : ((shape.shapeType == ParticleSystemShapeType.BoxEdge) ? 2 : 0));
			int num5 = (int)(NextFloat() * 3f) % 3;
			for (int i = 0; i < num4; i++)
			{
				position[(num5 + i) % 3] = ((NextFloat() < 0.5f) ? (-0.5f) : 0.5f);
			}
			direction = Vector3.forward;
			return true;
		}
		case ParticleSystemShapeType.Circle:
		case ParticleSystemShapeType.CircleEdge:
		{
			float num = ((shape.shapeType == ParticleSystemShapeType.CircleEdge) ? 1f : RadialFraction(shape.radiusThickness, 2));
			float x = NextFloat() * shape.arc * ((float)Math.PI / 180f);
			direction = new Vector3(MathF.Cos(x), MathF.Sin(x), 0f);
			position = direction * (shape.radius * num);
			return true;
		}
		case ParticleSystemShapeType.Rectangle:
			position = new Vector3(NextFloat() - 0.5f, NextFloat() - 0.5f, 0f);
			direction = Vector3.forward;
			return true;
		case ParticleSystemShapeType.SingleSidedEdge:
			position = new Vector3((NextFloat() * 2f - 1f) * shape.radius, 0f, 0f);
			direction = Vector3.up;
			return true;
		case ParticleSystemShapeType.Mesh:
			return SampleMesh(shape.mesh, shape.meshShapeType, out position, out direction);
		case ParticleSystemShapeType.MeshRenderer:
		{
			MeshFilter meshFilter = ((shape.meshRenderer != null) ? shape.meshRenderer.GetComponent<MeshFilter>() : null);
			return SampleMesh((meshFilter != null) ? meshFilter.sharedMesh : null, shape.meshShapeType, out position, out direction);
		}
		default:
			StubWarner.LogStub("SampleShapeLocal", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\ParticleSystemSimulation.cs", 531);
			position = Vector3.zero;
			direction = Vector3.forward;
			return true;
		}
	}

	private bool SampleMesh(Mesh mesh, ParticleSystemMeshShapeType type, out Vector3 position, out Vector3 direction)
	{
		position = Vector3.zero;
		direction = Vector3.forward;
		if (mesh == null)
		{
			return false;
		}
		Vector3[] verticesForReading = mesh.verticesForReading;
		if (verticesForReading.Length == 0)
		{
			return false;
		}
		int[] trianglesForReading = mesh.trianglesForReading;
		if (type == ParticleSystemMeshShapeType.Vertex || trianglesForReading.Length < 3)
		{
			int num = (int)(NextFloat() * (float)verticesForReading.Length) % verticesForReading.Length;
			Vector3[] normalsForReading = mesh.normalsForReading;
			position = verticesForReading[num];
			if (normalsForReading.Length == verticesForReading.Length)
			{
				direction = normalsForReading[num];
			}
			return true;
		}
		int num2 = (int)(NextFloat() * (float)(trianglesForReading.Length / 3)) % (trianglesForReading.Length / 3) * 3;
		Vector3 vector = verticesForReading[trianglesForReading[num2]];
		Vector3 vector2 = verticesForReading[trianglesForReading[num2 + 1]];
		Vector3 vector3 = verticesForReading[trianglesForReading[num2 + 2]];
		direction = Vector3.Cross(vector2 - vector, vector3 - vector).normalized;
		if (type == ParticleSystemMeshShapeType.Edge)
		{
			int num3 = (int)(NextFloat() * 3f) % 3;
			position = Vector3.Lerp(num3 switch
			{
				1 => vector2, 
				0 => vector, 
				_ => vector3, 
			}, num3 switch
			{
				1 => vector3, 
				0 => vector2, 
				_ => vector, 
			}, NextFloat());
			return true;
		}
		float num4 = NextFloat();
		float num5 = NextFloat();
		if (num4 + num5 > 1f)
		{
			num4 = 1f - num4;
			num5 = 1f - num5;
		}
		position = vector + (vector2 - vector) * num4 + (vector3 - vector) * num5;
		return true;
	}

	private float RadialFraction(float thickness, int power)
	{
		float num = 1f - Mathf.Clamp01(thickness);
		float x = Mathf.Lerp((power == 2) ? (num * num) : (num * num * num), 1f, NextFloat());
		if (power != 2)
		{
			return MathF.Cbrt(x);
		}
		return MathF.Sqrt(x);
	}

	private Vector3 RandomOnUnitSphere()
	{
		float num = NextFloat() * 2f - 1f;
		float x = NextFloat() * (float)Math.PI * 2f;
		float num2 = MathF.Sqrt(1f - num * num);
		return new Vector3(num2 * MathF.Cos(x), num2 * MathF.Sin(x), num);
	}

	private void EnsureCapacity(int required)
	{
		if (_position.Length < required)
		{
			int newSize = Math.Max(required, Math.Max(16, _position.Length * 2));
			Array.Resize(ref _position, newSize);
			Array.Resize(ref _velocity, newSize);
			Array.Resize(ref _age, newSize);
			Array.Resize(ref _lifetime, newSize);
			Array.Resize(ref _rotation, newSize);
			Array.Resize(ref _angularVelocity, newSize);
			Array.Resize(ref _startSize, newSize);
			Array.Resize(ref _startColor, newSize);
			Array.Resize(ref _seed, newSize);
			Array.Resize(ref _frame, newSize);
			Array.Resize(ref _currentSize, newSize);
			Array.Resize(ref _currentColor, newSize);
		}
	}

	private void RemoveAt(int i)
	{
		int num = --_count;
		_position[i] = _position[num];
		_velocity[i] = _velocity[num];
		_age[i] = _age[num];
		_lifetime[i] = _lifetime[num];
		_rotation[i] = _rotation[num];
		_angularVelocity[i] = _angularVelocity[num];
		_startSize[i] = _startSize[num];
		_startColor[i] = _startColor[num];
		_seed[i] = _seed[num];
		_frame[i] = _frame[num];
		_currentSize[i] = _currentSize[num];
		_currentColor[i] = _currentColor[num];
	}

	private uint NextUInt()
	{
		_rng ^= _rng << 13;
		_rng ^= _rng >> 17;
		_rng ^= _rng << 5;
		return _rng;
	}

	private float NextFloat()
	{
		return (float)(NextUInt() >> 8) * 5.9604645E-08f;
	}

	internal static float SeedFloat(uint seed, uint property)
	{
		int num = (int)seed ^ ((int)property * -1640531527);
		int num2 = (num ^ (num >>> 16)) * 2146121005;
		int num3 = (num2 ^ (num2 >>> 15)) * -2073254261;
		return (float)((uint)(num3 ^ (num3 >>> 16)) >> 8) * 5.9604645E-08f;
	}
}
