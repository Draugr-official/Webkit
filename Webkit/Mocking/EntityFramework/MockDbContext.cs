using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webkit.Mocking.EntityFramework
{
    public class MockDbContext : IDisposable
    {
        public void SaveChanges()
        {

        }

        public void Dispose()
        {
            // ...
        }
    }
}
