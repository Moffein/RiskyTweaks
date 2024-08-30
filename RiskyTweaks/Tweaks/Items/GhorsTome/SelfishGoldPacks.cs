using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskyTweaks.Tweaks.Items.GhorsTome
{
    public class SelfishGoldPacks : TweakBase<SelfishGoldPacks>
    {
        public override string ConfigCategoryString => "Items";

        public override string ConfigOptionName => "(Server-Side) Ghors Tome - Selfish Gold Packs";

        public override string ConfigDescriptionString => "Gold from this item is given to the person who picked it up, instead of distributing it among the team.";

        protected override void ApplyChanges()
        {
            //This seems like a convoluted way to do this.
            On.RoR2.MoneyPickup.OnTriggerStay += MoneyPickup_OnTriggerStay;
        }

        private void MoneyPickup_OnTriggerStay(On.RoR2.MoneyPickup.orig_OnTriggerStay orig, MoneyPickup self, UnityEngine.Collider other)
        {
            if (self.baseObject.name == "BonusMoneyPack(Clone)") //Only modify Tome, in case any other mod wants to add MoneyPickups
            {
                if (NetworkServer.active && self.alive)
                {
                    TeamIndex objectTeam = TeamComponent.GetObjectTeam(other.gameObject);
                    if (objectTeam == self.teamFilter.teamIndex)
                    {
                        bool modifyPickup = true;
                        CharacterBody cb = other.gameObject.GetComponent<CharacterBody>();
                        if (!(cb && cb.isPlayerControlled))
                        {
                            modifyPickup = false;
                        }

                        //Give gold to single player, instead of splitting it
                        if (modifyPickup && cb && cb.master)
                        {
                            self.alive = false;
                            Vector3 position = self.transform.position;

                            cb.master.GiveMoney((uint)self.goldReward);
                            if (self.pickupEffectPrefab)
                            {
                                EffectManager.SimpleEffect(self.pickupEffectPrefab, position, Quaternion.identity, true);
                            }
                            UnityEngine.Object.Destroy(self.baseObject);
                            return;
                        }
                    }
                }
            }


            orig(self, other);
        }
    }
}
