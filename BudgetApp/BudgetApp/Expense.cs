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
    // CLASS: Expense
    //        - An individual expens for budget program
    // ====================================================================
    /// <summary>
    /// Represents a financial expense,each expense haa an Id, Data, Amount, Description and Category.
    /// </summary>
    public class Expense
    {
        // ====================================================================
        // Properties
        // ====================================================================

        /// <summary>
        /// Gets the unique identifier for expense, this value cannot be changed after the expense is created
        /// </summary>
        /// <value>
        /// The integer identifier for the expense. 
        /// </value>
        public int Id { get; }

        /// <summary>
        /// Gets the data when the expense is created, this value cannot be changed after the expense is created.
        /// </summary>
        /// <value>
        /// The DateTime value representing when the expense occurred. 
        /// </value>
        public DateTime Date { get;  }

        /// <summary>
        /// Gets and sets the monetary amount of the expense.
        /// </summary>
        /// <value>
        /// The monetary value of expense.
        /// </value>
        public Double Amount { get; set; }

        /// <summary>
        /// Gets and sets the description of the expense.
        /// </summary>
        /// <value>
        /// A string describing.
        /// </value>
        public String Description { get; set; }

        /// <summary>
        /// Gets and sets the category ID with this expense
        /// </summary>
        /// <value>
        /// The integer representing the ID of the category. 
        /// </value>
        public int Category { get; set; }

        // ====================================================================
        // Constructor
        //    NB: there is no verification the expense category exists in the
        //        categories object
        // ====================================================================

        /// <summary>
        /// Initializes a new instance of the <see cref="Expense"/> class.
        /// </summary>
        /// <param name="id">The unique identifier for the expense.</param>
        /// <param name="date">The date of the expense.</param>
        /// <param name="category">The category ID associated with the expense.</param>
        /// <param name="amount">The monetary amount of the expense.</param>
        /// <param name="description">A brief description of the expense.</param>
        public Expense(int id, DateTime date, int category, Double amount, String description)
        {
            this.Id = id;
            this.Date = date;
            this.Category = category;
            this.Amount = amount;
            this.Description = description;
        }

        // ====================================================================
        // Copy constructor - does a deep copy
        // ====================================================================

        /// <summary>
        /// Initializes a new instance of the <see cref="Expense"/> class
        /// by copying the values from an existing <see cref="Expense"/> object.
        /// </summary>
        /// <param name="obj">The <see cref="Expense"/> object to copy from existing expense object.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="obj"/> is <c>null</c>.
        /// </exception>
        public Expense (Expense obj)
        {
            if(obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "Expense object cannot be null.");
            }
            this.Id = obj.Id;
            this.Date = obj.Date;
            this.Category = obj.Category;
            this.Amount = obj.Amount;
            this.Description = obj.Description;           
            this.Description = obj.Description;           
        }
    }
}
