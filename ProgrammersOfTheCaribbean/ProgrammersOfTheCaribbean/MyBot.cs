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
            islands.AddRange(state.EnemyIslands());
            List<Pirate> myPirates = state.AllMyPirates();
            state.Debug($"Number of pirates: {myPirates.Count}");
            Dictionary<Pirate, Island> pirateToIsland = new Dictionary<Pirate, Island>();
            List<Pirate> enemyPirates = state.AllEnemyPirates();

            for (int i = 0; i < myPirates.Count; i++)
            {
                Island closestIsland = GetClosestIsland(state, myPirates[i], islands);
                islands.Remove(closestIsland);
                pirateToIsland.Add(myPirates[i], closestIsland);
            }

            myPirates.ForEach(pirate =>
            {
                if (pirateToIsland.ContainsKey(pirate) && pirateToIsland[pirate] != null)
                {
                    Island island = pirateToIsland[pirate];
                    state.Debug($"Pirate {pirate.Id.ToString()} to island {island.Id.ToString()}");
                    List<Direction> movingDirections = state.GetDirections(pirate, island);

                    if (movingDirections.Count > 0 && !pirate.IsLost)
                    {
                        state.SetSail(pirate, movingDirections[0]);
                    }
                }
                else // Bot that has no island
                {
                    state.Debug("Nothing");
                    state.Debug(pirate.Id.ToString());
                    state.Debug($"{pirate.Loc.Col}, {pirate.Loc.Row}");
                }

            });
        }

        private Island GetClosestIsland(IPirateGame state, Pirate myPirate, List<Island> islands)
        {
            // Move from the island
            // Move to closest attacked island
            // 
            int closetDIstance = int.MaxValue;
            Island closestIsland = null;
            islands.ForEach(island =>
            {
                int distnace = state.Distance(myPirate, island);
                if (distnace < closetDIstance) {
                    closestIsland = island;
                    closetDIstance = distnace;
                }
            });

            return closestIsland;
        }
    }
}
