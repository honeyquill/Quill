using System.Collections.Generic;

namespace Quill.Commands
{
    public class FastCooldowns : ChatCommand
    {
        public FastCooldowns(): base("cooldowns", "Manage Cooldowns being on or off", FastCooldownsExecute, 1)
        {
        }
        private static void FastCooldownsExecute(string[] args, string player)
        {
            var localBeetle = BeetleUtils.GetLocalBeetle();
            var modifiedBeetle = localBeetle.name
                .Replace("(Clone)", "")
                .Replace("BeetleActor_", "").Trim();
            var beetleCache = Main.BeetleRegistry.GetBeetleCooldownCache();
            

            if (!beetleCache.ContainsKey(modifiedBeetle))
            {
                var ballCooldown = localBeetle.Stats.AbilityStatsBall.ChargeDuration;
                var ballNormalCooldown = localBeetle.Stats.AbilityStatsNormal.ChargeDuration;
                
                beetleCache.Add(modifiedBeetle, new BeetleData(ballCooldown, ballNormalCooldown));
            }
            
            if (args.Length != 1)
            {
                BeetleUtils.SendChatMessage("Usage: Cooldowns [on/off]");
            }
            
            switch (args[0])
            {
                case "on":
                    beetleCache[modifiedBeetle].ResetCooldowns(localBeetle);
                    BeetleUtils.SendChatMessage("cooldowns turning on");
                    break;
                case "off":
                    localBeetle.Stats.AbilityStatsBall.ChargeDuration = 0.1f;
                    localBeetle.Stats.AbilityStatsNormal.ChargeDuration = 0.1f;
                    BeetleUtils.SendChatMessage("Cooldowns turning off");
                    break;
                default:
                    BeetleUtils.SendChatMessage("Usage: Cooldowns [on/off]");
                    break;
            }
        }
    }
}