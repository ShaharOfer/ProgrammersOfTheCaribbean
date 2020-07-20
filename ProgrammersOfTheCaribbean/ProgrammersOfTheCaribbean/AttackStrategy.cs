using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Pirates;

namespace ProgrammersOfTheCaribbean
{
    public class AttackStrategy : IStrategy
    {
        public int AttackersSize { get; set; }

        private IStrategy _occupiedStrategy;

        public AttackStrategy(IStrategy occupiedStrategy)
        {
            _occupiedStrategy = occupiedStrategy;
            AttackersSize = 2;
        }

        public Dictionary<Pirate, Location> DoTurn(IPirateGame state, List<Pirate> myPirates, List<Island> islands)
        {
            List<List<Pirate>> enemyGroups = FindEnemyGroups(state);
            Dictionary<Pirate, Location> pirateToLocation = Attack(myPirates, enemyGroups);

            pirateToLocation.Concat(_occupiedStrategy.DoTurn(state, myPirates, state.Islands()));
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

            for (int i = 0; i < AttackersSize; i++)
            {
                pirateToLocation.Add(myPirates[i], location);
                myPirates.Remove(myPirates[i]);
            }

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
