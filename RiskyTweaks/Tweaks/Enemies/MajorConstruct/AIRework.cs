using UnityEngine.AddressableAssets;
using RoR2.CharacterAI;
using RoR2;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace RiskyTweaks.Tweaks.Enemies.MajorConstruct
{
    public class AIRework : TweakBase<AIRework>
    {
        public override string ConfigCategoryString => "Enemies - Xi Construct";

        public override string ConfigOptionName => "(Server-Side) AI Rework";

        public override string ConfigDescriptionString => "Makes the boss stay closer to players when fighting.";


        //Directly taken from https://github.com/OakPrime/XiConstructFix
        protected override void ApplyChanges()
        {
            AISkillDriver skillDriverFlee = ((IEnumerable<AISkillDriver>)Addressables.LoadAssetAsync<GameObject>((object)"RoR2/DLC1/MajorAndMinorConstruct/MegaConstructMaster.prefab").WaitForCompletion().GetComponents<AISkillDriver>()).Where<AISkillDriver>((Func<AISkillDriver, bool>)(x => x.customName == "FleeStep")).First<AISkillDriver>();
            AISkillDriver skillDriverFollow = ((IEnumerable<AISkillDriver>)Addressables.LoadAssetAsync<GameObject>((object)"RoR2/DLC1/MajorAndMinorConstruct/MegaConstructMaster.prefab").WaitForCompletion().GetComponents<AISkillDriver>()).Where<AISkillDriver>((Func<AISkillDriver, bool>)(x => x.customName == "FollowStep")).First<AISkillDriver>();
            AISkillDriver skillDriverFollowFast = ((IEnumerable<AISkillDriver>)Addressables.LoadAssetAsync<GameObject>((object)"RoR2/DLC1/MajorAndMinorConstruct/MegaConstructMaster.prefab").WaitForCompletion().GetComponents<AISkillDriver>()).Where<AISkillDriver>((Func<AISkillDriver, bool>)(x => x.customName == "FollowFast")).First<AISkillDriver>();
            AISkillDriver skillDriverShoot = ((IEnumerable<AISkillDriver>)Addressables.LoadAssetAsync<GameObject>((object)"RoR2/DLC1/MajorAndMinorConstruct/MegaConstructMaster.prefab").WaitForCompletion().GetComponents<AISkillDriver>()).Where<AISkillDriver>((Func<AISkillDriver, bool>)(x => x.customName == "ShootStep")).First<AISkillDriver>();

            skillDriverFollow.minDistance = 30.0f;
            skillDriverFollowFast.minDistance = 75.0f;
        }
    }
}
