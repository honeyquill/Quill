using Il2Cpp;
using Quill;
using Unity.Netcode;

namespace Quill
{
    public class SpawnDung : ChatCommand
    {
        public SpawnDung()
            : base("dung", "Creates a dung sized [1 - 5]", ExecuteSpawnDung, 1)
        {
        }
        private static void ExecuteSpawnDung(string[] args, string player)
        {
            if (args.Length < 1)
            {
                BeetleUtils.SendChatMessage("Usage: dung [1-5]");
                return;
            }

            if (!int.TryParse(args[0], out int value))
            {
                BeetleUtils.SendChatMessage("Argument must be a number between 1 and 5.");
                return;
            }

            if (value < 1 || value > 5)
            {
                BeetleUtils.SendChatMessage("Number must be between 1 and 5.");
                return;
            }
            var beetle = BeetleUtils.GetActorByName(player);
            BeetleUtils.SendChatMessage($"Spawning dung sized  {args[0]} for {BeetleUtils.GetPlayerName(beetle)}");
            var prefabSpawner = 
                UnityEngine.Object.FindObjectsOfType<NetworkPrefabSpawner>()[0];

            
            
            var rpcParams = new RpcParams(); // initializes new non null rpc params
            prefabSpawner.SpawnDungBall_ServerRpc(beetle.transform.position, beetle.Velocity, value, 0, rpcParams);
        }
    }
}