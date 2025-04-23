using System.Net;

namespace WH40k.Models;

public struct Stats {
	public int DamageDone { get; set; }
	public int Results { get; set; }
	public decimal SpecificOutcomePercentage { get; set; }
	public int PrecedingEventLikelihood { get; set; }
	public decimal OutcomeofEventOrBetter { get; set; }
}

public class ArmyList : DataReference {
	public List<Unit> Units { get; set; } = new List<Unit>();
}


public class Unit {
	public Unit(){}
    public Unit(ModelGroup termagants)
    {
		Models.Add(termagants);
    }

    public List<ModelGroup> Models { get; set; } = new List<ModelGroup>();
}

public class BoardModelStats {
	public int WoundsRemaining { get; set; }
	public bool Destroyed { get; set; }
}

public class ModelGroup : DataReference{
	public ModelReference Ref { get; set; }
	public List<WeaponReference> RangedWeapons { get; set; }
	public List<WeaponReference> MeleeWeapons { get; set; }
	public List<BoardModelStats> BoardModels { get; set; }

    internal void Initialize(int number)
    {
		BoardModels = new List<BoardModelStats>();
        for(var modelIdx=0;modelIdx<number;modelIdx++) {
			BoardModels.Add(
				new BoardModelStats{
					WoundsRemaining = Ref.Wound
				}
			);
		}
    }

    internal void SpawnBoardModels(int numModels)
	{
		BoardModels = new List<BoardModelStats>();
		
		for(var bm=0;bm<numModels;bm++){
			var stat = new BoardModelStats();
			stat.WoundsRemaining = Ref.Wound;
			BoardModels.Add(stat);	
		}
	}
}

public class DataReference {
	public List<string> Keywords { get; set; }
	public string Name { get; set; }
}

public class ModelReference : DataReference {
	public int Movement { get; set; }
	public int Toughness { get; set; }
	public int Save { get; set; }
	public int Wound { get; set; }
	public int Leadership { get; set; }
	public int ObjectiveControl { get; set; }
	
	public int? InvulnerableSave { get; set; }
	public int? FeelNoPain { get; set; }
	
	public ModelReference() { }
	
	public ModelReference(
		int mov,
		int tough,
		int save,
		int wound,
		int leadership,
		int oc) {
		Movement = mov;
		Toughness = tough;
		Save = save;
		Wound = wound;
		Leadership = leadership;
		ObjectiveControl = oc;
	}
}

public class WeaponReference : DataReference {
	public static int AutoHit = -1;
	public WeaponReference() { }
	
	public WeaponReference(
		int range, 
		string attacks, 
		int skill, 
		int strength,
		int armorpen, 
		int damage,
		string name)
	{
		Range = range;
		Attacks = attacks;
		Skill = skill;
		Strength = strength;
		ArmorPenetration = armorpen;
		Damage = damage;
		Name = name;
	}

	/// <summary>
	/// Range of -1 means melee only
	///</summary>
	public int Range { get; set; }
	public string Attacks { get; set; }
	/// <summary>
	/// Weapon / Balistics Skill
	/// </summary>
	public int Skill { get; set; }
	public int Strength { get; set; }
	public int ArmorPenetration { get; set; }
	public int Damage { get; set; }
}