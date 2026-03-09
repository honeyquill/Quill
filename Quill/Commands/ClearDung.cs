using System;

namespace Quill
{
    public class ClearDung : ChatCommand
    {
        public ClearDung() : base("clear", "destroys all dung", ClearDungExecute, 0)
        {
        }

        private static void ClearDungExecute(string[] args, string player)
        {
            var dung = UnityEngine.Object.FindObjectsOfType<Il2Cpp.DungBall>();
            var bunnies = UnityEngine.Object.FindObjectsOfType<Il2Cpp.BunnyPathJumper>();

            BeetleUtils.SendChatMessage("Clearing dung...");
            foreach (var i in dung)
            {
                UnityEngine.Object.Destroy(i.gameObject);
            }
            foreach (var i in bunnies)
            {
                UnityEngine.Object.Destroy(i.gameObject);
            }
        }
    }
}