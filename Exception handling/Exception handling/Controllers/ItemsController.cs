using Exception_handling.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Exception_handling.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : Controller
    {
        private readonly List<Item> _items;
        private int _nextId = 1;
        public ItemsController()
        {
            _items = new List<Item>();
        }
        // GET api/items
        [HttpGet]
        public IActionResult GetItems()
        {
            try
            {
                return Ok(_items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error:{ex.Message}");
            }
        }
        // GET: api/items/{id}
        [HttpGet("{id}", Name = "GetItem")]
        public IActionResult GetItem(int id)
        {
            try
            {
                var item = _items.FirstOrDefault(i => i.Id == id);
                if (item == null)
                    return NotFound($"Item with id {id} not found");

                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/items
        [HttpPost]
        public IActionResult AddItem([FromBody] Item item)
        {
            try
            {
                if (item == null)
                    return BadRequest("Item object is null");

                if (string.IsNullOrWhiteSpace(item.Title))
                    return BadRequest("Item title cannot be empty");

                item.Id = _nextId++;
                _items.Add(item);

                return CreatedAtRoute("GetItem", new { id = item.Id }, item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/items/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateItem(int id, [FromBody] Item item)
        {
            try
            {
                if (item == null)
                    return BadRequest("Item object is null");

                var existingItem = _items.FirstOrDefault(i => i.Id == id);
                if (existingItem == null)
                    return NotFound($"Item with id {id} not found");

                if (string.IsNullOrWhiteSpace(item.Title))
                    return BadRequest("Item title cannot be empty");

                existingItem.Title = item.Title;
                existingItem.Description = item.Description;

                return NoContent();
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
