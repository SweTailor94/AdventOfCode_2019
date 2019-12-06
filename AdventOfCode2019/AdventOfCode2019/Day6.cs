using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019
{
    class Day6
    {
        Dictionary<string, List<Orbiter>> DirectlyOrbits;
        private Orbiter _com;
        private Orbiter _you;
        private Orbiter _santa;

        public void ParseInput(string filename)
        {
            var lines = File.ReadAllLines(filename);
            DirectlyOrbits = new Dictionary<string, List<Orbiter>>();
            foreach (var line in lines)
            {
                var names = line.Split(')');
                if (names.Length != 2) throw new Exception($"Wrong kind of input: {line}");
                var orbiter = new Orbiter(names[0], names[1]);
                if (names[1] == "YOU") _you = orbiter;
                if (names[1] == "SAN") _santa = orbiter;

                if (DirectlyOrbits.ContainsKey(names[0]))
                {
                    DirectlyOrbits[names[0]].Add(orbiter);
                }
                else
                {
                    DirectlyOrbits.Add(names[0], new List<Orbiter>() { orbiter });
                }
            }

            _com = new Orbiter(null, "COM");
            int numNodes = 0;
            BuildTree(_com, ref numNodes);

            Console.WriteLine($"lines {lines.Length} ? Number of nodes {numNodes} ");
        }


        private void BuildTree(Orbiter o, ref int count)
        {
            count++;
            if (DirectlyOrbits.ContainsKey(o.Name))
            {
                o.InOrbit = DirectlyOrbits[o.Name];
                foreach (var orbiter in o.InOrbit)
                {
                    BuildTree(orbiter, ref count);
                }
            }
        }


        public int GetChecksum()
        {
            var checsumer = new ChecksumVisitor();
            _com.Accept(checsumer);

            return checsumer.CheckSum;
        }

        public int MinOrbitChanges()
        {
            var visitor = new OrbitChangeVisitor();
            _com.Accept(visitor);



            return visitor.OrbitalChanges;
        }

    }



    public abstract class Visitor
    {
        protected bool isDone = false;
        public bool IsDone => isDone;
        abstract public void VisitStart(Orbiter o);
        abstract public void VisitEnd(Orbiter o);
    }

    public class OrbitChangeVisitor : Visitor
    {
        private bool Done = false;
        private bool IsBacktrackingYou = false;
        private bool IsBacktrackingSanta = false;

        public int OrbitalChanges = 0;

        public override void VisitStart(Orbiter o)
        {
            IsBacktrackingSanta = false;
            IsBacktrackingYou = false;
        }

        public override void VisitEnd(Orbiter o)
        {
            if (Done) return;
            if (o.IsLeaf)
            {
                if (o.Name == "SAN")
                {
                    IsBacktrackingSanta = true;
                    o.LeadsToSanta = true;
                }
                else if (o.Name.Equals("YOU"))
                {
                    IsBacktrackingYou = true;
                    o.LeadsToYou = true;
                }
            }
            else
            {

                IsBacktrackingSanta = o.InOrbit.Any(x => x.LeadsToSanta) || IsBacktrackingSanta;
                IsBacktrackingYou = o.InOrbit.Any(x => x.LeadsToYou) || IsBacktrackingYou;

                if (IsBacktrackingYou && IsBacktrackingSanta)
                {
                    Done = true;
                }
                else if (IsBacktrackingSanta)
                {
                    o.LeadsToSanta = true;
                    OrbitalChanges++;
                }
                else if (IsBacktrackingYou)
                {
                    o.LeadsToYou = true;
                    OrbitalChanges++;


                }
            }
        }
    }

    public class ChecksumVisitor : Visitor
    {
        public int CheckSum = 0;
        private Orbiter lastNode;
        public override void VisitStart(Orbiter o)
        {

        }

        public override void VisitEnd(Orbiter o)
        {
            if (o.IsLeaf)
                o.TotalOrbiters = 0;
            else
                o.TotalOrbiters = o.InOrbit.Sum((orbiter => orbiter.TotalOrbiters + 1));
            CheckSum += o.TotalOrbiters;
        }
    }


    public class Orbiter : IComparable<Orbiter>
    {
        public string Name;
        public string Orbits;

        public int TotalOrbiters;

        public int DistToSan = 0;
        public int DistToYou = 0;

        public List<Orbiter> InOrbit = new List<Orbiter>();

        public Orbiter(string orbits, string name)
        {
            this.Orbits = orbits;
            this.Name = name;
            TotalOrbiters = 0;
        }

        public bool IsLeaf => InOrbit.Count == 0;
        public bool LeadsToYou { get; set; }
        public bool LeadsToSanta { get; set; }

        public void Accept(Visitor v)
        {
            v.VisitStart(this);


            foreach (var orb in InOrbit)
            {
                if (!v.IsDone) orb.Accept(v);
            }
            v.VisitEnd(this);
        }

        public int CompareTo(Orbiter other)
        {
            return this.Orbits.CompareTo(other.Orbits);
        }

        public override string ToString()
        {
            return $"{Orbits}){Name}";
        }
    }


}
