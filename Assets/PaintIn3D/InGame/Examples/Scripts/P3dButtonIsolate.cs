using UnityEngine;
using UnityEngine.EventSystems;

namespace PaintIn3D
{
	/// <summary>This component allows you to turn a UI element into a button that will activate the target GameObject, and deactivate its siblings. If the target GameObject is active, then the button will be faded in.</summary>
	[ExecuteInEditMode]
	[HelpURL(P3dHelper.HelpUrlPrefix + "P3dButtonIsolate")]
	[AddComponentMenu(P3dHelper.ComponentMenuPrefix + "Button Isolate")]
	public class P3dButtonIsolate : MonoBehaviour, IPointerDownHandler
	{
		public enum ToggleType
		{
			KeepSelected,
			ToggleSelection,
			SelectPrevious
		}

		/// <summary>If this GameObject is active, then the button will be faded in.</summary>
		public Transform Target { set { target = value; } get { return target; } } [SerializeField] private Transform target;

		/// <summary>If this button is already selected and you click/tap it again, what should happen?</summary>
		public ToggleType Toggle { set { toggle = value; } get { return toggle; } } [SerializeField] private ToggleType toggle;

		private Transform previousChild;

		protected virtual void Update()
		{
			if (target != null)
			{
				var group = GetComponent<CanvasGroup>();

				if (group != null)
				{
					group.alpha = target.gameObject.activeInHierarchy == true ? 1.0f : 0.5f;
				}
			}
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			if (target != null)
			{
				var parent = target.transform.parent;
				var active = target.gameObject.activeSelf;

				foreach (Transform child in parent.transform)
				{
					if (child.gameObject.activeSelf == true)
					{
						if (child != target)
						{
							previousChild = child;
						}

						child.gameObject.SetActive(false);
					}
				}

				switch (toggle)
				{
					case ToggleType.KeepSelected:
					{
						target.gameObject.SetActive(true);
					}
					break;

					case ToggleType.ToggleSelection:
					{
						target.gameObject.SetActive(active == false);
					}
					break;

					case ToggleType.SelectPrevious:
					{
						if (active == true && previousChild != null)
						{
							previousChild.gameObject.SetActive(true);
						}
						else
						{
							target.gameObject.SetActive(true);
						}
					}
					break;
				}
			}
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dButtonIsolate;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dButtonIsolate_Editor : P3dEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			BeginError(Any(tgts, t => t.Target == null));
				Draw("target", "If this GameObject is active, then the button will be faded in.");
			EndError();
			Draw("toggle", "If this button is already selected and you click/tap it again, what should happen?");
		}
	}
}
#endif