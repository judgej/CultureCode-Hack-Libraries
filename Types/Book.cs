namespace NewcastleLibrary.Data
{
    public class Book
    {
        #region Private Members
        private int code;
        private string isbn;
        private string title;
        private string author;
        private double distance;
        private string image;
        #endregion

        #region Public Properties
        /// <summary>Code</summary>
        public int Code { get { return code; } set { code = value; } }

        /// <summary>ISBN</summary>
        public string ISBN { get { return isbn; } set { isbn = value; } }

        /// <summary>Title</summary>
        public string Title { get { return title; } set { title = value; } }

        /// <summary>Author </summary>
        public string Author { get { return author; } set { author = value; } }

        /// <summary>Distance</summary>
        public double Distance { get { return distance; } set { distance = value; } }

        /// <summary>Image</summary>
        public string Image { get { return image; } set { image = value; } }
        #endregion
    }
}
