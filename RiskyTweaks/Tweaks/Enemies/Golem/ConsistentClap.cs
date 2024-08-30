using MonoMod.Cil;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Enemies.Golem
{
    public class ConsistentClap : TweakBase<ConsistentClap>
    {
        public override string ConfigCategoryString => "Enemies - Golem";

        public override string ConfigOptionName => "(Server-Side) Consistent Clap Damage";

        public override string ConfigDescriptionString => "Golem Clap always does 30 damage, insstead of scaling between 0-40 based on AoE range.";

        protected override void ApplyChanges()
        {
            SneedUtils.SetAddressableEntityStateField("RoR2/Base/Golem/EntityStates.GolemMonster.ClapState.asset", "damageCoefficient", "1.5");
            IL.EntityStates.GolemMonster.ClapState.FixedUpdate += ClapState_FixedUpdate;
        }

        private void ClapState_FixedUpdate(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(
                 x => x.MatchCallvirt<BlastAttack>("Fire")
                ))
            {
                c.EmitDelegate<Func<BlastAttack, BlastAttack>>(blastAttack =>
                {
                    blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                    return blastAttack;
                });
            }
            else
            {
                UnityEngine.Debug.LogError("RiskyTweaks: Golem ConsistentClap IL Hook failed");
            }
        }
    }
}
