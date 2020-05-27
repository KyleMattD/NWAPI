using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using NWAPI.Models;

namespace NWAPI.Controllers
{
    public class TerritoriesController : ApiController
    {
        private NorthwindEntities db = new NorthwindEntities();

        // GET: api/Territories
        public IQueryable<Territory> GetTerritories()
        {
            return db.Territories;
        }

        // GET: api/Territories/5
        [ResponseType(typeof(Territory))]
        public IHttpActionResult GetTerritory(string id)
        {
            Territory territory = db.Territories.Find(id);
            if (territory == null)
            {
                return NotFound();
            }

            return Ok(territory);
        }

        // PUT: api/Territories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTerritory(string id, Territory territory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != territory.TerritoryID)
            {
                return BadRequest();
            }

            db.Entry(territory).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TerritoryExists(id))
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

        // POST: api/Territories
        [ResponseType(typeof(Territory))]
        public IHttpActionResult PostTerritory(Territory territory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Territories.Add(territory);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (TerritoryExists(territory.TerritoryID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = territory.TerritoryID }, territory);
        }

        // DELETE: api/Territories/5
        [ResponseType(typeof(Territory))]
        public IHttpActionResult DeleteTerritory(string id)
        {
            Territory territory = db.Territories.Find(id);
            if (territory == null)
            {
                return NotFound();
            }

            db.Territories.Remove(territory);
            db.SaveChanges();

            return Ok(territory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TerritoryExists(string id)
        {
            return db.Territories.Count(e => e.TerritoryID == id) > 0;
        }
    }
}