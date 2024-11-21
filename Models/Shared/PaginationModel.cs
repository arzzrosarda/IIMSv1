
namespace IIMSv1.Models.Shared
{
    public class PaginationModel
    {

        public string ActionName { get; }
        public string ControllerName { get; }

        public string SearchText { get; }

        public int RecordsPerPage { get; }
        public int PageNum { get; }
        
        public int RecordTotal { get; }

        public int RecordStart 
        { 
            get
            {
                return ((PageNum - 1) * RecordsPerPage) + 1;
            }
        }

        public int RecordEnd
        {
            get
            {
                int recordend = (PageNum * RecordsPerPage);
                if (recordend > RecordTotal)
                {
                    return RecordTotal;
                }
                else
                {
                    return recordend;
                }
            }
        }

        public int PageTotal {
            get
            {
                int pagetotal = Global.GetPaginationTotalPages(RecordTotal, RecordsPerPage);
                return pagetotal;
            }
        }

        public PaginationModel(string ActionName, string ControllerName, string SearchText, int RecordTotal, int RecordsPerPage, int PageNum)
        {
            this.ActionName = ActionName;
            this.ControllerName = ControllerName;
            this.SearchText = SearchText.Trim();
            this.RecordTotal = RecordTotal;
            this.RecordsPerPage = RecordsPerPage;
            this.PageNum = PageNum;
        }


    }
}
