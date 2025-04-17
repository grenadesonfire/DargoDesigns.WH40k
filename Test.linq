Random rand = new Random((int)DateTime.Now.Ticks);

void Main()
{
	var tGaunts = Test.TermagauntSquad(10);
	var bigTGaunts = Test.TermagauntSquad(20);
	var brotherhoodTSquad = Test.GreyKnightTerminatorSquad();
	
	var simulations = 10000;
	
	RunSimulation(simulations, tGaunts, brotherhoodTSquad);
	RunSimulation(simulations, brotherhoodTSquad, tGaunts);
	
	RunSimulation(simulations, bigTGaunts, brotherhoodTSquad);
	RunSimulation(simulations, brotherhoodTSquad, bigTGaunts);
}

void RunSimulation(int simulations, Unit tGaunts, Unit brotherhoodTSquad)
{
	var tGauntsResult = new Dictionary<int, int>();

	for (var i = 0; i < simulations; i++)
	{
		var result = SimulateShooting(1, tGaunts, brotherhoodTSquad);

		if (!tGauntsResult.ContainsKey(result)) tGauntsResult.Add(result, 0);

		tGauntsResult[result]++;
		//SimulateShooting(1, brotherhoodTSquad, tGaunts).Dump("Terminators shooting gaunts");
	}

	tGauntsResult.ToList().OrderBy(gr => gr.Key).Dump();
}

/// <summary>
/// Returns wounds dealt
/// </summary>
int SimulateShooting(int rounds, Unit attacker, Unit defender)
{
	var wounds = 0;
	
	for(var r=0;r<rounds;r++){
		wounds += SimulateUnitRangedAttack(attacker, defender);
	}
	
	return wounds;
}

int SimulateUnitRangedAttack(Unit atk, Unit def){
	var wounds = 0;
	
	foreach(var mg in atk.Models){
		wounds += SimulateModelRangedAttack(mg, def.Models.OrderBy(m => m.Ref.Toughness).First());
	}
	
	return wounds;
}

int SimulateModelRangedAttack(ModelGroup atk, ModelGroup def)
{
	var wounds = 0;
	foreach(var weapon in atk.RangedWeapons){
		wounds += SimulateRangedWeaponAttack(def.BoardModels, weapon, def);
	}
	return wounds;
}

int SimulateRangedWeaponAttack(List<BoardModelStats> bModels, WeaponReference weapon, ModelGroup def)
{
	var chance = 1.0m;
	
	//Check for hit
	chance *= (6 - weapon.Skill+1);
	chance /= 6;
	
	//Check for wound
	chance *= TenthEdWoundChance(weapon.Strength, def.Ref.Toughness);
	chance /= 6;
	
	//Model Save, ignore invuln and ap for now
	var tsave = def.Ref.Save - 1;
//	tsave = !def.Ref.InvulnerableSave.HasValue ? tsave : (Math.Min(6-def.Ref.InvulnerableSave, tsave)); 
	chance *= tsave;
	chance /= 6;

	//FNP ignored for now
	var damageInstances = new List<int>();
	for (var attacks = 0; attacks < weapon.Attacks * bModels.Count(bm => !bm.Destroyed); attacks++)
	{
		var roll = rand.Next(100);
		var beat = (1-chance)*100;
		if (roll >= beat)
		{
			damageInstances.Add(weapon.Damage);
		}
	}
	
	var wounds = 0;
	
	foreach(var dInst in damageInstances) {
		
	}
	
	return wounds;
}

decimal TenthEdWoundChance(int str, int tough){
	if (str > 2*tough) return 5;
	if (str > tough) return 4;
	if (str == tough) return 3;
	if (str < tough && 2 * str > tough) return 2;
	return 1;
}

class Unit {
	public List<ModelGroup> Models { get; set; }
}

class BoardModelStats {
	public int WoundsRemaining { get; set; }
	public bool Destroyed { get; set; }
}

class ModelGroup {
	public ModelReference Ref { get; set; }
	public List<WeaponReference> RangedWeapons { get; set; }
	public List<WeaponReference> MeleeWeapons { get; set; }
	public List<BoardModelStats> BoardModels { get; set; }
}

class DataReference {
	public List<string> Keywords { get; set; }
}

class ModelReference : DataReference {
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

class WeaponReference : DataReference {
	public WeaponReference() { }
	
	public WeaponReference(
		int range, 
		int attacks, 
		int skill, 
		int strength,
		int armorpen, 
		int damage)
	{
		Range = range;
		Attacks = attacks;
		Skill = skill;
		Strength = strength;
		ArmorPenetration = armorpen;
		Damage = damage;
	}

	/// <summary>
	/// Range of -1 means melee only
	///</summary>
	public int Range { get; set; }
	public int Attacks { get; set; }
	/// <summary>
	/// Weapon / Balistics Skill
	/// </summary>
	public int Skill { get; set; }
	public int Strength { get; set; }
	public int ArmorPenetration { get; set; }
	public int Damage { get; set; }
}

class Test {
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
				Attacks = 1,
				Skill = 4,
				Strength = 3,
				ArmorPenetration = 0,
				Damage = 1
			}
		};

		var termagauntRanged = new List<WeaponReference>()
		{
			new WeaponReference(12,2,4,3,0,1)
		};

		return new Unit{
			Models = new List<ModelGroup>{
				new ModelGroup{
					Count = count,
					Ref = termagauntRef,
					MeleeWeapons = termaguantMelee,
					RangedWeapons = termagauntRanged
				}
			}
		};
	}
	
	public static Unit GreyKnightTerminatorSquad() {
		var justicar = GreyKnightsJusticar();
		var terminators = GreyKnightsTerminators();
		
		return new Unit {
			Models = new List<ModelGroup>(){
				justicar,
				terminators
			}	
		};
	}
	
	public static ModelGroup GreyKnightsJusticar() {
		return new ModelGroup {
			Count = 1, // should be 1
			MeleeWeapons = new List<WeaponReference> {
				new WeaponReference(-1,4,3,6,-2,2)
			},
			RangedWeapons = new List<WeaponReference> {
				new WeaponReference(24,2,3,4,0,1)
			},
			Ref = new ModelReference(5,5,2,3,6,2)
		};
	}

	public static ModelGroup GreyKnightsTerminators()
	{
		return new ModelGroup
		{
			Count = 4,
			MeleeWeapons = new List<WeaponReference> {
				new WeaponReference(-1,4,3,6,-2,2)
			},
			RangedWeapons = new List<WeaponReference> {
				new WeaponReference(24,2,3,4,0,1)
			},
			Ref = new ModelReference(5, 5, 2, 3, 6, 2)
		};
	}
}