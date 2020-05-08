using System;

namespace Bridge.Contract
{
    public interface IWriter
    {
        string InlineCode
        {
            get;
            set;
        }

        System.Text.StringBuilder Output
        {
            get;
            set;
        }

        bool IsNewLine
        {
            get;
            set;
        }

        Action Callback
        {
            get;
            set;
        }

        string ThisArg
        {
            get;
            set;
        }

        int[] IgnoreRange
        {
            get;
            set;
        }
    }
}