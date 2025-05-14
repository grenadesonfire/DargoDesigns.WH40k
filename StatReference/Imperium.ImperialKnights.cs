


using WH40k.Models;

namespace WH40k.StatReference.Imperium;

public static class ImperialKnightsUnits
{
    public static Unit CanisRexUnit() {
		var mg = new ModelGroup
		{
			MeleeWeapons = new List<WeaponReference>() {
				new WeaponReference(-1, "10", 2, 10, -2, 3, "Stomp")	
			},
			RangedWeapons = new List<WeaponReference>() {
				//new WeaponReference(36,"2D6",2,7,-1,2, "Chainbreaker las-impulsor - low intensity"),
                new WeaponReference(24,"1D6",2,14,-3,4, "Chainbreaker las-impulsor - High intensity", 1),
                new WeaponReference(36,"4",2,6,0,1, "Chainbreaker multi-laser", 1),
			},
			Ref = new ModelReference(10,12,3,22,5,10){
                Name = "Canis Rex",
				InvulnerableSave = 5,
                SustainedHitsRanged = 5,
			},
		};
		
		mg.SpawnBoardModels(1);
		
		return new Unit{
			Models = new List<ModelGroup>(){
				mg
			}
		};
	}

    public static Unit CanisRexUnitNoChainBreaker() {
		var mg = new ModelGroup
		{
			MeleeWeapons = new List<WeaponReference>() {
				new WeaponReference(-1, "10", 2, 10, -2, 3, "Stomp")	
			},
			RangedWeapons = new List<WeaponReference>() {
				//new WeaponReference(36,"2D6",2,7,-1,2, "Chainbreaker las-impulsor - low intensity"),
                new WeaponReference(24,"1D6",2,14,-3,4, "Chainbreaker las-impulsor - High intensity", 1),
                new WeaponReference(36,"4",2,6,0,1, "Chainbreaker multi-laser", 1),
			},
			Ref = new ModelReference(10,12,3,22,5,10){
                Name = "Canis Rex",
				InvulnerableSave = 5,
                //SustainedHitsRanged = 5,
			},
		};
		
		mg.SpawnBoardModels(1);
		
		return new Unit{
			Models = new List<ModelGroup>(){
				mg
			}
		};
	}

    public static Unit HelverinSquad() {
		var mg = new ModelGroup
		{
			MeleeWeapons = new List<WeaponReference>() {
				new WeaponReference(-1, "4", 3, 6, 0, 1, "Stomp")	
			},
			RangedWeapons = new List<WeaponReference>() {
				new WeaponReference(48,"4",3,9,-1,3, "Autocannon"), //Autocannon
				new WeaponReference(48,"4",3,9,-1,3, "Autocannon"),
				new WeaponReference(12,"1",3,9,-4, 6, "Melta") // damage should be d6 not 6 and has melta 2 keyword
			},
			Ref = new ModelReference(12,10,3,12,7,8){
				InvulnerableSave = 5
			},
		};
		
		mg.SpawnBoardModels(1);
		
		return new Unit{
			Models = new List<ModelGroup>(){
				mg
			}
		};
	}

	public static Unit HelverinSquadV2() {
		var mg = new ModelGroup
		{
			MeleeWeapons = new List<WeaponReference>() {
				new WeaponReference(-1, "4", 3, 6, 0, 1, "Stomp")	
			},
			RangedWeapons = new List<WeaponReference>() {
				new WeaponReference(48,"4",3,9,-1,3, "Autocannon"), //Autocannon
				new WeaponReference(48,"4",3,9,-1,3, "Autocannon"),
				new WeaponReference(12,"1",3,9,-4, 6, "Melta") // damage should be d6 not 6 and has melta 2 keyword
			},
			Ref = new ModelReference(mov: 12, tough: 9,3,12,7,8){
				InvulnerableSave = 5
			},
		};
		
		mg.SpawnBoardModels(1);
		
		return new Unit{
			Models = new List<ModelGroup>(){
				mg
			}
		};
	}
}