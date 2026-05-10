using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.RuntimeTests
{
    [TestClass]
    public class lcsulongTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task lcsulong_Run()
        {
            var code = """
// Original: External/dotnet_runtime/src/tests/JIT/Methodical/int64/arrays/lcs_ulong.cs
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

// namespace JitTest_lcs_ulong_arrays_cs
{
    public class LCS
    {
        private const int RANK = 4;

        private static String buildLCS(ulong[,,,] b, char[] X, ulong[] ind)
        {
            for (ulong i = 0; i < RANK; i++)
                if (ind[i] == 0) return "";

            ulong L = b[ind[0], ind[1], ind[2], ind[3]];
            if (L == RANK)
            {
                for (ulong i = 0; i < RANK; i++)
                    ind[i]--;
                ulong idx = ind[0];
                return buildLCS(b, X, ind) + X[idx];
            }
            if (L >= 0 && L < RANK)
            {
                ind[L]--;
                return buildLCS(b, X, ind);
            }
            throw new Exception();
        }

        private static void findLCS(ulong[,,,] c, ulong[,,,] b, char[][] seq, ulong[] len)
        {
            ulong[] ind = new ulong[RANK];
            for (ind[0] = 1; ind[0] < len[0]; ind[0]++)
            {
                for (ind[1] = 1; ind[1] < len[1]; ind[1]++)
                {
                    for (ind[2] = 1; ind[2] < len[2]; ind[2]++)
                    {
                        for (ind[3] = 1; ind[3] < len[3]; ind[3]++)
                        {
                            bool eqFlag = true;
                            for (ulong i = 1; i < RANK; i++)
                            {
                                if (seq[i][ind[i] - 1] != seq[i - 1][ind[i - 1] - 1])
                                {
                                    eqFlag = false;
                                    break;
                                }
                            }

                            if (eqFlag)
                            {
                                c[ind[0], ind[1], ind[2], ind[3]] =
                                    c[ind[0] - 1, ind[1] - 1, ind[2] - 1, ind[3] - 1] + 1;
                                b[ind[0], ind[1], ind[2], ind[3]] = RANK;
                                continue;
                            }

                            ulong R = 0;
                            ulong M = 0;
                            for (ulong i = 0; i < RANK; i++)
                            {
                                ind[i]--;
                                if (c[ind[0], ind[1], ind[2], ind[3]] + 1 > M)
                                {
                                    R = i + 1;
                                    M = c[ind[0], ind[1], ind[2], ind[3]] + 1;
                                }
                                ind[i]++;
                            }
                            if (R == 0 || M == 0)
                                throw new Exception();

                            c[ind[0], ind[1], ind[2], ind[3]] = checked(M - 1);
                            b[ind[0], ind[1], ind[2], ind[3]] = checked(R - 1);
                        }
                    }
                }
            }
        }

        // [Fact]
        // [OuterLoop]
        public static int TestEntryPoint()
        {
            Console.WriteLine("Test searches for ulongest common subsequence of 4 strings\n\n");
            String[] str = new String[RANK] {
                "The Sun has left his blackness",
                "and has found a fresher morning",
                "and the fair Moon rejoices",
                "in the clear and cloudless night"
            };

            ulong[] len = new ulong[RANK];
            char[][] seq = new char[RANK][];
            for (ulong i = 0; i < RANK; i++)
            {
                len[i] = checked((ulong)(str[i].Length + 1));
                seq[i] = str[i].ToCharArray();
            }

            ulong[,,,] c = new ulong[(int)len[0], (int)len[1], (int)len[2], (int)len[3]];
            ulong[,,,] b = new ulong[(int)len[0], (int)len[1], (int)len[2], (int)len[3]];

            findLCS(c, b, seq, len);

            for (ulong i = 0; i < RANK; i++)
                len[i]--;

            if ("n ha  es" == buildLCS(b, seq[0], len))
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
