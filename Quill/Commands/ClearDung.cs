using System;

namespace Quill
{
    public class ClearDung : ChatCommand
    {
        public ClearDung() : base("clear", "destroys all dung", ClearDungExecute, 0)
        {
        }

        private static void ClearDungExecute(string[] args)
        {
            var dung = UnityEngine.Object.FindObjectsOfType<Il2Cpp.DungBall>();
            
            BeetleUtils.SendChatMessage("Clearing dung...");
            foreach (var i in dung)
            { 
                UnityEngine.Object.Destroy(i.gameObject);
            }
        }
    }
}