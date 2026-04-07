using HarmonyLib;
using Il2Cpp;
using MelonLoader.TinyJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using static Quill.Main;

namespace Quill
{
    public class SaveStates : ChatCommand
    {
        static List<string> words = new List<string> { "dung", "fungusBeetle_mushroomlaunchpad", "RhinoBeetle_SmallSpike", "RhinoBeetle_BallSpike", "BombardierBeetle_SlipperyDecal" };
        public class SaveState
        {
            public List<PlayerData> players = new List<PlayerData>();
            public List<NetworkObjectData> NetworkObjects = new List<NetworkObjectData>();
            public List<DungData> Dungs = new List<DungData>();
        }
        [Serializable]
        public class NetworkObjectData
        {
            public string Name;
            public Vector3 Position;
            public Quaternion Rotation;
        }

        public static void Init()
        {
            HarmonyLib.Harmony harmony = new HarmonyLib.Harmony("com.quill.SaveStates");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }


        public SaveStates() : base("ss", "saves / loads savestates", SaveStatesExecute, 1)
        {
        }

        static SaveState currentState = new SaveState();
        static bool auto = false;
        private static void SaveStatesExecute(string[] args, string playername)
        {
            var AllNetWorkObject = UnityEngine.Object.FindObjectsOfType<NetworkManager>()[0].SpawnManager.SpawnedObjectsList;
            var AllNetWorkObjectSnapchot = new List<NetworkObject>();
            foreach (NetworkObject netObj in AllNetWorkObject)
            {
                if (!netObj.gameObject.name.Contains("BeetleActor") && !netObj.gameObject.name.Contains("DungBall"))
                    AllNetWorkObjectSnapchot.Add(netObj);
            }

            if (args.Length == 0)
            {
                BeetleUtils.SendChatMessage("Usage: !ss [save, load, auto]");
                return;
            }
            else

            {
                switch (args[0])
                {
                    case "save":
                        currentState.players.Clear();
                        currentState.Dungs.Clear();
                        currentState.NetworkObjects.Clear();
                        SaveCurrentState();
                        BeetleUtils.SendChatMessage("Saved current state!");
                        break;
                    case "load":
                        LoadCurrentState(AllNetWorkObjectSnapchot);

                        break;

                    case "auto":
                        BeetleUtils.SendChatMessage("Savestates " + (auto ? "wont " : "will") + "automatically load when you score");
                        auto = !auto;
                        break;
                    default:
                        BeetleUtils.SendChatMessage("Usage: SS [Save/Load]");
                        break;
                }
            }
        }
        public static void LoadCurrentState(List<NetworkObject> AllNetWorkObjectSnapchot)
        {
            ClearDung.ClearDungExecute(new string[0], "");
            var manager = UnityEngine.Object.FindObjectOfType<NetworkManager>();
            var prefabList = manager.NetworkConfig.Prefabs.m_Prefabs;
            var prefabSpawner = UnityEngine.Object.FindObjectOfType<NetworkPrefabSpawner>();

            foreach (var networkObject in AllNetWorkObjectSnapchot)
            {
                foreach (var prefab in prefabList)
                {
                    if (prefab.Prefab.name == networkObject.gameObject.name.Replace("(Clone)",""))
                    {
                        networkObject.Despawn();
                        break;
                    }
                }
            }
            foreach (var playerData in currentState.players)
            {
                BeetleUtils.Teleport(playerData);
            }
            foreach (var DunData in currentState.Dungs)
            {
                var rpcParams = new RpcParams(); // initializes new non null rpc params
                prefabSpawner.SpawnDungBall_ServerRpc(DunData.Position, Vector3.zero, DunData.Size, 0 ,rpcParams);
            }

            foreach (var data in currentState.NetworkObjects)
            {
                var prefab = new GameObject();
                string name = data.Name;

                if (name != null)
                {
                    foreach (var item in prefabList)

                    {
                        if (item.Prefab.name == name)
                        {
                            prefab = item.Prefab;
                            break;
                        }
                    }

                    GameObject obj = UnityEngine.Object.Instantiate(prefab, data.Position, data.Rotation);

                    var netObj = obj.GetComponent<NetworkObject>();
                    if (netObj != null)
                        netObj.Spawn();
                }
            }
        }
        public static void SaveCurrentState()
        {
            var AllNetWorkObject = UnityEngine.Object.FindObjectsOfType<NetworkManager>()[0].SpawnManager.SpawnedObjectsList;
            foreach (NetworkObject netObj in AllNetWorkObject)
            {
                if (!netObj.gameObject.name.Contains("BeetleActor") && !netObj.gameObject.name.Contains("DungBall"))
                {
                    currentState.NetworkObjects.Add(new NetworkObjectData
                    {
                        Name = netObj.name.Replace("(Clone)", ""),
                        Position = netObj.transform.position,
                        Rotation = netObj.transform.rotation
                    });
                }
            }
            foreach (var beetle in BeetleUtils.GetAllBeetles())
            {
                currentState.players.Add(new PlayerData
                {
                    NetworkID = beetle.OwnerClientId,
                    BeetleID = beetle.ClassData.BeetleType,
                    team = (TeamType)beetle._team.Value,
                    Position = beetle.transform.position,
                    Rotation = beetle.transform.rotation
                });
            }
            foreach (var dung in UnityEngine.Object.FindObjectsOfType<Il2Cpp.DungBall>())
            {
                currentState.Dungs.Add(new DungData
                {
                    Size = dung.Size,
                    Position = dung.transform.position
                });
            }
        }

        [HarmonyPatch(typeof(Goal), "BallEnteredGoal_ServerRpc")]
        class Goal_BallEnteredGoal_ServerRpc_Patch
        {
            static bool Prefix(ulong ballId)
            {
                if (!(Quill.Main._quillEnabled))
                    return true;

                if (auto)
                    SaveStatesExecute(new string[] { "load" }, "");

                return !auto;
            }
        }
    }
}