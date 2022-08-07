using UnityEngine;

namespace PaintIn3D
{
	/// <summary>This component makes the current gameObject follow the specified camera.</summary>
	[ExecuteInEditMode]
	[HelpURL(P3dHelper.HelpUrlPrefix + "P3dFollow")]
	[AddComponentMenu(P3dHelper.ComponentMenuPrefix + "Follow")]
	public class P3dFollow : MonoBehaviour
	{
		/// <summary>This allows you to set the transform that will be followed.</summary>
		public Transform Target { set { target = value; } get { return target; } } [SerializeField] private Transform target;

		/// <summary>This allows you to set the world space offset of the target transform.</summary>
		public Vector3 Offset { set { offset = value; } get { return offset; } } [SerializeField] private Vector3 offset;

		/// <summary>This allows you to set the Euler offset of the target transform.</summary>
		public Vector3 Tilt { set { tilt = value; } get { return tilt; } } [SerializeField] private Vector3 tilt;

		/// <summary>This allows you to set how quickly the transform follows.
		/// -1 = instant</summary>
		public float Dampening { set { dampening = value; } get { return dampening; } } [SerializeField] private float dampening = 10.0f;

		protected virtual void LateUpdate()
		{
			if (target != null)
			{
				var position = target.TransformPoint(offset);
				var rotation = target.rotation * Quaternion.Euler(tilt);
				var t        = P3dHelper.DampenFactor(dampening, Time.deltaTime);

				transform.position = Vector3.Lerp(transform.position, position, t);
				transform.rotation = Quaternion.Slerp(transform.rotation, rotation, t);
			}
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dFollow;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dFollow_Editor : P3dEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			BeginError(Any(tgts, t => t.Target == null));
				Draw("target", "This allows you to set the transform that will be followed.");
			EndError();
			Draw("offset", "This allows you to set the world space offset of the target transform.");
			Draw("tilt", "This allows you to set the Euler offset of the target transform.");
			Draw("dampening", "This allows you to set how quickly the transform follows.\n-1 = instant");
		}
	}
}
#endif