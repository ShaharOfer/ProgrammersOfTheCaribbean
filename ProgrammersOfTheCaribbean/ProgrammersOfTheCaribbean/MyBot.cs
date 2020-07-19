using System;
using System.Collections.Generic;
using Pirates;

namespace ProgrammersOfTheCaribbean
{
    class MyBot : IPirateBot
    {
        public void DoTurn(IPirateGame state)
        {
            // Initing stuff
            List<Island> islands = state.NeutralIslands();
            List<Pirate> myPirates = state.AllMyPirates();
            Dictionary<Pirate, Island> pirateToIsland = new Dictionary<Pirate, Island>();


            for (int i = 0; i < myPirates.Count; i++)
            {
                if (i < islands.Count)
                {
                    pirateToIsland.Add(myPirates[i], islands[i]);
                }
            }

            List<Island> lostIslands = state.EnemyIslands();
            for (int j = islands.Count; j < myPirates.Count; j++) 
            {
                pirateToIsland.Add(myPirates[j], lostIslands[j - islands.Count]);
            }

            myPirates.ForEach(pirate =>
            {
                Island island = pirateToIsland[pirate];
                List<Direction> movingDirections = state.GetDirections(pirate, island);
                state.Debug($"Pirate: {pirate.Id}\nLocation: {pirate.Loc.Row}, {pirate.Loc.Col}\nMove to: ");
                foreach(var direction in movingDirections)
                {
                    state.Debug(direction.ToString());
                }

                if (movingDirections.Count > 0)
                {
                    state.SetSail(pirate, movingDirections[0]);
                }

            });
        }
    }
}
