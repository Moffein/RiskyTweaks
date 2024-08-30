using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Items
{
    public class BottledChaosBlacklist : TweakBase<BottledChaosBlacklist>
    {
        public override string ConfigCategoryString => "Items";

        public override string ConfigOptionName => "(Server-Side) Bottled Chaos - Blacklist Equipments";

        public override string ConfigDescriptionString => "Blacklists Volcanic Egg and Eccentric Vase from being randomly triggered.";

        protected override void ApplyChanges()
        {
            RoR2Application.onLoad += OnLoad;
        }

        private void OnLoad()
        {
            RoR2Content.Equipment.FireBallDash.canBeRandomlyTriggered = false;
            RoR2Content.Equipment.Gateway.canBeRandomlyTriggered = false;
            EquipmentCatalog.randomTriggerEquipmentList.Remove(RoR2Content.Equipment.FireBallDash.equipmentIndex);
            EquipmentCatalog.randomTriggerEquipmentList.Remove(RoR2Content.Equipment.Gateway.equipmentIndex);
        }
    }
}
