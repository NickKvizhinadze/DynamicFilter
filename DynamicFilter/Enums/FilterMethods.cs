namespace DynamicFilter.Enums
{
    /// <summary>
    /// Enum for filter model property attribute methods
    /// </summary>
    public enum FilterMethods
    {
        /// <summary>
        /// Method for checking equality
        /// </summary>
        Equal,
        /// <summary>
        /// Method for checking if list contains element
        /// </summary>
        Contains,
        /// <summary>
        /// Method for checking if string contains substring
        /// </summary>
        StringContains,
        /// <summary>
        /// Method for checking if string  has value and contains substring
        /// </summary>
        HasValueAndContains,
        /// <summary>
        /// Method for checking if has value and equal
        /// </summary>
        HasValueAndEqual,
        /// <summary>
        /// Method for checking if value is greater then
        /// </summary>
        GreaterThan,
        /// <summary>
        /// Method for checking if value is equal or greater then
        /// </summary>
        GreaterThanOrEqual,
        /// <summary>
        /// Method for checking if value is less then
        /// </summary>
        LessThan,
        /// <summary>
        /// Method for checking if value is equal or less then
        /// </summary>
        LessThanOrEqual,
        /// <summary>
        /// Method for checking if value is not null
        /// </summary>
        IsNotNull,
        /// <summary>
        /// Method for checking if value not null
        /// </summary>
        IsNull
    }
}
