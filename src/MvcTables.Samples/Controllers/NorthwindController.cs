namespace MvcTables.Samples.Controllers
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web.Mvc;
    using App_Start;
    using Models;
    using NorthwindEntities;

    #endregion

    public class NorthwindController : Controller
    {
        private static readonly Uri NorthwindServiceUrl = new Uri(ConfigurationManager.AppSettings["NorthwindUrl"]);

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Categories()
        {
            return View();
        }

        public ActionResult EasyCategories()
        {
            return View();
        }

        public ActionResult Orders()
        {
            return View();
        }

        public ActionResult Invoices()
        {
            return View();
        }

        public ActionResult ParentChild()
        {
            return View();
        }

        public ActionResult KitchenSink()
        {
            return View();
        }

        public ActionResult Filters()
        {
            var entities = new NorthwindEntities(NorthwindServiceUrl);
            var customers =
                new Customer[] {null}.Union(
                                            entities.Orders.Expand(o => o.Customer)
                                                    .ToArray()
                                                    .Select(o => o.Customer)
                                                    .Distinct());
            return View(new FiltersModel
                            {
                                Customers = customers
                            });
        }

        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(IEnumerable<Employee> employees)
        {
            return PartialView("_Employees", employees);
        }

        public ActionResult ListCategoriesDefault(TableRequestModel request)
        {
            var entities = new NorthwindEntities(NorthwindServiceUrl);
            return TableResult.From(entities.Categories).Default(request);
        }

        public ActionResult ListEmployees(TableRequestModel request, bool readOnly = false)
        {
            var entities = new NorthwindEntities(NorthwindServiceUrl);
            return TableResult.From(entities.Employees).Build<EmployeeTable>(request);
        }

        public ActionResult ReadonlyEmployees(TableRequestModel request, IEnumerable<Employee> employees)
        {
            var result = TableResult.From(employees).Build<EmployeeTable>(request, employees.Count());
            return result;
        }

        public ActionResult ListInvoices(TableRequestModel request)
        {
            var entities = new NorthwindEntities(NorthwindServiceUrl);
            return TableResult.From(entities.Invoices).Build<InvoiceTable>(request);
        }

        public ActionResult ListCategories(TableRequestModel request)
        {
            var entities = new NorthwindEntities(NorthwindServiceUrl);
            return TableResult.From(entities.Categories).Build<CategoryTable>(request);
        }

        public ActionResult ListOrders(TableRequestModel request, FiltersModel model)
        {
            var entities = new NorthwindEntities(NorthwindServiceUrl);
            var orders = entities.Orders.Expand(o => o.Customer).Expand(o => o.Shipper).Expand(o => o.Order_Details);
            return TableResult.From(orders).Build<OrderTable>(request);
        }

        public ActionResult ListParentOrders(TableRequestModel request, string nameFilter)
        {
            var entities = new NorthwindEntities(NorthwindServiceUrl);
            IQueryable<Order> orders = entities.Orders.Expand(o => o.Customer).Expand(o => o.Shipper).Expand(o => o.Order_Details);
            if (!String.IsNullOrEmpty(nameFilter))
            {
                orders = orders.Where(o => o.Customer.ContactName.Contains(nameFilter));
            }
            return TableResult.From(orders).Build<ParentOrderTable>(request);
        }

        public ActionResult ListChildOrderDetails(TableRequestModel request, int? orderId)
        {
            var entities = new NorthwindEntities(NorthwindServiceUrl);
            IQueryable<Order_Detail> orderDetails = entities.Order_Details.Expand(o => o.Product);

            if (orderId == null)
            {
                orderDetails = orderDetails.Where(o => false);
            }
            else
            {
                orderDetails = orderDetails.Where(o => o.OrderID == orderId.Value);
            }

            return TableResult.From(orderDetails).Build<ChildOrderDetailTable>(request);
        }

        public ActionResult ListOrdersForFilters(TableRequestModel request, FiltersModel model)
        {
            var entities = new NorthwindEntities(NorthwindServiceUrl);
            IQueryable<Order> orders =
                entities.Orders.Expand(o => o.Customer).Expand(o => o.Shipper).Expand(o => o.Order_Details);
            if (!String.IsNullOrEmpty(model.SelectedCustomerId))
            {
                orders = orders.Where(o => o.CustomerID == model.SelectedCustomerId);
            }
            return TableResult.From(orders).Build<OrderFilterTable>(request);
        }
    }
}