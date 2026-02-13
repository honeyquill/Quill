using System;
using Il2Cpp;
using Unity.Netcode;
using static Quill.Main;

namespace Quill
{
    public class ChangeBeetle : ChatCommand
    {
        public ChangeBeetle() : base("beetle", "changes your beetle to the decided beetle", ChangeBeetleExecute, 1)
        {
        }

        private static void ChangeBeetleExecute(string[] args)
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
            
            var prefabSpawner = UnityEngine.Object.FindObjectsOfType<Il2Cpp.NetworkPrefabSpawner>()[0]; 
            prefabSpawner.SpawnClass_ServerRpc(id, new SpawnPositionData(), new SetTeamData(), new RpcParams());
        }
    }
}