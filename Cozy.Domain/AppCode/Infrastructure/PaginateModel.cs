using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cozy.Domain.AppCode.Infrastructure
{
    public abstract class PaginateModel
    {
        int pageIndex;
        int pageSize;

        public int PageIndex
        {
            get
            {
                return this.pageIndex < 1 ? 1 : this.pageIndex;
            }
            set
            {
                if (value >= 1)
                {
                    pageIndex = value;
                }
            }
        }
        public virtual int PageSize
        {
            get
            {
                return this.pageSize < 12 ? 12 : this.pageSize;

            }
            set
            {
                if (value >= 5)
                {
                    pageSize = value;
                }
            }
        }
    }
}