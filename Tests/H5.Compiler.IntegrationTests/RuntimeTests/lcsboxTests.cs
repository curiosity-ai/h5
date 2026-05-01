using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class lcsboxTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task lcsbox_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Methodical/Arrays/lcs/lcsbox.cs
using System;

public static class TestFramework
{
    public static void BeginTestCase(string message) { Console.WriteLine("BeginTestCase: " + message); }
    public static void EndTestCase() { Console.WriteLine("EndTestCase"); }
    public static void LogInformation(string message) { Console.WriteLine(message); }
    public static void LogError(string id, string message) { Console.WriteLine("Error " + id + ": " + message); }
    public static void BeginScenario(string message) { Console.WriteLine("BeginScenario: " + message); }
}


// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
// using Xunit;

// namespace JitTest_lcsbox_lcs_cs
{
    public class LCS
    {
        private const int RANK = 4;

        private static String buildLCS(int[,,,] b, char[] X, int[] ind)
        {
            for (int i = 0; i < RANK; i++)
                if (ind[i] == 0) return "";

            int L = (int)b.GetValue(ind);
            if (L == RANK)
            {
                for (int i = 0; i < RANK; i++)
                    ind[i]--;
                int idx = ind[0];
                return buildLCS(b, X, ind) + X[idx];
            }
            if (L >= 0 && L < RANK)
            {
                ind[L]--;
                return buildLCS(b, X, ind);
            }
            throw new Exception();
        }

        private static void findLCS(int[,,,] c, int[,,,] b, char[][] seq, int[] len)
        {
            int[] ind = new int[RANK];
            for (ind[0] = 1; ind[0] < len[0]; ind[0]++)
            {
                for (ind[1] = 1; ind[1] < len[1]; ind[1]++)
                {
                    for (ind[2] = 1; ind[2] < len[2]; ind[2]++)
                    {
                        for (ind[3] = 1; ind[3] < len[3]; ind[3]++)
                        {
                            bool eqFlag = true;
                            for (int i = 1; i < RANK; i++)
                            {
                                if (seq[i][ind[i] - 1] != seq[i - 1][ind[i - 1] - 1])
                                {
                                    eqFlag = false;
                                    break;
                                }
                            }

                            if (eqFlag)
                            {
                                c.SetValue(c[ind[0] - 1, ind[1] - 1, ind[2] - 1, ind[3] - 1] + 1, ind);
                                b.SetValue(RANK, ind);
                                continue;
                            }

                            int R = -1;
                            int M = -1;
                            for (int i = 0; i < RANK; i++)
                            {
                                ind[i]--;
                                int L = (int)c.GetValue(ind);
                                if (L > M)
                                {
                                    R = i;
                                    M = L;
                                }
                                ind[i]++;
                            }
                            if (R < 0 || M < 0)
                                throw new Exception();

                            c.SetValue(M, ind);
                            b.SetValue(R, ind);
                        }
                    }
                }
            }
        }

        // [Fact]
        public static int TestEntryPoint()
        {
            Console.WriteLine("Test searches for longest common subsequence of 4 strings\n\n");
            String[] str = new String[RANK] {
                "abbdccd",
                "abcbdcd",
                "abbcdcd",
                "bdabccd"
            };

            int[] len = new int[RANK];
            char[][] seq = new char[RANK][];
            for (int i = 0; i < RANK; i++)
            {
                len[i] = str[i].Length + 1;
                seq[i] = str[i].ToCharArray();
            }

            int[,,,] c = new int[len[0], len[1], len[2], len[3]];
            int[,,,] b = new int[len[0], len[1], len[2], len[3]];

            findLCS(c, b, seq, len);

            for (int i = 0; i < RANK; i++)
                len[i]--;

            String lcs = buildLCS(b, seq[0], len);
            if ("abccd" == lcs)
            {
                Console.WriteLine("Test passed");
                return 100;
            }
            else
            {
                Console.WriteLine("Test failed.");
                return 0;
            }
        }
    }
// }


public class Program
{
    public static int Main()
    {
        try {
            return LCS.TestEntryPoint();
        } catch(Exception e) {
            Console.WriteLine(e.ToString());
            return 0;
        }
        return 100;
    }
}
""";
            await RunTest(code);
        }
    }
}
