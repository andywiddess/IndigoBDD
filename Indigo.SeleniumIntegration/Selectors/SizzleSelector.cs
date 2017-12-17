using System;
using System.Diagnostics.CodeAnalysis;

namespace OpenQA.Selenium
{
    /// <summary>
    /// Searches the DOM elements using Sizzle selector.
    /// </summary>
    public class SizzleSelector 
        : AbstractSelector<SizzleSelector>
    {
        private const string _libraryVariable = "window.Sizzle";

        /// <summary>
        /// Initializes a new instance of the <see cref="SizzleSelector"/> class.
        /// </summary>
        /// <param name="selector">A string containing a selector expression.</param>
        /// <remarks>
        /// This constructor cannot be merged with <see cref="SizzleSelector(string,SizzleSelector)"/> constructor as
        /// it is resolved by reflection.
        /// </remarks>
        [SuppressMessage("ReSharper", "IntroduceOptionalParameters.Global")]
        public SizzleSelector(string selector)
            : this(selector, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SizzleSelector"/> class.
        /// </summary>
        /// <param name="selector">A string containing a selector expression.</param>
        /// <param name="context">A DOM Element, Document, or jQuery to use as context.</param>
        public SizzleSelector(string selector, SizzleSelector context)
            : base(selector, context)
        {
            Description = "By.SizzleSelector: {RawSelector}";
        }

        /// <summary>
        /// Gets the empty selector.
        /// </summary>
        public static SizzleSelector Empty
        {
            get
            {
                return new SizzleSelector("*");
            }
        }

        /// <summary>
        /// Gets the JavaScript to check if the prerequisites for the selector call have been met. The script should
        /// return <see langword="true" /> if the prerequisites are met; otherwise, <see langword="false" />.
        /// </summary>
        /// <inheritdoc />
        public override string CheckScript
        {
            get
            {
                return JavaScriptSnippets.CheckScriptCode(_libraryVariable);
            }
        }

        /// <summary>
        /// Gets the selector.
        /// </summary>
        /// <inheritdoc />
        public override string Selector
        {
            get
            {
                return "Sizzle('{RawSelector.Replace('\'', '\"')}'"
            + (Context != null ? ", {Context.Selector}[0]" : string.Empty) + ")";
            }
        }

        /// <summary>
        /// Loads the external library.
        /// </summary>
        /// <param name="driver">The web driver.</param>
        /// <inheritdoc />
        protected override void LoadExternalLibrary(IWebDriver driver)
        {
            driver.LoadSizzle();
        }

        /// <summary>
        /// Creates the context.
        /// </summary>
        /// <param name="contextSelector">The context selector.</param>
        /// <returns>
        /// The context.
        /// </returns>
        /// <inheritdoc />
        protected override SizzleSelector CreateContext(string contextSelector)
        {
            return new SizzleSelector(contextSelector);
        }
    }
}
