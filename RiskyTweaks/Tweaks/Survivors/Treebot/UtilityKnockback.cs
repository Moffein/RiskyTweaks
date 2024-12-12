using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RiskyTweaks.Tweaks.Survivors.Treebot
{
    public class UtilityKnockback : TweakBase<UtilityKnockback>
    {
        public override string ConfigCategoryString => "Survivors - REX";

        public override string ConfigOptionName => "(Server-Side) Utility Knockback Rework";

        public override string ConfigDescriptionString => "Reworks Utility knockback to be stronger and more consistent.";

        public override bool StopLoadOnConfigDisable => true;

        protected override void ApplyChanges()
        {
            IL.EntityStates.Treebot.Weapon.FireSonicBoom.OnEnter += FireSonicBoom_OnEnter;
        }

        private void FireSonicBoom_OnEnter(ILContext il)
        {
            bool error = true;
            ILCursor c = new ILCursor(il);

            //Make BullseyeSearch start slightly behind you
            if (c.TryGotoNext(
                 x => x.MatchCallvirt<BullseyeSearch>("RefreshCandidates")
                ))
            {
                c.EmitDelegate<Func<BullseyeSearch, BullseyeSearch>>(bullseye =>
                {
                    bullseye.searchOrigin -= bullseye.searchDirection;
                    bullseye.maxDistanceFilter += 1f;
                    return bullseye;
                });

                //Completely overwrite the vanilla force settings.
                if (c.TryGotoNext(
                     x => x.MatchCallvirt<HealthComponent>("TakeDamageForce")
                    ))
                {//Modify Force direction
                    c.Index -= 6;
                    c.Emit(OpCodes.Ldarg_0);   //FireSonicBoom
                    c.Emit(OpCodes.Ldloc, 4);   //Hurtbox
                    c.EmitDelegate<Func<Vector3, EntityStates.Treebot.Weapon.FireSonicBoom, HurtBox, Vector3>>((origForce, self, hurtBox) =>
                    {
                        Ray aimRay = self.GetAimRay();
                        Vector3 newForce = 2800f * aimRay.direction;
                        CharacterBody body = hurtBox.healthComponent.body;
                        if (body.characterMotor && body.characterMotor.isGrounded)
                        {
                            newForce.y = Mathf.Max(newForce.y, 1200f);
                        }

                        if (body.isChampion && body.characterMotor && body.characterMotor.isGrounded)
                        {
                            newForce.y /= 0.7f;
                        }

                        return newForce;
                    });

                    //Modify Force multiplier
                    c.Index += 3;
                    c.Emit(OpCodes.Ldloc, 4);   //Hurtbox
                    c.EmitDelegate<Func<float, HurtBox, float>>((origForceMult, hurtBox) =>
                    {
                        float mass = 1f;
                        CharacterBody body = hurtBox.healthComponent.body;
                        if (body.characterMotor)
                        {
                            mass = body.characterMotor.mass;
                        }
                        else if (body.rigidbody)
                        {
                            mass = body.rigidbody.mass;
                        }

                        float forceMult = Mathf.Max(mass / 100f, 1f);

                        if (body.isChampion && body.characterMotor && body.characterMotor.isGrounded)
                        {
                            forceMult *= 0.7f;
                        }

                        return forceMult;
                    });
                    error = false;
                }
            }

            if (error)
            {
                UnityEngine.Debug.LogError("RiskyTweaks: Treebot UtilityKnockback IL Hook failed");
            }
        }
    }
}
