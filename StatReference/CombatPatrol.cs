
using WH40k.Models;

namespace WH40k.StatReference.CombatPatrol
{
    public static class ImperialKnights {
        public static ArmyList DoubleArmigerList()
        {
            return new ArmyList
            {
                Name = "Double Armiger",
                Units = new List<Unit>{
                    Test.HelverinSquad(),
                    Test.HelverinSquad(),
                }
            };
        }
    }

    public static class Tyranids {
        public static ArmyList VardenghastSwarmList()
        {
            return new ArmyList
            {
                Name = "Vardenghast Swarm",
                Units = new List<Unit>{
                    Units.Tyranid.Units.PsycophageUnit(),
                    Units.Tyranid.Units.TyranidPrimeUnit(),
                    Units.Tyranid.Units.VonRyanLeaperUnit(),
                    Units.Tyranid.Units.BarbguantUnit(),
                    Units.Tyranid.Units.TermaguantUnit()
                }
            };
        }
    }
}

namespace WH40k.StatReference.CombatPatrol.Units.Tyranid
{
    public static class Units
    {
        public static Unit PsycophageUnit()
        {
            var psychophageModelGroup = new ModelGroup
            {
                Name = "Psychophage",
                Ref = new ModelReference(8, 9, 3, 10, 8, 3),
                RangedWeapons = new List<WeaponReference>{
                    //D6 attacks
                    new WeaponReference(12, "D6", WeaponReference.AutoHit, 6, -1, 1, "Psychoclastic Torrent")
                },
                MeleeWeapons = new List<WeaponReference>{
                    new WeaponReference(-1, "D6+1", 3, 6, -1, 2, "Talons and betentacled maw")
                }
            };

            psychophageModelGroup.Initialize(1);

            return new Unit
            {
                Models = new List<ModelGroup> {
                    psychophageModelGroup
                }
            };
        }

        public static Unit TyranidPrimeUnit()
        {
            var terrorOfVardenghast = new ModelGroup
            {
                Name = "Terror Of Vardenghast",
                Ref = new ModelReference(12, 5, 4, 6, 7, 1),
                RangedWeapons = new List<WeaponReference> { },
                MeleeWeapons = new List<WeaponReference>{
                    new WeaponReference(-1, "6", 2, 6, -1, 2, "Prime talons")
                }
            };

            terrorOfVardenghast.Initialize(1);

            return new Unit
            {
                Models = new List<ModelGroup> {
                    terrorOfVardenghast
                }
            };
        }

        public static Unit TermaguantUnit()
        {
            var termagants = new ModelGroup
            {
                Name = "Termagants",
                Ref = new ModelReference(6, 3, 5, 1, 8, 2),
                RangedWeapons = new List<WeaponReference> {
                    new WeaponReference(18,"1",4,5,0,1, "Fleshborer")
                },
                MeleeWeapons = new List<WeaponReference> {
                    new WeaponReference(-1,"1",4,3,0,1, "Chitinous claws and teeth")
                }
            };

            termagants.Initialize(10);

            return new Unit(termagants);
        }

        internal static Unit VonRyanLeaperUnit()
        {
            var vRyanLeap = new ModelGroup
            {
                Name = "Von Ryan's Leapers",
                Ref = new ModelReference(10, 5, 4, 3, 8, 1)
                {
                    InvulnerableSave = 6
                },
                RangedWeapons = new List<WeaponReference> { },
                MeleeWeapons = new List<WeaponReference>{
                    new WeaponReference(-1, "6", 3, 5, -1, 1, "Leaper's talons")
                }
            };

            vRyanLeap.Initialize(1);

            return new Unit(vRyanLeap);
        }

        internal static Unit BarbguantUnit()
        {
            var barbgaunts = new ModelGroup
            {
                Name = "Barbgaunts",
                Ref = new ModelReference(6, 4, 4, 2, 8, 1),
                RangedWeapons = new List<WeaponReference> {
                    new WeaponReference(24,"1D6",4,5,0,1, "Barblauncher")
                },
                MeleeWeapons = new List<WeaponReference> {
                    new WeaponReference(-1,"1",4,4,0,1, "Chitinous claws and teeth")
                }
            };

            barbgaunts.Initialize(10);

            return new Unit(barbgaunts);
        }
    }
}