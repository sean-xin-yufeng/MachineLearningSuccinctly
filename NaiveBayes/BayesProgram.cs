﻿using System;

namespace NaiveBayes
{
    class BayesProgram
    {
        /// <summary>
        /// 朴素贝叶斯分类，用于非数字的绝对数据
        /// </summary>
        /// <param name="args">系统变量</param>
        static void Main(string[] args)
        {
            Console.WriteLine("\nBegin Naive Bayes classification demo");
            Console.WriteLine("Goal is to predict (liberal/conservative) from job, " +
            "sex and income\n");
            string[][] rawData = new string[30][];
            rawData[0] = new string[] { "analyst", "male", "high", "conservative" };
            rawData[1] = new string[] { "barista", "female", "low", "liberal" };
            rawData[2] = new string[] { "cook", "male", "medium", "conservative" };
            rawData[3] = new string[] { "doctor", "female", "medium", "conservative" };
            rawData[4] = new string[] { "analyst", "female", "low", "liberal" };
            rawData[5] = new string[] { "doctor", "male", "medium", "conservative" };
            rawData[6] = new string[] { "analyst", "male", "medium", "conservative" };
            rawData[7] = new string[] { "cook", "female", "low", "liberal" };
            rawData[8] = new string[] { "doctor", "female", "medium", "liberal" };
            rawData[9] = new string[] { "cook", "female", "low", "liberal" };
            rawData[10] = new string[] { "doctor", "male", "medium", "conservative" };
            rawData[11] = new string[] { "cook", "female", "high", "liberal" };
            rawData[12] = new string[] { "barista", "female", "medium", "liberal" };
            rawData[13] = new string[] { "analyst", "male", "low", "liberal" };
            rawData[14] = new string[] { "doctor", "female", "high", "conservative" };
            rawData[15] = new string[] { "barista", "female", "medium", "conservative" };
            rawData[16] = new string[] { "doctor", "male", "medium", "conservative" };
            rawData[17] = new string[] { "barista", "male", "high", "conservative" };
            rawData[18] = new string[] { "doctor", "female", "medium", "liberal" };
            rawData[19] = new string[] { "analyst", "male", "low", "liberal" };
            rawData[20] = new string[] { "doctor", "male", "medium", "conservative" };
            rawData[21] = new string[] { "cook", "male", "medium", "conservative" };
            rawData[22] = new string[] { "doctor", "female", "high", "conservative" };
            rawData[23] = new string[] { "analyst", "male", "high", "conservative" };
            rawData[24] = new string[] { "barista", "female", "medium", "liberal" };
            rawData[25] = new string[] { "doctor", "male", "medium", "conservative" };
            rawData[26] = new string[] { "analyst", "female", "medium", "conservative" };
            rawData[27] = new string[] { "analyst", "male", "medium", "conservative" };
            rawData[28] = new string[] { "doctor", "female", "medium", "liberal" };
            rawData[29] = new string[] { "barista", "male", "medium", "conservative" };
            Console.WriteLine("The raw data is: \n");
            ShowData(rawData, 5, true);

            Console.WriteLine("Splitting data into 80%-20% train and test sets");
            string[][] trainData;
            string[][] testData;
            MakeTrainTest(rawData, 15, out trainData, out testData); // seed = 15 is nice
            Console.WriteLine("Done \n");
            Console.WriteLine("Training data: \n");
            ShowData(trainData, 5, true);
            Console.WriteLine("Test data: \n");
            ShowData(testData, 5, true);

            Console.WriteLine("Creating Naive Bayes classifier object");
            Console.WriteLine("Training classifier using training data");
            BayesClassifier bc = new BayesClassifier();
            bc.Train(trainData); // compute key count data structures
            Console.WriteLine("Done \n");

            double trainAccuracy = bc.Accuracy(trainData);
            Console.WriteLine("Accuracy of model on train data = " +
            trainAccuracy.ToString("F4"));
            double testAccuracy = bc.Accuracy(testData);
            Console.WriteLine("Accuracy of model on test data = " +
            testAccuracy.ToString("F4"));
            Console.WriteLine("\nPredicting politics for job = barista, sex = female, " + "income = medium \n");
            string[] features = new string[] { "barista", "female", "medium" };
            string liberal = "liberal";
            double pLiberal = bc.Probability(liberal, features);
            Console.WriteLine("Probability of liberal = " +
            pLiberal.ToString("F4"));
            string conservative = "conservative";
            double pConservative = bc.Probability(conservative, features);
            Console.WriteLine("Probability of conservative = " +
            pConservative.ToString("F4"));
            Console.WriteLine("\nEnd Naive Bayes classification demo\n");
            Console.ReadLine();
        }
        /// <summary>
        /// 将所有数据随机划分为训练数据和测试数据
        /// </summary>
        /// <param name="allData">全部数据</param>
        /// <param name="seed">随机数种子</param>
        /// <param name="trainData">返回训练数据</param>
        /// <param name="testData">返回测试数据</param>
        static void MakeTrainTest(string[][] allData, int seed, out string[][] trainData, out string[][] testData)
        {
            Random rnd = new Random(seed);
            int totRows = allData.Length;
            int numTrainRows = (int)(totRows * 0.80);
            int numTestRows = totRows - numTrainRows;
            trainData = new string[numTrainRows][];
            testData = new string[numTestRows][];
            string[][] copy = new string[allData.Length][]; // ref copy of all data
            for (int i = 0; i < copy.Length; ++i)
                copy[i] = allData[i];
            for (int i = 0; i < copy.Length; ++i) // scramble order
            {
                int r = rnd.Next(i, copy.Length);
                string[] tmp = copy[r];
                copy[r] = copy[i];
                copy[i] = tmp;
            }
            for (int i = 0; i < numTrainRows; ++i)
                trainData[i] = copy[i];
            for (int i = 0; i < numTestRows; ++i)
                testData[i] = copy[i + numTrainRows];
        }
        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="rawData">用于显示的二维数据</param>
        /// <param name="numRows">显示行数</param>
        /// <param name="indices">是否目录</param>
        static void ShowData(string[][] rawData, int numRows, bool indices)
        {
            for (int i = 0; i < numRows; ++i)
            {
                if (indices == true)
                    Console.Write("[" + i.ToString().PadLeft(2) + "] ");
                for (int j = 0; j < rawData[i].Length; ++j)
                {
                    string s = rawData[i][j];
                    Console.Write(s.PadLeft(14) + " ");
                }
                Console.WriteLine("");
            }
            if (numRows != rawData.Length - 1)
                Console.WriteLine(". . .");
            int lastRow = rawData.Length - 1;
            if (indices == true)
                Console.Write("[" + lastRow.ToString().PadLeft(2) + "] ");
            for (int j = 0; j < rawData[lastRow].Length; ++j)
            {
                string s = rawData[lastRow][j];
                Console.Write(s.PadLeft(14) + " ");
            }
            Console.WriteLine("\n");
        }
        /// <summary>
        /// 静态方法
        /// 用于把数字数据划分为非数字数据
        /// </summary>
        /// <param name="data">原数字数据数组</param>
        /// <param name="numBins">要划分的组数</param>
        /// <returns>返回分组情况</returns>
        static double[] MakeIntervals(double[] data, int numBins) // bin numeric data
        {
            double max = data[0]; // find min & max
            double min = data[0];
            for (int i = 0; i < data.Length; ++i)
            {
                if (data[i] < min) min = data[i];
                if (data[i] > max) max = data[i];
            }
            double width = (max - min) / numBins; // compute width
            double[] intervals = new double[numBins * 2]; // intervals
            intervals[0] = min;
            intervals[1] = min + width;
            for (int i = 2; i < intervals.Length - 1; i += 2)
            {
                intervals[i] = intervals[i - 1];
                intervals[i + 1] = intervals[i] + width;
            }
            intervals[0] = double.MinValue; // outliers
            intervals[intervals.Length - 1] = double.MaxValue;
            return intervals;
        }
        /// <summary>
        /// 数字数据分组之后，当新的数据到来的时候，确定属于哪一组
        /// </summary>
        /// <param name="x">待分组数据</param>
        /// <param name="intervals">分组情况，由MakeIntervals返回</param>
        /// <returns>返回该数据属于哪一组</returns>
        static int Bin(double x, double[] intervals)
        {
            for (int i = 0; i < intervals.Length - 1; i += 2)
            {
                if (x >= intervals[i] && x < intervals[i + 1])
                    return i / 2;
            }
            return -1; // error
        }
    }
}
