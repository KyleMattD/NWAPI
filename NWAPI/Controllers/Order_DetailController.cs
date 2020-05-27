using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using NWAPI.Models;

namespace NWAPI.Controllers
{
    public class Order_DetailController : ApiController
    {
        private NorthwindEntities db = new NorthwindEntities();

        // GET: api/Order_Detail
        public IQueryable<Order_Detail> GetOrder_Details()
        {
            return db.Order_Details;
        }

        // GET: api/Order_Detail/5
        [ResponseType(typeof(Order_Detail))]
        public IHttpActionResult GetOrder_Detail(int id)
        {
            Order_Detail order_Detail = db.Order_Details.Find(id);
            if (order_Detail == null)
            {
                return NotFound();
            }

            return Ok(order_Detail);
        }

        // PUT: api/Order_Detail/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrder_Detail(int id, Order_Detail order_Detail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order_Detail.OrderID)
            {
                return BadRequest();
            }

            db.Entry(order_Detail).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Order_DetailExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Order_Detail
        [ResponseType(typeof(Order_Detail))]
        public IHttpActionResult PostOrder_Detail(Order_Detail order_Detail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Order_Details.Add(order_Detail);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (Order_DetailExists(order_Detail.OrderID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = order_Detail.OrderID }, order_Detail);
        }

        // DELETE: api/Order_Detail/5
        [ResponseType(typeof(Order_Detail))]
        public IHttpActionResult DeleteOrder_Detail(int id)
        {
            Order_Detail order_Detail = db.Order_Details.Find(id);
            if (order_Detail == null)
            {
                return NotFound();
            }

            db.Order_Details.Remove(order_Detail);
            db.SaveChanges();

            return Ok(order_Detail);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Order_DetailExists(int id)
        {
            return db.Order_Details.Count(e => e.OrderID == id) > 0;
        }
    }
}