using System.Collections.Generic;
using UnityEngine;

namespace PaintIn3D
{
	/// <summary>This component shows you how to listen for and store paint commands added to any <b>P3dPaintableTexture</b> component in the scene.
	/// This component can then reset each paintable texture, and randomly apply one of the recorded paint commands.</summary>
	[HelpURL(P3dHelper.HelpUrlPrefix + "P3dCommandSerialization")]
	[AddComponentMenu(P3dHelper.ComponentHitMenuPrefix + "Command Serialization")]
	public class P3dCommandSerialization : MonoBehaviour
	{
		[System.Serializable]
		public struct CommandData
		{
			public P3dPaintableTexture PaintableTexture;

			[SerializeReference]
			public P3dCommand LocalCommand;
		}

		/// <summary>Should this component listen for added commands?</summary>
		public bool Listening { set { listening = value; } get { return listening; } } [SerializeField] private bool listening = true;

		/// <summary>All paintable textures and associated commands will be stored here.</summary>
		[SerializeField] private List<CommandData> commandDatas = new List<CommandData>();

		/// <summary>This method will pool and clear all commands.</summary>
		[ContextMenu("Clear")]
		public void Clear()
		{
			foreach (var commandData in commandDatas)
			{
				commandData.LocalCommand.Pool();
			}

			commandDatas.Clear();
		}

		/// <summary>This method will clear all paintable textures, and apply one random paint command that was recorded.</summary>
		[ContextMenu("Rebuild Random Command")]
		public void RebuildRandomCommand()
		{
			// Ignore added commands while this method is running
			var oldListening = listening;

			listening = false;

			// Loop through all paintable textures, and reset them to their original state
			foreach (var paintableTexture in P3dPaintableTexture.Instances)
			{
				paintableTexture.Clear();
			}

			// Randomly pick one command data
			if (commandDatas.Count > 0)
			{
				var index       = Random.Range(0, commandDatas.Count);
				var commandData = commandDatas[index];

				// Make sure it's still valid
				if (commandData.PaintableTexture != null)
				{
					// Convert the command to world space
					var command = commandData.LocalCommand.SpawnCopyWorld(commandData.PaintableTexture.transform);

					// Apply it to its paintable texture
					commandData.PaintableTexture.AddCommand(command);

					// Pool
					command.Pool();
				}
			}

			// Revert listening state
			listening = oldListening;
		}

		[ContextMenu("Serialize And Deserialize")]
		public void SerializeAndDeserialize()
		{
			// This JSON data can be saved or loaded like this
			// NOTE: This requires you to call P3dSerialization.TryRegister(texture) for every texture that is used by the paint commands
			// NOTE: You must modify the hash calculation inside each P3dSerialization.TryRegister method if you want the de/serialization of P3dPaintableTexture + Texture + P3dModel to work across clients or application runs

			var json = JsonUtility.ToJson(this);

			JsonUtility.FromJsonOverwrite(json, this);
		}

		protected virtual void OnEnable()
		{
			P3dPaintableTexture.OnAddCommandGlobal += HandleAddCommandGlobal;
		}

		protected virtual void OnDisable()
		{
			P3dPaintableTexture.OnAddCommandGlobal -= HandleAddCommandGlobal;
		}

		private void HandleAddCommandGlobal(P3dPaintableTexture paintableTexture, P3dCommand command)
		{
			// Ignore commands if we're not listening (this is automatically enabled when rebuilding the paintable texture)
			if (listening == true)
			{
				// Ignore preview paint commands
				if (command.Preview == false)
				{
					// Convert the command from world space to local space
					var localCommand = command.SpawnCopyLocal(paintableTexture.transform);

					// Create a CommandData instance for this command and paintable texture pair
					var commandData = new CommandData();

					// Assign the paintable texture and local command
					commandData.PaintableTexture = paintableTexture;
					commandData.LocalCommand     = localCommand;

					// Store the local command
					commandDatas.Add(commandData);
				}
			}
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dCommandSerialization;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dCommandSerialization_Editor : P3dEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("listening", "Should this component listen for added commands?");
		}
	}
}
#endif