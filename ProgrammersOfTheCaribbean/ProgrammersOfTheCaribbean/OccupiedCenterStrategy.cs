using System;
using System.Collections.Generic;
using System.Linq;
using Pirates;

namespace ProgrammersOfTheCaribbean
{
    public class OccupiedCenterStrategy : IStrategy
    {

        private Dictionary<Pirate, int> _pirateToIsland;
        private Dictionary<Pirate, Location> _pirateToLocation;

        public OccupiedCenterStrategy()
        {
            _pirateToIsland = new Dictionary<Pirate, int>();
            _pirateToLocation = new Dictionary<Pirate, Location>();
        }

        public Dictionary<Pirate, Location> DoTurn(IPirateGame state, List<Pirate> myPirates, List<Island> islands)
        {

            state.Debug("Occupied Center Strategy");

            _pirateToLocation = new Dictionary<Pirate, Location>();

            if (_pirateToIsland.Count == 0)
            {
                // Need to allocate pirates to islands
                AllocatePiratesToIslands(state, myPirates, islands);
            }

            // Here the pirates know their target islands
            myPirates.ForEach(pirate =>
            {
                if (!pirate.IsLost && _pirateToIsland.ContainsKey(pirate))
                {
                    // state.Debug((_pirateToIsland[pirate].Owner == Consts.ME).ToString());
                    if (state.GetIsland(_pirateToIsland[pirate]).Owner == Consts.ME)
                    {
                        state.Debug("Allocatin island to pirate: " + pirate.Id.ToString());
                        AllocatePirateToNewIsland(state, pirate, islands);
                    }


                    state.Debug($"Pirate {pirate.Id.ToString()} to island { state.GetIsland(_pirateToIsland[pirate]).Id.ToString()} At ({state.GetIsland(_pirateToIsland[pirate]).Loc.Row},{state.GetIsland(_pirateToIsland[pirate]).Loc.Col})");
                    _pirateToLocation.Add(pirate, state.GetIsland(_pirateToIsland[pirate]).Loc);
                }

                state.Debug($"{pirate.Id}  {_pirateToIsland[pirate].ToString()}");
            });


            return _pirateToLocation;

        }

        /// <summary>
        /// Checking if the first location is equal to the second
        /// </summary>
        /// <param name="l1"></param>
        /// <param name="l2"></param>
        /// <returns></returns>
        private bool SameLocation(Location l1, Location l2)
        {
            return l1.Row == l2.Row && l1.Col == l2.Col;
        }

        /// <summary>
        /// The main function to allocate the pirates
        /// </summary>
        /// <param name="state"></param>
        /// <param name="myPirates"></param>
        /// <param name="islands"></param>
        private void AllocatePiratesToIslands(IPirateGame state, List<Pirate> myPirates, List<Island> islands)
        {
            int piratesIndex = 0;
            var topClosestIslands = GetThreeClosestIslands(state, islands, myPirates[0].InitialLocation);
            int indexToInsert = topClosestIslands.Count == 3 ? 2 : 0;

            int numberOfPirets = (int)Math.Floor(myPirates.Count * 0.2);
            AllocatePiretsToIsland(topClosestIslands[0], myPirates, piratesIndex, piratesIndex + numberOfPirets);
            piratesIndex += numberOfPirets;

            numberOfPirets = (int)Math.Floor(myPirates.Count * 0.4);
            AllocatePiretsToIsland(topClosestIslands[indexToInsert], myPirates, piratesIndex, piratesIndex + numberOfPirets);
            piratesIndex += numberOfPirets;

            indexToInsert = topClosestIslands.Count == 3 ? 1 : 0;
            numberOfPirets = (int)Math.Floor(myPirates.Count * 0.4);
            AllocatePiretsToIsland(topClosestIslands[indexToInsert], myPirates, piratesIndex, piratesIndex + numberOfPirets);
            piratesIndex += numberOfPirets;
        }

        private List<Island> GetThreeClosestIslands(IPirateGame state, List<Island> islands, Location startLocation)
        {
            var topClosestIslands = islands.OrderBy(island => state.Distance(startLocation, island.Loc)).Take(3).ToList();
            return topClosestIslands;
        }

        /// <summary>
        /// Allocate pirate when his target island has been occupied
        /// </summary>
        /// <param name="state"></param>
        /// <param name="pirate"></param>
        /// <param name="islands"></param>
        private void AllocatePirateToNewIsland(IPirateGame state, Pirate pirate, List<Island> islands)
        {
            int islandNumber = GetIslandWithOneEnemy(islands, state.EnemyPirates());
            if (islandNumber == -1)
            {
                bool found = false;
                for (int i = 0; i < islands.Count; i++)
                {
                    if (_pirateToIsland.ContainsValue(islands[i].Id))
                    {
                        continue;
                    }
                    else
                    {
                        found = true;
                        _pirateToIsland[pirate] = islands[i].Id;
                        break;
                    }
                }

                if (!found)
                {
                    _pirateToIsland[pirate] = 2; // Default island
                }
            }
            else
            {

                state.Debug("Found island with 1 enemy");
                _pirateToIsland[pirate] = islandNumber;
            }

        }

        private void AllocatePiretsToIsland(Island islandToBeAllocated, List<Pirate> pirets, int startPiretIndex, int endPiretIndex)
        {
            for (int i = startPiretIndex; i < endPiretIndex; i++)
            {
                _pirateToIsland.Add(pirets[i], islandToBeAllocated.Id);
            }
        }

        /// <summary>
        /// Find island with 1 enemy
        /// </summary>
        /// <param name="islands"></param>
        /// <param name="enemy"></param>
        /// <returns></returns>
        private int GetIslandWithOneEnemy(List<Island> islands, List<Pirate> enemy)
        {
            foreach (Island island in islands)
            {
                int num = enemy.Where(pirate => SameLocation(pirate.Loc, island.Loc)).ToArray().Length;
                if (num == 1)
                {
                    return island.Id;
                }
            }

            return -1;
        }
    }
}
