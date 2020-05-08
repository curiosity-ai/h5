namespace HighFive.Contract
{
    public class HighFiveResourceInfoPart
    {
        /// <summary>
        /// Part name
        /// </summary>
        public string Name
        {
            get; set;
        }

        /// <summary>
        /// Assembly the part is related to
        /// </summary>
        public string Assembly
        {
            get; set;
        }

        /// <summary>
        /// The name the part is stored in resources
        /// </summary>
        public string ResourceName
        {
            get; set;
        }

    }

    public class HighFiveResourceInfo
    {
        public string FileName
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public string Path
        {
            get; set;
        }

        public HighFiveResourceInfoPart[] Parts
        {
            get; set;
        }
    }
}