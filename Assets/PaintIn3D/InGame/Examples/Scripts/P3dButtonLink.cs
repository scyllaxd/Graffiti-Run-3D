using UnityEngine;
using UnityEngine.EventSystems;

namespace PaintIn3D
{
	/// <summary>This component can open a URL. This can be done by attaching it to a clickable object, or manually from the Open methods.</summary>
	[HelpURL(P3dHelper.HelpUrlPrefix + "P3dButtonLink")]
	[AddComponentMenu(P3dHelper.ComponentMenuPrefix + "Button Link")]
	public class P3dButtonLink : MonoBehaviour, IPointerClickHandler
	{
		/// <summary>The URL that will be opened.</summary>
		public string Url { set { url = value; } get { return url; } } [SerializeField] private string url;

		public void OnPointerClick(PointerEventData eventData)
		{
			Open();
		}

		[ContextMenu("This allows you to manually open the current URL.")]
		public void Open()
		{
			Open(url);
		}

		/// <summary>This allows you to open the specified URL.</summary>
		public void Open(string url)
		{
			Application.OpenURL(url);
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dButtonLink;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dButtonLink_Editor : P3dEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("url", "The URL that will be opened.");
		}
	}
}
#endif