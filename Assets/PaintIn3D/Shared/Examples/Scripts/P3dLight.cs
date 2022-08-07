using UnityEngine;

namespace PaintIn3D
{
	/// <summary>This component will change the light intensity based on the current renderer.</summary>
	[ExecuteInEditMode]
	[RequireComponent(typeof(Light))]
	[HelpURL(P3dHelper.HelpUrlPrefix + "P3dLight")]
	[AddComponentMenu(P3dHelper.ComponentMenuPrefix + "Light")]
	public class P3dLight : MonoBehaviour
	{
		/// <summary>All light values will be multiplied by this before use.</summary>
		public float Multiplier { set { multiplier = value; } get { return multiplier; } } [SerializeField] private float multiplier = 1.0f;

		/// <summary>This allows you to control the intensity of the attached light when using the <b>Standard</b> rendering pipeline.
		/// -1 = The attached light intensity will not be modified.</summary>
		public float IntensityInStandard { set  { intensityInStandard = value; } get { return intensityInStandard; } } [SerializeField] private float intensityInStandard = 1.0f;

		/// <summary>This allows you to control the intensity of the attached light when using the <b>URP</b> rendering pipeline.
		/// -1 = The attached light intensity will not be modified.</summary>
		public float IntensityInURP { set  { intensityInURP = value; } get { return intensityInURP; } } [SerializeField] private float intensityInURP = 1.0f;

		/// <summary>This allows you to control the intensity of the attached light when using the <b>HDRP</b> rendering pipeline.
		/// -1 = The attached light intensity will not be modified.</summary>
		public float IntensityInHDRP { set  { intensityInHDRP = value; } get { return intensityInHDRP; } } [SerializeField] private float intensityInHDRP = 120000.0f;

		[System.NonSerialized]
		private Light cachedLight;

		[System.NonSerialized]
		private bool cachedLightSet;

		public Light CachedLight
		{
			get
			{
				if (cachedLightSet == false)
				{
					cachedLight    = GetComponent<Light>();
					cachedLightSet = true;
				}

				return cachedLight;
			}
		}

		protected virtual void Update()
		{
			var pipe = P3dShaderBundle.DetectProjectPipeline();

			if (P3dShaderBundle.IsStandard(pipe) == true)
			{
				ApplyIntensity(intensityInStandard);
			}
			else if (P3dShaderBundle.IsURP(pipe) == true)
			{
				ApplyIntensity(intensityInURP);
			}
			else if (P3dShaderBundle.IsHDRP(pipe) == true)
			{
				ApplyIntensity(intensityInHDRP);
			}
		}

		private void ApplyIntensity(float intensity)
		{
			if (intensity >= 0.0f)
			{
				if (cachedLightSet == false)
				{
					cachedLight    = GetComponent<Light>();
					cachedLightSet = true;
				}

				cachedLight.intensity = intensity * multiplier;
			}
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dLight;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dLight_Editor : P3dEditor
	{
		protected override void OnInspector()
		{
			Draw("multiplier", "All light values will be multiplied by this before use.");
			Draw("intensityInStandard", "This allows you to control the intensity of the attached light when using the Standard rendering pipeline.\n\n-1 = The attached light intensity will not be modified.");
			Draw("intensityInURP", "This allows you to control the intensity of the attached light when using the URP rendering pipeline.\n\n-1 = The attached light intensity will not be modified.");
			Draw("intensityInHDRP", "This allows you to control the intensity of the attached light when using the HDRP rendering pipeline.\n\n-1 = The attached light intensity will not be modified.");
		}
	}
}
#endif