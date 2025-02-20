using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================

namespace Budget
{
    // ====================================================================
    // CLASS: categories
    //        - A collection of category items,
    //        - Read / write to file
    //        - etc
    // ====================================================================
    /// <summary>
    /// Represents a collection of budget categories.It is responsible for managing 
    /// a list of <see cref="Category"/> objects and provides methods for handling budget 
    /// category data, including Read and write to file
    /// </summary>
    public class Categories
    {
        private static String DefaultFileName = "budgetCategories.txt";
        private List<Category> _Cats = new List<Category>();
        private string _FileName;
        private string _DirName;

        // ====================================================================
        // Properties
        // ====================================================================

        /// <summary>
        /// Gets the file name for the categories data file.
        /// </summary>
        /// <value>
        /// The string containing the name of the file used to store expenses.
        /// </value>
        public String FileName { get { return _FileName; } }

        /// <summary>
        /// Gets the directory name where the categories data file is located.
        /// </summary>
        /// <value>
        /// The string containing the name of the file used to store expenses.
        /// </value>
        public String DirName { get { return _DirName; } }

        // ====================================================================
        // Constructor
        // ====================================================================

        /// <summary>
        /// Initializes a new instance of the <see cref="Categories"/> class 
        /// and populates it with default categories. 
        /// </summary>
        public Categories()
        {
            SetCategoriesToDefaults();
        }

        // ====================================================================
        // get a specific category from the list where the id is the one specified
        // ====================================================================

        /// <summary>
        /// Retrieves a <see cref="Category"/> object from categorys list base on provided integer number
        /// if not found will throw an exception.
        /// </summary>
        /// <param name="i">An integer </param>
        /// <returns> A <see cref="Category"/> object </returns>
        /// <exception cref="Exception">Thrown if no category was found </exception>
        /// <example>
        /// <code>
        /// try
        /// {
        ///     categoryId = 5;
        ///     Categories categories = new Categories();
        ///     Category category = categories.GetCategoryFromId(categoryId);
        ///     Console.WriteLine($"Category found: {Category.Description}")
        /// }
        /// Catch(Exception ex)
        /// {
        ///     Console.WriteLine($"Error in {ex.Message}")
        /// }
        /// </code>
        /// </example>
        public Category GetCategoryFromId(int i)
        {
            Category c = _Cats.Find(x => x.Id == i);
            if (c == null)
            {
                throw new Exception("Cannot find category with id " + i.ToString());
            }
            return c;
        }

        // ====================================================================
        // populate categories from a file
        // if filepath is not specified, read/save in AppData file
        // Throws System.IO.FileNotFoundException if file does not exist
        // Throws System.Exception if cannot read the file correctly (parsing XML)
        // ====================================================================

        /// <summary>
        /// Reads catagories data from an XML file of file path, reset current categories list and verified file path, 
        /// it exist and then load it in the categories list
        /// </summary>
        /// <param name="filepath">The string of file path </param>
        /// <example>
        /// <code>
        /// string filePath = "./data/file.txt";
        /// Categories categories = new Categories();
        /// categories.ReadFromFile(filePath); 
        /// </code>
        /// </example>

        public void ReadFromFile(String filepath = null)
        {

            // ---------------------------------------------------------------
            // reading from file resets all the current categories,
            // ---------------------------------------------------------------
            _Cats.Clear();

            // ---------------------------------------------------------------
            // reset default dir/filename to null 
            // ... filepath may not be valid, 
            // ---------------------------------------------------------------
            _DirName = null;
            _FileName = null;

            // ---------------------------------------------------------------
            // get filepath name (throws exception if it doesn't exist)
            // ---------------------------------------------------------------
            filepath = BudgetFiles.VerifyReadFromFileName(filepath, DefaultFileName);

            // ---------------------------------------------------------------
            // If file exists, read it
            // ---------------------------------------------------------------
            _ReadXMLFile(filepath);
            _DirName = Path.GetDirectoryName(filepath);
            _FileName = Path.GetFileName(filepath);
        }

        // ====================================================================
        // save to a file
        // if filepath is not specified, read/save in AppData file
        // ====================================================================

        /// <summary>
        /// Save all categories in an XML file if the file path is verified
        /// </summary>
        /// <param name="filepath">The file path</param>
        /// <example>
        /// <code>
        /// string filePath = "./data/file.txt";
        /// Categories categories = new Categories();
        /// categories.SaveToFile(filePath);
        /// </code>
        /// </example>
        public void SaveToFile(String filepath = null)
        {
            // ---------------------------------------------------------------
            // if file path not specified, set to last read file
            // ---------------------------------------------------------------
            if (filepath == null && DirName != null && FileName != null)
            {
                filepath = DirName + "\\" + FileName;
            }

            // ---------------------------------------------------------------
            // just in case filepath doesn't exist, reset path info
            // ---------------------------------------------------------------
            _DirName = null;
            _FileName = null;

            // ---------------------------------------------------------------
            // get filepath name (throws exception if it doesn't exist)
            // ---------------------------------------------------------------
            filepath = BudgetFiles.VerifyWriteToFileName(filepath, DefaultFileName);

            // ---------------------------------------------------------------
            // save as XML
            // ---------------------------------------------------------------
            _WriteXMLFile(filepath);

            // ----------------------------------------------------------------
            // save filename info for later use
            // ----------------------------------------------------------------
            _DirName = Path.GetDirectoryName(filepath);
            _FileName = Path.GetFileName(filepath);
        }

        // ====================================================================
        // set categories to default
        // ====================================================================
        /// <summary>
        /// Resets the current categories and initializes the list with default categories
        /// </summary>
        /// <example>
        /// <code>
        /// Categories categories = new Categories();
        /// categories.SetCategoriesToDefaults();
        /// </code>
        /// </example>
        public void SetCategoriesToDefaults()
        {
            // ---------------------------------------------------------------
            // reset any current categories,
            // ---------------------------------------------------------------
            _Cats.Clear();

            // ---------------------------------------------------------------
            // Add Defaults
            // ---------------------------------------------------------------
            Add("Utilities", Category.CategoryType.Expense);
            Add("Rent", Category.CategoryType.Expense);
            Add("Food", Category.CategoryType.Expense);
            Add("Entertainment", Category.CategoryType.Expense);
            Add("Education", Category.CategoryType.Expense);
            Add("Miscellaneous", Category.CategoryType.Expense);
            Add("Medical Expenses", Category.CategoryType.Expense);
            Add("Vacation", Category.CategoryType.Expense);
            Add("Credit Card", Category.CategoryType.Credit);
            Add("Clothes", Category.CategoryType.Expense);
            Add("Gifts", Category.CategoryType.Expense);
            Add("Insurance", Category.CategoryType.Expense);
            Add("Transportation", Category.CategoryType.Expense);
            Add("Eating Out", Category.CategoryType.Expense);
            Add("Savings", Category.CategoryType.Savings);
            Add("Income", Category.CategoryType.Income);
        }

        // ====================================================================
        // Add category
        // ====================================================================

        /// <summary>
        /// Add a new category in Categories list 
        /// </summary>
        /// <param name="cat"> A category object </param>
        /// <example>
        /// <code>
        /// Categories categories = new Categories();
        /// Category cat = new Category(1,"Food",Category.CategoryType.Expense);
        /// categories.Add(cat);
        /// </code>
        /// </example>
        private void Add(Category cat)
        {
            _Cats.Add(cat);                    
        }

        /// <summary>
        /// Add a new category in Categories list 
        /// </summary>
        /// <param name="desc">The string for category description</param>
        /// <param name="type">The enum for category type</param>
        /// <example>
        /// <code>
        /// Categories categories = new Categories();
        /// categories.Add("Food",Category.CategoryType.Expense);
        /// </code>
        /// </example>
        public void Add(String desc, Category.CategoryType type)
        {
            int new_num = 1;
            if (_Cats.Count > 0)
            {
                new_num = (from c in _Cats select c.Id).Max();
                new_num++;
            }
            _Cats.Add(new Category(new_num, desc, type));
        }

        // ====================================================================
        // Delete category
        // ====================================================================
        /// <summary>
        /// Delete a category from categories list
        /// </summary>
        /// <param name="Id">An integer </param>
        /// <example>
        /// <code>
        /// Categories categories = new Categories();
        /// int id = 5;
        /// categories.Delete(id);
        /// </code>
        /// </example>
        public void Delete(int Id)
        {
            int i = _Cats.FindIndex(x => x.Id == Id);
            _Cats.RemoveAt(i);
        }

        // ====================================================================
        // Return list of categories
        // Note:  make new copy of list, so user cannot modify what is part of
        //        this instance
        // ====================================================================

        /// <summary>
        /// Return a copy of categories collection.
        /// </summary>
        /// <returns> A new list contain copies of all categories. </returns>
        /// <example>
        /// <code>
        /// Categories categories = new Categories();
        /// categories.List();
        /// </code>
        /// </example>
        public List<Category> List()
        {
            List<Category> newList = new List<Category>();
            foreach (Category category in _Cats)
            {
                newList.Add(new Category(category));
            }
            return newList;
        }

        // ====================================================================
        // read from an XML file and add categories to our categories list
        // ====================================================================
        private void _ReadXMLFile(String filepath)
        {
            // ---------------------------------------------------------------
            // read the categories from the xml file, and add to this instance
            // ---------------------------------------------------------------
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filepath);

                foreach (XmlNode category in doc.DocumentElement.ChildNodes)
                {
                    String id = (((XmlElement)category).GetAttributeNode("ID")).InnerText;
                    String typestring = (((XmlElement)category).GetAttributeNode("type")).InnerText;
                    String desc = ((XmlElement)category).InnerText;

                    Category.CategoryType type;
                    switch (typestring.ToLower())
                    {
                        case "income":
                            type = Category.CategoryType.Income;
                            break;
                        case "expense":
                            type = Category.CategoryType.Expense;
                            break;
                        case "credit":
                            type = Category.CategoryType.Credit;
                            break;
                        default:
                            type = Category.CategoryType.Expense;
                            break;
                    }
                    this.Add(new Category(int.Parse(id), desc, type));
                }
            }
            catch (Exception e)
            {
                throw new Exception("ReadXMLFile: Reading XML " + e.Message);
            }
        }

        // ====================================================================
        // write all categories in our list to XML file
        // ====================================================================

        private void _WriteXMLFile(String filepath)
        {
            try
            {
                // create top level element of categories
                XmlDocument doc = new XmlDocument();
                doc.LoadXml("<Categories></Categories>");

                // foreach Category, create an new xml element
                foreach (Category cat in _Cats)
                {
                    XmlElement ele = doc.CreateElement("Category");
                    XmlAttribute attr = doc.CreateAttribute("ID");
                    attr.Value = cat.Id.ToString();
                    ele.SetAttributeNode(attr);
                    XmlAttribute type = doc.CreateAttribute("type");
                    type.Value = cat.Type.ToString();
                    ele.SetAttributeNode(type);

                    XmlText text = doc.CreateTextNode(cat.Description);
                    doc.DocumentElement.AppendChild(ele);
                    doc.DocumentElement.LastChild.AppendChild(text);

                }

                // write the xml to FilePath
                doc.Save(filepath);

            }
            catch (Exception e)
            {
                throw new Exception("_WriteXMLFile: Reading XML " + e.Message);
            }
        }
    }
}

