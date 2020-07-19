using System;
using System.Collections.Generic;
using System.Linq;
using Pirates;

namespace ProgrammersOfTheCaribbean
{
    public class MyBot : IPirateBot
    {
        private Dictionary<Pirate, int> _pirateToIsland = new Dictionary<Pirate, int>();
        
        public void DoTurn(IPirateGame state)
        {
            List<Pirate> myPirates = state.AllMyPirates();
            if (_pirateToIsland.Count == 0)
            {
                // Need to allocate pirates to islands
                var targetIslands = state.EnemyIslands();
                targetIslands.AddRange(state.NeutralIslands());
                AllocatePiratesToIslands(state, myPirates, targetIslands);
            }
            // Here the pirates know their target islands

            myPirates.ForEach(pirate =>
            {
                if (!pirate.IsLost)
                {
                    state.Debug($"{pirate.Loc.ToString()}");
                    state.Debug((state.GetIsland(3).Owner).ToString());
                    state.Debug((state.GetIsland(3).Owner == Consts.ME).ToString());

                    // state.Debug((_pirateToIsland[pirate].Owner == Consts.ME).ToString());
                    if (state.GetIsland(_pirateToIsland[pirate]).Owner == Consts.ME)
                    {
                        state.Debug("In");
                        var enemyIslands = state.EnemyIslands();
                        enemyIslands.AddRange(state.NeutralIslands());
                        AllocatePirateToNewIsland(state, pirate, enemyIslands);
                    }


                    var directions = state.GetDirections(pirate, state.GetIsland(_pirateToIsland[pirate]));
                    state.SetSail(pirate, directions[0]);
                }
            });
         
        }

        private bool SameLocation(Location l1, Location l2)
        {
            return l1.Row == l2.Row && l1.Col == l2.Col;
        }

        private void AllocatePiratesToIslands(IPirateGame state, List<Pirate> myPirates, List<Island> islands)
        {
            int piratesIndex = 0;
            var topClosestIslands = GetThreeClosestIslands(state, islands, myPirates[0]);

            int numberOfPirets = (int)Math.Floor(myPirates.Count * 0.2);
            AllocatePiretsToIsland(topClosestIslands[0], myPirates, piratesIndex, piratesIndex + numberOfPirets); ;
            piratesIndex += numberOfPirets;

            numberOfPirets = (int)Math.Floor(myPirates.Count * 0.2);
            AllocatePiretsToIsland(topClosestIslands[2], myPirates, piratesIndex, piratesIndex + numberOfPirets); ;
            piratesIndex += numberOfPirets;

            numberOfPirets = (int)Math.Floor(myPirates.Count * 0.6);
            AllocatePiretsToIsland(topClosestIslands[1], myPirates, piratesIndex, piratesIndex + numberOfPirets); ;
            piratesIndex += numberOfPirets;

        }

        private List<Island> GetThreeClosestIslands(IPirateGame state, List<Island> islands, Pirate pirate)
        {
            var topClosestIslands = islands.OrderBy(island => state.Distance(pirate, island)).Take(3).ToList();
            return topClosestIslands;
        }

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
                    _pirateToIsland[pirate] = 2;
                }
            } 
            else
            {
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

        private int GetIslandWithOneEnemy(List<Island> islands, List<Pirate> enemy)
        {
            foreach(Island island in islands)
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
