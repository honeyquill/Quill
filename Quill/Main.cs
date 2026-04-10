using Il2Cpp;
using MelonLoader;
using Quill.Commands;
using System.Linq;
using System.Net;
using UnityEngine;
using static MelonLoader.MelonLogger;
using static Quill.BeetleUtils;
using Main = Quill.Main;


[assembly: MelonInfo(typeof(Main), "Quill", "1.0", "Bee & Spike")]


namespace Quill
{
    public class Main : MelonMod
    {
        public class PlayerData
        {
            public ulong NetworkID;
            public BeetleType BeetleID;
            public TeamType team;
            public Vector3 Position;
            public Quaternion Rotation;
        }
        
        public class DungData
        {
            public int Size;
            public Vector3 Position;
        }
        public static BeetleRegistry BeetleRegistry { get; } = new BeetleRegistry();

        public static bool _quillEnabled = false;
        public static bool inCustom = false;


        public override void OnInitializeMelon()
        {
            var chatCommands = MelonMod.RegisteredMelons.OfType<ChatCommands.Main>().FirstOrDefault();

            if (chatCommands == null)
            {
                MelonLogger.Warning("ChatCommands could not be found.");
                return;
            }
            chatCommands.RegisterCommand("dung", new SpawnDung());
            chatCommands.RegisterCommand("bunny", new BunnyModify());
            chatCommands.RegisterCommand("cooldowns", new FastCooldowns());
            chatCommands.RegisterCommand("clear", new ClearDung());
            chatCommands.RegisterCommand("beetle", new ChangeBeetle());
            chatCommands.RegisterCommand("goal", new DisableScoring());
            chatCommands.RegisterCommand("ss", new SaveStates());

            BeetleRegistry.RegisterNameToIdCache();
        }
    }
} 