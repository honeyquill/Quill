using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Il2Cpp;
using UnityEngine.InputSystem;

namespace Quill
{
    public static class BeetleUtils
    {
        public static string GetMapName()
        {
            try
            {
                var mapInitializer = UnityEngine.Object.FindObjectsOfType<Il2Cpp.MapInitializer>()[0];
                return mapInitializer.mapData.ToString().Replace("MapData_", "").Replace(" (MapDataSO)", "");
            }
            catch
            {
                return null;
            }

        }
        
        public static bool Pressed(Key key)
        {
            return Keyboard.current[key].wasPressedThisFrame;
        }
        
        public static string ModFolder()
        {
            var gameData = UnityEngine.Application.dataPath;
            // parent folder = game install folder
            var gameRoot = Directory.GetParent(gameData)?.FullName;
            if (gameRoot == null) return null;
            var modsFolder = Path.Combine(gameRoot, "Mods");
            return modsFolder;
        }
        
        public static BeetleActor[] GetAllBeetles()
        {
            return UnityEngine.Object.FindObjectsOfType<Il2Cpp.BeetleActor>();
        }
        
        public static BeetleActor GetLocalBeetle()
        {
            BeetleActor[] allBeetles = GetAllBeetles();
            if (allBeetles.Length == 0) { return null; }
            foreach (var beetle in allBeetles)
            {
                if (beetle.IsLocalPlayer) return beetle;
            }
            return null;
        }
        
        public static bool IsHost()
        {
            if (GetLocalBeetle() == null) return false;
            return GetLocalBeetle().IsHost;
        }
        
        public static void ApplyModifer(ModifierType modifier, DungBall dungBall, float duration)
        {
            dungBall.ModifiersController.AddModifierRpcDispatcher(modifier, duration);
        }

        public static List<(string player, string message)> GetChatHistory()
        {
            var chatlog = UnityEngine.Object.FindObjectOfType<Il2Cpp.ChatLog>();
            if (chatlog == null) return null;
            var input = chatlog.text.text;

            var result = new List<(string player, string message)>();
            var matches = Regex.Matches(input, @"<b><color=#.*?>(.*?)<\/color><\/b>:\s*(.*)");

            foreach (Match match in matches)
            {
                string player = match.Groups[1].Value;
                string message = match.Groups[2].Value;
                result.Add((player, message));
            }
            return result;
        }
        
        public static void Score(Il2Cpp.TeamType team)
        {
            BeetleActor[] allBeetles = BeetleUtils.GetAllBeetles();
            Goal[] goals = UnityEngine.Object.FindObjectsOfType<Goal>();

            foreach (var goal in goals)
            {
                if (goal == null) continue;
                if (goal.OwnerTeam != team)
                {
                    goal.BallEnteredGoal_ServerRpc(0);
                }
            }
        }
        public static string GetPlayerName(BeetleActor beetle)
        {
            if (beetle == null) return "Unknown";

            try
            {
                var nametags = PlayerNametagsController.Instance;
                if (nametags != null)
                {
                    NametagUI nametagText;
                    if (nametags._activeNametags.TryGetValue(beetle, out nametagText) && nametagText != null)
                    {
                        string txt = nametagText.name;
                        if (!string.IsNullOrEmpty(txt)) return txt;
                    }
                }
            }
            catch { return "player_" + beetle.NetworkBehaviourId; }


            return "Unknown";
        }
        public static void SendChatMessage(string message)
        {
            var chatPanel = UnityEngine.Object.FindObjectOfType<ChatPanel>();
            if (chatPanel != null)
            {
                chatPanel.SendChatMessage(message);
            }
        }
    }
}
