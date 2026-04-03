using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess
{
    public static class DbInitializer
    {
        public static void Initialize(SparqDbContext context, string imagePath)
        {
            if (!Path.Exists(imagePath))
            {
                throw new IOException("Image path does not exists");
            }

            context.Database.Migrate();

        }
    }
}
