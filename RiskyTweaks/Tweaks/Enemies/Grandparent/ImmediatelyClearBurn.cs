using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;

namespace RiskyTweaks.Tweaks.Enemies.Grandparent
{
    public class ImmediatelyClearBurn : TweakBase<ImmediatelyClearBurn>
    {
        public override string ConfigCategoryString => "Enemies - Grandparent";

        public override string ConfigOptionName => "(Server-Side) Immediately Clear Sun Afterburn";

        public override string ConfigDescriptionString => "Sun afterburn is immediately cleared when breaking LoS.";

        protected override void ApplyChanges()
        {
            On.RoR2.CharacterBody.UpdateBuffs += CharacterBody_UpdateBuffs;
        }

        private void CharacterBody_UpdateBuffs(On.RoR2.CharacterBody.orig_UpdateBuffs orig, CharacterBody self, float deltaTime)
        {
            bool hadSun = self.HasBuff(RoR2Content.Buffs.Overheat);

            orig(self, deltaTime);

            if (NetworkServer.active && hadSun && !self.HasBuff(RoR2Content.Buffs.Overheat))
            {
                DotController dotController = DotController.FindDotController(self.gameObject);

                if (dotController)
                {
                    for (int i = dotController.dotStackList.Count - 1; i >= 0; i--)
                    {
                        DotController.DotStack dotStack = dotController.dotStackList[i];
                        if (dotStack.dotIndex == DotController.DotIndex.Burn)
                        {
                            dotStack.damage = 0f;
                            dotStack.timer = 0f;
                            dotController.RemoveDotStackAtServer(i);
                        }
                    }
                }
            }
        }
    }
}
