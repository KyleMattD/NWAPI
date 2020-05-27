using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using NWAPI.Models;

namespace NWAPI.Controllers
{
    public class CustomerDemographicsController : ApiController
    {
        private NorthwindEntities db = new NorthwindEntities();

        // GET: api/CustomerDemographics
        public IQueryable<CustomerDemographic> GetCustomerDemographics()
        {
            return db.CustomerDemographics;
        }

        // GET: api/CustomerDemographics/5
        [ResponseType(typeof(CustomerDemographic))]
        public IHttpActionResult GetCustomerDemographic(string id)
        {
            CustomerDemographic customerDemographic = db.CustomerDemographics.Find(id);
            if (customerDemographic == null)
            {
                return NotFound();
            }

            return Ok(customerDemographic);
        }

        // PUT: api/CustomerDemographics/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCustomerDemographic(string id, CustomerDemographic customerDemographic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customerDemographic.CustomerTypeID)
            {
                return BadRequest();
            }

            db.Entry(customerDemographic).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerDemographicExists(id))
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

        // POST: api/CustomerDemographics
        [ResponseType(typeof(CustomerDemographic))]
        public IHttpActionResult PostCustomerDemographic(CustomerDemographic customerDemographic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CustomerDemographics.Add(customerDemographic);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CustomerDemographicExists(customerDemographic.CustomerTypeID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = customerDemographic.CustomerTypeID }, customerDemographic);
        }

        // DELETE: api/CustomerDemographics/5
        [ResponseType(typeof(CustomerDemographic))]
        public IHttpActionResult DeleteCustomerDemographic(string id)
        {
            CustomerDemographic customerDemographic = db.CustomerDemographics.Find(id);
            if (customerDemographic == null)
            {
                return NotFound();
            }

            db.CustomerDemographics.Remove(customerDemographic);
            db.SaveChanges();

            return Ok(customerDemographic);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerDemographicExists(string id)
        {
            return db.CustomerDemographics.Count(e => e.CustomerTypeID == id) > 0;
        }
    }
}