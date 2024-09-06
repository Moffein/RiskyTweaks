using MonoMod.Cil;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RiskyTweaks.Tweaks.Items
{
    public class PenniesDisableInBazaar : TweakBase<PenniesDisableInBazaar>
    {
        public override string ConfigCategoryString => "Items";

        public override string ConfigOptionName => "(Server-Side) Roll of Pennies - Disable in Bazaar";

        public override string ConfigDescriptionString => "Disables this item in the Bazaar.";

        protected override void ApplyChanges()
        {
            IL.RoR2.HealthComponent.TakeDamageProcess += HealthComponent_TakeDamageProcess;
        }

        private void HealthComponent_TakeDamageProcess(ILContext il)
        {
            bool error = true;
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After, x => x.MatchLdfld(typeof(HealthComponent.ItemCounts), "goldOnHurt")))
            {
                c.EmitDelegate<Func<int, int>>(origItemCount =>
                {
                    return SneedUtils.IsInBazaar() ? 0 : origItemCount;
                });
                error = false;
            }

            if (error)
            {
                Debug.LogError("RiskyTweaks: PenniesDisableInBazaar IL hook failed.");
            }
        }
    }
}
