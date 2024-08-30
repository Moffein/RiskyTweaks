using UnityEngine;
using RoR2;
using RoR2.CharacterAI;
using UnityEngine.AddressableAssets;

namespace RiskyTweaks.Tweaks.Enemies.Beetle
{
    public class BeetleBuff : TweakBase<BeetleBuff>
    {
        public override string ConfigCategoryString => "Enemies - Beetle";

        public override string ConfigOptionName => "(Server-Side) Buff Enemy";

        public override string ConfigDescriptionString => "Buffs hitbox size, slightly buffs attack speed and move speed.";

        public override bool StopLoadOnConfigDisable => true;

        protected override void ApplyChanges()
        {
            GameObject beetleObject = LegacyResourcesAPI.Load<GameObject>("prefabs/characterbodies/beetlebody");
            ModifyStats(beetleObject);
            ModifyAttack();
            ExpandHitbox(beetleObject);
        }

        private void ModifyStats(GameObject go)
        {
            CharacterBody cb = go.GetComponent<CharacterBody>();

            //cb.baseMaxHealth = 96f;
            //cb.levelMaxHealth = cb.baseMaxHealth * 0.3f;
            float newSpeed = 8f;   //Vanilla 6
            if (cb.baseMoveSpeed < newSpeed)    //Check in case SpeedyBeetles is installed
            {
                cb.baseMoveSpeed = newSpeed;
            }
        }

        private void ModifyAttack()
        {
            SneedUtils.SetAddressableEntityStateField("RoR2/Base/Beetle/EntityStates.BeetleMonster.HeadbuttState.asset", "baseDuration", "1.2");   //Vanilla 1.5

            //Increase AI Headbutt range
            GameObject masterObject = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Beetle/BeetleMaster.prefab").WaitForCompletion();

            float dist = 5f;
            AISkillDriver[] skills = masterObject.GetComponents<AISkillDriver>();
            foreach (AISkillDriver ai in skills)
            {
                if (ai.skillSlot == SkillSlot.Primary)
                {
                    if (ai.maxDistance < dist) ai.maxDistance = dist;
                }
            }
        }

        private void ExpandHitbox(GameObject enemyObject)
        {
            CharacterBody cb = enemyObject.GetComponent<CharacterBody>();
            HitBoxGroup hbg = cb.GetComponentInChildren<HitBoxGroup>();
            if (hbg.groupName == "Headbutt")
            {
                Transform hitboxTransform = hbg.hitBoxes[0].transform;
                //Debug.Log("Beetle Hitbox: " + hitboxTransform.localScale);    //(1.0, 1.0, 1.7)
                hitboxTransform.localScale = new Vector3(2.5f, 4f, 1.7f);

                //Debug.Log("Beetle Hitbox Pos: " + hitboxTransform.localPosition);   //(0.0, 0.3, 0.2)
                hitboxTransform.localPosition = new Vector3(0f, 0.3f, 0.2f);    //y is forward
            }
        }
    }
}
