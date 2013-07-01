namespace MvcTables.Render
{
    #region

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web.Mvc;
    using System.Web.Routing;

    #endregion

    internal class ComplexContentTag : IDisposable
    {
        private readonly TagBuilder _tagBuilder;
        private readonly TextWriter _writer;

        public ComplexContentTag(string tagname, TextWriter writer)
            : this(tagname, null, writer)
        {
        }

        public ComplexContentTag(string tagname, object attributes, TextWriter writer)
            : this(tagname, new RouteValueDictionary(attributes), writer)
        {
        }

        public ComplexContentTag(string tagname, IDictionary<string, object> attributes, TextWriter writer)
        {
            _tagBuilder = new TagBuilder(tagname);
            _writer = writer;
            if (attributes != null)
            {
                _tagBuilder.MergeAttributes(attributes);
            }
            writer.Write(_tagBuilder.ToString(TagRenderMode.StartTag));
        }

        #region IDisposable Members

        public void Dispose()
        {
            _writer.Write(_tagBuilder.ToString(TagRenderMode.EndTag));
        }

        #endregion
    }
}