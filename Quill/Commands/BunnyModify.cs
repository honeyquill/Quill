using Il2Cpp;

namespace Quill.Commands
{
    public class BunnyModify : ChatCommand
    {
        public BunnyModify()
            : base("bunny", "starts, stops, or changes the speed | of the bunny", ExecuteBunnyModify, 1)
        {
        }


        static void DespawnBunny()
        {
            var bunnies = UnityEngine.Object.FindObjectsOfType<BunnyPathJumper>();
            foreach (var bunny in bunnies)
            {
                UnityEngine.Object.Destroy(bunny.gameObject);
            }
        }

        private static void ExecuteBunnyModify(string[] args, string player)
        {
            if (args.Length < 1)
            {
                BeetleUtils.SendChatMessage("Usage: bunny [start, stop, speed (measured in seconds between bunnys)] [0-100]");
                return;
            }
            
            var bunnySpawner = 
                UnityEngine.Object.FindObjectsOfType<BunnySpawner>()[0]; 
            
            switch (args[0].ToLower())
            {
                case "start":
                    bunnySpawner.spawnInterval = 15;
                    bunnySpawner.spawnIntervalSuddenDeath = 10;
                    BeetleUtils.SendChatMessage("Starting the Bunny Spawner...");
                    break;
                case "stop":
                    bunnySpawner.spawnInterval = 9999999;
                    bunnySpawner.spawnIntervalSuddenDeath = 9999999;
                    BeetleUtils.SendChatMessage("Stopping the Bunny Spawner...");
                    DespawnBunny();
                    break;
                case "speed":
                    if (args.Length != 2)
                    {
                        BeetleUtils.SendChatMessage("missing args: bunny speed [0-100]");
                        return;
                    }

                    if (!int.TryParse(args[1], out int value))
                    {
                        BeetleUtils.SendChatMessage("Argument must be a number between 1 and 100.");
                    }
                    
                    if (value <= 0 || value >= 100)
                    {
                        BeetleUtils.SendChatMessage("Argument must be a number between 1 and 100.");
                        return;
                    }

                    bunnySpawner.spawnInterval = value;
                    bunnySpawner.spawnIntervalSuddenDeath = value;
                    bunnySpawner.SpawnBunnyAndUpdateTime();
                    DespawnBunny();

                    BeetleUtils.SendChatMessage("Setting spawn interval to " + value);
                    break;
                default:
                    BeetleUtils.SendChatMessage("Usage: bunny [start, stop, speed] [1-100]");
                    break;
            }
        }
    }
}