using JLChnToZ.VRC.VVMW;
using UdonVR.RecordPlayer;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using VRC.SDKBase.Editor.BuildPipeline;

namespace com.udonvr.recordplayer.vizvidEditor
{
    public class RecordPlayerAutoSetup : IVRCSDKBuildRequestedCallback
    {
        public int callbackOrder => -9999;

        public bool OnBuildRequested(VRCSDKRequestedBuildType requestedBuildType)
        {
            return true;
        }

        [PostProcessScene(-9999)]
        private static void ProcessScene()
        {
            RecordPlayer recordPlayer = GameObject.FindObjectOfType<RecordPlayer>();
            Core _vizVid = GameObject.FindObjectOfType<Core>();
            if (recordPlayer == null)
            {
                Debug.LogWarning("[UdonVR][VizVid AutoSetup] Record Player was not found");
                return;
            }

            if (_vizVid == null)
            {
                Debug.LogError("[UdonVR][VizVid AutoSetup] VizVid Core was not found");
                return;
            }

            if (recordPlayer.videoplayerModule != null && recordPlayer.videoplayerModule.GetType() != typeof(RecordPlayer_VideoPlayer_VizVid))
            {
                if (!EditorUtility.DisplayDialog(
                    "Existing Module found",
                    "There was a Module found attached to the record player, but it wasn't VizVid. Do you want to continue VizVid Auto Setup?",
                    "Yes", "No"))
                {
                    Debug.LogError($"[UdonVR][VizVid AutoSetup] Module found [{recordPlayer.videoplayerModule.name}] but is not for VizVid, Canceling AutoSetup");
                    return;
                }
            }

            RecordPlayer_VideoplayerClass module = GameObject.FindObjectOfType<RecordPlayer_VideoPlayer_VizVid>();
            if (module == null)
            {
                var loadedObject = AssetDatabase.LoadAssetAtPath("Packages/com.udonvr.recordplayer.vizvid/RecordPlayerModule-VizVid.prefab", typeof(UnityEngine.Object));
                if (loadedObject == null)
                {
                    string[] guids = AssetDatabase.FindAssets("RecordPlayerVideoModule - VizVid t:Prefab");
                    if (guids.Length > 0)
                    {
                        loadedObject = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guids[0]), typeof(UnityEngine.Object));
                    }
                }

                if (loadedObject == null)
                {
                    Debug.LogError("[UdonVR][VizVid AutoSetup] No Videoplayer module prefab found in Packages or Assets");
                    return;
                }

                if (!((GameObject)PrefabUtility.InstantiatePrefab(loadedObject)).transform.TryGetComponent(out module))
                {
                    Debug.LogError("[UdonVR][VizVid AutoSetup] No Videoplayer module found, failed to instantiate");
                    return;
                }
                else
                {
                    Debug.Log($"[UdonVR][VizVid AutoSetup] Instantiate VizVid videoplayer module: [{module.name}]");
                    recordPlayer.videoplayerModule = module;
                    if (module.GetType() != typeof(RecordPlayer_VideoPlayer_VizVid))
                    {
                        Debug.LogError("[UdonVR][VizVid AutoSetup] Created module found but is not for VizVid, Canceling AutoSetup");
                        return;
                    }
                }
            }
            else if (recordPlayer.videoplayerModule == null)
            {
                recordPlayer.videoplayerModule = module;
            }

            if (module.recordPlayer == null)
            {
                module.recordPlayer = recordPlayer;
                Debug.Log($"[UdonVR][VizVid AutoSetup] Module's recordPlayer Reference was null, binding to [{recordPlayer.name}]");
            }
            else
            {
                Debug.Log($"[UdonVR][VizVid AutoSetup] Module's recordPlayer Reference already filled in, skipping");
            }

            RecordPlayer_VideoPlayer_VizVid vizVidModule = (RecordPlayer_VideoPlayer_VizVid)module;
            if (vizVidModule.vizVid == null)
            {
                vizVidModule.vizVid = _vizVid;
                Debug.Log($"[UdonVR][VizVid AutoSetup] Module's vizVid Reference was null, binding to [{_vizVid.name}]");
            }
            else
            {
                Debug.Log($"[UdonVR][VizVid AutoSetup] Module's vizVid Reference already filled in, skipping");
            }

            EditorUtility.SetDirty(recordPlayer);
            EditorUtility.SetDirty(vizVidModule);
        }
    }
}