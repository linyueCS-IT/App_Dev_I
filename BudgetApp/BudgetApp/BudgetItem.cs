using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================

namespace Budget
{
    // ====================================================================
    // CLASS: BudgetItem
    //        A single budget item, includes Category and Expense
    // ====================================================================

    /// <summary>
    /// Represents an individual budget item. It contains details about the category, 
    /// expense, date, and financial amounts related to the item.
    /// </summary>
    public class BudgetItem
    {
        /// <summary>
        /// Gets and sets the category identifier.
        /// </summary>
        /// <value>
        /// The integer identifier associated with the category.
        /// </value>
        public int CategoryID { get; set; }

        /// <summary>
        /// Gets and sets the expense identifier.
        /// </summary>
        /// <value>
        /// The integer identifier for the expense. 
        /// </value>
        public int ExpenseID { get; set; }

        /// <summary>
        /// Gets and sets the date.
        /// </summary>
        /// <value>
        /// The DateTime value representing when the budget item occurred. 
        /// </value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets and sets the name of category.
        /// </summary>
        /// <value>
        /// The string representing the name of the categor. 
        /// </value>
        public String Category { get; set; }

        /// <summary>
        /// Gets or sets the short description.
        /// </summary>
        /// <value>
        /// A short string describing.
        /// </value>
        public String ShortDescription { get; set; }

        /// <summary>
        /// Gets and sets the amount.
        /// </summary>
        /// <value>
        /// The monetary value.
        /// </value>
        public Double Amount { get; set; }

        /// <summary>
        ///  Gets and sets the current balance.
        /// </summary>
        /// <value>
        /// The monetary balance value.
        /// </value>
        public Double Balance { get; set; }
    }
    /// <summary>
    /// Represents a budget summary by month, it includes month, the list of budget item details and total expense
    /// </summary>
    public class BudgetItemsByMonth
    {
        /// <summary>
        /// Gets and sets the month identifier.
        /// </summary>
        /// <value>
        /// The string representing the month (e.g., "January 2024").
        /// </value>
        public String Month { get; set; }

        /// <summary>
        /// Gets and sets the list of BudgetItem for this month.
        /// </summary>
        /// <value>
        /// A collection of BudgetItem objects containing all BudgetItem for the month.
        /// </value>
        public List<BudgetItem> Details { get; set; }

        /// <summary>
        /// Gets and sets the total amount for all BudgetItem in this month.
        /// </summary>
        /// <value>
        /// The sum of all BudgetItem amounts in the Details list.
        /// Represents the total of all income and expenses for the month.
        /// </value>
        public Double Total { get; set; }
    }

    /// <summary>
    /// Represents a budget summary by category, it includes category, the list of budget item details and total expense
    /// </summary>
    public class BudgetItemsByCategory
    {
        /// <summary>
        /// Gets and sets the category name.
        /// </summary>
        /// <value>
        /// The string category name.
        /// </value>
        public String Category { get; set; }

        /// <summary>
        /// Gets and sets the list of BudgetItem for this category.
        /// </summary>
        /// <value>
        /// A collection of BudgetItem objects.
        /// </value>
        public List<BudgetItem> Details { get; set; }

        /// <summary>
        /// Gets and sets the total amount for all BudgetItem in this category.
        /// </summary>
        /// <value>
        /// The sum of all BudgetItem amounts in the Details list.
        /// Represents the total amount spent or received for this category.
        /// </value>
        public Double Total { get; set; }

    }
}
