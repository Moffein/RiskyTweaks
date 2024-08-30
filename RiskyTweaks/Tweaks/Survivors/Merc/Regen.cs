using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RiskyTweaks.Tweaks.Survivors.Merc
{
    public class Regen : TweakBase<Regen>
    {
        public override string ConfigCategoryString => "Survivors - Mercenary";

        public override string ConfigOptionName => "(Server-Side) Regen Buff";

        public override string ConfigDescriptionString => "Gives Mercenary standard melee character regen.";

        public override bool StopLoadOnConfigDisable => true;

        protected override void ApplyChanges()
        {
            GameObject bodyPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/MercBody");
            CharacterBody cb = bodyPrefab.GetComponent<CharacterBody>();
            if (!cb) return;
            cb.baseRegen = 2.5f;
            cb.levelRegen = 0.5f;
        }
    }
}
