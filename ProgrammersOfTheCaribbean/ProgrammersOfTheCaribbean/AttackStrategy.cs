using System;
using System.Collections.Generic;
using System.Linq;
using Pirates;

namespace ProgrammersOfTheCaribbean
{
    public class AttackStrategy
    {
        private OccupiedCenterStrategy occupiedStrategy = new OccupiedCenterStrategy();

        public Dictionary<Pirate, Location> DoTurn(IPirateGame state)
        {
            List<List<Pirate>> enemyGroups = FindEnemyGroups(state);
            List<Pirate> myPirates = new List<Pirate>(state.MyPirates());
            Dictionary<Pirate, Location> pirateToLocation = Attack(myPirates, enemyGroups);

            pirateToLocation.Concat(occupiedStrategy.DoTurn(state, myPirates, state.Islands()));
            return pirateToLocation;
        }

        private Dictionary<Pirate, Location> Attack(List<Pirate> myPirates, List<List<Pirate>> enemyGroups)
        {
            Dictionary<Pirate, Location> pirateToLocation = new Dictionary<Pirate, Location>();
            Location location;
            if (enemyGroups[0].Count < 2)
            {
                location = new Location(enemyGroups[0][0].Loc);
            }
            else
            {
                location = new Location(enemyGroups[0][0].InitialLocation);
            }

            myPirates.ForEach(pirate =>
            {
                pirateToLocation.Add(pirate, location);
            });

            return pirateToLocation;
        }

        private List<List<Pirate>> FindEnemyGroups(IPirateGame state)
        {
            List<List<Pirate>> enemyGroups = new List<List<Pirate>>();
            List<Pirate> enemyPirates = state.EnemyPirates();
            for (int i = 0; i < enemyPirates.Count; i++)
            {
                enemyGroups.Add(new List<Pirate>());
                for (int j = i; j < enemyPirates.Count; j++)
                {
                    if (state.Distance(enemyPirates[i], enemyPirates[j]) < 5)
                    {
                        enemyGroups[i].Add(enemyPirates[j]);
                        enemyPirates.Remove(enemyPirates[j]);
                    }
                }
            }

            enemyGroups.Sort(new CompareList());
            return enemyGroups;
        }
    }

    class CompareList : IComparer<List<Pirate>>
    {
        public int Compare(List<Pirate> x, List<Pirate> y)
        {
            if (x.Count == y.Count)
            {
                return 0;
            }
            else if (x.Count > y.Count)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }
}
