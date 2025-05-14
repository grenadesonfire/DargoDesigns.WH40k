using WH40k.Models;

namespace WH40k.StatReference;

public class Test {
	public static Unit TermagauntSquad(int count) {
		var termagauntRef = new ModelReference{
			Movement = 6,
			Toughness = 3,
			Save = 5,
			Wound = 1,
			Leadership = 8,
			ObjectiveControl = 2
		};
		
		var termaguantMelee = new List<WeaponReference>(){
			new WeaponReference {
				Range = -1,
				Attacks = "1",
				Skill = 4,
				Strength = 3,
				ArmorPenetration = 0,
				Damage = 1
			}
		};

		var termagauntRanged = new List<WeaponReference>()
		{
			new WeaponReference(12,"2",4,3,0,1, "spike fists")
		};

		return new Unit{
			Models = new List<ModelGroup>{
				new ModelGroup{
					//Count = count,
					Ref = termagauntRef,
					MeleeWeapons = termaguantMelee,
					RangedWeapons = termagauntRanged
				}
			}
		};
	}
}