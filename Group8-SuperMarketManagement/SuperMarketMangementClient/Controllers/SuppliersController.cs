﻿using DataAccess.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SuperMarketMangementClient.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly HttpClient client = null;

        private readonly string JWTToken = "";
        private readonly string UserId = "";
        private readonly IServiceProvider _services;
        public SuppliersController(IServiceProvider services)
        {
            _services = services;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);
            ISession session = _services.GetRequiredService<IHttpContextAccessor>().HttpContext.Session;
            JWTToken = session.GetString("JWToken") ?? "";
            UserId = session.GetString("UserId") ?? "";
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {


            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyName,Address,Phone")] SupplierDTOCreate customer)
        {
            if (ModelState.IsValid)
            {

                HttpResponseMessage response = await client.PostAsJsonAsync("https://localhost:5000/api/Supplier", customer);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Create");
            }
            return View(customer);

        }
        public async Task<IActionResult> Edit(int id)
        {
            var sup = new SupplierDTORespone();
            HttpResponseMessage response = await client.GetAsync("https://localhost:5000/api/Supplier/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            sup = JsonSerializer.Deserialize<SupplierDTORespone>(strData, options);
            return View(sup);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("SupplierID,CompanyName,Address,Phone")] SupplierDTOPUT customer)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await client.PutAsJsonAsync("https://localhost:5000/api/Supplier/" + customer.SupplierID, customer);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else return NotFound();
            }
            return View(customer);

        }
        public async Task<IActionResult> Delete(int id)
        {
            var sup = new SupplierDTORespone();
            HttpResponseMessage response = await client.GetAsync("https://localhost:5000/api/Supplier/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            sup = JsonSerializer.Deserialize<SupplierDTORespone>(strData, options);
            return View(sup);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int SupplierID)
        {
            HttpResponseMessage response = await client.DeleteAsync("https://localhost:5000/api/Supplier/" + SupplierID);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Delete");
        }

    }
}
