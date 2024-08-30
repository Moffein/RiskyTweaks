using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.AddressableAssets;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

namespace RiskyTweaks.Tweaks.Enemies.Brother
{
    public class Phase4GuaranteedItem : TweakBase<Phase4GuaranteedItem>
    {
        public override string ConfigCategoryString => "Enemies - Mithrix";

        public override string ConfigOptionName => "(Server-Side) Phase 4 Guaranteed Item Return";

        public override string ConfigDescriptionString => "Hitting Mithrix is guaranteed to return an item, on a timed cooldown.";

        public override bool StopLoadOnConfigDisable => true;

        private BodyIndex brotherHurtIndex;

        protected override void ApplyChanges()
        {
            RoR2Application.onLoad += OnLoad;
            GameObject brotherHurtPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Brother/BrotherHurtBody.prefab").WaitForCompletion();
            brotherHurtPrefab.AddComponent<GuaranteedItemReturnComponent>();

            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        private void OnLoad()
        {
            brotherHurtIndex = BodyCatalog.FindBodyIndex("BrotherHurtBody");
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            orig(self, damageInfo);

            if (!NetworkServer.active || damageInfo.rejected || !damageInfo.attacker || self.body.bodyIndex != brotherHurtIndex) return;

            GuaranteedItemReturnComponent g = self.GetComponent<GuaranteedItemReturnComponent>();
            if (!g || !g.ShouldGuaranteedSteal(damageInfo.attacker)) return;

            CharacterBody attackerBody = damageInfo.attacker.GetComponent<CharacterBody>();
            if (!attackerBody || !attackerBody.inventory) return;

            ReturnStolenItemsOnGettingHit rsi = self.GetComponent<ReturnStolenItemsOnGettingHit>();
            if (rsi && rsi.itemStealController && rsi.itemStealController.ReclaimItemForInventory(attackerBody.inventory))
            {
                Debug.Log("Phase4Tweaks: Guaranteed item return for " + attackerBody);
                g.SetStealItemCooldown(damageInfo.attacker);

            }
        }
    }

    public class GuaranteedItemReturnComponent : MonoBehaviour
    {
        public static float cooldownDuration = 30f;
        public static float closeRangeDistanceSqr = 30f * 30f;
        public static float closeRangeCooldownMult = 3f;
        private List<AttackerCooldown> cooldowns = new List<AttackerCooldown>();

        private void FixedUpdate()
        {
            if (!NetworkServer.active) return;
            float closeRangeTime = Time.fixedDeltaTime * closeRangeCooldownMult;
            foreach (AttackerCooldown ac in cooldowns)
            {
                bool close = false;
                if (ac.attacker)
                {
                    float distSqr = (ac.attacker.transform.position - base.transform.position).sqrMagnitude;
                    if (distSqr <= GuaranteedItemReturnComponent.closeRangeDistanceSqr) close = true;
                }
                ac.ReduceCooldown(close ? closeRangeTime : Time.fixedDeltaTime);
            }

            //Remove dead attackers
            cooldowns = cooldowns.Where(ac => ac.attacker != null).ToList();
        }

        public void SetStealItemCooldown(GameObject attacker)
        {
            if (!attacker) return;
            AttackerCooldown ac = cooldowns.Find(c => c.attacker == attacker);
            if (ac != null)
            {
                ac.SetCooldown(GuaranteedItemReturnComponent.cooldownDuration);
            }
            else
            {
                ac = new AttackerCooldown(attacker, GuaranteedItemReturnComponent.cooldownDuration);
                cooldowns.Add(ac);
            }
        }

        public bool ShouldGuaranteedSteal(GameObject attacker)
        {
            if (!attacker) return false;
            AttackerCooldown ac = cooldowns.Find(c => c.attacker == attacker);
            if (ac == null)
            {
                ac = new AttackerCooldown(attacker, 0f);
                cooldowns.Add(ac);
            }

            return ac.CanSteal();
        }

        private class AttackerCooldown
        {
            public GameObject attacker;
            float cooldown;

            public AttackerCooldown(GameObject attacker, float cooldown)
            {
                this.attacker = attacker;
                this.cooldown = cooldown;
            }

            public void SetCooldown(float duration)
            {
                cooldown = duration;
            }

            public void ReduceCooldown(float duration)
            {
                if (cooldown > 0f)
                {
                    cooldown -= duration;
                }
            }

            public bool CanSteal()
            {
                return cooldown <= 0f;
            }
        }
    }
}
