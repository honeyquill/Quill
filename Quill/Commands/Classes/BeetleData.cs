using Il2Cpp;

namespace Quill
{
    public class BeetleData
    {
        public float BallCooldown;
        public float NormalCooldown;
        
        public BeetleData(float  ballCooldown, float normalCooldown)
        {
            this.BallCooldown = ballCooldown;
            this.NormalCooldown = normalCooldown;
        }
        
        public void ResetCooldowns(BeetleActor beetle)
        {
            beetle.Stats.AbilityStatsBall.ChargeDuration = BallCooldown;
            beetle.Stats.AbilityStatsNormal.ChargeDuration = NormalCooldown;

            beetle._abilityChargingBall.CurrentChargeTime = BallCooldown;
            beetle._abilityChargingNormal.CurrentChargeTime = NormalCooldown;
        }
    }
}