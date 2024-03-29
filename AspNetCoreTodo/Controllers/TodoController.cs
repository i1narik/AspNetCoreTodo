﻿using AspNetCoreTodo.Models;
using AspNetCoreTodo.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AspNetCoreTodo.Controllers
{
    public class TodoController : Controller
    {
        private readonly ITodoItemService _todoItemService;

        public TodoController(ITodoItemService todoItemService)
        {
            _todoItemService = todoItemService;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _todoItemService.GetIncompleteItemsAsync();
            var model = new TodoViewModel
            {
                Items = items
            };
            return View(model);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(TodoItem todoItem)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            var succesfull = await _todoItemService.AddItemAsync(todoItem);
            if (!succesfull)
            {
                return BadRequest("Could not add item");
            }
            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkDone(Guid id)
        {
            if (id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }
            var succesfull = await _todoItemService.MarkDoneAsync(id);
            if (!succesfull)
            {
                return BadRequest("Could not mark item as done");
            }
            return RedirectToAction("Index");
        }

    }
}
