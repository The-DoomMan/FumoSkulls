using BepInEx;
using Harmony;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FumoSkull
{
    [BepInPlugin("TonyFumos", "FumoSkull", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public Scene sScene;
        public static Dictionary<string, GameObject> allFumos = new Dictionary<string, GameObject>();
        bool fumofied;

        public static AssetBundle fumobundle;

        /*private void Start()
        {
            SceneManager.sceneLoaded += SceneManagerOnsceneLoaded;
        }*/

        private void Awake()
        {
            SceneManager.sceneLoaded += SceneManagerOnsceneLoaded;
            fumobundle = AssetBundle.LoadFromMemory(Resource1.fumoskull);
            fumobundle.LoadAllAssets();
            allFumos.Add("Crino", Plugin.fumobundle.LoadAsset<GameObject>("CrinoGO"));
            allFumos.Add("Reimu", Plugin.fumobundle.LoadAsset<GameObject>("ReimuGO"));
            allFumos.Add("YuYu", Plugin.fumobundle.LoadAsset<GameObject>("YuYuGO"));
        }

        private void SceneManagerOnsceneLoaded(Scene scene, LoadSceneMode mode)
        {
            sScene = scene;
        }

        [HarmonyPatch(typeof(Skull), "Start")]
        public static class fumofiy
        {
            public static void Prefix(Skull __instance)
            {
                Renderer masterSkull = __instance.gameObject.GetComponent<Renderer>();
                if (masterSkull)
                {
                    GameObject fumo;
                    string fumoType;
                    switch (__instance.GetComponent<ItemIdentifier>().itemType)
                    {
                        case ItemType.SkullBlue:
                            fumoType = "Crino";
                        break;

                        case ItemType.SkullRed:
                            fumoType = "Reimu";
                            break;

                        case ItemType.Torch:
                            fumoType = "YuYu";
                            break;

                        default:
                            return;
                     }
                    masterSkull.enabled = false;
                    GameObject fumo = allFumos[fumoType];
                    GameObject SkullFumo = GameObject.Instantiate(fumo, masterSkull.transform);
                    SkullFumo.transform.localRotation = Quaternion.Euler(90, 0, 0);
                }
            }
        }
    }
}
