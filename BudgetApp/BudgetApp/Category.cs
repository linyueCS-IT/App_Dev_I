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
    // CLASS: Category
    //        - An individual category for budget program
    //        - Valid category types: Income, Expense, Credit, Saving
    // ====================================================================
    /// <summary>
    /// Represents a financial category used for classifying transactions.
    /// Each category has an ID, description, and type (Income, Expense, Credit, or Savings).
    /// </summary>
    public class Category
    {
        // ====================================================================
        // Properties
        // ====================================================================
        /// <summary>
        /// Gets and sets the unique identifier for the category.
        /// </summary>
        /// <value>
        /// The integer identifier for the category. 
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets and sets the description of the category.
        /// </summary>
        /// <value>
        /// The string describes category.
        /// </value>
        public String Description { get; set; }

        /// <summary>
        /// Gets or sets the type of the category.
        /// </summary>
        /// <value>
        /// The CategoryType describes category type.
        /// </value>
        public CategoryType Type { get; set; }

        /// <summary>
        /// Specifies the type of financial category.
        /// </summary>
        /// <value>
        /// The enum describes category type.
        /// </value>
        public enum CategoryType
        {
            /// <summary>
            /// Represents income or money received as category.
            /// </summary>
            Income,

            /// <summary>
            /// Represents expenses or money spent as category.
            /// </summary>
            Expense,

            /// <summary>
            /// Represents credit as category.
            /// </summary>
            Credit,

            /// <summary>
            /// Represents savings as category.
            /// </summary>
            Savings
        };

        // ====================================================================
        // Constructor
        // ====================================================================

        /// <summary>
        /// Initializes a new instance of the <see cref="Category"/> class.
        /// </summary>
        /// <param name="id">The unique identifier for the category.</param>
        /// <param name="description">The description of the category.</param>
        /// <param name="type">The category type. Defaults to <see cref="CategoryType.Expense"/></param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="id"/> is less than 0.</exception>
        public Category(int id, String description, CategoryType type = CategoryType.Expense)
        {
            if (id < 0)
            {
                throw new ArgumentException("ID cannot be negative.", nameof(id));
            }
            this.Id = id;
            this.Description = description;
            this.Type = type;
        }

        // ====================================================================
        // Copy Constructor
        // ====================================================================

        /// <summary>
        /// Initializes a new instance of the <see cref="Category"/> class
        /// by copying an existing <see cref="Category"/> instance. 
        /// </summary>
        /// <param name="category">>The exsiting category instance.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="category"/> is <c>null</c>.
        /// </exception>
        public Category(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category), "Category cannot be null.");
            }
            this.Id = category.Id;;
            this.Description = category.Description;
            this.Type = category.Type;
        }
        // ====================================================================
        // String version of object
        // ====================================================================
        /// <summary>
        /// Returns the description of the category as its string representation.
        /// </summary>
        /// <returns>The description of a category</returns>
        /// <example>
        /// <code>
        /// Category category = new Category(1,"Food",CategoryType.Expense);
        /// console.WriteLine(category.ToString());
        /// </code>
        /// </example>
        public override string ToString()
        {
            return Description;
        }
    }
}

