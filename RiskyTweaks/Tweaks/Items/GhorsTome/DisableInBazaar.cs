using MonoMod.Cil;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Items.GhorsTome
{
    public class DisableInBazaar : TweakBase<DisableInBazaar>
    {
        public override string ConfigCategoryString => "Items";

        public override string ConfigOptionName => "(Server-Side) Ghors Tome - Disable in Bazaar";

        public override string ConfigDescriptionString => "Disables this item in the Bazaar.";

        protected override void ApplyChanges()
        {
            IL.RoR2.GlobalEventManager.OnCharacterDeath += GlobalEventManager_OnCharacterDeath;
        }

        private void GlobalEventManager_OnCharacterDeath(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After,
                 x => x.MatchLdsfld(typeof(RoR2Content.Items), "BonusGoldPackOnKill")
                ))
            {
                c.EmitDelegate<Func<ItemDef, ItemDef>>(origItem =>
                {
                    return SneedUtils.IsInBazaar() ? null : origItem;
                });
            }
            else
            {
                UnityEngine.Debug.LogError("RiskyTweaks: GhorsTome DisableInBazaar IL Hook failed");
            }
        }
    }
}
