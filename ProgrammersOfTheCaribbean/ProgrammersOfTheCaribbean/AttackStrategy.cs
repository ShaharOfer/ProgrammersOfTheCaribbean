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
            state.Debug("Attack");
            List<List<Pirate>> enemyGroups = FindEnemyGroups(state);
            Dictionary<Pirate, Location> pirateToLocation = Attack(state, myPirates, enemyGroups);

            pirateToLocation.Concat(_occupiedStrategy.DoTurn(state, myPirates, islands));
            return pirateToLocation;
        }

        private Dictionary<Pirate, Location> Attack(IPirateGame state, List<Pirate> myPirates, List<List<Pirate>> enemyGroups)
        {
            Dictionary<Pirate, Location> pirateToLocation = new Dictionary<Pirate, Location>();

            if (enemyGroups.Count > 0 && myPirates.Count >= 2)
            {
                Location location = new Location(2, 24);

                foreach (var group in enemyGroups)
                {
                    if (group.Count < 2)
                    {
                        if (!isTeam(state, myPirates))
                        {
                            int col = Math.Abs((myPirates[0].Loc.Col + myPirates[1].Loc.Col)/2);
                            int row = Math.Abs((myPirates[0].Loc.Row + myPirates[1].Loc.Row)/2);
                            location = new Location(col, row);
                        }
                        if (state.Distance(myPirates[0].Loc, group[0].Loc) < state.Distance(myPirates[0].Loc, location))
                        {
                            location = group[0].Loc;
                        }
                    }
                }

                for (int i = 0; i < AttackersSize; i++)
                {
                    pirateToLocation.Add(myPirates[0], location);
                    myPirates.Remove(myPirates[0]);
                }
                /*if (enemyGroups[0].Count < 2)
                {
                    state.Debug($"enemy location - {enemyGroups[0][0].Loc}");
                    location = enemyGroups[0][0].Loc;
                }*/
            }

            return pirateToLocation;
        }

        private bool isTeam(IPirateGame state, List<Pirate> myPirates)
        {
            return (state.Distance(myPirates[0], myPirates[1]) < 3);
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
                    if (state.Distance(enemyPirates[i], enemyPirates[j]) < 3)
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
