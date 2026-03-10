using System.Collections.Generic;

namespace Quill.Commands
{
    public class FastCooldowns : ChatCommand
    {
        public FastCooldowns(): base("cooldowns", "Manage Cooldowns being on or off", FastCooldownsExecute, 1)
        {
        }
        private static void FastCooldownsExecute(string[] args, string playername)
        {
            var playerActor = BeetleUtils.GetActorByName(playername);
            if (playerActor.OwnerClientId != 0)
            {
                BeetleUtils.SendChatMessage("Giving Back abilities");
                string[] newArgs = new string[] { playerActor.ClassData.BeetleType.ToString() };

                ChangeBeetle.ChangeBeetleExecute(newArgs, playername);
                return;
            }
            string modifiedBeetle = BeetleUtils.GetBeetletype(playerActor);
            var beetleCache = Main.BeetleRegistry.GetBeetleCooldownCache();
            

            if (!beetleCache.ContainsKey(modifiedBeetle))
            {
                var ballCooldown = playerActor.Stats.AbilityStatsBall.ChargeDuration;
                var ballNormalCooldown = playerActor.Stats.AbilityStatsNormal.ChargeDuration;
                
                beetleCache.Add(modifiedBeetle, new BeetleData(ballCooldown, ballNormalCooldown));
            }
            
            if (args.Length != 1)
            {
                BeetleUtils.SendChatMessage("Usage: Cooldowns [on/off]");
            }
            
            switch (args[0])
            {
                case "on":
                    beetleCache[modifiedBeetle].ResetCooldowns(playerActor);
                    BeetleUtils.SendChatMessage("cooldowns turning on");
                    break;
                case "off":
                    playerActor.Stats.AbilityStatsBall.ChargeDuration = 0.1f;
                    playerActor.Stats.AbilityStatsNormal.ChargeDuration = 0.1f;
                    BeetleUtils.SendChatMessage("Cooldowns turning off");
                    break;
                default:
                    BeetleUtils.SendChatMessage("Usage: Cooldowns [on/off]");
                    break;
            }
        }
    }
}