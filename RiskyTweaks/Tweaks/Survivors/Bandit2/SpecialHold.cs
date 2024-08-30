using EntityStates;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Survivors.Bandit2
{
    public class SpecialHold : TweakBase<SpecialHold>
    {
        public override string ConfigCategoryString => "Survivors - Bandit";

        public override string ConfigOptionName => "(Client-Side) Special Hold Button";

        public override string ConfigDescriptionString => "Revolver skills only fire after releasing the button.";

        protected override void ApplyChanges()
        {
            IL.EntityStates.Bandit2.Weapon.BasePrepSidearmRevolverState.FixedUpdate += BasePrepSidearmRevolverState_FixedUpdate;
        }

        private void BasePrepSidearmRevolverState_FixedUpdate(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After, x => x.MatchCall(typeof(EntityState), "get_fixedAge")))
            {
                c.Emit(OpCodes.Ldarg_0);//self
                c.EmitDelegate<Func<float, EntityStates.Bandit2.Weapon.BasePrepSidearmRevolverState, float>>((fixedAge, self) =>
                {
                    if (self.isAuthority && self.inputBank && self.inputBank.skill4.down)
                    {
                        return -1f;
                    }
                    return fixedAge;
                });
            }
        }
    }
}
