using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RiskyTweaks.Tweaks.Enemies
{
    public class NoBossRepeat : TweakBase<NoBossRepeat>
    {
        public override string ConfigCategoryString => "Enemies";

        public override string ConfigOptionName => "(Server-Side) No Boss Repeat";

        public override string ConfigDescriptionString => "Reduces the chance of getting the same teleporter boss multiple stages in a row.";

        public override bool StopLoadOnConfigDisable => true;

        private const int maxStages = 2;	//Set to 2 so that theres always 1 boss left (since most stages have 3 boss choices)
        private List<GameObject> previousCards = new List<GameObject>();


        protected override void ApplyChanges()
        {
            Run.onRunStartGlobal += Run_onRunStartGlobal;
            IL.RoR2.CombatDirector.SetNextSpawnAsBoss += CombatDirector_SetNextSpawnAsBoss;
        }

        private void CombatDirector_SetNextSpawnAsBoss(ILContext il)
        {
            bool error = true;

            //Mark already-used cards as unavailable.
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After,
                 x => x.MatchCallvirt<DirectorCard>("IsAvailable")
                ))
            {
                c.Emit(OpCodes.Ldloc_3);
                c.EmitDelegate<Func<bool, WeightedSelection<DirectorCard>.ChoiceInfo, bool>>((flag, choice) =>
                {
                    if (flag)
                    {
                        if (choice.value != null && choice.value.spawnCard != null)
                        {
                            if (previousCards.Contains(choice.value.spawnCard.prefab))
                            {
                                return false;
                            }
                        }
                    }
                    return flag;
                });


                //Add selected cards to the list
                if (c.TryGotoNext(
                    x => x.MatchCall<CombatDirector>("PrepareNewMonsterWave")
                    ))
                {
                    c.EmitDelegate<Func<DirectorCard, DirectorCard>>(card =>
                    {
                        if (card != null && card.spawnCard != null)
                        {
                            if (previousCards.Count > 0 && previousCards.Count + 1 > maxStages)
                            {
                                previousCards.RemoveAt(0);
                            }
                            previousCards.Add(card.spawnCard.prefab);
                        }
                        return card;
                    });
                    error = false;
                }
            }

            if (error)
            {
                Debug.LogError("RiskyTweaks: NoBossRepeat IL Hook failed");
            }
        }

        private void Run_onRunStartGlobal(Run obj)
        {
            previousCards.Clear();
        }
    }
}
