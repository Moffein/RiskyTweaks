using RoR2;
using RoR2.CharacterAI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SocialPlatforms;

namespace RiskyTweaks.Tweaks.Survivors.Engi
{
    public class MobileTurretBuff : TweakBase<MobileTurretBuff>
    {
        public override string ConfigCategoryString => "Survivors - Engineer";

        public override string ConfigOptionName => "(Server-Side) Mobile Turret Buff";

        public override string ConfigDescriptionString => "Mobile Turrets always sprint and have a longer range.";

        protected override void ApplyChanges()
        {
            GameObject prefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Engi/EngiWalkerTurretBody.prefab").WaitForCompletion();
            CharacterBody cb = prefab.GetComponent<CharacterBody>();
            cb.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
            if (cb.baseRegen < 1f)
            {
                cb.baseRegen = 1f;
                cb.levelRegen = 0.2f;
            }

            SneedUtils.SetAddressableEntityStateField("RoR2/Base/Engi/EntityStates.EngiTurret.EngiTurretWeapon.FireBeam.asset", "maxDistance", "45");
            Component[] aiDrivers = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Engi/EngiWalkerTurretMaster.prefab")
                .WaitForCompletion().GetComponents<AISkillDriver>();
            foreach (AISkillDriver asd in aiDrivers)
            {
                if (asd.customName != "Rest")
                {
                    asd.shouldSprint = true;
                }
                if (asd.customName == "ChaseAndFireAtEnemy")
                {
                    asd.maxDistance = 45f;
                }
            }
        }
    }
}
