using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GuessTheAnimal.Models;

namespace GuessTheAnimal.Controllers
{
    public class AnimalDetailsController : Controller
    {
        private AnimalEntities db = new AnimalEntities();

        // GET: AnimalDetails
        public ActionResult Index()
        {
            var animalDetails = db.AnimalDetails.Include(a => a.Animal);
            return View(animalDetails.ToList());
        }

        // GET: AnimalDetails
        public ActionResult Question()
        {
            var animalDetails = from element in db.AnimalDetails.Include(a => a.Animal)
                                group element by element.AnimalId
                                into groups
                                select groups.OrderBy(p => p.FactId).FirstOrDefault();
            var temp = animalDetails.ToList();
            @ViewBag.animalDetails = temp;

            return View(animalDetails.ToList().FirstOrDefault());
        }

        [HttpPost]
        public ActionResult Question(AnimalDetail animalDetail)
        {
            AnimalDetail temp = null;


            if (!string.IsNullOrEmpty(animalDetail.SelectedAnswer) && animalDetail.SelectedAnswer.Equals("Yes"))
            {
                temp = new BusinessLayer.BLAnimalDetail().GetNextFactForAnimal(animalDetail);
                ViewData["AnimalFound"] = true;
            }
            else if (!string.IsNullOrEmpty(animalDetail.SelectedAnswer) && animalDetail.SelectedAnswer.Equals("No"))
            {
                temp = new BusinessLayer.BLAnimalDetail().GetFactForNextAnimal((List<AnimalDetail>)@ViewBag.animalDetails, animalDetail);
                ViewData["AnimalFound"] = false;
            }
            ModelState.Clear();

            if (temp == null)
            {
                ViewData["EndGuess"] = true;

                var animal = db.Animals.Find(animalDetail.AnimalId);
                if (animal != null)
                {
                    ViewData["AnimalName"] = animal.Name;
                }
            }

            return View(temp);
        }

        // GET: AnimalDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnimalDetail animalDetail = db.AnimalDetails.Find(id);
            if (animalDetail == null)
            {
                return HttpNotFound();
            }
            return View(animalDetail);
        }

        // GET: AnimalDetails/Create
        public ActionResult Create()
        {
            ViewBag.AnimalId = new SelectList(db.Animals, "Id", "Name");
            return View();
        }

        // POST: AnimalDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FactId,Facts,AnimalId")] AnimalDetail animalDetail)
        {
            if (ModelState.IsValid)
            {
                db.AnimalDetails.Add(animalDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AnimalId = new SelectList(db.Animals, "Id", "Name", animalDetail.AnimalId);
            return View(animalDetail);
        }

        // GET: AnimalDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnimalDetail animalDetail =  db.AnimalDetails.Find(id);
            if (animalDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.AnimalId = new SelectList(db.Animals, "Id", "Name", animalDetail.AnimalId);
            return View(animalDetail);
        }

        // POST: AnimalDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "FactId,Facts,AnimalId")] AnimalDetail animalDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(animalDetail).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.AnimalId = new SelectList(db.Animals, "Id", "Name", animalDetail.AnimalId);
            return View(animalDetail);
        }

        // GET: AnimalDetails/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnimalDetail animalDetail = await db.AnimalDetails.FindAsync(id);
            if (animalDetail == null)
            {
                return HttpNotFound();
            }
            return View(animalDetail);
        }

        // POST: AnimalDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AnimalDetail animalDetail = await db.AnimalDetails.FindAsync(id);
            db.AnimalDetails.Remove(animalDetail);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
