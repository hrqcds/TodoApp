namespace TodoApp.Config
{

    public class ConfigApp
    {
        public static void Run(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
        }
    }


}