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
            List<Island> islands = state.NotMyIslands();
            List<Pirate> myPirates = state.MyPirates();
            List<Pirate> copyPirates = new List<Pirate>(myPirates);
            Dictionary<Pirate, Island> pirateToIsland = new Dictionary<Pirate, Island>();

            for (int i = 0; i < islands.Count; i++)
            {

                Pirate closestPirate = GetClosestPirate(state, islands[i], myPirates);

                //state.Debug($"closest: {closestPirate.Id.ToString()} to island: {islands[i]}");
                if (closestPirate != null)
                {
                    myPirates.Remove(closestPirate);
                    pirateToIsland.Add(closestPirate, islands[i]);
                }
            }

            copyPirates.ForEach(pirate =>
            {
                if (pirateToIsland.ContainsKey(pirate) && pirateToIsland[pirate] != null)
                {
                    Island island = pirateToIsland[pirate];
                    state.Debug($"Pirate {pirate.Id.ToString()} to island {island.Id.ToString()}");
                    List<Direction> movingDirections = state.GetDirections(pirate, island);

                    if (movingDirections.Count > 0 && !pirate.IsLost)
                    {
                        state.Debug($"Pirate: {pirate.Id.ToString()}");
                        movingDirections.ForEach(direc =>
                        {
                            state.Debug($"{direc}");
                        });
                        state.SetSail(pirate, movingDirections[0]);
                    }
                }
                else // Bot that has no island
                {
                    state.Debug($"Nothing {pirate.Id.ToString()}");
                    Island closestIsland = GetClosestIsland(state, pirate, state.NotMyIslands());
                    List<Direction> movingDirections = state.GetDirections(pirate, closestIsland);
                    state.SetSail(pirate, movingDirections[0]);
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

        private Pirate GetClosestPirate(IPirateGame state, Island island, List<Pirate> pirates)
        {
            // Move from the island
            // Move to closest attacked island
            // 
            int closetDIstance = int.MaxValue;
            Pirate closestPirate = null;
            pirates.ForEach(pirate =>
            {
                if (!pirate.IsLost)
                {
                    int distnace = state.Distance(pirate, island);
                    if (distnace < closetDIstance)
                    {
                        closestPirate = pirate;
                        closetDIstance = distnace;
                    }
                }
            });

            return closestPirate;
        }
    }
}
