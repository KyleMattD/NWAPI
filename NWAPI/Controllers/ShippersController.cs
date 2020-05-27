using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using NWAPI.Models;

namespace NWAPI.Controllers
{
    public class ShippersController : ApiController
    {
        private NorthwindEntities db = new NorthwindEntities();

        // GET: api/Shippers
        public IQueryable<Shipper> GetShippers()
        {
            return db.Shippers;
        }

        // GET: api/Shippers/5
        [ResponseType(typeof(Shipper))]
        public IHttpActionResult GetShipper(int id)
        {
            Shipper shipper = db.Shippers.Find(id);
            if (shipper == null)
            {
                return NotFound();
            }

            return Ok(shipper);
        }

        // PUT: api/Shippers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutShipper(int id, Shipper shipper)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != shipper.ShipperID)
            {
                return BadRequest();
            }

            db.Entry(shipper).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShipperExists(id))
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

        // POST: api/Shippers
        [ResponseType(typeof(Shipper))]
        public IHttpActionResult PostShipper(Shipper shipper)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Shippers.Add(shipper);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = shipper.ShipperID }, shipper);
        }

        // DELETE: api/Shippers/5
        [ResponseType(typeof(Shipper))]
        public IHttpActionResult DeleteShipper(int id)
        {
            Shipper shipper = db.Shippers.Find(id);
            if (shipper == null)
            {
                return NotFound();
            }

            db.Shippers.Remove(shipper);
            db.SaveChanges();

            return Ok(shipper);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ShipperExists(int id)
        {
            return db.Shippers.Count(e => e.ShipperID == id) > 0;
        }
    }
}