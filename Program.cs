// See https://aka.ms/new-console-template for more information
using WH40k.Simulator;
using WH40k.StatReference;
using WH40k.StatReference.CombatPatrol;
using WH40k.StatReference.Imperium.GreyKnights;

Console.WriteLine("Hello, World!");

var tGaunts = Test.TermagauntSquad(10);
var bigTGaunts = Test.TermagauntSquad(20);
var brotherhoodTSquad = GreyKnightsUnits.GreyKnightTerminatorSquad();
var helverin = WH40k.StatReference.Imperium.ImperialKnightsUnits.HelverinSquad();
var canisRex = WH40k.StatReference.Imperium.ImperialKnightsUnits.CanisRexUnit();

var simulations = 10;

//RunSimulation(simulations, tGaunts, brotherhoodTSquad);
//RunSimulation(simulations, brotherhoodTSquad, tGaunts);
//
//RunSimulation(simulations, bigTGaunts, brotherhoodTSquad);
//RunSimulation(simulations, brotherhoodTSquad, bigTGaunts);
// Simulator.StaticRunSimulation(simulations, brotherhoodTSquad, helverin, _ => Console.WriteLine("Finished."), new List<string>{ "shoot"} );
// Simulator.StaticRunSimulation(simulations, helverin, brotherhoodTSquad, _ => Console.WriteLine("Finished."), new List<string>{ "shoot"} );

Simulator.StaticRunSimulation(simulations, canisRex, brotherhoodTSquad, _ => Console.WriteLine("Finished."), new List<string>{ "shoot"} );