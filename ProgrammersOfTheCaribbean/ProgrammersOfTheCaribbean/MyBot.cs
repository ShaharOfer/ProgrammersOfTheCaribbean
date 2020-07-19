using System;
using System.Collections.Generic;
using System.Linq;
using Pirates;

namespace ProgrammersOfTheCaribbean
{
    public class MyBot : IPirateBot
    {
        private Dictionary<Pirate, Island> _pirateToIsland = new Dictionary<Pirate, Island>();
        
        public void DoTurn(IPirateGame state)
        {
            List<Pirate> myPirates = state.AllMyPirates();
            myPirates.Reverse();
            if (_pirateToIsland.Count == 0)
            {
                // Need to allocate pirates to islands
                AllocatePiratesToIslands(state, myPirates, state.Islands());
            }
            // Here the pirates know their target islands

            myPirates.ForEach(pirate =>
            {
                if (!pirate.IsLost)
                {
                    var directions = state.GetDirections(pirate, _pirateToIsland[pirate]);
                    state.SetSail(pirate, directions[0]);
                }
            });
         
        }

        private void AllocatePiratesToIslands(IPirateGame state, List<Pirate> myPirates, List<Island> islands)
        {
            int piratesIndex = 0;
            int currentIslandIndex = 0;
            var topClosestIslands = GetThreeClosestIslands(state, islands, myPirates[0]);

            int numberOfPirets = (int)Math.Floor(myPirates.Count * 0.2);
            AllocatePiretsToIsland(topClosestIslands[currentIslandIndex], myPirates, piratesIndex, piratesIndex + numberOfPirets); ;
            currentIslandIndex++;
            piratesIndex += numberOfPirets;

            numberOfPirets = (int)Math.Floor(myPirates.Count * 0.6);
            AllocatePiretsToIsland(topClosestIslands[currentIslandIndex], myPirates, piratesIndex, piratesIndex + numberOfPirets); ;
            currentIslandIndex++;
            piratesIndex += numberOfPirets;

            numberOfPirets = (int)Math.Floor(myPirates.Count * 0.2);
            AllocatePiretsToIsland(topClosestIslands[currentIslandIndex], myPirates, piratesIndex, piratesIndex + numberOfPirets); ;
        }

        private List<Island> GetThreeClosestIslands(IPirateGame state, List<Island> islands, Pirate pirate)
        {
            var topClosestIslands = islands.OrderBy(island => state.Distance(pirate, island)).Take(3).ToList();
            return topClosestIslands;
        }

        private void AllocatePiretsToIsland(Island islandToBeAllocated, List<Pirate> pirets, int startPiretIndex, int endPiretIndex)
        {
            for (int i = startPiretIndex; i < endPiretIndex; i++)
            {
                _pirateToIsland.Add(pirets[i], islandToBeAllocated);
            }
        }
    }
}
