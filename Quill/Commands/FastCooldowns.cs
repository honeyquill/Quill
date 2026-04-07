using System.Collections.Generic;

namespace Quill.Commands
{
    public class FastCooldowns : ChatCommand
    {
        public static bool cooldownsEnabeled = true;
        public FastCooldowns(): base("cooldowns", "Manage Cooldowns being on or off", FastCooldownsExecute, 1, FastCooldownsUpdate)
        {
        }
        private static void FastCooldownsExecute(string[] args, string playername)
        {
            if (args.Length > 1)
            {
                BeetleUtils.SendChatMessage("Usage: Cooldowns [on/off]");
            }
            if(args.Length == 0)
            {
                cooldownsEnabeled = !cooldownsEnabeled;
                return;
            }
            switch (args[0])
            {
                case "on":
                    cooldownsEnabeled = true;
                    break;
                case "off":
                    cooldownsEnabeled = false;
                    BeetleUtils.SendChatMessage("Cooldowns turning off");
                    break;
                default:
                    BeetleUtils.SendChatMessage("Usage: Cooldowns [on/off]");
                    break;
            }
        }
        public static void FastCooldownsUpdate()
        {
            if (!cooldownsEnabeled)
            {
                var localBeetle = BeetleUtils.GetLocalBeetle();
                if (localBeetle == null) return;
                localBeetle._abilityChargingNormal.SetChargeLerp(1); //Lerp is the same as percentage
                localBeetle._abilityChargingBall.SetChargeLerp(1);
            }
        }
    }
}