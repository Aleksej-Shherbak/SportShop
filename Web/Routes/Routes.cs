using Microsoft.AspNetCore.Builder;

namespace Web.Routes
{
    public static class Routes
    {
        public static void UserMvcWithRouting(this IApplicationBuilder app)
        {
            app.UseMvc(routes =>
            {
                // /
                routes.MapRoute("home", "", new
                {
                    controller = "Products",
                    action = "List",
                    category = "", page = 1
                });

                // Page2
                routes.MapRoute(null,
                    "Page{page}",
                    new
                    {
                        controller = "Products",
                        action = "List",
                        category = ""
                    },
                    new {page = @"\d+"}
                );

                // Category
                routes.MapRoute(null,
                    "{category}",
                    new
                    {
                        controller = "Products",
                        action = "List",
                        page = 1
                    });

                // Category/Page2
                routes.MapRoute(null,
                    "{category}/Page{page}",
                    new
                    {
                        controller = "Products",
                        action = "List",
                    },
                    new
                    {
                        page = @"\d+"
                    });

                routes.MapRoute(null, "{controller}/{action}");
            });
        }
    }
}