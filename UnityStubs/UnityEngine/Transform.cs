using System.Collections;
using Waylith.UnityCompat;

namespace UnityEngine;

public class Transform : Component, IEnumerable
{
	public class Enumerator : IEnumerator
	{
		private readonly Transform _transform;

		private int _index = -1;

		public object Current => _transform.GetChild(_index);

		public Enumerator(Transform transform)
		{
			_transform = transform;
		}

		public bool MoveNext()
		{
			_index++;
			return _index < _transform.childCount;
		}

		public void Reset()
		{
			_index = -1;
		}
	}

	private Vector3 _localPosition;

	private Vector3 _localScale = new Vector3(1f, 1f, 1f);

	private Quaternion _localRotation = Quaternion.identity;

	private static long _changeCounter;

	private long _changedAt = 1L;

	private long _clearedAt;

	public Quaternion localRotation
	{
		get
		{
			return _localRotation;
		}
		set
		{
			_localRotation = value;
			MarkChanged();
		}
	}

	public bool hasChanged
	{
		get
		{
			Transform transform = this;
			while (transform != null)
			{
				if (transform._changedAt > _clearedAt)
				{
					return true;
				}
				transform = transform.GetParentTransform();
			}
			return false;
		}
		set
		{
			_clearedAt = (value ? 0 : (++_changeCounter));
		}
	}

	public int childCount => UnityRuntime.Current?.World.GetTransformChildCount(this) ?? 0;

	public Vector3 forward => rotation * Vector3.forward;

	public Matrix4x4 localToWorldMatrix
	{
		get
		{
			Matrix4x4 matrix4x = Matrix4x4.TRS(_localPosition, _localRotation, _localScale);
			Transform parentTransform = GetParentTransform();
			if (!(parentTransform == null))
			{
				return parentTransform.localToWorldMatrix * matrix4x;
			}
			return matrix4x;
		}
	}

	public Vector3 position
	{
		get
		{
			return localToWorldMatrix.GetPosition();
		}
		set
		{
			Transform parentTransform = GetParentTransform();
			_localPosition = ((parentTransform == null) ? value : parentTransform.worldToLocalMatrix.MultiplyPoint3x4(value));
			MarkChanged();
		}
	}

	public Vector3 right => rotation * Vector3.right;

	public Quaternion rotation
	{
		get
		{
			Transform parentTransform = GetParentTransform();
			if (!(parentTransform == null))
			{
				return parentTransform.rotation * _localRotation;
			}
			return _localRotation;
		}
		set
		{
			Transform parentTransform = GetParentTransform();
			_localRotation = ((parentTransform == null) ? value : (Quaternion.Inverse(parentTransform.rotation) * value));
			MarkChanged();
		}
	}

	public Vector3 up => rotation * Vector3.up;

	public Vector3 localEulerAngles
	{
		get
		{
			return _localRotation.eulerAngles;
		}
		set
		{
			localRotation = Quaternion.Euler(value);
		}
	}

	public Vector3 localPosition
	{
		get
		{
			return _localPosition;
		}
		set
		{
			_localPosition = value;
			MarkChanged();
		}
	}

	public Vector3 localScale
	{
		get
		{
			return _localScale;
		}
		set
		{
			_localScale = value;
			MarkChanged();
		}
	}

	public Transform parent
	{
		get
		{
			return (Transform)(UnityRuntime.Current?.World.GetTransformParent(this));
		}
		set
		{
			SetParent(value, worldPositionStays: true);
		}
	}

	public Matrix4x4 worldToLocalMatrix => Matrix4x4.Inverse(localToWorldMatrix);

	public Vector3 eulerAngles
	{
		get
		{
			return rotation.eulerAngles;
		}
		set
		{
			rotation = Quaternion.Euler(value);
		}
	}

	public void SetParent(Transform parent)
	{
		SetParent(parent, worldPositionStays: true);
	}

	private void MarkChanged()
	{
		_changedAt = ++_changeCounter;
	}

	public IEnumerator GetEnumerator()
	{
		return new Enumerator(this);
	}

	public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
	{
		this.rotation = rotation;
		this.position = position;
	}

	public void SetLocalPositionAndRotation(Vector3 localPosition, Quaternion localRotation)
	{
		this.localPosition = localPosition;
		this.localRotation = localRotation;
	}

	public Transform GetChild(int index)
	{
		return (Transform)UnityRuntime.Current.World.GetTransformChild(this, index);
	}

	public bool IsChildOf(Transform parent)
	{
		Transform transform = this;
		while (transform != null)
		{
			if (transform == parent)
			{
				return true;
			}
			transform = transform.parent;
		}
		return false;
	}

	public void SetParent(Transform parent, bool worldPositionStays)
	{
		if (UnityRuntime.Current != null)
		{
			Vector3 vector = position;
			Quaternion quaternion = rotation;
			UnityRuntime.Current.World.SetTransformParent(this, parent);
			if (worldPositionStays)
			{
				position = vector;
				rotation = quaternion;
			}
			MarkChanged();
		}
	}

	public Vector3 InverseTransformPoint(Vector3 position)
	{
		return worldToLocalMatrix.MultiplyPoint3x4(position);
	}

	public Vector3 TransformDirection(Vector3 direction)
	{
		return rotation * direction;
	}

	public Vector3 InverseTransformDirection(Vector3 direction)
	{
		return Quaternion.Inverse(rotation) * direction;
	}

	public void LookAt(Vector3 worldPosition)
	{
		LookAt(worldPosition, Vector3.up);
	}

	public void LookAt(Vector3 worldPosition, Vector3 worldUp)
	{
		Vector3 vector = worldPosition - position;
		if (vector.sqrMagnitude > 0f)
		{
			rotation = Quaternion.LookRotation(vector, worldUp);
		}
	}

	public void LookAt(Transform target)
	{
		if (target != null)
		{
			LookAt(target.position);
		}
	}

	public Transform Find(string n)
	{
		if (string.IsNullOrEmpty(n))
		{
			return null;
		}
		int num = n.IndexOf('/');
		string text = ((num < 0) ? n : n.Substring(0, num));
		for (int i = 0; i < childCount; i++)
		{
			Transform child = GetChild(i);
			if (child != null && child.name == text)
			{
				if (num >= 0)
				{
					return child.Find(n.Substring(num + 1));
				}
				return child;
			}
		}
		return null;
	}

	public Vector3 TransformPoint(Vector3 position)
	{
		return localToWorldMatrix.MultiplyPoint3x4(position);
	}

	public Vector3 TransformVector(Vector3 vector)
	{
		return localToWorldMatrix.MultiplyVector(vector);
	}

	public Vector3 InverseTransformVector(Vector3 vector)
	{
		return worldToLocalMatrix.MultiplyVector(vector);
	}

	public int GetSiblingIndex()
	{
		return UnityRuntime.Current?.World.GetTransformSiblingIndex(this) ?? 0;
	}

	public void SetSiblingIndex(int index)
	{
		UnityRuntime.Current?.World.SetTransformSiblingIndex(this, index);
	}

	public void SetAsFirstSibling()
	{
		SetSiblingIndex(0);
	}

	public void SetAsLastSibling()
	{
		SetSiblingIndex(int.MaxValue);
	}

	private Transform GetParentTransform()
	{
		if (UnityRuntime.Current != null)
		{
			return (Transform)UnityRuntime.Current.World.GetTransformParent(this);
		}
		return null;
	}

	public void Translate(Vector3 translation)
	{
		position += rotation * translation;
	}

	public void Translate(float x, float y, float z)
	{
		Translate(new Vector3(x, y, z));
	}

	public void Translate(Vector3 translation, Space relativeTo)
	{
		position += ((relativeTo == Space.World) ? translation : TransformDirection(translation));
	}

	public void Translate(float x, float y, float z, Space relativeTo)
	{
		Translate(new Vector3(x, y, z), relativeTo);
	}

	public void Translate(Vector3 translation, Transform relativeTo)
	{
		position += ((relativeTo != null) ? relativeTo.TransformDirection(translation) : translation);
	}

	public void Translate(float x, float y, float z, Transform relativeTo)
	{
		Translate(new Vector3(x, y, z), relativeTo);
	}

	public void Rotate(Vector3 eulers)
	{
		Rotate(eulers, Space.Self);
	}

	public void Rotate(float xAngle, float yAngle, float zAngle)
	{
		Rotate(new Vector3(xAngle, yAngle, zAngle), Space.Self);
	}

	public void Rotate(float xAngle, float yAngle, float zAngle, Space relativeTo)
	{
		Rotate(new Vector3(xAngle, yAngle, zAngle), relativeTo);
	}

	public void Rotate(Vector3 axis, float angle)
	{
		Rotate(axis, angle, Space.Self);
	}

	public void Rotate(Vector3 eulers, Space relativeTo)
	{
		Quaternion quaternion = Quaternion.Euler(eulers.x, eulers.y, eulers.z);
		if (relativeTo == Space.Self)
		{
			localRotation *= quaternion;
		}
		else
		{
			rotation *= Quaternion.Inverse(rotation) * quaternion * rotation;
		}
	}

	public void Rotate(Vector3 axis, float angle, Space relativeTo)
	{
		rotation = Quaternion.AngleAxis(angle, (relativeTo == Space.Self) ? TransformDirection(axis) : axis) * rotation;
	}

	public void RotateAround(Vector3 point, Vector3 axis, float angle)
	{
		Quaternion quaternion = Quaternion.AngleAxis(angle, axis);
		position = point + quaternion * (position - point);
		rotation = quaternion * rotation;
	}
}
