MvcTables
========

An AJAX/HTML Table framework for ASP.Net MVC

Supports:
* paging, 
* sorting,
* filtering,
* Edit/Display templates as cell markup,
* Partial Views as cell markup,
* HtmlHelper style column configuration

Configuration
-------------
To save loads of messy boiler plate code clogging up Controller actions, the tables can be configured at startup.
Any class that inherits ``` TableConfigurator<TType> ``` can be registered int the *global.asax* thusly:
<dl>
    <dt>Global.asax</dt>
    <dd></dd>
</dl>
```C#
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //Regular MVC Setup hoo haa
            FluentlyConfigure.FromAssembly().SameAsType<MvcApplication>();
        }
    }
```
<dl>
  <dt>Auto Configuration</dt>
  <dd></dd>
</dl>
```C#
public class CategoryTable : MvcTable<Category>
{
    public override void Configure(ITableConfiguration<Category> config)
    {
        config
            .SetCssClass("table table-striped")
            .ScaffoldAllColumns();
    }
}
```
<dl>
  <dt>Basic Configuration</dt>
  <dd></dd>
</dl>
```C#
    public class InvoiceTable : MvcTable<Invoice>
    {
        public override void Configure(ITableConfiguration<Invoice> config)
        {
            config
                .SetCssClass("table table-striped")
                .DisplayForColumn(m => m.ProductName)
                .DisplayForColumn(m => m.Quantity)
                .DisplayForColumn(m => m.Region)
                .DisplayForColumn(m => m.Salesperson)
                .EditorForColumn(m => m.Discount);
        }
    }
```
<dl>
  <dt>More Complex Configuration</dt>
  <dd></dd>
</dl>
```C#
    public class OrderDetailTable : MvcTable<Order_Detail>
    {
        public override void Configure(ITableConfiguration<Order_Detail> config)
        {
            config
                .SetCssClass("table table-striped")
                .ConfigurePagingControl(p => p.ShowPagingControl(true))
                .DisplayForColumn(c => c.Product.ProductName, 
                    cfg => cfg.DefineFooter((h, rows) => rows.Any() ? "Order ID: " + rows.First().OrderID : ""))
                .DisplayForColumn(c => c.Quantity)
                .DisplayForColumn(c => c.UnitPrice)
                .DisplayForColumn(c => c.LineTotal, 
                    cfg => cfg.DefineFooter((h,rows) => rows.Any() ? rows.Sum(od => od.LineTotal).ToString("c") : ""))
                .ConfigurePagingControl(p => p.ShowPagingControl(false))
                .SetFilterSelector("childFilter");
                    
            SetName("ChildOrderDetails");
        }
    }
```
<dl>
  <dt>Runtime Configuration Overrides</dt>
  <dd></dd>
</dl>
```C#
    public ActionResult ListEmployees(TableRequestModel request)
    {
        var entities = new NorthwindEntities.NorthwindEntities(NorthwindServiceUrl);
        var invoices = entities.Employees;
        
        var result = new TableResult<Employee>(invoices, request);
        
        result.Overrides.EditorForColumn(d => d.HireDate, cfg => cfg.SetIndex(1));
        return result;
    }
```

