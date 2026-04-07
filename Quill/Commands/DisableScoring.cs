using System;
using Il2Cpp;
using Unity.Netcode;
using static Quill.Main;
using HarmonyLib;
using System.Reflection;
namespace Quill
{
    public class DisableScoring : ChatCommand
    {

        public static void Init()
        {
            HarmonyLib.Harmony harmony = new HarmonyLib.Harmony("com.quill.DisableGoals");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        static bool AllowScoring = true;
        public DisableScoring() : base("goal", "Disables / Enables goals", ChangeGoalAvaliability, 0)
        {
        }

        private static void ChangeGoalAvaliability(string[] args, string playername)
        {
            if (args.Length == 0)
            {
                AllowScoring = !AllowScoring;
                if (AllowScoring) BeetleUtils.SendChatMessage("Scoring is now Enabled"); else BeetleUtils.SendChatMessage("Scoring is now Disabled");
            }
            else
            {
                switch (args[0])
                {
                    case string s when s == "on" || s == "1":
                        AllowScoring = true;
                        BeetleUtils.SendChatMessage("Scoring is now Enabled");
                        break;
                    case string s when s == "off" || s == "0":
                        AllowScoring = false;
                        BeetleUtils.SendChatMessage("Scoring is now Disabled");
                        break;
                    default:
                        BeetleUtils.SendChatMessage("Usage: Score [on/off]");
                        break;
                }
            }

        }
        [HarmonyPatch(typeof(Goal), "BallEnteredGoal_ServerRpc")]
        class Goal_BallEnteredGoal_ServerRpc_Patch
        {
            static bool Prefix(ulong ballId)
            {
                if (Quill.Main._quillEnabled)
                    return AllowScoring;
                else
                    return true;
            }
        }
    }
}