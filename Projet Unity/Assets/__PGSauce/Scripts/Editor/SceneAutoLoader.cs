using System;
using MonKey.Extensions;
using PGSauce.Core.PGDebugging;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

/// <summary>
/// Scene auto loader.
/// </summary>
/// <description>
/// This class adds a File > Scene Autoload menu containing options to select
/// a "master scene" enable it to be auto-loaded when the user presses play
/// in the editor. When enabled, the selected scene will be loaded on play,
/// then the original scene will be reloaded on stop.
///
/// Based on an idea on this thread:
/// http://forum.unity3d.com/threads/157502-Executing-first-scene-in-build-settings-when-pressing-play-button-in-editor
/// </description>
[InitializeOnLoad]
static class SceneAutoLoader
{
	// Static constructor binds a playmode-changed callback.
	// [InitializeOnLoad] above makes sure this gets executed.
	static SceneAutoLoader()
	{
		MasterScene = EditorBuildSettings.scenes[0].path;
		EditorApplication.playmodeStateChanged += OnPlayModeChanged;
	}

	// Play mode change callback handles the scene load/reload.
	private static void OnPlayModeChanged()
	{
		if (UserPressedPlay)
		{
			// User pressed play -- autoload master scene.
			PreviousScene = EditorSceneManager.GetActiveScene().path;

			if (SceneUtility.GetBuildIndexByScenePath(PreviousScene) < 0)
			{
				PGDebug.Message($"{PreviousScene} not in build, don't autoload").Log();
				PreviousScene = "";
				return;
			}
			PGDebug.Message($"{PreviousScene} In build, auto load scene {MasterScene}").Log();
			
			if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
			{
				EditorSceneManager.OpenScene(MasterScene, OpenSceneMode.Single);
			}
			else
			{
				// User cancelled the save operation -- cancel play as well.
				EditorApplication.isPlaying = false;
			}
		}
		
		if (UserPressedStop && !PreviousScene.IsNullOrEmpty())
		{
			// User pressed stop -- reload previous scene.
			if (PreviousScene != MasterScene)
			{
				EditorApplication.update += ReloadLastScene;
			}
		}
	}

	private static void ReloadLastScene()
	{
		PGDebug.Message($"Trying to reload last scene {PreviousScene}").Log();
		if (EditorSceneManager.GetActiveScene().path != PreviousScene)
		{
			try
			{
				EditorSceneManager.OpenScene(PreviousScene);
				EditorApplication.update -= ReloadLastScene;
			}
			catch (Exception)
			{
				PGDebug.Message($"Failed to reload last scene, retrying").Log();
				// ignored
			}
		}
	}

	private static bool UserPressedStop => EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode;

	private static bool UserPressedPlay => !EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode;

	// Properties are remembered as editor preferences.
	private const string cEditorPrefMasterScene = "SceneAutoLoader.MasterScene";
	private const string cEditorPrefPreviousScene = "SceneAutoLoader.PreviousScene";
	
	private static string MasterScene
	{
		get => EditorPrefs.GetString(cEditorPrefMasterScene, "Master.unity");
		set => EditorPrefs.SetString(cEditorPrefMasterScene, value);
	}
	
	private static string PreviousScene
	{
		get => EditorPrefs.GetString(cEditorPrefPreviousScene, EditorApplication.currentScene);
		set => EditorPrefs.SetString(cEditorPrefPreviousScene, value);
	}
}