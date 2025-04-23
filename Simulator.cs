using WH40k.Models;

namespace WH40k.Simulator;

public class Simulator
{

    Random rand = new Random((int)DateTime.Now.Ticks);

    public static void RunArmySImulation(
        int numSimulations, 
        ArmyList primary, ArmyList secondary, 
        Action<IEnumerable<Stats>> postSimulation)
    {
        //For simulation count
        //Simulate Battle / half will be primary as first attacker half will be secondary as first attacker
        //Results
        //    Wounds Inflicted by both armies on each round
        //    Tabled percentage

        //SimulateBattle()
        //  Should take in attacker and defender
        //  wounded units should not fight nor be targeted for attacks  
        throw new NotImplementedException();
    }

    public static void StaticRunSimulation(int simulations, Unit attacker, Unit defender, Action<IEnumerable<Stats>> postSimulation)
    {
        var s = new Simulator();
        s.RunSimulation(simulations, attacker, defender, postSimulation);
    }

    public static void StaticRunSimulation(
        int simulations, Unit attacker,
        Unit defender, Action<IEnumerable<Stats>> postSimulation,
        List<string> phases)
    {

        var s = new Simulator();
        s.RunSimulation(simulations, attacker, defender, postSimulation, phases);
    }

    public void RunSimulation(int simulations, Unit attacker, Unit defender, Action<IEnumerable<Stats>> postSimulation, List<string> phases)
    {
        var woundResults = new Dictionary<int, int>();

        for (var i = 0; i < simulations; i++)
        {
            ResetWounds(attacker);
            ResetWounds(defender);

            var result = 0;
            foreach (var phase in phases)
            {
                switch (phase.ToLower())
                {
                    case "shoot":
                        result += SimulateRoundsOfShooting(1, attacker, defender);
                        // Fight back
                        SimulateRoundsOfShooting(1, defender, attacker);
                        break;
                    case "melee":
                        result += SimulateRoundsOfMelee(1, attacker, defender);
                        // Fight back
                        SimulateRoundsOfMelee(1, defender, attacker);
                        break;
                }
            }

            if (!woundResults.ContainsKey(result)) woundResults.Add(result, 0);

            woundResults[result]++;
        }

        var rawStats = woundResults.ToList().OrderBy(gr => gr.Key).ToList();

        var stats = ProcessSimulationResults(rawStats, simulations);

        //stats.Dump(title);
        postSimulation(stats);
    }

    public void RunSimulation(int simulations, Unit attacker, Unit defender, Action<IEnumerable<Stats>> postSimulation)
    {
        var woundResults = new Dictionary<int, int>();

        for (var i = 0; i < simulations; i++)
        {
            ResetWounds(attacker);
            ResetWounds(defender);

            var result = SimulateRoundsOfMelee(1, attacker, defender);

            if (!woundResults.ContainsKey(result)) woundResults.Add(result, 0);

            woundResults[result]++;
        }

        var rawStats = woundResults.ToList().OrderBy(gr => gr.Key).ToList();

        var stats = ProcessSimulationResults(rawStats, simulations);

        //stats.Dump(title);
        postSimulation(stats);
    }

    static void ResetWounds(Unit u)
    {
        foreach (var mg in u.Models)
        {
            foreach (var bs in mg.BoardModels)
            {
                bs.Destroyed = false;
                bs.WoundsRemaining = mg.Ref.Wound;
            }
        }
    }


    IEnumerable<Stats> ProcessSimulationResults(List<KeyValuePair<int, int>> results, int total)
    {
        return results.Select(r => new Stats
        {
            DamageDone = r.Key,
            Results = r.Value,
            SpecificOutcomePercentage = (r.Value / (total * 1.0m)) * 100m,
            PrecedingEventLikelihood = results.Where(re => re.Key >= r.Key).Sum(re => re.Value),
            OutcomeofEventOrBetter = ((results.Where(re => re.Key >= r.Key).Sum(re => re.Value)) / (total * 1.0m)) * 100m
        });
    }

    int SimulateRoundsOfMelee(int rounds, Unit attacker, Unit defender)
    {
        var wounds = 0;

        for (var r = 0; r < rounds; r++)
        {
            wounds += SimulateUnitMeleeAttack(attacker, defender);
        }

        return wounds;
    }

    /// <summary>
    /// Returns wounds dealt
    /// </summary>
    int SimulateRoundsOfShooting(int rounds, Unit attacker, Unit defender)
    {
        var wounds = 0;

        for (var r = 0; r < rounds; r++)
        {
            wounds += SimulateUnitRangedAttack(attacker, defender);
        }

        return wounds;
    }

    int SimulateUnitRangedAttack(Unit atk, Unit def)
    {
        var wounds = 0;

        foreach (var mg in atk.Models)
        {
            wounds += SimulateModelRangedAttack(mg, def);//.Models.OrderBy(m => m.Ref.Toughness).First());
        }

        return wounds;
    }

    int SimulateModelRangedAttack(ModelGroup atk, Unit def)
    {
        var wounds = 0;
        foreach (var weapon in atk.RangedWeapons)
        {
            wounds += SimulateWeaponAttack(weapon, atk.BoardModels.Count(), def);
        }
        return wounds;
    }

    int SimulateUnitMeleeAttack(Unit atk, Unit def)
    {
        var wounds = 0;

        foreach (var mg in atk.Models)
        {
            wounds += SimulateModelMeleeAttack(mg, def);//.Models.OrderBy(m => m.Ref.Toughness).First());
        }

        return wounds;
    }

    int SimulateModelMeleeAttack(ModelGroup atk, Unit def)
    {
        var wounds = 0;
        var weapon = atk.MeleeWeapons.First();

        wounds += SimulateWeaponAttack(weapon, atk.BoardModels.Count(), def);

        return wounds;
    }

    int SimulateWeaponAttack(WeaponReference weapon, int attackers, Unit def)
    {
        var weakestSaveRef = def.Models.OrderBy(m => m.Ref.Toughness).First();

        var chance = 1.0m;

        //Check for hit
        chance *= (6 - weapon.Skill + 1);
        chance /= 6;

        //Check for wound
        chance *= TenthEdWoundChance(weapon.Strength, weakestSaveRef.Ref.Toughness);
        chance /= 6;

        //Model Save, ignore invuln and ap for now
        var tsave = TenthEdInvulnAndAP(weapon.ArmorPenetration, weakestSaveRef.Ref.Save, weakestSaveRef.Ref.InvulnerableSave);
        chance *= tsave;
        chance /= 6;

        //FNP ignored for now
        var damageInstances = new List<int>();
        ///var allModels = //def.Models.Sum(m => m.BoardModels.Count(bm => !bm.Destroyed));
        var totalAttacks = CalculateAttacks(weapon.Attacks, attackers);
        for (var attacks = 0; attacks < totalAttacks; attacks++)
        {
            var roll = rand.Next(100);
            var beat = (1 - chance) * 100;
            if (roll >= beat)
            {
                damageInstances.Add(weapon.Damage);
            }
        }

        var wounds = 0;

        foreach (var dInst in damageInstances)
        {
            //Need to account for toughness / bodyguard and all that jazz
            var unitToHit = def.Models.SelectMany(m => m.BoardModels.Where(bm => !bm.Destroyed)).OrderBy(bm => bm.WoundsRemaining).FirstOrDefault(bm => !bm.Destroyed);
            if (unitToHit == null) continue;

            unitToHit.WoundsRemaining -= dInst;
            if (unitToHit.WoundsRemaining <= 0) unitToHit.Destroyed = true;
            wounds += dInst;
        }

        return wounds;
    }

    private int CalculateAttacks(string attacks, int attackers)
    {
        if (int.TryParse(attacks, out int simpleAttack))
        {
            return simpleAttack * attackers;
        }

        var breakdown = attacks.Split('+');
        var min = 0;
        if (breakdown.Length > 1)
        {
            min = int.Parse(breakdown[1]);
        }

        var numD6 = int.Parse(breakdown[0].Split(['D', 'd'])[0]);
        var attackNo = 0;

        for (var atker = 0; atker < attackers; atker++)
        {
            for (var dice = 0; dice < numD6; dice++)
            {
                attackNo += rand.Next(6) + 1;
            }
        }

        return min;
    }

    /// <summary>
    /// Assumes armorPen is 0 or negative
    /// </summary>
    decimal TenthEdInvulnAndAP(int armorPenetration, int save, int? invulnerableSave)
    {
        save -= armorPenetration;

        if (invulnerableSave.HasValue && invulnerableSave < save) return 6 - invulnerableSave.Value + 1;

        //should not factor into equation
        if (save > 6) return -1;

        //return chance of success from attackers perspective
        return save - 1;
    }

    decimal TenthEdWoundChance(int str, int tough)
    {
        if (str > 2 * tough) return 5; // 2+
        if (str > tough) return 4; // 3+
        if (str == tough) return 3; // 4+
        if (str < tough && 2 * str > tough) return 2;
        return 1;
    }
}