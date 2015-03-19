using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Juggle_Fest
{
    class Program
    {
        

        class Circuit
        {
            public int number;
            public int hand;
            public int eye;
            public int pizzazz;

            // Constructor
            public Circuit(string num, string h, string e, string p)
            {
                number  = int.Parse(num);
                hand    = int.Parse(h);
                eye     = int.Parse(e);
                pizzazz = int.Parse(p);
            }
        }

        class Juggler
        {
            public int number;
            public int hand;
            public int eye;
            public int pizzazz;

            public List<int> circuitPreference = new List<int>();
            public List<int> circuitScore = new List<int>();

            public Juggler(string num, string h, string e, string p)
            {
                number  = int.Parse(num);
                hand    = int.Parse(h);
                eye     = int.Parse(e);
                pizzazz = int.Parse(p);
            }
        
            public void insertCircuit(string circuitNumber)
            {
                circuitPreference.Add(int.Parse(circuitNumber));
            }

            public void insertScore(int score)
            {
                circuitScore.Add(score);
            }
            

        }

        class Assignment
        {
            public List<Circuit> circuit = new List<Circuit>();
            public List<Juggler> juggler = new List<Juggler>();

        }

        static void Main(string[] args)
        {
            string line; // Each line of file
            int circuitNumber = 0; // Number of circuit
            int jugglerNumber = 0; // Number of juggler
            List<Circuit> circuitList = new List<Circuit>(); // List of circuit
            List<Juggler> jugglerList = new List<Juggler>(); // List of juggler


            // Read the file and display it line by line.
            System.IO.StreamReader input = new System.IO.StreamReader("D:\\Work and Project\\GitHub\\Juggle-Fest\\input.txt");

            // Loop through each line in input file to get circuit and juggler data
            while ((line = input.ReadLine()) != null)
            {
                Console.WriteLine("Reading: " + line);
                if (line.StartsWith("C"))
                {
                    // Add circuit
                    addCircuit(line, circuitList);

                    // Increment circuit number
                    circuitNumber++;
                }
                else if (line.StartsWith("J"))
                {
                    // Add Juggler
                    addJuggler(line, jugglerList, circuitNumber);

                    // Increment juggler number
                    jugglerNumber++;
                }
                else
                {
                    // Skip... reserved for future need...
                }
            }

            // Calculate each juggler's circuit
            calculateJugglerCircuit(jugglerList, circuitList);

            // Choose the best each circuit
            Assignment juggleFest = new Assignment();

            juggleFest = chooseBest(jugglerList, circuitList, jugglerNumber, circuitNumber);


            // PRINTING DEBUG
            //.WriteLine("Number of circuit: " + circuitNumber);
            //Console.WriteLine("Number of juggler: " + jugglernumber);

            // Print all the circuit list
            //foreach (Circuit c in circuitList)
            //{
            //    Console.WriteLine("Circuit " + c.number + "\nHand: " + c.hand + "Eye: " + c.eye + "Pizzazz: " + c.pizzazz);
            //}

            

            // Try print juggler #2 data
            //Console.WriteLine("Juggler #   : " + jugglerList[1].number);
            //Console.WriteLine("Juggler hand: " + jugglerList[1].hand);
            //Console.WriteLine("Juggler eye : " + jugglerList[1].eye);
            //Console.WriteLine("Juggler pizz: " + jugglerList[1].pizzazz);
            //Console.WriteLine("circuit {0}: {1}", jugglerList[1].circuitPreference[0], jugglerList[1].circuitScore[0]);
            //Console.WriteLine("circuit {0}: {1}", jugglerList[1].circuitPreference[1], jugglerList[1].circuitScore[1]);
            //Console.WriteLine("circuit {0}: {1}", jugglerList[1].circuitPreference[2], jugglerList[1].circuitScore[2]);


            Console.ReadLine();
        }

        private static Assignment chooseBest(List<Juggler> jugglerList, List<Circuit> circuitList, int jugglerNumber, int circuitNumber)
        {
            int bestOf = jugglerNumber / circuitNumber;

            // Each circuit
            foreach (Circuit circuit  in circuitList)
            {
                // Find the best of (according to juggler and circuit number)
                foreach (Juggler juggler in jugglerList)
                { 
                    foreach (int jugglerCircuit in juggler.circuitPreference)
                    {
                        // Found
                        //if (circuit.number == jugglerCircuit)

                    }
                }
            }


            // ntaran dehhh
            return new Assignment();
        }

        private static void calculateJugglerCircuit(List<Juggler> jugglerList, List<Circuit> circuitList)
        {
            // Each juggler
            for (int person = 0; person < jugglerList.Count(); person++)
            {
                // Each circuit
                for (int circuitIndex = 0; circuitIndex < jugglerList[person].circuitPreference.Count(); circuitIndex++)
                {
                    int handScore;
                    int eyeScore;
                    int pizzazzScore;

                    // Get circuit preferences index
                    int circuitNumber = jugglerList[person].circuitPreference[circuitIndex];

                    // Hand
                    handScore = jugglerList[person].hand * circuitList[circuitNumber].hand;

                    // Eye
                    eyeScore = jugglerList[person].eye * circuitList[circuitNumber].eye;

                    // Pizzazz
                    pizzazzScore = jugglerList[person].pizzazz * circuitList[circuitNumber].pizzazz;

                    // Add new score to this circuit in juggler's data
                    jugglerList[person].insertScore(handScore + eyeScore + pizzazzScore);
                }
            }
        }

        private static void addJuggler(string line, List<Juggler> jugglerList, int circuitNumber)
        {
            // Construct regex for juggler line
            Regex jugglerRegex = buildJugglerRegex(circuitNumber);
            Match result = jugglerRegex.Match(line);

            // Construct new juggler (only fill basic data)
            Juggler newJuggler = new Juggler(result.Groups[1].ToString(), result.Groups[2].ToString(), result.Groups[3].ToString(), result.Groups[4].ToString());

            // Insert circuit(s) to new juggler
            GroupCollection jugglerParsed = result.Groups;
            for (int group = 5; group < jugglerParsed.Count; group++)
                newJuggler.insertCircuit(jugglerParsed[group].ToString());

            // Push new juggler data to juggler list
            jugglerList.Add(newJuggler);
        }

        private static Regex buildJugglerRegex(int circuitNumber)
        {
            string regexString;                    // Regex for juggler line
            string circuitPreferenceRegex  = null; // Regex depend on how many circuit

            // Append regex string of circuit
            for (int circuit = 0; circuit < circuitNumber; circuit++)
                circuitPreferenceRegex = string.Format(circuitPreferenceRegex + "C([0-9]*),?");

            // Construct regex for juggler
            regexString = string.Format(@"J J([0-9]*) H:([0-9]*) E:([0-9]*) P:([0-9])* " + circuitPreferenceRegex);
            Regex jugglerRegex = new Regex(regexString);

            // Pass back regex
            return jugglerRegex;
        }

        private static void addCircuit(string line, List<Circuit> circuitList)
        {
            // Construct regex for circuit
            Regex circuitRegex = new Regex(@"^C C([0-9]*) H:([0-9]*) E:([0-9]*) P:([0-9]*)$");
            Match result = circuitRegex.Match(line);

            // Create new circuit object
            Circuit newCircuit = new Circuit(result.Groups[1].ToString(), result.Groups[2].ToString(), result.Groups[3].ToString(), result.Groups[4].ToString());

            // // Push new circuit data to circuit list
            circuitList.Add(newCircuit);
        }


    }
}
