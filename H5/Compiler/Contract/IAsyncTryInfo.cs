using System;

namespace H5.Contract
{
    public interface IAsyncTryInfo
    {
        System.Collections.Generic.List<Tuple<string, string, int, int>> CatchBlocks { get; }

        int EndStep { get; set; }

        int FinallyStep { get; set; }

        int StartStep { get; set; }
    }
}