using Microsoft.AspNetCore.Mvc;
using TaskApi.Models;

namespace TaskApi.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private static readonly List<TaskItem> tasks = new()
        {
            new TaskItem
            {
                Id = 1,
                ClientName = "ACME SRL",
                ProjectName = "Sito v2",
                Title = "Setup progetto",
                Description = "Creare repository, pipeline base e struttura iniziale",
                Status = "in_progress",
                Priority = "high",
                DueDate = DateTime.UtcNow.AddDays(3),
                EstimatedHours = 4
            },
            new TaskItem
            {
                Id = 2,
                ClientName = "Studio Rossi",
                ProjectName = "Landing Funnel",
                Title = "Landing page iniziale",
                Description = "Landing responsive con form contatti",
                Status = "todo",
                Priority = "medium",
                DueDate = DateTime.UtcNow.AddDays(7),
                EstimatedHours = 6
            }
        };

        // GET: api/tasks
        [HttpGet]
        public ActionResult<IEnumerable<TaskItem>> GetTasks()
        {
            return Ok(tasks);
        }

        // GET: api/tasks/{id}
        [HttpGet("{id:int}")]
        public ActionResult<TaskItem> GetTaskById(int id)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        // POST: api/tasks
        [HttpPost]
        public ActionResult<TaskItem> CreateTask([FromBody] TaskItem newTask)
        {
            int nextId = tasks.Count == 0 ? 1 : tasks.Max(t => t.Id) + 1;
            newTask.Id = nextId;

            if (string.IsNullOrWhiteSpace(newTask.Status))
                newTask.Status = "todo";

            tasks.Add(newTask);

            return CreatedAtAction(nameof(GetTaskById), new { id = newTask.Id }, newTask);
        }

        // PUT: api/tasks/{id}
        [HttpPut("{id:int}")]
        public IActionResult UpdateTask(int id, [FromBody] TaskItem updatedTask)
        {
            var existing = tasks.FirstOrDefault(t => t.Id == id);
            if (existing == null) return NotFound();

            existing.ClientName = updatedTask.ClientName;
            existing.ProjectName = updatedTask.ProjectName;
            existing.Title = updatedTask.Title;
            existing.Description = updatedTask.Description;
            existing.Status = updatedTask.Status;
            existing.Priority = updatedTask.Priority;
            existing.DueDate = updatedTask.DueDate;
            existing.EstimatedHours = updatedTask.EstimatedHours;

            return NoContent();
        }

        // DELETE: api/tasks/{id}
        [HttpDelete("{id:int}")]
        public IActionResult DeleteTask(int id)
        {
            var existing = tasks.FirstOrDefault(t => t.Id == id);
            if (existing == null) return NotFound();

            tasks.Remove(existing);
            return NoContent();
        }
    }
}
