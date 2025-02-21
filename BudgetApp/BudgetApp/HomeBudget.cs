using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Dynamic;

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================


namespace Budget
{
    // ====================================================================
    // CLASS: HomeBudget
    //        - Combines categories Class and expenses Class
    //        - One File defines Category and Budget File
    //        - etc
    // ====================================================================
    /// <summary>
    /// Represents home budget operations, it contains Categories, Expenses and handle file (read and write) operations.
    /// </summary>
    public class HomeBudget
    {
        private string _FileName;
        private string _DirName;
        private Categories _categories;
        private Expenses _expenses;

        // ====================================================================
        // Properties
        // ===================================================================

        // Properties (location of files etc)

        /// <summary>
        /// Gets the name of the budget file.
        /// </summary>
        /// <value>
        /// The string representing the budget file name.
        /// </value>
        public String FileName { get { return _FileName; } }

        /// <summary>
        /// Gets the directory name where the budget file is stored.
        /// </summary>
        /// <value>
        /// The string representing the directory path.
        /// </value>
        public String DirName { get { return _DirName; } }

        /// <summary>
        /// Gets the full path of the budget file.
        /// </summary>
        /// <value>
        /// The complete file path combining directory and file name.
        /// Returns null if either directory or file name is not set.
        /// </value>
        public String PathName
        {
            get
            {
                if (_FileName != null && _DirName != null)
                {
                    return Path.GetFullPath(_DirName + "\\" + _FileName);
                }
                else
                {
                    return null;
                }
            }
        }

        // Properties (categories and expenses object)

        /// <summary>
        /// Gets the categories object.
        /// </summary>
        /// <value>
        /// The Categories object containing all budget categories content.
        /// </value>
        public Categories categories { get { return _categories; } }


        /// <summary>
        /// Gets the expenses object.
        /// </summary>
        /// <value>
        /// The Expenses object containing all budget expenses content.
        /// </value>
        public Expenses expenses { get { return _expenses; } }

        // -------------------------------------------------------------------
        // Constructor (new... default categories, no expenses)
        // -------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeBudget"/> class.
        /// and initialize a new instance of <see cref="Categories"/> and <see cref="Expenses"/>
        /// </summary>
        public HomeBudget()
        {
            _categories = new Categories();
            _expenses = new Expenses();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="HomeBudget"/> class.
        /// and initialize a new instance of <see cref="Categories"/> and <see cref="Expenses"/>
        /// </summary>
        /// <param name="budgetFileName">The path to the budget file from which data will be loaded.</param>
        // -------------------------------------------------------------------
        // Constructor (existing budget ... must specify file)
        // -------------------------------------------------------------------
        public HomeBudget(String budgetFileName)
        {
            _categories = new Categories();
            _expenses = new Expenses();
            ReadFromFile(budgetFileName);
        }

        #region OpenNewAndSave
        // ---------------------------------------------------------------
        // Read
        // Throws Exception if any problem reading this file
        // ---------------------------------------------------------------

        /// <summary>
        /// Reads budget of XML file if the file exist after verified, and convert to Categories (contains file name, categories list, file directory and file path)
        /// and Expenses (contains file name, categories list, file directory and file path), otherwise Thrown error message
        /// </summary>
        /// <param name="budgetFileName">The string XML file name</param>
        /// <exception cref="Exception">Thrown error message, such as file doen't exist or deesn't found or read XML file error</exception>
        /// <example>        
        /// <code>
        /// HomeBudget homeBudget = new HomeBudget();
        /// string budgetFileName = "./Data/test_expenses.exps";
        /// homeBudget.ReadFromFile(budgetFileName);
        /// </code>
        /// </example>

        public void ReadFromFile(String budgetFileName)
        {
            // ---------------------------------------------------------------
            // read the budget file and process
            // ---------------------------------------------------------------
            try
            {
                // get filepath name (throws exception if it doesn't exist)
                budgetFileName = BudgetFiles.VerifyReadFromFileName(budgetFileName, "");

                // If file exists, read it
                string[] filenames = System.IO.File.ReadAllLines(budgetFileName);

                // ----------------------------------------------------------------
                // Save information about budget file
                // ----------------------------------------------------------------
                string folder = Path.GetDirectoryName(budgetFileName);
                _FileName = Path.GetFileName(budgetFileName);

                // read the expenses and categories from their respective files
                _categories.ReadFromFile(folder + "\\" + filenames[0]);
                _expenses.ReadFromFile(folder + "\\" + filenames[1]);

                // Save information about budget file
                _DirName = Path.GetDirectoryName(budgetFileName);
                _FileName = Path.GetFileName(budgetFileName);

            }

            // ----------------------------------------------------------------
            // throw new exception if we cannot get the info that we need
            // ----------------------------------------------------------------
            catch (Exception e)
            {
                throw new Exception("Could not read budget info: \n" + e.Message);
            }
        }

        // ====================================================================
        // save to a file
        // saves the following files:
        //  filepath_expenses.exps  # expenses file
        //  filepath_categories.cats # categories files
        //  filepath # a file containing the names of the expenses and categories files.
        //  Throws exception if we cannot write to that file (ex: invalid dir, wrong permissions)
        // ====================================================================

        /// <summary>
        /// Reads a budget file and save 2 file, one is Categories file through expension ".cats", 
        /// another is Expenses file through expension ".exps"
        /// </summary>
        /// <param name="filepath">The string file path of read </param>
        /// <example>
        /// <code>
        /// string filePath = "./Data/test.budget";
        /// HomeBudget homeBudget = new HomeBudget();
        /// homeBudget.SaveToFile(filePath);
        /// </code>
        /// </example>
        public void SaveToFile(String filepath)
        {

            // ---------------------------------------------------------------
            // just in case filepath doesn't exist, reset path info
            // ---------------------------------------------------------------
            _DirName = null;
            _FileName = null;

            // ---------------------------------------------------------------
            // get filepath name (throws exception if we can't write to the file)
            // ---------------------------------------------------------------
            filepath = BudgetFiles.VerifyWriteToFileName(filepath, "");

            String path = Path.GetDirectoryName(Path.GetFullPath(filepath));
            String file = Path.GetFileNameWithoutExtension(filepath);
            String ext = Path.GetExtension(filepath);

            // ---------------------------------------------------------------
            // construct file names for expenses and categories
            // ---------------------------------------------------------------
            String expensepath = path + "\\" + file + "_expenses" + ".exps";
            String categorypath = path + "\\" + file + "_categories" + ".cats";

            // ---------------------------------------------------------------
            // save the expenses and categories into their own files
            // ---------------------------------------------------------------
            _expenses.SaveToFile(expensepath);
            _categories.SaveToFile(categorypath);

            // ---------------------------------------------------------------
            // save filenames of expenses and categories to budget file
            // ---------------------------------------------------------------
            string[] files = { Path.GetFileName(categorypath), Path.GetFileName(expensepath) };
            System.IO.File.WriteAllLines(filepath, files);

            // ----------------------------------------------------------------
            // save filename info for later use
            // ----------------------------------------------------------------
            _DirName = path;
            _FileName = Path.GetFileName(filepath);
        }
        #endregion OpenNewAndSave

        #region GetList

        // ============================================================================
        // Get all expenses list
        // NOTE: VERY IMPORTANT... budget amount is the negative of the expense amount
        // Reasoning: an expense of $15 is -$15 from your bank account.
        // ============================================================================

        /// <summary>
        /// Retrieves and combines budget items from categories and expenses within a specified date range.
        /// Creates a list of budget items with running balance calculations. 
        /// Note: The amount in the budget item is the negative of the expense amount 
        /// (e.g., an expense of $15 becomes -$15 in the budget).
        /// </summary>
        /// <param name="Start">The start date of the query range. If null, it defaults to January 1, 1900.</param>
        /// <param name="End">The end date of the query range. If null, it defaults to January 1, 2500.</param>
        /// <param name="FilterFlag">Determines whether to filter results based on the Category id, if it is true, filter throught category id
        /// otherwise filter through all categories</param>
        /// <param name="CategoryID">The ID of the category to filter expenses if FilterFlag is true.</param>
        /// <returns>A list of BudgetItem objects, each containing details about an expense, 
        /// including category, description, date, amount, and running balance.</returns>
        /// <example>
        /// <code>
        /// HomeBudget homeBudget = new HomeBudget("./Data/budget.txt");
        /// 
        /// var budgetItems = homeBudget.GetBudgetItems(
        ///     null,
        ///     null,
        ///     false,
        ///     0
        /// );
        /// if (budgetItems == null || budgetItems.Count == 0)
        /// {
        ///     Console.WriteLine("There is no eligible information");
        /// }
        /// else
        /// {
        ///     Console.WriteLine($"{"Date",-15}{"Description",-25}{"Amount",15}{"Balance",15}");
        ///     Console.WriteLine(new string ('-', 70));
        ///     foreach (BudgetItem budgetItem in budgetItems)
        ///     {
        ///         Console.WriteLine($"{budgetItem.Date.ToShortDateString(),-15} {budgetItem.ShortDescription,-25} {budgetItem.Amount,15:C2} {budgetItem.Balance,15:C2}");
        ///     }
        ///     Console.WriteLine(new string ('-', 70));
        ///     }
        /// }
        /// </code>
        /// <b>Sample output</b>
        /// <code>
        /// 
        ///                         Report Budget Items
        /// ---------------------------------------------------------------------
        /// Date Description                           Amount          Balance  |
        /// ---------------------------------------------------------------------
        /// 1/10/2018       hat(on credit)             ($10.00)         ($10.00)|
        /// 1/11/2018       hat                          $10.00           $0.00 |
        /// 1/10/2019       scarf(on credit)           ($15.00)         ($15.00)|
        /// 1/10/2020       scarf                        $15.00           $0.00 |
        /// 1/11/2020       McDonalds                  ($45.00)         ($45.00)|
        /// 1/12/2020       Wendys                     ($25.00)        ($70.00) |
        /// 2/1/2020        Pizza                      ($33.33)       ($103.33) |
        /// 2/10/2020       mittens                      $15.00        ($88.33) |
        /// 2/25/2020       Hat                          $25.00        ($63.33) |
        /// 2/27/2020       Pizza                      ($33.33)        ($96.66) |
        /// 7/11/2020       Cafeteria                  ($11.11)       ($107.77) |
        /// ---------------------------------------------------------------------
        /// </code>
        /// </example>
        public List<BudgetItem> GetBudgetItems(DateTime? Start, DateTime? End, bool FilterFlag, int CategoryID)
        {
            // ------------------------------------------------------------------------
            // return joined list within time frame
            // ------------------------------------------------------------------------
            Start = Start ?? new DateTime(1900, 1, 1);
            End = End ?? new DateTime(2500, 1, 1);

            var query = from c in _categories.List()
                        join e in _expenses.List() on c.Id equals e.Category
                        where e.Date >= Start && e.Date <= End
                        select new { CatId = c.Id, ExpId = e.Id, e.Date, Category = c.Description, e.Description, e.Amount };

            // ------------------------------------------------------------------------
            // create a BudgetItem list with totals,
            // ------------------------------------------------------------------------
            List<BudgetItem> items = new List<BudgetItem>();
            Double total = 0;

            foreach (var e in query.OrderBy(q => q.Date))
            {
                // filter out unwanted categories if filter flag is on
                if (FilterFlag && CategoryID != e.CatId)
                {
                    continue;
                }

                // keep track of running totals
                total = total - e.Amount;
                items.Add(new BudgetItem
                {
                    CategoryID = e.CatId,
                    ExpenseID = e.ExpId,
                    ShortDescription = e.Description,
                    Date = e.Date,
                    Amount = -e.Amount,
                    Category = e.Category,
                    Balance = total
                });
            }
            return items;
        }

        // ============================================================================
        // Group all expenses month by month (sorted by year/month)
        // returns a list of BudgetItemsByMonth which is 
        // "year/month", list of budget items, and total for that month
        // ============================================================================

        /// <summary>
        /// Groups budget items by month and calculates monthly totals.
        /// Each BudgetItemsByMonth contains the month, a list of BudgetItem,
        /// and the total amount for that month.
        /// </summary>
        /// <param name="Start">The start date of the query range. If null, it defaults to January 1, 1900.</param>
        /// <param name="End">The end date of the query range. If null, it defaults to January 1, 2500.</param>
        /// <param name="FilterFlag">Determines whether to filter results based on the Category id, if it is true, filter throught category id
        /// otherwise filter through all categories</param>
        /// <param name="CategoryID">The ID of the category to filter expenses if FilterFlag is true.</param>
        /// <returns>A list of BudgetItemsByMonth objects, each containing details about an expense, 
        /// including category, description, date, amount, and running balance.</returns>
        /// <example>
        /// <code>
        /// HomeBudget homeBudget = new HomeBudget("./Data/budget.txt");
        /// var budgetItemsByMonths = homeBudget.GetBudgetItems(
        ///     null,
        ///     null,
        ///     false,
        ///     0
        /// );
        /// if (budgetItemsByMonths == null || budgetItemsByMonths.Count == 0)
        /// {
        ///     Console.WriteLine("There is no eligible information");
        /// }
        /// else
        /// {
        ///     foreach (BudgetItemsByMonth budgetItemByMonth in budgetItemsByMonths)
        ///     {
        ///         Console.WriteLine($"\nMonth: {budgetItemByMonth.Month,0}");
        ///         Console.WriteLine($"{"Date",-12} {"Description",-30} {"Amount",10}");
        ///         Console.WriteLine(new string ('-', 54));
        ///         foreach (BudgetItem budgetItem in budgetItemByMonth.Details)
        ///         {
        ///             Console.WriteLine($"{budgetItem.Date.ToShortDateString(),-12} {budgetItem.ShortDescription,-30} {budgetItem.Amount,10:C2}");
        ///         }
        ///         Console.WriteLine(new string ('-', 54));
        ///         Console.WriteLine($"{"Month Total:",-43} {budgetItemByMonth.Total,10:C2}");
        ///     }
        /// }
        /// </code>
        /// <b>Sample output</b>
        /// <code>
        ///               Report Budget Items by Month
        /// --------------------------------------------------------
        /// Month: 2018/01                                         |
        /// Date Description                                Amount |
        /// --------------------------------------------------------
        /// 1/10/2018    hat(on credit)                   ($10.00) |
        /// 1/11/2018    hat                                $10.00 |
        /// --------------------------------------------------------
        /// Month Total:                                     $0.00
        /// 
        /// --------------------------------------------------------
        /// Month: 2019/01                                         |
        /// Date Description                                Amount |
        /// --------------------------------------------------------
        /// 1/10/2019    scarf(on credit)                 ($15.00) |
        /// --------------------------------------------------------
        /// /Month Total:                                  ($15.00)
        /// 
        /// --------------------------------------------------------
        /// Month: 2020/01                                         |
        /// Date Description                                Amount |
        /// --------------------------------------------------------
        /// 1/10/2020    scarf                              $15.00 |
        /// 1/11/2020    McDonalds                        ($45.00) |
        /// 1/12/2020    Wendys                           ($25.00) |
        /// --------------------------------------------------------
        /// Month Total:                                  ($55.00)
        /// 
        /// --------------------------------------------------------
        /// Month: 2020/02                                         |
        /// Date Description                                Amount |
        /// --------------------------------------------------------
        /// 2/1/2020     Pizza                            ($33.33) |
        /// 2/10/2020    mittens                            $15.00 |
        /// 2/25/2020    Hat                                $25.00 |
        /// 2/27/2020    Pizza                            ($33.33) |
        /// --------------------------------------------------------
        /// Month Total:                                  ($26.66)
        /// 
        /// --------------------------------------------------------
        /// Month: 2020/07                                         |
        /// Date Description                                Amount |
        /// --------------------------------------------------------
        /// 7/11/2020    Cafeteria                        ($11.11) |
        /// --------------------------------------------------------
        /// Month Total:                                  ($11.11)
        /// </code>
        /// </example>

        public List<BudgetItemsByMonth> GetBudgetItemsByMonth(DateTime? Start, DateTime? End, bool FilterFlag, int CategoryID)
        {
            // -----------------------------------------------------------------------
            // get all items first
            // -----------------------------------------------------------------------
            List<BudgetItem> items = GetBudgetItems(Start, End, FilterFlag, CategoryID);

            // -----------------------------------------------------------------------
            // Group by year/month
            // -----------------------------------------------------------------------
            var GroupedByMonth = items.GroupBy(c => c.Date.Year.ToString("D4") + "/" + c.Date.Month.ToString("D2"));

            // -----------------------------------------------------------------------
            // create new list
            // -----------------------------------------------------------------------
            var summary = new List<BudgetItemsByMonth>();
            foreach (var MonthGroup in GroupedByMonth)
            {
                // calculate total for this month, and create list of details
                double total = 0;
                var details = new List<BudgetItem>();
                foreach (var item in MonthGroup)
                {
                    total = total + item.Amount;
                    details.Add(item);
                }

                // Add new BudgetItemsByMonth to our list
                summary.Add(new BudgetItemsByMonth
                {
                    Month = MonthGroup.Key,
                    Details = details,
                    Total = total
                });
            }
            return summary;
        }

        // ============================================================================
        // Group all expenses by category (ordered by category name)
        // ============================================================================

        /// <summary>
        /// Groups budget items by category and calculates totals for each category.
        /// Each BudgetItemsByCategory contains the category name, a list of BudgetItem,
        /// and the total amount for that category.
        /// </summary>
        /// <param name="Start">The start date of the query range. If null, it defaults to January 1, 1900.</param>
        /// <param name="End">The end date of the query range. If null, it defaults to January 1, 2500.</param>
        /// <param name="FilterFlag">Determines whether to filter results based on the Category id, if it is true, filter throught category id
        /// otherwise filter through all categories</param>
        /// <param name="CategoryID">The ID of the category to filter expenses if FilterFlag is true.</param>
        /// <returns>A list of BudgetItemsByCategory objects, each containing details about an expense, 
        /// including category, description, date, amount, and running balance.</returns>
        /// <example>
        /// <code>
        /// HomeBudget homeBudget = new HomeBudget("./Data/budget.txt");
        /// var budgetItemsByCategories = homeBudget.GetBudgetItems(
        ///     null,
        ///     null,
        ///     false,
        ///     0
        /// );
        /// if (budgetItemsByCategories == null || budgetItemsByCategories.Count == 0)
        /// {
        ///    Console.WriteLine("There is no eligible information");
        /// }
        /// else
        /// {
        ///     foreach (BudgetItemsByCategory budgetItemByCategory in budgetItemsByCategories)
        ///     {
        ///         Console.WriteLine($"\nCategory: {budgetItemByCategory.Category,-10}");
        ///         Console.WriteLine($"{"Date",-12} {"Description",-30} {"Amount",10}");
        ///         Console.WriteLine(new string ('-', 54));
        ///         foreach (BudgetItem budgetItem in budgetItemByCategory.Details)
        ///         {
        ///             Console.WriteLine($"{budgetItem.Date.ToShortDateString(),-12} {budgetItem.ShortDescription,-30} {budgetItem.Amount,10:C2}");
        ///         }
        ///         Console.WriteLine(new string ('-', 54));
        ///         Console.WriteLine($"{"Category Total:", -43} {budgetItemByCategory.Total,10:C2}");
        ///     }
        /// }
        /// </code>
        /// <b>Sample output</b>
        /// <code>
        ///             Report Budget Items by Category
        /// -----------------------------------------------------
        /// Category: Clothes                                   |
        /// Date Description                             Amount |
        /// -----------------------------------------------------
        /// 1/10/2018    hat(on credit)                ($10.00) |
        /// 1/10/2019    scarf(on credit)              ($15.00) |
        /// -----------------------------------------------------
        /// Category Total:                             ($25.00)
        /// 
        /// -----------------------------------------------------
        /// Category: Credit Card                               |
        /// Date Description                             Amount |
        /// -----------------------------------------------------
        /// 1/11/2018    hat                             $10.00 |
        /// 1/10/2020    scarf                           $15.00 |
        /// 2/10/2020    mittens                         $15.00 |
        /// 2/25/2020    Hat                             $25.00 |
        /// -----------------------------------------------------
        /// Category Total:                              $65.00
        /// 
        /// -----------------------------------------------------
        /// Category: Eating Out                                |
        /// Date Description                             Amount |
        /// -----------------------------------------------------
        /// 1/11/2020    McDonalds                     ($45.00) |
        /// 1/12/2020    Wendys                        ($25.00) |
        /// 2/1/2020     Pizza                         ($33.33) |
        /// 2/27/2020    Pizza                         ($33.33) |
        /// 7/11/2020    Cafeteria                     ($11.11) |
        /// -----------------------------------------------------
        /// Category Total:                           ($147.77)
        /// </code>
        /// </example>
        public List<BudgetItemsByCategory> GeBudgetItemsByCategory(DateTime? Start, DateTime? End, bool FilterFlag, int CategoryID)
        {
            // -----------------------------------------------------------------------
            // get all items first
            // -----------------------------------------------------------------------
            List<BudgetItem> items = GetBudgetItems(Start, End, FilterFlag, CategoryID);

            // -----------------------------------------------------------------------
            // Group by Category
            // -----------------------------------------------------------------------
            var GroupedByCategory = items.GroupBy(c => c.Category);

            // -----------------------------------------------------------------------
            // create new list
            // -----------------------------------------------------------------------
            var summary = new List<BudgetItemsByCategory>();
            foreach (var CategoryGroup in GroupedByCategory.OrderBy(g => g.Key))
            {
                // calculate total for this category, and create list of details
                double total = 0;
                var details = new List<BudgetItem>();
                foreach (var item in CategoryGroup)
                {
                    total = total + item.Amount;
                    details.Add(item);
                }

                // Add new BudgetItemsByCategory to our list
                summary.Add(new BudgetItemsByCategory
                {
                    Category = CategoryGroup.Key,
                    Details = details,
                    Total = total
                });
            }
            return summary;
        }
        // ============================================================================
        // Group all events by category and Month
        // creates a list of Dictionary objects (which are objects that contain key value pairs).
        // The list of Dictionary objects includes:
        //          one dictionary object per month with expenses,
        //          and one dictionary object for the category totals
        // 
        // Each per month dictionary object has the following key value pairs:
        //           "Month", <the year/month for that month as a string>
        //           "Total", <the total amount for that month as a double>
        //            and for each category for which there is an expense in the month:
        //             "items:category", a List<BudgetItem> of all items in that category for the month
        //             "category", the total amount for that category for this month
        //
        // The one dictionary for the category totals has the following key value pairs:
        //             "Month", the string "TOTALS"
        //             for each category for which there is an expense in ANY month:
        //             "category", the total for that category for all the months
        // ============================================================================

        /// <summary>
        /// Retrieves a summarized budget report by category and month. This method calculates the total amount for each month, 
        /// groups the budget items by category, and provides a detailed breakdown of expenses for each category. Additionally, 
        /// it calculates the overall totals for each category across all months and returns this information in a list of dictionaries.
        /// </summary>
        /// <param name="Start">The start date of the query range. If null, it defaults to January 1, 1900.</param>
        /// <param name="End">The end date of the query range. If null, it defaults to January 1, 2500.</param>
        /// <param name="FilterFlag">Determines whether to filter results based on the Category id. If true, filters by category ID; otherwise, includes all categories.</param>
        /// <param name="CategoryID">The ID of the category to filter expenses if FilterFlag is true.</param>
        /// <returns>
        /// A list of dictionaries where each dictionary represents a month and contains the following keys:
        /// - "Month" : A string or date representing the month.
        /// - "Total" : The total budget amount for that month.
        /// - "details: CategoryName" : A list of budget items for a specific category in that month.
        /// - "CategoryName" : The total amount for that category in that month.
        /// The final entry in the list contains the overall totals for each category across all months, under the "TOTALS" entry.
        /// </returns>
        /// <example>
        /// <code>
        /// HomeBudget homeBudget = new HomeBudget("./Data/budget.txt");
        /// var summary = homeBudget.GetBudgetItems(
        ///     2018.1.1,
        ///     2019.12.31,
        ///     false,
        ///     0
        /// );
        /// if (summary == null || summary.Count == 0 || 
        ///     (summary.Count == 1 &amp;&amp; summary[0]["Month"].ToString() == "TOTALS"))
        /// {
        ///     Console.WriteLine("There is no eligible information");
        ///     return;
        /// }
        /// else
        /// {
        ///     Console.WriteLine("\n\tReport Budget by Category and Month\n");
        ///     foreach (Dictionary&lt;string, object&gt; record in summary)
        ///     {
        ///         Console.WriteLine($"Month: {record["Month"]}");
        ///         if (record["Month"].ToString() != "TOTALS")
        ///         {
        ///             Console.WriteLine($"Monthly Total: {record["Total"]:C2}\n");
        ///             foreach (string key in record.Keys)
        ///             {
        ///                 if (key.StartsWith("details:"))
        ///                 {
        ///                     string category = key.Substring(8);
        ///                     Console.WriteLine($"Category: {category}");
        ///                     Console.WriteLine($"{"Date",-16} {"Description",-25} {"Amount",-10}");
        ///                     Console.WriteLine(new string ('-', 54));
        ///                     List&lt;BudgetItem&gt; details = (List&lt;BudgetItem&gt;)record[key];
        ///                     foreach (BudgetItem budgetItem in details)
        ///                     {
        ///                         Console.WriteLine($"{budgetItem.Date.ToShortDateString(),-16}" +
        ///                                           $"{budgetItem.ShortDescription,-25}" +
        ///                                           $"{budgetItem.Amount,-10:C2}");
        ///                     }
        ///                     Console.WriteLine(new string ('-', 54));
        ///                     Console.WriteLine($"Category Total: {record[category]}\n");
        ///                 }
        ///             }
        ///         }
        ///         else
        ///         {
        ///             Console.WriteLine($"Category Total:");
        ///             Console.WriteLine(new string('-', 54));
        ///             foreach (string key in record.Keys)
        ///             {
        ///                 if (key != "Month")
        ///                 {
        ///                     Console.WriteLine($"{key,-38} {record[key],10:C2}");
        ///                 }
        ///             }
        ///             Console.WriteLine(new string('-', 54));
        ///         }
        ///     }
        /// }
        /// </code>
        /// <b>Sample output</b>
        /// <code>
        ///
        ///         Report Budget by Category and Month
        /// ===================================================
        ///
        /// Month: 2018/01
        /// Monthly Total: $0.00
        /// ---------------------------------------------------
        /// Category: Clothes
        /// Date               Description               Amount
        /// 1/10/2018         hat (on credit)          ($10.00)
        /// ---------------------------------------------------
        /// Category Total: -10
        /// ---------------------------------------------------
        /// Category: Credit Card
        /// Date               Description               Amount
        /// 1/11/2018         hat                        $10.00
        /// ---------------------------------------------------
        /// Category Total: 10
        /// ===================================================
        ///
        /// Month: 2019/01
        /// Monthly Total: ($15.00)
        /// ---------------------------------------------------
        /// Category: Clothes
        /// Date               Description               Amount
        /// 1/10/2019         scarf (on credit)        ($15.00)
        /// ---------------------------------------------------
        /// Category Total: -15
        /// ===================================================
        ///
        /// Month: TOTALS
        /// Category Total:
        /// ---------------------------------------------------
        /// Credit Card                                  $10.00
        /// Clothes                                    ($25.00)
        /// ===================================================
        /// </code>
        /// </example>

        public List<Dictionary<string, object>> GetBudgetDictionaryByCategoryAndMonth(DateTime? Start, DateTime? End, bool FilterFlag, int CategoryID)
        {
            // -----------------------------------------------------------------------
            // get all items by month 
            // -----------------------------------------------------------------------
            List<BudgetItemsByMonth> GroupedByMonth = GetBudgetItemsByMonth(Start, End, FilterFlag, CategoryID);

            // -----------------------------------------------------------------------
            // loop over each month
            // -----------------------------------------------------------------------
            var summary = new List<Dictionary<string, object>>();
            var totalsPerCategory = new Dictionary<String, Double>();

            foreach (var MonthGroup in GroupedByMonth)
            {
                // create record object for this month
                Dictionary<string, object> record = new Dictionary<string, object>();
                record["Month"] = MonthGroup.Month;
                record["Total"] = MonthGroup.Total;

                // break up the month details into categories
                var GroupedByCategory = MonthGroup.Details.GroupBy(c => c.Category);

                // -----------------------------------------------------------------------
                // loop over each category
                // -----------------------------------------------------------------------
                foreach (var CategoryGroup in GroupedByCategory.OrderBy(g => g.Key))
                {
                    // calculate totals for the cat/month, and create list of details
                    double total = 0;
                    var details = new List<BudgetItem>();

                    foreach (var item in CategoryGroup)
                    {
                        total = total + item.Amount;
                        details.Add(item);
                    }

                    // add new properties and values to our record object
                    record["details:" + CategoryGroup.Key] = details;
                    record[CategoryGroup.Key] = total;

                    // keep track of totals for each category
                    if (totalsPerCategory.TryGetValue(CategoryGroup.Key, out Double CurrentCatTotal))
                    {
                        totalsPerCategory[CategoryGroup.Key] = CurrentCatTotal + total;
                    }
                    else
                    {
                        totalsPerCategory[CategoryGroup.Key] = total;
                    }
                }
                // add record to collection
                summary.Add(record);
            }
            // ---------------------------------------------------------------------------
            // add final record which is the totals for each category
            // ---------------------------------------------------------------------------
            Dictionary<string, object> totalsRecord = new Dictionary<string, object>();
            totalsRecord["Month"] = "TOTALS";

            foreach (var cat in categories.List())
            {
                try
                {
                    totalsRecord.Add(cat.Description, totalsPerCategory[cat.Description]);
                }
                catch { }
            }
            summary.Add(totalsRecord);

            return summary;
        }
        #endregion GetList
    }
}
