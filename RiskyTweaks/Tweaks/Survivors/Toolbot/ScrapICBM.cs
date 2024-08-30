using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RiskyTweaks.Tweaks.Survivors.Toolbot
{
    public class ScrapICBM : TweakBase<ScrapICBM>
    {
        public override string ConfigCategoryString => "Survivors - MUL-T";

        public override string ConfigOptionName => "(Client-Side) Scrap Launcher - ICBM Synergy";

        public override string ConfigDescriptionString => "Scrap Launcher synergizes with ICBM.";

        public override bool StopLoadOnConfigDisable => true;

        protected override void ApplyChanges()
        {
            //Not the best place to hook.
            On.EntityStates.Toolbot.FireGrenadeLauncher.ModifyProjectileAimRay += FireGrenadeLauncher_ModifyProjectileAimRay;
        }

        private Ray FireGrenadeLauncher_ModifyProjectileAimRay(On.EntityStates.Toolbot.FireGrenadeLauncher.orig_ModifyProjectileAimRay orig, EntityStates.Toolbot.FireGrenadeLauncher self, Ray aimRay)
        {
            if (self.characterBody && self.characterBody.inventory)
            {
                int icbmCount = self.characterBody.inventory.GetItemCount(DLC1Content.Items.MoreMissile);
                int stackCount = icbmCount - 1;

                if (icbmCount > 0)
                {
                    float damageMult = 1f;
                    if (stackCount > 0) damageMult += 0.5f * stackCount;

                    self.damageCoefficient *= damageMult;


                    Vector3 rhs = Vector3.Cross(Vector3.up, aimRay.direction);
                    Vector3 axis = Vector3.Cross(aimRay.direction, rhs);

                    float currentSpread = 0f;
                    float angle = 0f;
                    float num2 = 0f;
                    num2 = UnityEngine.Random.Range(1f + currentSpread, 1f + currentSpread) * 3f;   //Bandit is x2
                    angle = num2 / 2f;  //3 - 1 rockets

                    Vector3 direction = Quaternion.AngleAxis(-num2 * 0.5f, axis) * aimRay.direction;
                    Quaternion rotation = Quaternion.AngleAxis(angle, axis);
                    Ray aimRay2 = new Ray(aimRay.origin, direction);
                    for (int i = 0; i < 3; i++)
                    {
                        if (i != 1) //Middle rocket is already fired by vanilla skill
                        {
                            RoR2.Projectile.ProjectileManager.instance.FireProjectile(self.projectilePrefab,
                                aimRay2.origin, Util.QuaternionSafeLookRotation(aimRay2.direction),
                                self.gameObject,
                                self.damageStat * self.damageCoefficient,
                                self.force,
                                self.RollCrit(),
                                DamageColorIndex.Default,
                                null,
                                -1f);
                        }
                        aimRay2.direction = rotation * aimRay2.direction;
                    }
                }
            }

            return orig(self, aimRay);
        }
    }
}
