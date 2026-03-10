using System;
using Il2Cpp;
using Unity.Netcode;
using static Quill.Main;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using System.Collections.Generic;
namespace Quill
{
    public class SaveStates : ChatCommand
    {

        public class SaveState
        {
            public List<PlayerData> players = new List<PlayerData>();
            public List<DungData> dungs = new List<DungData>();
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
        private static void SaveStatesExecute(string[] args, string playername)
        {
            if (args.Length == 0)
            {
                BeetleUtils.SendChatMessage("Usage: !ss [save, load]");
                return;
            }
            else

            {
                switch (args[0])
                {
                    case "save":
                        currentState.players.Clear();
                        currentState.dungs.Clear();
                        foreach (var player in BeetleUtils.GetAllBeetles())
                        {
                            PlayerData playerData = new PlayerData
                            {
                                NetworkID = player.OwnerClientId,
                                BeetleID = player.ClassData.BeetleType,
                                Position = player.transform.position,
                                Rotation = player.transform.rotation,
                                team = player.Team
                            };

                            currentState.players.Add(playerData);
                        }

                        foreach(var dung in UnityEngine.Object.FindObjectsOfType<DungBall>())
                        {
                            DungData dungData = new DungData
                            {
                                Size = dung.Size,
                                Position = dung.transform.position
                            };

                            currentState.dungs.Add(dungData);
                        }
                        break;
                    case "load":
                        ClearDung.ClearDungExecute(new string[0], playername); //the name is not used in the function so it doesnt matter what i put here
                        foreach (var playerData in currentState.players)
                        {
                            BeetleUtils.Teleport(playerData);
                        }
                        foreach (var dungData in currentState.dungs)
                        {
                            var prefabSpawner =
                                UnityEngine.Object.FindObjectsOfType<NetworkPrefabSpawner>()[0];



                            var rpcParams = new RpcParams(); // initializes new non null rpc params
                            prefabSpawner.SpawnDungBall_ServerRpc(dungData.Position, Vector3.zero, dungData.Size, 0, rpcParams);
                        }

                        break;
                    default:
                        BeetleUtils.SendChatMessage("Usage: Score [on/off]");
                        break;
                }
            }

        }
    }
}