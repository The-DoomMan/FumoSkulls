using BepInEx;
using BepInEx.Logging;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

namespace FumoSkull
{
    [BepInPlugin("Tony.Fumoskulls", "Fumo Skulls", "1.2")]
    public class FumoSkulls : BaseUnityPlugin
    {
        public static Dictionary<string, GameObject> allFumos = new Dictionary<string, GameObject>();

        Harmony fumo;

        public static AssetBundle fumobundle;
        static Shader unlit;

        private void Awake()
        {
            unlit = Shader.Find("Unlit/Texture");
            fumobundle = AssetBundle.LoadFromMemory(Resource1.fumoskulls);
            fumobundle.LoadAllAssets();
            fumo = new Harmony("Tony.Fumoskulls");
            fumo.PatchAll();
            allFumos.Add("Cirno", fumobundle.LoadAsset<GameObject>("CirnoGO"));
            allFumos.Add("Reimu", fumobundle.LoadAsset<GameObject>("ReimuGO"));
            allFumos.Add("YuYu", fumobundle.LoadAsset<GameObject>("YuYuGO"));
            allFumos.Add("Koishi", fumobundle.LoadAsset<GameObject>("KoishiGO"));
            allFumos.Add("Sakuya", fumobundle.LoadAsset<GameObject>("SakuyaGO"));
        }

        [HarmonyPatch(typeof(Skull), "Start")]
        public static class fumofiyskull
        {
            public static void Prefix(Skull __instance)
            {
                Renderer masterSkull = __instance.gameObject.GetComponent<Renderer>();
                if (masterSkull)
                {
                    string fumoType;
                    Vector3 fumoposition = new Vector3(0.15f, 0, 0.15f);
                    Quaternion fumorotation = Quaternion.Euler(0, 20, 0);
                    Vector3 fumoscale = new Vector3(1.5f, 1.5f, 1.5f);
                    switch (__instance.GetComponent<ItemIdentifier>().itemType)
                    {
                        case ItemType.SkullBlue:
                            fumoType = "Cirno";
                            fumoposition = new Vector3(0.05f, 0, 0.2f);
                            break;

                        case ItemType.SkullRed:
                            fumoType = "Reimu";
                            break;

                        default:
                            fumoType = "Reimu";
                            return;
                    }
                    masterSkull.enabled = false;
                    CreateFumo(fumoType, masterSkull.transform, fumoposition, fumorotation, fumoscale);
                }
            }
        }

        [HarmonyPatch(typeof(Grenade), "Start")]
        public static class fumofiyrocket
        {
            public static void Postfix(Grenade __instance)
            {
                Renderer[] masterSkull = __instance.gameObject.GetComponentsInChildren<MeshRenderer>();
                if (masterSkull.Length > 0 && __instance.rocket)
                {
                    for (int i = 0; i < masterSkull.Length; i++)
                    {
                        masterSkull[i].enabled = false;
                    }
                    Vector3 fumoposition = new Vector3(0f, 0f, 2f);
                    Quaternion fumorotation = Quaternion.Euler(0, 0, 60);
                    Vector3 fumoscale = new Vector3(1f, 1f, 1f) * 10f;
                    CreateFumo("Sakuya", __instance.transform, fumoposition, fumorotation, fumoscale);
                }
            }
        }

        [HarmonyPatch(typeof(Ferryman), "Start")]
        public static class FerrymanHeadPatcher
        {
            public static void Prefix(Ferryman __instance)
            {

                GameObject Ferryhead = __instance.GetComponent<EnemyIdentifier>().weakPoint;
                if (Ferryhead)
                {
                    string fumoType;
                    Vector3 fumoposition = new Vector3(0, 0.0015f, 0);
                    Quaternion fumorotation = Quaternion.Euler(270, 0, 0);
                    Vector3 fumoscale = new Vector3(1, 1, 1) * 0.0075f;
                    fumoType = "Cirno";
                    CreateFumo(fumoType, Ferryhead.transform, fumoposition, fumorotation, fumoscale);
                }
            }
        }


        [HarmonyPatch(typeof(Torch), "Start")]
        public static class fumofiytorch
        {
            public static void Prefix(Torch __instance)
            {
                Renderer masterSkull = __instance.gameObject.GetComponentInChildren<MeshRenderer>();
                if (masterSkull)
                {
                    Vector3 fumoposition = new Vector3(0, 0.1f, 0);
                    Quaternion fumorotation = Quaternion.Euler(270, 270, 0);
                    Vector3 fumoscale = new Vector3(1, 1, 1) * 2.75f;
                    string fumoType = "YuYu";
                    masterSkull.enabled = false;
                    CreateFumo(fumoType, masterSkull.transform.parent.transform, fumoposition, fumorotation, fumoscale);
                }
            }
        }

        [HarmonyPatch(typeof(Soap), "Start")]
        public static class fumofiysoap
        {
            public static void Prefix(Soap __instance)
            {
                Renderer masterSkull = __instance.gameObject.GetComponentInChildren<MeshRenderer>();
                if (masterSkull)
                {
                    Vector3 fumoposition = new Vector3(0, 0.1f, 0);
                    Quaternion fumorotation = Quaternion.Euler(270, 270, 0);
                    Vector3 fumoscale = new Vector3(1, 1, 1) * 2.75f;
                    string fumoType = "Koishi";
                    masterSkull.enabled = false;
                    CreateFumo(fumoType, masterSkull.transform.parent.transform, fumoposition, fumorotation, fumoscale);
                }
            }
        }

        public static void CreateFumo(string fumoType, Transform masterSkull, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Debug.Log("Swapping " + masterSkull.name + " to " + fumoType);
            GameObject _fumo = allFumos[fumoType];
            GameObject SkullFumo = GameObject.Instantiate(_fumo, masterSkull);
            SkullFumo.active = true;
            SkullFumo.transform.localRotation = rotation;
            SkullFumo.transform.localPosition = position;
            SkullFumo.transform.localScale = scale;
            Renderer[] fumomatter = SkullFumo.GetComponentsInChildren<Renderer>();
            foreach (Renderer ren in fumomatter)
            {
                Material[] fumomaterial = ren.materials;
                foreach (Material mat in fumomaterial)
                {
                    mat.shader = unlit;
                }
            }
        }
    }
}
