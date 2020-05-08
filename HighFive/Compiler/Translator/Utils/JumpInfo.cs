using H5.Contract;
using System.Text;

namespace H5.Translator
{
    public class JumpInfo : IJumpInfo
    {
        public JumpInfo(StringBuilder output, int position, bool @break)
        {
            this.Output = output;
            this.Position = position;
            this.Break = @break;
        }

        public StringBuilder Output
        {
            get;
            set;
        }

        public int Position
        {
            get;
            set;
        }

        public bool Break
        {
            get;
            set;
        }
    }
}