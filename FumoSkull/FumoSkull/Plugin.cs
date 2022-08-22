using BepInEx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FumoSkull
{
    [BepInPlugin("TonyFumos", "FumoSkull", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public Scene sScene;
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
        }

        private void SceneManagerOnsceneLoaded(Scene scene, LoadSceneMode mode)
        {
            sScene = scene;
        }

        private void Update()
        {
            GameObject[] rootGameObjects = sScene.GetRootGameObjects();
            for (int i = 0; i < rootGameObjects.Length; i++)
                {
                    foreach (Renderer renderer in rootGameObjects[i].GetComponentsInChildren<Renderer>(true))
                    {
                        if (renderer.gameObject.layer == 22 && renderer.gameObject.GetComponent<Skull>() && !renderer.GetComponent<fumofiy>())
                        {
                                  renderer.gameObject.AddComponent<fumofiy>();
                                  renderer.gameObject.GetComponent<fumofiy>().enabled = true;
                                  renderer.gameObject.GetComponent<fumofiy>().masterSkull = renderer.gameObject;
                                  renderer.gameObject.GetComponent<fumofiy>().DoTheFumo();
                        }
                    }
                }
        }

        public class fumofiy : MonoBehaviour
        {
            public void DoTheFumo()
            {
                if(masterSkull)
                {
                    switch (masterSkull.GetComponent<ItemIdentifier>().itemType)
                    {
                        case ItemType.SkullBlue:
                            fumoType = "Crino";
                            fumo = Plugin.fumobundle.LoadAsset<GameObject>("CrinoGO");
                        break;

                        case ItemType.SkullRed:
                            fumoType = "Reimu";
                            fumo = Plugin.fumobundle.LoadAsset<GameObject>("ReimuGO");
                            break;

                        case ItemType.Torch:
                            fumoType = "YuYu";
                            fumo = Plugin.fumobundle.LoadAsset<GameObject>("YuYuGO");
                            break;

                        default:
                            fumoType = "";
                            break;
                    }
                    if(fumoType != "")
                    {
                        masterSkull.GetComponent<Renderer>().enabled = false;
                        GameObject SkullFumo = GameObject.Instantiate(fumo, masterSkull.transform);
                        SkullFumo.transform.localRotation = Quaternion.Euler(90, 0, 0);
                    }
                }
            }

            public GameObject masterSkull;
            GameObject fumo;
            string fumoType;
        }
    }
}
