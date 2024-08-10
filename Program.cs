using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Lab3

{

    class Program
    {
        
        static int compute_element(int[,] A, int[,] B, int row, int column)
        {
            int rez = 0;
            for (int i=0; i<A.GetLength(1); i++)
            {
                rez = rez + (A[row,i] * B[i,column]);
            }
            return rez;
        }

        static void pararel_task_row(int [,]A,int [,] B, int[,] C, int idxThread,int nrThreads,int size)
        {

            int beginIdx = (idxThread * size) / nrThreads;
            int endIdx = ((idxThread +1) * size) / nrThreads;
            int n = B.GetLength(1) ;

            for (int i=beginIdx; i<endIdx; i++)
            {
                C[i/n, i % n] = compute_element(A, B, i/n,i%n);
            }
            
        }

        static void pararel_task_col(int[,] A, int[,] B, int[,] C, int idxThread, int nrThreads, int size)
        {

            int beginIdx = (idxThread * size) / nrThreads;
            int endIdx = ((idxThread + 1) * size) / nrThreads;
            int m = A.GetLength(0);


            for (int i = beginIdx; i < endIdx; i++)
            {       
                C[i % m, i / m] = compute_element(A, B, i % m, i / m);
            }

        }

        static void pararel_task_k(int[,] A, int[,] B, int[,] C, int idxThread, int nrThreads, int size)
        {

            int n = B.GetLength(1);

            for (int i = idxThread; i < size; i=i+nrThreads)
            {
                C[i / n, i % n] = compute_element(A, B, i / n, i % n);
            }

        }

        static void Main(string[] args)
        {

            Random rnd = new Random();
            int m=30;
            int k= 50;                  // A -> m lines, k colums; B -> k lines n colums
            int n= 50;
            int nr_threads = 4;
            List<Thread> threads = new List<Thread>();

            int[,] A = new int[m, k];
            int[,] B = new int[k, n];
            int[,] C = new int[m, n];


            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    A[i, j] = 1;
                }
            }

            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    B[i, j] = 1;
                }
            }


           // Stopwatch watch = new Stopwatch();
           // watch.Start();

            for (int i = 0; i < nr_threads; i++)
            {
                int index = i;
                
                ThreadPool.QueueUserWorkItem(new WaitCallback(state =>
                {
                    
                    pararel_task_row(A,B,C,index,nr_threads,m*n);

                }));

                //Thread thread = new Thread(() => pararel_task_row(A,B,C, index, nr_threads, m*n));
                //thread.Start();
                //threads.Add(thread);
                
            }

           

            //for (int i = 0; i < nr_threads; i++)
            //{
            //    threads[i].Join();
            //}

            //watch.Stop();
            //long elapsedTicks = watch.ElapsedTicks;
            //double mircosec = (double)elapsedTicks / Stopwatch.Frequency * 1000000;
            

            Console.WriteLine("A:");
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < k; j++)
                         {
                             Console.Write(A[i, j]);
                    Console.Write(" ");
                         }
                         Console.WriteLine();
                     }

                     Console.WriteLine("B:");
                     for (int i = 0; i < k; i++)
                     {
                         for (int j = 0; j < n; j++)
                         {
                             Console.Write(B[i, j]);
                    Console.Write(" ");
                        }
                         Console.WriteLine();
                     }

                     Console.WriteLine("C:");
                     for (int i = 0; i < m; i++)
                     {
                        for (int j = 0; j < n; j++)
                        {
                            Console.Write(C[i,j]);
                             Console.Write(" ");
                         }
                          Console.WriteLine();
                     }


            //Console.WriteLine($"Execution Time: {mircosec} mircroseconds");
            Console.WriteLine("Done !");
            Console.ReadLine();

        }
    }

}
