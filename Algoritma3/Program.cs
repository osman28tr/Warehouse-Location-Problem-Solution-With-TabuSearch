using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algoritma3
{
    public class Program
    {
        static int tabuSize = 10;
        static int maxIterations = 100;
        static Random random = new Random();
        static void Main(string[] args)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string binDebugPath = Path.Combine("bin", "Debug");
            string dosyaYolu = currentDirectory.Replace(binDebugPath, "");
            dosyaYolu = Path.Combine(dosyaYolu, "Datas", "wl_16_1.txt");

            StreamReader sr = new StreamReader(dosyaYolu);

            string line = sr.ReadLine();
            string[] words = line.Split();
            int depoSayisi = int.Parse(words[0]);
            int musteriSayisi = int.Parse(words[1]);

            int[] depoKapasiteleri = new int[depoSayisi];
            double[] depoKurulumMaliyetleri = new double[depoSayisi];
            int[] musteriTalepleri = new int[musteriSayisi];
            double[][] musteriMaliyetleri = new double[musteriSayisi][];

            for (int i = 0; i < depoSayisi; i++)
            {
                line = sr.ReadLine();
                words = line.Split();
                depoKapasiteleri[i] = int.Parse(words[0]);
                depoKurulumMaliyetleri[i] = double.Parse(words[1], System.Globalization.CultureInfo.InvariantCulture);
            }

            for (int i = 0; i < musteriSayisi; i++)
            {
                line = sr.ReadLine();
                words = line.Split();
                musteriTalepleri[i] = int.Parse(words[0]);
                musteriMaliyetleri[i] = new double[depoSayisi];

                line = sr.ReadLine();
                words = line.Split();
                for (int j = 0; j < depoSayisi; j++)
                {
                    musteriMaliyetleri[i][j] = double.Parse(words[j], System.Globalization.CultureInfo.InvariantCulture);
                }
            }

            sr.Close();

            int[] solution = TabuSearch(depoSayisi, musteriSayisi, depoKurulumMaliyetleri, depoKapasiteleri, musteriTalepleri, musteriMaliyetleri);

            double optimalMaliyet = 1 / CalculateFitness(solution, depoKurulumMaliyetleri, depoKapasiteleri, musteriTalepleri, musteriMaliyetleri);


            Console.Write(optimalMaliyet);
            Console.WriteLine();
            for (int i = 0; i < solution.Length; i++)
            {
                Console.Write("{0} ", solution[i]);
            }
            Console.ReadLine();
        }
        static int[] GenerateRandomSolution(int numDepots, int numCustomers)
        {
            int[] solution = new int[numCustomers];
            for (int i = 0; i < numCustomers; i++)
            {
                solution[i] = random.Next(numDepots);
            }
            return solution;
        }

        static double CalculateFitness(int[] solution, double[] depotCosts, int[] depotCapacities, int[] customerDemands, double[][] customerCosts)
        {
            int numDepots = depotCosts.Length;
            int numCustomers = customerDemands.Length;

            int[] depotLoad = new int[numDepots];

            double totalCost = 0;
            for (int i = 0; i < numCustomers; i++)
            {
                int depotIndex = solution[i];
                depotLoad[depotIndex] += customerDemands[i];
                totalCost += customerCosts[i][depotIndex];
            }

            for (int i = 0; i < numDepots; i++)
            {
                totalCost += depotCosts[i] * Math.Ceiling((double)depotLoad[i] / depotCapacities[i]);
            }

            return 1 / totalCost;
        }

        static int[] TabuSearch(int numDepots, int numCustomers, double[] depotCosts, int[] depotCapacities, int[] customerDemands, double[][] customerCosts)
        {
            int[] currentSolution = GenerateRandomSolution(numDepots, numCustomers);
            int[] bestSolution = (int[])currentSolution.Clone();
            double currentFitness = CalculateFitness(currentSolution, depotCosts, depotCapacities, customerDemands, customerCosts);
            double bestFitness = currentFitness;

            Queue<int[]> tabuList = new Queue<int[]>();
            if (numCustomers == 500)
            {
                maxIterations = 10;
            }

            for (int iteration = 0; iteration < maxIterations; iteration++)
            {
                int[] bestNeighbor = null;
                double bestNeighborFitness = double.MinValue;

                for (int i = 0; i < numCustomers; i++)
                {
                    for (int j = 0; j < numDepots; j++)
                    {
                        int[] neighbor = (int[])currentSolution.Clone();
                        neighbor[i] = j;

                        if (!IsTabu(neighbor, tabuList) && CalculateFitness(neighbor, depotCosts, depotCapacities, customerDemands, customerCosts) > bestNeighborFitness)
                        {
                            bestNeighbor = neighbor;
                            bestNeighborFitness = CalculateFitness(neighbor, depotCosts, depotCapacities, customerDemands, customerCosts);
                        }
                    }
                }

                if (bestNeighbor == null)
                {
                    break;
                }

                tabuList.Enqueue(bestNeighbor);
                if (tabuList.Count > tabuSize)
                {
                    tabuList.Dequeue();
                }

                currentSolution = bestNeighbor;
                currentFitness = bestNeighborFitness;

                if (currentFitness > bestFitness)
                {
                    bestSolution = (int[])currentSolution.Clone();
                    bestFitness = currentFitness;
                }
            }

            return bestSolution;
        }

        static bool IsTabu(int[] solution, Queue<int[]> tabuList)
        {
            foreach (int[] tabuSolution in tabuList)
            {
                bool isEqual = true;
                for (int i = 0; i < solution.Length; i++)
                {
                    if (solution[i] != tabuSolution[i])
                    {
                        isEqual = false;
                        break;
                    }
                }

                if (isEqual)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
