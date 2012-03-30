namespace NewcastleLibrary.Data
{
    /// <summary>Branch</summary>
    public class Library
    {
        #region Private Members
        private int code = 0;
        private string name = "";
        private double distance = 0.0;
        private int borrowers = 0;
        private string url = "";
        #endregion

        #region Public Properties
        /// <summary>Code</summary>
        public int Code { get { return code; } set { code = value; } }

        /// <summary>Name</summary>
        public string Name { get { return name; } set { name = value; } }

        /// <summary>Distance</summary>
        public double Distance { get { return distance; } set { distance = value; } }

        /// <summary>Borrowers</summary>
        public int Borrowers { get { return borrowers; } set { borrowers = value; } }

        /// <summary>Url</summary>
        public string Url { get { return url; } set { url = value; } }
        #endregion
    }
}
