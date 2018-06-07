using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinancialPortal.Helpers;
using FinancialPortal.Models;
using HouseHoldFinance.Helpers;
using HouseHoldFinance.Models;

namespace HouseHoldFinance.Controllers
{
    public class PersonalAccountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PersonalAccounts
        [AuthorizeHouseholdRequired]
        public ActionResult Index()
        {
            int hId = User.Identity.GetHouseholdId().Value;
            var personalAccounts = db.PersonalAccounts.Where(p => p.HouseholdId == hId)
                .Include(p => p.Household);
            return View(personalAccounts.ToList());
        }

        // GET: PersonalAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonalAccount personalAccount = db.PersonalAccounts.Find(id);
            if (personalAccount == null)
            {
                return HttpNotFound();
            }

            var debits = personalAccount.Transactions.Where(t => t.Type == false).Sum(t => t.Amount);
            ViewBag.Balance = (personalAccount.Balance - debits);

            var credits = personalAccount.Transactions.Where(t => t.Type == true).Sum(t => t.Amount);
            ViewBag.Balance += credits;

            var dtrx = personalAccount.Transactions.Where(t => t.Type = false && t.Reconciled == true).Sum(t => t.Amount);
            ViewBag.RecBalance = (personalAccount.Balance - dtrx);

            var ctrx = personalAccount.Transactions.Where(t => t.Type == true && t.Reconciled == true).Sum(t => t.Amount);
            ViewBag.RecBalance += ctrx;
            return View(personalAccount);
        }

        // GET: PersonalAccounts/Create
        public ActionResult Create()
        {
            ViewBag.CreatedById = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name");
            return View();
        }

        // POST: PersonalAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,HouseholdId,Name,Balance,ReconciledBalance,CreatedById,IsDeleted")] PersonalAccount personalAccount)
        {
            if (ModelState.IsValid)
            {
                db.PersonalAccounts.Add(personalAccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CreatedById = new SelectList(db.Users, "Id", "FirstName", personalAccount.CreatedById);
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", personalAccount.HouseholdId);
            return View(personalAccount);
        }

        // GET: PersonalAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonalAccount personalAccount = db.PersonalAccounts.Find(id);
            if (personalAccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreatedById = new SelectList(db.Users, "Id", "FirstName", personalAccount.CreatedById);
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", personalAccount.HouseholdId);
            return View(personalAccount);
        }

        // POST: PersonalAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HouseholdId,Name,Balance,ReconciledBalance,CreatedById,IsDeleted")] PersonalAccount personalAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(personalAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreatedById = new SelectList(db.Users, "Id", "FirstName", personalAccount.CreatedById);
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", personalAccount.HouseholdId);
            return View(personalAccount);
        }

        // GET: PersonalAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonalAccount personalAccount = db.PersonalAccounts.Find(id);
            if (personalAccount == null)
            {
                return HttpNotFound();
            }
            return View(personalAccount);
        }

        // POST: PersonalAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PersonalAccount personalAccount = db.PersonalAccounts.Find(id);
            db.PersonalAccounts.Remove(personalAccount);
            db.SaveChanges();
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
