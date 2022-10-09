using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Routes
{
    public class AppRoute
    {
        public static void Run(WebApplication app)
        {
            TodoRoute.Run(app);
        }
    }

    internal class TodoRoute
    {
        public static void Run(WebApplication app)
        {
            app.MapGet("/todos", async (AppContextDb context) =>
            {
                return await context.Todos.ToListAsync();
            })
            .WithName("TodoList")
            .WithTags("Todo");

            app.MapGet("/todos/{id}", async (AppContextDb c, string id) =>
            {
                if (id == null) return Results.BadRequest("Parâmetro ID não encontrado");

                var r = Guid.TryParse(id, out Guid NewGuid);

                if (!r) return Results.BadRequest("Id não é válido");

                var result = await c.Todos.FindAsync(NewGuid);

                return result
                    is Todo todo
                    ? Results.Ok(todo)
                    : Results.NotFound("Todo not found");
            })
            .Produces<Todo>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("FindTodo")
            .WithTags("Todo");

            app.MapPost("/todos/create", async (AppContextDb context, CreateTodoRequest t) =>
            {
                if (t.Title == null || t.Value == null) return Results.NotFound("Params not found");

                var newTodo = new Todo { Id = Guid.NewGuid(), Title = t.Title, Value = t.Value };
                context.Todos.Add(newTodo);
                var result = await context.SaveChangesAsync();
                return result > 0
                    ? Results.Created("Nova tarefa criada com sucesso", newTodo)
                    : Results.BadRequest();
            })
            .Produces<Todo>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("CreateTodo")
            .WithTags("Todo");

            app.MapPut("/todos/{id}", async (AppContextDb c, string id, UpdateTodoRequest t) =>
            {
                if (id == null) return Results.BadRequest("Parâmetro ID não encontrado");

                var r = Guid.TryParse(id, out Guid NewGuid);

                if (!r) return Results.BadRequest("Id não é válido");

                if (t.Title == null || t.Value == null) return Results.NotFound("Params not found");

                var todoExist = await c.Todos.SingleOrDefaultAsync(t => t.Id == NewGuid);

                if (todoExist != null)
                {
                    todoExist.Title = t.Title;
                    todoExist.Value = t.Value;
                    c.Todos.Update(todoExist);
                    var result = await c.SaveChangesAsync();
                    return result > 0
                        ? Results.NoContent()
                        : Results.BadRequest("Houve um problema ao salvar o registro");

                }

                return Results.NotFound("Tarefa não encontrada");

            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("UpdateTodo")
            .WithTags("Todo");

            app.MapDelete("todos/{id}", async (AppContextDb c, string id) =>
            {
                if (id == null) return Results.BadRequest("Parâmetro ID não encontrado");

                var r = Guid.TryParse(id, out Guid NewGuid);

                if (!r) return Results.BadRequest("ID não é válido");

                var todoExist = await c.Todos.SingleOrDefaultAsync(t => t.Id == NewGuid);

                if (todoExist == null) return Results.NotFound("Tarefa não encontrada");

                c.Todos.Remove(todoExist);

                var result = await c.SaveChangesAsync();

                return result > 0
                    ? Results.NoContent()
                    : Results.BadRequest("Houve um problema ao salvar o registro ");

            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("DeleteTodo")
            .WithTags("Todo");
        }
    }

    internal record CreateTodoRequest(string? Title, string? Value);
    internal record UpdateTodoRequest(string? Title, string? Value);
}



