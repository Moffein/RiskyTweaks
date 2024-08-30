using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;
using UnityEngine;

namespace RiskyTweaks.Tweaks.Interactables
{
    public class BloodShrineMinReward : TweakBase<BloodShrineMinReward>
    {
        public override string ConfigCategoryString => "Interactables";

        public override string ConfigOptionName => "(Server-Side) Blood Shrine Minimum Reward";

        public override string ConfigDescriptionString => "Blood Shrines always reward enough money for at least 1 chest.";

        public override bool StopLoadOnConfigDisable => true;

        protected override void ApplyChanges()
        {
            IL.RoR2.ShrineBloodBehavior.AddShrineStack += ShrineBloodBehavior_AddShrineStack;
        }

        private void ShrineBloodBehavior_AddShrineStack(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(
                x => x.MatchStloc(1)
                ))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<uint, ShrineBloodBehavior, uint>>((cost, self) =>
                {
                    if (!Stage.instance || !Run.instance) return cost;

                    int chestCost = Run.instance.GetDifficultyScaledCost(25, Stage.instance.entryDifficultyCoefficient);

                    float mult = (float)self.purchaseInteraction.cost / 50f;
                    int newCost = (int)(chestCost * mult);
                    return (uint)Mathf.Max((int)cost, newCost);
                });
            }
            else
            {
                UnityEngine.Debug.LogError("RiskyTweaks: BloodShrineMinReward IL Hook failed");
            }
        }
    }
}
