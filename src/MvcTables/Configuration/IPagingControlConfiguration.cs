using System;

namespace MvcTables.Configuration
{
    public interface IPagingControlConfiguration : IFluentInterface
    {
        /// <summary>
        ///     Defines the css class that will be applied to the outermost pager element
        /// </summary>
        /// <param name="class">The css class(es) to be applied</param>
        /// <returns>
        ///     The current <see cref="IPagingControlConfiguration" /> for chaining config methods
        /// </returns>
        IPagingControlConfiguration SetContainerCssClass(string @class);

        /// <summary>
        ///     Defines the css class that will decorate disabled page links
        /// </summary>
        /// <param name="class">The css class(es) to be applied</param>
        /// <returns>
        ///     The current <see cref="IPagingControlConfiguration" /> for chaining config methods
        /// </returns>
        IPagingControlConfiguration SetDisabledClass(string @class);

        /// <summary>
        ///     Defines the css class that will decorate active page links
        /// </summary>
        /// <param name="class">The css class(es) to be applied</param>
        /// <returns>
        ///     The current <see cref="IPagingControlConfiguration" /> for chaining config methods
        /// </returns>
        IPagingControlConfiguration SetActiveClass(string @class);

        /// <summary>
        ///     Defines the text for the previous page link
        /// </summary>
        /// <param name="text">The text disaplayed</param>
        /// <returns>
        ///     The current <see cref="IPagingControlConfiguration" /> for chaining config methods
        /// </returns>
        IPagingControlConfiguration SetPreviousPageText(string text);

        /// <summary>
        ///     Defines the text for the next page link
        /// </summary>
        /// <param name="text">The text disaplayed</param>
        /// <returns>
        ///     The current <see cref="IPagingControlConfiguration" /> for chaining config methods
        /// </returns>
        IPagingControlConfiguration SetNextPageText(string text);

        /// <summary>
        ///     Defines the text for the first page link
        /// </summary>
        /// <param name="text">The text disaplayed</param>
        /// <returns>
        ///     The current <see cref="IPagingControlConfiguration" /> for chaining config methods
        /// </returns>
        IPagingControlConfiguration SetFirstPageText(string text);

        /// <summary>
        ///     Defines the text for the first page link
        /// </summary>
        /// <param name="text">The text disaplayed</param>
        /// <returns>
        ///     The current <see cref="IPagingControlConfiguration" /> for chaining config methods
        /// </returns>
        IPagingControlConfiguration SetLastPageText(string text);

        /// <summary>
        ///     Defines the sizes for page size control
        /// </summary>
        /// <param name="sizes">The page sizes to be displayed</param>
        /// <returns>
        ///     The current <see cref="IPagingControlConfiguration" /> for chaining config methods
        /// </returns>
        IPagingControlConfiguration SetPageSizes(params int[] sizes);

    }
}