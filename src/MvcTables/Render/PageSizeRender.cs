using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcTables.Configuration;

namespace MvcTables.Render
{
    internal class PageSizeRender
    {
        public void RenderPageSize(PagingControlConfiguration pageConfig, int currentPageSize, string tableId, ControllerContext context)
        {
            if(pageConfig.PageSizes == null)
                return;

            var writer = WriterHelper.GetWriter(context);
            var name = StaticReflection.StaticReflection.GetMember((TableRequestModel m) => m.PageSize).Name;
            using (new ComplexContentTag("select", new Dictionary<string, object>() { {"data-target", tableId}, {"class", pageConfig.TableDefinition.FilterExpression + " mvc-table-page-size"}, {"name", name }}, writer))
            {
                foreach (var pageSize in pageConfig.PageSizes)
                {

                    if (currentPageSize == pageSize)
                    {
                        using (new ComplexContentTag("option", new { value = pageSize, selected = "selected" }, writer))
                        {
                            writer.Write(pageSize);
                        }
                    }
                    else
                    {
                        using (new ComplexContentTag("option", new { value = pageSize}, writer))
                        {
                            writer.Write(pageSize);
                        }
                    }
                    
                }

            }
        }
    }
}