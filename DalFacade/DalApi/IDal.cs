using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    public interface IDal
    {
        IDependency Dependency { get; }
        IEngineer Engineer { get; }
        ITask Task { get; }

        DateTime? startProject { get; set; }
        DateTime? deadlineProject { get; set; }

        public void Reset();
    }
}
