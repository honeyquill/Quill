using System;
using Il2Cpp;
using MelonLoader;
using Unity.Netcode;
using UnityEngine;
using static Quill.Main;

namespace Quill
{
    public class ChangeBeetle : ChatCommand
    {
        public ChangeBeetle() : base("beetle", "changes your beetle to the decided beetle", ChangeBeetleExecute, 1,null)
        {
        }

        public static void ChangeBeetleExecute(string[] args, string playername)
        {
            if (args.Length != 1)
            {
                BeetleUtils.SendChatMessage("usage: beetle [beetle name] (common aliases/ID works)");
                return;
            }
            
            var desiredBeetle = args[0].ToLower();
            
            Main.BeetleRegistry.TryGetId(desiredBeetle, out var id);
            if (id == -1)
            {
                BeetleUtils.SendChatMessage("unknown beetle " + id);
                return;
            }
            
            var PlayerActor = BeetleUtils.GetActorByName(playername);
            var PlayerId = PlayerActor.OwnerClientId;
            var PlayerTeam = PlayerActor.Team;

            var prefabSpawner = UnityEngine.Object.FindObjectsOfType<Il2Cpp.NetworkPrefabSpawner>()[0]; //This code only works on true Blue For SOME reason so i just set the team of the beetle after spawning it, this is really jank but it works so im not gonna complain
                if (GameState.CurrentState != GameState.State.Lobby_Custom)
                {
                    var MapInitializer = UnityEngine.Object.FindObjectsOfType<Il2Cpp.MapInitializer>()[0];

                    Vector3 OriginalSpawnPos = MapInitializer.SpawnPositions[0].spawnTransform.position;
                    Quaternion OriginalSpawnRotaion = MapInitializer.SpawnPositions[0].spawnTransform.rotation;

                    MapInitializer.SpawnPositions[0].spawnTransform.position = PlayerActor.transform.position;
                    MapInitializer.SpawnPositions[0].spawnTransform.rotation = PlayerActor.transform.rotation;

                    prefabSpawner.SpawnClassAndSetTeam(PlayerId, TeamType.Blue, id);
                    foreach( var beetle in BeetleUtils.GetAllBeetles())
                    {
                        if (beetle.OwnerClientId == PlayerId)
                        {
                            beetle._team.Value = (int)PlayerTeam;
                        }
                    }

                    MapInitializer.SpawnPositions[0].spawnTransform.position = OriginalSpawnPos;
                    MapInitializer.SpawnPositions[0].spawnTransform.rotation = OriginalSpawnRotaion;
                }

                else
                    BeetleUtils.SendChatMessage("You cannot change your beetle in the lobby!");
        }
    }
}