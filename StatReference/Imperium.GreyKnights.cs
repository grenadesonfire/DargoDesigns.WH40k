

using WH40k.Models;

namespace WH40k.StatReference.Imperium.GreyKnights;

public static class GreyKnightsUnits
{
    public static Unit GreyKnightTerminatorSquad()
    {
        var justicar = GreyKnightsJusticar();
        justicar.SpawnBoardModels(1);
        var terminators = GreyKnightsTerminators();
        terminators.SpawnBoardModels(4);

        return new Unit
        {
            Models = new List<ModelGroup>(){
                justicar,
                terminators
            }
        };
    }

    public static ModelGroup GreyKnightsJusticar()
    {
        return new ModelGroup
        {
            BoardModels = new List<BoardModelStats>() {
                new BoardModelStats()
            },
            MeleeWeapons = new List<WeaponReference> {
                new WeaponReference(-1,"4",3,6,-2,2, "Nemesis force weapon")
            },
            RangedWeapons = new List<WeaponReference> {
                new WeaponReference(24,"2",3,4,0,1, "Storm Bolter")
            },
            Ref = new ModelReference(5, 5, 2, 3, 6, 2)
        };
    }

    public static ModelGroup GreyKnightsTerminators()
    {
        return new ModelGroup
        {
            BoardModels = new List<BoardModelStats>() {
                new BoardModelStats(),
                new BoardModelStats(),
                new BoardModelStats(),
                new BoardModelStats(),
            },
            MeleeWeapons = new List<WeaponReference> {
                new WeaponReference(-1,"4",3,6,-2,2, "Nemesis force weapon")
            },
            RangedWeapons = new List<WeaponReference> {
                new WeaponReference(24,"2",3,4,0,1, "Storm Bolter")
            },
            Ref = new ModelReference(5, 5, 2, 3, 6, 2)
        };
    }
}